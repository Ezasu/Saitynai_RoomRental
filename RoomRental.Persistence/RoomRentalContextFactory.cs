using Microsoft.EntityFrameworkCore;

namespace RoomRental.Persistence
{
    public class RoomRentalContextFactory : DesignTimeDbContextFactoryBase<RoomRentalContext>
    {
        protected override RoomRentalContext CreateNewInstance(DbContextOptions<RoomRentalContext> options)
        {
            return new RoomRentalContext(options);
        }
    }
}
