using AngleSharp;
using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;

namespace Rockaway.WebApp.Tests;

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
