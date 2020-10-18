using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JqueryDatatableExample.Models
{
    public class EmployeeViewModel
    {
        //private  string trimname;
        public int Id { get; set; }

        //public  string EmployeeName {
        //    get { return trimname; }
        //    set { trimname = value.Trim().ToString() != "" ? trimname : null; }
        //        }

        [Display(Name = "Employee Address")]
        public string EmployeeAddress { get; set; }

        [Display(Name = "Department Name")]
        public string DepartmentName { get; set; }

        public string EmployeeName { get; set; }

        [Required(ErrorMessage = "Please select atleast one department")]
        public List<DeptName> deptNames { get; set; }

        public int EmployeeId { get; set; }

        public string deptName { get; set; }

        public string Attachment { get; set; }

        public byte[] fileAttachment { get; set; }


        public string FileName { get; set; }

        [Required(ErrorMessage ="Please upload the file")]
        public HttpPostedFileBase fileuploader { get; set; }

        public List<Countrys> countrys { get; set; }

        public List<string> Selectedcountry { get; set; }
    }

    public class DeptName
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please select atleast one department")]
        public string DepartName { get; set; }

        public bool IsSelected { get; set; }

    }

    public class Countrys
    {
        public int Id { get; set; }

        public string CountryName { get; set; }

        public int EmployeeId { get; set; }

    }
}