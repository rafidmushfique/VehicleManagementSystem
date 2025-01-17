using LILI_VMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace LILI_VMS.Controllers
{
    public class MaintenanceBillApprovalController : BaseController
    {
        private readonly dbVehicleManagementContext _context;

        public MaintenanceBillApprovalController(dbVehicleManagementContext context)
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

            var UnitCode = HttpContext.Session.GetString("UnitCode");
            var PlantId = long.Parse(HttpContext.Session.GetString("PlantId"));
            var req = (from b in _context.TblMaintenanceBills.DefaultIfEmpty()
                       from r in _context.TblMaintenanceRequisitions
                       from v in _context.TblVehicleSetups
                       from c in _context.TblMaintenanceTypes
                       where b.RequisitionNo == r.RequisitionNo
                       && r.VehicleCode == v.VehicleCode
                       && r.MaintenanceTypeCode == c.MaintenanceTypeCode
                       && b.UnitCode == UnitCode
                       && b.PlantId == PlantId
                       //select p
                       select new TblMaintenanceBill
                       {
                           Id = b.Id,
                           BillNo = b.BillNo,
                           MaintenanceTypeName = c.MaintenanceTypeName,
                           RequisitionNo = b.RequisitionNo,
                           RequisitionDate = r.RequisitionDate,
                           RegistrationNo = v.RegistrationNo,
                           VehicleType = _context.TblVehicleTypes.Where(x => x.VehicleTypeCode == v.TypeOfVehicle).FirstOrDefault().VehicleTypeName,
                           SuppWorkShopName = _context.TblSupplierWorkshopInformations.Where(x => x.SuppWorkShopCode == r.SuppWorkShopCode).FirstOrDefault().SuppWorkShopName,
                           Comments = b.Comments,
                           TotalPrice = _context.TblMaintenanceBillPartsDetails.Where(x => x.BillNo == b.BillNo).Sum(x => x.TotalPrice),
                           Status = _context.TblMaintenanceBillApprovalStatuses.FirstOrDefault(x => x.BillNo == b.BillNo).ApprovalStatus ?? "Pending",
                           Idate = b.Idate,
                       });



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
                    // sort by status: Pending > Approved > Rejected,(number value to for order add more if necessary), then by Id desc
                    req = req.OrderBy(s => s.Status == "Pending" ? 1 :s.Status == "Approved" ? 2 :3).ThenByDescending(s => s.Id);
                    break;
            }
             int pageSize =  20;
            return View(await PaginatedList<TblMaintenanceBill>.CreateAsync(req.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
        public async Task<IActionResult> MaintenanceBillApprovalPortal(int vId) 
        {
            TblMaintenanceBill model = new TblMaintenanceBill();
            var reqNo = _context.TblMaintenanceBills.Where(x=>x.Id==vId).Select(i=>i.RequisitionNo).First();
            var vehicleCode = _context.TblMaintenanceRequisitions.Where(s => s.RequisitionNo == reqNo).First().VehicleCode;
            var workShopCode = _context.TblMaintenanceRequisitions.Where(x => x.RequisitionNo == reqNo).First().SuppWorkShopCode;
            if (reqNo == null)
            {
                NotFound();
            }
            else 
            {
                var result = _context.TblMaintenanceBills.Where(s => s.RequisitionNo == reqNo).First();
              
                var vehicleInfo = _context.TblVehicleSetups.Where(s => s.VehicleCode == vehicleCode).First();
                var reqInfo = _context.TblMaintenanceRequisitions.First(x=>x.RequisitionNo == result.RequisitionNo);
                model.Id = result.Id;
                model.BillNo = _context.TblMaintenanceBills.Where(x => x.Id == vId).Select(i => i.BillNo).First();
                model.VehicleNo = result.VehicleNo;
                model.VehicleType = result.VehicleType;
                model.RequisitionNo = result.RequisitionNo;
                model.RequisitionDate = result.RequisitionDate;
                model.VehicleNo = vehicleCode;
                model.VehicleType = _context.TblVehicleTypes.Where(x=>x.VehicleTypeCode== vehicleInfo.TypeOfVehicle).First().VehicleTypeName;

                model.RegistrationNo = vehicleInfo.RegistrationNo;
                model.Driver = vehicleInfo.DriverName;
                model.LastMaintenanceDate = result.LastMaintenanceDate;
                model.LastMaintenanceKm = result.LastMaintenanceKm;
                model.CurrentMaintenanceDate = result.CurrentMaintenanceDate;
                model.CurrentMaintenanceKm = result.CurrentMaintenanceKm;
                model.SuppWorkShopCode = result.SuppWorkShopCode;
                model.Comments = result.Comments;
                model.SuppWorkShopCode = workShopCode;

                model.SuppWorkShopName = _context.TblSupplierWorkshopInformations.Where(x => x.SuppWorkShopCode == workShopCode).First().SuppWorkShopName ;
                model.MaintenanceTypeName = _context.TblMaintenanceTypes.FirstOrDefault(x => x.MaintenanceTypeCode == reqInfo.MaintenanceTypeCode).MaintenanceTypeName;

                model.Comments= result.Comments;
                var detail_business_list = (from c in _context.TblMaintenanceBillPartsDetails
                                            where (c.BillNo == model.BillNo)
                                            select new TblMaintenanceBillPartsDetail
                                            {
                                                Id = c.Id,
                                                PartsCode = c.PartsCode,
                                                PartsName = _context.TblPartsSetups.Where(x => x.PartsCode == c.PartsCode).FirstOrDefault().PartsName,
                                                RequitionQty = c.RequitionQty,
                                                UnitPrice = c.UnitPrice,
                                                TotalPrice = c.TotalPrice,
                                                ServiceCharge = c.ServiceCharge
                                            });

                ViewBag.TotalCal = detail_business_list.Sum(x => x.TotalPrice);
                model.TblMaintenanceBillPartsDetails = detail_business_list.ToList();
            }

            var userAuthRole = "";

           
                var userRoleId = _context.TblUserAuthRoles.FirstOrDefault(x => x.UserId == User.Identity.Name)?.UserRole;
                var RoleName = _context.AspNetRoles.FirstOrDefault(x => x.Id == userRoleId)?.Name;
                var MenuIdentity = _context.MenuMaster.FirstOrDefault(x => x.UserRoll == RoleName && x.MenuUrl == "MaintenanceBillApproval")?.MenuIdentity;

                userAuthRole = _context.TblUserAuthRoles.FirstOrDefault(x => x.UserId == User.Identity.Name && x.MenuIdentity == MenuIdentity)?.AuthRole;


            
            ViewBag.UserAuthStatus = userAuthRole;

            var userAuthRoleName = "";
            userAuthRoleName = _context.TblAuthRoleTypes.FirstOrDefault(x => x.AuthTypeCode == userAuthRole)?.AuthTypeName;
            ViewBag.UserAuthText = userAuthRoleName;

            return View(model);
        }
        public async Task<JsonResult> MaintenanceBillStatus(string BillNo, string Status)
        {
            try
            {
                if (_context.TblMaintenanceBillApprovalStatuses.Any(x => x.BillNo == BillNo))
                {
                    var BillStatusToUpdate = _context.TblMaintenanceBillApprovalStatuses.Where(x => x.BillNo == BillNo).First();
                    BillStatusToUpdate.Approver = User.Identity.Name;
                    BillStatusToUpdate.ApprovalStatus = Status;
                    BillStatusToUpdate.RequisitionStatusDate = DateTime.Now;
                    if (await TryUpdateModelAsync<TblMaintenanceBillApprovalStatus>(
                       BillStatusToUpdate,
                       "",
                       s => s.Approver,
                       s => s.ApprovalStatus,
                       s => s.RequisitionStatusDate
                       )) ;
                }
                else
                {
                    var UnitCode = HttpContext.Session.GetString("UnitCode");
                    TblMaintenanceBillApprovalStatus gpstatus = new TblMaintenanceBillApprovalStatus();
                    gpstatus.RequisitionStatusDate = DateTime.Now;
                    gpstatus.Approver = User.Identity.Name;
                    gpstatus.ApprovalStatus = Status;
                    gpstatus.BillNo = BillNo;
                    gpstatus.UnitCode = UnitCode;

                    await _context.TblMaintenanceBillApprovalStatuses.AddAsync(gpstatus);
                }
                await _context.SaveChangesAsync();

                var msg = "Maintenance Bill Status Changed To : " + Status;
                return Json(new { success = true, message = msg });
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = ex.Message });
            }


        }
    }
}
