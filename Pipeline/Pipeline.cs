using PluginCore;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Pipeline
{
    public enum State
    {
        NotStarted,
        Completed,
        Continue,
    }
    public interface IPipelineNode : ITickablePlugin
    {
        State TickResult { get; }
    }
    /// <summary>
    /// pipeline中间节点
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    public interface IPipelineNode<TInput, TOutput> : IPipelineNode
    {
        TOutput Output { get; }
        TInput Input { get; }
    }
    /// <summary>
    /// pipeline输出节点
    /// </summary>
    /// <typeparam name="TOutput"></typeparam>
    public interface IPipelineNodeSink<TOutput> : IPipelineNode
    {
        TOutput Output { get; }
    }
    public interface IPipelineNodeSource<TInput> : IPipelineNode
    {
        TInput Input { get; }
    }
    public interface IPipeline : ITickablePlugin
    {
        void OnChildStarted(IPipelineNode node);
    }
    /// <summary>
    /// 同时只能执行一个节点的pipeline,复刻当时unity代码,
    /// </summary>
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
            var json = GetPipelineDesc();
            if (json == null)
            {
                Console.WriteLine($"getpipelinedesc failed");
                return;
            }
            SortNode(json);
            _currentIdx = 0;
        }
        private void SortNode(PipelineDescFile json)
        {
            IPipelineNode[] backup = new IPipelineNode[_nodes.Count];
            _nodes.CopyTo(backup);
            _nodes.Clear();
            foreach (var p in json.Pipelines)
            {
                foreach (var n in backup)
                {
                    if (p.Name == n.Name)
                    {
                        _nodes.Add(n);
                        break;
                    }
                }
            }
        }
        private PipelineDescFile? GetPipelineDesc()
        {
            var processPath = System.Environment.ProcessPath;
            var dir = Path.GetDirectoryName(processPath);
            if (dir == null)
            {
                Console.WriteLine("no path found!");
                return null;
            }
            var jsonPath = Path.Combine(dir, "pipeline.json");
            var str = File.ReadAllText(jsonPath);
            var options = new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
            };
            var json = JsonSerializer.Deserialize<PipelineDescFile>(str, options);
            if (json == null)
            {
                Console.WriteLine("pipeline json failed");
                return null;
            }
            return json;
        }

        public void Stop()
        {
            Console.WriteLine(nameof(Stop));
        }

        public void Init(List<IPlugin> dependencies)
        {
            _dependencies.AddRange(dependencies);
        }

        public void Tick(float deltaTime)
        {
            if (_currentIdx <= 0)
            {
                return;
            }
            var _current = _nodes[_currentIdx];
            _current.Tick(deltaTime);
            if (_current.TickResult == State.Completed)
            {
                _currentIdx++;
                _currentIdx %= _nodes.Count;
            }
        }

        public void OnChildStarted(IPipelineNode node)
        {
            _nodes.Add(node);
        }

        private PluginDesc _desc;
        private readonly List<IPlugin> _dependencies = new List<IPlugin>();
        private readonly List<IPipelineNode> _nodes = new List<IPipelineNode>();
        private int _currentIdx;
    }
}