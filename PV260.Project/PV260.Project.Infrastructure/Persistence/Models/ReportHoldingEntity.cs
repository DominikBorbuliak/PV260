using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PV260.Project.Infrastructure.Persistence.Models;
public class ReportHoldingEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid ReportId { get; set; }

    [ForeignKey(nameof(ReportId))]
    public virtual ReportEntity? Report { get; set; }

    public string Ticker { get; set; } = string.Empty;

    public string Company { get; set; } = string.Empty;

    public int Shares { get; set; }

    public decimal Weight { get; set; }
}

