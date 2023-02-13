using Pipeline;
using PluginCore;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using Windows.Win32;

namespace WindowCapture
{
    public class CaptureResult
    {
    }
    public class WindowCapture : ICommandLinePlugin
    {
        public WindowCapture(PluginDesc desc)
        {
            Desc = desc;
        }
        public CaptureResult? Output => null;

        public List<IPlugin>? Dependencies => _dependencies;

        public PluginDesc Desc { get { return _desc; } set { _desc = value; } }

        public string[] CommandLine { get; set; }

        public string TargetName = string.Empty;
        public void Start()
        {
            //Console.WriteLine("please input process id");
            //var idStr = Console.ReadLine();
            //if (string.IsNullOrEmpty(idStr))
            //{
            //    Console.WriteLine($"no id !!!");
            //    return;
            //}
            //TargetName = idStr;
            if(CommandLine == null ||string.IsNullOrEmpty( CommandLine[0]))
            {
                Console.WriteLine("no command line!");
                return;
            }
            TargetName = CommandLine[0];
            if (!int.TryParse(TargetName, out int id))
            {
                return;
            }
            using var process = System.Diagnostics.Process.GetProcessById(id);
            if (process == null)
            {
                return;

            }
            var hWnd = process.MainWindowHandle;
            Windows.Win32.Foundation.HWND wnd = new Windows.Win32.Foundation.HWND(hWnd);
            var hr = PInvoke.GetWindowRect(wnd, out Windows.Win32.Foundation.RECT rect);
            if (!hr)
            {
                return;
            }
            using Bitmap bmp = new(rect.Width, rect.Height);
            using var g = Graphics.FromImage(bmp);
            IntPtr hdc = g.GetHdc();
            try
            {
                using SafeHandle sh = new DeleteObjectSafeHandle(hdc);
                hr = PInvoke.PrintWindow(wnd, sh, Windows.Win32.Storage.Xps.PRINT_WINDOW_FLAGS.PW_CLIENTONLY);
                if (hr)
                {
                    bmp.Save(@"e:\save.bmp", System.Drawing.Imaging.ImageFormat.Png);
                }
            }
            finally
            {
                g.ReleaseHdc(hdc);
            }
            return;
            //System.Diagnostics.Process(TargetName);
            //FreeLibrarySafeHandle handle;
            //handle = PInvoke.GetModuleHandle(TargetName);

            //PInvoke.GetWindowRect(handle, )
        }

        public void Init(List<IPlugin>? dependencies)
        {
            _dependencies = dependencies;
        }

        public void Stop()
        {
            Console.WriteLine($"stopping, name = {nameof(WindowCapture)}");
        }

        public void Dispose()
        {
        }
        private List<IPlugin> _dependencies;
        private PluginDesc _desc;
    }
}