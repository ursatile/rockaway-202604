using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Rockaway.WebApp.Data;
using Rockaway.WebApp.Data.Entities;

namespace Rockaway.WebApp.Pages {
	public class ArtistsModel(RockawayDbContext context) : PageModel {
		public IList<Artist> Artists { get; set; } = null!;

		public async Task OnGetAsync() {
			Artists = await context.Artists.ToListAsync();
		}
	}
}
