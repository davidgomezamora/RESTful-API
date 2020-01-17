using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ApplicationCore.Services;
using Infraestructure.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Versioning;
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
            /*
             * ---------------------------------------------------------
             * Configuración del servicio de serialización de datos diferentes a JSON
             * ---------------------------------------------------------
             */

            services.AddControllers(options => {
                // Definir en true si queremos que se retorne un código 406 Not Acceptable; cuando se solicite en la cabezera Accept, un formato no soportado
                // Definir en false si queremos que se retorne la información en el formato por omición y no se entregue un código 406 Not Acceptable
                options.ReturnHttpNotAcceptable = true;

                /*
                 * Respeta la cabezera HTTP (Accept), donde el cliente espesifica el formato en el que requiere recibir la información por parte del controlador/web API.
                 * Si el formato solicitado no es soportado, se retorna la información en el formato definido por omición por .Net Core, el cual es JSON.
                 */
                options.RespectBrowserAcceptHeader = true;

                // Adiciona soporte al formato XML; como serialización de retorno de datos, por parte del controlador/web API
                options.OutputFormatters.Add(
                    new XmlDataContractSerializerOutputFormatter());
            }).AddXmlDataContractSerializerFormatters();

            /*
             * ---------------------------------------------------------
             * Configuración del contexto de la base de datos y la cadena de conexión
             * ---------------------------------------------------------
             */

            // Establece el contexto de la base de datos y define la cadena de conexión establecida en el archivo appsettings.json
            services.AddDbContext<AdventureWorks2017Context>(options => {
                // ConnectionsString > ConnectionDatabase
                options.UseSqlServer(this.Configuration.GetConnectionString("ConnectionDatabase"));
            });

            /*
             * ---------------------------------------------------------
             * Configuración de los ambitos de la aplicación
             * ---------------------------------------------------------
             */

            /*
             * Repositorio
             * Requiere del paquete Nuget: Repository (David Andrés Gómez Zamora)
             */
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            /*
             *Contexto de la base de datos
             * Requiere del paquete Nuget: Microsoft.EntityFrameworkCore (Nuget.org), no es necesario instalarlo si se tiene instalado el paquete Repository (David Andrés Gómez Zamora)
             */
            services.AddScoped(typeof(DbContext), typeof(AdventureWorks2017Context));

            /*
             * Servicios de la capa ApplicationCore
             * Requiere la dependencia con la capa ApplicationCore
             */
            services.AddScoped(typeof(IProductService), typeof(ProductService));

            /*
             * ---------------------------------------------------------
             * Configuración del versionamiento de las web APIs/Controladores
             * ---------------------------------------------------------
             */

            /*
             * Versionamiento de una web API
             * Requiere del paquete Nuget: Microsoft.AspNetCore.Mvc.Versioning (Nuget.org)
             */
            services.AddApiVersioning(options => {
                // Cabezera HTTP, donde debe espesificarse la versión del web API a usar
                HeaderApiVersionReader multiVersionReader = new HeaderApiVersionReader("x-version");
                // Indica que en la petición señalamos qué versión de la API soporta la petición que hemos realizado.
                options.ReportApiVersions = true;

                // En caso de que no se notifique la versión en la petición, cómo tratamos dicha petición (si se envía un error o bien si asume la versión por defecto).
                options.AssumeDefaultVersionWhenUnspecified = true;
                // Versión por defecto la API
                options.DefaultApiVersion = new ApiVersion(1, 0);

                // Ubicación donde indicamos la versión, ya sea por QueryString o por HeaderAPIVersión
                options.ApiVersionReader = multiVersionReader;
            });

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
