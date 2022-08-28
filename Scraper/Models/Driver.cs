using System;
using System.Collections.Generic;
namespace Scraper.Models
{
    public class Driver
    {
        public int Id { get; set; }
        public int Position { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Nationality { get; set; }
        public string Team { get; set; }
        public int Points { get; set; }
    }
}
