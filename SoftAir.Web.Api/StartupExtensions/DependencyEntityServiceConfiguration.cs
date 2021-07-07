using Microsoft.Extensions.DependencyInjection;
using SoftAir.Services.Interfaces;
using SoftAir.Services.Services;

namespace SoftAir.Web.Api.StartupExtensions
{
    /// <summary>
    /// Dependency injection configurator for entity services
    /// </summary>
    public static partial class DependencyСonfigurator
    {
        /// <summary>
        /// Add dependency injection for services
        /// </summary>
        /// <param name="services"></param>
        public static void AddEntityServices(this IServiceCollection services)
        {
            services.AddScoped<IAircraftService, AircraftService>();
        }
    }
}
