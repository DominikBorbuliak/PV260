using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PV260.Project.Domain.Models;

public class ReportDiff
{
    public List<HoldingChange> Changes { get; set; } = new();
}
