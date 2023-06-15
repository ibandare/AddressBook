using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using AddressBook.Model;
using System.Xml;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Threading.Tasks;

namespace AddressBook.Service.Impl
{
    public class XmlExportService : IExportService
    {
        public const string Type = "XML";

        public async Task Export(Func<int, int, Task<List<Entry>>> provider)
        {
            using XmlTextWriter textWriter = new("Data.xml", null);
            
            textWriter.WriteStartDocument();
           
            textWriter.WriteStartElement("TestProgram");

            await Util.ChunkedAction.Invoke(provider, async items =>
            {
                foreach (Entry entry in items)
                {
                    textWriter.WriteStartElement("Record");
                    textWriter.WriteAttributeString("id", entry.Id.ToString());
                    textWriter.WriteElementString("Date", entry.Date);
                    textWriter.WriteElementString("FirstName", entry.FirstName);
                    textWriter.WriteElementString("LastName", entry.LastName);
                    textWriter.WriteElementString("MiddleName", entry.MiddleName);
                    textWriter.WriteElementString("City", entry.City);
                    textWriter.WriteElementString("Country", entry.Country);
                    textWriter.WriteEndElement();
                }
            });

            textWriter.WriteEndElement();
            textWriter.WriteEndDocument();
            textWriter.Close();
        }

        public bool Supports(string token)
        {
            return token == Type;
        }
    }
}