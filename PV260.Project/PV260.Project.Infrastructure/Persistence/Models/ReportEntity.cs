using System.ComponentModel.DataAnnotations;

namespace PV260.Project.Infrastructure.Persistence.Models;

public class ReportEntity
{
    [Key]
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public ICollection<ReportHoldingEntity> Holdings { get; set; } = new List<ReportHoldingEntity>();

    public ICollection<ReportChangeEntity> Changes { get; set; } = new List<ReportChangeEntity>();
}

