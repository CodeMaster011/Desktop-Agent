﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopAgentWpf.Backups
{
    [Agent("{5D9B2D87-958B-4564-9C13-ACF536CC2403}")]
    public class BackupAgent : BaseAgent, IEventHandler
    {
        public void Handle(string eventName, object eventData)
        {
            if (Launcher.LauncherAgent.AfterProcessExit == eventName
                && eventData is Launcher.LauncherAgentOption launcherAgentOption)
            {
                var option = this.GetOption<BackupAgentOption>(AgentName);
                if (launcherAgentOption.AppName != option.AppName)
                    return;

                Backup(option);
            }
        }

        protected void Backup(BackupAgentOption option)
        {
            var backupText = GetBackupText(option.BackupFormat);
            var srcLocation = option.Location;
            var desLocation = Path.Combine(option.BackupLocation, backupText);
            if (!option.Compression)
            {
                if (!Directory.Exists(desLocation))
                    Directory.CreateDirectory(desLocation);

                var allDirs = Directory.GetDirectories(srcLocation, "**", SearchOption.AllDirectories)
                    .Select(d => d.Replace(srcLocation, desLocation))
                    .ToArray();
                foreach (var item in allDirs)
                    if (!Directory.Exists(item))
                        Directory.CreateDirectory(item);

                var allSrcFiles = Directory.GetFiles(srcLocation, "**", SearchOption.AllDirectories);
                foreach (var srcFilePath in allSrcFiles)
                {
                    var dirFilePath = srcFilePath.Replace(srcLocation, desLocation);
                    File.Copy(srcFilePath, dirFilePath);
                }
            }
            else
            {
                // todo: Do GZIP Compression
            }
        }

        protected string GetBackupText(string backupFormat)
        {
            var date = DateTime.Now;
            return backupFormat.Replace("[date]", date.ToString("yyyy_MM_dd"))
                .Replace("[time]", date.ToString("HH_mm_ss"));
        }

        public override void Initialize(string agentName)
        {
            base.Initialize(agentName);
            Global.EventPool.RegisterForEvent(Launcher.LauncherAgent.AfterProcessExit, this);
        }
    }

    public class BackupAgentOption
    {
        public string AppName { get; set; }
        public string Location { get; set; }
        public string BackupLocation { get; set; }
        public string BackupFormat { get; set; }
        public bool AutoBackupAfterExit { get; set; }
        public bool Compression { get; set; }
    }
}
