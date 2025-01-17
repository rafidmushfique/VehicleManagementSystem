using LILI_VMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace LILI_VMS.Controllers
{
    [Authorize]
    public class BillApprovalController : BaseController
    {
        private readonly dbVehicleManagementContext _context;
        public BillApprovalController(dbVehicleManagementContext context)
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
                         CustomerName = _context.TblGpbillGpdetails.Where(x => x.BillNo == c.BillNo).Select(x => x.Customer).Distinct().ToList(),
                         Status = _context.TblGpbillApprovalStatuses.Where(x => x.BillNo == c.BillNo).OrderByDescending(x => x.Id).FirstOrDefault().ApprovalStatus ?? "Pending",
                         TotalAmount = c.TotalAmount
                     }); 
            




            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(s => s.BillNo.Contains(searchString) || s.RegNo.Contains(searchString) || s.VehicleCode.Contains(searchString) || s.Status.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    model = model.OrderByDescending(s => s.BillNo);
                    break;

                default:
                    // sort by status: Pending > Approved > Rejected,(number value to for order add more if necessary), then by Id desc
                    var userRoleId = _context.TblUserAuthRoles.FirstOrDefault(x => x.UserId == User.Identity.Name)?.UserRole;
                    var RoleName = _context.AspNetRoles.FirstOrDefault(x => x.Id == userRoleId)?.Name;
                    var MenuIdentity = _context.MenuMaster.FirstOrDefault(x => x.UserRoll == RoleName && x.MenuUrl == "BillApproval")?.MenuIdentity;

                     var userAuthRole = _context.TblUserAuthRoles.FirstOrDefault(x => x.UserId == User.Identity.Name && x.MenuIdentity == MenuIdentity)?.AuthRole;

                    if (userAuthRole == "Checked")
                    {
                        model = model.OrderBy(s => s.Status == "Pending" ? 1 : s.Status == "Checked" ? 2 : s.Status == "Approved" ? 3 : 4).ThenByDescending(s => s.Id);
                        break;
                    }
                    else 
                    {
                        model = model.OrderBy(s => s.Status == "Checked" || s.Status == "Recommend" ? 1 : s.Status == "Pending" ? 2 : s.Status == "Approved" ? 3 : 4).ThenByDescending(s => s.Id);
                        break;
                    }
                


            }
            int pageSize =  15;
            return View(await PaginatedList<TblGpbill>.CreateAsync(model.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public async Task<IActionResult> BillApprovalPortal(int vId)
        {

            TblGpbill model = new TblGpbill();
            model = _context.TblGpbills.Where(x => x.Id == vId).FirstOrDefault();
            if (model == null)
            {
                NotFound();
            }
            else 
            {
                model.RegNo = _context.TblVehicleSetups.Where(x => x.VehicleCode == model.VehicleCode).Select(i => i.RegistrationNo).First().ToString();


                model.TblGpbillExpenseDetails = (from c in _context.TblGpbillExpenseDetails
                                                 from e in _context.TblExpenseHeadSetups
                                                 where c.ExpenseCode == e.ExpenseCode && c.BillNo == model.BillNo
                                                 select new TblGpbillExpenseDetail
                                                 {
                                                     ExpenseCode = e.ExpenseCode,
                                                     ExpenseName = e.ExpenseName,
                                                     Unit = e.Unit,
                                                     Quantity = c.Quantity,
                                                     Rate = c.Rate,
                                                     TotalPrice = c.TotalPrice,
                                                 }
                                               ).ToList();
                var total = model.TblGpbillExpenseDetails.Sum(x => x.TotalPrice);
                model.Total = total;
                var TblGpbillGpdetails = _context.TblGpbillGpdetails.Where(x => x.BillNo == model.BillNo).ToList();
                if (TblGpbillGpdetails != null)
                {
                    model.TblGpbillGpdetails = TblGpbillGpdetails;
                }
                model.TblReturnTripInfos = _context.TblReturnTripInfos.Where(x => x.BillNo == model.BillNo).ToList();

                var userAuthRole = "";
               
             
                    var userRoleId = _context.TblUserAuthRoles.FirstOrDefault(x => x.UserId == User.Identity.Name)?.UserRole;
                    var RoleName = _context.AspNetRoles.FirstOrDefault(x => x.Id == userRoleId)?.Name;
                    var MenuIdentity = _context.MenuMaster.FirstOrDefault(x => x.UserRoll == RoleName && x.MenuUrl == "BillApproval")?.MenuIdentity;
               
                    userAuthRole = _context.TblUserAuthRoles.FirstOrDefault(x => x.UserId == User.Identity.Name && x.MenuIdentity == MenuIdentity)?.AuthRole;
                   



                ViewBag.UserAuthStatus = userAuthRole;

                var userAuthRoleName = "";
                userAuthRoleName = _context.TblAuthRoleTypes.FirstOrDefault(x => x.AuthTypeCode == userAuthRole)?.AuthTypeName;
                ViewBag.UserAuthText = userAuthRoleName;
            }

            return View(model);
        }
        public async Task<JsonResult> BillStatus (string BillNo, string Status)
        {
            try
            {
     
              
                    var UnitCode = HttpContext.Session.GetString("UnitCode");
                    TblGpbillApprovalStatus gpstatus = new TblGpbillApprovalStatus();
                    gpstatus.BillStatusDate = DateTime.Now;
                    gpstatus.Approver = User.Identity.Name;
                    gpstatus.ApprovalStatus = Status;
                    gpstatus.BillNo = BillNo;
                    gpstatus.UnitCode = UnitCode;

                    await _context.AddAsync(gpstatus);
                    
                    await _context.SaveChangesAsync();

                var msg = "Bill Status Changed To : " + Status;
                return Json(new { success = true, message = msg });
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = ex.Message });
            }
  

        }
    }
}
