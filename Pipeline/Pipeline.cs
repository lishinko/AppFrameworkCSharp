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
        public string Name => "流水线引擎,插件集成测试";

        public List<IPlugin> Dependencies => new List<IPlugin>();

        public void Dispose()
        {
            Console.WriteLine(nameof(Dispose));
        }

        public void Init()
        {
            Console.WriteLine("init");
        }

        public void Start()
        {
            Console.WriteLine(nameof(Start));
        }

        public void Stop()
        {
            Console.WriteLine(nameof(Stop));
        }
    }
}