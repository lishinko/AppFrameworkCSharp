using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginCore
{
    public class PluginManager : IDisposable
    {
        public void LoadPlugins()
        {
            var processPath = System.Environment.ProcessPath;
            var dir = Path.GetDirectoryName(processPath);
            if (dir == null)
            {
                Console.WriteLine("no path found!");
                return;
            }
            var jsonPath = System.IO.Path.Combine(dir, "plugins.json");
            var str = System.IO.File.ReadAllText(jsonPath);
            var json = System.Text.Json.JsonSerializer.Deserialize<PluginDescFile>(str);
            if (json == null)
            {
                Console.WriteLine("plugin json failed");
                return;
            }
            var plugins = json.Plugins;
            foreach (var plugin in plugins)
            {
                var pluginPath = Path.Combine(dir, plugin.Name, $"{plugin.Name}.dll");
                var handle = Activator.CreateInstance(pluginPath, plugin.Type);
                if (handle == null)
                {
                    Console.WriteLine($"createInstance failed, name = {plugin.Name}");
                    continue;
                }
                var obj = handle.Unwrap();
                if (obj == null)
                {
                    Console.WriteLine($"unwrap failed, name = {plugin.Name}");
                    continue;
                }
                var p = (IPlugin)obj;
                Console.WriteLine($"found plugin p {p.Name}");
                _plugins.Add(p);
            }
        }
        public void Run()
        {
            foreach(var p in _plugins)
            {
                p.Init();
            }
            foreach(var p in _plugins)
            {
                p.Start();
            }
        }

        public void Dispose()
        {
            foreach(var p in _plugins)
            {
                p.Dispose();
            }
            _plugins.Clear();
        }

        private readonly List<IPlugin> _plugins = new List<IPlugin>();
    }
}
