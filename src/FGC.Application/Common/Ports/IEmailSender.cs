

namespace FGC.Application.Common.Ports;
/// <summary>
/// Abstração para envio de e-mails no sistema.
/// </summary>
public interface IEmailSender
{
    /// <summary>
    /// Envia um e-mail para o destinatário especificado.
    /// </summary>
    Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default);
}