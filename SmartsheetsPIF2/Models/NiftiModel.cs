using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Smartsheetsproject.Models
{
  public class NiftiModel  {


        public long pipelineId { get; set; } //sheetID

        public long projectId { get; set; } //rowId

        public string projectName { get; set; }

        [BindProperty]
        public IEnumerable<SelectListItem> intake_options { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yy}", ApplyFormatInEditMode = true)]
        public DateTime dateRequested { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime briefDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime liveDate { get; set; }

        public string type { get; set; }

        public string intakeType { get; set; }

        //public string category { get; set; }

        [BindProperty]
        public IEnumerable<SelectListItem> CategoryList { get; set; }

        [BindProperty]
        public IList<string> SelectedCategory { get; set; }



        public string clientGroup { get; set; }

        public string clientBudget { get; set; }

        public string clientStakeholder { get; set; }
    }

}