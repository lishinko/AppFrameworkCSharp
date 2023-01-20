using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginCore
{
    public class PipelineDesc
    {
        public string Name;
    }
    public class PipelineDescFile
    {
        public List<PipelineDesc> Pipelines { get; set; }
    }
}
