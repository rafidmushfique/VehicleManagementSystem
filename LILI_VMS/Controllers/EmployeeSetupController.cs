using LILI_VMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.OpenXmlFormats.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LILI_VMS.Controllers
{
    public class EmployeeSetupController : BaseController
    {
        private readonly dbVehicleManagementContext _context;

        public EmployeeSetupController(dbVehicleManagementContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {

            return View();
        }
        public IActionResult Create() 
        {
            var PlantId = long.Parse(HttpContext.Session.GetString("PlantId"));
            var userList = _context.AspNetUsers.ToList();
            var UnitList = _context.TblBusinessUnitSetupInfos.Where(x => x.Type == "Unit" && x.PlantId == PlantId).ToList();
            var RoleList = _context.AspNetRoles.ToList();
            var AuthList = _context.TblAuthRoleTypes.ToList();

          
            ViewBag.UserList = userList;
            ViewBag.UnitList = UnitList;
            ViewBag.RoleList = RoleList.Where(x => x.Name != "Admin" && x.Name != "Admin2").ToList();
            ViewBag.AuthList = AuthList;
            return View();
        
        }

        public ActionResult CreateEmployeeSetup(EmployeeSetupVM model)
        {
            try
            {

                var userRole = model.UserRole;
                List<TblUserAuthRole> modelData = new List<TblUserAuthRole>();
                modelData = model.TblUserMenuAuth.ToList();
                var newrecordData = modelData.Where(m => !_context.TblUserAuthRoles.Any(x => x.Id == m.Id)).ToList();

                if (newrecordData.Any())
                {
                    _context.AddRange(newrecordData);
                }

                if (model.PlantCodes != null)
                {
                    AddUserUnitMapping(model.UserId, model.PlantCodes);
                }

                if (!string.IsNullOrEmpty(userRole))
                {
                    AddUserRole(model.UserId, userRole);
                }
              
                _context.SaveChanges(); 


            }
            catch (Exception ex)
            {

                throw;
            }
          

            return RedirectToAction(nameof(Create));
        }

        public JsonResult GetRoleWiseMenuList(string RoleId)
        {

            var Role=_context.AspNetRoles.FirstOrDefault(x => x.Id == RoleId).Name;
            try
            {
                List<MenuMaster> model = new List<MenuMaster>();
                if (Role == "Manager" || Role == "Admin")
                {
                     model = (from c in _context.MenuMaster
                                 where c.UserRoll.ToUpper() == Role.ToUpper() && c.ParentMenuId != "*"
                                 select c).ToList();
                }

                return Json(model);
            }
            catch (System.Exception)
            {

                return Json("");
            }
        

           
        }
               
        public bool AddUserUnitMapping(string user, string[] units)
        {
            var PlantId = HttpContext.Session.GetString("PlantId");
            var existingMappings = _context.TblUserWiseUnitAndPlantCodes
                                    .Where(x => x.UserId == user && units.Contains(x.UnitCode))
                                    .Select(x => x.UnitCode)
                                    .ToHashSet();

            var newUnits = units
            .Where(unit => !existingMappings.Contains(unit))
            .ToList();
            

            if (newUnits.Any())
            {
                List<TblUserWiseUnitAndPlantCode> modelRange = new List<TblUserWiseUnitAndPlantCode>();

                foreach (var unit in newUnits)
                {
                    TblUserWiseUnitAndPlantCode model = new TblUserWiseUnitAndPlantCode
                    {
                        UserId = user,
                        UnitCode = unit,
                        PlantId = long.Parse(PlantId)
                    };

                    modelRange.Add(model);
                }


                try
                {

                    _context.TblUserWiseUnitAndPlantCodes.AddRange(modelRange);
                    return true;
                }
                catch (Exception ex)
                {

                    return false;
                }
            }
            else {
                return true;
            }




        }
        public bool AddUserRole(string user, string userrole) {
            var userId = _context.AspNetUsers.FirstOrDefault(x => x.Email == user)?.Id;

           
           
            if (userId != null)
            {
                if (!_context.AspNetUserRoles.Any(x => x.UserId == userId))
                {
                    AspNetUserRoles role = new AspNetUserRoles();
                    role.UserId = userId;
                    role.RoleId = userrole;

                    _context.Add(role);
                }
               

                return true;
            }
            else 
            { 
             return false;
            }

        }

        public JsonResult GetUserWiseInfo(string user)
        {
            EmployeeSetupVM model = new EmployeeSetupVM();

            model.TblUserMenuAuth = _context.TblUserAuthRoles.Where(x => x.UserId == user).ToList();
            var userInfo = (from u in _context.AspNetUsers
                             from au in _context.AspNetUserRoles
                             where u.Id == au.UserId && u.Email == user
                             select au).FirstOrDefault();
            model.UserRole =  userInfo.RoleId;
            if (_context.TblUserWiseUnitAndPlantCodes.Any(x => x.UserId == user))
            {
                model.PlantCodes = _context.TblUserWiseUnitAndPlantCodes.Where(x => x.UserId == user).Select(s => s.UnitCode).ToArray();

               
            }
            return Json(model);
        }

        public JsonResult DeleteUserAuth(int id) 
        {
            if (_context.TblUserAuthRoles.Any(x => x.Id == id))
            {
                try
                {
                    var model = _context.TblUserAuthRoles.First(x => x.Id == id);
                    _context.TblUserAuthRoles.Remove(model);
                    _context.SaveChanges();

                    return Json("success");
                }
                catch (Exception ex)
                {

                    return Json("Error");
                }


            }
            else {
                return Json("NotExists");
            }
        }
    }
}
