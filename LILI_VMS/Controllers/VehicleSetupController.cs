using LILI_VMS;
using LILI_VMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using NPOI.HPSF;
using NPOI.SS.Formula.Functions;
using NuGet.Packaging.Signing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;


namespace LILI_VMS.Controllers
{
    [Authorize]
    public class VehicleSetup : BaseController
    {
        private readonly dbVehicleManagementContext _context;

        public VehicleSetup(dbVehicleManagementContext context)
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
            IQueryable<LILI_VMS.Models.TblVehicleSetup> model = from c in _context.TblVehicleSetups
                                                                from b in _context.TblBusinessUnitSetupInfos
                                                                where c.BusinessCode == b.BusinessCode && c.PlantId == PlantId
                                                                select new TblVehicleSetup
                                                                {
                                                                    Id = c.Id,
                                                                    VehicleCode= c.VehicleCode,
                                                                    RegistrationNo = c.RegistrationNo,
                                                                    Vendor = c.Vendor,
                                                                    BusinessName = b.BusinessName,
                                                                    Colour = c.Colour,
                                                                    Brand = c.Brand,
                                                                    Status = c.Status
                                                                };


            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(s => s.RegistrationNo.Contains(searchString) || s.VehicleCode.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    model = model.OrderByDescending(s => s.VehicleCode);
                    break;

                default:
                    model = model.OrderByDescending(s => s.Id);
                    break;
            }
             int pageSize =  20;
            return View(await PaginatedList<TblVehicleSetup>.CreateAsync(model.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public IActionResult Create()
        {
            var PlantId = long.Parse(HttpContext.Session.GetString("PlantId"));
            TblVehicleSetup model = new TblVehicleSetup();

            List<TblBusinessUnitSetupInfo> businessList = new List<TblBusinessUnitSetupInfo>();

            businessList = (from c in _context.TblBusinessUnitSetupInfos
                              where c.Type == "Business" && c.PlantId == PlantId
                            select new TblBusinessUnitSetupInfo
                              {
                                BusinessName = c.BusinessName,
                                BusinessCode = c.BusinessCode
                            }).ToList();
            businessList.Insert(0, new TblBusinessUnitSetupInfo
            {
                BusinessName = "-Select Business-",
                BusinessCode = "*"
            });
            ViewBag.BusinessList = businessList;

            List<TblVehicleOwner> vehicleOwnerList = new List<TblVehicleOwner>();

            vehicleOwnerList = (from c in _context.TblVehicleOwners
                            select new TblVehicleOwner
                            {
                                OwnerName = c.OwnerName,
                                OwnerCode = c.OwnerCode
                            }).ToList();
            vehicleOwnerList.Insert(0, new TblVehicleOwner
            {
                OwnerName = "-Select OwnerName-",
                OwnerCode = "*"
            });
            ViewBag.VehicleOwnerList = vehicleOwnerList;

            List<TblVehicleType> vehicleTypeList = new List<TblVehicleType>();

            vehicleTypeList = (from c in _context.TblVehicleTypes
                                select new TblVehicleType
                                {
                                    VehicleTypeName = c.VehicleTypeName,
                                    VehicleTypeCode = c.VehicleTypeCode
                                }).ToList();
            vehicleTypeList.Insert(0, new TblVehicleType
            {
                VehicleTypeName = "-Select Vehicle Type-",
                VehicleTypeCode = "*"
            });
            ViewBag.VehicleTypeList = vehicleTypeList;

            List<TblVehicleSize> vehicleSizeList = new List<TblVehicleSize>();

            vehicleSizeList = (from c in _context.TblVehicleSizes
                               select new TblVehicleSize
                               {
                                   VehicleSizeName = c.VehicleSizeName,
                                   VehicleSizeCode = c.VehicleSizeCode
                               }).ToList();
            vehicleSizeList.Insert(0, new TblVehicleSize
            {
                VehicleSizeCode = "*",
                VehicleSizeName = "-Select Vehicle Size-"
            });
            ViewBag.VehicleSizeList = vehicleSizeList;


            List<TblDocumentType> documentTypeList = new List<TblDocumentType>();

            documentTypeList = (from c in _context.TblDocumentTypes
                               select new TblDocumentType
                               {
                                   DocumentTypeName = c.DocumentTypeName,
                                   DocumentTypeCode = c.DocumentTypeCode
                               }).ToList();
            documentTypeList.Insert(0, new TblDocumentType
            {
                DocumentTypeName = "-Select VehicleTypeName-",
                DocumentTypeCode = "*"
            });
            ViewBag.DocumentTypeList = documentTypeList;

            model.VehicleCode = GenerateCode();
            model.InclusionDate = DateTime.Now;
            model.RegistrationDate = DateTime.Now;
            model.IssueDate = DateTime.Now;
            model.ExpiryDate = DateTime.Now;

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateVehicleSetup(TblVehicleSetup model)
        {
            try
            {
                //TempData["msg"] = "Data Save Unsuccessful";
                //return View("Create", model);

                if (ModelState.IsValid)
                {
                    if (DoesCodeExists(model.VehicleCode))
                    {
                        model.VehicleCode = GenerateCode();
                        if (model.TblVehicleDocumentDetails?.Any() == true)
                        {
                            foreach (var item in model.TblVehicleDocumentDetails)
                            {

                                item.VehicleCode = model.VehicleCode;

                            }
                        }
                    }
                    var UnitCode = HttpContext.Session.GetString("UnitCode");
                    var PlantId = long.Parse(HttpContext.Session.GetString("PlantId"));
                    model.Iuser = User.Identity.Name;
                    model.Idate = DateTime.Now;
                    model.UnitCode = UnitCode;
                    model.PlantId = PlantId;
                    _context.Add(model);
                    await _context.SaveChangesAsync();


                    //TempData["msg"] = "Success message text.";

                }

            }
            catch (Exception ex)
            {

                return View("Create", model);

            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Update(int vId) {
            TblVehicleSetup vehicleSetupModel = new TblVehicleSetup();

            List<TblBusinessUnitSetupInfo> businessList = new List<TblBusinessUnitSetupInfo>();

            businessList = (from c in _context.TblBusinessUnitSetupInfos
                            where c.Type == "Business"
                            select new TblBusinessUnitSetupInfo
                            {
                                BusinessName = c.BusinessName,
                                BusinessCode = c.BusinessCode
                            }).ToList();
            businessList.Insert(0, new TblBusinessUnitSetupInfo
            {
                BusinessName = "-Select Business-",
                BusinessCode = "*"
            });
            ViewBag.BusinessList = businessList;

            List<TblVehicleOwner> vehicleOwnerList = new List<TblVehicleOwner>();

            vehicleOwnerList = (from c in _context.TblVehicleOwners
                                select new TblVehicleOwner
                                {
                                    OwnerName = c.OwnerName,
                                    OwnerCode = c.OwnerCode
                                }).ToList();
            vehicleOwnerList.Insert(0, new TblVehicleOwner
            {
                OwnerName = "-Select OwnerName-",
                OwnerCode = "*"
            });
            ViewBag.VehicleOwnerList = vehicleOwnerList;

            List<TblVehicleType> vehicleTypeList = new List<TblVehicleType>();

            vehicleTypeList = (from c in _context.TblVehicleTypes
                               select new TblVehicleType
                               {
                                   VehicleTypeName = c.VehicleTypeName,
                                   VehicleTypeCode = c.VehicleTypeCode
                               }).ToList();
            vehicleTypeList.Insert(0, new TblVehicleType
            {
                VehicleTypeName = "-Select VehicleTypeName-",
                VehicleTypeCode = "*"
            });
            ViewBag.VehicleTypeList = vehicleTypeList;

            List<TblVehicleSize> vehicleSizeList = new List<TblVehicleSize>();

            vehicleSizeList = (from c in _context.TblVehicleSizes
                               select new TblVehicleSize
                               {
                                   VehicleSizeName = c.VehicleSizeName,
                                   VehicleSizeCode = c.VehicleSizeCode
                               }).ToList();
            vehicleSizeList.Insert(0, new TblVehicleSize
            {
                VehicleSizeCode = "*",
                VehicleSizeName = "-Select Vehicle Size-"
            });
            ViewBag.VehicleSizeList = vehicleSizeList;

            List<TblDocumentType> documentTypeList = new List<TblDocumentType>();

            documentTypeList = (from c in _context.TblDocumentTypes
                                select new TblDocumentType
                                {
                                    DocumentTypeName = c.DocumentTypeName,
                                    DocumentTypeCode = c.DocumentTypeCode
                                }).ToList();
            documentTypeList.Insert(0, new TblDocumentType
            {
                DocumentTypeName = "-Select DocumentTypeName-",
                DocumentTypeCode = "*"
            });
            ViewBag.DocumentTypeList = documentTypeList;

            vehicleSetupModel = _context.TblVehicleSetups.Where(s => s.Id == vId).First();
            vehicleSetupModel.IssueDate = DateTime.Now;
            vehicleSetupModel.ExpiryDate = DateTime.Now;

           
            //vehicleSetupModel.Id = result.Id;
            //vehicleSetupModel.BusinessCode = result.BusinessCode;
            //vehicleSetupModel.VehicleCode = result.VehicleCode;
            //vehicleSetupModel.InclusionDate = result.InclusionDate;
            //vehicleSetupModel.Owner = result.Owner;
            //vehicleSetupModel.Vendor = result.Vendor;
            //vehicleSetupModel.VendorMobile = result.VendorMobile;
            //vehicleSetupModel.RegistrationNo = result.RegistrationNo;
            //vehicleSetupModel.RegistrationDate = result.RegistrationDate;
            //vehicleSetupModel.PurchasePrice = result.PurchasePrice;
            //vehicleSetupModel.Depriciation = result.Depriciation;
            //vehicleSetupModel.ManufacturingYear = result.ManufacturingYear;
            //vehicleSetupModel.ModelNo = result.ModelNo;
            //vehicleSetupModel.Brand = result.Brand;
            //vehicleSetupModel.Colour = result.Colour;
            //vehicleSetupModel.Weight = result.Weight;
            //vehicleSetupModel.Length = result.Length;
            //vehicleSetupModel.Width = result.Width;
            //vehicleSetupModel.Height = result.Height;
            //vehicleSetupModel.TypeOfVehicle = result.TypeOfVehicle;
            //vehicleSetupModel.SizeOfVehicle = result.SizeOfVehicle;
            //vehicleSetupModel.PresentMeterKm = result.PresentMeterKm;
            //vehicleSetupModel.TyreSize = result.TyreSize;
            //vehicleSetupModel.QtyofTyre = result.QtyofTyre;
            //vehicleSetupModel.Cc = result.Cc;
            //vehicleSetupModel.CapacityMt = result.CapacityMt;
            //vehicleSetupModel.Kpl = result.Kpl;
            //vehicleSetupModel.FuelTankCapacity = result.FuelTankCapacity;
            //vehicleSetupModel.DriverName = result.DriverName;
            //vehicleSetupModel.DriverStaffId = result.DriverStaffId;
            //vehicleSetupModel.DriverMobileNo = result.DriverMobileNo;
            //vehicleSetupModel.DriverDailyAllowance = result.DriverDailyAllowance;
            //vehicleSetupModel.HelperName = result.HelperName;
            //vehicleSetupModel.HelperStaffId = result.HelperStaffId;
            //vehicleSetupModel.HelperMobileNo = result.HelperMobileNo;
            //vehicleSetupModel.HelperDailyAllowance = result.HelperDailyAllowance;
            //vehicleSetupModel.Comments = result.Comments;
            //vehicleSetupModel.DriverJoiningDate = result.DriverJoiningDate;
            //vehicleSetupModel.HelperJoiningDate = result.HelperJoiningDate;
            var detail_business_list = (from master in _context.TblVehicleSetups
                                        from bus in _context.TblVehicleDocumentDetails
                                        from ex in _context.TblDocumentTypes
                                        where (master.VehicleCode == bus.VehicleCode && bus.DocumentTypeCode == ex.DocumentTypeCode && master.VehicleCode== vehicleSetupModel.VehicleCode)
                                        orderby bus.Id
                                        select new TblVehicleDocumentDetail
                                        {
                                            Id = bus.Id,
                                            VehicleCode = bus.VehicleCode,
                                            FileName = bus.FileName,
                                            OriginalFileName = bus.OriginalFileName,
                                            FileType = bus.FileType,
                                            Location = bus.Location,

                                            DocumentTypeCode = bus.DocumentTypeCode,
                                            DocumentTypeName = _context.TblDocumentTypes.Where(x => x.DocumentTypeCode == bus.DocumentTypeCode).FirstOrDefault().DocumentTypeName,
                                            IssueDate = bus.IssueDate,
                                            ExpiryDate = bus.ExpiryDate
                                        });

            vehicleSetupModel.TblVehicleDocumentDetails = detail_business_list.ToList();


            return View(vehicleSetupModel);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateVehicleSetup(TblVehicleSetup model) {
            var vId=   model.Id;
            var vehicleCode = model.VehicleCode;
            try
            {

                var vehicleSetupToUpdate = await _context.TblVehicleSetups.FirstOrDefaultAsync(s=>s.Id==vId);
                vehicleSetupToUpdate.Edate= DateTime.Now;
                vehicleSetupToUpdate.Euser= User.Identity.Name;
                if (await TryUpdateModelAsync<TblVehicleSetup>(
                    vehicleSetupToUpdate,
                    "",
                    s => s.BusinessCode,
                    s => s.VehicleCode,
                    s => s.InclusionDate,
                    s => s.Owner,
                    s => s.Vendor,
                    s => s.VendorMobile,
                    s => s.RegistrationNo,
                    s => s.RegistrationDate,
                    s => s.PurchasePrice,
                    s => s.Depriciation,
                    s => s.ManufacturingYear,
                    s => s.ModelNo,
                    s => s.Brand,
                    s => s.Colour,
                    s => s.Weight,
                    s => s.Length,
                    s => s.Width,
                    s => s.Height,
                    s => s.TypeOfVehicle,
                     s => s.SizeOfVehicle,
                    s => s.PresentMeterKm,
                    s => s.TyreSize,
                    s => s.QtyofTyre,
                    s => s.Cc,
                    s => s.CapacityMt,
                    s => s.Kpl,
                    s => s.FuelTankCapacity,
                    s => s.DriverName,
                    s => s.DriverStaffId,
                    s => s.DriverMobileNo,
                    s => s.DriverDailyAllowance,
                    s => s.HelperName,
                    s => s.HelperStaffId,
                    s => s.HelperMobileNo,
                    s => s.HelperDailyAllowance,
                    s => s.Comments,
                    s => s.DriverJoiningDate,
                    s => s.HelperJoiningDate,
                    s => s.Status

                    )) ;

                _context.TblVehicleDocumentDetails.RemoveRange(_context.TblVehicleDocumentDetails.Where(s => s.VehicleCode == vehicleCode));

                List<TblVehicleDocumentDetail> vehicleList = new List<TblVehicleDocumentDetail>();
                foreach (var item in model.TblVehicleDocumentDetails)
                {
                    TblVehicleDocumentDetail vehicleDocumentDetail = new TblVehicleDocumentDetail();
                    vehicleDocumentDetail.VehicleCode = vehicleCode;

                    vehicleDocumentDetail.FileName = item.FileName;
                    vehicleDocumentDetail.Location = item.Location;
                    vehicleDocumentDetail.OriginalFileName = item.OriginalFileName;
                    vehicleDocumentDetail.FileType = item.FileType;

                    vehicleDocumentDetail.DocumentTypeCode = item.DocumentTypeCode;
                    vehicleDocumentDetail.DocumentTypeName = _context.TblDocumentTypes.Where(x => x.DocumentTypeCode == item.DocumentTypeCode).FirstOrDefault().DocumentTypeName;
                    vehicleDocumentDetail.IssueDate = item.IssueDate;
                    vehicleDocumentDetail.ExpiryDate = item.ExpiryDate;
                   
                    vehicleList.Add(vehicleDocumentDetail);
                }
                await _context.AddRangeAsync(vehicleList);

                await _context.SaveChangesAsync();

               
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.");
            }
            return RedirectToAction(nameof(Index));
        }


        public bool Delete(int vId)
        {
            try
            {
                TblVehicleSetup model= _context.TblVehicleSetups.Where(s=> s.Id == vId).First();

                if (model != null)
                {
                    _context.TblVehicleDocumentDetails.RemoveRange(_context.TblVehicleDocumentDetails.Where(d => d.VehicleCode == model.VehicleCode));
                    _context.TblVehicleSetups.Remove(model);
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

        [HttpPost]
        public async Task<ActionResult> DeleteDetail(int Id)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var detVehicleDocument = _context.TblVehicleDocumentDetails.Where(d => d.Id == Id).FirstOrDefault();
                _context.TblVehicleDocumentDetails.Remove(detVehicleDocument);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

            }
            finally
            {
                await transaction.CommitAsync();
            }
            return Ok();
        }

        public async ValueTask<JsonResult> UploadFile(IFormFile file)
        {
            var sa = new JsonSerializerSettings();
            AttachmentFileViewModel data = new AttachmentFileViewModel();
            //HttpPostedFileBase file = Request.Files["file"];
            if (file != null)
            {
                IFormFile Attachment = null;
                var (originalFileName, filename, fileLocation, extension) = await UploadFileAndReturnFileName(file);
                data = new AttachmentFileViewModel
                {
                    OriginalFileName = originalFileName,
                    Filename = filename,
                    FileLocation = fileLocation,
                    Extension = extension
                };
            }
            else {
                data = new AttachmentFileViewModel
                {
                    OriginalFileName = "",
                    Filename = "",
                    FileLocation = "",
                    Extension = ""
                };
            }
           

            return Json(data);
        }

        #region private classes
        public string GenerateCode()
        {
            //var generatedCode = "";
            //// var yearMonth = DateTime.Now.ToString("yyyyMM");

            //    var result = _context.TblVehicleSetups.OrderBy(x => x.Id).Select(x => x.VehicleCode).LastOrDefault();
            //    var lastGrn = string.IsNullOrEmpty(result) ? "00000" : result;


            //    var last5digits = "1";
            //    if (lastGrn.Length > 3)
            //    {
            //        last5digits = lastGrn.Substring(lastGrn.Length - 3);
            //    }

            //    int lastNumber = Int32.Parse(last5digits) + 1;
            //    string lastNumberString = lastNumber.ToString("D3");
            //    //             return $"{companyCode}{plantCode}gr{yearMonth}{lastNumberString}";
            //    generatedCode = $"V-{lastNumberString}";

            //Generate Requisition No.---------Start
            String sDate = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));
            //String dy = datevalue.Day.ToString("00");
            String mn = datevalue.Month.ToString("00");
            String yy = datevalue.Year.ToString().Substring(2, 2);
            var CodeNo = "VH-" + yy + mn;
            String maxId = "";
            String maxNo = (from c in _context.TblVehicleSetups.Where(c => c.VehicleCode.Substring(0, 7) == CodeNo).OrderByDescending(t => t.Id) select c.VehicleCode.Substring(7, 3)).FirstOrDefault();
            if (maxNo == null)
            {
                maxId = "001";
            }
            else
            {
                maxId = (Convert.ToInt16(maxNo) + 1).ToString("000");
            }
            CodeNo = "VH-" + yy + mn + maxId;


            return CodeNo;

           
        }

        private async Task<(string, string, string, string)> UploadFileAndReturnFileName(IFormFile file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            try
            {

                string uniqueFileName = GenerateUniqueFileName(file.FileName);
                string extension = Path.GetExtension(file.FileName);

                var newFileName = String.Concat(uniqueFileName);

                string filePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload")).Root + $@"{newFileName}";
                //string filePath = Path.Combine(_hostEnvironment.WebRootPath,"Upload"); ;
                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }


                //FtpWebRequest request = (FtpWebRequest)WebRequest.Create(filePath);
                //request.Method = WebRequestMethods.Ftp.UploadFile;


                return (file.FileName, uniqueFileName, filePath, extension);
            }

            catch (WebException e)
            {
                string status = ((FtpWebResponse)e.Response).StatusDescription;
                throw new Exception(status);
            }
        }
        private static string GenerateUniqueFileName(string fileName)
        {
            var unixTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            fileName = Path.GetFileName(fileName);
            return $"{unixTimestamp}"
                   + "_"
                   + Guid.NewGuid()
                   + Path.GetExtension(fileName);
        }

        public bool DoesCodeExists(string VehicleCode)
        {

            return _context.TblVehicleSetups.Any(e => e.VehicleCode == VehicleCode);
        }


        [HttpPost]
        public bool RemoveFile(string fileName)
        {

            string filePathToDelete = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", fileName);

            if (System.IO.File.Exists(filePathToDelete))
            {
                System.IO.File.Delete(filePathToDelete);
                // Optionally, you can add additional logic or messages after file deletion
            }

            // Redirect to the action or view after deletion
            return true;
        }

        #endregion
    }
}
