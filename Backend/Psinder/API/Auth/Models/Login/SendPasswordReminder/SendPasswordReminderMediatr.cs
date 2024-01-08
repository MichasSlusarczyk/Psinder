using MediatR;

namespace Psinder.API.Auth.Models.Logins;

public class SendPasswordReminderMediatr : IRequest<SendPasswordReminderResponse>
{
    public SendPasswordReminderRequest Request { get; set; }
}
