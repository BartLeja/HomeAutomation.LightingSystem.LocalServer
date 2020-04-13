using System;
using System.Threading.Tasks;
using HomeAutomation.LightingSystem.LocalServer.Utilities.Models;
using Newtonsoft.Json;

namespace HomeAutomation.LightingSystem.LocalServer.Utilities
{
    internal class AppSettingsLoader
    {
        public async Task<AppSettingsModel> GetAppSetings()
        {
            var packageFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            var appsettingsFile = await packageFolder.GetFileAsync("appsettings.json");
            var appSettingsJson = await Windows.Storage.FileIO.ReadTextAsync(appsettingsFile);
            return JsonConvert.DeserializeObject<AppSettingsModel>(appSettingsJson);
        }
    }
}
