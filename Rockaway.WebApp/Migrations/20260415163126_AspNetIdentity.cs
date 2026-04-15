using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rockaway.WebApp.Migrations {
	/// <inheritdoc />
	public partial class AspNetIdentity : Migration {
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder) {
			migrationBuilder.CreateTable(
				name: "IdentityRole",
				columns: table => new {
					Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
					Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
					NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
					ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
				},
				constraints: table => {
					table.PrimaryKey("PK_IdentityRole", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "IdentityUser",
				columns: table => new {
					Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
					UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
					NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
					Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
					NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
					EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
					PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
					SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
					ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
					PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
					PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
					TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
					LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
					LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
					AccessFailedCount = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table => {
					table.PrimaryKey("PK_IdentityUser", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "IdentityRoleClaim<string>",
				columns: table => new {
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
					ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
					ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
				},
				constraints: table => {
					table.PrimaryKey("PK_IdentityRoleClaim<string>", x => x.Id);
					table.ForeignKey(
						name: "FK_IdentityRoleClaim<string>_IdentityRole_RoleId",
						column: x => x.RoleId,
						principalTable: "IdentityRole",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "IdentityUserClaim<string>",
				columns: table => new {
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
					ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
					ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
				},
				constraints: table => {
					table.PrimaryKey("PK_IdentityUserClaim<string>", x => x.Id);
					table.ForeignKey(
						name: "FK_IdentityUserClaim<string>_IdentityUser_UserId",
						column: x => x.UserId,
						principalTable: "IdentityUser",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "IdentityUserLogin<string>",
				columns: table => new {
					LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
					ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
					ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
					UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
				},
				constraints: table => {
					table.PrimaryKey("PK_IdentityUserLogin<string>", x => new { x.LoginProvider, x.ProviderKey });
					table.ForeignKey(
						name: "FK_IdentityUserLogin<string>_IdentityUser_UserId",
						column: x => x.UserId,
						principalTable: "IdentityUser",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "IdentityUserRole<string>",
				columns: table => new {
					UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
					RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
				},
				constraints: table => {
					table.PrimaryKey("PK_IdentityUserRole<string>", x => new { x.UserId, x.RoleId });
					table.ForeignKey(
						name: "FK_IdentityUserRole<string>_IdentityRole_RoleId",
						column: x => x.RoleId,
						principalTable: "IdentityRole",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_IdentityUserRole<string>_IdentityUser_UserId",
						column: x => x.UserId,
						principalTable: "IdentityUser",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "IdentityUserToken<string>",
				columns: table => new {
					UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
					LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
					Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
					Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
				},
				constraints: table => {
					table.PrimaryKey("PK_IdentityUserToken<string>", x => new { x.UserId, x.LoginProvider, x.Name });
					table.ForeignKey(
						name: "FK_IdentityUserToken<string>_IdentityUser_UserId",
						column: x => x.UserId,
						principalTable: "IdentityUser",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.InsertData(
				table: "IdentityUser",
				columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
				values: new object[] { "rockaway-sample-admin-user", 0, "9c87461b-ba6e-412b-b91b-b7da20cc0b93", "admin@rockaway.dev", true, true, null, "ADMIN@ROCKAWAY.DEV", "ADMIN@ROCKAWAY.DEV", "AQAAAAIAAYagAAAAEPr4dAVFmCvx1RTdM2rKSQkd2JgORo+HtkTnflGJhNPzVL2aOHc4aNAbnmeGJglPyg==", null, true, "435634a8-a54b-4470-94c2-15bba2271941", false, "admin@rockaway.dev" });

			migrationBuilder.CreateIndex(
				name: "RoleNameIndex",
				table: "IdentityRole",
				column: "NormalizedName",
				unique: true,
				filter: "[NormalizedName] IS NOT NULL");

			migrationBuilder.CreateIndex(
				name: "IX_IdentityRoleClaim<string>_RoleId",
				table: "IdentityRoleClaim<string>",
				column: "RoleId");

			migrationBuilder.CreateIndex(
				name: "EmailIndex",
				table: "IdentityUser",
				column: "NormalizedEmail");

			migrationBuilder.CreateIndex(
				name: "UserNameIndex",
				table: "IdentityUser",
				column: "NormalizedUserName",
				unique: true,
				filter: "[NormalizedUserName] IS NOT NULL");

			migrationBuilder.CreateIndex(
				name: "IX_IdentityUserClaim<string>_UserId",
				table: "IdentityUserClaim<string>",
				column: "UserId");

			migrationBuilder.CreateIndex(
				name: "IX_IdentityUserLogin<string>_UserId",
				table: "IdentityUserLogin<string>",
				column: "UserId");

			migrationBuilder.CreateIndex(
				name: "IX_IdentityUserRole<string>_RoleId",
				table: "IdentityUserRole<string>",
				column: "RoleId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder) {
			migrationBuilder.DropTable(
				name: "IdentityRoleClaim<string>");

			migrationBuilder.DropTable(
				name: "IdentityUserClaim<string>");

			migrationBuilder.DropTable(
				name: "IdentityUserLogin<string>");

			migrationBuilder.DropTable(
				name: "IdentityUserRole<string>");

			migrationBuilder.DropTable(
				name: "IdentityUserToken<string>");

			migrationBuilder.DropTable(
				name: "IdentityRole");

			migrationBuilder.DropTable(
				name: "IdentityUser");
		}
	}
}