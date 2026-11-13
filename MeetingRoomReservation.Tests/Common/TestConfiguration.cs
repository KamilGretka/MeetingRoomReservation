using System.Text.Json;

namespace MeetingRoomReservation.Tests.Common
{
    public static class TestConfiguration
    {
        public static string GetConnectionString(string name)
        {
            var appSettingsPath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
            using var appSettings = JsonDocument.Parse(File.ReadAllText(appSettingsPath));
            var connectionString = appSettings.RootElement
                .GetProperty("ConnectionStrings")
                .GetProperty(name)
                .GetString();

            return connectionString
                ?? throw new InvalidOperationException($"Connection string '{name}' is missing from appsettings.json.");
        }
    }
}
