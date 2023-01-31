using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Smartsheetsproject.Models
{
  public class NiftiModel  {


        public long pipelineId { get; set; } //sheetID

        public long projectId { get; set; } //rowId

        public string projectName { get; set; }

        [BindProperty]
        public IEnumerable<SelectListItem> intake_options { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yy}", ApplyFormatInEditMode = true)]
        public DateTime dateRequested { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime briefDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime liveDate { get; set; }

        public string projectType { get; set; }

        public string projectDetails { get; set; }

        public string intakeType { get; set; }

        [BindProperty]
        public IEnumerable<SelectListItem> CategoryList { get; set; }

        [BindProperty]
        public IList<string> SelectedCategory { get; set; }

        public string clientGroup { get; set; }

        //[Required(ErrorMessage = "Please enter a number.")]
        //[RegularExpression("^[0-9]*$", ErrorMessage = "Must be a number")]}
        public string clientBudget { get; set; }


        public string clientStakeholder { get; set; }

        //public bool removeProject { get; set; }

        public string am { get; set; }

        public string pm { get; set; }

        public string jira { get; set; }
        //[Required(ErrorMessage = "Please provide a link")]


        //public class DateGreaterThan : ValidationAttribute
        //{
        //    private string _startDatePropertyName;
        //    public DateGreaterThan(string startDatePropertyName)
        //    {
        //        _startDatePropertyName = startDatePropertyName;
        //    }

        //    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        //    {
        //        var propertyInfo = validationContext.ObjectType.GetProperty(_startDatePropertyName);
        //        var propertyValue = propertyInfo.GetValue(validationContext.ObjectInstance, null);

        //        if ((DateTime)value > (DateTime)propertyValue)
        //        {
        //            return ValidationResult.Success;
        //        }
        //        else
        //        {
        //            return new ValidationResult("The end date occurs before the start date");
        //        }
        //    }
        //}
    }

}