using MimeKit;
using Rockaway.WebApp.Models;

namespace Rockaway.WebApp.Services.Mail;

public class SmtpMailSender(ISmtpRelay smtpRelay) : IMailSender {

	private readonly MailboxAddress mailFrom = new("Rockaway Tickets", "tickets@rockaway.dev");

	public MimeMessage BuildOrderConfirmationMail(TicketOrderMailData data) {
		var message = new MimeMessage();
		message.Subject = $"Your tickets to {data.Artist.Name} at {data.VenueName}";
		message.From.Add(mailFrom);
		message.To.Add(new MailboxAddress(data.CustomerName, data.CustomerEmail));
		var bb = new BodyBuilder {
			HtmlBody = $"This is order confirmation {data.Reference} (in HTML!)",
			TextBody = $"This is order confirmation {data.Reference} (in plain text)"
		};
		message.Body = bb.ToMessageBody();
		return message;
	}

	public async Task<string> SendOrderConfirmationAsync(TicketOrderMailData data) {
		var message = BuildOrderConfirmationMail(data);
		return await smtpRelay.SendMailAsync(message);
	}
}