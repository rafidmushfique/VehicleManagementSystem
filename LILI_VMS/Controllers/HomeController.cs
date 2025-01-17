using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LILI_VMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NPOI.Util;
using System.Drawing;

namespace LILI_VMS.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly dbVehicleManagementContext _context;
        public HomeController(dbVehicleManagementContext context)
        {
                _context = context;
        }
        public IActionResult Index()
        {
            var userName = User.Identity.Name;
            var PlantId = "0";
            if (_context.TblUserWiseUnitAndPlantCodes.Any(x => x.UserId == userName))
            {
                 PlantId = (_context.TblUserWiseUnitAndPlantCodes.FirstOrDefault(x => x.UserId == userName)?.PlantId ?? 0).ToString();
                HttpContext.Session.SetString("PlantId", PlantId);
            }
            var isVendorUser = false;
            if (_context.TblUserWiseVendorCodes.Any(x => x.UserId == userName))
            {
                isVendorUser = true;
            }
        
            var parsePlant = long.Parse(PlantId);
            var UnitList = _context.TblBusinessUnitSetupInfos.FromSqlRaw("EXEC sp_GetUserWiseUnitList {0},{1}", userName, parsePlant);

            ViewBag.UnitList = UnitList;

            ViewBag.VendorUser = isVendorUser;
            ViewBag.SessionData = HttpContext.Session.GetString("UnitCode");
                
            return View();
        }
        [HttpPost]
        public IActionResult ChangeUnit()
        {
            HttpContext.Session.Clear();
            return Json(new { redirectUrl = Url.Action("Index", "Home") });

        }
         public ActionResult SetSession(string UnitCode, string UnitName)
        {
            var data = "failed";
            var user = User.Identity.Name;
            var PlantId = _context.TblUserWiseUnitAndPlantCodes.FirstOrDefault(x => x.UserId == user)?.PlantId;

            var UnitCount = _context.TblBusinessUnitSetupInfos.Count(x => x.PlantId == PlantId);
            if (!String.IsNullOrEmpty(UnitCode))
            {
                //var activePlantName = _bmsCommonService.BMSUnit.PlantRepository.GetAll().Where(c=>c.Id == Convert.ToInt64(PlantId)).FirstOrDefault().PlantName;
                //var userId= HttpContext.User.Identity.Name;
                //var activeUserName = _bmsCommonService.BMSUnit.UserProfileRepository.Fetch().Where(c => c.UserId == userId).FirstOrDefault().UserName;
                //Session["ActivePlantName"] = activePlantName;
                //Session["ActiveUserName"] = activeUserName;
                //Session["PlantId"] = PlantId;
                //Session["CompanyId"] = "1";
                //data = "success";



                HttpContext.Session.SetString("UnitCode", UnitCode);
                HttpContext.Session.SetString("UserName", user);
                HttpContext.Session.SetString("UnitName", UnitName);
                data = "success";
            }
            else if (User.Identity.Name == "admin@yahoo.com" && UnitCount == 0)
            {
                HttpContext.Session.SetString("UnitCode", "0");
                HttpContext.Session.SetString("UserName", User.Identity.Name);
                HttpContext.Session.SetString("UnitName", "Admin");
                data = "success";
            }
            else if (User.Identity.Name == "TestAdmin@yahoo.com" && UnitCount == 0) 
            { 
                HttpContext.Session.SetString("UnitCode", "0");
                HttpContext.Session.SetString("UserName", User.Identity.Name);
                HttpContext.Session.SetString("UnitName", "Admin");
                data = "success";
            }
            return Json(data);
        }


        public IActionResult Login()
        {
            return RedirectToAction("LogIn", "Account");
        }
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
