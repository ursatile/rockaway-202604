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
	public class VenuesModel : PageModel {
		private readonly Rockaway.WebApp.Data.RockawayDbContext _context;

		public VenuesModel(Rockaway.WebApp.Data.RockawayDbContext context) {
			_context = context;
		}

		public IList<Venue> Venue { get; set; } = default!;

		public async Task OnGetAsync() {
			Venue = await _context.Venues.ToListAsync();
		}
	}
}
