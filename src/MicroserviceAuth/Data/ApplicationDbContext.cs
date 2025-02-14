﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MicroserviceAuth.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
	{ }

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);

		// Customize the ASP.NET Identity model and override the defaults if needed
		// Ad esempio, puoi rinominare le tabelle di ASP.NET Identity e altro
		// Aggiungi le tue personalizzazioni dopo aver chiamato base.OnModelCreating(builder);
	}
}
