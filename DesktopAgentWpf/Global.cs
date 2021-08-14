using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DesktopAgentWpf
{
    public class Global
    {
        public static Configuration Configuration { get; set; }
        public static IAgent[] ActiveAgents { get; set; }
        public static EventPool EventPool { get; } = new EventPool();
        public static Dictionary<string, object> State { get; } = new Dictionary<string, object>();


        public static void LoadConfiguration()
        {
            var cnfPath = Path.Combine(Environment.CurrentDirectory, "agent.config.json");
            if (!File.Exists(cnfPath))
                throw new FileNotFoundException("agent.config.json missing");

            var json = File.ReadAllText(cnfPath);
            Configuration = Newtonsoft.Json.JsonConvert.DeserializeObject<Configuration>(json);
        }

        public static void DiscoveryAgents()
        {
            var agentTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.GetCustomAttribute<AgentAttribute>() != null)
                .ToArray();
            
            var activeAgent = new List<IAgent>(agentTypes.Length);

            foreach (var agentConfig in Configuration.Agents)
            {
                var agentType = agentTypes.FirstOrDefault(a => a.GetCustomAttribute<AgentAttribute>()?.AgentId == agentConfig.AgentId);
                var agent = Activator.CreateInstance(agentType) as IAgent;
                if (agent == null)
                    throw new InvalidOperationException($"{agentType.FullName} is not an {nameof(IAgent)}");

                if (activeAgent.Find(a => a.AgentName == agentConfig.Name) != null)
                    throw new InvalidOperationException($"Duplicate agent name \"{agentConfig.Name}\" detected.");

                agent.Initialize(agentConfig.Name);

                activeAgent.Add(agent);
            }

            ActiveAgents = activeAgent.ToArray();
        }
    }
}
