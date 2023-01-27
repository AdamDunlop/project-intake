using Microsoft.AspNetCore.Mvc;
using Smartsheetsproject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Smartsheet.Api.Models;
using Smartsheet.Api;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;


namespace Smartsheetsproject.Controllers
{
    public class PDSController : Controller
    {

        public static long sheetId = 4495384983693188;


        [HttpGet]
        public IActionResult List()
        {
            var sheet = LoadSheet(sheetId, initSheet());
            return View(GetRows(sheet));
        }


        [HttpGet]
        public IActionResult Edit(long id)
        {
            PDSModel model = new PDSModel();
            model = GetProjectEdit(id);
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(PDSModel model)
        {
            if (!ModelState.IsValid)
            {
                PDSModel model_lists = GetPickLists(model.project_Id);
                model.lob_options = model_lists.lob_options;
                model.status_options = model_lists.status_options;
                model.SpecsList = model_lists.SpecsList;
                return View(model);
            }
            else
            {
                updateProject(model);
                return RedirectToAction("List");
            }
        }
        [HttpGet]
        public IActionResult Details(long id)
        {
            PDSModel model = new PDSModel();
            model = GetProjectDetails(id);
            return View(model);
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

        public List<PDSModel> GetRows(Sheet sheet)
        {
            List<PDSModel> project_list = new List<PDSModel>();

            foreach (var row in sheet.Rows)
            {
                if (!string.IsNullOrWhiteSpace(row.Cells.ElementAt(0).DisplayValue))
                {
                    PDSModel project = new PDSModel();
                    project.project_Id = (long)row.Id;
                    foreach (var cell in row.Cells)
                    {
                        long columnid = cell.ColumnId.Value;
                        string columnName = sheet.GetColumnById(columnid).Title.ToString();
                        Console.WriteLine("Column Name: " + columnName + " -- Cell Value: " + cell.DisplayValue);
                        switch (columnName)
                        {

                            case "LOB":
                                project.lob = cell.DisplayValue;
                                break;

                            case "Tenrox Code":
                                project.tenroxCode = cell.DisplayValue;
                                break;

                            case "Project":
                                project.projectName = cell.DisplayValue;
                                break;

                            case "Status":
                                project.status = cell.DisplayValue;
                                break;

                            case "Start":
                                if (project.startDate != null)
                                {
                                    project.startDate = Convert.ToDateTime(cell.Value);
                                }
                                    //project.startDate = DateTime.ParseExact( cell.Value.ToString(), "mm,dd,yyyy",null);
                                break;

                            case "Ship":
                                if (project.endDate != null)
                                {
                                    project.endDate = Convert.ToDateTime(cell.Value);
                                }
                                //project.endDate = DateTime.ParseExact(cell.Value.ToString(), "mm,dd,yyyy", null);
                                break;

                            case "Assigned to":
                                project.assignedTo = cell.DisplayValue;
                                break;

                            case "PM":
                                project.pm = cell.DisplayValue;
                                break;

                        }
                    }
                    project_list.Add(project);
                }
            }
            return project_list;
        }

        public PDSModel GetProjectEdit(long row_id)
        {
            PDSModel project = new PDSModel();
            project.project_Id = row_id;
            Sheet sheet = LoadSheet(sheetId, initSheet());

            project.lob_options = Get_lobs_picklist(sheet.GetColumnByIndex(0));
            project.status_options = Get_status_picklist(sheet.GetColumnByIndex(3));
            project.SpecsList = Get_Specs_List(sheet.GetColumnByIndex(17));

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
                            case "LOB":
                                foreach (var item in project.lob_options)
                                {
                                    if (item.Value.ToString() == cell.DisplayValue)
                                    {
                                        item.Selected = true;
                                    }
                                }
                                //project.lob = cell.DisplayValue;
                                break;

                            case "Tenrox Code":
                                project.tenroxCode = cell.DisplayValue;
                                break;

                            case "Project":
                                project.projectName = cell.DisplayValue;
                                break;

                            case "Type of Work":
                                project.typeOfWork = cell.DisplayValue;
                                break;

                            case "Status":
                                foreach (var item in project.status_options)
                                {
                                    if (item.Value.ToString() == cell.DisplayValue)
                                    {
                                        item.Selected = true;
                                    }
                                }
                                break;

                            case "Start":
                                project.startDate = Convert.ToDateTime(cell.Value);
                                break;

                            case "Ship":
                                project.endDate = Convert.ToDateTime(cell.Value);
                                break;

                            //case "PM":
                            //    project.pm = cell.DisplayValue;
                            //    break;

                            //case "Assigned to":
                            //    project.assignedTo = cell.DisplayValue;
                            //    break;

                            case "Collab Deck":
                                project.collabDeck = cell.DisplayValue;
                                break;

                            case "FxF Deck":
                                project.fxfDeck = cell.DisplayValue;
                                break;

                            case "PSDs":
                                project.PSDs = cell.DisplayValue;
                                break;

                            case "Final Delivery":
                                project.finalDeliveryFolder = cell.DisplayValue;
                                break;

                            case "WBS":
                                project.wbs_link = cell.DisplayValue;
                                break;

                            case "Deliverables Tracker":
                                project.deliverables_tracker_link = cell.DisplayValue;
                                break;

                            case "Description":
                                project.description = cell.DisplayValue;
                                break;

                            case "Total Deliverables":
                                project.deliverables = cell.DisplayValue;
                                break;

                            case "Specs":

                                // List<SelectListItem> selectedvalues = new List<SelectListItem>();

                                if (cell.DisplayValue != null)
                                {
                                    List<string> selectedvalues = new List<string>();

                                    var list = cell.DisplayValue.Split(",");

                                    foreach (var spec in list)
                                    {
                                        IEnumerable<SelectListItem> variable = project.SpecsList.Where(x => x.Text.Contains(spec.TrimStart(' ')));

                                        var id = "";

                                        foreach (var i in variable)
                                        {
                                            id = i.Value;
                                        }

                                        //selectedvalues.Add(new SelectListItem { Value = id, Text = spec, Selected = true });
                                        selectedvalues.Add(id);

                                        variable = null;
                                    }
                                    project.SelectedSpecs = selectedvalues;
                                }
                                break;
                        }
                    }
                }
            }
            return project;
        }

        public PDSModel GetProjectDetails(long row_id)
        {
            PDSModel project = new PDSModel();
            project.project_Id = row_id;
            Sheet sheet = LoadSheet(sheetId, initSheet());

            //Get LOB options
            project.lob_options = Get_lobs_picklist(sheet.GetColumnByIndex(0));
            //Get Status options
            project.status_options = Get_status_picklist(sheet.GetColumnByIndex(3));

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
                            

                            case "LOB":
                                project.lob = cell.DisplayValue;
                                break;

                            case "Tenrox Code":
                                project.tenroxCode = cell.DisplayValue;
                                break;

                            case "Project":
                                project.projectName = cell.DisplayValue;
                                break;

                            case "Status":    
                                project.status = cell.DisplayValue;
                                break;

                            case "Type of Work":
                                project.typeOfWork = cell.DisplayValue;
                                break;

                            case "Start":
                                project.startDate = Convert.ToDateTime(cell.Value);
                                break;

                            case "Ship":
                                project.endDate = Convert.ToDateTime(cell.Value);
                                break;

                            case "Assigned to":
                                project.assignedTo = cell.DisplayValue;
                                break;

                            case "PM":
                                project.pm = cell.DisplayValue;
                                break;

                            case "Creative Lead":
                                project.creativeLead = cell.DisplayValue;
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
                                    project.wbs_link = cell.DisplayValue;
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
                                    project.deliverables_tracker_link = cell.DisplayValue;
                                }
                                break;


                            case "Collab Deck":
                                project.collabDeck = cell.DisplayValue;
                                break;

                            case "FxF Deck":
                                project.fxfDeck = cell.DisplayValue;
                                break;

                            case "PSDs":
                                project.PSDs = cell.DisplayValue;
                                break;

                            case "Final Delivery":
                                project.finalDeliveryFolder = cell.DisplayValue;
                                break;

                            case "Description":
                                project.description = cell.DisplayValue;
                                break;

                            case "Total Deliverables":
                                project.deliverables = cell.DisplayValue;
                                break;

                            case "Specs":
                            
                                //TBD
                                break;


                        }
                    }
                }
            }
            return project;
        }

        public void updateProject(PDSModel project)
        {
            SmartsheetClient smartsheet_CL = initSheet();
            Sheet sheet = LoadSheet(sheetId, smartsheet_CL);

            int row_number = 0;
            foreach (var row_b in sheet.Rows)
            {
                if (row_b.Id == project.project_Id)
                {
                    row_number = row_b.RowNumber.Value;
                }
            }

            Row row = sheet.GetRowByRowNumber(row_number);
            var rowToTupdate = new Row();

            var lob_cell = new Cell();
            var tenrox_cell = new Cell();
            var project_cell = new Cell();
            var type_cell = new Cell();
            var status_cell = new Cell();
            var start_cell = new Cell();
            var end_cell = new Cell();
            var assigned_cell = new Cell();
            var collab_cell = new Cell();
            var psds_cell = new Cell();
            var fxf_cell = new Cell();
            var final_delivery_cell = new Cell();
            var pm_cell = new Cell();
            var deliverables_cell = new Cell();
            var description_cell = new Cell();
            var creative_cell = new Cell();
            var wbs_cell = new Cell();
            var deliverables_tracker_cell = new Cell();
            var specs_cell = new Cell();

            foreach (var cell in row.Cells)
            {
                long columnid = cell.ColumnId.Value;
                string columnName = sheet.GetColumnById(columnid).Title.ToString();

                switch (columnName)
                {

                    case "Project":
                        project_cell.ColumnId = columnid;
                        project_cell.Value = project.projectName;
                        break;

                    case "Tenrox Code":
                        tenrox_cell.ColumnId = columnid;
                        tenrox_cell.Value = project.tenroxCode;
                        break;

                    case "LOB":
                        lob_cell.ColumnId = columnid;
                        lob_cell.Value = project.lob;
                        break;

                    case "Status":
                        status_cell.ColumnId = columnid;
                        status_cell.Value = project.status;
                        break;

                    case "Start":
                        start_cell.ColumnId = columnid;
                        start_cell.Value = project.startDate;
                        break;

                    case "Ship":
                        end_cell.ColumnId = columnid;
                        end_cell.Value = project.endDate;
                        break;

                    //case "Type of Work":
                    //    type_cell.ColumnId = columnid;
                    //    type_cell.Value = project.typeOfWork;
                    //    break;

                    //case "Assigned to":
                    //    assigned_cell.ColumnId = columnid;
                    //    assigned_cell.Value = project.assignedTo;
                    //    break;

                    //case "PM":
                    //    pm_cell.ColumnId = columnid;
                    //    pm_cell.Value = project.pm;
                    //    break;

                    case "WBS":
                        wbs_cell.ColumnId = columnid;
                        wbs_cell.Value = project.wbs_link;
                        break;

                    case "Deliverables Tracker":
                        deliverables_tracker_cell.ColumnId = columnid;
                        deliverables_tracker_cell.Value = project.deliverables_tracker_link;
                        break;

                    //case "Collab Deck":
                    //    collab_cell.ColumnId = columnid;
                    //    collab_cell.Value = project.collabDeck;
                    //    break;

                    case "FxF Deck":
                        fxf_cell.ColumnId = columnid;
                        fxf_cell.Value = project.fxfDeck;
                        break;

                    case "PSDs":
                        psds_cell.ColumnId = columnid;
                        psds_cell.Value = project.PSDs;
                        break;

                    case "Final Delivery":
                        final_delivery_cell.ColumnId = columnid;
                        final_delivery_cell.Value = project.finalDeliveryFolder;
                        break;

                    case "Total Deliverables":
                        deliverables_cell.ColumnId = columnid;
                        deliverables_cell.Value = project.deliverables;
                        break;

                    case "Description":
                        description_cell.ColumnId = columnid;
                        description_cell.Value = project.description;
                        break;


                    case "Specs":
                        specs_cell.ColumnId = columnid;
                        ObjectValue objct = null;
                        bool flag = false;
                        //count = project.SelectedSpecs.Count();

                        if (project.SelectedSpecs != null)
                        {

                            foreach (var size in project.SelectedSpecs)

                            {
                                if (size != null)
                                {
                                    if (size.Contains("System.String"))
                                    {
                                        flag = true;
                                    }
                                }
                            }
                            if (flag)
                            {
                                project.SelectedSpecs.RemoveAt(project.SelectedSpecs.Count() - 1);
                            }

                            if (project.SelectedSpecs.Count() == 0) {
                                project.SelectedSpecs.Add("TBD");
                                objct = new MultiPicklistObjectValue(project.SelectedSpecs);

                            }
                            else
                            {
                                objct = new MultiPicklistObjectValue(project.SelectedSpecs);

                            }

                        }
                        specs_cell.ObjectValue = objct;
                        break;

                }
            }

            rowToTupdate = new Row
            {
                Id = project.project_Id,
                Cells = new Cell[] {
                    lob_cell,
                    tenrox_cell,
                    //type_cell,
                    project_cell,
                    status_cell,
                    start_cell,
                    end_cell,
                    wbs_cell,
                    deliverables_tracker_cell,
                    //collab_cell,
                    fxf_cell,
                    psds_cell,
                    final_delivery_cell,
                    deliverables_cell,
                    description_cell,
                    specs_cell
                }
            };

            try
            {
                IList<Row> updatedRow = smartsheet_CL.SheetResources.RowResources.UpdateRows(
                sheet.Id.Value,
                new Row[] { rowToTupdate }
            );
                TempData["Result"] = "Success";
            }
            catch (Exception e)
            {
                Console.WriteLine("Project Update has Failed: " + e.Message + e.Data.ToString());
                TempData["Result"] = "Failed";
            };
        }

        public PDSModel GetPickLists(long row_id)
        {
            PDSModel project = new PDSModel();
            project.project_Id = row_id;
            Sheet sheet = LoadSheet(sheetId, initSheet());

            project.lob_options = Get_lobs_picklist(sheet.GetColumnByIndex(0));
           
            project.status_options = Get_status_picklist(sheet.GetColumnByIndex(3));

            project.SpecsList = Get_Specs_List(sheet.GetColumnByIndex(17));

            return project;
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
        public ICollection<SelectListItem> Get_Specs_List(Column specs_col)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            //int cont = 0;
            foreach (var spec in specs_col.Options)
            {
                //cont++;
                //list.Add(new SelectListItem { Text = spec, Value = cont.ToString()});
                list.Add(new SelectListItem { Text = spec, Value = spec });
            }
            return list;
        }
        public IEnumerable<SelectListItem> Get_lobs_picklist(Column lob_col)
        {
            List<SelectListItem> options = new List<SelectListItem>();
            foreach (var lob in lob_col.Options)
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

    }

}
