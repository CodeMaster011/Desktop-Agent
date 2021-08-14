using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DesktopAgentWpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Global.LoadConfiguration();
            Global.DiscoveryAgents();


            Global.EventPool.RaiseEvent(GlobalEvents.ApplicationStart, null);
        }
    }
}
