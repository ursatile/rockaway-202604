using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Rockaway.WebApp.Data;
using Rockaway.WebApp.Data.Entities;

namespace Rockaway.WebApp.Pages {
	public class VenuesModel(Rockaway.WebApp.Data.RockawayDbContext context) : PageModel {
		public IList<Venue> Venue { get; set; } = default!;

		public async Task OnGetAsync() {
			Venue = await context.Venues.ToListAsync();
		}
	}
}
