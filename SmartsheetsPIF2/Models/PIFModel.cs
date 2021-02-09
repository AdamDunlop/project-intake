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
        public string lob { get; set; }
        [BindProperty]
        public IEnumerable<SelectListItem> lob_options { get; set; }
        public string projectName { get; set; }
        public string status { get; set; }
        [BindProperty]
        public IEnumerable<SelectListItem> status_options { get; set; }
        public string producer { get; set; }
        public string requestType { get; set; }
        public string team { get; set; }
        [BindProperty]
        public IEnumerable<SelectListItem> team_options { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime startDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime endDate { get; set; }

        [DataType(DataType.Url)]
        [DisplayFormat(DataFormatString = "PIF", ApplyFormatInEditMode = true)]
        public string pif_link { get; set; }
    }
}
