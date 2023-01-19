using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
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
            var options = new System.Text.Json.JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
            };
            var json = System.Text.Json.JsonSerializer.Deserialize<PluginDescFile>(str, options);
            if (json == null)
            {
                Console.WriteLine("plugin json failed");
                return;
            }
            var plugins = json.Plugins;
            if (plugins == null)
            {
                Console.WriteLine("no plugin found");
                return;
            }
            foreach (var plugin in plugins)
            {
                //var pluginPath = Path.Combine(dir, plugin.Name, $"{plugin.Name}.dll");
                var pluginPath = Path.Combine(dir, $"{plugin.Name}.dll");
                if (!File.Exists(pluginPath))
                {
                    continue;
                }
                //var handle = Activator.CreateInstance(pluginPath, plugin.Type);
                var handle = Activator.CreateInstanceFrom(pluginPath, plugin.Type);
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
            foreach (var p in _plugins)
            {
                p.Init();
            }
            foreach (var p in _plugins)
            {
                p.Start();
            }
        }

        public void Dispose()
        {
            foreach (var p in _plugins)
            {
                p.Dispose();
            }
            _plugins.Clear();
        }

        private readonly List<IPlugin> _plugins = new List<IPlugin>();
    }
}
