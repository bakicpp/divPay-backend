using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Business.DependencyResolvers.Autofac;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>(builder =>
                {
                    builder.RegisterModule(new AutofacBusinessModule());
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}







//using Autofac;
//using Autofac.Core;
//using Autofac.Extensions.DependencyInjection;
//using Business.Abstract;
//using Business.Concrete;
//using Business.DependencyResolvers.Autofac;
//using Core.Utilities.Security.Encryption;
//using Core.Utilities.Security.JWT;
//using DataAccess.Abstract;
//using DataAccess.Concrete.EntityFramework;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.IdentityModel.Tokens;
//using Microsoft.Extensions.Configuration;

//     var builder = WebApplication.CreateBuilder(args);
//        // Add services to the container.

//        builder.Services.AddControllers();

//        public IConfiguration Configuration     { get; }



//var tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();

//        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//            .AddJwtBearer(options =>
//            {
//                options.TokenValidationParameters = new TokenValidationParameters
//                {
//                    ValidateIssuer = true,
//                    ValidateAudience = true,
//                    ValidateLifetime = true,
//                    ValidIssuer = tokenOptions.Issuer,
//                    ValidAudience = tokenOptions.Audience,
//                    ValidateIssuerSigningKey = true,
//                    IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey)
//                };
//            });

//        //builder.Services.AddSingleton<IProductService, ProductManager>();
//        //builder.Services.AddSingleton<IProductDal, EfProductDal>();
//        builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer<ContainerBuilder>(builder =>
//        {
//            builder.RegisterModule(new AutofacBusinessModule());
//        });



//        var app = builder.Build();

//        // Configure the HTTP request pipeline.

//        app.UseHttpsRedirection();

//        app.UseAuthorization();

//        app.UseAuthentication();

//        app.MapControllers();

//        app.Run();