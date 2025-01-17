using LILI_VMS;
using LILI_VMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace LILI_VMS.Controllers
{
    [Authorize]
    public class MaintenanceRequisition : BaseController
    {
        private readonly dbVehicleManagementContext _context;

        public MaintenanceRequisition(dbVehicleManagementContext context)
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
            //var UnitCode = _context.TblUserWiseUnitAndPlantCodes.FirstOrDefault(x => x.UserId == User.Identity.Name)?.UnitCode;
            var UnitCode = HttpContext.Session.GetString("UnitCode");
            var PlantId = long.Parse(HttpContext.Session.GetString("PlantId"));
            IQueryable<LILI_VMS.Models.TblMaintenanceRequisition> model = from c in _context.TblMaintenanceRequisitions
                                                                          from v in _context.TblVehicleSetups
                                                                          from w in _context.TblSupplierWorkshopInformations
                                                                          from m in _context.TblMaintenanceTypes
                                                                          where c.VehicleCode == v.VehicleCode && c.SuppWorkShopCode == w.SuppWorkShopCode && c.MaintenanceTypeCode == m.MaintenanceTypeCode && c.UnitCode == UnitCode && c.PlantId == PlantId
                                                                          select new TblMaintenanceRequisition
                                                                          {
                                                                              Id = c.Id,
                                                                              RequisitionNo = c.RequisitionNo,
                                                                              RequisitionDate = c.RequisitionDate,
                                                                              RegistrationNo = v.RegistrationNo,
                                                                              SuppWorkShopName = w.SuppWorkShopName,
                                                                              MaintenanceTypeName =  m.MaintenanceTypeName,
                                                                              ApprovalStatus = _context.TblMaintenanceRequisitionApprovalStatus.Where(x => x.RequisitionNo == c.RequisitionNo).OrderByDescending(x => x.Id).FirstOrDefault().ApprovalStatus ?? "Pending",
                                                                              TotalPrice = _context.TblMaintenanceRequisitionPartsDetails.Where(x=>x.RequisitionNo == c.RequisitionNo).Sum(x=>x.TotalPrice),
                                                                              Comments = c.Comments
                                                                          };


            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(s => s.RequisitionNo.Contains(searchString) || s.RequisitionNo.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    model = model.OrderByDescending(s => s.RequisitionNo);
                    break;

                default:
                    model = model.OrderByDescending(s => s.RequisitionNo);
                    break;
            }
             int pageSize =  20;
            return View(await PaginatedList<TblMaintenanceRequisition>.CreateAsync(model.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public IActionResult Create()
        {
            var PlantId = long.Parse(HttpContext.Session.GetString("PlantId"));
            TblMaintenanceRequisition model = new TblMaintenanceRequisition();

            List<TblMaintenanceType> maintenanceTypesList = new List<TblMaintenanceType>();

            maintenanceTypesList = (from c in _context.TblMaintenanceTypes
                              //where c.Type == "Business"
                            select new TblMaintenanceType
                              {
                                MaintenanceTypeName = c.MaintenanceTypeName,
                                MaintenanceTypeCode = c.MaintenanceTypeCode
                            }).ToList();
            maintenanceTypesList.Insert(0, new TblMaintenanceType
            {
                MaintenanceTypeName = "-Select MaintenanceTypeName-",
                MaintenanceTypeCode = "*"
            });
            ViewBag.MaintenanceTypeList = maintenanceTypesList;

            List<TblPartsSetup> partsSetupList = new List<TblPartsSetup>();

            partsSetupList = (from c in _context.TblPartsSetups
                              where c.PlantId == PlantId
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



            model.RequisitionNo = GenerateRequisitionNo();
            model.RequisitionDate = DateTime.Now;
            //model.LastMaintenanceDate = DateTime.Now;
            model.CurrentMaintenanceDate = DateTime.Now;

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateMaintenanceRequisition(TblMaintenanceRequisition model)
        {
            try
            {
                //TempData["msg"] = "Data Save Unsuccessful";
                //return View("Create", model);

                if (ModelState.IsValid)
                {
                    if (DoesToolCodeExists(model.RequisitionNo))
                    {
                        model.RequisitionNo = GenerateRequisitionNo().ToString();
                    }
                      
                 
                       //var UnitCode = _context.TblUserWiseUnitAndPlantCodes.FirstOrDefault(x => x.UserId == User.Identity.Name)?.UnitCode;
                    var UnitCode = HttpContext.Session.GetString("UnitCode");
                    var PlantId = long.Parse(HttpContext.Session.GetString("PlantId"));
                    List<TblMaintenanceRequisitionPartsDetail> maintenanceRequisitionPartsDetailList = new List<TblMaintenanceRequisitionPartsDetail>();
                    model.UnitCode= UnitCode;
                    model.PlantId = PlantId;
                    model.Iuser = User.Identity.Name;
                    model.Idate = DateTime.Now;
                   

                    //var index = 001;
                    //if (model.TblMaintenanceRequisitionPartsDetails != null)
                    //{
                    //    foreach (var item in model.TblMaintenanceRequisitionPartsDetails)
                    //    {
                    //        TblMaintenanceRequisitionPartsDetail itemDetail = new TblMaintenanceRequisitionPartsDetail();

                    //        itemDetail.RequisitionNo = model.-;
                    //        itemDetail.PartsCode = item.PartsCode;
                    //        itemDetail.RequitionQty = item.RequitionQty;
                    //        itemDetail.UnitPrice = item.UnitPrice;
                    //        itemDetail.TotalPrice = item.TotalPrice;
                    //        itemDetail.ServiceCharge = item.ServiceCharge;

                    //        maintenanceRequisitionPartsDetailList.Add(itemDetail);
                    //        index++;

                    //    }
                    //}

                    //model.TblMaintenanceRequisitionPartsDetails = maintenanceRequisitionPartsDetailList.ToList();

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
                //List<TblMaintenanceType> maintenanceTypesList = new List<TblMaintenanceType>();

                //maintenanceTypesList = (from c in _context.TblMaintenanceTypes
                //                            //where c.Type == "Business"
                //                        select new TblMaintenanceType
                //                        {
                //                            MaintenanceTypeName = c.MaintenanceTypeName,
                //                            MaintenanceTypeCode = c.MaintenanceTypeCode
                //                        }).ToList();
                //maintenanceTypesList.Insert(0, new TblMaintenanceType
                //{
                //    MaintenanceTypeName = "-Select MaintenanceTypeName-",
                //    MaintenanceTypeCode = "*"
                //});
                //ViewBag.MaintenanceTypeList = maintenanceTypesList;

                TempData["msg"] = "Error Occured : "+ex.Message;
                return RedirectToAction(nameof(Create));

            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Update(int vId) {
            TblMaintenanceRequisition model = new TblMaintenanceRequisition();

            List<TblMaintenanceType> maintenanceTypesList = new List<TblMaintenanceType>();

            maintenanceTypesList = (from c in _context.TblMaintenanceTypes
                                        //where c.Type == "Business"
                                    select new TblMaintenanceType
                                    {
                                        MaintenanceTypeName = c.MaintenanceTypeName,
                                        MaintenanceTypeCode = c.MaintenanceTypeCode
                                    }).ToList();
            maintenanceTypesList.Insert(0, new TblMaintenanceType
            {
                MaintenanceTypeName = "-Select MaintenanceTypeName-",
                MaintenanceTypeCode = "*"
            });
            ViewBag.MaintenanceTypeList = maintenanceTypesList;

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

   
            model = _context.TblMaintenanceRequisitions.Where(s => s.Id == vId).First();
            var vehicleInfo= _context.TblVehicleSetups.Where(s => s.VehicleCode == model.VehicleCode).First();

            model.RegistrationNo = vehicleInfo.RegistrationNo;
            model.SuppWorkShopName = _context.TblSupplierWorkshopInformations.Where(x => x.SuppWorkShopCode == model.SuppWorkShopCode).FirstOrDefault().SuppWorkShopName;
            model.VehicleSize = vehicleInfo.SizeOfVehicle;
            model.VehicleTypeName = _context.TblVehicleTypes.Where(x => x.VehicleTypeCode == vehicleInfo.TypeOfVehicle).FirstOrDefault().VehicleTypeName;
            var detail_business_list = (
                                        from bus in _context.TblMaintenanceRequisitionPartsDetails
                                        //from ex in _context.TblDocumentTypes
                                        where ( bus.RequisitionNo == model.RequisitionNo)
                                        orderby bus.Id
                                        select new TblMaintenanceRequisitionPartsDetail
                                        {
                                            Id = bus.Id,
                                            RequisitionNo = bus.RequisitionNo,
                                            PartsCode = bus.PartsCode,
                                            PartsName = _context.TblPartsSetups.Where(x => x.PartsCode == bus.PartsCode).FirstOrDefault().PartsName,
                                            RequitionQty = bus.RequitionQty,
                                            UnitPrice = bus.UnitPrice,
                                            TotalPrice = bus.TotalPrice,
                                            ServiceCharge = bus.ServiceCharge
                                        });

            ViewBag.TotalCal = detail_business_list.Sum(x=>x.TotalPrice);
            model.TblMaintenanceRequisitionPartsDetails = detail_business_list.ToList();


            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateMaintenanceRequisition(TblMaintenanceRequisition model) {
            var vId=   model.Id;
            var requisitionNo = model.RequisitionNo;
            try
            {

                var vehicleSetupToUpdate = await _context.TblMaintenanceRequisitions.FirstOrDefaultAsync(s=>s.Id==vId);
                vehicleSetupToUpdate.Edate= DateTime.Now;
                vehicleSetupToUpdate.Euser= User.Identity.Name;
                if (await TryUpdateModelAsync<TblMaintenanceRequisition>(
                    vehicleSetupToUpdate,
                    "",
                    s => s.RequisitionNo,
                    s => s.RequisitionDate,
                    s => s.MaintenanceTypeCode,
                    s => s.VehicleCode,
                    //s => s.RegistrationNo,
                    //s => s.VehicleTypeName,
                    s => s.Driver,
                    s => s.LastMaintenanceDate,
                    s => s.LastMaintenanceKm,
                    s => s.CurrentMaintenanceDate,
                    s => s.CurrentMaintenanceKm,
                    s => s.SuppWorkShopCode,
                    s => s.SuppWorkShopName,
                      s => s.RunOfKm,
                    s => s.Comments

                    )) ;

                _context.TblMaintenanceRequisitionPartsDetails.RemoveRange(_context.TblMaintenanceRequisitionPartsDetails.Where(s => s.RequisitionNo == requisitionNo));

                foreach (var item in model.TblMaintenanceRequisitionPartsDetails)
                {
                    TblMaintenanceRequisitionPartsDetail maintenanceRequisitionPartsDetail = new TblMaintenanceRequisitionPartsDetail();
                   
                    maintenanceRequisitionPartsDetail.RequisitionNo = item.RequisitionNo;
                    maintenanceRequisitionPartsDetail.PartsCode = item.PartsCode;
                    //maintenanceRequisitionPartsDetail.PartsName = _context.TblPartsSetups.Where(x => x.PartsCode == item.PartsCode).FirstOrDefault().PartsName;
                    maintenanceRequisitionPartsDetail.RequitionQty = item.RequitionQty;
                    maintenanceRequisitionPartsDetail.UnitPrice = item.UnitPrice;
                    maintenanceRequisitionPartsDetail.TotalPrice = item.TotalPrice;
                    maintenanceRequisitionPartsDetail.UnitPrice = item.UnitPrice;
                    maintenanceRequisitionPartsDetail.ServiceCharge = item.ServiceCharge;
                    await _context.AddAsync(maintenanceRequisitionPartsDetail);

                }
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.");
            }
            return View(model);
        }


        public bool Delete(int vId)
        {
            try
            {
                TblMaintenanceRequisition model= _context.TblMaintenanceRequisitions.Where(s=> s.Id == vId).First();

                if (model != null)
                {
                    _context.TblMaintenanceRequisitionPartsDetails.RemoveRange(_context.TblMaintenanceRequisitionPartsDetails.Where(d => d.RequisitionNo == model.RequisitionNo));
                    _context.TblMaintenanceRequisitions.Remove(model);
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
                var detMaintenanceRequisitionPartsDetails = _context.TblMaintenanceRequisitionPartsDetails.Where(d => d.Id == Id).FirstOrDefault();
                _context.TblMaintenanceRequisitionPartsDetails.Remove(detMaintenanceRequisitionPartsDetails);
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

        public ActionResult GetReqPrintView(int Id)
        {
     

            var data = _context.RequisitionInfoVM.FromSqlRaw("Exec sp_GetRequisitionPrintData {0}",Id).ToList();
            var model = data.FirstOrDefault();
            model.PlantName = HttpContext.Session.GetString("UnitName");
            model.PartsDetails = (from m in _context.TblMaintenanceRequisitionPartsDetails
                                  from s in _context.TblPartsSetups
                                  where m.PartsCode == s.PartsCode && m.RequisitionNo == model.RequisitionNo
                                  select new TblMaintenanceRequisitionPartsDetail
                                  {
                                      Id = m.Id,
                                      PartsCode = m.PartsCode,
                                      PartsName = s.PartsName,
                                      BrandName = s.Brand,
                                      RequitionQty = m.RequitionQty,
                                      UnitPrice = m.UnitPrice,
                                      TotalPrice = m.TotalPrice,
                                      Comments = m.Comments
                                  }).ToList();


            model.TotalPriceSum = model.PartsDetails.Sum(m => m.TotalPrice);
        
            return View("ReqPrintView",model);
        }

        #region
        public string GenerateRequisitionNo()
        {
            //var generatedCode = "";
            //// var yearMonth = DateTime.Now.ToString("yyyyMM");

            //    var result = _context.TblMaintenanceRequisitions.OrderBy(x => x.Id).Select(x => x.RequisitionNo).LastOrDefault();
            //    var lastGrn = string.IsNullOrEmpty(result) ? "0000000" : result;


            //    var last5digits = "1";
            //    if (lastGrn.Length > 5)
            //    {
            //        last5digits = lastGrn.Substring(lastGrn.Length - 3);
            //    }

            //    int lastNumber = Int32.Parse(last5digits) + 1;
            //    string lastNumberString = lastNumber.ToString("D5");
            //    //             return $"{companyCode}{plantCode}gr{yearMonth}{lastNumberString}";
            //    generatedCode = $"R-{lastNumberString}";

            //Generate Requisition No.---------Start
            String sDate = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));
            //String dy = datevalue.Day.ToString("00");
            String mn = datevalue.Month.ToString("00");
            String yy = datevalue.Year.ToString().Substring(2, 2);
            var CodeNo = "RQ-" + yy + mn;
            String maxId = "";
            String maxNo = (from c in _context.TblMaintenanceRequisitions.Where(c => c.RequisitionNo.Substring(0, 7) == CodeNo).OrderByDescending(t => t.Id) select c.RequisitionNo.Substring(7, 3)).FirstOrDefault();
            if (maxNo == null)
            {
                maxId = "001";
            }
            else
            {
                maxId = (Convert.ToInt16(maxNo) + 1).ToString("000");
            }
            CodeNo = "RQ-" + yy + mn + maxId;


            return CodeNo;

            
        }
        public bool DoesToolCodeExists(string vToolCode)
        {

            return _context.TblMaintenanceRequisitions.Any(e => e.RequisitionNo == vToolCode);
        }

        #region modal Product Search
        public IActionResult SearchProduct()
        {

            var PlantId = long.Parse(HttpContext.Session.GetString("PlantId"));
            var model = new List<TblVehicleSetup>(from b in _context.TblVehicleSetups
                                                  from p in _context.TblBusinessUnitSetupInfos
                                                  where (b.BusinessCode == p.BusinessCode) && b.PlantId == PlantId
                                                  select new TblVehicleSetup
                                                  {
                                                      VehicleCode = b.VehicleCode,
                                                      BusinessName = p.BusinessName,
                                                      Owner = b.Owner,
                                                      RegistrationNo = b.RegistrationNo,
                                                      Vendor = b.Vendor,
                                                  });

            return PartialView("_ProductPartial", model);
        }

        [HttpPost]
        public JsonResult SetProduct(string productId)
        {
            var keyVal = _context.TblVehicleSetups.Where(c => c.VehicleCode == productId).ToList();
            if (keyVal != null)
            {
                productId = keyVal.FirstOrDefault().VehicleCode;
            }

            var vLastMaintenanceDate = (from c in _context.TblMaintenanceRequisitions
                                        from r in _context.TblMaintenanceRequisitionApprovalStatus
                                        where c.RequisitionNo == r.RequisitionNo && r.ApprovalStatus == "Approved" && c.VehicleCode == productId
                                        select c.CurrentMaintenanceDate).FirstOrDefault();

            var vLastMaintenanceKM = (from c in _context.TblMaintenanceRequisitions
                                      from r in _context.TblMaintenanceRequisitionApprovalStatus
                                      where c.RequisitionNo == r.RequisitionNo && r.ApprovalStatus == "Approved" && c.VehicleCode == productId
                                      select c.CurrentMaintenanceKm).FirstOrDefault();
            if (productId.Length >= 0)
            {
                //var sa = new JsonSerializerSettings();

                var productInfo = from b in _context.TblVehicleSetups
                                  //from p in _context.TblBusinessUnitSetupInfos
                                  where ( b.VehicleCode== @productId)
                                  select new TblVehicleSetup
                                  {
                                      VehicleCode = b.VehicleCode,
                                      RegistrationNo = b.RegistrationNo,
                                      TypeOfVehicle = _context.TblVehicleTypes.Where(x => x.VehicleTypeCode == b.TypeOfVehicle).FirstOrDefault().VehicleTypeName,
                                      SizeOfVehicle = _context.TblVehicleSizes.FirstOrDefault(x=>x.VehicleSizeCode == b.SizeOfVehicle).VehicleSizeName,
                                      DriverName = b.DriverName,
                                      //LastMaintenanceDate = _context.TblMaintenanceRequisitions.Where(x => x.VehicleCode == @productId).Max(x => x.CurrentMaintenanceDate),
                                      LastMaintenanceDate = (vLastMaintenanceDate== default) ? DateTime.Now:vLastMaintenanceDate ,
                                      //LastMaintenanceKm = _context.TblMaintenanceRequisitions.Where(x => x.VehicleCode == @productId).OrderByDescending(x => x.RequisitionNo).First().CurrentMaintenanceKm,
                                      LastMaintenanceKm = (vLastMaintenanceKM== null) ? 0 : vLastMaintenanceKM,

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

            var PlantId = long.Parse(HttpContext.Session.GetString("PlantId"));
            var productList = new List<TblVehicleSetup>(from b in _context.TblVehicleSetups
                                                  from p in _context.TblBusinessUnitSetupInfos
                                                  where (b.BusinessCode == p.BusinessCode) && b.PlantId == PlantId && (b.RegistrationNo.ToUpper().Contains(searchKey.ToUpper()))
                                                  select new TblVehicleSetup
                                                  {
                                                      VehicleCode = b.VehicleCode,
                                                      BusinessName = p.BusinessName,
                                                      Owner = b.Owner,
                                                      RegistrationNo = b.RegistrationNo,
                                                      Vendor = b.Vendor,
                                                  });
            return Json(productList);
        }


       

        #endregion


        #region modal Workshop Search
        public IActionResult SearchWorkshop()
        {
            var PlantId = long.Parse(HttpContext.Session.GetString("PlantId"));
            var model = new List<TblSupplierWorkshopInformation>(from b in _context.TblSupplierWorkshopInformations
                                                  //from p in _context.TblBusinessUnitSetupInfos
                                                  where (b.Type == "WorkShop" && b.PlantId == PlantId)
                                                  select new TblSupplierWorkshopInformation
                                                  {
                                                      SuppWorkShopCode = b.SuppWorkShopCode,
                                                      SuppWorkShopName = b.SuppWorkShopName
                                                  });

            return PartialView("_WorkshopPartial", model);
        }

        [HttpPost]
        public JsonResult SetWorkshop(string productId)
        {
            var keyVal = _context.TblSupplierWorkshopInformations.Where(c => c.SuppWorkShopCode == productId).ToList();
            if (keyVal != null)
            {
                productId = keyVal.FirstOrDefault().SuppWorkShopCode;
            }

            if (productId.Length >= 0)
            {
                //var sa = new JsonSerializerSettings();

                var productInfo = from b in _context.TblSupplierWorkshopInformations
                                      //from p in _context.TblBusinessUnitSetupInfos
                                  where (b.SuppWorkShopCode == @productId)
                                  select new TblSupplierWorkshopInformation
                                  {
                                      SuppWorkShopCode = b.SuppWorkShopCode,
                                      SuppWorkShopName = b.SuppWorkShopName

                                  };

                return Json(productInfo);
            }
            else
            {
                return Json("");
            }
        }

        [HttpPost]
        public JsonResult SearchWorkShopByKey(string searchKey)
        {
            var errorViewModel = new ErrorViewModel();

            var productList = new List<TblSupplierWorkshopInformation>(from b in _context.TblSupplierWorkshopInformations
                                                            //from p in _context.TblBusinessUnitSetupInfos
                                                        where (b.SuppWorkShopCode.ToUpper().Contains(searchKey.ToUpper())|| b.SuppWorkShopName.ToUpper().Contains(searchKey.ToUpper()))
                                                        select new TblSupplierWorkshopInformation
                                                        {
                                                            SuppWorkShopCode = b.SuppWorkShopCode,
                                                            SuppWorkShopName = b.SuppWorkShopName
                                                        });

            return Json(productList);
        }




        #endregion


        #endregion
    }
}
