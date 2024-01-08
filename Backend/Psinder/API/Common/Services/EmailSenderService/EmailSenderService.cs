using System.Net;
using System.Net.Mail;

namespace Psinder.API.Common;

public class EmailSenderService : IEmailSenderService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailSenderService> _logger;

    public EmailSenderService(
        IConfiguration configuration,
        ILogger<EmailSenderService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SendVerificationWorkerEmail(string to, string verificationCode, string password)
    {

        var site = _configuration.GetSection("Frontend:Path").Value;
        var url = @$"{site}/verify-registration-success?registerVerificationToken={verificationCode}";

        string mailTitle = "Psinder Team";
        string subject = "Psinder - Your message for registration verification!";
        var body = @$"<!DOCTYPE html>
                        <html>
                            <body style=""background -color:#ff7f26;text-align:center;"">
                            <h1 style=""color:#000;"">Welcome to Psinder!</h1>
                            <h2 style=""color:#000;"">This is your temporary password. Change it after first login: {password}</h2>
                            <h2 style=""color:#000;"">Click on the link below to complete your registration:</h2>
                            <a style=""text-decoration:none;all:unset;"" href=""{url}"">
                                <label style=""color:red;font-size:20px;border:5px dotted;border-radius:20px; padding:5px"">Activate your account</label>
                            </a>
                            <h2 style=""color:#000;"">Best regards,<br>
                                                      Psinder Team</h2>
                            </body>
                        </html>;";
        await SendEmail(to, mailTitle, subject, body);
    }


    public async Task SendVerificationEmail(string to, string verificationCode)
    {

        var site = _configuration.GetSection("Frontend:Path").Value;
        var url = @$"{site}/verify-registration-success?registerVerificationToken={verificationCode}";

        string mailTitle = "Psinder Team";
        string subject = "Psinder - Your message for registration verification!";
        var body = @$"<!DOCTYPE html>
                        <html>
                            <body style=""background -color:#ff7f26;text-align:center;"">
                            <h1 style=""color:#000;"">Welcome to Psinder!</h1>
                            <h2 style=""color:#000;"">Click on the link below to complete your registration:</h2>
                            <a style=""text-decoration:none;all:unset;"" href=""{url}"">
                                <label style=""color:red;font-size:20px;border:5px dotted;border-radius:20px; padding:5px"">Activate your account</label>
                            </a>
                            <h2 style=""color:#000;"">Best regards,<br>
                                                      Psinder Team</h2>
                            </body>
                        </html>;";
        await SendEmail(to, mailTitle, subject, body);
    }

    public async Task SendPasswordReminderEmail(string to, string verificationCode)
    {

        var site = _configuration.GetSection("Frontend:Path").Value;
        var url = @$"{site}/reset-password?remindPasswordToken={verificationCode}";

        string mailTitle = "Psinder Team";
        string subject = "Psinder - Reset your login password!";
        var body = @$"<!DOCTYPE html>
                        <html>
                            <body style=""background -color:#ff7f26;text-align:center;"">
                            <h1 style=""color:#000;"">Psinder Team here!</h1>
                            <h2 style=""color:#000;"">Click on the link below to reset your password:</h2>
                            <a style=""text-decoration:none;all:unset;"" href=""{url}"">
                                <label style=""color:red;font-size:20px;border:5px dotted;border-radius:20px; padding:5px"">Reset your password</label>
                            </a>
                            <h2 style=""color:#000;"">Best regards,<br>
                                                      Psinder Team</h2>
                            </body>
                        </html>;";
        await SendEmail(to, mailTitle, subject, body);
    }

    private async Task SendEmail(string to, string mailTitle, string subject, string body)
    {
        var serverName = _configuration.GetSection("StmpServer:Name").Value;
        var serverPort = Convert.ToInt32(_configuration.GetSection("StmpServer:Port").Value);
        var credentialEmail = _configuration.GetSection("StmpServer:Credential:Email").Value;
        var credentialPassword = _configuration.GetSection("StmpServer:Credential:AppPassword").Value;

        MailMessage message = new MailMessage(new MailAddress(credentialEmail, mailTitle), new MailAddress(to));
        message.Subject = subject;
        message.IsBodyHtml = true;
        message.Body = body;

        SmtpClient client = new SmtpClient(serverName);
        client.Port = serverPort;
        client.DeliveryMethod = SmtpDeliveryMethod.Network;
        client.UseDefaultCredentials = false;
        client.EnableSsl = true;
        client.Credentials = new NetworkCredential(credentialEmail, credentialPassword);

        try
        {
            await client.SendMailAsync(message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex?.Message);
            throw;
        }
    }


}
