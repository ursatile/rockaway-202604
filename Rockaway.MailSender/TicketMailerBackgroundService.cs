using EasyNetQ;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rockaway.Messages;

namespace Rockaway.MailSender;

public class TicketMailerBackgroundService(
	IBus bus,
	ILogger<TicketMailerBackgroundService> logger,
	IMailSender mailSender) : IHostedService {
	public async Task StartAsync(CancellationToken cancellationToken) {
		logger.LogInformation($"Starting TicketMailerBackgroundService");

		await bus.PubSub.SubscribeAsync<TicketOrderMailMessage>(
			"rockaway.mailsender",
			async data => await SendMailAsync(data));

	}

	private async Task SendMailAsync(TicketOrderMailMessage mail) {
		await mailSender.SendOrderConfirmationAsync(mail);
	}


	public Task StopAsync(CancellationToken cancellationToken) {
		logger.LogInformation($"Stopping TicketMailerBackgroundService");
		return Task.CompletedTask;
	}
}
