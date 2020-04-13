namespace HomeAutomation.LightingSystem.LocalServer.Models
{
    internal class LightPoint
    {
        public string Id { get; set; }
        public string CustomName { get; set; }
  
        public string[] BulbsId { get; set; }
    }
}
