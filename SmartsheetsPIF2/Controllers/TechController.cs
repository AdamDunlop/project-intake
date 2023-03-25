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

namespace Smartsheetsproject2.Controllers
{
    public class TechController : Controller
    {

        public static long sheetId = 4277161877104516;

        [HttpGet]
        public IActionResult List()
        {
            var sheet = LoadSheet(sheetId, initSheet());
            return View(GetRows(sheet));
        }

        [HttpGet]
        public IActionResult Details(long id)
        {
            TechModel model = new TechModel();
            model = GetProjectDetails(id);
            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(long id)
        {
            TechModel model = new TechModel();
            model = GetProjectEdit(id);
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(TechModel model)
        {
            //if (!ModelState.IsValid)
            //{
            //    TechModel model_lists = GetPickLists(model.pipelineId);
            //    model.lob_options = model_lists.lob_options;
            //    model.status_options = model_lists.status_options;
            //    model.team_options = model_lists.team_options;
            //    model.SpecsList = model_lists.SpecsList;
            //    return View(model);
            //    //return RedirectToAction("Edit", new { id = model.pipelineId });
            //}
            //else
            //{
                updateProject(model);      
                return RedirectToAction("List");
            //}
        }
        public SmartsheetClient initSheet()
        {
            // Initialize client
            SmartsheetClient smartsheet_CL = new SmartsheetBuilder().SetAccessToken("x5QRZ9m5hrjG4xdlnnHbQVcxHZnrC37oYPpO8")
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
                null                        
            );
            Console.WriteLine("Loaded " + sheet.Rows.Count + " rows from sheet: " + sheet.Name);
            return sheet;
        }
        public List<TechModel> GetRows(Sheet sheet)
        {
            List<TechModel> project_list = new List<TechModel>();

            foreach (var row in sheet.Rows)
            {
                if (!string.IsNullOrWhiteSpace(row.Cells.ElementAt(0).DisplayValue))
                {
                    TechModel project = new TechModel();
                    project.pipelineId = (long)row.Id;
                    foreach (var cell in row.Cells)
                    {
                        long columnid = cell.ColumnId.Value;
                        string columnName = sheet.GetColumnById(columnid).Title.ToString();
                        Console.WriteLine("Column Name: " + columnName + " -- Cell Value: " + cell.DisplayValue);
                        switch (columnName)
                        {

                            case "Project Name":
                                project.projectName = cell.DisplayValue;
                                break;

                            case "LOB":
                                project.lob = cell.DisplayValue;
                                break;

                            case "Tenrox":
                                project.tenrox = cell.DisplayValue;
                                break;

                            case "Status":
                                project.status = cell.DisplayValue;
                                break;

                            case "Start Date":
                                if (project.startDate != null) 
                                {
                                    project.startDate = Convert.ToDateTime(cell.Value);
                                }
                                
                                //project.startDate = DateTime.ParseExact( cell.Value.ToString(), "mm,dd,yyyy",null);
                                break;

                            case "Due Date":
                                if (project.dueDate != null) 
                                {
                                    project.dueDate = Convert.ToDateTime(cell.Value);
                                }                                
                                //project.endDate = DateTime.ParseExact(cell.Value.ToString(), "mm,dd,yyyy", null);
                                break;

                    
                        }
                    }
                    project_list.Add(project);
                }
            }
            return project_list;
        }
        public TechModel GetProjectEdit(long row_id)
        {
            TechModel project = new TechModel();
            project.pipelineId = row_id;
            Sheet sheet = LoadSheet(sheetId, initSheet());

            ////Get LOB options
            //project.lob_options = Get_lobs_picklist(sheet.GetColumnByIndex(0));
            ////Get Status options
            //project.status_options = Get_status_picklist(sheet.GetColumnByIndex(3));
            
            ////Get Specs List
            //project.SpecsList = Get_Specs_List(sheet.GetColumnByIndex(11));

            ////Get Team options
            //project.team_options = Get_team_picklist(sheet.GetColumnByIndex(16));

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
                            case "Project Name":
                                project.projectName = cell.DisplayValue;
                                break;

                            case "LOB":
                                foreach (var item in project.lob_options)
                                {
                                    if (item.Value.ToString() == cell.DisplayValue)
                                    {
                                        //project.lob_options.
                                        item.Selected = true;
                                    }
                                }
                                //project.lob = cell.DisplayValue;
                                break;

                            case "Tenrox":
                                project.tenrox = cell.DisplayValue;
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

                            case "Start Date":
                                if (cell.Value != null) 
                                {
                                    project.startDate = Convert.ToDateTime(cell.Value);
                                }                                
                                break;

                            case "Due Date":
                                if (cell.Value != null) 
                                {
                                    project.dueDate = Convert.ToDateTime(cell.Value);
                                }                                
                                break;

                            case "PM":
                                project.pm = cell.DisplayValue;
                                break;

                            case "PSDs":
                                project.PSDs = cell.DisplayValue;
                                break;

                            case "Box Folder":
                                project.boxFolder = cell.DisplayValue;
                                break;

                            case "WBS":
                                project.wbs_link = cell.DisplayValue;
                                break;

                            case "Deliverables Tracker":
                                project.deliverables_tracker_link = cell.DisplayValue;
                                break;

                            case "Specs":


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

                         

                            case "Description":
                                project.description = cell.DisplayValue;
                                break;

                         case "Number Of Sets":
                            project.numberOfSets = cell.DisplayValue;
                            break;
 
                         case "Animated Per Set":
                            project.animatedPerSet = cell.DisplayValue;
                            break;

                         case "Static Per Set":
                            project.staticPerSet = cell.DisplayValue;
                            break;
                        }
                    }
                }
            }
            return project;
        }
        public TechModel GetProjectDetails(long row_id)
        {
            TechModel project = new TechModel();
            project.pipelineId = row_id;
            Sheet sheet = LoadSheet(sheetId, initSheet());

            ////Get LOB options
            //project.lob_options = Get_lobs_picklist(sheet.GetColumnByIndex(0));
            ////Get Status options
            //project.status_options = Get_status_picklist(sheet.GetColumnByIndex(3));
            ////Get Team options
            //project.team_options = Get_status_picklist(sheet.GetColumnByIndex(16));

            foreach (var row in sheet.Rows)
            {
                if (row.Id == row_id)
                {
                    foreach (var cell in row.Cells)
                    {
                        long columnid = cell.ColumnId.Value;
                        string columnName = sheet.GetColumnById(columnid).Title.ToString();
                        //Console.WriteLine("Column Name: " + columnName + " -- Cell Value: " + cell.DisplayValue);
                        switch (columnName)
                        {
                            case "Project Name":
                                project.projectName = cell.DisplayValue;
                                break;

                            case "LOB":
                                project.lob = cell.DisplayValue;
                                break;

                            case "Tenrox":
                                project.tenrox = cell.DisplayValue;
                                break;

                            case "Status":
                                project.status = cell.DisplayValue;
                                break;

                            case "Start Date":
                                if (cell.Value != null)
                                {
                                    project.startDate = Convert.ToDateTime(cell.Value);
                                }
                                break;

                            case "Due Date":
                                if (cell.Value != null)
                                {
                                    project.dueDate = Convert.ToDateTime(cell.Value);
                                }
                                break;

                            case "PM":
                                project.pm = cell.DisplayValue;
                                break;


                            case "AM":
                                project.am = cell.DisplayValue;
                                break;

                            case "PSDs":
                                project.PSDs = cell.DisplayValue;
                                break;

                            case "Box Folder":
                                project.boxFolder = cell.DisplayValue;
                                break;

                            case "Specs":
                                //TBD
                                break;

                            case "Number Of Sets":
                                project.numberOfSets = cell.DisplayValue;
                                break;

                            case "Animated Per Set":
                                project.animatedPerSet = cell.DisplayValue;
                                break;

                            case "Static Per Set":
                                project.staticPerSet = cell.DisplayValue;
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

                        }
                    }
                }
            }
            return project;
        }

        public void updateProject(TechModel project)
        {
            SmartsheetClient smartsheet_CL = initSheet();
            Sheet sheet = LoadSheet(sheetId, smartsheet_CL);

            int row_number = 0;
            foreach (var row_b in sheet.Rows)
            {
                if (row_b.Id == project.pipelineId)
                {
                    row_number = row_b.RowNumber.Value;
                }
            }

            Row row = sheet.GetRowByRowNumber(row_number);
            var rowToTupdate = new Row();

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
            var notes_cell = new Cell();
            var number_of_sets_cell = new Cell();
            var animated_per_set_cell = new Cell();
            var static_per_set_cell = new Cell();

            foreach (var cell in row.Cells)
            {
                long columnid = cell.ColumnId.Value;
                string columnName = sheet.GetColumnById(columnid).Title.ToString();

                switch (columnName)
                {
                    case "Project Name":
                        project_cell.ColumnId = columnid;
                        project_cell.Value = project.projectName;
                        break;

                    case "LOB":
                        lob_cell.ColumnId = columnid;
                        lob_cell.Value = project.lob;
                        break;

                    case "Tenrox":
                        tenrox_cell.ColumnId = columnid;
                        tenrox_cell.Value = project.tenrox;
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
                        end_cell.Value = project.dueDate;
                        break;

                    case "Box":
                        box_folder_cell.ColumnId = columnid;
                        box_folder_cell.Value = project.boxFolder;
                        break;

                    //case "FxF":
                    //    collab_deck_cell.ColumnId = columnid;
                    //    collab_deck_cell.Value = project.fxf;
                    //    break;

                    case "PSDs":
                        psds_cell.ColumnId = columnid;
                        psds_cell.Value = project.PSDs;
                        break;

                    case "Specs":
                        specs_cell.ColumnId = columnid;
                        ObjectValue objct = null;
                        bool flag = false;
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

                            if (project.SelectedSpecs.Count() == 0)
                            {
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

                    case "Description":
                        notes_cell.ColumnId = columnid;
                        notes_cell.Value = project.description;
                        break;

             
                    case "WBS":
                        wbs_cell.ColumnId = columnid;
                        wbs_cell.Value = project.wbs_link;
                        break;
                }
            }

            rowToTupdate = new Row
            {
                Id = project.pipelineId,
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
                    wbs_cell,
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
                Console.WriteLine("Project Update has Failed: " + e.Message);
                TempData["Result"] = "Failed";
            };
        }

        //public TechModel GetPickLists(long row_id)
        //{
        //    TechModel project = new TechModel();
        //    project.pipelineId = row_id;
        //    Sheet sheet = LoadSheet(sheetId, initSheet());

        //    //Get LOB options
        //    project.lob_options = Get_lobs_picklist(sheet.GetColumnByIndex(0));
        //    //Get Status options
        //    project.status_options = Get_status_picklist(sheet.GetColumnByIndex(3));
        //    //Get Team options
        //    project.team_options = Get_status_picklist(sheet.GetColumnByIndex(16));
        //    //Get Specs List
        //    project.SpecsList = Get_Specs_List(sheet.GetColumnByIndex(11));


        //    return project;
        //}
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

        //public ICollection<SelectListItem> Get_Specs_List(Column specs_col)
        //{
        //    List<SelectListItem> list = new List<SelectListItem>();
        //    //int cont = 0;
        //    foreach (var spec in specs_col.Options)
        //    {
        //        //cont++;
        //        //list.Add(new SelectListItem { Text = spec, Value = cont.ToString()});
        //        list.Add(new SelectListItem { Text = spec, Value = spec });
        //    }
        //    return list;
        //}
        //public IEnumerable<SelectListItem> Get_lobs_picklist(Column lob_col)
        //{
        //    List<SelectListItem> options = new List<SelectListItem>();
        //    foreach (var lob in lob_col.Options)
        //    {
        //        options.Add(new SelectListItem { Text = lob, Value = lob });
        //    }
        //    return options;
        //}
        //public IEnumerable<SelectListItem> Get_status_picklist(Column status_col)
        //{
        //    List<SelectListItem> options = new List<SelectListItem>();
        //    foreach (var status in status_col.Options)
        //    {
        //        options.Add(new SelectListItem { Text = status, Value = status });
        //    }
        //    return options;
        //}
        //public IEnumerable<SelectListItem> Get_team_picklist(Column team_col)
        //{
        //    List<SelectListItem> options = new List<SelectListItem>();
        //    foreach (var team in team_col.Options)
        //    {
        //        options.Add(new SelectListItem { Text = team, Value = team });
        //    }
        //    return options;
        //}
    }
}
