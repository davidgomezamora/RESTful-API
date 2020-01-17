using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Repository;

namespace WebAPI
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
            services.AddControllers();

            /*
             * ---------------------------------------------------------
             * Configuración del contexto de la base de datos y la cadena de conexión
             * ---------------------------------------------------------
             */

            // Establece el contexto de la base de datos y define la cadena de conexión establecida en el archivo appsettings.json
            /*services.AddDbContext<DatabaseContext>(options => {
                options.UseSqlServer(this.Configuration.GetConnectionString("ConnectionDatabase"));
            });*/

            /*
             * ---------------------------------------------------------
             * Configuración de los ambitos de la aplicación
             * ---------------------------------------------------------
             */

            // Repositorio
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            // Contexto de la base de datos
            // services.AddScoped(typeof(DbContext), typeof(DatabaseContext));

            // Servicios de la capa ApplicationCore
            // services.AddScoped(typeof(IBlogService), typeof(BlogService));

            /*
             * ---------------------------------------------------------
             * Configuración del versionamiento de las web APIs/Controladores
             * ---------------------------------------------------------
             */

            /*services.AddApiVersioning(options => {
                // Cabezera HTTP, donde debe espesificarse la versión del web API a usar
                HeaderApiVersionReader multiVersionReader = new HeaderApiVersionReader("api-version");
                // Indica que en la petición señalamos qué versión de la API soporta la petición que hemos realizado.
                options.ReportApiVersions = true;

                // En caso de que no se notifique la versión en la petición, cómo tratamos dicha petición (si se envía un error o bien si asume la versión por defecto).
                options.AssumeDefaultVersionWhenUnspecified = true;
                // Versión por defecto la API
                options.DefaultApiVersion = new ApiVersion(2, 0);

                // Ubicación donde indicamos la versión, ya sea por QueryString o por HeaderAPIVersión
                options.ApiVersionReader = multiVersionReader;
            });*/

            /*
             * ---------------------------------------------------------
             * Configuración del servicio de serialización de datos
             * ---------------------------------------------------------
             */

            /*services.AddMvc(options => {
                // Respeta la cabezera HTTP (Accept), donde el cliente espesifica el formato en el que requiere recibir la información por parte del controlador/web API.
                // Si el formato solicitado no es soportado, se retorna la información en el formato definido por omición por .Net Core, el cual es JSON.

                options.RespectBrowserAcceptHeader = true;
                // Adiciona soporte al formato XML; como serialización de retorno de datos, por parte del controlador/web API
                options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
            });*/

            // Adiciona soporte al formato XML; como serialización de entrada de datos, por parte del controlador/web API
            // services.AddMvc().AddXmlSerializerFormatters();

            /*
             * ---------------------------------------------------------
             * Configuración del servicio de seguridad => CORS
             * ---------------------------------------------------------
             */

            services.AddCors(options =>
            {
                // Cors para uso en entorno de producción
                options.AddPolicy("Production", builder =>
                {
                    builder.WithOrigins("https://www.xyz.com", "https://xyz.com").AllowAnyHeader().AllowAnyMethod();
                });

                // Cors para uso en entorno de desarrollo
                options.AddPolicy("Development", builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // Usa el CORS espesificado
                app.UseCors("Development");
            } else
            {
                // Usa el CORS espesificado
                app.UseCors("Production");
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
