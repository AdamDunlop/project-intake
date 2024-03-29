﻿using Microsoft.AspNetCore.Mvc;
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
    public class DesignController : Controller
    {

        public static long sheetId = 638982942943108;


        [HttpGet]
        public IActionResult List()
        {
            var sheet = LoadSheet(sheetId, initSheet());
            return View(GetRows(sheet));
        }


        [HttpGet]
        public IActionResult Edit(long id)
        {
            DesignModel model = new DesignModel();
            model = GetProjectEdit(id);
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(DesignModel model)
        {
            if (!ModelState.IsValid)
            {
                DesignModel model_lists = GetPickLists(model.pipelineId);
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
            DesignModel model = new DesignModel();
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

        public List<DesignModel> GetRows(Sheet sheet)
        {
            List<DesignModel> project_list = new List<DesignModel>();

            foreach (var row in sheet.Rows)
            {
                if (!string.IsNullOrWhiteSpace(row.Cells.ElementAt(0).DisplayValue))
                {
                    DesignModel project = new DesignModel();
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

                            case "Assigned To":
                                project.assignedTo = cell.DisplayValue;
                                break;

                            case "PM":
                                project.pm = cell.DisplayValue;
                                break;

                            case "AM":
                                project.pm = cell.DisplayValue;
                                break;

                            case "WBS":
                                project.wbs = cell.DisplayValue;
                                break;

                            case "Box":
                                project.box = cell.DisplayValue;
                                break;

                            case "Figma":
                                project.figma = cell.DisplayValue;
                                break;

                            case "Specs":
                                project.specs_list = cell.DisplayValue;
                                break;

                            case "Hours":
                                project.hours = cell.DisplayValue;
                                break;

                        }
                    }
                    project_list.Add(project);
                }
            }
            return project_list;
        }

        public DesignModel GetProjectDetails(long row_id)
        {
            DesignModel project = new DesignModel();
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

                            case "Intake Type":
                                project.intakeType = cell.DisplayValue;
                                break;

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

                            //case "Jira":
                            //    project.jira = cell.DisplayValue;
                            //    break;

                            case "Start Date":
                                project.startDate = Convert.ToDateTime(cell.Value);
                                break;

                            case "Due Date":
                                project.dueDate = Convert.ToDateTime(cell.Value);
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


                            case "Box":
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

                            case "Figma":
                                if (cell.Value != null)
                                {
                                    string[] figma_link = cell.DisplayValue.Split(" ");
                                    foreach (var item in figma_link)
                                    {
                                        if (item.Contains("https:"))
                                        {
                                            figma_link = item.Split('"');
                                            foreach (var url in figma_link)
                                            {
                                                if (url.Contains("https:"))
                                                {
                                                    cell.DisplayValue = url.Trim('"'); ;
                                                }
                                            }
                                        }
                                    }
                                    project.figma = cell.DisplayValue;
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

        public DesignModel GetProjectEdit(long row_id)
        {
            DesignModel project = new DesignModel();
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

                            case "Box":
                                project.box = cell.DisplayValue;
                                break;

                            case "Figma":
                                project.figma = cell.DisplayValue;
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

        

        public void updateProject(DesignModel project)
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
            var start_date_cell = new Cell();
            var due_date_cell = new Cell();
            var wbs_cell = new Cell();
            var box_cell = new Cell();
            var figma_cell = new Cell();
            var specs_cell = new Cell();
            var assigned_cell = new Cell();

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
                        start_date_cell.ColumnId = columnid;
                        start_date_cell.Value = project.startDate;
                        break;

                    case "Due Date":
                        due_date_cell.ColumnId = columnid;
                        due_date_cell.Value = project.dueDate;
                        break;

                    case "WBS":
                        wbs_cell.ColumnId = columnid;
                        wbs_cell.Value = project.wbs;
                        break;

                    case "Box":
                        box_cell.ColumnId = columnid;
                        box_cell.Value = project.box;
                        break;

                    case "Figma":
                        figma_cell.ColumnId = columnid;
                        figma_cell.Value = project.figma;
                        break;

                    case "Assigned To":
                        assigned_cell.ColumnId = columnid;
                        assigned_cell.Value = project.assignedTo;
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

                }
            }

            rowToTupdate = new Row
            {
                Id = project.pipelineId,
                Cells = new Cell[] {
                    lob_cell,
                    status_cell,         
                    start_date_cell,
                    due_date_cell,
                    wbs_cell,
                    box_cell,
                    figma_cell,
                    specs_cell,
                    assigned_cell
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

        public DesignModel GetPickLists(long row_id)
        {
            DesignModel project = new DesignModel();
            project.pipelineId = row_id;
            Sheet sheet = LoadSheet(sheetId, initSheet());

            project.lob_options = Get_lobs_picklist(sheet.GetColumnByIndex(24));
            project.status_options = Get_status_picklist(sheet.GetColumnByIndex(25));
            project.SpecsList = Get_Specs_List(sheet.GetColumnByIndex(26));

            return project;
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
