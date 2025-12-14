using System.Reflection;
using System.Runtime.InteropServices;

namespace BeghToolsUi
{
    public static class GlobalHelpers
    {
        public static IEnumerable<Type> GetTypesImplementing<TInterface>()
        {
            return Assembly.GetExecutingAssembly()
                           .GetTypes()
                           .Where(t => typeof(TInterface).IsAssignableFrom(t) && t.IsClass);
        }

        public static IEnumerable<Type> GetTypesWithAttribute<TAttribute>() where TAttribute : Attribute
        {
            return Assembly.GetExecutingAssembly()
                           .GetTypes()
                           .Where(t => t.IsClass && t.GetCustomAttribute<TAttribute>() != null);
        }

        public static Type? GetTypeByName(string typeName)
        {
            return Assembly.GetExecutingAssembly()
                           .GetTypes()
                           .FirstOrDefault(t => t.Name == typeName);
        }

        public static string GetCurrentExecutablePath()
        {
            return System.Diagnostics.Process.GetCurrentProcess().MainModule!.FileName;
        }

        public static List<string> WPFImageUnsupportedFormats = new() { "webp" };
    }
}
