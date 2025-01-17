using LILI_VMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using NPOI.POIFS.Crypt.Dsig;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace LILI_VMS.Controllers
{
    [Authorize]
    public class TyreChangeController : BaseController
    {
        private readonly dbVehicleManagementContext _context;

        public TyreChangeController(dbVehicleManagementContext context)
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
            //IQueryable<TblTyreChangeLog> model = _context.TblTyreChangeLogs.Where(x=>x.PlantId == PlantId);

            var model = from c in _context.TblTyreChangeLogs
                        from v in _context.TblVehicleSetups
                        where c.VehicleCode == v.VehicleCode
                        select new TblTyreChangeLog
                        {
                         Id =c.Id,
                         TyreChangeCode = c.TyreChangeCode,
                         RegistrationNo = v.RegistrationNo,
                         Comments = c.Comments,
                         ChangeType = c.ChangeType
                        };

            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(s => s.TyreChangeCode.Contains(searchString)|| s.RegistrationNo.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    model = model.OrderByDescending(s => s.TyreChangeCode);
                    break;

                default:
                    model = model.OrderBy(s =>
                                      s.ChangeType == "Assign" ? 1 :
                                      s.ChangeType == "Exchange" ? 2 :
                                      s.ChangeType == "Replace" ? 3 :4 ).ThenByDescending(x => x.Id);
                    break;
            }
             int pageSize =  20;
            return View(await PaginatedList<TblTyreChangeLog>.CreateAsync(model.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
        public async Task<IActionResult> Create()
        {
            TblTyreChangeLog model = new TblTyreChangeLog();
            model.TyreChangeCode = GenerateCode();
            var vehicleList = _context.TblVehicleSetups.ToList();
            ViewBag.VehicleList = vehicleList;
            
            return View(model);
        }
        public async Task<IActionResult> Update(int Id)
        {
            if (!_context.TblTyreChangeLogs.Any(x => x.Id == Id))
            {
                NotFound();
            }
            var data = await _context.TblTyreChangeLogs.FirstOrDefaultAsync(x => x.Id == Id);
            var vehicleList = _context.TblVehicleSetups.ToList();
            ViewBag.VehicleList = vehicleList;
            ViewBag.TyreListInfo = (from c in _context.TblTyreChangeLogs
                                    from t in _context.TblTyreSetups
                                    where c.TyreSetupCode == t.TyreSetupCode && c.VehicleCode == data.VehicleCode && c.TyreChangeCode == data.TyreChangeCode
                                    select (new TblTyreChangeLog
                                    {
                                        TyreChangeCode = c.TyreChangeCode,
                                        TyreNumber = t.TyreNumber,
                                        TyreSetupCode = c.TyreSetupCode,
                                        InstallationDate = c.InstallationDate,
                                        StartingKm = c.StartingKm,
                                        TyreSize = t.TyreSize,
                                        TyreBrand = t.TyreBrand,
                                        PrevVehicleCode = c.PrevVehicleCode,
                                        EndKm = c.EndKm,
                                        VehicleCode = c.VehicleCode,
                                        Status = c.Status
                                    })).ToList();
            return View(data);
        }
        public async Task<ActionResult> CreateChangeLog(TblTyreChangeLog model)
        {
            try
            {
                model.Iuser = User.Identity.Name;
                model.Idate = DateTime.Now;
                await _context.AddAsync(model);
                if (!string.IsNullOrEmpty(model.CurrentTyreCode))
                {
                    if (model.ChangeType == "Replace")
                    {
                        var dataToUpdate = _context.TblTyreChangeLogs.FirstOrDefault(x => x.TyreSetupCode == model.CurrentTyreCode && x.Status == "Active");
                        
                        dataToUpdate.Status = "Inactive";
                    }
                    else if (model.ChangeType == "Exchange") 
                    {
                        var dataToUpdate = _context.TblTyreChangeLogs.FirstOrDefault(x => x.TyreSetupCode == model.CurrentTyreCode && x.Status == "Active");
                        dataToUpdate.Status = "Inactive";

                        var preDataToUpdate = _context.TblTyreChangeLogs.FirstOrDefault(x => x.TyreSetupCode == model.TyreSetupCode && x.Status == "Spare");
                        preDataToUpdate.Status = "Exchanged";
                    }
                }
                var tyreSetup = _context.TblTyreSetups.FirstOrDefault(x => x.TyreSetupCode == model.TyreSetupCode);

                tyreSetup.Status = "Active";


                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }
        public async Task<JsonResult> GetTyreInfo(string TyreCode) {

            try
            {
                var result = _context.TblTyreSetups.FirstOrDefault(x=>x.TyreSetupCode == TyreCode);
                return Json(result);
            }
            catch (Exception ex)
            {

                return Json("");
            }
            
        }
        public async Task<JsonResult> GetTyreListByType(string ChangeType)
        {
            try
            {
                var data = await _context.TblTyreSetups.FromSqlRaw("Exec sp_GetTyreList {0}", ChangeType).ToListAsync();
                return Json(data);
            }
            catch (Exception)
            {

                return Json("");
            }
        }
        public async Task<JsonResult> GetVehicleTyreSetup(string VehicelCode)
        {
            if (!_context.TblTyreChangeLogs.Any(x => x.VehicleCode == VehicelCode))
            {
                NotFound();
                return Json("");
            }
            else {

                try
                {
                    var data = await (from c in _context.TblTyreChangeLogs
                                      from t in _context.TblTyreSetups
                                      from v in _context.TblVehicleSetups
                                      where c.TyreSetupCode == t.TyreSetupCode && c.VehicleCode == v.VehicleCode && c.VehicleCode == VehicelCode
                                      select (new TblTyreChangeLog
                                      {
                                          TyreChangeCode = c.TyreChangeCode,
                                          TyreNumber = t.TyreNumber,
                                          TyreSetupCode = c.TyreSetupCode,
                                          InstallationDate = c.InstallationDate,
                                          StartingKm = c.StartingKm ?? 0,
                                          TyreSize = t.TyreSize ?? "-",
                                          TyreBrand = t.TyreBrand ?? "-",
                                          PrevVehicleCode = c.PrevVehicleCode,
                                          PrevRegistrationNo = _context.TblVehicleSetups.FirstOrDefault(x=>x.VehicleCode == c.PrevVehicleCode).RegistrationNo ?? "-",
                                          EndKm = c.EndKm ?? 0,
                                          VehicleCode = c.VehicleCode ?? "-",
                                          Status = c.Status,
                                          RegistrationNo = v.RegistrationNo ?? "-"
                                      })).ToListAsync();
                    return Json(data);
                }
                catch (Exception ex)
                {

                    throw;
                }
                

                
            }
        }
        public string GenerateCode()
        {
            String sDate = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));
            //String dy = datevalue.Day.ToString("00");
            String mn = datevalue.Month.ToString("00");
            String yy = datevalue.Year.ToString().Substring(2, 2);
            var CodeNo = "TL-" + yy + mn;
            String maxId = "";
            String maxNo = (from c in _context.TblTyreChangeLogs.Where(c => c.TyreChangeCode.Substring(0, 7) == CodeNo).OrderByDescending(t => t.Id) select c.TyreChangeCode.Substring(7, 3)).FirstOrDefault();
            if (maxNo == null)
            {
                maxId = "001";
            }
            else
            {
                maxId = (Convert.ToInt16(maxNo) + 1).ToString("000");
            }
            CodeNo = "TL-" + yy + mn + maxId;


            return CodeNo;
        }
    }
}
