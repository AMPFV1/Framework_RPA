using Microsoft.Office.Interop.Excel;
using _excel = Microsoft.Office.Interop.Excel;

namespace Framework_RPA
{
    public class DataFile
    {
        // Create an excel application object, workbook oject and worksheet object
        private _Application excel = new _excel.Application();
        private Workbook workbook;
        private Worksheet worksheet;

        // Method creates a new Excel file by creating a new Excel workbook with a single worksheet
        public void NewdataFile()
        {
            this.workbook = excel.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
            this.worksheet = this.workbook.Worksheets[1];
            this.worksheet.Name = "Testdaten";
            this.worksheet.Cells[1, 1] = "id";
            this.worksheet.Cells[1, 1].Font.Bold = true;

            this.worksheet.Cells[1, 2] = "Bezeichnung";
            this.worksheet.Cells[1, 2].Font.Bold = true;
        }

        // Method saves workbook at a specified path
        public void SaveAs(string path)
        {
            workbook.SaveAs(path);
        }

        // Method closes Excel file
        public void Close()
        {
            workbook.Close();
        }
    }
}
