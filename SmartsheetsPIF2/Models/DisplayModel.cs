using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartsheetsPIF.Models
{
    public class DisplayModel
    {
        public long pif_Id { get; set; }
        [Required(ErrorMessage = "Please enter the name")]

        public string projectName { get; set; }

        public string lob { get; set; }

        [BindProperty]
        public IEnumerable<SelectListItem> lob_options { get; set; }

        [Required(ErrorMessage = "Please provide the Tenrox Code")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Tenrox Code must be numeric")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "Must be 9 characters")]
        public string tenroxCode { get; set; }

        public string status { get; set; }

        [BindProperty]
        public IEnumerable<SelectListItem> status_options { get; set; }

        public string producer { get; set; }

        public string requestType { get; set; }

        [BindProperty]
        public IEnumerable<SelectListItem> team_options { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Please provide a start date")]
        public DateTime startDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Please provide an end date")]
        [DateGreaterThan("startDate")]
        public DateTime endDate { get; set; }

        public string team { get; set; }
        [Required(ErrorMessage = "Please provide a link")]

        public string Deck { get; set; }
        [Required(ErrorMessage = "Please provide a link")]

        public string PSDs { get; set; }
        [Required(ErrorMessage = "Please provide a link")]

        public string boxFolder { get; set; }
        [Required(ErrorMessage = "Please provide a link")]

        public string finalDeliveryFolder { get; set; }

        [Required(ErrorMessage = "Please provide the project's details")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Please provide more information or N/A")]

        public string Specs { get; set; }

        [BindProperty]
        public IEnumerable<SelectListItem> SpecsList { get; set; }

        //[BindProperty]
        //public IEnumerable<SelectListItem>  SelectedSpecs { get; set; }

        [BindProperty]
        public IList<string> SelectedSpecs { get; set; }

        [Required(ErrorMessage = "Please provide a link")]
        public string wbs_link { get; set; }

        [Required(ErrorMessage = "Please provide a link")]
        public string deliverables_tracker_link { get; set; }

        [Required(ErrorMessage = "Please enter a number.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Must be a number")]
        public string numberOfSets { get; set; }

        [Required(ErrorMessage = "Please enter a number.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Must be a number")]
        public string animatedPerSet { get; set; }

        [Required(ErrorMessage = "Please enter a number.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Must be a number")]
        public string staticPerSet { get; set; }
    }

    /*Validators*/
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

