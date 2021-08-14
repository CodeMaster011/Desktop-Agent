using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopAgentWpf
{
    public abstract class BaseAgent : IAgent
    {
        public string AgentName { get; private set; }
        public virtual void Initialize(string agentName) { AgentName = agentName; }
    }
}
