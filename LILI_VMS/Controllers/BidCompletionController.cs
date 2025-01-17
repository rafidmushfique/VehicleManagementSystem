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
    public class BidCompletionController : BaseController
    {
        private readonly dbVehicleManagementContext _context;
        public BidCompletionController(dbVehicleManagementContext context)
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
            var UnitCode = HttpContext.Session.GetString("UnitCode");
            var PlantId = long.Parse(HttpContext.Session.GetString("PlantId"));
            //var req = from c in _context.TblBidSchedules
            //          where c.Status != "New" && c.UnitCode == UnitCode && c.PlantId == PlantId
            //          select c;
            var req = from c in _context.TblBidSchedules
                      from a in _context.TblBidSubmissionApprovals
                      where c.Status != "New" && c.UnitCode == UnitCode && c.PlantId == PlantId && c.BidScheduleNo == a.BidScheduleNo
                      select new TblBidSchedule
                      {
                          Id = c.Id,
                          BidScheduleNo = c.BidScheduleNo,
                          BidScheduleDate = c.BidScheduleDate,
                          LoadPoint = c.LoadPoint,
                          CapacityMt = c.CapacityMt,
                          Status = c.Status,
                          CustomerNames = _context.TblBidScheduleCustomers.Where(x => x.BidScheduleNo == c.BidScheduleNo).Select(s => s.CustomerName).Distinct().ToList(),
                          VendorName = _context.TblUserWiseVendorCodes.FirstOrDefault(x=>x.VendorCode == a.VendorCode).VendorName
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
                                      s.Status == "Completed" ? 3 : 4).ThenByDescending(x=>x.Id);
                    break;
            }
             int pageSize =  20;
            return View(await PaginatedList<TblBidSchedule>.CreateAsync(req.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
        public IActionResult BidDetails(int id)
        {
            TblBidSchedule model = new TblBidSchedule();
            var statusList = (from c in _context.TblStatuses
                              where new[] { "Completed", "Rejected", "Hold" }.Contains(c.StatusCode)
                              select new TblStatus { 
                              StatusCode = c.StatusCode,
                              StatusName = c.StatusName,  
                             }).ToList();
            statusList.Insert(0, new TblStatus { StatusCode = "*", StatusName = "Select Status" });

            ViewBag.Status = statusList;
            var Bidno = _context.TblBidSchedules.FirstOrDefault(s => s.Id == id).BidScheduleNo;
            model = _context.TblBidSchedules.FirstOrDefault(s => s.Id == id);
            var vendorCode = _context.TblBidSubmissionApprovals.FirstOrDefault(x => x.BidScheduleNo == Bidno).VendorCode;
            model.VendorCode = vendorCode;

            var spResult = _context.VMVendorInfo.FromSqlRaw("EXEC sp_GetVendorInfo {0}", vendorCode).AsEnumerable().FirstOrDefault();
            model.VendorName = spResult.VendorName;
            model.VendorAddress = spResult.Add1 +","+ spResult.Add2;
            model.VendorContact = spResult.Mobile;


            model.BidAmount = _context.TblBidSubmissionApprovals.FirstOrDefault(x => x.BidScheduleNo == Bidno && x.VendorCode == vendorCode).BidAmount;
            model.VehicleRegNo = _context.TblBidWinnerVehicleInfos.FirstOrDefault(x => x.BidScheduleNo == Bidno && x.VendorCode == vendorCode)?.VehicleRegNo;
            model.DriverName= _context.TblBidWinnerVehicleInfos.FirstOrDefault(x => x.BidScheduleNo == Bidno && x.VendorCode == vendorCode)?.DriverName;
            model.DriverContact = _context.TblBidWinnerVehicleInfos.FirstOrDefault(x => x.BidScheduleNo == Bidno && x.VendorCode == vendorCode)?.DriverContactNo;
            model.BidApproverComment = _context.TblBidSubmissionApprovals.FirstOrDefault(x => x.BidScheduleNo == Bidno).Comments;
            model.TblBidScheduleCustomers = _context.TblBidScheduleCustomers.Where(x=>x.BidScheduleNo == model.BidScheduleNo).ToList();
            model.VendorComment = _context.TblBidWinnerVehicleInfos.FirstOrDefault(x => x.BidScheduleNo == Bidno)?.Comments;
            return View(model);
        }

        public bool Complition(int Id, string Status, string Comments)
        {
            

            try
            {
                var ModelToUpdate = _context.TblBidSchedules.FirstOrDefault(x => x.Id == Id);
                ModelToUpdate.Status = Status;
                ModelToUpdate.Comments = Comments;

                _context.SaveChanges();

                return true;
            }
            catch (Exception)
            {

                return false;
            }
         

        }
    }
}
