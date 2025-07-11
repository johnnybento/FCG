
using FGC.Application.Common.Ports;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace FGC.Infrastructure.Services.Email;
/// <summary>
/// Implementação de IEmailSender usando SMTP.
/// Configurações em appsettings.json (Smtp:Host, Smtp:Port, Smtp:User, Smtp:Password, Smtp:From, Smtp:EnableSsl).
/// </summary>
public class SmtpEmailSender : IEmailSender
{
    private readonly SmtpClient _client;
    private readonly string _from;

    public SmtpEmailSender(IConfiguration config)
    {
        var host = config["Smtp:Host"];
        var port = int.Parse(config["Smtp:Port"]!);
        var user = config["Smtp:User"];
        var pass = config["Smtp:Password"];
        _from = config["Smtp:From"];
        var enableSsl = bool.Parse(config["Smtp:EnableSsl"] ?? "true");

        _client = new SmtpClient(host, port)
        {
            Credentials = new NetworkCredential(user, pass),
            EnableSsl = enableSsl
        };
    }

    public async Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
    {
        using var mail = new MailMessage(_from, to, subject, body) { IsBodyHtml = true };
        await _client.SendMailAsync(mail, cancellationToken);
    }
}
