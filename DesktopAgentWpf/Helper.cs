using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DesktopAgentWpf
{
    [System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    sealed class AgentAttribute : Attribute
    {
        public AgentAttribute(string agentId)
        {
            if (string.IsNullOrWhiteSpace(agentId))
            {
                throw new ArgumentException($"'{nameof(agentId)}' cannot be null or whitespace.", nameof(agentId));
            }

            AgentId = agentId;
        }

        public string AgentId { get; }
    }

    public static class Helper
    {
        public static TOption GetOption<TOption>(this IAgent agent, string agentName) where TOption: class
        {
            var agentDec = Global.Configuration.Agents.FirstOrDefault(a => a.Name == agentName);
            if (agentDec == null) return default;
            return JsonConvert.DeserializeObject<TOption>(JsonConvert.SerializeObject(agentDec.Options));
        }
    }
}
