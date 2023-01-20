using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginCore
{
    public interface IPlugin : IDisposable
    {
        string Name => Desc.Name;
        List<IPlugin> Dependencies { get; }
        void Init(List<IPlugin> dependencies);
        void Start();
        void Stop();
        PluginDesc Desc { get; }
    }

}
