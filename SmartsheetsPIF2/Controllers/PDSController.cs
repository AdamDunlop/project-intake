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

                            case "Tenrox Code":
                                pif.tenroxCode = cell.DisplayValue;
                                break;

                            case "Project":
                                pif.projectName = cell.DisplayValue;
                                break;

                            case "Status":
                                pif.status = cell.DisplayValue;
                                break;

                            case "Start":
                                if (pif.startDate != null)
                                {
                                    pif.startDate = Convert.ToDateTime(cell.Value);
                                }
                                    //pif.startDate = DateTime.ParseExact( cell.Value.ToString(), "mm,dd,yyyy",null);
                                break;

                            case "Ship":
                                if (pif.endDate != null)
                                {
                                    pif.endDate = Convert.ToDateTime(cell.Value);
                                }
                                //pif.endDate = DateTime.ParseExact(cell.Value.ToString(), "mm,dd,yyyy", null);
                                break;

                            case "Assigned to":
                                pif.assignedTo = cell.DisplayValue;
                                break;

                            case "PM":
                                pif.pm = cell.DisplayValue;
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
            pif.status_options = Get_status_picklist(sheet.GetColumnByIndex(3));

            pif.SpecsList = Get_Specs_List(sheet.GetColumnByIndex(17));

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
                                        item.Selected = true;
                                    }
                                }
                                //pif.lob = cell.DisplayValue;
                                break;

                            case "Tenrox Code":
                                pif.tenroxCode = cell.DisplayValue;
                                break;

                            case "Project":
                                pif.projectName = cell.DisplayValue;
                                break;

                            case "Type of Work":
                                pif.typeOfWork = cell.DisplayValue;
                                break;

                            case "Status":
                                foreach (var item in pif.status_options)
                                {
                                    if (item.Value.ToString() == cell.DisplayValue)
                                    {
                                        item.Selected = true;
                                    }
                                }
                                break;

                            case "Start":
                                pif.startDate = Convert.ToDateTime(cell.Value);
                                break;

                            case "Ship":
                                pif.endDate = Convert.ToDateTime(cell.Value);
                                break;

                            //case "PM":
                            //    pif.pm = cell.DisplayValue;
                            //    break;

                            //case "Assigned to":
                            //    pif.assignedTo = cell.DisplayValue;
                            //    break;

                            case "Collab Deck":
                                pif.collabDeck = cell.DisplayValue;
                                break;

                            case "FxF Deck":
                                pif.fxfDeck = cell.DisplayValue;
                                break;

                            case "PSDs":
                                pif.PSDs = cell.DisplayValue;
                                break;

                            case "Final Delivery":
                                pif.finalDeliveryFolder = cell.DisplayValue;
                                break;

                            case "WBS":
                                pif.wbs_link = cell.DisplayValue;
                                break;

                            case "Deliverables Tracker":
                                pif.deliverables_tracker_link = cell.DisplayValue;
                                break;

                            case "Description":
                                pif.description = cell.DisplayValue;
                                break;

                            case "Total Deliverables":
                                pif.deliverables = cell.DisplayValue;
                                break;

                            case "Specs":

                                // List<SelectListItem> selectedvalues = new List<SelectListItem>();

                                if (cell.DisplayValue != null)
                                {
                                    List<string> selectedvalues = new List<string>();

                                    var list = cell.DisplayValue.Split(",");

                                    foreach (var spec in list)
                                    {
                                        IEnumerable<SelectListItem> variable = pif.SpecsList.Where(x => x.Text.Contains(spec.TrimStart(' ')));

                                        var id = "";

                                        foreach (var i in variable)
                                        {
                                            id = i.Value;
                                        }

                                        //selectedvalues.Add(new SelectListItem { Value = id, Text = spec, Selected = true });
                                        selectedvalues.Add(id);

                                        variable = null;
                                    }
                                    pif.SelectedSpecs = selectedvalues;
                                }
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
            pif.status_options = Get_status_picklist(sheet.GetColumnByIndex(3));

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

                            case "Tenrox Code":
                                pif.tenroxCode = cell.DisplayValue;
                                break;

                            case "Project":
                                pif.projectName = cell.DisplayValue;
                                break;

                            case "Status":    
                                pif.status = cell.DisplayValue;
                                break;

                            case "Type of Work":
                                pif.typeOfWork = cell.DisplayValue;
                                break;

                            case "Start":
                                pif.startDate = Convert.ToDateTime(cell.Value);
                                break;

                            case "Ship":
                                pif.endDate = Convert.ToDateTime(cell.Value);
                                break;

                            case "Assigned to":
                                pif.assignedTo = cell.DisplayValue;
                                break;

                            case "PM":
                                pif.pm = cell.DisplayValue;
                                break;

                            case "Creative Lead":
                                pif.creativeLead = cell.DisplayValue;
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


                            case "Collab Deck":
                                pif.collabDeck = cell.DisplayValue;
                                break;

                            case "FxF Deck":
                                pif.fxfDeck = cell.DisplayValue;
                                break;

                            case "PSDs":
                                pif.PSDs = cell.DisplayValue;
                                break;

                            case "Final Delivery":
                                pif.finalDeliveryFolder = cell.DisplayValue;
                                break;

                            case "Description":
                                pif.description = cell.DisplayValue;
                                break;

                            case "Total Deliverables":
                                pif.deliverables = cell.DisplayValue;
                                break;

                            case "Specs":
                            
                                //TBD
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
                        project_cell.Value = pif.projectName;
                        break;

                    case "Tenrox Code":
                        tenrox_cell.ColumnId = columnid;
                        tenrox_cell.Value = pif.tenroxCode;
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

                    //case "Type of Work":
                    //    type_cell.ColumnId = columnid;
                    //    type_cell.Value = pif.typeOfWork;
                    //    break;

                    //case "Assigned to":
                    //    assigned_cell.ColumnId = columnid;
                    //    assigned_cell.Value = pif.assignedTo;
                    //    break;

                    //case "PM":
                    //    pm_cell.ColumnId = columnid;
                    //    pm_cell.Value = pif.pm;
                    //    break;

                    case "WBS":
                        wbs_cell.ColumnId = columnid;
                        wbs_cell.Value = pif.wbs_link;
                        break;

                    case "Deliverables Tracker":
                        deliverables_tracker_cell.ColumnId = columnid;
                        deliverables_tracker_cell.Value = pif.deliverables_tracker_link;
                        break;

                    //case "Collab Deck":
                    //    collab_cell.ColumnId = columnid;
                    //    collab_cell.Value = pif.collabDeck;
                    //    break;

                    case "FxF Deck":
                        fxf_cell.ColumnId = columnid;
                        fxf_cell.Value = pif.fxfDeck;
                        break;

                    case "PSDs":
                        psds_cell.ColumnId = columnid;
                        psds_cell.Value = pif.PSDs;
                        break;

                    case "Final Delivery":
                        final_delivery_cell.ColumnId = columnid;
                        final_delivery_cell.Value = pif.finalDeliveryFolder;
                        break;

                    case "Total Deliverables":
                        deliverables_cell.ColumnId = columnid;
                        deliverables_cell.Value = pif.deliverables;
                        break;

                    case "Description":
                        description_cell.ColumnId = columnid;
                        description_cell.Value = pif.description;
                        break;


                    case "Specs":
                        specs_cell.ColumnId = columnid;
                        ObjectValue objct = null;
                        bool flag = false;
                        if (pif.SelectedSpecs != null)
                        {
                            foreach (var size in pif.SelectedSpecs)
                            {
                                if (size.Contains("System.String"))
                                {
                                    flag = true;
                                }
                            }
                            if (flag)
                            {
                                pif.SelectedSpecs.RemoveAt(pif.SelectedSpecs.Count() - 1);
                            }

                            objct = new MultiPicklistObjectValue(pif.SelectedSpecs);
                        }
                        specs_cell.ObjectValue = objct;

                        break;

                }
            }

            rowToTupdate = new Row
            {
                Id = pif.pif_Id,
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
            PDSModel pif = new PDSModel();
            pif.pif_Id = row_id;
            Sheet sheet = LoadSheet(sheetId, initSheet());

            pif.lob_options = Get_lobs_picklist(sheet.GetColumnByIndex(0));
           
            pif.status_options = Get_status_picklist(sheet.GetColumnByIndex(3));

            pif.SpecsList = Get_Specs_List(sheet.GetColumnByIndex(17));

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
