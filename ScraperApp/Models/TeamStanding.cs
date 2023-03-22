using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScraperApp.Models;

internal class TeamStanding
{
    public int Id { get; set; }
    public Team Team { get; set; }
    public int Points { get; set; }
    public int Year { get; set; }
}
