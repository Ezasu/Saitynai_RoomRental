using Microsoft.EntityFrameworkCore;
using RoomRental.Application.Interfaces;
using RoomRental.Domain.Entities.RoomRental;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RoomRental.Persistence
{
    public class RoomRentalContext : DbContext, IRoomRentalContext
    {
        public RoomRentalContext(DbContextOptions<RoomRentalContext> options) : base(options) { }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<User> Users { get; set; }

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RoomRentalContext).Assembly);
        }
    }
}
