using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Smartsheetsproject.Models
{
    public class VideoModel
    {

        public string intakeType { get; set; }

        public long pipelineId { get; set; }

        public string projectName { get; set; }

        public string lob { get; set; }

        public string tenrox { get; set; }

        public string status { get; set; }

        public string assignedTo { get; set; }

        public string pm { get; set; }

        public string am { get; set; }

        public string jira { get; set; }

        public string frameio { get; set; }

        public string script { get; set; }

        public string hours { get; set; }

        public string specs_list { get; set; }

        //[Required(ErrorMessage = "Please provide a link")]
        public string PSDs { get; set; }

        //[Required(ErrorMessage = "Please provide a link")]
        public string box { get; set; }

        [Required(ErrorMessage = "Please provide a link")]
        public string wbs { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yy}", ApplyFormatInEditMode = true)]
        public DateTime startDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yy}", ApplyFormatInEditMode = true)]
        [DateGreaterThan("startDate")]
        public DateTime dueDate { get; set; }
   
        [BindProperty]
        public IEnumerable<SelectListItem> lob_options { get; set; }

        [BindProperty]
        public IEnumerable<SelectListItem> status_options { get; set; }

        [BindProperty]
        public IEnumerable<SelectListItem> SpecsList { get; set; }

        [BindProperty]
        public IList<string> SelectedSpecs { get; set; }
    }

}
