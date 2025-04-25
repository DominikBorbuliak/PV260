using PV260.Project.Domain.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace PV260.Project.Infrastructure.Persistence.Models;
public class ReportChangeEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid ReportId { get; set; }

    [ForeignKey(nameof(ReportId))]
    public virtual ReportEntity? Report { get; set; }

    public string Ticker { get; set; } = string.Empty;

    public string Company { get; set; } = string.Empty;

    public ChangeType ChangeType { get; set; }

    public int OldShares { get; set; }
    public int NewShares { get; set; }
}

