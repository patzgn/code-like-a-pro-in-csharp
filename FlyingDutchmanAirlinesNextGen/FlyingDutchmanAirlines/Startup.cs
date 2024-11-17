using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines.RepositoryLayer.Interfaces;
using FlyingDutchmanAirlines.ServiceLayer;
using FlyingDutchmanAirlines.ServiceLayer.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace FlyingDutchmanAirlines;

public class Startup
{
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRouting();
        app.UseEndpoints(endpoints => endpoints.MapControllers());

        app.UseSwagger();
        app.UseSwaggerUI(swagger =>
            swagger.SwaggerEndpoint("/swagger/v1/swagger.json", "FlyingDutchmanAirlines"));
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddTransient<IBookingService, BookingService>();
        services.AddTransient<IFlightService, FlightService>();

        services.AddTransient<IAirportRepository, AirportRepository>();
        services.AddTransient<IBookingRepository, BookingRepository>();
        services.AddTransient<ICustomerRepository, CustomerRepository>();
        services.AddTransient<IFlightRepository, FlightRepository>();

        services.AddDbContext<FlyingDutchmanAirlinesContext>();

        services.AddSwaggerGen();
    }
}
