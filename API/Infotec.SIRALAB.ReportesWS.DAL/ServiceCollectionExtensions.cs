using Infotec.SIRALAB.ReportesWS.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infotec.SIRALAB.ReportesWS.DAL
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterDataServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<SiralabContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            
            return services;
        }
    }
}