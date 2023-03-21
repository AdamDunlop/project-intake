﻿using System;
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
        public DateTime dueDate { get; set; }

        public string type { get; set; }

        [BindProperty]
        public IEnumerable<SelectListItem> type_options { get; set; }

        public string producer { get; set; }

        public string pm { get; set; }

        public string masters { get; set; }

        public string deliverables { get; set; }

        public string description { get; set;  }

    }
}
