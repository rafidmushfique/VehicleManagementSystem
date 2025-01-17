using LILI_VMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.EntityFrameworkCore;
using NPOI.POIFS.Crypt.Dsig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace LILI_VMS.Controllers
{
    public class VendorSmsController : BaseController
    {
        private readonly dbVehicleManagementContext _context;

        public VendorSmsController(dbVehicleManagementContext context)
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

            //var VendorCodeTemp = _context.TblUserWiseUnitAndPlantCodes.FirstOrDefault(x => x.UserId == User.Identity.Name).UnitCode;


            var PlantId = long.Parse(HttpContext.Session.GetString("PlantId"));
            var req = from c in _context.TblVendorSmsmasters
                      where c.PlantId == PlantId
                      select c;


            if (!String.IsNullOrEmpty(searchString))
            {
                req = req.Where(s => s.VendorSmsCode.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    req = req.OrderByDescending(s => s.VendorSmsCode);
                    break;

                default:
                    req = req.OrderByDescending(s => s.VendorSmsCode);
                    break;
            }
             int pageSize =  20;
            return View(await PaginatedList<TblVendorSmsmaster>.CreateAsync(req.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public async Task<IActionResult> Create()
        { 
            TblVendorSmsmaster  model = new TblVendorSmsmaster();
            model.VendorSmsCode = GeneratedCode();
            if (User.Identity.Name != "admin@yahoo.com")
            {
                model.UnitCode = _context.TblUserWiseUnitAndPlantCodes.FirstOrDefault(x => x.UserId == User.Identity.Name)?.UnitCode;
            }
            else {
                model.UnitCode = "";
            }
            
            return View(model);
        }

        public async Task<IActionResult> Update(int Id)
        {
            TblVendorSmsmaster model = new TblVendorSmsmaster();
            model = _context.TblVendorSmsmasters.FirstOrDefault(x => x.Id == Id);
            model.TblVendorSmsdetails = _context.TblVendorSmsdetails.Where(x=>x.VendorSmsCode== model.VendorSmsCode).ToList();
            return View(model);
        }
        public async Task<ActionResult> CreateVenorNotification(TblVendorSmsmaster model)
        {
            model.Iuser = User.Identity.Name;
            model.Idate = DateTime.Now;
            try
            {
                if (model.TblVendorSmsdetails.Count > 0)
                {
                    await _context.AddAsync(model);
                    await _context.SaveChangesAsync();
                }
                
            }
            catch (Exception ex)
            {

                return RedirectToAction("Create");
            }


            return RedirectToAction(nameof(Index));
        }
        public async Task<JsonResult> GetVendorListForSms(string Date)
        {
            try
            {
                var data = _context.VMVendorListForSMS.FromSqlRaw("Exec sp_GetVendorListForSMS {0}", Date).ToList();
                return Json(data);

            }
            catch (Exception ex)
            {

                return Json("");
            }
            

            
        }
        public string GeneratedCode() {
            //Generate Requisition No.---------Start
            String sDate = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));
            //String dy = datevalue.Day.ToString("00");
            String mn = datevalue.Month.ToString("00");
            String yy = datevalue.Year.ToString().Substring(2, 2);
            var CodeNo = "VN-" + yy + mn;
            String maxId = "";
            String maxNo = (from c in _context.TblVendorSmsmasters.Where(c => c.VendorSmsCode.Substring(0, 7) == CodeNo).OrderByDescending(t => t.Id) select c.VendorSmsCode.Substring(7, 3)).FirstOrDefault();
            if (maxNo == null)
            {
                maxId = "001";
            }
            else
            {
                maxId = (Convert.ToInt16(maxNo) + 1).ToString("000");
            }
            CodeNo = "VN-" + yy + mn + maxId;

            return CodeNo;
        }
    }
}
