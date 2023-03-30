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
    public class VideoController : Controller
    {

        public static long sheetId = 5887595533100932;


        [HttpGet]
        public IActionResult List()
        {
            var sheet = LoadSheet(sheetId, initSheet());
            return View(GetRows(sheet));
        }


        [HttpGet]
        public IActionResult Edit(long id)
        {
            VideoModel model = new VideoModel();
            model = GetProjectEdit(id);
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(VideoModel model)
        {
            if (!ModelState.IsValid)
            {
                VideoModel model_lists = GetPickLists(model.pipelineId);
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
            VideoModel model = new VideoModel();
            model = GetProjectDetails(id);
            return View(model);
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
                null                        // Nullable<long> page
            );
            Console.WriteLine("Loaded " + sheet.Rows.Count + " rows from sheet: " + sheet.Name);
            return sheet;
        }

        public List<VideoModel> GetRows(Sheet sheet)
        {
            List<VideoModel> project_list = new List<VideoModel>();
            //Get_lobs_picklist(sheet.GetColumnByIndex(0));

            foreach (var row in sheet.Rows)
            {
                if (!string.IsNullOrWhiteSpace(row.Cells.ElementAt(0).DisplayValue))
                {
                    VideoModel project = new VideoModel();
                    project.pipelineId = (long)row.Id;
                    foreach (var cell in row.Cells)
                    {
                        long columnid = cell.ColumnId.Value;
                        string columnName = sheet.GetColumnById(columnid).Title.ToString();
                        Console.WriteLine("Column Name: " + columnName + " -- Cell Value: " + cell.DisplayValue);
                        switch (columnName)
                        {

                            case "Intake Type":
                                project.intakeType = cell.DisplayValue;
                                break;

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
                                break;

                            case "Due Date":
                                if (project.dueDate != null)
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

                            case "Assigned To":
                                project.assignedTo = cell.DisplayValue;
                                break;

                            case "PSDs":
                                project.box = cell.DisplayValue;
                                break;

                            case "Script":
                                project.box = cell.DisplayValue;
                                break;

                            case "WBS":
                                project.wbs = cell.DisplayValue;
                                break;

                            case "Box":
                                project.box = cell.DisplayValue;
                                break;

                            case "Figma":
                                project.frameio = cell.DisplayValue;
                                break;

                            case "Specs":
                                project.specs_list = cell.DisplayValue;
                                break;

                      
                        }
                    }
                    project_list.Add(project);
                }
            }
            return project_list;
        }


        public VideoModel GetProjectDetails(long row_id)
        {
            VideoModel project = new VideoModel();
            project.pipelineId = row_id;
            Sheet sheet = LoadSheet(sheetId, initSheet());

            project.lob_options = Get_lobs_picklist(sheet.GetColumnByIndex(24));
            project.status_options = Get_status_picklist(sheet.GetColumnByIndex(25));
            project.SpecsList = Get_Specs_List(sheet.GetColumnByIndex(26));


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

                            case "Project Name":
                                project.projectName = cell.DisplayValue;
                                break;

                            case "Tenrox":
                                project.tenrox = cell.DisplayValue;
                                break;

                            case "Status":
                                project.status = cell.DisplayValue;
                                break;

                            case "Start Date":
                                project.startDate = Convert.ToDateTime(cell.Value);
                                //project.startDate = DateTime.ParseExact( cell.Value.ToString(), "mm,dd,yyyy",null);
                                break;

                            case "Due Date":
                                project.dueDate = Convert.ToDateTime(cell.Value);
                                //project.endDate = DateTime.ParseExact(cell.Value.ToString(), "mm,dd,yyyy", null);
                                break;

                            case "PM":
                                project.pm = cell.DisplayValue;
                                break;

                            case "AM":
                                project.am = cell.DisplayValue;
                                break;

                            case "Assigned To":
                                project.assignedTo = cell.DisplayValue;
                                break;

                            case "WBS":
                                if (cell.Value != null)
                                {
                                    string[] wbs_link = cell.DisplayValue.Split(" ");
                                    foreach (var item in wbs_link)
                                    {
                                        if (item.Contains("https:"))
                                        {
                                            wbs_link = item.Split('"');
                                            foreach (var url in wbs_link)
                                            {
                                                if (url.Contains("https:"))
                                                {
                                                    cell.DisplayValue = url.Trim('"'); ;
                                                }
                                            }
                                        }
                                    }
                                    project.wbs = cell.DisplayValue;
                                }
                                break;


                            case "PSDs":
                                if (cell.Value != null)
                                {
                                    string[] psds_link = cell.DisplayValue.Split(" ");
                                    foreach (var item in psds_link)
                                    {
                                        if (item.Contains("https:"))
                                        {
                                            psds_link = item.Split('"');
                                            foreach (var url in psds_link)
                                            {
                                                if (url.Contains("https:"))
                                                {
                                                    cell.DisplayValue = url.Trim('"'); ;
                                                }
                                            }
                                        }
                                    }
                                    project.PSDs = cell.DisplayValue;
                                }
                                break;

                            case "Script":
                                if (cell.Value != null)
                                {
                                    string[] box_link = cell.DisplayValue.Split(" ");
                                    foreach (var item in box_link)
                                    {
                                        if (item.Contains("https:"))
                                        {
                                            box_link = item.Split('"');
                                            foreach (var url in box_link)
                                            {
                                                if (url.Contains("https:"))
                                                {
                                                    cell.DisplayValue = url.Trim('"'); ;
                                                }
                                            }
                                        }
                                    }
                                    project.box = cell.DisplayValue;
                                }
                                break;

                            case "Frame.io":
                                if (cell.Value != null)
                                {
                                    string[] frameio_link = cell.DisplayValue.Split(" ");
                                    foreach (var item in frameio_link)
                                    {
                                        if (item.Contains("https:"))
                                        {
                                            frameio_link = item.Split('"');
                                            foreach (var url in frameio_link)
                                            {
                                                if (url.Contains("https:"))
                                                {
                                                    cell.DisplayValue = url.Trim('"'); ;
                                                }
                                            }
                                        }
                                    }
                                    project.frameio = cell.DisplayValue;
                                }
                                break;

                            case "Specs":
                                project.specs_list = cell.DisplayValue;
                                break;
                        }
                    }
                }
            }
            return project;
        }

        public VideoModel GetProjectEdit(long row_id)
        {
            VideoModel project = new VideoModel();
            project.pipelineId = row_id;
            Sheet sheet = LoadSheet(sheetId, initSheet());

            project.lob_options = Get_lobs_picklist(sheet.GetColumnByIndex(24));
            project.status_options = Get_status_picklist(sheet.GetColumnByIndex(25));
            project.SpecsList = Get_Specs_List(sheet.GetColumnByIndex(26));

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
                                break;

                            case "Project Name":
                                project.projectName = cell.DisplayValue;
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
                                project.startDate = Convert.ToDateTime(cell.Value);
                                break;

                            case "Due Date":
                                project.dueDate = Convert.ToDateTime(cell.Value);
                                break;

                            case "Assigned To":
                                project.assignedTo = cell.DisplayValue;
                                break;

                            case "WBS":
                                project.wbs = cell.DisplayValue;
                                break;

                                //case "PSDs":
                                //    project.PSDs = cell.DisplayValue;
                                //    break;

                                //case "Frame.io":
                                //    project.frameio = cell.DisplayValue;
                                //    break;

                                //case "Script":
                                //    project.frameio = cell.DisplayValue;
                                //    break;

                                //case "Specs":

                                //    // List<SelectListItem> selectedvalues = new List<SelectListItem>();

                                //    if (cell.DisplayValue != null)
                                //    {
                                //        List<string> selectedvalues = new List<string>();

                                //        var list = cell.DisplayValue.Split(",");

                                //        foreach (var spec in list)
                                //        {
                                //            IEnumerable<SelectListItem> variable = project.SpecsList.Where(x => x.Text.Contains(spec.TrimStart(' ')));

                                //            var id = "";

                                //            foreach (var i in variable)
                                //            {
                                //                id = i.Value;
                                //            }

                                //            //selectedvalues.Add(new SelectListItem { Value = id, Text = spec, Selected = true });
                                //            selectedvalues.Add(id);

                                //            variable = null;
                                //        }
                                //        project.SelectedSpecs = selectedvalues;
                                //    }
                                //    break;

                        }
                    }
                }
            }
            return project;
        }

       

        public void updateProject(VideoModel project)
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
            var status_cell = new Cell();
            var start_cell = new Cell();
            var due_cell = new Cell();
            var assigned_cell = new Cell();
            var wbs_cell = new Cell();
            //var script_cell = new Cell();
            //var frameio_cell = new Cell();
            //var psds_cell = new Cell();
            //var specs_cell = new Cell();


            foreach (var cell in row.Cells)
            {
                long columnid = cell.ColumnId.Value;
                string columnName = sheet.GetColumnById(columnid).Title.ToString();

                switch (columnName)
                {

                    case "LOB":
                        lob_cell.ColumnId = columnid;
                        lob_cell.Value = project.lob;
                        break;

                    case "Status":
                        status_cell.ColumnId = columnid;
                        status_cell.Value = project.status;
                        break;

                    case "Start Date":
                        start_cell.ColumnId = columnid;
                        start_cell.Value = project.startDate;
                        break;

                    case "Due Date":
                        due_cell.ColumnId = columnid;
                        due_cell.Value = project.dueDate;
                        break;

                    case "WBS":
                        wbs_cell.ColumnId = columnid;
                        wbs_cell.Value = project.wbs;
                        break;

                    case "Assigned To":
                        assigned_cell.ColumnId = columnid;
                        assigned_cell.Value = project.assignedTo;
                        break;

                        //case "Frame.io":
                        //    frameio_cell.ColumnId = columnid;
                        //    frameio_cell.Value = project.frameio;
                        //    break;

                        //case "Script":
                        //    script_cell.ColumnId = columnid;
                        //    script_cell.Value = project.script;
                        //    break;

                        //case "Specs":
                        //    specs_cell.ColumnId = columnid;
                        //    specs_cell.Value = project.SpecsList;
                        //    break;



                }
            }

            rowToTupdate = new Row
            {
                Id = project.pipelineId,
                Cells = new Cell[] {
                    lob_cell,
                    status_cell,
                    start_cell,
                    due_cell,
                    assigned_cell,
                    wbs_cell
                    //frameio_cell,
                    //script_cell,
                    //specs_cell
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
                Console.WriteLine("Update row Failed: " + e.Message + e.Data.ToString());
                TempData["Result"] = "Failed";

            };


        }

        public VideoModel GetPickLists(long row_id)
        {
            VideoModel project = new VideoModel();
            project.pipelineId = row_id;
            Sheet sheet = LoadSheet(sheetId, initSheet());

            project.lob_options = Get_lobs_picklist(sheet.GetColumnByIndex(24));
            project.status_options = Get_status_picklist(sheet.GetColumnByIndex(25));
            project.SpecsList = Get_Specs_List(sheet.GetColumnByIndex(26));


            return project;
        }
        //public void runthroughallsheets()
        //{
        //    /*
        //    // List all sheets
        //    PaginatedResult<Sheet> sheets = smartsheet_CL.SheetResources.ListSheets(
        //        null,               // IEnumerable<SheetInclusion> includes
        //        null,               // PaginationParameters
        //        null                // Nullable<DateTime> modifiedSince = null
        //    );
        //    //Console.WriteLine("Found " + sheets.TotalCount + " sheets");
        //    //iteration through all Sheet IDs
        //    for (int i = 0; sheets.TotalCount > i; i++) {
        //        ///Console.WriteLine("Loading sheet position: " + i);
        //        //Console.WriteLine("Loading sheet id: " + (long)sheets.Data[i].Id);
        //        GetSheet((long)sheets.Data[i].Id,smartsheet);
        //    }
        //    long sheetId = 1478730146178948; //Display TEST
        //    */
        //}

     

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

    }



}
