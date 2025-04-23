namespace PV260.Project.Domain.Options.SMTP;

public class SMTPOptions
{
    public const string Key = "SMTP";

    public required string Host { get; set; }
    public required int Port { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}