using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScraperApp.Models;

internal class DriverStanding
{
    public int DriverStandingId { get; set; }
    public int Position { get; set; }
    public Driver Driver { get; set; }
    public Team Team { get; set; }
    public float Points { get; set; }
    public int Year { get; set; }
}
