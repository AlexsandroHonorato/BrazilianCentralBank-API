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


using System.IO;
using AutoMapper;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

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
            ConfigureSwagger(services);
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

        private void ConfigureSwagger(IServiceCollection services) {

            services.AddVersionedApiExplorer(options => {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            })
          .AddApiVersioning(options => {
              options.DefaultApiVersion = new ApiVersion(1, 0);
              options.AssumeDefaultVersionWhenUnspecified = true;
              options.ReportApiVersions = true;
          });

            var apiProviderDescription = services.BuildServiceProvider().GetService<IApiVersionDescriptionProvider>();

            services.AddSwaggerGen(options => {

                foreach (var descriptionVersion in apiProviderDescription.ApiVersionDescriptions) {

                    options.SwaggerDoc(descriptionVersion.GroupName, new Microsoft.OpenApi.Models.OpenApiInfo() {
                        Title = "Banco Central do Brasil API",
                        Version = descriptionVersion.ApiVersion.ToString(),
                        TermsOfService = new Uri("http://ArquiVaiSeusTermosDeUso.com.br"),
                        Description = "API Desenvolvida Para Pegar as Taxas de Cotações",
                        License = new Microsoft.OpenApi.Models.OpenApiLicense {
                            Name = "BancoCentralDoBrasil License",
                            Url = new Uri("http://minhaslicensas.com")
                        },
                        Contact = new Microsoft.OpenApi.Models.OpenApiContact {
                            Name = "Desenvolvido por Alexsandro Honorato",
                            Email = "alexsandrohonorato@gmail.com",
                            Url = new Uri("https://www.linkedin.com/in/alexsandro-honorato-da-silva-62480825/")
                        }
                    });
                }

                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

                options.IncludeXmlComments(xmlCommentsFullPath);

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme() {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = $"JWT Authorization header using the Bearer scheme."+
                   "\r\n\r\n Enter 'Bearer'[space] and then your token in the text input below."+
                    "\r\n\r\nExample: \"Bearer 12345abcdef\"",
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                          {
                              Reference = new OpenApiReference
                              {
                                  Type = ReferenceType.SecurityScheme,
                                  Id = "Bearer"
                              }
                          },
                         new string[] {}
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider apiVersionDescriptionProvider) {

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("Cors"); // CORS

            app.UseAuthorization();

            app.UseAuthentication();

            app.UseSwagger()
                .UseSwaggerUI(options => {
                    foreach (var descriptionVersion in apiVersionDescriptionProvider.ApiVersionDescriptions) {
                        options.SwaggerEndpoint($"/Swagger/{descriptionVersion.GroupName}/swagger.json", descriptionVersion.GroupName.ToLowerInvariant());
                    }

                    options.RoutePrefix = "";
                });

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
