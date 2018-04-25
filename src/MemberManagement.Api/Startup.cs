﻿using AutoMapper;
using MediatR;
using MemberManagement.Api.Infrastructure;
using MemberManagement.AppCore.Interfaces;
using MemberManagement.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MemberManagement.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            services.AddDbContext<MemberManagementContext>(
                options => options.UseSqlServer(
                    Configuration.GetConnectionString("MemberManagementConnection")));

            ConfigureServices(services);
        }

        public void ConfigureTestingServices(IServiceCollection services)
        {
            services.AddDbContext<MemberManagementContext>(c =>
                c.UseInMemoryDatabase("MemberManagement"));

            ConfigureServices(services);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper();
            services.AddMvc();
            services.AddCors();

            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped(typeof(IMemberRepository), typeof(MemberRepository));
            services.AddMediatR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder => builder.WithOrigins("http://localhost:4200")
                .WithHeaders("accept", "content-type", "origin", "x-custom-header")
                .AllowAnyMethod());
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseMvc();
        }
    }
}