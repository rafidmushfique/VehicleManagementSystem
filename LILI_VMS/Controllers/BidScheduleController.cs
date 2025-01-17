using LILI_VMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LILI_VMS.Controllers
{
    [Authorize]
    public class BidScheduleController : BaseController
    {
        private readonly dbVehicleManagementContext _context;

        public BidScheduleController(dbVehicleManagementContext context)
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

            //var VendorCodeTemp = _context.TblUserWiseUnitAndPlantCodes.FirstOrDefault(x => x.UserId == User.Identity.Name).UnitCode;

            var UnitCode = HttpContext.Session.GetString("UnitCode");
            var PlantId = long.Parse(HttpContext.Session.GetString("PlantId"));
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
                          CustomerNames = _context.TblBidScheduleCustomers.Where(x=>x.BidScheduleNo == c.BidScheduleNo).Select(s=>s.CustomerName).Distinct().ToList()
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
                                      s.Status == "Completed" ? 4 : 5).ThenByDescending(x => x.Id);
                    break;
            }
             int pageSize =  20;
            return View(await PaginatedList<TblBidSchedule>.CreateAsync(req.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
        public IActionResult Create() {
           
            TblBidSchedule model = new TblBidSchedule();
            model.BidScheduleDate = DateTime.Now;
            model.BidScheduleStartDate = DateTime.Now;
            model.BidScheduleEndDate = DateTime.Today.AddHours(23).AddMinutes(59); ;
            model.BidScheduleNo = GenerateScheduleCode();
            model.ErrorMessage = "ok";
            if (User.Identity.Name != "admin@yahoo.com")
            {
                //var UnitCode = _context.TblUserWiseUnitAndPlantCodes.FirstOrDefault(x => x.UserId == User.Identity.Name).UnitCode;
                var UnitCode = HttpContext.Session.GetString("UnitCode");

                model.UnitName = _context.TblBusinessUnitSetupInfos.FirstOrDefault(x => x.BusinessCode == UnitCode)?.BusinessName;
            }
                
            return View(model); 
        }
        public ActionResult CreateSchedule(TblBidSchedule model)
        {
            try
            {
                model.Idate = DateTime.Now;
                model.Iuser = User.Identity.Name;
                model.Status = "New";

                //var unitcode = _context.TblUserWiseUnitAndPlantCodes.FirstOrDefault(x => x.UserId == User.Identity.Name)?.UnitCode;
                var UnitCode = HttpContext.Session.GetString("UnitCode");
                var PlantId = long.Parse(HttpContext.Session.GetString("PlantId"));
                model.UnitCode = UnitCode;
                model.PlantId = PlantId;
                _context.Add(model);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                model.ErrorMessage = "Internet connection error! Please check your network.";

                return View("Create", model);
            }
           
        }
        public IActionResult Update(int Id)
        {
            TblBidSchedule model = new TblBidSchedule();
            model = _context.TblBidSchedules.Where(x => x.Id == Id).FirstOrDefault();
            var currentDateTime = DateTime.Now;
            var inTime = false;
            if (currentDateTime > model.BidScheduleStartDate && currentDateTime < model.BidScheduleEndDate)
            {
                inTime = true;
            }
            ViewBag.intime = inTime;
            var customerList = _context.TblBidScheduleCustomers.Where(x => x.BidScheduleNo == model.BidScheduleNo).ToList();
            var vendorList = _context.TblBidScheduleVendors.Where(x => x.BidScheduleNo == model.BidScheduleNo).ToList();
            model.TblBidScheduleVendors = vendorList;
            model.TblBidScheduleCustomers = customerList;
            return View(model);
        }
        public ActionResult UpdateSchedule(TblBidSchedule model) {
            try
            {

                return View(model.Id);
            }
            catch (Exception ex)
            {

                return View(model.Id);
            }
        }

        public ActionResult CustomerInfo() {
            List<VMCustomerInfo> customer = new List<VMCustomerInfo>();
            return PartialView("CustomerSearchModal",customer);
        }
        public JsonResult SearchCustomer(string searchKey) {
            var SearchParameter = (searchKey == null) ? "" : searchKey;
            var model = _context.VMCustomerInfo.FromSqlRaw("Exec sp_SearchBidCustomer {0}", SearchParameter).ToList();
            return Json(model);
        }


        public ActionResult VendorInfo() { 
            List<VMVendorInfo> vendor = new List<VMVendorInfo>();
            return PartialView("VendorSearchModal",vendor);
        }
        public JsonResult SearchVendor(string searchKey)
        {

            var SearchParameter = (searchKey == null) ? "" : searchKey;
            var FoodPlantId = "";
            if (User.Identity.Name != "admin@yahoo.com")
            {
                //var UnitCode = _context.TblUserWiseUnitAndPlantCodes.FirstOrDefault(x => x.UserId == User.Identity.Name).UnitCode;

                var UnitCode = HttpContext.Session.GetString("UnitCode");
                FoodPlantId = _context.TblBusinessUnitSetupInfos.FirstOrDefault(x => x.BusinessCode == UnitCode).FgPlantId;
            }


            var model = _context.VMVendorInfo.FromSqlRaw("Exec sp_SearchBidVendor {0},{1}", SearchParameter, FoodPlantId).ToList();
            return Json(model);
        }

        public bool Delete(int vId)
        {
            try
            {

                if (_context.TblBidSchedules.Any(x => x.Id == vId))
                {
                    var bidModel = _context.TblBidSchedules.First(x => x.Id == vId);
                    var bidCount = _context.TblBidProposals.Count(x => x.BidScheduleNo == bidModel.BidScheduleNo); ;
                    if (bidModel.Status == "New" && bidCount < 1)
                    {
                        if (_context.TblBidScheduleCustomers.Any(x => x.BidScheduleNo == bidModel.BidScheduleNo))
                        {
                            _context.RemoveRange(_context.TblBidScheduleCustomers.Where(x => x.BidScheduleNo == bidModel.BidScheduleNo));
                        }
                        if (_context.TblBidScheduleVendors.Any(x => x.BidScheduleNo == bidModel.BidScheduleNo))
                        {
                            _context.RemoveRange(_context.TblBidScheduleVendors.Where(x => x.BidScheduleNo == bidModel.BidScheduleNo));
                        }
                        _context.Remove(bidModel);
                        _context.SaveChanges();
                    }
                    else {
                        return false;
                    }
                
                
                }


                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public string GenerateScheduleCode() {
            //Generate Requisition No.---------Start
            String sDate = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));
            //String dy = datevalue.Day.ToString("00");
            String mn = datevalue.Month.ToString("00");
            String yy = datevalue.Year.ToString().Substring(2, 2);
            var CodeNo = "SC-" + yy + mn;
            String maxId = "";
            String maxNo = (from c in _context.TblBidSchedules.Where(c => c.BidScheduleNo.Substring(0, 7) == CodeNo).OrderByDescending(t => t.Id) select c.BidScheduleNo.Substring(7, 3)).FirstOrDefault();
            if (maxNo == null)
            {
                maxId = "001";
            }
            else
            {
                maxId = (Convert.ToInt16(maxNo) + 1).ToString("000");
            }
            CodeNo = "SC-" + yy + mn + maxId;

            return CodeNo;
        }
    }
}
