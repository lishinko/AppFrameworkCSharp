using PluginCore;
using System.Diagnostics;

namespace Pipeline
{
    public interface INode : IPlugin
    {

    }
    public interface IPipeline : IPlugin
    {

    }
    public class Pipeline : IPipeline
    {
        public Pipeline(PluginDesc desc)
        {
            Console.WriteLine("ctor");
            _desc = desc;
        }
        public string Name => "流水线引擎,插件集成测试";

        public List<IPlugin> Dependencies => _dependencies;

        public PluginDesc Desc => _desc;

        public void Dispose()
        {
            Console.WriteLine(nameof(Dispose));
        }

        public void Start()
        {
            Console.WriteLine(nameof(Start));
        }

        public void Stop()
        {
            Console.WriteLine(nameof(Stop));
        }

        public void Init(List<IPlugin> dependencies)
        {
            _dependencies.AddRange(dependencies);
        }

        private PluginDesc _desc;
        private readonly List<IPlugin> _dependencies = new List<IPlugin>();
    }
}