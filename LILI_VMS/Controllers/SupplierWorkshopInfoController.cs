using LILI_VMS;
using LILI_VMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace LILI_VMS.Controllers
{
    [Authorize]
    public class SupplierWorkshopInfoController : BaseController
    {
        private readonly dbVehicleManagementContext _context;

        public SupplierWorkshopInfoController(dbVehicleManagementContext context)
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
            IQueryable<LILI_VMS.Models.TblSupplierWorkshopInformation> model = _context.TblSupplierWorkshopInformations.Where(x=>x.PlantId == PlantId);


            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(s => s.SuppWorkShopName.Contains(searchString) || s.SuppWorkShopName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    model = model.OrderByDescending(s => s.SuppWorkShopCode);
                    break;

                default:
                    model = model.OrderBy(s => s.Idate);
                    break;
            }
             int pageSize =  20;
            return View(await PaginatedList<TblSupplierWorkshopInformation>.CreateAsync(model.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public IActionResult Create()
        {  
            TblSupplierWorkshopInformation model = new TblSupplierWorkshopInformation();
            //var newGeneratedCode = GenerateToolCode();
            var data = _context.TblWorkShopTypes.ToList();
            data.Insert(0, new TblWorkShopType { WstypeCode = "*", WstypeName = "Select WorkShop Type" });
            ViewBag.ListOfWSType = data;
            model.Type = "Business";
            model.SuppWorkShopCode = GenerateBusinessCode(model.Type.ToString());
            TempData["msg"] = "";
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateSupplierWorkshopInfo(TblSupplierWorkshopInformation model)
        {
            try
            {
               //TempData["msg"] = "Data Save Unsuccessful";
                //return View("Create", model);

                if (ModelState.IsValid)
                {
                    if (DoesToolCodeExists(model.SuppWorkShopCode))
                    {
                        model.SuppWorkShopCode = GenerateBusinessCode(model.Type.ToString()).ToString();
                    }
                    var PlantId = long.Parse(HttpContext.Session.GetString("PlantId"));
                    model.Iuser = User.Identity.Name;
                    model.Idate = DateTime.Now;
                    model.PlantId = PlantId;
                    _context.Add(model);
                    await _context.SaveChangesAsync();

                   
                }
                else 
                {
                    TempData["msg"] = "Data Save Unsuccessful";
                    return View("Create", model);
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Error Occured : "+ex.Message;
                return View("Create", model);

            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Update(int vId) {
            var data = _context.TblWorkShopTypes.ToList();
            data.Insert(0, new TblWorkShopType { WstypeCode = "*", WstypeName = "Select WorkShop Type" });
            ViewBag.ListOfWSType = data;
            TblSupplierWorkshopInformation businessModel = new TblSupplierWorkshopInformation();
            var result = _context.TblSupplierWorkshopInformations.Where(s=>s.Id== vId).First();
            businessModel.Id = result.Id;
            businessModel.Type = result.Type;
            businessModel.SuppWorkShopCode = result.SuppWorkShopCode;
            businessModel.SuppWorkShopName = result.SuppWorkShopName;
            businessModel.ContactNo = result.ContactNo;
            businessModel.OwnerName = result.OwnerName;
            businessModel.Address = result.Address;
            businessModel.Comments = result.Comments;
            businessModel.Tin = result.Tin;
            businessModel.Bin = result.Bin;
            businessModel.Vat = result.Vat;
            businessModel.BankName = result.BankName;
            businessModel.AccountNumber = result.AccountNumber;
            businessModel.AccountName = result.AccountName;
            businessModel.Branch = result.Branch;
            businessModel.RoutingNumber = result.RoutingNumber;

         return View(businessModel);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateSupplierWorkshopInfo(TblSupplierWorkshopInformation model) {
            var vId=   model.Id;
            try
            {
                var toolSetupToUpdate = await _context.TblSupplierWorkshopInformations.FirstOrDefaultAsync(s=>s.Id==vId);
                toolSetupToUpdate.Edate= DateTime.Now;
                toolSetupToUpdate.Euser= User.Identity.Name;
                if (await TryUpdateModelAsync<TblSupplierWorkshopInformation>(
                    toolSetupToUpdate,
                    "",
                    s => s.Type,
                    s => s.SuppWorkShopName,
                    s => s.ContactNo,
                    s => s.OwnerName,
                    s => s.Address,
                    s => s.Comments,
                    s => s.BankName,
                     s => s.AccountName,
                    s => s.AccountNumber,
                    s => s.Branch,
                    s => s.RoutingNumber,
                    s => s.Tin,
                    s => s.Bin,
                    s => s.Vat


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
                TblSupplierWorkshopInformation model= _context.TblSupplierWorkshopInformations.Where(s=> s.Id == vId).First();
                if (model != null)
                {
                    _context.TblSupplierWorkshopInformations.Remove(model);
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
            if (busUnit == "Supplier")
            {
                //var result = _context.TblSupplierWorkshopInformations.Where(x => x.Type == "Supplier").OrderBy(x => x.Id).Select(x => x.SuppWorkShopCode).LastOrDefault();
                //var lastGrn = string.IsNullOrEmpty(result) ? "00000" : result;


                //var last5digits = "1";
                //if (lastGrn.Length > 3)
                //{
                //    last5digits = lastGrn.Substring(lastGrn.Length - 3);
                //}

                //int lastNumber = Int32.Parse(last5digits) + 1;
                //string lastNumberString = lastNumber.ToString("D3");
                ////             return $"{companyCode}{plantCode}gr{yearMonth}{lastNumberString}";
                //generatedCode = $"S-{lastNumberString}";


                //Generate Requisition No.---------Start
                String sDate = DateTime.Now.ToString();
                DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));
                //String dy = datevalue.Day.ToString("00");
                String mn = datevalue.Month.ToString("00");
                String yy = datevalue.Year.ToString().Substring(2, 2);
                var CodeNo = "SU-" + yy + mn;
                String maxId = "";
                String maxNo = (from c in _context.TblSupplierWorkshopInformations.Where(c => c.SuppWorkShopCode.Substring(0, 7) == CodeNo && c.Type=="Supplier").OrderByDescending(t => t.Id) select c.SuppWorkShopCode.Substring(7, 3)).FirstOrDefault();
                if (maxNo == null)
                {
                    maxId = "001";
                }
                else
                {
                    maxId = (Convert.ToInt16(maxNo) + 1).ToString("000");
                }
                CodeNo = "SU-" + yy + mn + maxId;

                generatedCode = CodeNo;

            }
            else
            {
                //var result = _context.TblSupplierWorkshopInformations.Where(x => x.Type == "WorkShop").OrderBy(x => x.Id).Select(x => x.SuppWorkShopCode).LastOrDefault();
                //var lastGrn = string.IsNullOrEmpty(result) ? "00000" : result;


                //var last5digits = "1";
                //if (lastGrn.Length > 3)
                //{
                //    last5digits = lastGrn.Substring(lastGrn.Length - 3);
                //}

                //int lastNumber = Int32.Parse(last5digits) + 1;
                //string lastNumberString = lastNumber.ToString("D3");
                ////             return $"{companyCode}{plantCode}gr{yearMonth}{lastNumberString}";
                //generatedCode = $"W-{lastNumberString}";

                //Generate Requisition No.---------Start
                String sDate = DateTime.Now.ToString();
                DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));
                //String dy = datevalue.Day.ToString("00");
                String mn = datevalue.Month.ToString("00");
                String yy = datevalue.Year.ToString().Substring(2, 2);
                var CodeNo = "WS-" + yy + mn;
                String maxId = "";
                String maxNo = (from c in _context.TblSupplierWorkshopInformations.Where(c => c.SuppWorkShopCode.Substring(0, 7) == CodeNo && c.Type == "WorkShop").OrderByDescending(t => t.Id) select c.SuppWorkShopCode.Substring(7, 3)).FirstOrDefault();
                if (maxNo == null)
                {
                    maxId = "001";
                }
                else
                {
                    maxId = (Convert.ToInt16(maxNo) + 1).ToString("000");
                }
                CodeNo = "WS-" + yy + mn + maxId;
                generatedCode = CodeNo;

            }

            return generatedCode;
        }
        public bool DoesToolCodeExists(string vToolCode)
        {

            return _context.TblBusinessUnitSetupInfos.Any(e => e.BusinessCode == vToolCode);
        }
        #endregion
    }
}
