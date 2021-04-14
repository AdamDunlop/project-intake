using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SmartsheetsPIF.Models
{
    public class SocialModel
    {
        public long pif_Id { get; set; }

        public string projectName { get; set; }

        public string lob { get; set; }

        [BindProperty]
        public IEnumerable<SelectListItem> lob_options { get; set; }

        //public string tenroxCode { get; set; }

        public string status { get; set; }

        [BindProperty]
        public IEnumerable<SelectListItem> status_options { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yy}", ApplyFormatInEditMode = true)]
        public DateTime startDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yy}", ApplyFormatInEditMode = true)]
        public DateTime endDate { get; set; }

        public string type { get; set; }

        [BindProperty]
        public IEnumerable<SelectListItem> type_options { get; set; }

        public string producer { get; set; }

        public string pm { get; set; }

        public string masters { get; set; }

        public string deliverables { get; set; }

        public string notes { get; set;  }

    }
}
