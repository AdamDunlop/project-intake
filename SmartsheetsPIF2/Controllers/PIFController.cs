using Microsoft.AspNetCore.Mvc;
using SmartsheetsPIF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Smartsheet.Api.Models;
using Smartsheet.Api;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace SmartsheetsPIF2.Controllers
{
    public class PIFController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        public static long sheetId = 508222156105604;

        [HttpGet]
        public IActionResult ListPIFs()
        {
            var sheet = LoadSheet(sheetId, initSheet());
            return View(GetRows(sheet));
        }

        [HttpGet]
        public IActionResult Details(long id)
        {
            PIFModel model = new PIFModel();
            model = GetProjectDetails(id);
            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(long id)
        {
            PIFModel model = new PIFModel();
            model = GetProjectEdit(id);
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(PIFModel model)
        {
            if (!ModelState.IsValid)
            {
                PIFModel model_lists = GetPickLists(model.pif_Id);
                model.lob_options = model_lists.lob_options;
                model.status_options = model_lists.status_options;
                model.team_options = model_lists.team_options;
                return View(model);
                //return RedirectToAction("Edit", new { id = model.pif_Id });
            }
            else 
            {
                updateProject(model);
                //return View("Index");
                TempData["Success"] = "Success";
                return RedirectToAction("ListPIFs");
            }
        }
        public SmartsheetClient initSheet()
        {
            // Initialize client
            SmartsheetClient smartsheet_CL = new SmartsheetBuilder().SetAccessToken("voic91jwhahv2mws2cow5frz96")
                // TODO: Set your API access in environment variable SMARTSHEET_ACCESS_TOKEN or else here
                .Build();
            return smartsheet_CL;
        }
        public Sheet LoadSheet(long sheetId, SmartsheetClient smartsheet_CL)
        {
            // Load the entire sheet
            var sheet = smartsheet_CL.SheetResources.GetSheet(
                sheetId,           // long sheetId
                null,                       // IEnumerable<SheetLevelInclusion> includes
                null,                       // IEnumerable<SheetLevelExclusion> excludes
                null,                       // IEnumerable<long> rowIds
                null,                       // IEnumerable<int> rowNumbers
                null,                       // IEnumerable<long> columnIds
                null,                       // Nullable<long> pageSize
                null                        // Nullable<long> page
            );
            Console.WriteLine("Loaded " + sheet.Rows.Count + " rows from sheet: " + sheet.Name);
            return sheet;
        }
        public List<PIFModel> GetRows(Sheet sheet)
        {
            List<PIFModel> pif_list = new List<PIFModel>();
            //Get_lobs_picklist(sheet.GetColumnByIndex(0));

            foreach (var row in sheet.Rows)
            {
                if (!string.IsNullOrWhiteSpace(row.Cells.ElementAt(0).DisplayValue)) {
                    PIFModel pif = new PIFModel();
                    pif.pif_Id = (long)row.Id;
                    foreach (var cell in row.Cells)
                    {
                        long columnid = cell.ColumnId.Value;
                        string columnName = sheet.GetColumnById(columnid).Title.ToString();
                        Console.WriteLine("Column Name: " + columnName + " -- Cell Value: " + cell.DisplayValue);
                        switch (columnName)
                        {

                            case "Project":
                                pif.projectName = cell.DisplayValue;
                                break;

                            case "LOB":
                                pif.lob = cell.DisplayValue;
                                break;

                            case "Tenrox Codes":
                                pif.tenroxCode = cell.DisplayValue;
                                break;

                            case "Status":
                                pif.status = cell.DisplayValue;
                                break;

                            case "Start":
                                pif.startDate = Convert.ToDateTime(cell.Value);
                                //pif.startDate = DateTime.ParseExact( cell.Value.ToString(), "mm,dd,yyyy",null);
                                break;

                            case "Ship":
                                pif.endDate = Convert.ToDateTime(cell.Value);
                                //pif.endDate = DateTime.ParseExact(cell.Value.ToString(), "mm,dd,yyyy", null);
                                break;

                            case "Team":
                                pif.team = cell.DisplayValue;
                                break;                           
                        }
                    }
                    pif_list.Add(pif);
                }
            }
            return pif_list;
        }
        public PIFModel GetProjectEdit(long row_id)
        {
            PIFModel pif = new PIFModel();
            pif.pif_Id = row_id;
            Sheet sheet = LoadSheet(sheetId, initSheet());

            //Get LOB options
            pif.lob_options = Get_lobs_picklist(sheet.GetColumnByIndex(0));
            //Get Status options
            pif.status_options = Get_status_picklist(sheet.GetColumnByIndex(3));
            //Get Team options
            pif.team_options = Get_status_picklist(sheet.GetColumnByIndex(16));
            foreach (var row in sheet.Rows)
            {
                if (row.Id == row_id) {
                    foreach (var cell in row.Cells)
                    {
                        long columnid = cell.ColumnId.Value;
                        string columnName = sheet.GetColumnById(columnid).Title.ToString();
                        Console.WriteLine("Column Name: " + columnName + " -- Cell Value: " + cell.DisplayValue);
                        switch (columnName)
                        {                            
                            case "Project":
                                pif.projectName = cell.DisplayValue;
                                break;

                            case "LOB":
                                foreach (var item in pif.lob_options)
                                {
                                    if (item.Value.ToString() == cell.DisplayValue)
                                    {
                                        //pif.lob_options.
                                        item.Selected = true;
                                    }
                                }
                                //pif.lob = cell.DisplayValue;
                                break;

                            case "Tenrox Code":
                                pif.tenroxCode = cell.DisplayValue;
                                break;

                            case "Status":
                                pif.status = cell.DisplayValue;
                                break;

                            case "Start":
                                pif.startDate = Convert.ToDateTime(cell.Value);
                                break;

                            case "Ship":
                                pif.endDate = Convert.ToDateTime(cell.Value);
                                break;

                            case "PM":
                                pif.producer = cell.DisplayValue;
                                break;

                            case "Type":
                                pif.requestType = cell.DisplayValue;
                                break;

                            case "Team":
                                pif.team = cell.DisplayValue;
                                break;

                            case "Deck":
                                pif.Deck = cell.DisplayValue;
                                break;

                            case "Final Delivery":
                                pif.finalDeliveryFolder = cell.DisplayValue;
                                break;

                            case "PSDs":
                                pif.PSDs = cell.DisplayValue;
                                break;

                            case "Box Folder":
                                pif.boxFolder = cell.DisplayValue;
                                break;

                            case "WBS":
                                pif.wbs_link = cell.DisplayValue;
                                break;

                            case "Deliverables Tracker":
                                pif.deliverables_tracker_link = cell.DisplayValue;
                                break;

                            case "Specs":
                                pif.Specs = cell.DisplayValue;
                                break;

                            case "Number Of Sets":
                                pif.numberOfSets = cell.DisplayValue;
                                break;

                            case "Animated Per Set":
                                pif.animatedPerSet = cell.DisplayValue;
                                break;

                            case "Static Per Set":
                                pif.staticPerSet = cell.DisplayValue;
                                break;

                        }
                    }
                }
            }
            return pif;
        }

        public PIFModel GetProjectDetails(long row_id)
        {
            PIFModel pif = new PIFModel();
            pif.pif_Id = row_id;
            Sheet sheet = LoadSheet(sheetId, initSheet());

            //Get LOB options
            pif.lob_options = Get_lobs_picklist(sheet.GetColumnByIndex(0));
            //Get Status options
            pif.status_options = Get_status_picklist(sheet.GetColumnByIndex(3));
            //Get Team options
            pif.team_options = Get_status_picklist(sheet.GetColumnByIndex(16));

            foreach (var row in sheet.Rows)
            {
                if (row.Id == row_id)
                {
                    foreach (var cell in row.Cells)
                    {
                        long columnid = cell.ColumnId.Value;
                        string columnName = sheet.GetColumnById(columnid).Title.ToString();
                        Console.WriteLine("Column Name: " + columnName + " -- Cell Value: " + cell.DisplayValue);
                        switch (columnName)
                        {
                            case "Project":
                                pif.projectName = cell.DisplayValue;
                                break;

                            case "LOB":
                                pif.lob = cell.DisplayValue;
                                break;

                            case "Tenrox Code":
                                pif.tenroxCode = cell.DisplayValue;
                                break;

                            case "Status":
                                pif.status = cell.DisplayValue;
                                break;

                            case "Start":
                                pif.startDate = Convert.ToDateTime(cell.Value);
                                //pif.startDate = DateTime.ParseExact( cell.Value.ToString(), "mm,dd,yyyy",null);
                                break;

                            case "Ship":
                                pif.endDate = Convert.ToDateTime(cell.Value);
                                //pif.endDate = DateTime.ParseExact(cell.Value.ToString(), "mm,dd,yyyy", null);
                                break;

                            case "PM":
                                pif.producer = cell.DisplayValue;
                                break;

                            case "Type":
                                pif.requestType = cell.DisplayValue;
                                break;

                            case "Team":
                                pif.team = cell.DisplayValue;
                                break;

                            case "Deck":
                                pif.Deck = cell.DisplayValue;
                                break;

                            case "Final Delivery":
                                pif.finalDeliveryFolder = cell.DisplayValue;
                                break;

                            case "PSDs":
                                pif.PSDs = cell.DisplayValue;
                                break;

                            case "Box Folder":
                                pif.boxFolder = cell.DisplayValue;
                                break;

                            case "Specs":
                                pif.Specs = cell.DisplayValue;
                                break;

                            case "Number Of Sets":
                                pif.numberOfSets = cell.DisplayValue;
                                break;

                            case "Animated Per Set":
                                pif.animatedPerSet = cell.DisplayValue;
                                break;

                            case "Static Per Set":
                                pif.staticPerSet = cell.DisplayValue;
                                break;


                            case "WBS":
                                if (cell.Value != null)
                                {
                                    string[] wbs = cell.DisplayValue.Split(" ");
                                    foreach (var item in wbs)
                                    {
                                        if (item.Contains("https:"))
                                        {
                                            wbs = item.Split('"');
                                            foreach (var url in wbs)
                                            {
                                                if (url.Contains("https:"))
                                                {
                                                    cell.DisplayValue = url.Trim('"'); ;
                                                }
                                            }
                                        }
                                    }
                                    pif.wbs_link = cell.DisplayValue;
                                }
                                break;

                            case "Deliverables Tracker":

                                if (cell.Value != null)
                                {
                                    string[] tracker = cell.DisplayValue.Split(" ");
                                    foreach (var item in tracker)
                                    {
                                        if (item.Contains("https:"))
                                        {
                                            tracker = item.Split('"');
                                            foreach (var url in tracker)
                                            {
                                                if (url.Contains("https:"))
                                                {
                                                    cell.DisplayValue = url.Trim('"'); ;
                                                }
                                            }
                                        }
                                    }
                                    pif.deliverables_tracker_link = cell.DisplayValue;
                                }
                                break;

                        }
                    }
                }
            }
            return pif;
        }

        public void updateProject(PIFModel pif)
        {
            SmartsheetClient smartsheet_CL = initSheet();
            Sheet sheet = LoadSheet(sheetId, smartsheet_CL);

            int row_number = 0;
            foreach (var row_b in sheet.Rows)
            {
                if (row_b.Id == pif.pif_Id) 
                {
                    row_number = row_b.RowNumber.Value;
                }
            }

            Row row = sheet.GetRowByRowNumber(row_number);
            var rowToTupdate = new Row ();

            var lob_cell = new Cell();
            var project_cell = new Cell();
            var status_cell = new Cell();
            var tenrox_cell = new Cell();
            var start_cell = new Cell();
            var end_cell = new Cell();
            var team_cell = new Cell();
            var collab_deck_cell = new Cell();
            var box_folder_cell = new Cell();
            var psds_cell = new Cell();
            var final_delivery_cell = new Cell();
            var wbs_cell = new Cell();
            var deliverables_t_cell = new Cell();
            var specs_cell = new Cell();
            var number_of_sets_cell = new Cell();
            var animated_per_set_cell = new Cell();
            var static_per_set_cell = new Cell();


            foreach (var cell in row.Cells)
            {
                long columnid = cell.ColumnId.Value;
                string columnName = sheet.GetColumnById(columnid).Title.ToString();
               
                switch (columnName)
                {

                    case "Project":
                        project_cell.ColumnId = columnid;
                        project_cell.Value = pif.projectName;
                        break;

                    case "LOB":
                        lob_cell.ColumnId = columnid;
                        lob_cell.Value = pif.lob;
                        break;

                    case "Tenrox Code":
                        tenrox_cell.ColumnId = columnid;
                        tenrox_cell.Value = pif.tenroxCode;
                        break;

                    case "Status":
                        status_cell.ColumnId = columnid;
                        status_cell.Value = pif.status;
                        break;

                    case "Start":
                        start_cell.ColumnId = columnid;
                        start_cell.Value = pif.startDate;
                        break;

                    case "Ship":
                        end_cell.ColumnId = columnid;
                        end_cell.Value = pif.endDate;
                        break;

                    case "Team":
                        team_cell.ColumnId = columnid;
                        team_cell.Value = pif.team;
                        break;

                    case "Box Folder":
                        box_folder_cell.ColumnId = columnid;
                        box_folder_cell.Value = pif.boxFolder;
                        break;

                    case "Deck":
                        collab_deck_cell.ColumnId = columnid;
                        collab_deck_cell.Value = pif.Deck;
                        //Uri url = new Uri(collab_cell.Value.ToString());
                        //collab_cell.Hyperlink = ;
                        //Console.WriteLine("URL: " + collab_cell.Hyperlink);
                        break;

                    case "PSDs":
                        psds_cell.ColumnId = columnid;
                        psds_cell.Value = pif.PSDs;
                        break;

                    case "Final Delivery":
                        final_delivery_cell.ColumnId = columnid;
                        final_delivery_cell.Value = pif.finalDeliveryFolder;
                        break;

                    case "Specs":
                        specs_cell.ColumnId = columnid;
                        specs_cell.Value = pif.Specs;
                        break;

                    case "Number Of Sets":
                        number_of_sets_cell.ColumnId = columnid;
                        number_of_sets_cell.Value = pif.numberOfSets;
                        break;

                    case "Animated Per Set":
                        animated_per_set_cell.ColumnId = columnid;
                        animated_per_set_cell.Value = pif.animatedPerSet;
                        break;

                    case "Static Per Set":
                        static_per_set_cell.ColumnId = columnid;
                        static_per_set_cell.Value = pif.staticPerSet;
                        break;

                    case "Deliverables Tracker":
                        deliverables_t_cell.ColumnId = columnid;
                        deliverables_t_cell.Value = pif.deliverables_tracker_link;
                        break;

                    case "WBS":
                        wbs_cell.ColumnId = columnid;
                        wbs_cell.Value = pif.wbs_link;
                        break;

                   
                }
            }

            rowToTupdate = new Row
            {
                Id = pif.pif_Id,
                Cells = new Cell[] {
                    lob_cell,
                    project_cell,
                    status_cell,
                    start_cell,
                    end_cell,
                    team_cell,
                    tenrox_cell,
                    collab_deck_cell,
                    box_folder_cell,
                    psds_cell,
                    final_delivery_cell,
                    wbs_cell,
                    deliverables_t_cell,
                    specs_cell,
                    number_of_sets_cell,
                    animated_per_set_cell,
                    static_per_set_cell
                }
            };

            try
            {
                IList<Row> updatedRow = smartsheet_CL.SheetResources.RowResources.UpdateRows(
                sheet.Id.Value,
                new Row[] { rowToTupdate }
            );
            }
            catch (Exception e)
            {
                Console.WriteLine("Update row Failed: " + e.Message);
            };
        }


        public PIFModel GetPickLists(long row_id) {
            PIFModel pif = new PIFModel();
            pif.pif_Id = row_id;
            Sheet sheet = LoadSheet(sheetId, initSheet());

            //Get LOB options
            pif.lob_options = Get_lobs_picklist(sheet.GetColumnByIndex(0));
            //Get Status options
            pif.status_options = Get_status_picklist(sheet.GetColumnByIndex(3));
            //Get Team options
            pif.team_options = Get_status_picklist(sheet.GetColumnByIndex(16));

            return pif;
        }
        public void runthroughallsheets()
        {
            /*
            // List all sheets
            PaginatedResult<Sheet> sheets = smartsheet_CL.SheetResources.ListSheets(
                null,               // IEnumerable<SheetInclusion> includes
                null,               // PaginationParameters
                null                // Nullable<DateTime> modifiedSince = null
            );
            //Console.WriteLine("Found " + sheets.TotalCount + " sheets");
            //iteration through all Sheet IDs
            for (int i = 0; sheets.TotalCount > i; i++) {
                ///Console.WriteLine("Loading sheet position: " + i);
                //Console.WriteLine("Loading sheet id: " + (long)sheets.Data[i].Id);
                GetSheet((long)sheets.Data[i].Id,smartsheet);
            }
            long sheetId = 1478730146178948; //Display TEST
            */
        }
        public IEnumerable<SelectListItem> Get_lobs_picklist(Column lob_col)
        {
            List<SelectListItem> options = new List<SelectListItem>();
            foreach(var lob in lob_col.Options)
            {
                options.Add(new SelectListItem { Text = lob, Value = lob });
            }
            return options;
        }
        public IEnumerable<SelectListItem> Get_status_picklist(Column status_col)
        {
            List<SelectListItem> options = new List<SelectListItem>();
            foreach (var status in status_col.Options)
            {
                options.Add(new SelectListItem { Text = status, Value = status });
            }
            return options;
        }
        public IEnumerable<SelectListItem> Get_team_picklist(Column team_col)
        {
            List<SelectListItem> options = new List<SelectListItem>();
            foreach (var team in team_col.Options)
            {
                options.Add(new SelectListItem { Text = team, Value = team });
            }
            return options;
        }
    }
}
