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
    public class WinningBidController : Controller
    {
        private readonly dbVehicleManagementContext _context;

        public WinningBidController(dbVehicleManagementContext context)
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
            var VendorCodeTemp = _context.TblUserWiseVendorCodes.Where(x => x.UserId == User.Identity.Name).Select(i => i.VendorCode).ToList();
            var req =  from c in _context.TblBidSchedules
                       from a in _context.TblBidSubmissionApprovals
                       from b in _context.TblBusinessUnitSetupInfos
                       where c.BidScheduleNo == a.BidScheduleNo  && VendorCodeTemp.Contains(a.VendorCode) && b.BusinessCode == c.UnitCode
                       select new TblBidSchedule
                       {
                           Id = c.Id,
                           BidScheduleNo = c.BidScheduleNo,
                           BidScheduleDate = c.BidScheduleDate,
                           BidScheduleStartDate = c.BidScheduleStartDate,
                           BidScheduleEndDate = c.BidScheduleEndDate,
                           CapacityMt = c.CapacityMt,
                           Status = c.Status,
                           CustomerNames = _context.TblBidScheduleCustomers.Where(x => x.BidScheduleNo == c.BidScheduleNo).Select(s => s.CustomerName).Distinct().ToList(),
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
                                    s.Status == "Completed" ? 3 : 4).ThenByDescending(x => x.Id);
                    break;
            }
             int pageSize =  20;
            return View(await PaginatedList<TblBidSchedule>.CreateAsync(req.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
        public IActionResult Edit(int Id)
        {
            //if (!_context.TblBidSchedules.Any(x => x.Id == Id))
            //{ 
            // NotFound();
            //}

            TblBidWinnerVehicleInfo model = new TblBidWinnerVehicleInfo();
            //var VendorCodeTemp = _context.TblUserWiseVendorCodes.FirstOrDefault(x => x.UserId == User.Identity.Name)?.VendorCode;
            var VendorCodeTemp = _context.TblUserWiseVendorCodes.Where(x => x.UserId == User.Identity.Name).Select(i => i.VendorCode).ToList();
            var result = _context.TblBidSchedules.FirstOrDefault(x => x.Id == Id);
            var currentDateTime = DateTime.Now;
            var inTime = false;
            if (currentDateTime > result.BidScheduleStartDate && currentDateTime < result.BidScheduleEndDate)
            {
                inTime = true;
            }
            ViewBag.intime = inTime;

            model.Status = result.Status;
            var check = _context.TblBidWinnerVehicleInfos.Any(x => x.BidScheduleNo == result.BidScheduleNo && VendorCodeTemp.Contains(x.VendorCode));
            if (check)
            {
                var bidWresult = _context.TblBidWinnerVehicleInfos.FirstOrDefault(x => x.BidScheduleNo == result.BidScheduleNo && VendorCodeTemp.Contains(x.VendorCode) );
                model.VehicleRegNo = bidWresult?.VehicleRegNo;
                model.DriverContactNo = bidWresult?.DriverContactNo;
                model.DriverName = bidWresult?.DriverName;
                model.Comments= bidWresult?.Comments;
                model.VendorCode = bidWresult?.VendorCode;
            }
            model.BidScheduleNo =result.BidScheduleNo;
            model.BidScheduleDate = result.BidScheduleDate;
            model.BidScheduleStartDate = result.BidScheduleStartDate;
            model.BidScheduleEndDate = result.BidScheduleEndDate;
            model.LoadPoint = result.LoadPoint;
            model.CapacityMt = result.CapacityMt;
            model.VendorCode = _context.TblBidSubmissionApprovals.FirstOrDefault(x => x.BidScheduleNo == model.BidScheduleNo).VendorCode;

            ViewBag.CustomerList = _context.TblBidScheduleCustomers.Where(x => x.BidScheduleNo == result.BidScheduleNo).ToList();


            return View(model);
        }
        public ActionResult CreateOrEditBidVehicleInfo(TblBidWinnerVehicleInfo model)
        {
            try
            {

                var check = _context.TblBidWinnerVehicleInfos.Any(x => x.BidScheduleNo == model.BidScheduleNo && x.VendorCode == model.VendorCode);
                if (check)
                {
                    TblBidWinnerVehicleInfo bidToUpdate = new TblBidWinnerVehicleInfo();
                    bidToUpdate = _context.TblBidWinnerVehicleInfos.FirstOrDefault(x => x.BidScheduleNo == model.BidScheduleNo && x.VendorCode == model.VendorCode);
                    bidToUpdate.VehicleRegNo = model.VehicleRegNo;
                    bidToUpdate.DriverName = model.DriverName;
                    bidToUpdate.DriverContactNo = model.DriverContactNo;
                    bidToUpdate.Comments = model.Comments;
                }
                else 
                {
                   
                   
                    model.EntryNo = GenerateEntryCode();
                    model.EntryDate = DateTime.Now;
                    _context.Add(model);
                  
                }
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                return RedirectToAction("Index");
            }
        }

        public string GenerateEntryCode()
        {
            //Generate Requisition No.---------Start
            String sDate = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));
            //String dy = datevalue.Day.ToString("00");
            String mn = datevalue.Month.ToString("00");
            String yy = datevalue.Year.ToString().Substring(2, 2);
            var CodeNo = "OB-" + yy + mn;
            String maxId = "";
            String maxNo = (from c in _context.TblBidWinnerVehicleInfos.Where(c => c.EntryNo.Substring(0, 7) == CodeNo).OrderByDescending(t => t.Id) select c.EntryNo.Substring(7, 3)).FirstOrDefault();
            if (maxNo == null)
            {
                maxId = "001";
            }
            else
            {
                maxId = (Convert.ToInt16(maxNo) + 1).ToString("000");
            }
            CodeNo = "OB-" + yy + mn + maxId;

            return CodeNo;
        }
    }
}
