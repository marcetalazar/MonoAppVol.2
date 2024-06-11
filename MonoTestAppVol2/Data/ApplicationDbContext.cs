﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MonoTestAppVol2.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<VehicleModels.Vehicle> VehicleModels { get; set; }
    public DbSet<VehicleMake.Make> VehicleMakes { get; set; }

}

