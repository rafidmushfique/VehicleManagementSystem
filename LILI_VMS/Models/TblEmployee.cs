using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LILI_VMS.Models
{
    public partial class TblEmployee
    {
        public TblEmployee()
        {
            tblEmpEducationalDetail = new List<TblEmployeeEducationalDetail>();
            TblEmployeeExpert = new List<TblEmployeeExpert>();
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string EmpId { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        [Display(Name = "Grade")]
        public int EmpGrade { get; set; }
        public bool IsActive { get; set; }



        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{yyyy-MM-dd hh:mm}")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime JoiningDate { get; set; }

        public TblEmployee IdNavigation { get; set; }
        public TblEmployee InverseIdNavigation { get; set; }
        //public ICollection<TblEmployeeEducationalDetail> tblEmpEducationalDetail { get; set; }

        public List<TblEmployeeEducationalDetail> tblEmpEducationalDetail { get; set; }
        public List<TblEmployeeExpert> TblEmployeeExpert { get; set; }
    }
}
