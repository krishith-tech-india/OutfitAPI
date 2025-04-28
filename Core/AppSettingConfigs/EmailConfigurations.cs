namespace Core.AppSettingConfigs
{
    public class EmailConfigurations
    {
        public string DisplayName { get; set; } = null!;
        public string Host { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int Port { get; set; }
        public bool EnableSSL { get; set; }
    }
}
