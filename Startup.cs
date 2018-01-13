using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using AutoMapper;
using backendhotel.Auth;
using drevolution.data;
using drevolution.Helpers;
using drevolution.Models;
using drevolution.Models.Entities;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using static drevolution.Helpers.Constants;

namespace backendhotel
{
  public class Startup
  {


 
    public IConfigurationRoot Configuration { get; }

    public Startup(IHostingEnvironment env)
    {
      var builder = new ConfigurationBuilder()
          .SetBasePath(env.ContentRootPath)
          .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
          .AddEnvironmentVariables();
      Configuration = builder.Build();
    }


    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
      
      // Add framework services.
      services.AddDbContext<ApplicationDbContext>(options =>
      options.UseSqlServer(
       Configuration.GetConnectionString("Connection"),
       b => b.MigrationsAssembly("backendhotel"))
      );
      

      Privilegios privilegios = new Privilegios();

      privilegios
        .Add(new Privilegio(POLITICAS.USER))
        .Add(new Privilegio(POLITICAS.ATIVO))
        .Add(new Privilegio(POLITICAS.FUNCIONARIO))
        .Add(new Privilegio(POLITICAS.ADMIN))
     
        .Build(services);

      

      services.AddIdentity<AppUser, IdentityRole>
          (o =>
          {
            // configure identity options
            o.Password.RequireDigit = false;
            o.Password.RequireLowercase = false;
            o.Password.RequireUppercase = false;
            o.Password.RequireNonAlphanumeric = false;
            o.Password.RequiredLength = 4;

          })
          .AddEntityFrameworkStores<ApplicationDbContext>()
          .AddDefaultTokenProviders();

      services.AddMvc().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());
      services.AddAutoMapper();

     

      var signingConfigurations = new SigningConfigurations();
      services.AddSingleton(signingConfigurations);
      

      var tokenConfigurations = new TokenConfigurations();
      new ConfigureFromConfigurationOptions<TokenConfigurations>(
          Configuration.GetSection("JwtIssuerOptions"))
              .Configure(tokenConfigurations);
      services.AddSingleton(tokenConfigurations);


      services.AddAuthentication(authOptions =>
      {
        authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
       authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
       
      }).AddJwtBearer(bearerOptions =>
      {
        var paramsValidation = bearerOptions.TokenValidationParameters;
        
        paramsValidation.IssuerSigningKey = signingConfigurations.Key;
        paramsValidation.ValidAudience = tokenConfigurations.Audience;
        paramsValidation.ValidIssuer = tokenConfigurations.Issuer;

        // Valida a assinatura de um token recebido
        paramsValidation.ValidateIssuerSigningKey = true;

        // Verifica se um token recebido ainda é válido
        paramsValidation.ValidateLifetime = true;

        // Tempo de tolerância para a expiração de um token (utilizado
        // caso haja problemas de sincronismo de horário entre diferentes
        // computadores envolvidos no processo de comunicação)
        paramsValidation.ClockSkew = TimeSpan.Zero;
        
      });
      
      

      // Retirar CORS em PRODUÇÃO!!!!!!!!!!!!!!!!!!
      services.AddCors(options =>
      {
        options.AddPolicy("AllowAllHeaders",
              builder =>
              {
                builder.AllowAnyOrigin()
                               .AllowAnyHeader()
                               .AllowAnyMethod()
                               .AllowCredentials();
              });
      });

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
      // Retirar CORS em PRODUÇÃO!!!!!!!!!!!!!!!!!!
      app.UseCors("AllowAllHeaders");


      loggerFactory.AddConsole();

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      
      app.UseDefaultFiles();
      app.UseStaticFiles();
      app.UseMvc();
    }
  }
  


}
