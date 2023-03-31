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

        public string staging { get; set; }

        public string box { get; set; }

        public string hours { get; set; }

        public string specs_list { get; set; }

        [Required(ErrorMessage = "Please provide a link")]
        public string wbs { get; set; }

        [BindProperty]
        public IEnumerable<SelectListItem> status_options { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yy}", ApplyFormatInEditMode = true)]
        public DateTime techStart { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yy}", ApplyFormatInEditMode = true)]
        //[DateGreaterThan("startDate")]
        public DateTime techDue { get; set; }

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
}

