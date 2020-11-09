using Microsoft.EntityFrameworkCore;
using RoomRental.Domain.Entities.RoomRental;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RoomRental.Application.Interfaces
{
    public interface IRoomRentalContext
    {
        DbSet<Room> Rooms { get; set; }
        DbSet<Building> Buildings { get; set; }
        DbSet<Reservation> Reservations { get; set; }
        DbSet<User> Users { get; set; }
        Task<int> SaveChangesAsync();
    }
}
