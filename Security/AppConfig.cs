namespace StellarStreamAPI.Security
{
    public class AppConfig
    {
        public CorsConfig Cors { get; set; } = new CorsConfig();
    }

    public class CorsConfig
    {
        public List<string> AllowedOrigins { get; set; } = new List<string>();
    }
}
