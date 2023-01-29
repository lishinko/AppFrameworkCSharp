using Pipeline;
using PluginCore;

namespace WindowCapture
{
    public class CaptureResult
    {
    }
    public class WindowCapture : IPipelineNodeSink<CaptureResult>
    {
        public CaptureResult Output => throw new NotImplementedException();

        public State TickResult => throw new NotImplementedException();

        public List<IPlugin> Dependencies => throw new NotImplementedException();

        public PluginDesc Desc => throw new NotImplementedException();

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Init(List<IPlugin> dependencies)
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public void Tick(float deltaTime)
        {
            throw new NotImplementedException();
        }
    }
}