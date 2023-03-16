using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScraperApp.Models;

internal class RaceResult
{
    public int Id { get; set; }
    //public int CircuitID { get; set; }
    public string Date { get; set; }
    public int Laps { get; set; }
    public string Time { get; set; }

    //public int DriverID { get; set; }
    //public Driver Driver { get; set; }
}
