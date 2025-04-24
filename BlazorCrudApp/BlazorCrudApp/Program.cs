using BlazorCrudApp.Client.Pages;
using BlazorCrudApp.Components;
using BlazorCrudApp.Data;
using BlazorCrudApp.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorCrudApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ✅ Read connection string from appsettings.json
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // ✅ Register EF Core SQL Server DB
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
                options.EnableSensitiveDataLogging(); // shows actual query values
                options.LogTo(Console.WriteLine);     // logs to terminal/output
            });

            // ✅ Register API Controllers
            builder.Services.AddControllers();

            // ✅ Register Razor components (Blazor WebAssembly rendering)
            builder.Services.AddRazorComponents()
                .AddInteractiveWebAssemblyComponents()
                .AddInteractiveServerComponents();

            // ✅ Register HttpClient for WebAssembly (named client)
            builder.Services.AddHttpClient("BlazorCrudApp.Client", client =>
            {
                client.BaseAddress = new Uri("http://localhost:5017/"); // adjust to match your dev port
            });

            builder.Services.AddScoped(sp =>
                sp.GetRequiredService<IHttpClientFactory>().CreateClient("BlazorCrudApp.Client"));

            var app = builder.Build();

            // 🔧 Middleware
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAntiforgery();

            // ✅ Map Blazor components
            app.MapRazorComponents<App>()
                .AddInteractiveWebAssemblyRenderMode()
                .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);

            // ✅ Map API endpoints
            app.MapControllers();

            app.Run();
        }
    }
}
