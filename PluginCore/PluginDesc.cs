using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginCore
{
    public enum PluginThread
    {
        Main,
        WindowsEvent,
        Any,
        ParentPlugin,
    }
    public class PluginDesc
    {
        public string Name { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// 通过反射创建类
        /// </summary>
        public string Type { get; set; }
        public readonly List<string> Dependencies = new List<string>();
        public PluginThread Thread { get; set; }
    }
    public class PluginDescFile
    {
        public List<PluginDesc> Plugins { get; set; }
    }
}
