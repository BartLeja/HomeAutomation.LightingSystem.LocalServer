using System;
using System.Collections.Generic;

namespace HomeAutomation.LightingSystem.LocalServer.Dto
{
    internal class LightPointDto
    {
        public Guid Id { get; set; }
        public string CustomName { get; set; }
        public bool IsAvailable { get; set; } = true;
        public List<LightBulbDto> LightBulbs { get; set; }
    }
}
