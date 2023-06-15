using CsvHelper.Configuration.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace AddressBook.Model
{
    [DebuggerDisplay("{Id} - {FirstName} {MiddleName} {City}")]
    public class Entry
    {
        [Key]
        [Ignore]
        public int Id { get; set; }

        [Index(0)] 
        public string? Date { get; set; }

        [Index(1)] 
        public string? FirstName { get; set; }

        [Index(2)] 
        public string? LastName { get; set; }

        [Index(3)] 
        public string? MiddleName { get; set; }

        [Index(4)] 
        public string? City { get; set; }

        [Index(5)] 
        public string? Country { get; set; }

        public override string? ToString()
        {
            return $"{Id} - {FirstName} {MiddleName} {City}";
        }
    }
}
