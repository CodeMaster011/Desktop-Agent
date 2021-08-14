using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopAgentWpf
{
    public interface IAgent
    {
        string AgentName { get; }
        void Initialize(string agentName);
    }
}
