using LILI_VMS.Data;
using LILI_VMS.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LILI_VMS.Services
{
    public class MenuMasterService:IMenuMasterService
    {
        private readonly dbVehicleManagementContext _dbContext;
        private readonly IHttpContextAccessor _httpContext;

        //dbEFTestContext _dbContext = new dbEFTestContext();

        public MenuMasterService(dbVehicleManagementContext dbContext, IHttpContextAccessor httpContext)
        {
            _dbContext = dbContext;
            _httpContext = httpContext;
        }

        public IEnumerable<MenuMaster> GetMenuMaster()
        {
            List<MenuMaster> result = new List<MenuMaster>();
            var User = _httpContext.HttpContext?.User;
            if (User != null && User.Identity.IsAuthenticated)
            {
                var roleClaim = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(s => s.Value).ToList();
                result = _dbContext.MenuMaster.Where(m => roleClaim.Contains(m.UserRoll)).OrderBy(m => m.MenuOrder).ToList();

            }
            return result;
        }
        public string GetUnitName()
        {
            string v = _httpContext.HttpContext.Session?.GetString("UnitName");
            return v;
        }
        public IEnumerable<MenuMaster> GetMenuMaster(string UserRole)
        {
            //var result = _dbContext.MenuMaster.Where(m => m.UserRoll == UserRole).ToList();
            var result = _dbContext.MenuMaster.Where(m => m.UserRoll == UserRole).OrderBy(m=>m.MenuOrder).ToList();
            return result;
        }
    }
}
