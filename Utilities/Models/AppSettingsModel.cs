namespace HomeAutomation.LightingSystem.LocalServer.Utilities.Models
{
    internal class AppSettingsModel
    {
        public DevEnvironment DevEnvironment { get; set; }
        public ProdEnvironment ProdEnvironment { get; set; }
    }

    internal class DevEnvironment : ISettings
    {
        public string IdentityServerbaseUrl { get; set; }
        public string SignalRHubUrl { get; set; }
        public string HomeAutomationLightingSystemApi { get; set; }
        public string HomeAutomationLightingSystemId { get; set; }
        public Authorizationcredentials AuthorizationCredentials { get; set; }
    }

    internal class Authorizationcredentials
    {
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
    }

    internal class ProdEnvironment : ISettings
    {
        public string IdentityServerbaseUrl { get; set; }
        public string SignalRHubUrl { get; set; }
        public string HomeAutomationLightingSystemApi { get; set; }
        public string HomeAutomationLightingSystemId { get; set; }
        public Authorizationcredentials AuthorizationCredentials { get; set; }
    }

    internal interface ISettings
    {
         string IdentityServerbaseUrl { get; set; }
         string SignalRHubUrl { get; set; }
         string HomeAutomationLightingSystemApi { get; set; }
         string HomeAutomationLightingSystemId { get; set; }
         Authorizationcredentials AuthorizationCredentials { get; set; }
    }

}
