using Rockaway.WebApp.Models;
using System.Collections;
using System.Threading.Channels;

namespace Rockaway.WebApp.Services.Mail;

public interface IMailQueue {
	public Task AddMailToQueueAsync(TicketOrderMailData item);
	public Task<TicketOrderMailData> FetchMailFromQueueAsync(CancellationToken token);
}

public class TicketOrderMailQueue : IMailQueue {

	private readonly Channel<TicketOrderMailData> channel;

	public TicketOrderMailQueue(int capacity = 100) {
		var options = new BoundedChannelOptions(capacity) {
			FullMode = BoundedChannelFullMode.Wait
		};
		channel = Channel.CreateBounded<TicketOrderMailData>(options);
	}

	public async Task AddMailToQueueAsync(TicketOrderMailData data)
		=> await channel.Writer.WriteAsync(data);

	public async Task<TicketOrderMailData> FetchMailFromQueueAsync(CancellationToken token)
		=> await channel.Reader.ReadAsync(token);
}

public class TicketMailerBackgroundService(
	IMailQueue queue,
	ILogger<TicketMailerBackgroundService> logger,
	IMailSender mailSender) : BackgroundService {
	public override Task StartAsync(CancellationToken cancellationToken) {
		logger.LogInformation($"Starting TicketMailerBackgroundService");
		return base.StartAsync(cancellationToken);
	}

	public override Task StopAsync(CancellationToken cancellationToken) {
		logger.LogInformation($"Stopping TicketMailerBackgroundService");
		return base.StopAsync(cancellationToken);
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		=> await ProcessMailQueue(stoppingToken);

	private async Task ProcessMailQueue(CancellationToken token) {
		while (!token.IsCancellationRequested) {
			try {
				var mailItem = await queue.FetchMailFromQueueAsync(token);
				await mailSender.SendOrderConfirmationAsync(mailItem);
				logger.LogInformation("Sent order mail # {ref} to {email}", mailItem.Reference, mailItem.CustomerEmail);
			} catch (OperationCanceledException) {
				logger.LogDebug("Exiting ProcessMailQueue due to OperationCanceledException (this is fine!)");
				break;
			}
		}
	}
}
