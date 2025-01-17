using LILI_VMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace LILI_VMS.Controllers
{
    [Authorize]
    public class MaintenanceBill : BaseController
    {
        private readonly dbVehicleManagementContext _context;

        public MaintenanceBill(dbVehicleManagementContext context)
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
            //var UnitCode = _context.TblUserWiseUnitAndPlantCodes.FirstOrDefault(x => x.UserId == User.Identity.Name)?.UnitCode;
            var UnitCode = HttpContext.Session.GetString("UnitCode");
            var PlantId = long.Parse(HttpContext.Session.GetString("PlantId"));
            var req = (from b in _context.TblMaintenanceBills.DefaultIfEmpty()
                       from r in _context.TblMaintenanceRequisitions 
                       from v in _context.TblVehicleSetups 
                       from c in _context.TblMaintenanceTypes 
                        where  b.RequisitionNo ==  r.RequisitionNo 
                        && r.VehicleCode == v.VehicleCode 
                        && r.MaintenanceTypeCode == c.MaintenanceTypeCode
                        && b.UnitCode== UnitCode
                        && b.PlantId == PlantId
                       //select p
                       select new TblMaintenanceBill
                       {
                           Id = b.Id,
                           BillNo = b.BillNo,
                           MaintenanceTypeName = c.MaintenanceTypeName,
                           RequisitionNo = b.RequisitionNo,
                           RequisitionDate = r.RequisitionDate,
                           RegistrationNo =v.RegistrationNo,
                           VehicleType = _context.TblVehicleTypes.Where(x => x.VehicleTypeCode == v.TypeOfVehicle).FirstOrDefault().VehicleTypeName,
                           SuppWorkShopName = _context.TblSupplierWorkshopInformations.Where(x => x.SuppWorkShopCode == r.SuppWorkShopCode).FirstOrDefault().SuppWorkShopName,
                           Comments=b.Comments,
                           TotalPrice = _context.TblMaintenanceBillPartsDetails.Where(x=>x.BillNo == b.BillNo).Sum(x=>x.TotalPrice),
                           Status = _context.TblMaintenanceBillApprovalStatuses.FirstOrDefault(x=>x.BillNo == b.BillNo).ApprovalStatus ?? "Pending",
                           Idate = b.Idate,
                       }) ;



            if (!String.IsNullOrEmpty(searchString))
            {
                req = req.Where(s => s.BillNo.Contains(searchString) || s.RequisitionNo.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    req = req.OrderByDescending(s => s.BillNo);
                    break;

                default:
                    req = req.OrderByDescending(s => s.BillNo);
                    break;
            }
             int pageSize =  20;
            return View(await PaginatedList<TblMaintenanceBill>.CreateAsync(req.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public IActionResult Create()
        {  
            TblMaintenanceBill model = new TblMaintenanceBill();


            List<TblPartsSetup> partsSetupList = new List<TblPartsSetup>();

            partsSetupList = (from c in _context.TblPartsSetups
                                  //where c.Type == "Business"
                              select new TblPartsSetup
                              {
                                  PartsName = c.PartsName,
                                  PartsCode = c.PartsCode
                              }).ToList();
            partsSetupList.Insert(0, new TblPartsSetup
            {
                PartsName = "-Select PartsName-",
                PartsCode = "*"
            });
            ViewBag.PartsSetupList = partsSetupList;
            model.BillNo = GenerateBillNo();
            model.BillDate = DateTime.Now;
            model.CurrentMaintenanceDate = DateTime.Now;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateMaintenanceBill(TblMaintenanceBill model)
        {
            try
            {

                
                if (ModelState.IsValid)
                {
                    //var  UnitCode = _context.TblUserWiseUnitAndPlantCodes.FirstOrDefault(x => x.UserId == User.Identity.Name)?.UnitCode;
                    var PlantId = long.Parse(HttpContext.Session.GetString("PlantId"));
                    var UnitCode = HttpContext.Session.GetString("UnitCode");
                    if (DoesToolCodeExists(model.BillNo))
                    {
                        model.BillNo = GenerateBillNo().ToString();
                    }
                    model.Iuser = User.Identity.Name;
                    model.Idate = DateTime.Now;
                    model.PlantId = PlantId;
                    model.UnitCode = UnitCode;
                    _context.Add(model);
                    await _context.SaveChangesAsync();


                    TempData["msg"] = "Success message text.";

                }

                else
                {
                    TempData["msg"] = "Data Save Unsuccessful";
                   
                }
            }
            catch (Exception ex)
            {

                TempData["msg"] = "Error Occured : "+ex.Message;
                return View("Create", model);

            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Update(int vId) {

          
            TblMaintenanceBill model = new TblMaintenanceBill();
            model = _context.TblMaintenanceBills.FirstOrDefault(x => x.Id == vId);
            var partsDetails = _context.TblMaintenanceBillPartsDetails.Where(x=>x.BillNo==model.BillNo).ToList();

            var vehicleCode = _context.TblMaintenanceRequisitions.FirstOrDefault(x => x.RequisitionNo == model.RequisitionNo).VehicleCode;
            var vehicleInfo =_context.TblVehicleSetups.FirstOrDefault(x=>x.VehicleCode==vehicleCode);
            model.VehicleNo = vehicleCode;
            model.RegistrationNo = _context.TblVehicleSetups.FirstOrDefault(x => x.VehicleCode == vehicleCode).RegistrationNo;
            model.VehicleType = _context.TblVehicleTypes.FirstOrDefault(x => x.VehicleTypeCode == vehicleInfo.TypeOfVehicle).VehicleTypeName;
            model.Driver = vehicleInfo.DriverName;

            model.CurrentMaintenanceDate = _context.TblMaintenanceRequisitions.FirstOrDefault(x => x.RequisitionNo == model.RequisitionNo).CurrentMaintenanceDate;
            model.CurrentMaintenanceKm = _context.TblMaintenanceRequisitions.FirstOrDefault(x => x.RequisitionNo == model.RequisitionNo).CurrentMaintenanceKm;
            model.LastMaintenanceDate = _context.TblMaintenanceRequisitions.FirstOrDefault(x => x.RequisitionNo == model.RequisitionNo).LastMaintenanceDate;
            model.LastMaintenanceKm = _context.TblMaintenanceRequisitions.FirstOrDefault(x => x.RequisitionNo == model.RequisitionNo).LastMaintenanceKm;
            var workShopCode = _context.TblMaintenanceRequisitions.FirstOrDefault(x => x.RequisitionNo == model.RequisitionNo).SuppWorkShopCode;
            model.SuppWorkShopCode = workShopCode;


            model.SuppWorkShopName = _context.TblSupplierWorkshopInformations.FirstOrDefault(x => x.SuppWorkShopCode == workShopCode).SuppWorkShopName;

            List<TblMaintenanceBillPartsDetail> PartsData = new List<TblMaintenanceBillPartsDetail>();
            foreach (var item in partsDetails)
            {
                TblMaintenanceBillPartsDetail part = new TblMaintenanceBillPartsDetail();
            
                part.BillNo = item.BillNo;
                part.PartsCode = item.PartsCode;
                part.PartsName = _context.TblPartsSetups.FirstOrDefault(x=>x.PartsCode==item.PartsCode).PartsName;
                part.RequitionQty = item.RequitionQty;
                part.UnitPrice = item.UnitPrice;
                part.TotalPrice = item.TotalPrice;
                part.ServiceCharge = item.ServiceCharge;
                PartsData.Add(part);
            }
            ViewBag.TotalCal = PartsData.Sum(x => x.TotalPrice);
            model.TblMaintenanceBillPartsDetails= PartsData;
            List<TblPartsSetup> partsSetupList = new List<TblPartsSetup>();

            partsSetupList = (from c in _context.TblPartsSetups
                                  //where c.Type == "Business"
                              select new TblPartsSetup
                              {
                                  PartsName = c.PartsName,
                                  PartsCode = c.PartsCode
                              }).ToList();
            partsSetupList.Insert(0, new TblPartsSetup
            {
                PartsName = "-Select PartsName-",
                PartsCode = "*"
            });
            ViewBag.PartsSetupList = partsSetupList;

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateMaintenanceBill(TblMaintenanceBill model) {
            var vId=   model.Id;
            try
            {
                _context.TblMaintenanceBillPartsDetails.RemoveRange(_context.TblMaintenanceBillPartsDetails.Where(s => s.BillNo == model.BillNo));
                List<TblMaintenanceBillPartsDetail> PartsDtl = new List<TblMaintenanceBillPartsDetail>();
                foreach (var item in model.TblMaintenanceBillPartsDetails)
                {
                    TblMaintenanceBillPartsDetail Parts = new TblMaintenanceBillPartsDetail();
                    Parts.PartsCode = item.PartsCode;
                    Parts.BillNo = model.BillNo;
                    //maintenanceRequisitionPartsDetail.PartsName = _context.TblPartsSetups.Where(x => x.PartsCode == item.PartsCode).FirstOrDefault().PartsName;
                    Parts.RequitionQty = item.RequitionQty;
                    Parts.UnitPrice = item.UnitPrice;
                    Parts.TotalPrice = item.TotalPrice;
                    Parts.UnitPrice = item.UnitPrice;
                    Parts.ServiceCharge = item.ServiceCharge;
                    PartsDtl.Add(Parts);
                }
                await _context.AddRangeAsync(PartsDtl);
                await _context.SaveChangesAsync();
            
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.");
            }
            return RedirectToAction(nameof(Index));
        }


        public bool Delete(int vId)
        {
            try
            {
                TblMaintenanceBill model= _context.TblMaintenanceBills.Where(s=> s.Id == vId).First();

                if (model != null)
                {
                    _context.TblMaintenanceBillApprovalStatuses.RemoveRange(_context.TblMaintenanceBillApprovalStatuses.Where(d => d.BillNo == model.BillNo));
                    _context.TblMaintenanceBillPartsDetails.RemoveRange(_context.TblMaintenanceBillPartsDetails.Where(d => d.BillNo == model.BillNo));
                    _context.TblMaintenanceBills.Remove(model);
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

        [HttpPost]
        public async Task<ActionResult> DeleteDetail(int Id)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var detMaintenanceBillPartsDetails = _context.TblMaintenanceBillPartsDetails.Where(d => d.Id == Id).FirstOrDefault();
                _context.TblMaintenanceBillPartsDetails.Remove(detMaintenanceBillPartsDetails);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

            }
            finally
            {
                await transaction.CommitAsync();
            }
            return Ok();
        }


        [HttpPost]
        public JsonResult GetMaintenanceBillPartsDetail(string productId, string billNo)
        {
            var errorViewModel = new ErrorViewModel();
           // var sa = new JsonSerializerSettings();

            var productList = (
                                        from bus in _context.TblMaintenanceRequisitionPartsDetails
                                        where (bus.RequisitionNo == productId)
                                        orderby bus.Id

                                        select new TblMaintenanceBillPartsDetail
                                        {
                                            Id = bus.Id,
                                            //RequisitionNo = bus.RequisitionNo,
                                            BillNo = billNo,
                                            PartsCode = bus.PartsCode,
                                            PartsName = _context.TblPartsSetups.Where(x => x.PartsCode == bus.PartsCode).FirstOrDefault().PartsName,
                                            RequitionQty = bus.RequitionQty,
                                            UnitPrice = bus.UnitPrice,
                                            TotalPrice = bus.TotalPrice,
                                            ServiceCharge = bus.ServiceCharge
                                        });


            return Json(productList);
        }


      
        public string GenerateBillNo()
        {
            var generatedCode = "";
            // var yearMonth = DateTime.Now.ToString("yyyyMM");

                var result = _context.TblMaintenanceBills.OrderBy(x => x.Id).Select(x => x.BillNo).LastOrDefault();
                var lastGrn = string.IsNullOrEmpty(result) ? "0000000" : result;


                var last5digits = "1";
                if (lastGrn.Length > 5)
                {
                    last5digits = lastGrn.Substring(lastGrn.Length - 3);
                }

                int lastNumber = Int32.Parse(last5digits) + 1;
                string lastNumberString = lastNumber.ToString("D5");
                //             return $"{companyCode}{plantCode}gr{yearMonth}{lastNumberString}";
                generatedCode = $"B-{lastNumberString}";

            //Generate Requisition No.---------Start
            String sDate = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));
            //String dy = datevalue.Day.ToString("00");
            String mn = datevalue.Month.ToString("00");
            String yy = datevalue.Year.ToString().Substring(2, 2);
            var CodeNo = "BM-" + yy + mn;
            String maxId = "";
            String maxNo = (from c in _context.TblMaintenanceBills.Where(c => c.BillNo.Substring(0, 7) == CodeNo).OrderByDescending(t => t.Id) select c.BillNo.Substring(7, 3)).FirstOrDefault();
            if (maxNo == null)
            {
                maxId = "001";
            }
            else
            {
                maxId = (Convert.ToInt16(maxNo) + 1).ToString("000");
            }
            CodeNo = "BM-" + yy + mn + maxId;


            return CodeNo;
            //return generatedCode;
        }
        public bool DoesToolCodeExists(string vToolCode)
        {

            return _context.TblMaintenanceBills.Any(e => e.BillNo == vToolCode);
        }

      
        public IActionResult SearchRequisition()
        {
            var PlantId = long.Parse(HttpContext.Session.GetString("PlantId"));
            var model = new List<TblMaintenanceRequisition>(from b in _context.TblMaintenanceRequisitions
                                                            from r in _context.TblMaintenanceRequisitionApprovalStatus 
                                                            from v in _context.TblVehicleSetups
                                                            where b.VehicleCode==v.VehicleCode && b.RequisitionNo== r.RequisitionNo && r.ApprovalStatus == "Approved" && b.PlantId == PlantId
                                                            select new TblMaintenanceRequisition
                                                            {
                                                                RequisitionNo = b.RequisitionNo,
                                                                RegistrationNo = v.RegistrationNo,
                                                                VehicleCode = b.VehicleCode,
                                                                Owner = v.Owner,
                                                                Vendor = v.Vendor,
                                                            });

            return PartialView("_ReqPartial", model);
        }

        [HttpPost]
        public JsonResult SetRequisition(string productId) //productId 
        {

            var keyVal = _context.TblMaintenanceRequisitions.Where(c => c.RequisitionNo == productId).ToList();
            if (keyVal != null)
            {
                productId = keyVal.FirstOrDefault().RequisitionNo;
            }

            if (productId.Length >= 0)
            {
                //var sa = new JsonSerializerSettings();

                var productInfo = from r in _context.TblMaintenanceRequisitions
                                  from v in _context.TblVehicleSetups
                                  from c in _context.TblMaintenanceTypes
                                  where ( r.RequisitionNo == @productId && r.VehicleCode==v.VehicleCode && r.MaintenanceTypeCode == c.MaintenanceTypeCode)
                                  select new TblMaintenanceBill
                                  {
                                      RequisitionNo= productId,
                                      VehicleNo = r.VehicleCode,
                                      RegistrationNo =v.RegistrationNo,
                                      MaintenanceTypeName = c.MaintenanceTypeName,
                                      Driver =v.DriverName,
                                      VehicleType = _context.TblVehicleTypes.Where(x => x.VehicleTypeCode == v.TypeOfVehicle).FirstOrDefault().VehicleTypeName,
                                      LastMaintenanceDate=r.LastMaintenanceDate,
                                      LastMaintenanceKm=r.LastMaintenanceKm,
                                      CurrentMaintenanceDate=r.CurrentMaintenanceDate,
                                      CurrentMaintenanceKm=r.CurrentMaintenanceKm,
                                      SuppWorkShopCode=r.SuppWorkShopCode,
                                      SuppWorkShopName = _context.TblSupplierWorkshopInformations.Where(x => x.SuppWorkShopCode == r.SuppWorkShopCode).FirstOrDefault().SuppWorkShopName,

                                  };

                return Json(productInfo);
            }
            else
            {
                return Json("");
            }
        }


        [HttpPost]
        public JsonResult SearchProductByKey(string searchKey)
        {
            var errorViewModel = new ErrorViewModel();
            List<TblMaintenanceRequisition> model = new List<TblMaintenanceRequisition>();
            model = (from b in _context.TblMaintenanceRequisitions
                     from v in _context.TblVehicleSetups
                     where b.VehicleCode == v.VehicleCode
                     &&( v.RegistrationNo.Contains(searchKey) || b.RequisitionNo.Contains(searchKey))
                     select new TblMaintenanceRequisition
                     {
                         RequisitionNo = b.RequisitionNo,
                         RegistrationNo = v.RegistrationNo,
                         VehicleCode = b.VehicleCode,
                         Owner = v.Owner,
                         Vendor = v.Vendor,
                     }).ToList();

            return Json(model);
        }

        public ActionResult MaintenanceBillPrintView(int Id) 
        {
            if (!_context.TblMaintenanceBills.Any(x => x.Id == Id))
            { 
             NotFound();
            }
            var BillNo = _context.TblMaintenanceBills.FirstOrDefault(x => x.Id == Id).BillNo;
            var model = _context.VMMaintenanceBill.FromSqlRaw("Exec sp_MaintenanceBillPrintView {0}", BillNo).ToList();
            var totalSum = model.Sum(x => x.TotalPrice);
            ViewBag.TotalAmount = totalSum;
            return View("_MaintenanceBillPrintView",model);
        }
        
    }
}
