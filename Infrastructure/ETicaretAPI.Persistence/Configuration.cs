using Microsoft.Extensions.Configuration;

namespace ETicaretAPI.Persistence;

static class Configuration
{
    public static string ConnectionString
    {
        get
        {
            ConfigurationManager configurationManager = new ConfigurationManager();

            configurationManager.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(),
                "../../Presentation/ETicaretAPI.API"));
            
            configurationManager.AddJsonFile("appsettings.json");

            var connectionString = configurationManager.GetConnectionString("PostgreSQL");

            return connectionString;
        }
    }
}