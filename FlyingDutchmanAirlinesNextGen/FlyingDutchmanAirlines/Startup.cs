using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines.RepositoryLayer.Interfaces;
using FlyingDutchmanAirlines.ServiceLayer;
using FlyingDutchmanAirlines.ServiceLayer.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace FlyingDutchmanAirlines;

public class Startup
{
    public void Configure(IApplicationBuilder app)
    {
        app.UseRouting();
        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddTransient<IFlightService, FlightService>();

        services.AddTransient<IFlightRepository, FlightRepository>();
        services.AddTransient<IAirportRepository, AirportRepository>();

        services.AddDbContext<FlyingDutchmanAirlinesContext>();
    }
}
