using LILI_VMS;
using LILI_VMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
    public class MaintenanceRequisitionApproval : BaseController
    {
        private readonly dbVehicleManagementContext _context;

        public MaintenanceRequisitionApproval(dbVehicleManagementContext context)
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
            //IQueryable<LILI_VMS.Models.TblMaintenanceRequisition> model = _context.TblMaintenanceRequisitions;
            var UnitCode = HttpContext.Session.GetString("UnitCode");
            var PlantId = long.Parse(HttpContext.Session.GetString("PlantId"));
            var req = from s in _context.TblMaintenanceRequisitions
                      from v in _context.TblVehicleSetups
                      from p in _context.TblSupplierWorkshopInformations
                      from m in _context.TblMaintenanceTypes
                      where (s.VehicleCode == v.VehicleCode && s.SuppWorkShopCode == p.SuppWorkShopCode && s.MaintenanceTypeCode ==  m.MaintenanceTypeCode && s.UnitCode == UnitCode && s.PlantId == PlantId)
                      select new TblMaintenanceRequisition
                      {
                          Id = s.Id,
                          RequisitionNo = s.RequisitionNo,
                          RequisitionDate = s.RequisitionDate,
                          RegistrationNo = v.RegistrationNo,
                          VehicleTypeName = _context.TblVehicleTypes.FirstOrDefault(x=> x.VehicleTypeCode == v.TypeOfVehicle).VehicleTypeName,
                          SuppWorkShopName = p.SuppWorkShopName,
                          MaintenanceTypeName = m.MaintenanceTypeName,
                          Comments = s.Comments,
                          TotalPrice = _context.TblMaintenanceRequisitionPartsDetails.Where(x => x.RequisitionNo == s.RequisitionNo).Sum(x => x.TotalPrice),
                          ApprovalStatus = _context.TblMaintenanceRequisitionApprovalStatus.Where(x => x.RequisitionNo == s.RequisitionNo).OrderByDescending(x => x.Id).FirstOrDefault().ApprovalStatus ?? "Pending"
                      };


            if (!String.IsNullOrEmpty(searchString))
            {
                //model = model.Where(s => s.RequisitionNo.Contains(searchString) || s.RequisitionNo.Contains(searchString));

                req = req.Where(s => s.RequisitionNo.Contains(searchString)
                       || s.RequisitionNo.Contains(searchString)
                       || s.RequisitionNo.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    //model = model.OrderByDescending(s => s.RequisitionNo);
                    req = req.OrderByDescending(s => s.RequisitionNo);
                    break;

                default:
                    //model = model.OrderBy(s => s.Idate);
                    req = req.OrderByDescending(s => s.RequisitionNo);
                    break;
            }
             int pageSize =  20;
            return View(await PaginatedList<TblMaintenanceRequisition>.CreateAsync(req.AsNoTracking(), pageNumber ?? 1, pageSize));
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

            var detail_business_list = ( from bus in _context.TblMaintenanceRequisitionPartsDetails
                                        //from ex in _context.TblDocumentTypes
                                        where ( bus.RequisitionNo == model.RequisitionNo )
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
            ViewBag.TotalCal = detail_business_list.Sum(x => x.TotalPrice);
            model.TblMaintenanceRequisitionPartsDetails = detail_business_list.ToList();

            var userAuthRole = "";

          
                var userRoleId = _context.TblUserAuthRoles.FirstOrDefault(x => x.UserId == User.Identity.Name)?.UserRole;
                var RoleName = _context.AspNetRoles.FirstOrDefault(x => x.Id == userRoleId)?.Name;
                var MenuIdentity = _context.MenuMaster.FirstOrDefault(x => x.UserRoll == RoleName && x.MenuUrl == "MaintenanceRequisitionApproval")?.MenuIdentity;

                userAuthRole = _context.TblUserAuthRoles.FirstOrDefault(x => x.UserId == User.Identity.Name && x.MenuIdentity == MenuIdentity)?.AuthRole;


            
            ViewBag.UserAuthStatus = userAuthRole;

            var userAuthRoleName = "";
            userAuthRoleName = _context.TblAuthRoleTypes.FirstOrDefault(x => x.AuthTypeCode == userAuthRole)?.AuthTypeName;
            ViewBag.UserAuthText = userAuthRoleName;

            return View(model);
        }

        [HttpPost]
        public IActionResult RequisitionApprovalStatus(string requisitionNo, string approvalStatus)
        {
            try
            {
               
                    var UnitCode = HttpContext.Session.GetString("UnitCode");
                    TblMaintenanceRequisitionApprovalStatus ras = new TblMaintenanceRequisitionApprovalStatus();
                    ras.RequisitionNo = requisitionNo;
                    ras.RequisitionStatusDate = DateTime.Now;
                    ras.ApprovalStatus = approvalStatus;
                    ras.Approver = User.Identity.Name;
                    ras.UnitCode = UnitCode;

                    _context.Add(ras);
                    _context.SaveChanges();




                    //var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                    //var config = builder.Build();
                    //string constring = config.GetConnectionString("DefaultConnection");

                    //SqlConnection con = new SqlConnection(constring);

                    //string queryDel = "DELETE tblMaintenanceRequisitionApprovalStatus WHERE RequisitionNo =" + "'" + ras.RequisitionNo + "'";
                    //SqlCommand cmdDel = new SqlCommand(queryDel, con);
                    //con.Open();
                    //int d = cmdDel.ExecuteNonQuery();
                    //con.Close();

                    //string query = "INSERT INTO tblMaintenanceRequisitionApprovalStatus(RequisitionNo, RequisitionStatusDate, ApprovalStatus, Approver, UnitCode) " +
                    //                "values ('" + ras.RequisitionNo + "','" + ras.RequisitionStatusDate + "','" + ras.ApprovalStatus + "','" + ras.Approver + "','" + ras.UnitCode + "')";
                    //SqlCommand cmd = new SqlCommand(query, con);
                    //con.Open();
                    //int i = cmd.ExecuteNonQuery();
                    //con.Close();

               
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            //return View(student);
            return RedirectToAction(nameof(Index));

        }


    }
}
