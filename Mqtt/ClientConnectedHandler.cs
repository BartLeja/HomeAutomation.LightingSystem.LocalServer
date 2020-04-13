using MQTTnet.Server;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HomeAutomation.LightingSystem.LocalServer.Mqtt
{
    internal class ClientConnectedHandler : IMqttServerClientConnectedHandler
    {
        public async Task HandleClientConnectedAsync(MqttServerClientConnectedEventArgs eventArgs)
        {
            Debug.WriteLine($"Client connected {eventArgs.ClientId}");
        }
    }
}
