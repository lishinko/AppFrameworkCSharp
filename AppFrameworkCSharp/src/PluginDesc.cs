using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFrameworkCSharp.src
{
    public enum PluginThread
    {
        Main,
        WindowsEvent,
        Any,
        ParentPlugin,
    }
    internal class PluginDesc
    {
        public string Name { get; set; }    
        public string Description { get; set; }
        public List<string> Dependencies { get; set; }
        public PluginThread Thread { get; set; } 
    }
    internal class PluginDescFile
    {
        List<PluginDesc> Plugins { get; set; }
    }
}
