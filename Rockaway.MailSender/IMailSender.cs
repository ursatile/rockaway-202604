// Rockaway.WebApp/Services/Mail/IMailSender.cs

using Rockaway.Messages;

namespace Rockaway.MailSender;

public interface IMailSender {
	Task<string> SendOrderConfirmationAsync(TicketOrderMailMessage data);
}