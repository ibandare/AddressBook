using AddressBook.Model;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AddressBook.Service.Impl
{
    internal class ExcelExportService : IExportService
    {
        public const string Type = "Excel";

        public async Task Export(Func<int, int, Task<List<Entry>>> provider)
        {

            using var outputStream = new FileStream("Data.xlsx", FileMode.OpenOrCreate);

            using var spreadsheetDocument = SpreadsheetDocument.Create(outputStream, SpreadsheetDocumentType.Workbook);

            var workbookPart = spreadsheetDocument.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();

            var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(new SheetData());

            var sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild(new Sheets());

            var sheet = new Sheet()
            {
                Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart),
                SheetId = 1,
                Name = "Data"
            };
            sheets.Append(sheet);

            var sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();
            await Util.ChunkedAction.Invoke(provider, async items =>
            {
                foreach (var item in items)
                {
                    var dataRow = BuildRow(item);
                    sheetData.AppendChild(dataRow);
                }
            });
            // Save the workbook
            workbookPart.Workbook.Save();

        }

        private static Row BuildRow(Entry item)
        {
            var row = new Row();

            new List<Cell>{
                BuildStringCell(item.Date),
                BuildStringCell(item.FirstName),
                BuildStringCell(item.LastName),
                BuildStringCell(item.MiddleName),
                BuildStringCell(item.City),
                BuildStringCell(item.Country)
            }.ForEach(cell => row.AppendChild(cell));
            return row;
        }

        private static Cell BuildStringCell(string value)
        {
            return new Cell()
            {
                DataType = CellValues.String,
                CellValue = new CellValue(value)
            };
        }

        public bool Supports(string token)
        {
            return token == Type;
        }

    }
}
