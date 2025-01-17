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
    public class TyreSetupController : BaseController
    {
        private readonly dbVehicleManagementContext _context;

        public TyreSetupController(dbVehicleManagementContext context)
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
            IQueryable<TblTyreSetup> model = _context.TblTyreSetups.Where(x=>x.PlantId == PlantId);
           



            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(s => s.TyreSetupCode.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    model = model.OrderByDescending(s => s.TyreSetupCode);
                    break;

                default:
                    model = model.OrderByDescending(s => s.Id);
                    break;
            }
             int pageSize =  20;
            return View(await PaginatedList<TblTyreSetup>.CreateAsync(model.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
        public async Task<IActionResult> Create()
        {
            //var vehicleList = _context.TblVehicleSetups.ToList();
            //ViewBag.ListOfVehicle = vehicleList;
            TblTyreSetup model = new TblTyreSetup();
            model.TyreSetupCode = GenerateCode();
            return View(model);
        }
        public async Task<ActionResult> CreateTyreSetup(TblTyreSetup model)
        {
            try
            {
                var UnitCode = HttpContext.Session.GetString("UnitCode");
                var PlantId = long.Parse(HttpContext.Session.GetString("PlantId"));
                model.Idate = DateTime.Now;
                model.Iuser = User.Identity.Name;
                model.Status = "New";
                model.UnitCode = UnitCode;
                model.PlantId = PlantId;
                await _context.AddAsync(model);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                return View("Create", model);
            }

        }
        public async Task<IActionResult> Update(int id)
        {
            if (!_context.TblTyreSetups.Any(x => x.Id == id))
            {
                NotFound();
                return RedirectToAction(nameof(Index));
            }
            else 
            {

                TblTyreSetup data = _context.TblTyreSetups.FirstOrDefault(x => x.Id == id);
                return View(data);
            }
        }
        public async Task<ActionResult> UpdateTyreSetup(int id, TblTyreSetup model) 
        {
          var modelToUpdate = _context.TblTyreSetups.FirstOrDefault(x => x.Id == id);
            modelToUpdate.Edate = DateTime.Now;
            modelToUpdate.Euser = User.Identity.Name;
            if (await TryUpdateModelAsync<TblTyreSetup>(
                    modelToUpdate,
                    "",
                    s => s.TyreNumber,
                    s => s.Status,
                    s => s.TyreBrand,
                    s=> s.Comments
                    )) ;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public bool Delete(int id)
        {
            try
            {
                if (!_context.TblTyreSetups.Any(x => x.Id == id))
                {
                    return false;
                }
                else 
                {
                   
                    TblTyreSetup model =  _context.TblTyreSetups.First(x => x.Id == id);
                    if (_context.TblTyreChangeLogs.Any(x => x.TyreSetupCode == model.TyreSetupCode))
                    {
                        return false;
                    }
                    else
                    {
                        _context.TblTyreSetups.Remove(model);
                        _context.SaveChanges();

                        return true;
                    }
                }

                
            }
            catch (Exception)
            {

                return false;
            }
           
           

        }
        public string GenerateCode() 
        {
            String sDate = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));
            //String dy = datevalue.Day.ToString("00");
            String mn = datevalue.Month.ToString("00");
            String yy = datevalue.Year.ToString().Substring(2, 2);
            var CodeNo = "TS-" + yy + mn;
            String maxId = "";
            String maxNo = (from c in _context.TblTyreSetups.Where(c => c.TyreSetupCode.Substring(0, 7) == CodeNo).OrderByDescending(t => t.Id) select c.TyreSetupCode.Substring(7, 3)).FirstOrDefault();
            if (maxNo == null)
            {
                maxId = "001";
            }
            else
            {
                maxId = (Convert.ToInt16(maxNo) + 1).ToString("000");
            }
            CodeNo = "TS-" + yy + mn + maxId;


            return CodeNo;
        }
    }
}
