using System.Threading.Tasks;
using HomeAutomation.LightingSystem.LocalServer.Clients;
using HomeAutomation.LightingSystem.LocalServer.Models;
using HomeAutomation.LightingSystem.LocalServer.Mqtt;
using HomeAutomation.LightingSystem.LocalServer.Utilities.Models;
using Newtonsoft.Json;

namespace HomeAutomation.LightingSystem.LocalServer.Services
{
    internal class LightPointsMenager
    {
        private readonly MqttServer _mqttServer;
        private readonly SignalRClient _signalRClient;
        private readonly RestClient _restClient;
        private readonly ISettings _settings;

        public LightPointsMenager(ISettings settings)
        {
            _settings = settings;
            // _logger = new Logger.Logger("HomeAutomationLocalServer");
            _mqttServer = new MqttServer();
            _restClient = new RestClient( _settings.HomeAutomationLightingSystemApi);
            _signalRClient = new SignalRClient();
        }

        public async Task ConnectToSignalRServer()
        {
            var token = await _restClient.GetToken(
                _settings.IdentityServerbaseUrl,
                _settings.AuthorizationCredentials);
            await _signalRClient.ConnectToSignalR(token, _settings.SignalRHubUrl);

            _signalRClient.ReceiveLightPoint += async (s, e) =>
            {
                var lightPointStatus = new LightPointStatus()
                {
                    Id = e.LightBulbId.ToString(),
                    Status = e.Status
                };

                string lightPointStatusPayload = JsonConvert.SerializeObject(lightPointStatus);

                await _mqttServer.PublishMessage("lightChange", lightPointStatusPayload);
            };

            _signalRClient.RestOfLightPoint += async (s, e) =>
            {
                await _mqttServer.PublishMessage("reset", e.LightPointId.ToString());
            };

            _signalRClient.HardRestOfLightPoint += async (s, e) =>
            {
                await _mqttServer.PublishMessage("hardReset", e.LightPointId.ToString());
            };
        }

        public async Task StartMqttServer()
        {
            var mqttServer = await _mqttServer.ServerRun();
          
            mqttServer.ClientConnectedHandler = new ClientConnectedHandler();
            mqttServer.ClientDisconnectedHandler = new ClientDisconnectedHandler(_restClient, mqttServer);
            mqttServer.ApplicationMessageReceivedHandler = new MessageHandler(
                _settings, 
                _restClient, 
                _signalRClient);
        }
    }
}
