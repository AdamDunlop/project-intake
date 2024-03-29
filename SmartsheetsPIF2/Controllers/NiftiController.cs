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


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Smartsheetsproject.Controllers
{
    public class NiftiController : Controller
    {


        public static long sheetId = 5536034378278788;


        [HttpGet]
        public IActionResult List()
        {
            var sheet = LoadSheet(sheetId, initSheet());
            return View(GetRows(sheet));
        }

        [HttpGet]
        public IActionResult Details(long id)
        {
            NiftiModel model = new NiftiModel();
            model = GetProjectDetails(id);
            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(long id)
        {
            NiftiModel model = new NiftiModel();
            model = GetProjectEdit(id);
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(NiftiModel model)
        {   

            if (!ModelState.IsValid)
            {
                NiftiModel model_lists = GetListValues(model.pipelineId);
                model.intake_options = model_lists.intake_options;
                model.CategoryList = model_lists.CategoryList;

                return View(model);
                //return RedirectToAction("Edit", new { id = model.pipelineId });
            }
            else
            {
                updateProject(model);
                //return View("Index");
                TempData["Success"] = "Success";
                return RedirectToAction("List");
            };
         
        }

        [HttpGet]
        public IActionResult Delete()
        {
            NiftiModel model = new NiftiModel();

            return View(model);
        }


        public SmartsheetClient initSheet()
        {
            // Initialize client
            SmartsheetClient smartsheet_CL = new SmartsheetBuilder().SetAccessToken("x5QRZ9m5hrjG4xdlnnHbQVcxHZnrC37oYPpO8")
                .Build();
            return smartsheet_CL;
        }

        public Sheet LoadSheet(long sheetId, Smartsheet.Api.SmartsheetClient smartsheet_CL)
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

        public List<NiftiModel> GetRows(Sheet sheet)
        {
            List<NiftiModel> project_list = new List<NiftiModel>();

            foreach (var row in sheet.Rows)
            {
                if (!string.IsNullOrWhiteSpace(row.Cells.ElementAt(0).DisplayValue))
                {
                    NiftiModel project = new NiftiModel();
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

                            case "Tenrox":
                                project.tenrox = cell.DisplayValue;
                                break;

                            case "Intake Type":
                                project.intakeType = cell.DisplayValue;
                                break;

                            case "Requestor Name":
                                project.requestorName = cell.DisplayValue;
                                break;

                            case "Requestor Email":
                                project.requestorEmail = cell.DisplayValue;
                                break;

                            case "In / OOS":
                                project.inOos = cell.DisplayValue;
                                break;

                            case "Level of Effort":
                                project.loe = cell.DisplayValue;
                                break;

                            case "Size":
                                project.size = cell.DisplayValue;
                                break;

                            case "Priority":
                                project.priority = cell.DisplayValue;
                                break;

                            case "Client":
                                project.client = cell.DisplayValue;
                                break;

                            case "Project Details":
                                project.projectDetails = cell.DisplayValue;
                                break;

                            case "Date Requested":
                                if (project.dateRequested != null)
                                {
                                    project.dateRequested = Convert.ToDateTime(cell.Value);
                                }
                                //project.startDate = DateTime.ParseExact( cell.Value.ToString(), "mm,dd,yyyy",null);
                                break;

                            case "Project Type":
                                project.projectType = cell.DisplayValue;
                                break;

                            case "Team(s)":
                                project.teamCategory = cell.DisplayValue;
                                break;

                            case "Client Group":
                                project.clientGroup = cell.DisplayValue;
                                break;

                            case "Brief Date":
                                if (project.briefDate != null)
                                {
                                    project.briefDate = Convert.ToDateTime(cell.Value);
                                }
                                //project.startDate = DateTime.ParseExact( cell.Value.ToString(), "mm,dd,yyyy",null);
                                break;

                            case "Live Date":
                                if (project.liveDate != null)
                                {
                                    project.liveDate = Convert.ToDateTime(cell.Value);
                                }
                                //project.endDate = DateTime.ParseExact(cell.Value.ToString(), "mm,dd,yyyy", null);
                                break;


                            case "AM":
                                project.am = cell.DisplayValue;
                                break;


                            case "PM":
                                project.pm = cell.DisplayValue;
                                break;

                            case "WBS":
                                project.wbs = cell.DisplayValue;
                                break;

                            case "Box Link to Brief":
                                project.briefLink = cell.DisplayValue;
                                break;


                        }
                    }
                    project_list.Add(project);
                }
            }
            return project_list;
        }


            public NiftiModel GetProjectDetails(long row_id)
        {
            NiftiModel project = new NiftiModel();
            project.pipelineId = row_id;
            Sheet sheet = LoadSheet(sheetId, initSheet());

            project.intake_options = Get_Intake_List(sheet.GetColumnByIndex(0));
            project.CategoryList = Get_Category_List(sheet.GetColumnByIndex(8));

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

                            case "Tenrox":
                                project.tenrox = cell.DisplayValue;
                                break;

                            case "Intake Type":
                                project.intakeType = cell.DisplayValue;
                                break;

                            case "Requestor Name":
                                project.requestorName = cell.DisplayValue;
                                break;

                            case "Requestor Email":
                                project.requestorEmail = cell.DisplayValue;
                                break;

                            case "In / OOS":
                                project.inOos = cell.DisplayValue;
                                break;

                            case "Level of Effort":
                                project.loe = cell.DisplayValue;
                                break;

                            case "Size":
                                project.size = cell.DisplayValue;
                                break;

                            case "Priority":
                                project.priority = cell.DisplayValue;
                                break;

                            case "Client":
                                project.client = cell.DisplayValue;
                                break;

                            case "Date Requested":
                                project.dateRequested = Convert.ToDateTime(cell.Value);
                                break;

                            case "Brief Date":
                                project.briefDate = Convert.ToDateTime(cell.Value);
                                break;

                            case "Live Date":
                                project.liveDate = Convert.ToDateTime(cell.Value);
                                break;

                            case "Project Type":
                                project.projectType = cell.DisplayValue;
                                break;

                            case "Team(s)":
                                project.teamCategory = cell.DisplayValue;
                                break;

                            case "Lead Client Stakeholder":
                                project.clientStakeholder = cell.DisplayValue;
                                break;

                            case "Client Group":
                                project.clientGroup = cell.DisplayValue;
                                break;

                            case "Client Budget":
                                project.clientBudget = cell.DisplayValue;
                                break;

                            case "Project Details":
                                project.projectDetails = cell.DisplayValue;
                                break;

                            case "AM":
                                project.am = cell.DisplayValue;
                                break;

                            case "PM":
                                project.pm = cell.DisplayValue;
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

                            case "Box Link to Brief":
                                if (cell.Value != null)
                                {
                                    string[] brief = cell.DisplayValue.Split(" ");
                                    foreach (var item in brief)
                                    {
                                        if (item.Contains("https:"))
                                        {
                                            brief = item.Split('"');
                                            foreach (var url in brief)
                                            {
                                                if (url.Contains("https:"))
                                                {
                                                    cell.DisplayValue = url.Trim('"'); ;
                                                }
                                            }
                                        }
                                    }
                                    project.briefLink = cell.DisplayValue;
                                }
                                break;
                        }
                    }
                }
            }
            return project;
        }


        public NiftiModel GetProjectEdit(long row_id)
        {
            NiftiModel project = new NiftiModel();
            project.pipelineId = row_id;
            Sheet sheet = LoadSheet(sheetId, initSheet());

            project.intake_options = Get_Intake_List(sheet.GetColumnByIndex(0));
            project.CategoryList = Get_Category_List(sheet.GetColumnByIndex(8));

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

                            case "Tenrox":
                                project.tenrox = cell.DisplayValue;
                                break;

                            case "Intake Type":
                                project.intakeType = cell.DisplayValue;
                                break;

                            case "Project Type":
                                project.projectType = cell.DisplayValue;
                                break;

                            case "Project Details":
                                project.projectDetails = cell.DisplayValue;
                                break;

                            case "Team(s)":

                            // List<SelectListItem> selectedvalues = new List<SelectListItem>();

                            if (cell.DisplayValue != null)
                            {
                                List<string> selectedvalues = new List<string>();

                                var list = cell.DisplayValue.Split(",");

                                foreach (var cat in list)
                                {
                                    IEnumerable<SelectListItem> variable = project.CategoryList.Where(x => x.Text.Contains(cat.TrimStart(' ')));

                                    var id = "";

                                    foreach (var i in variable)
                                    {
                                        id = i.Value;
                                    }

                                    //selectedvalues.Add(new SelectListItem { Value = id, Text = spec, Selected = true });
                                    selectedvalues.Add(id);

                                    variable = null;
                                }
                                project.SelectedCategory = selectedvalues;
                            }
                            break;

                            case "AM":
                                project.am = cell.DisplayValue;
                                break;

                            case "PM":
                                project.pm = cell.DisplayValue;
                                break;

                            case "WBS":
                                project.wbs = cell.DisplayValue;
                                break;

                            case "Box Link to Brief":
                                project.briefLink = cell.DisplayValue;
                                break;
                        }
                    }
                }
            }
            return project;
        }
   

        public void updateProject(NiftiModel project)
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

            var project_name_cell = new Cell();
            var tenrox_cell = new Cell();
            var intake_cell = new Cell();
            var project_details_cell = new Cell();
            var project_type_cell = new Cell();
            //var project_brief_cell = new Cell();
            //var project_live_cell = new Cell();
            var teams_cell = new Cell();
            var am_cell = new Cell();
            var pm_cell = new Cell();
            var wbs_cell = new Cell();
            var brief_link_cell = new Cell();

            foreach (var cell in row.Cells)
            {
                long columnid = cell.ColumnId.Value;
                string columnName = sheet.GetColumnById(columnid).Title.ToString();

                switch (columnName)
                {

                    

                    case "Project Name":
                        project_name_cell.ColumnId = columnid;
                        project_name_cell.Value = project.projectName;
                        break;

                    case "Tenrox":
                        tenrox_cell.ColumnId = columnid;
                        tenrox_cell.Value = project.tenrox;
                        break;

                    case "Intake Type":
                        intake_cell.ColumnId = columnid;
                        intake_cell.Value = project.intakeType;
                        break;

                    case "Project Details":
                        project_details_cell.ColumnId = columnid;
                        project_details_cell.Value = project.projectDetails;
                        break;

                    case "Project Type":
                        project_type_cell.ColumnId = columnid;
                        project_type_cell.Value = project.projectType;
                        break;

                    case "Team(s)":
                        teams_cell.ColumnId = columnid;
                        ObjectValue objct = null;
                        bool flag = false;
                        //count = project.SelectedSpecs.Count();

                        if (project.SelectedCategory != null)
                        {

                            foreach (var cat in project.SelectedCategory)

                            {
                                if (cat != null)
                                {
                                    if (cat.Contains("System.String"))
                                    {
                                        flag = true;
                                    }
                                }
                            }
                            if (flag)
                            {
                                project.SelectedCategory.RemoveAt(project.SelectedCategory.Count() - 1);
                            }

                            if (project.SelectedCategory.Count() == 0)
                            {
                                project.SelectedCategory.Add("TBD");
                                objct = new MultiPicklistObjectValue(project.SelectedCategory);

                            }
                            else
                            {
                                objct = new MultiPicklistObjectValue(project.SelectedCategory);
                            }

                        }
                        teams_cell.ObjectValue = objct;
                        break;

                    case "AM":
                        am_cell.ColumnId = columnid;
                        am_cell.Value = project.am;
                        break;

                    case "PM":
                        pm_cell.ColumnId = columnid;
                        pm_cell.Value = project.pm;
                        break;

                    case "WBS":
                        wbs_cell.ColumnId = columnid;
                        wbs_cell.Value = project.wbs;
                        break;

                    case "Box Link to Brief":
                        brief_link_cell.ColumnId = columnid;
                        brief_link_cell.Value = project.briefLink;
                        break;


                }
            }

            rowToTupdate = new Row
            {
                Id = project.pipelineId,
                Cells = new Cell[] {
                    project_name_cell,
                    tenrox_cell,
                    intake_cell,
                    project_details_cell,
                    teams_cell,
                    am_cell,
                    pm_cell,
                    wbs_cell,
                    brief_link_cell

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

        public NiftiModel GetListValues(long row_id)
        {
            NiftiModel project = new NiftiModel();
            project.pipelineId = row_id;
            Sheet sheet = LoadSheet(sheetId, initSheet());

            project.intake_options = Get_Intake_List(sheet.GetColumnByIndex(0));
            project.CategoryList = Get_Category_List(sheet.GetColumnByIndex(8));

            return project;
        }

        public IEnumerable<SelectListItem> Get_Intake_List(Column type_col)
        {
            List<SelectListItem> options = new List<SelectListItem>();
            foreach (var type in type_col.Options)
            {
                options.Add(new SelectListItem { Text = type, Value = type });
            }
            return options;
        }

        public ICollection<SelectListItem> Get_Category_List(Column category_col)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            //int cont = 0;
            foreach (var spec in category_col.Options)
            {
                //cont++;
                //list.Add(new SelectListItem { Text = spec, Value = cont.ToString()});
                list.Add(new SelectListItem { Text = spec, Value = spec });
            }
            return list;
        }
        
    }
}
