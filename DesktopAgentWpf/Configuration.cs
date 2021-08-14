using System.Collections.Generic;

namespace DesktopAgentWpf
{
    public class Configuration
    {
        public string Version { get; set; } = "1.0";
        public bool ShowUi { get; set; }
        public AgentDescription[] Agents { get; set; }
    }

    public class AgentDescription
    {
        public string AgentId { get; set; }
        public string Name { get; set; }
        public Dictionary<string, object> Options { get; set; }
    }
}
