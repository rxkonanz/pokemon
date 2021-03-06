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
using PokeApi.Data;
using PokeApi.Repositories;

namespace PokeApi
{
    public class Startup
    {
        string SpecificOriginsPolicy = "_specificOriginsPolicy";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            
            string connectionString = Configuration.GetConnectionString("Default");
            string myAllowSpecificOrigins = Configuration.GetValue<string>("MyAllowSpecificOrigins");

            services.AddCors(options => 
                options.AddPolicy(name: SpecificOriginsPolicy,
                    builder =>
                    {
                        builder.WithOrigins(myAllowSpecificOrigins);
                    })
            );

            services.AddDbContext<ApplicationContext>(options =>
                options.UseNpgsql(connectionString)
            );

            services.AddTransient<IDataService, DataService>();
            services.AddTransient<IPokemonRepository, PokemonRepository>();
            services.AddTransient<ITypeElementRepository, TypeElementRepository>();
            services.AddTransient<IPokemonTypeElementRepository, PokemonTypeElementRepository>();
            services.AddTransient<IPokemonWeaknessRepository, PokemonWeaknessRepository>();
            services.AddTransient<IPokemonNextEvolutionRepository, PokemonNextEvolutionRepository>();
            services.AddTransient<IPokemonPrevEvolutionRepository, PokemonPrevEvolutionRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(SpecificOriginsPolicy);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            serviceProvider.GetService<IDataService>().InitializeDB();
        }
    }
}
