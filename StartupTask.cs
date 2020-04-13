using Windows.ApplicationModel.Background;
using HomeAutomation.LightingSystem.LocalServer.Clients;
using HomeAutomation.LightingSystem.LocalServer.Services;
using HomeAutomation.LightingSystem.LocalServer.Utilities;
using HomeAutomation.LightingSystem.LocalServer.Utilities.Models;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace HomeAutomation.LightingSystem.LocalServer
{
    public sealed class StartupTask : IBackgroundTask
    {
        private static BackgroundTaskDeferral _deferral = null;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            // 
            // TODO: Insert code to perform background work
            //
            // If you start any asynchronous methods here, prevent the task
            // from closing prematurely by using BackgroundTaskDeferral as
            // described in http://aka.ms/backgroundtaskdeferral
            //

            _deferral = taskInstance.GetDeferral();
            var appSettingsLoader = new AppSettingsLoader();
            var appSettings = await appSettingsLoader.GetAppSetings();
            ISettings settings;

            var isProdEnvironment = true;
            if (isProdEnvironment)
            {
                settings = appSettings.ProdEnvironment;
            }
            else
            {
                settings = appSettings.DevEnvironment;
            }
            
            var lightPointManager = new LightPointsMenager(settings);
            await lightPointManager.StartMqttServer();
            await lightPointManager.ConnectToSignalRServer();
        }
    }
}
