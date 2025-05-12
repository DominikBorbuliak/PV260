using MimeKit.Text;

namespace PV260.Project.Domain.Models;

public class EmailConfiguration
{
    public required IEnumerable<string> Recipients { get; set; }

    public required string Message { get; set; }

    public required string Subject { get; set; }

    public TextFormat Format { get; set; } = TextFormat.Text;
}