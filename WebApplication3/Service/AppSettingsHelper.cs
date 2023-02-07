namespace WebApplication3.Service
{
    public static class AppSettingsHelper
    {
        private static IConfiguration? _config;
        public static void AppSettingsConfigure(IConfiguration config)
        {
            _config = config;
        }

        public static string Setting(string key)
        {
            return _config!.GetSection(key).Value;
        }
    }
}