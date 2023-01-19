﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFrameworkCSharp.src
{
    internal interface IPlugin : IDisposable
    {
        List<IPlugin> Dependencies { get; }
        void Init();
        void Start();
        void Stop();
    }
    public abstract class PluginBase : IPlugin
    {
        List<IPlugin> IPlugin.Dependencies => throw new NotImplementedException();

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Init()
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
    }

}
