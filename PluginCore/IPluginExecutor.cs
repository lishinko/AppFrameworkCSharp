using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginCore
{
    public interface IPluginExecutor : IDisposable
    {
        void Build(PluginManager manager);
        void Start();
        void Stop();
    }
    public interface ICommandLineExecutor : IPluginExecutor
    {
        void BuildFile(string runtimeFile);
    }
    public class CommandLineAppInfo
    {
        public string Name { get; set; }
        public string CommandLine { get; set; }
    }
    public class WindowsCommandLineExecutor : ICommandLineExecutor
    {
        public void Build(PluginManager manager)
        {
            _manager = manager;
        }
        public void BuildFile(string runtimeFile)
        {
            var json = PluginManager.LoadJson<CommandLineAppInfo>(runtimeFile);
            if (json == null)
            {
                return;
            }
            var plugin = _manager.GetPlugin(json.Name) as ICommandLinePlugin;
            if (plugin == null)
            {
                return;
            }
            var commands = json.CommandLine.Split(' ', '\t');
            if (commands.Length >= 2)
            {
                _exe = commands[0];
                _args = commands[1..];
            }
        }
        public void Dispose()
        {

        }

        public void Start()
        {
            if(_exe == null)
            {
                return;
            }
            var process = System.Diagnostics.Process.Start(_exe, _args);
            process.Start();
            process.WaitForExit();
        }

        public void Stop()
        {

        }
        private PluginManager? _manager;
        private string? _exe;
        private string[]? _args;
    }
}
