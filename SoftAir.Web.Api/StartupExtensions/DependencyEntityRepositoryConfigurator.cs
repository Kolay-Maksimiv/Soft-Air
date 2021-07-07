using Microsoft.Extensions.DependencyInjection;
using SoftAir.Data.Abstract;
using SoftAir.Data.Domain.Aircraft;

namespace SoftAir.Web.Api.StartupExtensions
{
    /// <summary>
    /// Dependency injection configurator for entity repositories
    /// </summary>
    public static partial class DependencyСonfigurator
    {
        /// <summary>
        /// Add dependency injection for repositories
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static void AddEntityRepositories(this IServiceCollection services)
        {
            services.AddScoped<IGenericRepository<Aircraft>, GenericRepository<Aircraft>>();
        }
    }
}
