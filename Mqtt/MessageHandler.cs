using HomeAutomation.LightingSystem.LocalServer.Clients;
using HomeAutomation.LightingSystem.LocalServer.Dto;
using HomeAutomation.LightingSystem.LocalServer.Enums;
using HomeAutomation.LightingSystem.LocalServer.Models;
using HomeAutomation.LightingSystem.LocalServer.Utilities.Models;
using MQTTnet;
using MQTTnet.Client.Receiving;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HomeAutomation.LightingSystem.LocalServer.Mqtt
{
    internal class MessageHandler : IMqttApplicationMessageReceivedHandler
    {
        private readonly SignalRClient _signalRClient;
        private readonly RestClient _restClient;
        private readonly ISettings _settings;

        public MessageHandler(
            ISettings settings,
            RestClient restClient,
            SignalRClient signalRClient)
        {
            _settings = settings;
            _restClient = restClient;
            _signalRClient = signalRClient;
        }

        public async Task HandleApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs eventArgs)
        {
            string payload = Encoding.UTF8.GetString(eventArgs.ApplicationMessage.Payload);
            string topic = eventArgs.ApplicationMessage.Topic;

            if (topic.Equals(MqttMessagesType.ConnectedToServer.ToString()))
            {
                await UserConnected(payload);
            }
            else if (topic.Equals(MqttMessagesType.SwitchLightChange.ToString()))
            {
                await SwitchLight(payload);
            }
            else if (topic.Equals(MqttMessagesType.LightPointReset.ToString()))
            {
                await LightPointReset(payload);
            }
        }

        private async Task UserConnected(string payload)
        {
            //TODO try catch serializer can crash
            LightPoint lightPointSwitch = JsonConvert.DeserializeObject<LightPoint>(payload);
            try
            {
                var lightPoint = await _restClient.GetLightPoints(Guid.Parse(lightPointSwitch.Id));
                if (lightPoint.CustomName == null)
                {
                    var lightBulb = new List<LightBulbDto>();
                    foreach (var id in lightPointSwitch.BulbsId)
                    {
                        lightBulb.Add(new LightBulbDto() { Id = Guid.Parse(id) });
                    }

                    var lightPointDto = new LightPointDto()
                    {
                        CustomName = lightPointSwitch.CustomName,
                        Id = Guid.Parse(lightPointSwitch.Id),
                        LightBulbs = lightBulb
                    };

                    //TODO auto generation of guid and saving to memeory of rPi
                    await _restClient.AddLightPoint(Guid.Parse(_settings.HomeAutomationLightingSystemId), lightPointDto);
                }
                else
                {
                    await _restClient.EnableLightPoint(Guid.Parse(lightPointSwitch.Id));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private async Task SwitchLight(string payload)
        {
            LightPointStatus lightPointStatus = JsonConvert.DeserializeObject<LightPointStatus>(payload);
            await _signalRClient.InvokeSendStatusMethod(Guid.Parse(lightPointStatus.Id), lightPointStatus.Status);
        }

        private async Task LightPointReset(string payload)
        {
            LightPointRest lightPointReset = JsonConvert.DeserializeObject<LightPointRest>(payload);
            await _restClient.DeleteLightPoint(Guid.Parse(lightPointReset.Id));
        }
    }
}
