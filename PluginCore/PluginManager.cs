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
                var handle = Activator.CreateInstanceFrom(pluginPath, plugin.Type, false, System.Reflection.BindingFlags.Public, null, new object[] { plugin }, null, null); ;
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
            var list = BuildDependencies();
            foreach (var p in _plugins)
            {
                var d = list[p];
                p.Init(d ?? new List<IPlugin>());
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
        /// <summary>
        /// 添加依赖项,这里没有检查依赖项是否循环
        /// </summary>
        private Dictionary<IPlugin, List<IPlugin>?> BuildDependencies()
        {
            Dictionary<IPlugin, List<IPlugin>?> plugins = new Dictionary<IPlugin, List<IPlugin>?>();
            foreach (var p in _plugins)
            {
                if (p.Desc.Dependencies.Count <= 0)
                {
                    plugins.Add(p, null);
                }
            }
            for (int loopGuard = 0; plugins.Count < _plugins.Count && loopGuard < 1000; loopGuard++)
            {
                foreach (var p in _plugins)
                {
                    if (plugins.ContainsKey(p))
                    {
                        continue;
                    }
                    var deps = new List<IPlugin>();
                    foreach (var s in p.Desc.Dependencies)
                    {
                        foreach (var dep in plugins.Keys)
                        {
                            if (dep != p && s == dep.Name && !deps.Contains(dep))
                            {
                                deps.Add(dep);
                            }
                        }
                    }
                    if (deps.Count >= p.Desc.Dependencies.Count)
                    {
                        plugins.Add(p, deps);
                    }
                }
            }
            if (plugins.Count < _plugins.Count)
            {
                throw new InvalidDataException($"create dependencies failed, maybe there is circular dependencies!");
            }
            return plugins;
        }

        private readonly List<IPlugin> _plugins = new List<IPlugin>();
        private readonly ThreadManager _threadManager = new ThreadManager();
    }
}
