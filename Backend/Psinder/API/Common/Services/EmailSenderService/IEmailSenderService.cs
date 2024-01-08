namespace Psinder.API.Common;

public interface IEmailSenderService
{
    public Task SendVerificationWorkerEmail(string to, string verificationToken, string password);

    public Task SendVerificationEmail(string to, string verificationToken);

    public Task SendPasswordReminderEmail(string to, string passwordReminderToken);
}
