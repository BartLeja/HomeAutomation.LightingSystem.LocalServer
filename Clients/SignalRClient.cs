using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace HomeAutomation.LightingSystem.LocalServer.Clients
{
    internal class SignalRClient
    {
        private HubConnection _connection;

        public event EventHandler<ReceiveLightPointEventArgs> ReceiveLightPoint;
        public event EventHandler<HardRestOfLightPointEventArgs> HardRestOfLightPoint;
        public event EventHandler<RestOfLightPointEventArgs> RestOfLightPoint;
        
        public SignalRClient()
        {
        }

        public async Task ConnectToSignalR(string token, string signalRHubUrl)
        {
            _connection = new HubConnectionBuilder()
                .WithUrl(signalRHubUrl,
                options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(token);
                })
                .Build();
           
            _connection.Closed += async (error) =>
            {
                var connectionState = false;
                while (!connectionState)
                {
                    await Task.Delay(new Random().Next(0, 5) * 1000);
                    try
                    {
                        await _connection.StartAsync();
                        connectionState = true;
                    }
                    catch (Exception ex)
                    {
                        connectionState = false;
                        Console.WriteLine(ex);
                    }
                }
            };

            _connection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                Debug.WriteLine($"Message from {user} recived. {message}");
               // _logger.Log($"Message from {user} recived. {message}", typeof(SignalRClient).Namespace, "");
            });

            _connection.On<Guid, bool>("ReceiveLightPointStatus", (Guid lightBulbId, bool status) =>
             {
                Debug.WriteLine($"Message from {lightBulbId} recived.");
                 //// _logger.Log($"Message from {lightPointNumber} recived.", typeof(SignalRClient).Namespace, "");
                 var args = new ReceiveLightPointEventArgs
                 {
                     LightBulbId = lightBulbId,
                     Status = status
                 };
                 ReceiveLightPoint?.Invoke(this, args);
             });
            
            _connection.On<Guid>("HardRestOfLightPoint", (Guid lightBulbId) =>
            {
                HardRestOfLightPoint?.Invoke(this, 
                    new HardRestOfLightPointEventArgs {
                        LightPointId = lightBulbId
                    });
            });


            _connection.On<Guid>("RestOfLightPoint", (Guid lightBulbId) =>
            {
                RestOfLightPoint?.Invoke(
                    this, 
                    new RestOfLightPointEventArgs {
                        LightPointId = lightBulbId
                    });
            });



            try
            {
                await _connection.StartAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            
        }

        public async Task InvokeSendStatusMethod(Guid lightBulbId, bool status)
        {
            try
            {
                await _connection.InvokeAsync("SendLightPointStatus", lightBulbId, status);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        
        public async Task InvokeSendMessageMethod(string user, string message)
        {
            Debug.WriteLine("SendMessage");
            //_logger.Log("SendMessage", typeof(SignalRClient).Namespace, "");
            try
            {
                await _connection.InvokeAsync("SendMessage", user, message);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }

    internal class ReceiveLightPointEventArgs : EventArgs
    {
        public Guid LightBulbId { get; set; }
        public bool Status { get; set; }
    }

   internal class RestOfLightPointEventArgs : EventArgs
    {
        public Guid LightPointId { get; set; }
    }

    internal class HardRestOfLightPointEventArgs : EventArgs
    {
        public Guid LightPointId { get; set; }
    }

}
