using FlyingDutchmanAirlines.DatabaseLayer;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines_Tests.Stubs;

public class FlyingDutchmanAirlinesContext_Stub : FlyingDutchmanAirlinesContext
{
    public FlyingDutchmanAirlinesContext_Stub(DbContextOptions<FlyingDutchmanAirlinesContext> options)
        : base(options)
    {
        base.Database.EnsureDeleted();
    }

    public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await base.SaveChangesAsync(cancellationToken);

        return base.Bookings.First().CustomerId switch
        {
            1 => 1,
            _ => throw new Exception("Database Error!")
        };
    }
}
