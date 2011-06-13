using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Office.Interop.Excel;
using System.Data;
using System.IO;
using GemBox.Spreadsheet;

namespace WebApplication_OLAP
{
    public class ExcelManager
    {
        // load data to excel
        public bool ExcelExport(System.Data.DataTable objTable, System.Data.DataTable objTableNodes, string sFileName)
        {
            ExcelFile ef = new ExcelFile();

            try
            {
                CreateExcelFile(sFileName);

                // Load Excel file.
                ef.LoadXls(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + sFileName);

                // Select the first worksheet from the file.
                ExcelWorksheet ws = ef.Worksheets[0];

                // Extract the data from the worksheet to the DataTable.
                // Data is extracted starting at first row and first column for 10 rows or until the first empty row appears.
                // ws.ExtractToDataTable(objTable, 10, ExtractDataOptions.StopAtFirstEmptyRow, ws.Rows[0], ws.Columns[0]);

                // Insert the data from DataTable to the worksheet starting at cell "A1"
                ws.InsertDataTable(objTable, "D5", true);

                if (objTableNodes != null)
                    ws.InsertDataTable(objTableNodes, "R5", true);

                // Save the file to XLS format.
                ef.SaveXls(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + sFileName);

                // open the excel file
                //object misValue = System.Reflection.Missing.Value;
                //Application appExcel = new Application();
                //Workbook wbExcel = appExcel.Workbooks.Open(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + sFileName, misValue, misValue, misValue, misValue, misValue, misValue, misValue, misValue, misValue, misValue, misValue, misValue, misValue, misValue);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        // create excel file
        private void CreateExcelFile(string sName)
        {
            Microsoft.Office.Interop.Excel.Application xlApp;
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlApp = new Microsoft.Office.Interop.Excel.ApplicationClass();
            xlWorkBook = xlApp.Workbooks.Add(misValue);

            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            xlWorkSheet.Cells[2, 2] = "Olap Mining Report";

            xlWorkBook.SaveAs(sName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();
        }
    }
}
