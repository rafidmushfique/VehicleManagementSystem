using LILI_VMS.Models;
using LILI_VMS.Models.ReportsViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace LILI_VMS.Controllers
{
    [Authorize]
    public class BillNoList
    { 
      public string BillNo { get; set; }
      public string BillName { get; set; }
    }
    public class RegNoList
    {
        public string RegNo { get; set; }
        public string VehicleName { get; set; }
    }
    public class ReportController : BaseController
    {
        private readonly dbVehicleManagementContext _context;
        public ReportController(dbVehicleManagementContext context)
        {
                _context = context;
        }
        public ActionResult VehicleMovementBill(string BillNo = "")
        {

            var BillNoList = (from c in _context.TblGpbills
                             select new BillNoList()
                             {
                                 BillNo = c.BillNo,
                                 BillName = c.BillNo
                             }).ToList();
            BillNoList.Insert(0, new BillNoList { BillNo = "*", BillName = "Select Bill" });

            ViewBag.ListOfBillNo = BillNoList.ToList();
            var BillNoParameter = (BillNo == null) ? "" : BillNo;

            var model = _context.VehicleMovementBillReport.FromSqlRaw("Exec rsp_ReportVehicleManagementBill {0}", BillNoParameter).AsEnumerable().Select(
                 c => new VehicleMovementBillReport 
                 {
                     Id = c.Id,
                     LoadingDate = c.LoadingDate,
                     KPL = c.KPL,
                     RegNo = c.RegNo,
                     BillDate = c.BillDate,
                     Weight = c.Weight,
                     DriverName = c.DriverName,
                     ClosingKM = c.ClosingKM,
                     RunOfKm = c.RunOfKm,
                     StartingKm = c.StartingKm,
                     TotalLoadMt = c.TotalLoadMt,
                     Customer = c.Customer,
                     CustomerAddress = c.CustomerAddress,
                     ExpenseName = c.ExpenseName,
                     Unit = c.Unit,
                     TotalPrice = c.TotalPrice,
                     Comments = c.Comments

                 }
                );
            return View(model);

        }
        public ActionResult VehicleWiseExpenditure(string VehicleOwner = "", string VehicleRegNo = "", string Month = "", string DateFrom ="", string DateTo= "")
        {

            List<SelectListItem> monthList = new List<SelectListItem>();
            monthList.Add(new SelectListItem { Value = "<--Select Month-->", Text = "00" });
            monthList.Add(new SelectListItem { Value = "January", Text = "01" });
            monthList.Add(new SelectListItem { Value = "February", Text = "02" });
            monthList.Add(new SelectListItem { Value = "March", Text = "03" });
            monthList.Add(new SelectListItem { Value = "April", Text = "04" });
            monthList.Add(new SelectListItem { Value = "May", Text = "05" });
            monthList.Add(new SelectListItem { Value = "June", Text = "06" });
            monthList.Add(new SelectListItem { Value = "July", Text = "07" });
            monthList.Add(new SelectListItem { Value = "August", Text = "08" });
            monthList.Add(new SelectListItem { Value = "September", Text = "09" });
            monthList.Add(new SelectListItem { Value = "October", Text = "10" });
            monthList.Add(new SelectListItem { Value = "November", Text = "11" });
            monthList.Add(new SelectListItem { Value = "December", Text = "12" });
            ViewBag.ListOfMonth = monthList;

            var PlantId = long.Parse(HttpContext.Session.GetString("PlantId"));

            var regnolist = (from c in _context.TblVehicleSetups
                             where c.PlantId == PlantId
                             select new RegNoList{ 
                                RegNo = c.RegistrationNo,
                                VehicleName = c.RegistrationNo
                            }).ToList();

            regnolist.Insert(0, new RegNoList { RegNo = "*", VehicleName = "Select Vehicle" });
            ViewBag.ListOfRegNo = regnolist;

            var ownerList = (from c in _context.TblVehicleOwners
                             select new TblVehicleOwner
                             {
                                 OwnerCode = c.OwnerCode,
                                 OwnerName = c.OwnerName
                             }).ToList();
            ownerList.Insert(0, new TblVehicleOwner { OwnerCode = "*", OwnerName = "Select Owner" });
            ViewBag.ListOfOwner = ownerList;

            List<VehicleWiseExpenditureReport> model = new List<VehicleWiseExpenditureReport>();
            DateFrom = DateFrom ?? "";
            DateTo = DateTo ?? "";
            try
            {
                model = _context.VehicleWiseExpenditureReport.FromSqlRaw("Exec sp_VehicleExpenditureReport {0},{1},{2},{3},{4},{5}", VehicleOwner, VehicleRegNo, Month, DateFrom, DateTo,PlantId).ToList();
            }
            catch (Exception ex)
            {

                throw;
            }
            
            
            
            return View(model); 
        }
        public ActionResult VendorPerformance(string Vendor="",string Unit = "", string Month="", string DateFrom="",string DateTo="")
        {

            return View();
        }
        public JsonResult GetVehicleNoList(string vOwnerCode)
        {

            var regnolist = (from c in _context.TblVehicleSetups
                             where c.Owner == vOwnerCode
                             select new RegNoList
                             {
                                 RegNo = c.RegistrationNo,
                                 VehicleName = c.RegistrationNo
                             }).Distinct().ToList();

           return Json(regnolist);
           
        }
    }
}
