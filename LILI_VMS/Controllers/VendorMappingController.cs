using Humanizer;
using LILI_VMS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using Org.BouncyCastle.Math.EC.Rfc7748;

namespace LILI_VMS.Controllers
{
    [Authorize]
    public class VendorMappingController : BaseController
    {
        private readonly dbVehicleManagementContext _context;

        public VendorMappingController(dbVehicleManagementContext context)
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
            var req = from c in _context.TblUserWiseVendorCodes
                      select c;



            if (!String.IsNullOrEmpty(searchString))
            {
                req = req.Where(s => s.VendorName.Contains(searchString)|| s.VendorCode.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    req = req.OrderByDescending(s => s.VendorName);
                    break;

                default:
                    req = req.OrderBy(x=>x.Id);
                    break;
            }
             int pageSize =  20;
            return View(await PaginatedList<TblUserWiseVendorCode>.CreateAsync(req.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public async Task<IActionResult> Create()
        {
            var userList = _context.AspNetUsers.ToList();

            ViewBag.UserList = userList;
            return View();
        }

        public async Task<ActionResult> CreateVendorMapping(VendorMappingSaveVM model)
        {
            try
            {
             var userName = User.Identity.Name;
             List <TblUserWiseVendorCode> newModel = new List <TblUserWiseVendorCode>();
             newModel = model.TblUserWiseVendorCode;
             var newVendorMappings = newModel
            .Where(m => !_context.TblUserWiseVendorCodes.Any(v => v.VendorCode == m.VendorCode && v.UserId == userName))
            .ToList();

                if (newVendorMappings.Any())
                {
                    await _context.AddRangeAsync(newVendorMappings);
                    await _context.SaveChangesAsync();


                }
                if (!string.IsNullOrEmpty(newModel.FirstOrDefault().UserId))
                {
                    var result = ActivateVendorUser(newModel.FirstOrDefault().UserId);
                }
               
            }
            catch (Exception ex)
            {

                throw;
            }
            return RedirectToAction(nameof(Create));
        }

        public ActionResult VendorModal()
        {
            List<VMVendorInfo> vendor = new List<VMVendorInfo>();
            return PartialView("SearchVendorModal", vendor);
        }
        public JsonResult SearchVendor(string searchKey)
        {

            var SearchParameter = (searchKey == null) ? "" : searchKey;
            var FoodPlantId = "";
            if (User.Identity.Name != "admin@yahoo.com")
            {
                var UnitCode = _context.TblUserWiseUnitAndPlantCodes.FirstOrDefault(x => x.UserId == User.Identity.Name).UnitCode;
                FoodPlantId = _context.TblBusinessUnitSetupInfos.FirstOrDefault(x => x.BusinessCode == UnitCode).FgPlantId;
            }


            var model = _context.VMVendorInfo.FromSqlRaw("Exec sp_SearchBidVendor {0},{1}", SearchParameter, FoodPlantId).ToList();
            return Json(model);
        }

        public JsonResult GetUserVendorMapInfo(string UserId)
        {
            try
            {
                var data = _context.TblUserWiseVendorCodes.Where(x => x.UserId == UserId).ToList();
                //return Json(data);
                return Json(data);
            }
            catch (Exception ex)
            {

                return Json("");
            }
           
        }

        public bool DeleteVendorMap(int Id)
        {
            try
            {
                if (_context.TblUserWiseVendorCodes.Any(x => x.Id == Id))
                {
                    var modelToRemove = _context.TblUserWiseVendorCodes.First(x => x.Id == Id);
                    _context.Remove(modelToRemove);
                    _context.SaveChanges();
                    return true;
                }
                else {
                    return false;
                }
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public bool ActivateVendorUser(string User)
        {
            try
            {
                

                var result = _context.SpResponseVM.FromSqlRaw("EXEC usp_AssignRoleToVendorUser {0}", User).ToList();

                return true;

            }
            catch (Exception ex)
            {

                return false;
            }
        }
    }
}
