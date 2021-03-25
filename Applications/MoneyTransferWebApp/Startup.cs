using ApplicationServices;
using Common.EFCoreDataAccess;
using Domain.Repositories;
using Domain.Services.External.BankService;
using EFCoreDataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MockBankService;
using MoneyTransferWebApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyTransferWebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddDbContextPool<CoreEFCoreDbContext>(options =>
            {
                options.UseSqlServer(Configuration["ConnectionStrings:DevConnection"]);
            });

            services.AddScoped<ICoreUnitOfWork, CoreEFCoreUnitOfWork>();
            services.AddScoped<EFCoreUnitOfWork, CoreEFCoreUnitOfWork>();
            services.AddScoped<IBankService, BankService>();

            services.AddScoped((IServiceProvider serviceProvider) =>
            {
                var coreUnitOfWork = serviceProvider.GetRequiredService<ICoreUnitOfWork>();
                var bankService = serviceProvider.GetRequiredService<IBankService>();

                var accountService = new AccountService(coreUnitOfWork, bankService);
                return accountService;
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
