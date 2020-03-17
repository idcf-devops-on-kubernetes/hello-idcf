using System;
using System.IO;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HelloIdcf
{
    public class Startup
    {
        
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel(op => { op.Limits.KeepAliveTimeout = TimeSpan.FromSeconds(1); });
                    webBuilder.UseStartup<Startup>();
                });
        
        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await using var stream = typeof(Startup).Assembly.GetManifestResourceStream("HelloIdcf.index.html");
                    using var streamReader = new StreamReader(stream);
                    var index = await streamReader.ReadToEndAsync();

                    context.Response.ContentType = "text/html";
                    await context.Response.WriteAsync(index);
                    
                });
                
                endpoints.MapGet("/server-info", async context =>
                {
                    var serverInfo = new { hostname = Environment.MachineName, time = DateTime.Now };
                    
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonSerializer.Serialize(serverInfo));
                });
            });
        }
    }
}
