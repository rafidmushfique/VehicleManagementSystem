using LILI_VMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LILI_VMS.Controllers
{
    [Authorize]
    public class BidProposalController : Controller
    {
        private readonly dbVehicleManagementContext _context;
        public BidProposalController(dbVehicleManagementContext context)
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

            ViewData["CurrentFilter"] = searchString;
            //IQueryable<LILI_VMS.Models.TblMaintenanceBill> model = _context.TblMaintenanceBills;
            var VendorCodeTemp = _context.TblUserWiseVendorCodes.Where(x => x.UserId == User.Identity.Name).Select(i=>i.VendorCode).ToList();
            var currentDate = DateTime.Now;
            var req = from c in _context.TblBidSchedules
                      from v in _context.TblBidScheduleVendors
                      from b in _context.TblBusinessUnitSetupInfos
                      where c.BidScheduleNo == v.BidScheduleNo && VendorCodeTemp.Contains(v.VendorCode) && b.BusinessCode == c.UnitCode
                      select new TblBidSchedule { 
                       Id = c.Id,
                       BidScheduleNo = c.BidScheduleNo,
                       BidScheduleDate = c.BidScheduleDate,
                       BidScheduleStartDate = c.BidScheduleStartDate,
                       CustomerNames = _context.TblBidScheduleCustomers.Where(x => x.BidScheduleNo == c.BidScheduleNo).Select(s => s.CustomerName).Distinct().ToList(),
                       CapacityMt = c.CapacityMt,
                       BidScheduleEndDate = c.BidScheduleEndDate,
                       Status = _context.TblBidProposals.Any(bp => bp.BidScheduleNo == c.BidScheduleNo && bp.VendorCode == v.VendorCode && c.Status =="New") ? "Bidded" : (currentDate >= c.BidScheduleStartDate && currentDate <= c.BidScheduleEndDate ? c.Status : "Closed"),
                          UnitName = b.BusinessName
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
                                       s.Status == "Approved" ? 2 :
                                       s.Status == "Bidded" ? 3 :
                                       s.Status == "Completed" ? 4 : 5).ThenByDescending(x => x.Id);
                    break;
            }
             int pageSize =  20;
            return View(await PaginatedList<TblBidSchedule>.CreateAsync(req.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public IActionResult ProposeBid(int Id) {
            //if (!_context.TblBidSchedules.Any(x => x.Id == Id))
            //{ 
            // NotFound();
            //}


            TblBidProposal model = new TblBidProposal();
            //var VendorCodeTemp = _context.TblUserWiseVendorCodes.FirstOrDefault(x => x.UserId == User.Identity.Name).VendorCode;
            var VendorCodeTemp = _context.TblUserWiseVendorCodes.Where(x => x.UserId == User.Identity.Name).Select(i => i.VendorCode).ToList();
            var result = _context.TblBidSchedules.FirstOrDefault(x=>x.Id == Id);
            var currentDateTime = DateTime.Now;
            var inTime = false;
            if (currentDateTime > result.BidScheduleStartDate && currentDateTime < result.BidScheduleEndDate)
            {
                inTime = true;
            }
            ViewBag.Satus = result.Status;
            ViewBag.InTime = inTime;

            if (result != null)
                {
                    model.BidScheduleNo = result.BidScheduleNo;
                    model.BidScheduleStartDate = result.BidScheduleStartDate;
                    model.BidScheduleDate = result.BidScheduleDate;
                    model.BidScheduleEndDate = result.BidScheduleEndDate;
                    model.LoadPoint = result.LoadPoint;
                    model.CapacityMt = result.CapacityMt;
                    model.BidCreatorComment = result.Comments;
                    ViewBag.CustomerList = _context.TblBidScheduleCustomers.Where(x => x.BidScheduleNo == result.BidScheduleNo).ToList();
                }
                var check = _context.TblBidProposals.Any(x => x.BidScheduleNo == result.BidScheduleNo && VendorCodeTemp.Contains(x.VendorCode));
                if (check)
                {
                    var data = _context.TblBidProposals.FirstOrDefault(x => x.BidScheduleNo == result.BidScheduleNo && VendorCodeTemp.Contains(x.VendorCode));
                    model.BidAmount = data.BidAmount;
                    model.Comments = data.Comments;
                    model.VendorCode = data.VendorCode;
                }

                model.BidProposalNo = GenerateProposeCode();
                model.BidProposalDate = DateTime.Now;

                return View(model);
            
         
        }

     
        public ActionResult CreateOrEditProposal(TblBidProposal model) 
        { 
         
          
            try
            {
                var check = _context.TblBidProposals.Any(x => x.BidScheduleNo == model.BidScheduleNo && x.VendorCode == model.VendorCode);
                var currentDate = DateTime.Now;
                var BidScheduleNo = model.BidScheduleNo;


                if (!check)
                {
                    var VendorCodeTemp = _context.TblUserWiseVendorCodes.Where(x => x.UserId == User.Identity.Name).Select(i => i.VendorCode).ToList();

                    model.VendorCode = _context.TblBidScheduleVendors.Where(x => x.BidScheduleNo == model.BidScheduleNo && VendorCodeTemp.Contains(x.VendorCode)).FirstOrDefault().VendorCode;
                    model.UnitCode = _context.TblUserWiseVendorCodes.FirstOrDefault(x => x.UserId == User.Identity.Name).VendorCode;
                    model.Iuser = User.Identity.Name;
                    model.Idate = DateTime.Now;
                    _context.Add(model);

                }
                else
                {
                    TblBidProposal bidProposal = new TblBidProposal();
                    bidProposal = _context.TblBidProposals.FirstOrDefault(x => x.BidScheduleNo == model.BidScheduleNo && x.VendorCode == model.VendorCode);

                    bidProposal.BidAmount = model.BidAmount;
                    bidProposal.Comments = model.Comments;

                }
              
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception exs)
            {

                throw;
            }
        }
        public string GenerateProposeCode()
        {
            //Generate Requisition No.---------Start
            String sDate = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));
            //String dy = datevalue.Day.ToString("00");
            String mn = datevalue.Month.ToString("00");
            String yy = datevalue.Year.ToString().Substring(2, 2);
            var CodeNo = "BP-" + yy + mn;
            String maxId = "";
            String maxNo = (from c in _context.TblBidProposals.Where(c => c.BidProposalNo.Substring(0, 7) == CodeNo).OrderByDescending(t => t.Id) select c.BidProposalNo.Substring(7, 3)).FirstOrDefault();
            if (maxNo == null)
            {
                maxId = "001";
            }
            else
            {
                maxId = (Convert.ToInt16(maxNo) + 1).ToString("000");
            }
            CodeNo = "BP-" + yy + mn + maxId;

            return CodeNo;
        }

        public ActionResult UpdateBidStatus(string BidScheduleNo)
        {
            var BidToUpdate = _context.TblBidSchedules.FirstOrDefault(x => x.BidScheduleNo == BidScheduleNo);
            BidToUpdate.Status = "Bidded";

            return Ok();
        }
    }
}
