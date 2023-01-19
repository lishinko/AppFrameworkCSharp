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
        }
    }
}