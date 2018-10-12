using Microsoft.Extensions.Configuration;

namespace CarRentalNovility.Web.Infrastructure
{
    public static class ConfigurationExtensions
    {
        public static string GetLoggingFileTarget(this IConfiguration configuration) => configuration.GetSection("Logging").GetValue<string>("FileTarget");
        
        public static bool IsUseInMemoryDb(this IConfiguration configuration) => configuration.GetValue<bool>("USE_IN_MEMORY_DB");

        public static string GetDbConnectionString(this IConfiguration configuration) => configuration.GetConnectionString("MyHumbleTest");
    }
}
