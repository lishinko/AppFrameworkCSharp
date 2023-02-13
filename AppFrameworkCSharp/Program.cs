using PluginCore;

namespace AppFrameworkCSharp
{

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            using PluginManager manager = new PluginManager();
            manager.LoadPlugins();
            using var executor = new NormalExecutor();
            executor.Build(manager);

            Console.WriteLine("please input process id");
            var idStr = Console.ReadLine();
            if (string.IsNullOrEmpty(idStr))
            {
                Console.WriteLine($"no id !!!");
                return;
            }
            var c = new WindowCapture.WindowCapture();
            c.CommandLine = new[] { idStr }; 
            executor.Build(c);
            //executor.BuildFile("cmd.json");
            executor.Start();
            manager.Run();
        }
    }
}