using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScraperApp.Models;

internal class DriverStanding
{
    public int Id { get; set; }
    public int Position { get; set; }
    public int DriverId { get; set; }
    public Driver Driver { get; set; }
    public int TeamId { get; set; }
    public Team Team { get; set; }
    public int Points { get; set; }
}
