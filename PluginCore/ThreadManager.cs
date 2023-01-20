using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginCore
{
    internal struct ThreadInfo
    {
        public Thread Thread;
        public CancellationTokenSource Cts;
    }
    internal class ThreadManager : IDisposable
    {
        public CancellationToken RunInDedicatedThread(Action action)
        {
            var threadStart = new ThreadStart(action);
            Thread thread = new Thread(threadStart);
            var src = new CancellationTokenSource();
            var token = src.Token;
            thread.Start(token);
            _dedicatedThreads.Add(new ThreadInfo { Thread = thread, Cts = src });
            return token;
        }
        public CancellationToken RunInThreadPool(Action action)
        {
            var src = new CancellationTokenSource();
            var token = src.Token;
            var task = Task.Run(action, token);
            _threadPoolCts.Add(src);
            return token;
        }

        public void Dispose()
        {
            foreach (var cts in _threadPoolCts)
            {
                cts.Cancel();
            }
            foreach (var thread in _dedicatedThreads)
            {
                thread.Cts.Cancel();
            }
            foreach (var thread in _dedicatedThreads)
            {
                thread.Thread.Join();
            }
            _threadPoolCts.Clear();
            _dedicatedThreads.Clear();
        }

        private readonly List<ThreadInfo> _dedicatedThreads = new List<ThreadInfo>();
        private readonly List<CancellationTokenSource> _threadPoolCts = new List<CancellationTokenSource>();
    }
}
