using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebApp;

internal class Program
{
    public static Config Config { get; private set; }

    public static void Main(string[] args)
    {
        Config = Config.GetFromEnvironment();
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        
        // Add services to the container.
        builder.Services.AddRazorPages();
        builder.Services.AddControllers();

        WebApplication app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();
        app.MapRazorPages();
        app.MapDefaultControllerRoute();
        
        app.Run();
    }
}

internal struct Config
{
    [JsonPropertyName("base_path")]
    public string BlogPath { get; set; }

    public static Config GetFromEnvironment()
    {
        string path = Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new DirectoryNotFoundException(), 
            "config.json");

        if (!File.Exists(path))
        {
            throw new FileNotFoundException("No configuration file!");
        }

        return JsonSerializer.Deserialize<Config>(path, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        });
    }
}
