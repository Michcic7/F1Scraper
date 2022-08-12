using System;
using System.Collections.Generic;
namespace Scraper.Models
{
    public class Driver
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Nationality { get; set; }
        public string Born { get; set; }
        public string Team { get; set; }
        public int CarNumber { get; set; }
        public string Entries { get; set; }
        public int Wins { get; set; }
        public int Podiums { get; set; }
        public double CareerPoints { get; set; }
        public int PolePositions { get; set; }
        public int FastestLaps { get; set; }
        public string FirstEntry { get; set; }
        public string LastEntry { get; set; }
        public string Position { get; set; }
    }
}
