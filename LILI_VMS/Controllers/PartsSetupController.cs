using LILI_VMS;
using LILI_VMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace LILI_VMS.Controllers
{
    [Authorize]
    public class PartsSetupController : BaseController
    {
        private readonly dbVehicleManagementContext _context;

        public PartsSetupController(dbVehicleManagementContext context)
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
            var PlantId = long.Parse(HttpContext.Session.GetString("PlantId"));
            ViewData["CurrentFilter"] = searchString;
            IQueryable<LILI_VMS.Models.TblPartsSetup> model = (from p in _context.TblPartsSetups
                                                               from u in _context.TblPartsUoms
                                                               where p.Uomcode == u.Uomcode && p.PlantId == PlantId
                                                               select new TblPartsSetup
                                                               {
                                                                   Id = p.Id,
                                                                   PartsCode = p.PartsCode,
                                                                   PartsName = p.PartsName,
                                                                   NpartsName = p.NpartsName,
                                                                   Model = p.Model,
                                                                   Brand = p.Brand,
                                                                   Origin = p.Origin,
                                                                   Uomname = u.Uomname,
                                                                   Remarks = p.Remarks,
                                                               }

                                                              ) ;


            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(s => s.PartsName.Contains(searchString) || s.PartsCode.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    model = model.OrderByDescending(s => s.PartsCode);
                    break;

                default:
                    model = model.OrderByDescending(s => s.Id);
                    break;
            }
             int pageSize =  20;
            return View(await PaginatedList<TblPartsSetup>.CreateAsync(model.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public IActionResult Create()
        {  
            TblPartsSetup model = new TblPartsSetup();
            //var newGeneratedCode = GenerateToolCode();
            //List<TblPartsUom> partsUomList = new List<TblPartsUom>();
            //partsUomList = (from uom in _context.TblPartsUoms
            //                select uom).ToList();
            //partsUomList.Insert(0, new TblPartsUom { Uomcode = "0", Uomname = "Select Unit" });
            //ViewBag.ListOfUnit = partsUomList;
            model.PartsCode = GeneratePartsCode();

            var partsUomList = (from u in _context.TblPartsUoms
                                    select new
                                    {
                                        Uomcode = u.Uomcode,
                                        Uomname = u.Uomname,
                                    }).ToList();
            partsUomList.Insert(0, new { Uomcode = "", Uomname = "Select Unit" });
            ViewBag.ListOfUnit = partsUomList.ToList();

           
            TempData["msg"] = "";
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> CreatePartsSetup(TblPartsSetup data_model)
        {
            try
            {
               //TempData["msg"] = "Data Save Unsuccessful";
                //return View("Create", model);

                if (ModelState.IsValid)
                {
                    if (DoesToolCodeExists(data_model.PartsCode))
                    {
                        data_model.PartsCode = GeneratePartsCode().ToString();
                    }
                    var PlantId = long.Parse(HttpContext.Session.GetString("PlantId"));
                    // model.Uomcode = "01";
                    data_model.PlantCode = "02";
                    data_model.Iuser = User.Identity.Name;
                    data_model.Idate = DateTime.Now;
                    data_model.PlantId = PlantId;
                    _context.Add(data_model);
                    await _context.SaveChangesAsync();

                   
                }
                else 
                {
                    TempData["msg"] = "Data Save Unsuccessful";
                    return View("Create", data_model);
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Error Occured : "+ex.Message;

                var partsUomList = (from u in _context.TblPartsUoms
                                    select new
                                    {
                                        Uomcode = u.Uomcode,
                                        Uomname = u.Uomname,
                                    }).ToList();
                partsUomList.Insert(0, new { Uomcode = "", Uomname = "Select Unit" });
                ViewBag.ListOfUnit = partsUomList.ToList();
                return View("Create", data_model);

            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Update(int vId) {
            TblPartsSetup Model = new TblPartsSetup();

            var partsUomList = (from u in _context.TblPartsUoms
                                select new
                                {
                                    Uomcode = u.Uomcode,
                                    Uomname = u.Uomname,
                                }).ToList();
            partsUomList.Insert(0, new { Uomcode = "", Uomname = "Select Unit" });
            ViewBag.ListOfUnit = partsUomList.ToList();

            var result = _context.TblPartsSetups.Where(s=>s.Id== vId).First();
            Model.Id = result.Id;
            Model.PartsCode = result.PartsCode;
            Model.PartsName = result.PartsName;
            Model.Model = result.Model;
            Model.Origin = result.Origin;
            Model.Brand = result.Brand;
            Model.Uomcode = result.Uomcode;
            Model.Remarks = result.Remarks;
            Model.NpartsName = result.NpartsName;

         return View(Model);
        }
        [HttpPost]
        public async Task<IActionResult> UpdatePartsSetup(TblPartsSetup data_model) {
            var vId= data_model.Id;
            try
            {
                var toolSetupToUpdate = await _context.TblPartsSetups.FirstOrDefaultAsync(s=>s.Id==vId);
                toolSetupToUpdate.Edate= DateTime.Now;
                toolSetupToUpdate.Euser= User.Identity.Name;
                if (await TryUpdateModelAsync<TblPartsSetup>(
                    toolSetupToUpdate,
                    "",
                    s => s.PartsName,
                    s => s.Model,
                    s => s.Origin,
                    s => s.Brand,
                    s => s.Uomcode,
                    s => s.Remarks,
                    s => s.NpartsName
                    
                    )) ;
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.");
            }
            return View(data_model);
        }
        public bool Delete(int vId)
        {
            try
            {
                TblPartsSetup data_model = _context.TblPartsSetups.Where(s=> s.Id == vId).First();
                if (data_model != null)
                {
                    _context.TblPartsSetups.Remove(data_model);
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
        public string GeneratePartsCode()
        {
            //var generatedCode = "";
            //// var yearMonth = DateTime.Now.ToString("yyyyMM");
            //    var result = _context.TblPartsSetups.OrderBy(x => x.Id).Select(x => x.PartsCode).LastOrDefault();
            //    var lastGrn = string.IsNullOrEmpty(result) ? "00000" : result;


            //    var last5digits = "1";
            //    if (lastGrn.Length > 3)
            //    {
            //        last5digits = lastGrn.Substring(lastGrn.Length - 3);
            //    }

            //    int lastNumber = Int32.Parse(last5digits) + 1;
            //    string lastNumberString = lastNumber.ToString("D3");
            //    //             return $"{companyCode}{plantCode}gr{yearMonth}{lastNumberString}";
            //    generatedCode = $"P-{lastNumberString}";

            //Generate Requisition No.---------Start
            String sDate = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));
            //String dy = datevalue.Day.ToString("00");
            String mn = datevalue.Month.ToString("00");
            String yy = datevalue.Year.ToString().Substring(2, 2);
            var CodeNo = "PA-" + yy + mn;
            String maxId = "";
            String maxNo = (from c in _context.TblPartsSetups.Where(c => c.PartsCode.Substring(0, 7) == CodeNo).OrderByDescending(t => t.Id) select c.PartsCode.Substring(7, 3)).FirstOrDefault();
            if (maxNo == null)
            {
                maxId = "001";
            }
            else
            {
                maxId = (Convert.ToInt16(maxNo) + 1).ToString("000");
            }
            CodeNo = "PA-" + yy + mn + maxId;


            return CodeNo;
        }
        public bool DoesToolCodeExists(string vToolCode)
        {

            return _context.TblPartsSetups.Any(e => e.PartsCode == vToolCode);
        }
        #endregion
    }
}
