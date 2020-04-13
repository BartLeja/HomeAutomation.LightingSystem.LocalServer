using System.Diagnostics;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Server;

namespace HomeAutomation.LightingSystem.LocalServer.Mqtt
{
    internal class MqttServer
    {
        private IMqttServer mqttServer;
        private MqttServerOptionsBuilder optionsBuilder;
     
        public MqttServer()
        {
        }
        public async Task<IMqttServer> ServerRun()
        {
            Debug.WriteLine("TestMQTTServer");
            optionsBuilder = new MqttServerOptionsBuilder()
                .WithConnectionBacklog(100)
                .WithDefaultEndpointPort(8081);

            mqttServer = new MqttFactory().CreateMqttServer();
            
            await mqttServer.StartAsync(optionsBuilder.Build());
            return mqttServer;
        }
        
        public async Task PublishMessage(string topic ,string payload)
        {
            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(payload)
                .WithExactlyOnceQoS()
                .WithRetainFlag()
                .Build();

            await mqttServer.PublishAsync(message);
        }
    }
}
