using Rockaway.WebApp.Models;
using System.Threading.Channels;
using EasyNetQ;
using Rockaway.Messages;

namespace Rockaway.WebApp.Services.Mail;

public interface IMailQueue {
	public Task AddMailToQueueAsync(TicketOrderMailData item);
	public Task<TicketOrderMailData> FetchMailFromQueueAsync(CancellationToken token);
}

public class EasyNetTicketOrderMailQueue(IBus bus) : IMailQueue {
	public async Task AddMailToQueueAsync(TicketOrderMailData item) {
		var message = new TicketOrderMailMessage(
			item.CustomerEmail, item.Artist.Name, item.VenueName, item.Date.ToString());
		await bus.PubSub.PublishAsync(message);
	}

	public Task<TicketOrderMailData> FetchMailFromQueueAsync(CancellationToken token) {
		throw new NotImplementedException();
	}
}

public class ChannelBasedTicketOrderMailQueue : IMailQueue {

	private readonly Channel<TicketOrderMailData> channel;

	public ChannelBasedTicketOrderMailQueue(int capacity = 100) {
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