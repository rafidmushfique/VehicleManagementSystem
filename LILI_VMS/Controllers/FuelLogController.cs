using LILI_VMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace LILI_VMS.Controllers
{
    public class FuelLogController : BaseController
    {
        private readonly dbVehicleManagementContext _context;
        public FuelLogController(dbVehicleManagementContext context)
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
            IQueryable<LILI_VMS.Models.TblFuelLog> model = from c in _context.TblFuelLogs
                                                           from v in _context.TblVehicleSetups
                                                           where c.VehicleCode == v.VehicleCode && c.PlantId == PlantId
                                                           select new TblFuelLog
                                                           {
                                                               Id = c.Id,
                                                               FuelLogNo = c.FuelLogNo,
                                                               FuelLogDate = c.FuelLogDate,
                                                               Comments = c.Comments,
                                                               RegistrationNo = v.RegistrationNo,
                                                           };


            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(s => s.FuelLogNo.Contains(searchString) || s.RegistrationNo.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    model = model.OrderByDescending(s => s.FuelLogNo);
                    break;

                default:
                    model = model.OrderBy(s => s.FuelLogDate);
                    break;
            }
             int pageSize =  20;
            return View(await PaginatedList<TblFuelLog>.CreateAsync(model.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public IActionResult Create() 
        {
            TblFuelLog model = new TblFuelLog();
            model.FuelLogNo = GenerateCode();
            model.FuelLogDate = DateTime.Now;

            PrepareViewData();

            return View(model);
        }
        public ActionResult CreateFuelLog(TblFuelLog model) 
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var PlantId = long.Parse(HttpContext.Session.GetString("PlantId"));
                    model.Iuser = User.Identity.Name;
                    model.Idate = DateTime.Now;
                    model.PlantId = PlantId;
                    _context.Add(model);
                    _context.SaveChanges();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {

                    ViewBag.ErrorMessage = "An error occurred while processing your request. Please try again.";

                }
              
            }
         

                model.FuelLogNo = GenerateCode();
                model.FuelLogDate = DateTime.Now;

            PrepareViewData();

            return View("Create",model);
            
            
        }

        public IActionResult Update(int Id)
        {

            if (_context.TblFuelLogs.Any(x => x.Id == Id))
            {
                TblFuelLog model = _context.TblFuelLogs.FirstOrDefault(x=>x.Id== Id);

                model.TblFuelLogDetails = (from c in _context.TblFuelLogDetails
                                          from f in _context.TblFuelTypes
                                          where c.FuelTypeCode == f.FuelTypeCode && c.FuelLogNo == model.FuelLogNo
                                          select new TblFuelLogDetail
                                          {
                                              Id = c.Id,
                                              FuelLogNo = c.FuelLogNo,
                                              FuelStationName = c.FuelStationName,
                                              Quantity = c.Quantity,
                                              Rate = c.Rate,
                                              Amount = c.Amount,
                                              FuelTypeCode = c.FuelTypeCode,
                                              FuelTypeName = f.FuelTypeName

                                          }).ToList();

                PrepareViewData();
                return View(model);
            }
            else {

                ViewBag.ErrorMessage = "An error occurred while processing your request. Please try again.";
                return RedirectToAction(nameof(Index));
            }

        }
        public async Task<ActionResult> UpdateFuelLog(TblFuelLog model)
        {
            try
            {
                TblFuelLog modelToUpdate = _context.TblFuelLogs.FirstOrDefault(x=>x.Id== model.Id);
                model.Euser = User.Identity.Name;
                model.Edate = DateTime.Now;

                if (await TryUpdateModelAsync<TblFuelLog>(
                      modelToUpdate,
                      "",
                      s => s.Comments

                   )) ;

                if (_context.TblFuelLogDetails.Any(x => x.FuelLogNo == model.FuelLogNo))
                {
                    _context.TblFuelLogDetails.RemoveRange(_context.TblFuelLogDetails.Where(x=>x.FuelLogNo== model.FuelLogNo));
                }
                
                List<TblFuelLogDetail> fuelLogDetal = new List<TblFuelLogDetail>();
                foreach (var item in model.TblFuelLogDetails)
                {
                    TblFuelLogDetail fuelItem = new TblFuelLogDetail();
                    fuelItem.FuelLogNo = item.FuelLogNo;
                    fuelItem.FuelStationName = item.FuelStationName;
                    fuelItem.Quantity = item.Quantity;
                    fuelItem.Rate = item.Rate;
                    fuelItem.Amount = item.Amount;
                    fuelItem.FuelTypeCode = item.FuelTypeCode;
                    fuelLogDetal.Add(fuelItem);
                }
                _context.TblFuelLogDetails.AddRange(fuelLogDetal);  


                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
               

                ViewBag.ErrorMessage = "An error occurred while processing your request. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        public bool Delete(int Id) 
        {
            if (_context.TblFuelLogs.Any(x => x.Id == Id))
            {
                try
                {
                    var model = _context.TblFuelLogs.FirstOrDefault(x => x.Id == Id);
                    if (model != null) 
                    {
                        if (_context.TblFuelLogDetails.Any(x => x.FuelLogNo == model.FuelLogNo))
                        {
                            _context.RemoveRange(_context.TblFuelLogDetails.Where(x => x.FuelLogNo == model.FuelLogNo));
                        }

                        _context.Remove(model);
                        return true;
                    }
                    else
                    {

                        return false;
                    }
                   
                    
                }
                catch (Exception ex)
                {

                    return false;
                }

            }
            else { 
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
            var CodeNo = "FL-" + yy + mn;
            String maxId = "";
            String maxNo = (from c in _context.TblFuelLogs.Where(c => c.FuelLogNo.Substring(0, 7) == CodeNo).OrderByDescending(t => t.Id) select c.FuelLogNo.Substring(7, 3)).FirstOrDefault();
            if (maxNo == null)
            {
                maxId = "001";
            }
            else
            {
                maxId = (Convert.ToInt16(maxNo) + 1).ToString("000");
            }
            CodeNo = "FL-" + yy + mn + maxId;


            return CodeNo;
        }


        private void PrepareViewData() {

            var PlantId = long.Parse(HttpContext.Session.GetString("PlantId"));
            var VehicleList = _context.TblVehicleSetups.Where(x=>x.PlantId == PlantId).ToList();
            ViewBag.VehicleList = VehicleList;
            
            var FuelType = _context.TblFuelTypes.ToList();
            ViewBag.FuelType = FuelType;
        }
    }
}
