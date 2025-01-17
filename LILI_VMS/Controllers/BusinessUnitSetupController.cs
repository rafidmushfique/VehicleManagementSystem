using LILI_VMS;
using LILI_VMS.Controllers;
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


namespace LILI_TTS.Controllers
{
    [Authorize]
    public class BusinessUnitSetupController : BaseController
    {
        private readonly dbVehicleManagementContext _context;

        public BusinessUnitSetupController(dbVehicleManagementContext context)
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
            IQueryable<LILI_VMS.Models.TblBusinessUnitSetupInfo> model = _context.TblBusinessUnitSetupInfos.Where(x=>x.PlantId == PlantId);


            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(s => s.BusinessName.Contains(searchString) || s.BusinessCode.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    model = model.OrderByDescending(s => s.BusinessCode);
                    break;

                default:
                    model = model.OrderByDescending(s => s.Id).ThenBy(x=>x.BusinessCode);
                    break;
            }
             int pageSize =  20;
            return View(await PaginatedList<TblBusinessUnitSetupInfo>.CreateAsync(model.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public async Task<IActionResult> Create()
        {  
            TblBusinessUnitSetupInfo model = new TblBusinessUnitSetupInfo();
            //var newGeneratedCode = GenerateToolCode();
            var PlantParameter = "";
            var plantList = await _context.PlantVm.FromSqlRaw("EXEC sp_GetAllPlants {0}", PlantParameter).ToListAsync();
            plantList.Insert(0, new PlantVM { PlantCode="*",PlantName="Select Plant"  });
            ViewBag.ListOfPlants = plantList;
            model.Type = "Business";
            model.BusinessCode = GenerateBusinessCode(model.Type.ToString());
            TempData["msg"] = "";
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateBusinessSetup(TblBusinessUnitSetupInfo model)
        {
            try
            {
                var UnitCode = HttpContext.Session.GetString("UnitCode");
                //TempData["msg"] = "Data Save Unsuccessful";
                //return View("Create", model);

                var PlantId = long.Parse(HttpContext.Session.GetString("PlantId"));
                if (DoesToolCodeExists(model.BusinessCode))
                    {
                        model.BusinessCode = GenerateBusinessCode(model.Type.ToString()).ToString();
                    }
                    
                    model.Iuser = User.Identity.Name;
                    model.Idate = DateTime.Now;
                    model.PlantId = PlantId;
                    _context.Add(model);
                    await _context.SaveChangesAsync();
                  
                
              
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Error Occured : "+ex.Message;
                return View("Create", model);

            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Update(int vId) {
            TblBusinessUnitSetupInfo businessModel = new TblBusinessUnitSetupInfo();
            var result = _context.TblBusinessUnitSetupInfos.Where(s=>s.Id== vId).First();
            businessModel.Id = result.Id;
            businessModel.Type = result.Type;
            businessModel.BusinessCode = result.BusinessCode;
            businessModel.BusinessName = result.BusinessName;
            businessModel.Address = result.Address;
            businessModel.Description = result.Description;
            businessModel.PlantId = result.PlantId;
            var PlantParameter = "";
            var plantList =  _context.PlantVm.FromSqlRaw("EXEC sp_GetAllPlants {0}", PlantParameter).ToList();

            plantList.Insert(0, new PlantVM { PlantCode = "*", PlantName = "Select Plant" });
            ViewBag.ListOfPlants = plantList;

            return View(businessModel);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateBusinessSetup(TblBusinessUnitSetupInfo model) {
            var vId=   model.Id;
            try
            {
                var toolSetupToUpdate = await _context.TblBusinessUnitSetupInfos.FirstOrDefaultAsync(s=>s.Id==vId);
                toolSetupToUpdate.Edate= DateTime.Now;
                toolSetupToUpdate.Euser= User.Identity.Name;
                if (await TryUpdateModelAsync<TblBusinessUnitSetupInfo>(
                    toolSetupToUpdate,
                    "",
                    s => s.Type,
                    s => s.BusinessName,
                    s => s.Address,
                    s => s.Description
                    
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
            return View(model);
        }
        public bool Delete(int vId)
        {
            try
            {
                TblBusinessUnitSetupInfo model= _context.TblBusinessUnitSetupInfos.Where(s=> s.Id == vId).First();
                if (model != null)
                {
                    _context.TblBusinessUnitSetupInfos.Remove(model);
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
        public string GenerateBusinessCode(string busUnit)
        {
            var generatedCode = "";
            // var yearMonth = DateTime.Now.ToString("yyyyMM");
            if (busUnit == "Business")
            {
                var result = _context.TblBusinessUnitSetupInfos.Where(x => x.Type == "Business").OrderBy(x => x.Id).Select(x => x.BusinessCode).LastOrDefault();
                var lastGrn = string.IsNullOrEmpty(result) ? "00000" : result;


                var last5digits = "1";
                if (lastGrn.Length > 3)
                {
                    last5digits = lastGrn.Substring(lastGrn.Length - 3);
                }

                int lastNumber = Int32.Parse(last5digits) + 1;
                string lastNumberString = lastNumber.ToString("D3");
                //             return $"{companyCode}{plantCode}gr{yearMonth}{lastNumberString}";
                generatedCode = $"B-{lastNumberString}";
            }
            else
            {
                var result = _context.TblBusinessUnitSetupInfos.Where(x => x.Type == "Unit").OrderBy(x => x.Id).Select(x => x.BusinessCode).LastOrDefault();
                var lastGrn = string.IsNullOrEmpty(result) ? "00000" : result;


                var last5digits = "1";
                if (lastGrn.Length > 3)
                {
                    last5digits = lastGrn.Substring(lastGrn.Length - 3);
                }

                int lastNumber = Int32.Parse(last5digits) + 1;
                string lastNumberString = lastNumber.ToString("D3");
                //             return $"{companyCode}{plantCode}gr{yearMonth}{lastNumberString}";
                generatedCode = $"U-{lastNumberString}";
            }
          
            return generatedCode;
        }
        public bool DoesToolCodeExists(string vToolCode)
        {

            return _context.TblBusinessUnitSetupInfos.Any(e => e.BusinessCode == vToolCode);
        }
        public async Task<JsonResult> GetPlantInfo(string vPlantCode) {
            
            var plantList = await _context.PlantVm.FromSqlRaw("EXEC sp_GetAllPlants {0}", vPlantCode).ToListAsync();
            return Json(plantList);
        }
        #endregion
    }
}
