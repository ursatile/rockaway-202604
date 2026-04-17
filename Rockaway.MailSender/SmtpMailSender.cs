using MimeKit;
using Rockaway.Messages;

namespace Rockaway.MailSender;

public class SmtpMailSender(ISmtpRelay smtpRelay) : IMailSender {

	private readonly MailboxAddress mailFrom = new("Rockaway Tickets", "tickets@rockaway.dev");

	public MimeMessage BuildOrderConfirmationMail(TicketOrderMailMessage data) {
		var message = new MimeMessage();
		message.Subject = $"Your tickets to {data.ArtistName} at {data.VenueName}";
		message.From.Add(mailFrom);
		message.To.Add(new MailboxAddress(data.CustomerEmail, data.CustomerEmail));
		var bb = new BodyBuilder {
			HtmlBody = $"This is order confirmation {data.ShowDate} (in HTML!)",
			TextBody = $"This is order confirmation {data.ShowDate} (in plain text)"
		};
		message.Body = bb.ToMessageBody();
		return message;
	}

	public async Task<string> SendOrderConfirmationAsync(TicketOrderMailMessage data) {
		var message = BuildOrderConfirmationMail(data);
		return await smtpRelay.SendMailAsync(message);
	}
}