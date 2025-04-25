using System.ComponentModel.DataAnnotations;

namespace PV260.Project.Infrastructure.Persistence.Models;

public class ReportEntity
{
    [Key]
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<ReportHoldingEntity> Holdings { get; set; } = new List<ReportHoldingEntity>();

    public virtual ICollection<ReportChangeEntity> Changes { get; set; } = new List<ReportChangeEntity>();
}

