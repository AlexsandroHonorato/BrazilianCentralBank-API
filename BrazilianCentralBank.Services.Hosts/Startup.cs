using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using BrazilianCentralBank.Infrastructure.Data.Interfaces;
using BrazilianCentralBank.Infrastructure.Data.Base;
using BrazilianCentralBank.Infrastructure.Data;
using BrazilianCentralBank.Domain.Entities;
using BrazilianCentralBank.Application.ViewModels;
using BrazilianCentralBank.Application.Interfaces;
using BrazilianCentralBank.Application;
using BrazilianCentralBank.Services.Hosts.Command;

namespace BrazilianCentralBank.Services.Hosts {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            ConfigureTokenJWT(services);
            ConfigureDI(services);
            ConfigureAutoMapper(services);
            services.AddControllers();
            services.AddCors(options => options.AddPolicy("Cors", builder => {
                builder.AllowAnyOrigin();
                builder.AllowAnyMethod();
                builder.AllowAnyHeader();
            }));
        }

        //Configuração do JWT
        private void ConfigureTokenJWT(IServiceCollection services) {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = Configuration["AppSettings:Issuer"],
                    ValidAudience = Configuration["AppSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["AppSettings:Token"]))
                };
            });
        }

        private void ConfigureDI(IServiceCollection services) {
            services.AddSingleton<IRepositoryBase>(ctx => new RepositoryBase(connectionString: Configuration.GetConnectionString("DbConnection")));  
            services.AddScoped<ICurrencyQuotationRepository, CurrencyQuotationRepository>();
            services.AddScoped<ICurrencyQuotationServices, CurrencyQuotationServices>();
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddScoped<ICurrencyServices, CurrencyServices>();

        }

        private void ConfigureAutoMapper(IServiceCollection services) {
            MapperConfiguration AutoMapperConfig = new AutoMapper.MapperConfiguration(cfg => {
                cfg.CreateMap<Currency, CurrencyViewModel>().ReverseMap();
                cfg.CreateMap<CurrencyCommand, CurrencyViewModel>().ReverseMap();
                cfg.CreateMap<CurrencyQuotation, CurrencyQuotationViewModel>().ReverseMap();
                cfg.CreateMap<CurrencyQuotationCommand, CurrencyQuotationViewModel>().ReverseMap();
            });

            IMapper mapper = AutoMapperConfig.CreateMapper();

            services.AddSingleton(mapper);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("Cors"); // CORS

            app.UseAuthorization();

            app.UseAuthentication();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
