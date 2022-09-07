using DevExpress.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MayCatServer
{
    static class PrintHelper
    {
        public static void PrintExcel(string file)
        {
            try
            {
                // Create a new Workbook object.
                Workbook workbook = new Workbook();

                // Load a document from a file.
                workbook.LoadDocument(file);

                // Create an object that contains printer settings.
                PrinterSettings printerSettings = new PrinterSettings();
                printerSettings.PrintToFile = false;

                // Specify that the first three pages should be printed.
                printerSettings.PrintRange = PrintRange.AllPages;

                // Set the number of copies to print.
                printerSettings.Copies = 1;

                // Print the workbook using the specified printer settings.
                workbook.Print(printerSettings);

                workbook.Dispose();
            }
            catch { }
        }
    }
}
