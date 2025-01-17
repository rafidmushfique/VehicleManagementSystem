using LILI_VMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using NPOI.OpenXmlFormats.Wordprocessing;
using NPOI.SS.Formula.Functions;
using LILI_VMS.Models.ReportsViewModels;
using Microsoft.AspNetCore.Http;

namespace LILI_VMS.Controllers
{
    [Authorize]
    public class BillEntryController : BaseController
    {
        private readonly dbVehicleManagementContext _context;
        public BillEntryController(dbVehicleManagementContext context)
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

            //var data = (from c in _context.TblGpbills
            //           from v in _context.TblVehicleSetups
            //           from b in _context.TblBusinessUnitSetupInfos
            //           where c.BusinessCode == b.BusinessCode && c.VehicleCode == v.VehicleCode
            //           select new TblGpbill
            //           {
            //               BillDate = c.BillDate,
            //               BillNo = c.BillNo,
            //               LoadingDate = c.LoadingDate,
            //               Comments = c.Comments,
            //               VehicleCode = v.VehicleCode,
            //               RegNo = v.RegistrationNo,
            //               VehicleType = v.TypeOfVehicle,

            //           }).AsEnumerable();

            //var UnitCode = _context.TblUserWiseUnitAndPlantCodes.FirstOrDefault(x => x.UserId == User.Identity.Name)?.UnitCode;
            var UnitCode = HttpContext.Session.GetString("UnitCode");
            var PlantId = long.Parse(HttpContext.Session.GetString("PlantId"));
            IQueryable<TblGpbill> model = null;
            if (!string.IsNullOrEmpty(UnitCode))
            {
                model = (from c in _context.TblGpbills
                                               from v in _context.TblVehicleSetups
                                               where c.VehicleCode == v.VehicleCode && (c.UnitCode == UnitCode) && c.PlantId == PlantId
                                               select new TblGpbill
                                               {
                                                   Id = c.Id,
                                                   BillDate = c.BillDate,
                                                   BillNo = c.BillNo,
                                                   LoadingDate = c.LoadingDate,
                                                   Comments = c.Comments,
                                                   VehicleCode = v.VehicleCode,
                                                   RegNo = v.RegistrationNo,
                                                   VehicleType = v.TypeOfVehicle,
                                                   TotalLoadMt = c.TotalLoadMt,
                                                   TotalAmount = c.TotalAmount
                                               }
                               );
            }
            else {
                 model = (from c in _context.TblGpbills
                                               from v in _context.TblVehicleSetups
                                               where c.VehicleCode == v.VehicleCode
                                               select new TblGpbill
                                               {
                                                   Id = c.Id,
                                                   BillDate = c.BillDate,
                                                   BillNo = c.BillNo,
                                                   LoadingDate = c.LoadingDate,
                                                   Comments = c.Comments,
                                                   VehicleCode = v.VehicleCode,
                                                   RegNo = v.RegistrationNo,
                                                   VehicleType = v.TypeOfVehicle,
                                                   TotalLoadMt = c.TotalLoadMt,
                                                   TotalAmount = c.TotalAmount
                                               }
                                   );
            }



            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(s => s.BillNo.Contains(searchString) || s.RegNo.Contains(searchString) || s.VehicleCode.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    model = model.OrderByDescending(s => s.BillNo);
                    break;

                default:
                    model = model.OrderByDescending(s => s.Id);
                    break;
            }
            int pageSize =  20;
            return View(await PaginatedList<TblGpbill>.CreateAsync(model.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public IActionResult Create()
        {
            var UnitCode = HttpContext.Session.GetString("UnitCode");
            var PlantId = long.Parse(HttpContext.Session.GetString("PlantId"));

            TblGpbill model = new TblGpbill();
            model.BillNo = GenerateBillCode();
            model.BillDate = DateTime.Now;
            model.LoadingDate = DateTime.Now;
            model.EntranceDate = DateTime.Now;
            var expenseHeadData= _context.TblExpenseHeadSetups.Where(x=>x.PlantId==PlantId).ToList();
            List<TblGpbillExpenseDetail> expenseList = new List<TblGpbillExpenseDetail>();
            foreach (var item in expenseHeadData)
            { 
             TblGpbillExpenseDetail expense = new TblGpbillExpenseDetail();
                expense.ExpenseCode = item.ExpenseCode;
                expense.ExpenseName = item.ExpenseName;
                expense.Unit = item.Unit;
                expense.Rate = item.Rate;
                expense.Quantity = 0;
                expense.TotalPrice = 0;
                expense.Comments = item.Comments;
                expenseList.Add(expense);
            }

            model.TblGpbillExpenseDetails= expenseList;

            var BusinessList = _context.TblBusinessUnitSetupInfos.Where(x=>x.Type=="Business").ToList();
            BusinessList.Insert(0, new TblBusinessUnitSetupInfo { BusinessCode = "*", BusinessName = "Select Business/Unit" });
            ViewBag.ListOfBusiness = BusinessList.ToList();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateBill(TblGpbill model)
        {
            try
            {
                //TempData["msg"] = "Data Save Unsuccessful";
                //return View("Create", model);

                if (ModelState.IsValid)
                {
                    //var UnitCode = _context.TblUserWiseUnitAndPlantCodes.FirstOrDefault(x => x.UserId == User.Identity.Name)?.UnitCode;
                    var UnitCode = HttpContext.Session.GetString("UnitCode");
                    var PlantId = long.Parse(HttpContext.Session.GetString("PlantId"));
                    if (DoesToolCodeExists(model.BillNo))
                    {
                        model.BillNo = GenerateBillCode();
                    }
                    model.UnitCode = UnitCode;
                    model.PlantId = PlantId;
                    model.Iuser = User.Identity.Name;
                    model.Idate = DateTime.Now;

                    _context.Add(model);
                    await _context.SaveChangesAsync();

                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Error Occured : " + ex.Message;
                return View("Create", model);

            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Update(int vId)
        {

            TblGpbill model = new TblGpbill();
            model = _context.TblGpbills.Where(x => x.Id == vId).FirstOrDefault();
            model.RegNo = _context.TblVehicleSetups.Where(x => x.VehicleCode == model.VehicleCode).Select(i=>i.RegistrationNo).First().ToString();


            if (_context.TblGpbillApprovalStatuses.Any(x => x.BillNo == model.BillNo))
            {
                model.Status = _context.TblGpbillApprovalStatuses.FirstOrDefault(x => x.BillNo == model.BillNo).ApprovalStatus;
            }
            else {
                model.Status = "Pending";
            }
            

            model.TblGpbillExpenseDetails= (from c in _context.TblGpbillExpenseDetails
                                            from e in _context.TblExpenseHeadSetups
                                            where c.ExpenseCode== e.ExpenseCode && c.BillNo== model.BillNo
                                            select new TblGpbillExpenseDetail 
                                            { 
                                             ExpenseCode=e.ExpenseCode,
                                             ExpenseName =e.ExpenseName,
                                             Unit = e.Unit,
                                             Quantity = c.Quantity,
                                             Rate = c.Rate,
                                             TotalPrice = c.TotalPrice,
                                             Comments = c.Comments,
                                            }
                                           ).ToList();

            var total = model.TblGpbillExpenseDetails.Sum(x=>x.TotalPrice);
            model.Total = total;
            var TblGpbillGpdetails = _context.TblGpbillGpdetails.Where(x => x.BillNo == model.BillNo).ToList();
            if (TblGpbillGpdetails != null)
            {
                model.TblGpbillGpdetails = TblGpbillGpdetails;
            }
            model.TblReturnTripInfos = _context.TblReturnTripInfos.Where(x => x.BillNo == model.BillNo).ToList();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateBill(TblGpbill model)
        {
            var vId = model.Id;
            try
            {

                var ApprovalStatus = "";
                if (_context.TblGpbillApprovalStatuses.Any(x => x.BillNo == model.BillNo))
                {
                    ApprovalStatus = _context.TblGpbillApprovalStatuses.FirstOrDefault(x => x.BillNo == model.BillNo).ApprovalStatus;
                }

                if (ApprovalStatus != "Approved")
                {
                    var BillToUpdate = await _context.TblGpbills.FirstOrDefaultAsync(s => s.Id == vId);
                    BillToUpdate.Edate = DateTime.Now;
                    BillToUpdate.Euser = User.Identity.Name;
                    if (await TryUpdateModelAsync<TblGpbill>(
                        BillToUpdate,
                        "",
                        s => s.BillDate,
                        s => s.TotalLoadMt,
                        s => s.StartingKm,
                        s => s.ClosingKm,
                        s => s.Comments,
                        s => s.RunOfKm,
                        s => s.EntranceDate,
                        s => s.TotalAmount,
                        s => s.IsReturned


                        )) ;
               

                    if (_context.TblGpbillGpdetails.Any(x => x.BillNo == model.BillNo))
                    {
                        var GpDetailToRemove = _context.TblGpbillGpdetails.Where(x => x.BillNo == BillToUpdate.BillNo);
                        _context.TblGpbillGpdetails.RemoveRange(GpDetailToRemove);
                    }

                    if (_context.TblGpbillExpenseDetails.Any(x => x.BillNo == model.BillNo))
                    {
                        var GpExpenseToRemove = _context.TblGpbillExpenseDetails.Where(x => x.BillNo == BillToUpdate.BillNo);
                        _context.TblGpbillExpenseDetails.RemoveRange(GpExpenseToRemove);
                    }

                    if (_context.TblReturnTripInfos.Any(x => x.BillNo == model.BillNo))
                    {
                        var ReturnTripToRemove = _context.TblReturnTripInfos.Where(x => x.BillNo == BillToUpdate.BillNo);
                        _context.TblReturnTripInfos.RemoveRange(ReturnTripToRemove);
                    }

                    List<TblGpbillGpdetail> gpdetial = new List<TblGpbillGpdetail>();
                    foreach (var item in model.TblGpbillGpdetails)
                    {
                        TblGpbillGpdetail gpitem = new TblGpbillGpdetail();
                        gpitem.BillNo = model.BillNo;
                        gpitem.CustomerAddress = item.CustomerAddress;
                        gpitem.Customer = item.Customer;
                        gpitem.CustomerCode = item.CustomerCode;
                        gpitem.Gpno = item.Gpno;
                        gpitem.LoadOfMt = item.LoadOfMt;
                        gpdetial.Add(gpitem);
                    }
                    _context.TblGpbillGpdetails.AddRange(gpdetial);

                    List<TblGpbillExpenseDetail> expenseDetails = new List<TblGpbillExpenseDetail>();
                    foreach (var item in model.TblGpbillExpenseDetails)
                    {
                        TblGpbillExpenseDetail exitem = new TblGpbillExpenseDetail();
                        exitem.BillNo = model.BillNo;
                        exitem.ExpenseCode = item.ExpenseCode;
                        exitem.Rate = item.Rate;
                        exitem.Quantity = item.Quantity;
                        exitem.TotalPrice = item.TotalPrice;
                        expenseDetails.Add(exitem);
                    }
                    _context.TblGpbillExpenseDetails.AddRange(expenseDetails);

                    List<TblReturnTripInfo> retrunTripDetails = new List<TblReturnTripInfo>();
                    foreach (var item in model.TblReturnTripInfos)
                    {
                        TblReturnTripInfo tripInfo = new TblReturnTripInfo();
                        tripInfo.BillNo = model.BillNo;
                        tripInfo.CustomerName = item.CustomerName;
                        tripInfo.LoadingDate = item.LoadingDate;
                        tripInfo.UnloadingDate = item.UnloadingDate;
                        tripInfo.LoadingPoint = item.LoadingPoint;
                        tripInfo.UnloadingPoint = item.UnloadingPoint;
                        tripInfo.TotalAmount = item.TotalAmount;
                        tripInfo.Comments = item.Comments;
                        tripInfo.Weight = item.Weight;
                        retrunTripDetails.Add(tripInfo);
                    }

                    _context.TblReturnTripInfos.AddRange(retrunTripDetails);

                    await _context.SaveChangesAsync();

                }
                

                //return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.");
            }
            return RedirectToAction(nameof(Index));
        }
        public bool DoesToolCodeExists(string vCode)
        {

            return _context.TblExpenseHeadSetups.Any(e => e.ExpenseCode == vCode);
        }
        public string GenerateBillCode()
        {
            //Generate Requisition No.---------Start
            String sDate = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));
            //String dy = datevalue.Day.ToString("00");
            String mn = datevalue.Month.ToString("00");
            String yy = datevalue.Year.ToString().Substring(2, 2);
            var CodeNo = "BN-" + yy + mn;
            String maxId = "";
            String maxNo = (from c in _context.TblGpbills.Where(c => c.BillNo.Substring(0, 7) == CodeNo).OrderByDescending(t => t.Id) select c.BillNo.Substring(7, 3)).FirstOrDefault();
            if (maxNo == null)
            {
                maxId = "001";
            }
            else
            {
                maxId = (Convert.ToInt16(maxNo) + 1).ToString("000");
            }
            CodeNo = "BN-" + yy + mn + maxId;

            return CodeNo;

        }

        public IActionResult VehicleInformation()
        {
            TblVehicleSetup model = new TblVehicleSetup();

            return PartialView("_VehicleInformation", model);
        }
        public JsonResult SearchVehicleInformation(string SearchKey)
        {
            var PlantId = long.Parse(HttpContext.Session.GetString("PlantId"));
            var SearchParameter = (SearchKey == null) ? "" : SearchKey;
            var model = _context.VehicleInformationVM.FromSqlRaw("Exec sp_SearchVehicleInformation {0},{1}", SearchParameter,PlantId).ToList();
            return Json(model);
        }

        public JsonResult SetVehicleInfo(string VehicleCode)
        {
            var StartingKm = GetStartingKm(VehicleCode);
            var model = (from c in _context.TblVehicleSetups
                         where c.VehicleCode == VehicleCode
                         select new  {
                             VehicleCode = c.VehicleCode,
                             RegistrationNo = c.RegistrationNo,
                             DriverName = c.DriverName,
                             DriverMobileNo = c.DriverMobileNo,
                             HelperName = c.HelperName,
                             HelperMobileNo = c.HelperMobileNo,
                             Kpl= c.Kpl,
                             StartingKm = StartingKm,

                         }).ToList();
            return Json(model);
        }
        public IActionResult GpInformation()
        {
            TblGpbillGpdetail gpdetail = new TblGpbillGpdetail();
            return PartialView("_GPList", gpdetail);
        }
        public JsonResult SearchGPList(string PlantCode, string SearchKey, string RegNo)
        {
            if (_context.TblUserWiseUnitAndPlantCodes.Any(x => x.UserId == User.Identity.Name))
            {
                PlantCode = _context.TblUserWiseUnitAndPlantCodes.FirstOrDefault(x => x.UserId == User.Identity.Name).PlantId.ToString();
            }
            
            var PlantParameter = (PlantCode == null) ? "" : PlantCode;
            var SearchParameter = (SearchKey == null) ? "" : SearchKey;
            var model = _context.GPListVM.FromSqlRaw("Exec sp_SearchGPList {0},{1}", PlantParameter, SearchParameter).AsEnumerable().Select(
                c=> new GPListVM
                {
                    Id = c.Id,
                    GPNo = c.GPNo,
                    GPDate = c.GPDate,
                    GpDateString= c.GPDate.ToString("yyyy-MM-dd"),
                    VehicleNo = c.VehicleNo,
                    CustomerCode = c.CustomerCode,
                    CustomerName = c.CustomerName,
                    CustomerAddress = c.CustomerAddress,
                    LoadMT = c.LoadMT

                }
                ).ToList();
            if (!string.IsNullOrEmpty(RegNo))
            {
                model = model.Where(x => x.VehicleNo.ToUpper().Contains(RegNo.ToUpper())).ToList(); 
            }
            
            return Json(model);
        }
        public JsonResult SetGPInfo(string PlantCode, string GpNo) 
        {
            if (GpNo == null || GpNo == "")
            {
                return Json("");

            }
            else {
                var PlantParameter = (PlantCode == null) ? "P011" : PlantCode;
            
                var model = _context.GPListVM.FromSqlRaw("Exec sp_GetGPList {0},{1}", PlantParameter, GpNo).AsEnumerable().Select(
                c => new GPListVM
                {
                    Id = c.Id,
                    GPNo = c.GPNo,
                    GPDate = c.GPDate,
                    GpDateString = c.GPDate.ToString("yyyy-MM-dd"),
                    VehicleNo = c.VehicleNo,
                    CustomerCode = c.CustomerCode,
                    CustomerName = c.CustomerName,
                    CustomerAddress = c.CustomerAddress,
                    LoadMT = c.LoadMT,
                    TotalLoadMT = c.TotalLoadMT,

                }
                ).ToList();
                return Json(model);
            }
            
        }
        public JsonResult GetPlantCode(string BusinessCode) 
        {
            var model = _context.TblBusinessUnitSetupInfos.Where(x=>x.BusinessCode== BusinessCode).ToList();
            return Json(model);
        }
        public decimal GetStartingKm(string VehicleNo)
        {
            if (_context.TblGpbills.Any(x => x.VehicleCode == VehicleNo))
            {
                try
                {
                    var lastEntry = _context.TblGpbills
                          .Where(x => x.VehicleCode == VehicleNo)
                          .OrderByDescending(x => x.Id)
                          .Select(x => x.ClosingKm)
                          .FirstOrDefault();
                    return lastEntry + 1;
                }
                catch (Exception ex)
                {

                    return 0;
                }
               
            }
            else {
                return 0;
            }

            
        }

        public bool Delete(int vId)
        {
            try
            {
                TblGpbill model = _context.TblGpbills.Where(s => s.Id == vId).First();
                var ApprovalStatus = "";
                
                if (_context.TblGpbillApprovalStatuses.Any(x => x.BillNo == model.BillNo))
                {
                     ApprovalStatus = _context.TblGpbillApprovalStatuses.FirstOrDefault(x => x.BillNo == model.BillNo).ApprovalStatus;
                }
               

                if (model != null && ApprovalStatus != "Approved")
                {
                    
                    _context.TblGpbillExpenseDetails.RemoveRange(_context.TblGpbillExpenseDetails.Where(d => d.BillNo == model.BillNo));
                    if (_context.TblReturnTripInfos.Any(x => x.BillNo == model.BillNo))
                    {
                        _context.TblReturnTripInfos.RemoveRange(_context.TblReturnTripInfos.Where(x=>x.BillNo==model.BillNo));
                    }
                    if (_context.TblGpbillGpdetails.Any(x => x.BillNo == model.BillNo))
                    {
                        _context.TblGpbillGpdetails.RemoveRange(_context.TblGpbillGpdetails.Where(d => d.BillNo == model.BillNo));
                    }
                       
                    _context.TblGpbills.Remove(model);
                    _context.SaveChanges();
                    return true;
                }
                else
                {

                    return false;
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Error Occurred while trying to Delete data.";
                return false;
            }
        }

        public ActionResult GetPrintView(int Id) {
            if (!_context.TblGpbills.Any(x => x.Id == Id)) {
                NotFound();
            }
            var BillNo = _context.TblGpbills.FirstOrDefault(x => x.Id == Id).BillNo;


            var model = _context.BillInfoVM.FromSqlRaw("Exec sp_GetBillExpenseInfo {0}", BillNo);


            var expenseDetail = (from c in _context.TblGpbillExpenseDetails
                          from v in _context.TblExpenseHeadSetups
                          where c.ExpenseCode == v.ExpenseCode && c.BillNo== BillNo
                          select new TblGpbillExpenseDetail
                          {
                            BillNo = c.BillNo,
                            ExpenseName = v.ExpenseName,
                            ExpenseCode = c.ExpenseCode,
                            Unit = v.Unit,
                            Rate = c.Rate,
                            Quantity = c.Quantity,
                            TotalPrice = c.TotalPrice,
                            Comments = c.Comments,
                          }).ToList() ;
            var result = _context.TblGpbillGpdetails.Where(x => x.BillNo == BillNo).ToList();
            var GpList = result.Select(m => m.Gpno).Distinct().ToList();
            var CustomerAddressList = result.Select(x=>x.CustomerAddress).ToList();
           
            var gpListString = "";
            var customerAddressString = "";
            for (int i = 0; i < GpList.Count; i++)
            {
                gpListString += GpList[i];

                // Add a comma if it's not the last item
                if (i < GpList.Count - 1)
                {
                    gpListString += ",\n";
                }
            }

           
            var totalPriceSum = expenseDetail.Sum(e => e.TotalPrice);

          
            var totalPriceExcluding = expenseDetail
                .Where(e => e.ExpenseCode != "EC-2404005")
                .Sum(e => e.TotalPrice);
            ViewBag.GpNoString = gpListString;
            ViewBag.ExpenseDetails = expenseDetail;
            ViewBag.CustomerAddress = CustomerAddressList;
            ViewBag.TotalPriceSum = totalPriceSum;
            ViewBag.TotalPriceExcludingFuel = totalPriceExcluding;
            return View("_BillPrintView",model);
        }

    }
    }

