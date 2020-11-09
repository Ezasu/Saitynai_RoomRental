using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoomRental.Domain.Entities.RoomRental;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoomRental.Persistence.Configurations
{
    public class RoomsConfiguration : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).HasColumnName("ID").ValueGeneratedOnAdd();
        }
    }
}
