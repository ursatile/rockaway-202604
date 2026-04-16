// Rockaway.WebApp/Data/Sample/SampleData.Users.cs

using Microsoft.AspNetCore.Identity;

namespace Rockaway.WebApp.Data.Sample;

public static partial class SampleData {
	public static class Users {
		static Users() {
			Admin = new() {
				Id = "rockaway-sample-admin-user",
				Email = "admin@rockaway.dev",
				NormalizedEmail = "admin@rockaway.dev".ToUpperInvariant(),
				UserName = "admin@rockaway.dev",
				NormalizedUserName = "admin@rockaway.dev".ToUpperInvariant(),
				LockoutEnabled = true,
				EmailConfirmed = true,
				PhoneNumberConfirmed = true,
				SecurityStamp = "3c97071d-3e21-43a2-9d33-9d34bfdc362c",
				ConcurrencyStamp = "0e259722-7dca-478c-a5c0-b67923aeea8c",
				PasswordHash = "AQAAAAIAAYagAAAAEApY1qm1sD6qkAFm1wuBEV6+IdiTuGTqfmK6HWGPkG8MWRQ/fDljgEy39GRMp3VJ6g=="
			};
		}
		public static IdentityUser Admin { get; }
	}
}