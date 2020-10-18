using JqueryDatatableExample.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace JqueryDatatableExample.Controllers
{
    public class HomeController : Controller
    {
        smrutiEntities db = new smrutiEntities();
        public ActionResult Index()
        {
            //var model=  (from E in db.Employees
            //   join D in db.Departments
            //   on E.Id equals D.EmployeeId
            //   select new EmployeeViewModel {Id=E.Id,EmployeeAddress=E.EmployeeAddress,EmployeeName=E.EmployeeName,DepartmentName=D.DepartmentName }).Distinct().ToList();
            EmployeeViewModel emp = new EmployeeViewModel();

            List<DeptName> deptNames = new List<DeptName> { new DeptName { Id = 1, DepartName = "IT" }, new DeptName { Id = 2, DepartName = "Mechanical" } };
            emp.deptNames = deptNames;
            return View("Index");
        }



        [HttpPost]
        public JsonResult EmployeeGrid(DataTableAjaxPostModel model)
        {
            try
            {
                int filteredResultsCount;
                int totalResult;
                var res = usergrid(model, out filteredResultsCount, out totalResult);

                return Json(new
                {
                    draw = model.draw,
                    recordsTotal = totalResult,
                    recordsFiltered = filteredResultsCount,
                    data = res
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpGet]
        public ActionResult AddEmployee()
        {
            EmployeeViewModel emp = new EmployeeViewModel();
            List<Countrys> countryList = new List<Countrys>();
            List<DeptName> deptNames = new List<DeptName> { new DeptName { Id = 1, DepartName = "IT" }, new DeptName { Id = 2, DepartName = "Mechanical" } };
            emp.deptNames = deptNames;
            var country = db.Countries.ToList();
            foreach (var item in country)
            {
                Countrys countrys = new Countrys();
                countrys.Id = item.Id;
                countrys.CountryName = item.CountryName;
                countryList.Add(countrys);
            }
            emp.countrys = countryList;
            return View("AddEmployee1", emp);
        }

        [HttpGet]
        public ActionResult AddEmployeeModal()
        {
            EmployeeViewModel emp = new EmployeeViewModel();
           
            List<DeptName> deptNames = new List<DeptName> { new DeptName { Id = 1, DepartName = "IT" }, new DeptName { Id = 2, DepartName = "Mechanical" } };
            emp.deptNames = deptNames;
            return PartialView("AddEmployee", emp);
        }

        [HttpPost]
        public ActionResult SaveEmployee(FormCollection collection, EmployeeViewModel model)
        {
            string[] departmentarray;
            //string strselecteddepartment = collection["myParams"];
            //departmentarray = strselecteddepartment.Split(',');

            EmployeeViewModel Vm = new EmployeeViewModel();
            Department department = new Department();
            List<Countrys> countryList = new List<Countrys>();
            var country = db.Countries.ToList();
            foreach (var item in country)
            {
                Countrys countrys = new Countrys();
                countrys.Id = item.Id;
                countrys.CountryName = item.CountryName;
                countryList.Add(countrys);
            }
            model.countrys = countryList;
            Employee emp = new Employee();
            var pic = System.Web.HttpContext.Current.Request.Files["fileuploader"];
            byte[] bytes;
           int filesize= pic.ContentLength;
            using (var stream = new MemoryStream(727420768))
            {
                pic.InputStream.CopyTo(stream);
                bytes = stream.ToArray();
            }
            if(filesize > 1024)
            {
               ModelState.AddModelError("fileuploader", "Max size file");
            }
            if (ModelState.IsValid)
            {
                if (model.Id == 0)
                {
                    emp.EmployeeName = model.EmployeeName;
                    emp.EmployeeAddress = model.EmployeeAddress;
                    emp.Attachment = bytes;
                    emp.FileName = pic.FileName;

                    //department.DepartmentName = model.deptName;
                    db.Employees.Add(emp);
                    db.SaveChanges();
                    var id = emp.Id;

                    //foreach (var item in departmentarray)
                    //{
                    department.EmployeeId = id;
                    department.DepartmentName = model.deptName;
                    db.Departments.Add(department);
                    db.SaveChanges();
                    // }


                    //    return Json("Success", JsonRequestBehavior.AllowGet);

                }
                // return Json("Fail", JsonRequestBehavior.AllowGet);
                //return RedirectToAction("Index");
            }
            return View("AddEmployee1", model);

        }




        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }



        public List<EmployeeViewModel> usergrid(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResult)
        {
            List<EmployeeViewModel> finalresult = new List<EmployeeViewModel>();
            var searchBy = (model.search != null) ? model.search.value : null;
            var take = model.length;
            var skip = model.start;
            string sortBy = string.Empty;
            bool sortDir = true;
            if (model.order != null)
            {
                var column = model.order.Select(t => t.column).First();
                var dir = model.order.Select(t => t.dir).First();
                switch (column)
                {
                    case 1:
                        sortBy = "EmployeeName";
                        sortDir = dir.ToLower() == "desc";
                        break;

                    case 2:
                        sortBy = "EmployeeAddress";
                        sortDir = dir.ToLower() == "desc";
                        break;

                    case 3:
                        sortBy = "DepartmentName";
                        sortDir = dir.ToLower() == "desc";
                        break;

                    default:
                        sortBy = "EmployeeName";
                        sortDir = dir.ToLower() == "desc";
                        break;

                }
            }

            if (string.IsNullOrEmpty(searchBy))
            {
                var result = GetAllEmployeeForGrid().AsQueryable<EmployeeViewModel>().OrderBy<EmployeeViewModel>(sortBy, sortDir).ToList();
                filteredResultsCount = result.Count();
                totalResult = filteredResultsCount;
                finalresult = result.Skip(skip).Take(take).ToList();

            }
            else
            {
                var result = GetAllEmployeeForGrid().Where(x => (x.EmployeeName ?? string.Empty).ToLower().Contains(searchBy.ToLower()) ||
                (x.EmployeeAddress ?? string.Empty).ToLower().Contains(searchBy.ToLower()) || (x.DepartmentName ?? string.Empty).ToLower().Contains(searchBy.ToLower())).ToList();

                //result.AsQueryable<EmployeeViewModel>().ToList();
                totalResult = GetAllEmployeeForGrid().AsQueryable<EmployeeViewModel>().Count();
                filteredResultsCount = result.Count();
                finalresult = result.Skip(skip).Take(take).ToList();
            }

            return finalresult;
        }

        public List<EmployeeViewModel> GetAllEmployeeForGrid()
        {
            //List<EmployeeViewModel> Employeemodel = new List<EmployeeViewModel>();
            var model = (from E in db.Employees
                         join D in db.Departments
                         on E.Id equals D.EmployeeId
                         select new EmployeeViewModel { Id = E.Id, EmployeeAddress = E.EmployeeAddress, EmployeeName = E.EmployeeName, DepartmentName = D.DepartmentName }).Distinct().ToList();
            return model;
        }

        public ActionResult EditEmployee(int Id)
        {
            EmployeeViewModel Evm = new EmployeeViewModel();
            List<DeptName> listdept = new List<DeptName>();
            DeptName departmentname = new DeptName();
            var Emp = db.Employees.Where(x => x.Id == Id).FirstOrDefault();
            var Dept = db.Departments.Where(x => x.EmployeeId == Id).ToList();
            Evm.EmployeeName = Emp.EmployeeName;
            Evm.EmployeeAddress = Emp.EmployeeAddress;
            Evm.EmployeeId = Emp.Id;
            Evm.FileName = Emp.FileName;
            
            foreach (var item in Dept)
            {
                    departmentname.DepartName = item.DepartmentName;
                    departmentname.IsSelected = true;
               
                listdept.Add(departmentname);
            }
            Evm.deptNames = listdept;
            return PartialView("AddEmployee1", Evm);
        }

        public ActionResult Email()
        {
            EmailMessageModel email = new EmailMessageModel();
            return View("Email", email);
        }

        [HttpPost]
        public ActionResult Email(EmailMessageModel model,List<HttpPostedFileBase> fileuploader)
        {


            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.EnableSsl = true;
            smtp.Timeout = 100000;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(model.Email, model.Password);
            MailMessage mm = new MailMessage(model.Email, model.To);

            if (fileuploader != null)
            {
                foreach (HttpPostedFileBase fileuploaders in fileuploader)
                {
                    string fileName = Path.GetFileName(fileuploaders.FileName);
                    mm.Attachments.Add(new Attachment(fileuploaders.InputStream, fileName));
                }
            }
            
            string body = string.Empty;
            body = "Hi Smruti your Email Id is " + model.Email + ",</br></br>";
            body += "</br></br><p style='color:red'>* This is testing mail.Please do not reply.</p>";
            mm.Body = body;
            mm.Subject = model.Subject;
            mm.IsBodyHtml = true;

            smtp.Send(mm);
            ViewBag.Message = "Email sent.";


            return View();
        }

        [HttpGet]
        public ActionResult Download(int Id)
        {
            EmployeeViewModel Evm = new EmployeeViewModel();
            var Emp = db.Employees.Where(x => x.Id == Id).FirstOrDefault();
            Evm.EmployeeId = Emp.Id;
            Evm.fileAttachment = Emp.Attachment;
            Evm.FileName = Emp.FileName;
            return File(Emp.Attachment, Path.GetExtension(Emp.FileName), Emp.FileName);

        }

    }
}
