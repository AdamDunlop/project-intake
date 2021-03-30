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


namespace SmartsheetsPIF.Controllers
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
                PDSModel model_lists = GetPickLists(model.pif_Id);
                model.lob_options = model_lists.lob_options;
                model.status_options = model_lists.status_options;
                return View(model);
                //return RedirectToAction("Edit", new { id = model.pif_Id });
            }
            else
            {
                updateProject(model);
                //return View("Index");
                TempData["Success"] = "Success";
                return RedirectToAction("PDSPIFs");
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
            List<PDSModel> pif_list = new List<PDSModel>();

            foreach (var row in sheet.Rows)
            {
                if (!string.IsNullOrWhiteSpace(row.Cells.ElementAt(0).DisplayValue))
                {
                    PDSModel pif = new PDSModel();
                    pif.pif_Id = (long)row.Id;
                    foreach (var cell in row.Cells)
                    {
                        long columnid = cell.ColumnId.Value;
                        string columnName = sheet.GetColumnById(columnid).Title.ToString();
                        Console.WriteLine("Column Name: " + columnName + " -- Cell Value: " + cell.DisplayValue);
                        switch (columnName)
                        {
                            

                            case "LOB":
                                pif.lob = cell.DisplayValue;
                                break;

                            case "Tenrox":
                                pif.tenrox = cell.DisplayValue;
                                break;

                            case "Project":
                                pif.projectName = cell.DisplayValue;
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
                                pif.pm = cell.DisplayValue;
                                break;

                            case "Type of Work":
                                pif.typeOfWork = cell.DisplayValue;
                                break;

                            case "Assigned to":
                                pif.assignedTo = cell.DisplayValue;
                                break;

                            case "Deliverables":
                                pif.deliverables = cell.DisplayValue;
                                break;

                            case "Desciption":
                                pif.description = cell.DisplayValue;
                                break;

                            case "Creative Lead":
                                pif.creativeLead = cell.DisplayValue;
                                break;


                        }
                    }
                    pif_list.Add(pif);
                }
            }
            return pif_list;
        }

        public PDSModel GetProjectEdit(long row_id)
        {
            PDSModel pif = new PDSModel();
            pif.pif_Id = row_id;
            Sheet sheet = LoadSheet(sheetId, initSheet());

            pif.lob_options = Get_lobs_picklist(sheet.GetColumnByIndex(0));
            pif.status_options = Get_status_picklist(sheet.GetColumnByIndex(4));

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

                            case "Tenrox":
                                pif.tenrox = cell.DisplayValue;
                                break;

                            case "Project":
                                pif.projectName = cell.DisplayValue;
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
                                pif.pm = cell.DisplayValue;
                                break;

                            case "Type of Work":
                                pif.typeOfWork = cell.DisplayValue;
                                break;

                            case "Deliverables":
                                pif.deliverables = cell.DisplayValue;
                                break;

                            case "Description":
                                pif.description = cell.DisplayValue;
                                break;

                            case "Creative Lead":
                                pif.creativeLead = cell.DisplayValue;
                                break;
                        }
                    }
                }
            }
            return pif;
        }

        public PDSModel GetProjectDetails(long row_id)
        {
            PDSModel pif = new PDSModel();
            pif.pif_Id = row_id;
            Sheet sheet = LoadSheet(sheetId, initSheet());

            //Get LOB options
            pif.lob_options = Get_lobs_picklist(sheet.GetColumnByIndex(0));
            //Get Status options
            pif.status_options = Get_status_picklist(sheet.GetColumnByIndex(2));
            //Get Team options

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
                                pif.lob = cell.DisplayValue;
                                break;

                            case "Tenrox":
                                pif.tenrox = cell.DisplayValue;
                                break;

                            case "Project":
                                pif.projectName = cell.DisplayValue;
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
                                pif.pm = cell.DisplayValue;
                                break;

                            case "Type of Work":
                                pif.typeOfWork = cell.DisplayValue;
                                break;

                            case "Description":
                                pif.description = cell.DisplayValue;
                                break;

                            case "Deliverables":
                                pif.deliverables = cell.DisplayValue;
                                break;


                        }
                    }
                }
            }
            return pif;
        }

        public void updateProject(PDSModel pif)
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
            var rowToTupdate = new Row();

            var lob_cell = new Cell();
            var tenrox_cell = new Cell();
            var project_cell = new Cell();
            var type_cell = new Cell();
            var status_cell = new Cell();
            var start_cell = new Cell();
            var end_cell = new Cell();
            var assigned_cell = new Cell();
            var pm_cell = new Cell();
            var deliverables_cell = new Cell();
            var description_cell = new Cell();
            var creative_cell = new Cell();

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

                    case "Tenrox":
                        tenrox_cell.ColumnId = columnid;
                        tenrox_cell.Value = pif.tenrox;
                        break;

                    case "LOB":
                        lob_cell.ColumnId = columnid;
                        lob_cell.Value = pif.lob;
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

                    case "Type of Work":
                        type_cell.ColumnId = columnid;
                        type_cell.Value = pif.typeOfWork;
                        break;

                    case "Assigned to":
                        assigned_cell.ColumnId = columnid;
                        assigned_cell.Value = pif.assignedTo;
                        break;

                    case "PM":
                        pm_cell.ColumnId = columnid;
                        pm_cell.Value = pif.pm;
                        break;

                    case "Total Deliverables":
                        deliverables_cell.ColumnId = columnid;
                        deliverables_cell.Value = pif.deliverables;
                        break;

                    case "Description":
                        description_cell.ColumnId = columnid;
                        description_cell.Value = pif.description;
                        break;

                    case "Creative Lead":
                        creative_cell.ColumnId = columnid;
                        creative_cell.Value = pif.creativeLead;
                        break;

                        //case "Static Per Set":
                        //    static_per_set_cell.ColumnId = columnid;
                        //    static_per_set_cell.Value = pif.staticPerSet;
                        //    break;

                        //case "Deliverables Tracker":
                        //    deliverables_t_cell.ColumnId = columnid;
                        //    deliverables_t_cell.Value = pif.deliverables_tracker_link;
                        //    break;

                        //case "WBS":
                        //    wbs_cell.ColumnId = columnid;
                        //    wbs_cell.Value = pif.wbs_link;
                        //    break;


                }
            }

            rowToTupdate = new Row
            {
                Id = pif.pif_Id,
                Cells = new Cell[] {
                    lob_cell,
                    tenrox_cell,
                    project_cell,
                    status_cell,
                    start_cell,
                    end_cell,
                    assigned_cell,
                    type_cell,
                    deliverables_cell,
                    pm_cell,
                    creative_cell

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

        public PDSModel GetPickLists(long row_id)
        {
            PDSModel pif = new PDSModel();
            pif.pif_Id = row_id;
            Sheet sheet = LoadSheet(sheetId, initSheet());

            pif.lob_options = Get_lobs_picklist(sheet.GetColumnByIndex(0));
           
            pif.status_options = Get_status_picklist(sheet.GetColumnByIndex(5));


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
