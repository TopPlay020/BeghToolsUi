using System.Reflection;
using System.Windows;

namespace BeghToolsUi
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider Services { get; private set; } = default!;

        protected override void OnStartup(StartupEventArgs startupEventArgs)
        {
            var services = new ServiceCollection();

            //Load Transientable classes
            foreach (var type in GetTypesImplementing<ITransientable>())
                services.AddTransient(type);


            //Load Singleton classes
            foreach (var type in GetTypesImplementing<ISingletonable>())
                services.AddSingleton(type);

            Services = services.BuildServiceProvider();
            if (startupEventArgs.Args.Length != 0)
                foreach (var type in GetTypesImplementing<IArgumentPlayable>())
                {
                    var attr = type.GetCustomAttribute<ArgumentPlayableAttribute>()!;
                    if (startupEventArgs.Args[0] == attr.ArgumentName)
                    {
                        (GetRequiredService<IArgumentPlayable>(type)).PlayWithArgument(startupEventArgs);
                        Shutdown();
                        return;
                    }

                }
            else
            {
                foreach (var type in GetTypesImplementing<IAutoStartGUI>())
                    GetRequiredService<IAutoStartGUI>(type);
            }
        }
    }

}
