using AutoMapper;
using BusinessLayer.Contracts;
using CarRentalNovility.BusinessLayer;
using CarRentalNovility.DataLayer;
using CarRentalNovility.Web.Dto;
using CarRentalNovility.Web.Infrastructure;
using DataLayer.Repositories;
using DataLayer.Repositories.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Reflection;

namespace CarRentalNovility
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
            services.AddMvc()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                    .AddJsonOptions(options =>
                    {
                        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    });

            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new Info { Title = "CarRental", Version = "v1" });
                opt.DescribeAllEnumsAsStrings();

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                opt.IncludeXmlComments(xmlPath);
            });

            services.AddLogging();

            services.AddAutoMapper(opt =>
            {
                opt.AddProfile(typeof(MappingProfile));
            });

            AddDbContext(services);

            AddBusinessServices(services);
        }

        private void AddDbContext(IServiceCollection services)
        {
            if (Configuration.IsUseInMemoryDb())
            {
                services.AddDbContext<CarRentalDbContext>(opt =>
                {
                    opt.UseInMemoryDatabase("Test");
                });
            }
            else
            {
                services.AddDbContext<CarRentalDbContext>(opt =>
                {
                    opt.UseSqlServer(Configuration.GetDbConnectionString());
                });
            }
            //services.AddTransient<Seeder>();
        }

        private static void AddBusinessServices(IServiceCollection services)
        {
            services.AddScoped<IRepositoryCar, RepositoryCar>();
            services.AddScoped<IRepositoryCarType, RepositoryCarType>();
            services.AddScoped<IRepositoryClient, RepositoryClient>();
            services.AddScoped<IRepositoryReservation, RepositoryReservation>();
            services.AddScoped<IBLClient, BLClient>();
            services.AddScoped<IBLReservation, BLReservation>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IConfiguration configuration, ILoggerFactory loggerFactory, CarRentalDbContext dbContext) //,Seeder seeder)
        {
            loggerFactory.AddFile(configuration.GetLoggingFileTarget());

            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(opt =>
            {
                opt.SwaggerEndpoint("/swagger/v1/swagger.json", "CarRental V1");
                opt.RoutePrefix = string.Empty;
            });

            app.UseMiddleware<ExceptionMiddleware>();
            app.UseMvc();

            if (env.IsDevelopment() && Configuration.IsUseInMemoryDb())
            {
                dbContext.Database.EnsureCreated(); //EnsureCreated doesn't run migrations but create the db if not exists and run the HasData methods to seed the db.
                //seeder.Seed().Wait(); // Now I'm using EF Core 2.1 seeding. See OnModelCreating in CarRentalDbContext
            }
        }
    }
}
