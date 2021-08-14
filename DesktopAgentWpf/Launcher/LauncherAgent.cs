using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DesktopAgentWpf.Launcher
{
    [Agent("{7632E294-1AE8-4E51-9477-2F8071368EF3}")]
    public class LauncherAgent : BaseAgent, IEventHandler
    {
        public const string BeforeProcessStart = "BeforeProcessStart";
        public const string AfterProcessStart = "AfterProcessStart";
        public const string AfterProcessExit = "AfterProcessExit";

        public void Handle(string eventName, object eventData)
        {
            if (eventName == GlobalEvents.ApplicationStart)
            {
                var option = this.GetOption<LauncherAgentOption>(AgentName);
                if (option.AutoStart)
                    StartProcess(option);
            }
        }

        protected virtual void StartProcess(LauncherAgentOption option)
        {
            Global.EventPool.RaiseEvent(BeforeProcessStart, option);
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = option.Location,
                    WorkingDirectory = option.WorkingDir,
                }
            };
            process.Start();

            ThreadPool.QueueUserWorkItem(async _ =>
            {
                await process.WaitForExitAsync();
                Process_Exited();
            });

            Global.EventPool.RaiseEvent(AfterProcessStart, option);
        }

        private void Process_Exited()
        {
            var options = this.GetOption<LauncherAgentOption>(AgentName);
            Global.EventPool.RaiseEvent(AfterProcessExit, options);

            if (options.AutoExitAfterClose)
                Environment.Exit(0);
        }

        public override void Initialize(string agentName)
        {
            base.Initialize(agentName);

            Global.EventPool.RegisterForEvent(GlobalEvents.ApplicationStart, this);
        }
    }

    public class LauncherAgentOption
    {
        public string AppName { get; set; }
        public string Location { get; set; }
        public string WorkingDir { get; set; }
        public bool AutoStart { get; set; }
        public bool AutoExitAfterClose { get; set; }
    }
}
