using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LILI_VMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace LILI_VMS.Controllers
{
    public static class HtmlContentExtensions
    {
        public static string ToHtmlStringT(this IHtmlContent htmlContent)
        {
            using (var writer = new StringWriter())
            {
                htmlContent.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
                return writer.ToString(); 
            }
        }
    }

    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly dbVehicleManagementContext _context;
        public EmployeeController(dbVehicleManagementContext context)
        {
            _context = context;
        }

        //public IActionResult Index()
        //{
        //    return View(eFTestContext.TblEmployee.ToList());
        //}

        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["EmpIdSortParm"] = String.IsNullOrEmpty(sortOrder) ? "empId_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var employees = from s in _context.TblEmployee
                           select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                employees = employees.Where(s => s.Name.Contains(searchString)
                                       || s.EmpId.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    employees = employees.OrderByDescending(s => s.Name);
                    break;
                case "empId_desc":
                    employees = employees.OrderByDescending(s => s.EmpId);
                    break;
                //case "Date":
                //    employees = employees.OrderBy(s => s.EnrollmentDate);
                //    break;
                //case "date_desc":
                //    employees = employees.OrderByDescending(s => s.EnrollmentDate);
                //    break;
                default:
                    employees = employees.OrderBy(s => s.Name);
                    break;
            }
             int pageSize =  20;
            return View(await PaginatedList<TblEmployee>.CreateAsync(employees.AsNoTracking(), pageNumber ?? 1, pageSize));

            //return View(await employees.AsNoTracking().ToListAsync());
        }

        public ActionResult Create()
        {
            TblEmployee entities = new TblEmployee();

            List<TblEmpGrade> empGradeList = new List<TblEmpGrade>();
            empGradeList = (from grade in _context.TblEmpGrade
                            select grade).ToList();
            empGradeList.Insert(0, new TblEmpGrade { Id = 0, GradeName = "Select Grade" });
            ViewBag.ListOfGrade = empGradeList;

            entities.JoiningDate = DateTime.Now;

            List<SelectListItem> results = new List<SelectListItem>();
            results.Add(new SelectListItem { Text = "A+", Value = "1" });
            results.Add(new SelectListItem { Text = "A", Value = "2" });
            results.Add(new SelectListItem { Text = "B+", Value = "3" });
            results.Add(new SelectListItem { Text = "B", Value = "4" });
            results.Add(new SelectListItem { Text = "C", Value = "5" });
            results.Add(new SelectListItem { Text = "F", Value = "6" });
            ViewData["ResultList"] = results;

            return View(entities);
            //return View();
        }

        [HttpPost, ActionName("CreateEmployee")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEmployee(TblEmployee emp)
        //[Bind("EnrollmentDate,FirstMidName,LastName")] Student student)
          {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(emp);
                    await _context.SaveChangesAsync();

                    //var employee = new TblEmployee
                    //{
                    //    Name = emp.Name,
                    //    EmpId = emp.EmpId,
                    //    Designation = emp.Designation,
                    //    Department = emp.Department,
                    //    EmpGrade = emp.EmpGrade,
                    //    IsActive = emp.IsActive,
                    //    JoiningDate = emp.JoiningDate
                    //};
                    //eFTestContext.TblEmployee.Add(employee);


                    //if (emp.TblEmployeeExpert != null)
                    //{
                    //    foreach (var item in emp.TblEmployeeExpert.ToList())
                    //    {

                    //        TblEmployeeExpert expertDetail = new TblEmployeeExpert();
                    //        expertDetail.EmpId = emp.EmpId;
                    //        expertDetail.ExpertiesId = item.ExpertiesId;

                    //        await eFTestContext.AddAsync(expertDetail);
                    //    }
                    //}


                    //if (emp.tblEmpEducationalDetail != null)
                    //{
                    //    foreach (var item in emp.tblEmpEducationalDetail.ToList())
                    //    {
                    //        TblEmployeeEducationalDetail itemDetail = new TblEmployeeEducationalDetail();
                    //        itemDetail.EmpId = emp.EmpId;
                    //        itemDetail.ExamId = item.ExamId;
                    //        itemDetail.Result = item.Result;
                    //        itemDetail.Comment = item.Comment;

                    //        await eFTestContext.AddAsync(itemDetail);
                    //    }
                    //}
                    //await eFTestContext.SaveChangesAsync();




                    //TempData["Success"] = "Success message text.";

                }
                else
                {
                    ViewData["Error"] = "Error message text.";
                    return View(emp);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            //return View(student);
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public bool Delete(int id)
        //public ActionResult Delete(int id)
        {
            try
            {
                TblEmployee emp = _context.TblEmployee.Where(s => s.Id == id).First();
                _context.TblEmployeeEducationalDetail.RemoveRange(_context.TblEmployeeEducationalDetail.Where(d => d.EmpId == emp.EmpId));
                _context.TblEmployee.Remove(emp);
                _context.SaveChanges();
                return true;
                //return RedirectToAction(nameof(Index));



            }
            catch (Exception ex)
            {
                return false;
                //return RedirectToAction(nameof(Index));
            }
        }

        public ActionResult Update(int id)
        {
            Models.TblEmployee empModel = new Models.TblEmployee();
            var dt = _context.TblEmployee.Where(s => s.Id == id).First();
            empModel.Id = dt.Id;
            empModel.Name = dt.Name;
            empModel.EmpId = dt.EmpId;
            empModel.Designation = dt.Designation;
            empModel.Department = dt.Department;
            empModel.EmpGrade = dt.EmpGrade;
            empModel.IsActive = dt.IsActive;
            empModel.JoiningDate = dt.JoiningDate;
            empModel.tblEmpEducationalDetail = _context.TblEmployeeEducationalDetail.Where(d => d.EmpId == dt.EmpId).ToList();
            //empModel.TblEmployeeExpert = eFTestContext.TblEmployeeExpert.Where(d => d.EmpId == dt.EmpId).ToList();

            var employeeExpert = from empExp in _context.TblEmployeeExpert
                                 from exp in _context.TblExpertArea
                                 where (empExp.EmpId == dt.EmpId && empExp.ExpertiesId==exp.Id)
                                 select new TblEmployeeExpert
                                 {
                                     ExpertiesId = empExp.ExpertiesId,
                                     ExpertArea = exp.ExpertArea,
                                     Description = exp.Description
                                 };

            empModel.TblEmployeeExpert = employeeExpert.ToList();

            List<TblEmpGrade> empGradeList = new List<TblEmpGrade>();
            empGradeList = (from grade in _context.TblEmpGrade
                            select grade).ToList();
            empGradeList.Insert(0, new TblEmpGrade { Id = 0, GradeName = "Select Grade" });
            ViewBag.ListOfGrade = empGradeList;

            List<SelectListItem> results = new List<SelectListItem>();
            results.Add(new SelectListItem { Text = "A+", Value = "1" });
            results.Add(new SelectListItem { Text = "A", Value = "2" });
            results.Add(new SelectListItem { Text = "B+", Value = "3" });
            results.Add(new SelectListItem { Text = "B", Value = "4" });
            results.Add(new SelectListItem { Text = "C", Value = "5" });
            results.Add(new SelectListItem { Text = "F", Value = "6" });
            ViewData["ResultList"] = results;

            //return View(eFTestContext.TblEmployee.Where(s => s.Id == id).First());
            return View(empModel);
        }

        //[HttpPost]
        //public ActionResult UpdateEmployee(TblEmployee emp)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        TblEmployee e = eFTestContext.TblEmployee.Where(s => s.Id == emp.Id).First();
        //        e.Name = emp.Name;
        //        e.EmpId = emp.EmpId;
        //        e.Designation = emp.Designation;
        //        e.Department = emp.Department;
        //        eFTestContext.SaveChanges();
        //    }
        //    return RedirectToAction("Index", "Employee");
        //}

        [HttpPost, ActionName("UpdateEmployee")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateEmployee(int id, TblEmployee emp)
        {
            if (id != emp.Id)
            {
                return NotFound();
            }
            try
            {
                //eFTestContext.Update(emp);
                //await eFTestContext.SaveChangesAsync();

                var employeeToUpdate = await _context.TblEmployee.FirstOrDefaultAsync(s => s.Id == id);
                if (await TryUpdateModelAsync<TblEmployee>(
                    employeeToUpdate,
                    "",
                    s => s.Name, s => s.EmpId, s => s.Designation, s => s.Department, s=> s.EmpGrade, s=> s.IsActive, s=>s.JoiningDate))

                    _context.TblEmployeeExpert.RemoveRange(_context.TblEmployeeExpert.Where(d => d.EmpId == emp.EmpId));

                if (emp.TblEmployeeExpert != null)
                {
                    foreach (var item in emp.TblEmployeeExpert.ToList())
                    {
                        TblEmployeeExpert itemDetail = new TblEmployeeExpert();
                        itemDetail.EmpId = emp.EmpId;
                        itemDetail.ExpertiesId = item.ExpertiesId;

                        await _context.AddAsync(itemDetail);
                    }
                    await _context.SaveChangesAsync();
                }

                _context.TblEmployeeEducationalDetail.RemoveRange(_context.TblEmployeeEducationalDetail.Where(d => d.EmpId == emp.EmpId));
                    //await eFTestContext.SaveChangesAsync();

                    if (emp.tblEmpEducationalDetail != null)
                    {
                        foreach (var item in emp.tblEmpEducationalDetail.ToList())
                        {
                            TblEmployeeEducationalDetail itemDetail = new TblEmployeeEducationalDetail();
                            itemDetail.EmpId = emp.EmpId;
                            itemDetail.ExamId = item.ExamId;
                            itemDetail.Result = item.Result;
                            itemDetail.Comment = item.Comment;

                            await _context.AddAsync(itemDetail);
                        }
                        await _context.SaveChangesAsync();
                    }


                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                //Log the error (uncomment ex variable name and write a log.)
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.");
            }
        //}
            return View(emp);
        }

        
        public IActionResult Experties()
        {
           //var model = new List<tblExpertArea>();
           var model = new List<TblExpertArea>(from c in _context.TblExpertArea select c);
            //var model = from c in eFTestContext.tblExpertArea select c;
           return PartialView("_ExpertAreaPartial", model);
        }

        [HttpPost]
        public JsonResult SetExperties(int expertiesId)
        {
            var keyVal = _context.TblExpertArea.Where(c => c.Id == expertiesId).ToList();
            if(keyVal != null)
            {
                expertiesId = keyVal.FirstOrDefault().Id;
            }

            if(expertiesId >= 0)
            {
                var sa = new JsonSerializerSettings();
                var expertiesInfo = from c in _context.TblExpertArea.Where(c => c.Id == expertiesId).ToList()
                                    select new 
                                    {
                                        c.Id,
                                        c.ExpertArea,
                                        c.Description
                                    };
                return Json(expertiesInfo, sa);
            }
            else
            {
                return Json("");
            }
        }

    }
}