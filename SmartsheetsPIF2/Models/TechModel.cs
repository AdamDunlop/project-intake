using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Smartsheetsproject.Models
{
    public class TechModel
    {

        public string intakeType { get; set; }

        public long pipelineId { get; set; }

        public string projectName { get; set; }

        public string lob { get; set; }

        public string tenrox { get; set; }

        public string assignedTo { get; set; }

        public string pm { get; set; }

        public string am { get; set; }

        public string status { get; set; }

        public string jira { get; set; }

        public string figma { get; set; }

        public string box { get; set; }

        public string specs_list { get; set; }

        [Required(ErrorMessage = "Please provide a link")]
        public string wbs { get; set; }

        [BindProperty]
        public IEnumerable<SelectListItem> status_options { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Please provide a start date")]
        public DateTime startDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Please provide an end date")]
        [DateGreaterThan("startDate")]
        public DateTime dueDate { get; set; }

        [Required(ErrorMessage = "Please provide a link")]
        public string FxF { get; set; }

        [Required(ErrorMessage = "Please provide a link")]
        public string PSDs { get; set; }


        ////[Required(ErrorMessage = "Please provide the project's details")]
        //[StringLength(500, MinimumLength = 3, ErrorMessage = "Please provide more information or N/A")]
        //public string description { get; set; }

    

        [BindProperty]
        public IEnumerable<SelectListItem> lob_options { get; set; }

        [BindProperty]
        public IEnumerable<SelectListItem> SpecsList { get; set; }

        [BindProperty]
        public IList<string> SelectedSpecs { get; set; }
        //[Required(ErrorMessage = "Please enter a number.")]
        //[RegularExpression("^[0-9]*$", ErrorMessage = "Must be a number")]
        //public string numberOfSets { get; set; }

    }

    ///*Validators*/
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

