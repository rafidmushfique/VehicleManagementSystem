using LILI_VMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LILI_VMS.Controllers
{
    [Authorize]
    public class BidApprovalController : BaseController
    {
        private readonly dbVehicleManagementContext _context;

        public BidApprovalController(dbVehicleManagementContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            var UnitCode = HttpContext.Session.GetString("UnitCode");
            var PlantId = long.Parse(HttpContext.Session.GetString("PlantId"));
            ViewData["CurrentFilter"] = searchString;
            var req = from c in _context.TblBidSchedules
                      where c.UnitCode == UnitCode && c.PlantId == PlantId
                      select new TblBidSchedule
                      {
                          Id = c.Id,
                          BidScheduleNo = c.BidScheduleNo,
                          BidScheduleDate = c.BidScheduleDate,
                          LoadPoint = c.LoadPoint,
                          CapacityMt = c.CapacityMt,
                          Status = c.Status,
                          CustomerNames = _context.TblBidScheduleCustomers.Where(x => x.BidScheduleNo == c.BidScheduleNo).Select(s => s.CustomerName).Distinct().ToList(),
                          BidCount = _context.TblBidProposals.Count(x => x.BidScheduleNo == c.BidScheduleNo).ToString()
                      };



            if (!String.IsNullOrEmpty(searchString))
            {
                req = req.Where(s => s.BidScheduleNo.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    req = req.OrderByDescending(s => s.BidScheduleNo);
                    break;

                default:
                    req = req.OrderBy(s =>
                                      s.Status == "New" ? 1 :
                                      s.Status == "Bidded" ? 2 :
                                      s.Status == "Approved" ? 3 :
                                      s.Status == "Completed" ? 4 : 5).ThenByDescending(x=>x.Id);
                    break;
            }
             int pageSize =  20;
            return View(await PaginatedList<TblBidSchedule>.CreateAsync(req.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
        public IActionResult BidDecision(int id) 
        {
            if (!_context.TblBidSchedules.Any(x => x.Id == id))
            { 
                NotFound();
            }


            var model = _context.TblBidSchedules.Where(x => x.Id == id).FirstOrDefault();
            var currentDateTime = DateTime.Now;
            var inTime = false;
            if (currentDateTime > model.BidScheduleEndDate)
            {
                inTime = true;
            }
            ViewBag.intime = inTime;
            var customerList = _context.TblBidScheduleCustomers.Where(x=>x.BidScheduleNo== model.BidScheduleNo).ToList();
            var vendorList = (from c in _context.TblBidScheduleVendors
                             from p in _context.TblBidProposals
                             where c.BidScheduleNo == p.BidScheduleNo && c.VendorCode==p.VendorCode && c.BidScheduleNo == model.BidScheduleNo
                             select new TblBidScheduleVendor { 
                              VendorCode = c.VendorCode,
                              VendorName = c.VendorName,
                              Add1 = c.Add1,
                              Add2 = c.Add2,
                              Mobile = c.Mobile,
                              BidAmount = p.BidAmount,
                              Comments = p.Comments
                             }).ToList();
                         
            model.TblBidScheduleVendors = vendorList;
            model.TblBidScheduleCustomers = customerList;
            var check = _context.TblBidSubmissionApprovals.Any(x => x.BidScheduleNo == model.BidScheduleNo);
            if (check)
            {
                var bidApproval = _context.TblBidSubmissionApprovals.FirstOrDefault(x => x.BidScheduleNo == model.BidScheduleNo);
                model.VendorCode = bidApproval.VendorCode;
                model.BidApproverComment = bidApproval.Comments;
            }
            else {
                model.VendorCode = "";
            }
            return View(model); 
        }

        public bool Finalize(TblBidSubmissionApproval model)
        {
            try
            {
                var check = _context.TblBidSubmissionApprovals.Any(x => x.BidScheduleNo == model.BidScheduleNo);
                if (check)
                {
                    var bidApprovalToUpdate = _context.TblBidSubmissionApprovals.FirstOrDefault(x => x.BidScheduleNo == model.BidScheduleNo);
                    bidApprovalToUpdate.VendorCode = model.VendorCode;
                    //bidApprovalToUpdate.ApprovalDate = DateTime.Now;
                    bidApprovalToUpdate.BidAmount = model.BidAmount;
                    bidApprovalToUpdate.Comments = model.Comments;
                }
                else 
                { 
                        model.ApprovalNo = GenerateApprovalNo();
                        model.ApprovalDate = DateTime.Now;
                        var res = UpdateBidStatus(model.BidScheduleNo);
                        _context.Add(model);
                }
           
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
            
        }
        public  ActionResult UpdateBidStatus(string BidScheduleNo)
        {
            var BidToUpdate = _context.TblBidSchedules.FirstOrDefault(x => x.BidScheduleNo == BidScheduleNo);
            BidToUpdate.Status = "Approved";

            return Ok();
        }
        public string GenerateApprovalNo()
        {
            //Generate Requisition No.---------Start
            String sDate = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));
            //String dy = datevalue.Day.ToString("00");
            String mn = datevalue.Month.ToString("00");
            String yy = datevalue.Year.ToString().Substring(2, 2);
            var CodeNo = "BA-" + yy + mn;
            String maxId = "";
            String maxNo = (from c in _context.TblBidSubmissionApprovals.Where(c => c.ApprovalNo.Substring(0, 7) == CodeNo).OrderByDescending(t => t.Id) select c.ApprovalNo.Substring(7, 3)).FirstOrDefault();
            if (maxNo == null)
            {
                maxId = "001";
            }
            else
            {
                maxId = (Convert.ToInt16(maxNo) + 1).ToString("000");
            }
            CodeNo = "BA-" + yy + mn + maxId;

            return CodeNo;
        }
    }
}
