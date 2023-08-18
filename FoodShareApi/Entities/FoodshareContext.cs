using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FoodShareApi.Entities;

public partial class FoodshareContext : DbContext
{
    public FoodshareContext()
    {
    }

    public FoodshareContext(DbContextOptions<FoodshareContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<DonationRequest> DonationRequests { get; set; }
    public virtual DbSet<Feeder> Feeders { get; set; }
    public virtual DbSet<Donor> Donors { get; set; }
    
    //don't think this is necessary
    public virtual DbSet<Password> Passwords { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.ToTable("user");

            entity.Property(e => e.username).HasMaxLength(45);
            entity.Property(e => e.contact_person).HasMaxLength(45);
            entity.Property(e => e.email_address).HasMaxLength(45);
            entity.Property(e => e.feeder_id);
            entity.Property(e => e.donor_id);
        });

        modelBuilder.Entity<Password>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.ToTable("password");

            entity.Property(e => e.password).HasMaxLength(100);
        });

        modelBuilder.Entity<DonationRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("donation_request");

            // entity.Property(e => e.RequestDate).HasColumnType("datetime");
            entity.Property(e => e.ItemName).HasMaxLength(45);
            entity.Property(e => e.ItemQuantity);
            entity.Property(e => e.ItemQuantityType).HasMaxLength(45);
            entity.Property(e => e.feeder_id);
            entity.Property(e => e.donor_id);
        });

        modelBuilder.Entity<Feeder>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.ToTable("feeder");

            // entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.organization).HasMaxLength(45);
            entity.Property(e => e.description).HasMaxLength(45);
            entity.Property(e => e.branch).HasMaxLength(45);
            entity.Property(e => e.zipCode);
            
        });

        modelBuilder.Entity<Donor>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.ToTable("donor");

            // entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.organization).HasMaxLength(45);
            entity.Property(e => e.branch).HasMaxLength(45);
            entity.Property(e => e.zipCode);
            
        });

        

        //Authorization
        //Adding two identity roles: feeder and donor
        modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole { Name = "Feeder", NormalizedName = "FEEDER", Id = Guid.NewGuid().ToString(), ConcurrencyStamp = Guid.NewGuid().ToString() });
        modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole { Name = "Donor", NormalizedName = "DONOR", Id = Guid.NewGuid().ToString(), ConcurrencyStamp = Guid.NewGuid().ToString() });
        

        OnModelCreatingPartial(modelBuilder);
         
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
