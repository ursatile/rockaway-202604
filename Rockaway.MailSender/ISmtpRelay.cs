using MimeKit;

namespace Rockaway.MailSender;

public interface ISmtpRelay {
	Task<string> SendMailAsync(MimeMessage message);
}