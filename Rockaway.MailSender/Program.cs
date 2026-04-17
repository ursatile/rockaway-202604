using System.Text.Json;
using EasyNetQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NodaTime;
using Rockaway.MailSender;

await Host.CreateDefaultBuilder(args)
	.ConfigureServices((host, services) => {
		var smtpSettings
			= host.Configuration.GetSection("Smtp").Get<SmtpSettings>()
			  ?? new SmtpSettings();

		services.AddSingleton(smtpSettings);

		services.AddSingleton<IClock>(SystemClock.Instance);
		services.AddTransient<IMailSender, SmtpMailSender>();
		services.AddSingleton<ISmtpRelay, SmtpRelay>();
		services.AddSingleton(smtpSettings);

		const string AMQP = "host=localhost";
		var bus = RabbitHutch.CreateBus(AMQP, options => options.EnableSystemTextJson());
		services.AddSingleton(bus);
		services.AddHostedService<TicketMailerBackgroundService>();
	})
	.Build().RunAsync();
