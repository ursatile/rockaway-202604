using Rockaway.WebApp.Data.Entities;

namespace Rockaway.WebApp.Areas.Admin.Models;

public class ShowPostModel : ShowViewModel {
	public ShowPostModel() { }

	public ShowPostModel(Show show) : base(show) {
		HeadlineArtistId = show.HeadlineArtist?.Id ?? Guid.Empty;
		InvalidDates = show.Venue.Shows.Select(s => s.Date).ToList();
	}
	public Guid HeadlineArtistId { get; set; }

	public List<LocalDate> InvalidDates = [];
}