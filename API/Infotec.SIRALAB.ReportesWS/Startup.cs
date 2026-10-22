using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Infotec.SIRALAB.ReportesWS.DAL;
using Infotec.SIRALAB.ReportesWS.DAL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace Infotec.SIRALAB.ReportesWS
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers()
                .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddControllersWithViews()
                .AddJsonOptions(options =>
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
            services.RegisterDataServices(Configuration);
            services.AddTransient<IAnalisisService, AnalisisService>();
            services.AddTransient<IReportesService, ReportesService>();
            services.AddTransient<IUspReporteAnalisisHeader, UspReporteAnalisisHeaderService>();
            services.AddTransient<IUspReporteMensualHeader, UspReporteMensualHeader>();
            services.AddTransient<IConsecionService, ConsecionService>();
            services.AddTransient<ILaboratorioAcreditacionService, LaboratorioAcreditacionService>();
            services.AddTransient<ILaboratorioVigenciaService, LaboratorioVigenciaService>();
            services.AddTransient<IUspReporteMensualCalculoService, UspReporteMensualCalculosService>();
            services.AddTransient<IQrService, QrService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Infotec.SIRALAB.ReportesWS", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (!env.IsProduction())
            //{
            app.UseDeveloperExceptionPage();
            app.UseSwagger();

#if DEBUG
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Infotec.SIRALAB.ReportesWS v1"));
#else
            app.UseSwaggerUI(c => c.SwaggerEndpoint("../swagger/v1/swagger.json", "Infotec.SIRALAB.ReportesWS v1"));
#endif
            //}

            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"images")),
                RequestPath = new PathString("/images")
            });

            app.UseDirectoryBrowser(new DirectoryBrowserOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"images")),
                RequestPath = new PathString("/images")
            });

            //app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
