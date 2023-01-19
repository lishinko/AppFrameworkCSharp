using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFrameworkCSharp.src
{
    internal class PluginManager
    {
        public void LoadPlugins()
        {
            var dir = System.Environment.CurrentDirectory;
            var path = System.IO.Path.Combine(dir, "plugins.json");
            var str = System.IO.File.ReadAllText(path);
            var json = System.Text.Json.JsonSerializer.Deserialize<PluginDescFile>(str);

        }
    }
}
