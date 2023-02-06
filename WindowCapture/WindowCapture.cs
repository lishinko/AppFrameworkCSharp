using Pipeline;
using PluginCore;
using System.ComponentModel;
using Windows.Win32;

namespace WindowCapture
{
    public class CaptureResult
    {
    }
    public class WindowCapture
    {
        public CaptureResult Output => throw new NotImplementedException();
        public string TargetName = string.Empty;
        public void Init()
        {
        }
        public void Start()
        {
            //System.Diagnostics.Process(TargetName);
            FreeLibrarySafeHandle handle;
            handle = PInvoke.GetModuleHandle(TargetName);

            PInvoke.GetWindowRect(handle, )
        }

    }
}