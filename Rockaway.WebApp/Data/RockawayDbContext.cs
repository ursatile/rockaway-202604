// Rockaway.WebApp/Data/RockawayDbContext.cs

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rockaway.WebApp.Data.Entities;
using Rockaway.WebApp.Data.Sample;

namespace Rockaway.WebApp.Data;

// We must declare a constructor that takes a DbContextOptions<RockawayDbContext>
// if we want to use Asp.NET to configure our database connection and provider.
public class RockawayDbContext(DbContextOptions<RockawayDbContext> options)
	: IdentityDbContext(options) {

	public DbSet<Artist> Artists { get; set; } = null!;
	public DbSet<Venue> Venues { get; set; } = null!;


	protected override void OnModelCreating(ModelBuilder modelBuilder) {
		base.OnModelCreating(modelBuilder);

		foreach (var entity in modelBuilder.Model.GetEntityTypes()) {
			entity.SetTableName(entity.DisplayName());
		}

		modelBuilder.Entity<Artist>(entity => {
			entity.HasIndex(artist => artist.Slug).IsUnique();
		});

		modelBuilder.Entity<Artist>().HasData(SampleData.Artists.AllArtists);
		modelBuilder.Entity<Venue>().HasData(SampleData.Venues.AllVenues);

		// modelBuilder.Entity<IdentityUser>().HasData(SampleData.Users.Admin);

	}
}