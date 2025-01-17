using LILI_VMS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace LILI_VMS.Controllers
{
    public class ExpenseHeadSetupController : BaseController
    {
        private readonly dbVehicleManagementContext _context;

        public ExpenseHeadSetupController(dbVehicleManagementContext context)
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
            var PlantId = long.Parse(HttpContext.Session.GetString("PlantId"));
            IQueryable<LILI_VMS.Models.TblExpenseHeadSetup> model = _context.TblExpenseHeadSetups.Where(x=>x.PlantId == PlantId);


            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(s => s.ExpenseName.Contains(searchString) || s.ExpenseCode.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    model = model.OrderByDescending(s => s.ExpenseCode);
                    break;

                default:
                    model = model.OrderBy(s => s.Idate);
                    break;
            }
             int pageSize =  20;
            return View(await PaginatedList<TblExpenseHeadSetup>.CreateAsync(model.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public IActionResult Create()
        {
            TblExpenseHeadSetup model = new TblExpenseHeadSetup();
            model.ExpenseCode = GenerateExpenseCode();

           
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateExpenseSetup(TblExpenseHeadSetup model)
        {
            try
            {
                //TempData["msg"] = "Data Save Unsuccessful";
                //return View("Create", model);

                if (ModelState.IsValid)
                {
                    if (DoesToolCodeExists(model.ExpenseCode))
                    {
                        model.ExpenseCode = GenerateExpenseCode();
                    }
                    var UnitCode = HttpContext.Session.GetString("UnitCode");
                    var PlantId = long.Parse(HttpContext.Session.GetString("PlantId"));

                    model.Iuser = User.Identity.Name;
                    model.Idate = DateTime.Now;
                    model.UnitCode = UnitCode;
                    model.PlantId = PlantId;
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

            TblExpenseHeadSetup model = new TblExpenseHeadSetup();
            model = _context.TblExpenseHeadSetups.Where(x => x.Id == vId).FirstOrDefault();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateExpenseSetup(TblExpenseHeadSetup model)
        {
            var vId = model.Id;
            try
            {
                var ExpenseSetupToUpdate = await _context.TblExpenseHeadSetups.FirstOrDefaultAsync(s => s.Id == vId);
                ExpenseSetupToUpdate.Edate = DateTime.Now;
                ExpenseSetupToUpdate.Euser = User.Identity.Name;
                if (await TryUpdateModelAsync<TblExpenseHeadSetup>(
                    ExpenseSetupToUpdate,
                    "",
                    s => s.ExpenseName,
                    s => s.Unit,
                    s => s.Rate,
                    s => s.Comments

                    )) ;
                await _context.SaveChangesAsync();

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
        public bool Delete(int vId)
        {
            try
            {
                TblExpenseHeadSetup model = _context.TblExpenseHeadSetups.Where(s => s.Id == vId).First();
                if (model != null)
                {
                    _context.TblExpenseHeadSetups.Remove(model);
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

        #region private classes
        public string GenerateExpenseCode()
        {
            //Generate Requisition No.---------Start
            String sDate = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));
            //String dy = datevalue.Day.ToString("00");
            String mn = datevalue.Month.ToString("00");
            String yy = datevalue.Year.ToString().Substring(2, 2);
            var ECNo = "EC-" + yy + mn;
            String maxId = "";
            String maxNo = (from c in _context.TblExpenseHeadSetups.Where(c => c.ExpenseCode.Substring(0, 7) == ECNo).OrderByDescending(t => t.Id) select c.ExpenseCode.Substring(7, 3)).FirstOrDefault();
            if (maxNo == null)
            {
                maxId = "001";
            }
            else
            {
                maxId = (Convert.ToInt16(maxNo) + 1).ToString("000");
            }
            ECNo = "EC-" + yy + mn + maxId;

            return ECNo; 

        }
        public bool DoesToolCodeExists(string vCode)
        {

            return _context.TblExpenseHeadSetups.Any(e => e.ExpenseCode == vCode);
        }
        #endregion
    }
}

