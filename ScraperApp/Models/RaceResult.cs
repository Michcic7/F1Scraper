using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScraperApp.Models;

internal class RaceResult
{
    public int Id { get; set; }
    public int Position { get; set; }
    public Circuit Circuit { get; set; }
    public DateOnly Date { get; set; }
    public Driver Driver { get; set; }
    public Team Team { get; set; }
    public int Laps { get; set; }
    public string Time { get; set; }
    public float Points { get; set; }
    public int Year { get; set; }
}
