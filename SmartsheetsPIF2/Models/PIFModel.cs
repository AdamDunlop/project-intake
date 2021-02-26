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
        public string projectName { get; set; }

        public string lob { get; set; }
        [BindProperty]
        public IEnumerable<SelectListItem> lob_options { get; set; }
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
        public DateTime startDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime endDate { get; set; }
        public string team { get; set; }
        public string collabDeck { get; set; }
        public string finalDeck { get; set; }
        public string photoShop { get; set; }
        public string deliverables { get; set; }
        public string deliverablesTracker { get; set; }

    }

}
