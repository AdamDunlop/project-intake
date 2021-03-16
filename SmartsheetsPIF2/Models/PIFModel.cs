using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartsheetsPIF.Models
{
    public class PIFModel
    {
        public long pif_Id { get; set; }
        [Required(ErrorMessage = "Please enter the Project's Name.")]
        public string projectName { get; set; }
        public string lob { get; set; }
        [BindProperty]
        public IEnumerable<SelectListItem> lob_options { get; set; }
        [Required(ErrorMessage = "TBD.")]
        public string tenroxCode { get; set; }
        public string status { get; set; }
        [BindProperty]
        public IEnumerable<SelectListItem> status_options { get; set; }
        public string producer { get; set; }
        public string requestType { get; set; }
        [BindProperty]
        public IEnumerable<SelectListItem> team_options { get; set; }
        [Required(ErrorMessage = "TBD.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime startDate { get; set; }
        [Required(ErrorMessage = "TBD.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime endDate { get; set; }
        [Required(ErrorMessage = "TBD.")]
        public string team { get; set; }
        [Required(ErrorMessage = "TBD.")]
        public string Deck { get; set; }
        [Required(ErrorMessage = "TBD.")]
        public string PSDs { get; set; }
        [Required(ErrorMessage = "TBD.")]
        public string boxFolder { get; set; }
        [Required(ErrorMessage = "TBD.")]
        public string finalDeliveryFolder { get; set; }
        [Required(ErrorMessage = "TBD.")]
        public string Specs { get; set; }
        [Required(ErrorMessage = "TBD.")]
        public string wbs_link { get; set; }
        [Required(ErrorMessage = "TBD.")]
        public string deliverables_tracker_link { get; set; }
        [Required(ErrorMessage = "TBD.")]
        public string numberOfSets { get; set; }
        [Required(ErrorMessage = "TBD.")]
        public string animatedPerSet { get; set; }
        [Required(ErrorMessage = "TBD.")]
        public string staticPerSet { get; set; }
    }

}
