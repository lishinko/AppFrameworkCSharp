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
    public enum TickInfo
    {
        NotSupported,
        AppTickable,
        SelfTickable,
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
        /// <summary>
        /// 可以支持tick,暂时不支持unity那种update/Fixedupdate/lateupdate的"单组件多tick"系统
        /// 特殊值:-1:不支持tick, 0:引擎控制tick的fps
        /// </summary>
        public TickInfo Tick { get; set; }
    }
    public class PluginDescFile
    {
        public List<PluginDesc> Plugins { get; set; }
    }
}
