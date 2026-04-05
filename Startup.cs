using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ERP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Hangfire;
using Hangfire.SqlServer;
using ERP_API.Service.Parceiros.Interface;
using ERP_API.Controllers;
using ERP_API.Service.Parceiros;
using ERP_API.Domain.Entidades;
using Azure.Storage.Blobs;
using ERP_API.Service;
using System.Net.Http;
using System.Net.Http.Json;
using ERP_API.Service.BI2Service;

namespace WebApplication1
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

            var connection = Configuration["ConnectionStrings:DefaultConnection"];
             services.AddDbContext<Context>(options =>
                options.UseSqlServer(connection, sqlOptions =>
                {
                    sqlOptions.CommandTimeout(180); // 3 minutos para operações pesadas
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(5),
                        errorNumbersToAdd: null);
                }));


            services.AddHangfire(configuration => configuration
               .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
               .UseSimpleAssemblyNameTypeSerializer()
               .UseRecommendedSerializerSettings()
               .UseSqlServerStorage(connection, new SqlServerStorageOptions
               {
                   CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                   SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                   QueuePollInterval = TimeSpan.Zero,
                   UseRecommendedIsolationLevel = true,
                   DisableGlobalLocks = true,
               }));

            services.AddHangfireServer();

            // Add the processing server as IHostedService
            // services.AddHangfireServer();
            services.AddHttpClient();
            services.AddScoped<IRedeService, RedeService>();
            services.AddHttpClient<IUniqueService, UniqueService>();
            services.AddSingleton(x => new BlobServiceClient(Configuration["ConnectionStrings:StorageAccount"]));
            services.AddScoped<ERP_API.Service.IBlobStorageService, ERP_API.Service.BlobStorageService>();
            services.AddScoped<IConciliadoraService, ConciliadoraService>();
            services.AddScoped<IConciliadoraDashBoardService, ConciliadoraDashBoardService>();
            services.AddScoped<IRelatorioBIService, RelatorioBIService>();


            services.AddControllers();

            //Token
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            });

            var key = Encoding.ASCII.GetBytes(ERP.Models.SecurityToken.Settings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

           //  #if DEBUG
            services.AddSwaggerGen(c => {

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "JWTToken_Auth_API",
                    Version = "v1"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",                    
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference {
                            Type =  ReferenceType.SecurityScheme,
                                Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
                });
            });
            // #endif

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
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseCors(x => x
                  .AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader());
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMyErrorHandler();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHangfireDashboard("/hangfire");

            //// JOB 1: Enviar extratos mensais para clientes ativos
            //RecurringJob.AddOrUpdate(
            //    "enviar-extratos-mensais",
            //    () => ChamarEndpoint("/api/Cliente/enviar-extratos-mes"),
            //    Cron.Monthly(1, 7), // Todo dia 1º de cada mês às 7h da manhã
            //    TimeZoneInfo.Local
            //);

            //// JOB 2: Contas a receber (avisar 3 dias antes)
            //RecurringJob.AddOrUpdate(
            //      "contas-receber-tres-dias",
            //      () => ChamarEndpoint("/api/Financeiro/listaContaReceberTresDias"),
            //      Cron.Daily(8), // Todo dia �s 8h da manh�
            //      TimeZoneInfo.Local
            //  );

            //// JOB 3: Contas que vencem HOJE (avisar no dia)
            //RecurringJob.AddOrUpdate(
            //    "contas-receber-atual",
            //    () => ChamarEndpoint("/api/Financeiro/listaContaReceberAtual"),
            //    Cron.Daily(9), // Todo dia �s 9h da manh�
            //    TimeZoneInfo.Local
            //);

    



            //// JOB 4: Contas VENCIDAS (cobrar atrasados)
            //RecurringJob.AddOrUpdate(
            //    "contas-receber-atraso",
            //    () => ChamarEndpoint("/api/Financeiro/listaContaReceberAtraso"),
            //    Cron.Daily(10), // Todo dia �s 10h da manh�
            //    TimeZoneInfo.Local
            //);
        }

        // RecurringJob.AddOrUpdate<UsuarioController>("AlteracaoSenha", m => m.AlterarSenha(), Cron.Minutely(), TimeZoneInfo.Local);

        ///  app.UseHangfireDashboard();

        public static async Task ChamarEndpoint(string endpoint)
        {
            using var client = new HttpClient();

            // AJUSTE PARA SUA URL (desenvolvimento ou produ��o)
            var baseUrl = "https://api-sovarejo-desenv-bahjcfhzfbghe9h8.brazilsouth-01.azurewebsites.net/"; // ou sua URL de produ��o
            client.BaseAddress = new Uri(baseUrl);

            HttpResponseMessage response;


            response = await client.GetAsync(endpoint);
            

            response.EnsureSuccessStatusCode();
        }

    }
}
