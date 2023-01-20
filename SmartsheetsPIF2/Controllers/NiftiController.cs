using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Smartsheet.Api;
using Smartsheet.Api.Models;
using SmartsheetsPIF.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmartsheetsPIF.Controllers
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
          
                updateProject(model);
                return RedirectToAction("List");
         
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
                    project.pipeline_Id = (long)row.Id;
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

                            case "Intake Type":
                                project.intakeType = cell.DisplayValue;
                                break;

                            case "Date Requested":
                                if (project.dateRequested != null)
                                {
                                    project.dateRequested = Convert.ToDateTime(cell.Value);
                                }
                                //pif.startDate = DateTime.ParseExact( cell.Value.ToString(), "mm,dd,yyyy",null);
                                break;

                            case "Type of Project":
                                project.type = cell.DisplayValue;
                                break;

                            case "Client Group":
                                project.clientGroup = cell.DisplayValue;
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
            project.pipeline_Id = row_id;
            Sheet sheet = LoadSheet(sheetId, initSheet());



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


                            case "Intake Type":
                                project.intakeType = cell.DisplayValue;
                                break;

                            case "Date Requested":
                                project.dateRequested = Convert.ToDateTime(cell.Value);
                                break;

                            case "Type of Project":
                                project.type = cell.DisplayValue;
                                break;

                            case "Lead Client Stakeholder":
                                project.clientStakeholder = cell.DisplayValue;
                                break;

                            case "Client Budget":
                                project.clientBudget = cell.DisplayValue;
                                break;

                            case "Project Briefing Date":
                                project.briefDate = Convert.ToDateTime(cell.Value);
                                break;

                            case "Project Live Date":
                                project.liveDate = Convert.ToDateTime(cell.Value);
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
            project.pipeline_Id = row_id;
            Sheet sheet = LoadSheet(sheetId, initSheet());


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
                if (row_b.Id == project.pipeline_Id)
                {
                    row_number = row_b.RowNumber.Value;
                }
            }

            Row row = sheet.GetRowByRowNumber(row_number);
            var rowToTupdate = new Row();
            var project_cell = new Cell();


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


                }
            };

            rowToTupdate = new Row
            {
                Id = project.pipeline_Id,
                Cells = new Cell[] {
                    project_cell
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
        
    }
}