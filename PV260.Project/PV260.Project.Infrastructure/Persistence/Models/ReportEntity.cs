using System.ComponentModel.DataAnnotations;

namespace PV260.Project.Infrastructure.Persistence.Models;

public class ReportEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string ReportJson { get; set; } = string.Empty;

    [Required]
    public string DiffJson { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

