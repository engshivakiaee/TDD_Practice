﻿using DeskBooker.Core.Domain;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DeskBooker.DataAccess
{
    public class DeskBookerContext : DbContext
    {
        public DeskBookerContext(DbContextOptions<DeskBookerContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<DeskBooking> DeskBooking { get; set; }
        public DbSet<Desk> Desk { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Desk>().HasData(new Desk()
            {
                Id=1,
                 Description="Desk 1"
            });

            modelBuilder.Entity<Desk>().HasData(new Desk()
            {
                Id = 2,
                Description = "Desk 2"
            });
        }
    }
}
