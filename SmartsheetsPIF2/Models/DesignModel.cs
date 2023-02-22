﻿using System;
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

        public string projectName { get; set; }

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
        public DateTime dueDate { get; set; }

        //[BindProperty]
        //public IEnumerable<SelectListItem> SpecsList { get; set; }

        [BindProperty]
        public IList<string> SelectedSpecs { get; set; }

        public string lob { get; set; }

        public string assignedTo { get; set; }

        public string jira { get; set; }

        [Required(ErrorMessage = "Please enter the wbs embed link.")]
        public string wbs { get; set; }

        [Required(ErrorMessage = "Please enter the Box link.")]
        public string box { get; set; }

        [Required(ErrorMessage = "Please enter the Figma link.")]
        public string figma { get; set; }

        public string am { get; set; }

        public string pm { get; set; }
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
                return new ValidationResult("The end date occurs before the start date");
            }
        }
    }
}
