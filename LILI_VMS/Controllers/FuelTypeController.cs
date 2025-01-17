using LILI_VMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace LILI_VMS.Controllers
{
    public class FuelTypeController : BaseController
    {
        private readonly dbVehicleManagementContext context;

        public FuelTypeController(dbVehicleManagementContext context)
        {
            this.context = context;
        }
        public IActionResult FuelSetup()
        {
            TblFuelType model = new TblFuelType();
            model.FuelTypeCode = GenerateCode();
            var FueTypelList = context.TblFuelTypes.ToList();
            ViewBag.FueTypelList = FueTypelList;
            return View(model);
        }
        public ActionResult AddFuelType(TblFuelType model)
        {
         context.Add(model);
         context.SaveChanges();

         return RedirectToAction(nameof(FuelSetup));

        }

        public string GenerateCode()
        {
            String sDate = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));
            //String dy = datevalue.Day.ToString("00");
            String mn = datevalue.Month.ToString("00");
            String yy = datevalue.Year.ToString().Substring(2, 2);
            var CodeNo = "FT-" + yy + mn;
            String maxId = "";
            String maxNo = (from c in context.TblFuelTypes.Where(c => c.FuelTypeCode.Substring(0, 7) == CodeNo).OrderByDescending(t => t.Id) select c.FuelTypeCode.Substring(7, 3)).FirstOrDefault();
            if (maxNo == null)
            {
                maxId = "001";
            }
            else
            {
                maxId = (Convert.ToInt16(maxNo) + 1).ToString("000");
            }
            CodeNo = "FT-" + yy + mn + maxId;


            return CodeNo;
        }
    }
}
