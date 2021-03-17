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
        [Required(ErrorMessage = "Please provide a name.")]
        public string projectName { get; set; }
        public string lob { get; set; }
        [BindProperty]
        public IEnumerable<SelectListItem> lob_options { get; set; }
        [Required(ErrorMessage = "Please provide the Tenrox Code.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Tenrox Code must be numeric")]
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
        [Required(ErrorMessage = "Please provide a start date.")]
        public DateTime startDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Please provide a ship date.")]
        public DateTime endDate { get; set; }
        public string team { get; set; }
        [Required(ErrorMessage = "Please provide a link.")]
        public string Deck { get; set; }
        [Required(ErrorMessage = "Please provide a link.")]
        public string PSDs { get; set; }
        [Required(ErrorMessage = "Please provide a link.")]
        public string boxFolder { get; set; }
        [Required(ErrorMessage = "Please provide a link.")]
        public string finalDeliveryFolder { get; set; }
        [Required(ErrorMessage = "Please provide the details.")]
        public string Specs { get; set; }
        [Required(ErrorMessage = "Please provide a link.")]
        public string wbs_link { get; set; }
        [Required(ErrorMessage = "Please provide a link.")]
        public string deliverables_tracker_link { get; set; }
        [Required(ErrorMessage = "Please enter a number.")]
        public string numberOfSets { get; set; }
        [Required(ErrorMessage = "Please enter a number.")]
        public string animatedPerSet { get; set; }
        [Required(ErrorMessage = "Please enter a number.")]
        public string staticPerSet { get; set; }
    }

}
