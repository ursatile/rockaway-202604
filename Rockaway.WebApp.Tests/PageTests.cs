using AngleSharp;
using Microsoft.AspNetCore.Mvc.Testing;
using Rockaway.WebApp.Services;
using Shouldly;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;

namespace Rockaway.WebApp.Tests;

public class ServiceTests {

	private static ServerStatus CreateTestStatus() {
		return new ServerStatus() {
			Assembly = "TEST_ASSEMBLY",
			Modified = new DateTimeOffset(2021, 2, 3, 4, 5, 6, TimeSpan.Zero).ToString("O"),
			Hostname = "TEST_HOST",
			DateTime = new DateTimeOffset(2022, 3, 4, 5, 6, 7, TimeSpan.Zero).ToString("O")
		};
	}

	private class TestStatusReporter : IReportServerStatus {
		public ServerStatus GetStatus() => CreateTestStatus();
	}

	private static readonly JsonSerializerOptions jsonSerializerOptions
		= new(JsonSerializerDefaults.Web);

	[Fact]
	public async Task Status_Reporter_Has_Correct_Build_Time() {
		await using var factory = new WebApplicationFactory<Program>()
			.WithWebHostBuilder(builder => builder.ConfigureServices(services =>
				services.AddSingleton<IReportServerStatus>(new TestStatusReporter())));
		using var client = factory.CreateClient();
		var json = await client.GetStringAsync("/status");
		var status = JsonSerializer.Deserialize<ServerStatus>(json, jsonSerializerOptions);
		var testStatus = CreateTestStatus();
		status.ShouldNotBeNull();
		status.Hostname.ShouldBe(testStatus.Hostname);
		status.Assembly.ShouldBe(testStatus.Assembly);

	}
}

public class PageTests {

	static readonly IBrowsingContext browsingContext
		= BrowsingContext.New(Configuration.Default);

	[Fact]
	public async Task Index_Page_Returns_Success() {
		await using var factory = new WebApplicationFactory<Program>();
		using var client = factory.CreateClient();
		using var response = await client.GetAsync("/");
		response.EnsureSuccessStatusCode();
	}

	[Fact]
	public async Task ContactUs_Page_Returns_Success() {
		await using var factory = new WebApplicationFactory<Program>();
		using var client = factory.CreateClient();
		using var response = await client.GetAsync("/contact");
		response.EnsureSuccessStatusCode();
	}

	[Theory]
	[InlineData("/", "Rockaway")]
	[InlineData("/contact", "Contact Us")]
	[InlineData("/privacy", "Privacy Policy")]
	public async Task Homepage_Title_Has_Correct_Content(string path, string title) {
		await using var factory = new WebApplicationFactory<Program>();
		using var client = factory.CreateClient();
		var html = await client.GetStringAsync(path);
		var dom = await browsingContext.OpenAsync(req => req.Content(html));
		var titleElement = dom.QuerySelector("title");
		titleElement.ShouldNotBeNull();
		titleElement.InnerHtml.ShouldBe(title);
	}

	[Fact]
	public async Task Contact_Us_Has_Valid_Email() {
		await using var factory = new WebApplicationFactory<Program>();
		using var client = factory.CreateClient();
		var html = await client.GetStringAsync("/contact");
		var dom = await browsingContext.OpenAsync(req => req.Content(html));
		var emailLink = dom.QuerySelector("#contact-us-email-address");
		emailLink.ShouldNotBeNull();
		emailLink.Attributes["href"].Value.ShouldContain("@");
	}
}
