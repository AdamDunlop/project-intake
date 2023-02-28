using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Smartsheetsproject.Models
{
    public class DesignModel
    {

        public long pipelineId { get; set; }

        public long rowId { get; set; }

        public string intakeType { get; set; }

        public string projectName { get; set; }

        public string lob { get; set; }

        public string tenrox { get; set; }

        [BindProperty]
        public IEnumerable<SelectListItem> lob_options { get; set; }

        public string status { get; set; }

        [BindProperty]
        public IEnumerable<SelectListItem> status_options { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yy}", ApplyFormatInEditMode = true)]
        public DateTime startDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yy}", ApplyFormatInEditMode = true)]
        [DateGreaterThan("startDate")]
        public DateTime dueDate { get; set; }

        public string assignedTo { get; set; }

        [Required(ErrorMessage = "Please enter the Box embed Link")]
        public string box { get; set; }

        [Required(ErrorMessage = "Please enter the Figma embed Link")]
        public string figma { get; set; }


        [Required(ErrorMessage = "Please enter the wbs Link")]
        public string wbs { get; set; }


    }

    public class DateGreaterThan : ValidationAttribute
    {
        private string _startDatePropertyName;
        public DateGreaterThan(string startDatePropertyName)
        {
            _startDatePropertyName = startDatePropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var propertyInfo = validationContext.ObjectType.GetProperty(_startDatePropertyName);
            var propertyValue = propertyInfo.GetValue(validationContext.ObjectInstance, null);

            if ((DateTime)value > (DateTime)propertyValue)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Cannot set Due Date before the Start Date.");
            }
        }
    }
}
