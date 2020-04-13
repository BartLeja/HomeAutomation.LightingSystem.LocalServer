using System;

namespace HomeAutomation.LightingSystem.LocalServer.Dto
{
    internal class LightBulbDto
    {
        public Guid Id { get; set; }
        public bool Status { get; set; } = false;
    }
}
