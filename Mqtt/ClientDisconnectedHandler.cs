using HomeAutomation.LightingSystem.LocalServer.Clients;
using MQTTnet.Server;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HomeAutomation.LightingSystem.LocalServer.Mqtt
{
    internal class ClientDisconnectedHandler : IMqttServerClientDisconnectedHandler
    {
        private readonly RestClient _restClient;
        private readonly IMqttServer _mqttServer;

        public ClientDisconnectedHandler(RestClient restClient, IMqttServer mqttServer)
        {
            _restClient = restClient;
            _mqttServer = mqttServer;
        }

        public async Task HandleClientDisconnectedAsync(MqttServerClientDisconnectedEventArgs eventArgs)
        {
            Debug.WriteLine($"Client disconnected {eventArgs.ClientId}");
            await _mqttServer.ClearRetainedApplicationMessagesAsync();
            try
            {
                await _restClient.DisableLightPoint(Guid.Parse(eventArgs.ClientId));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
