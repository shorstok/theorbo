using System;
using Autofac;
using log4net;
using theorbo.Logging;
using theorbo.Services;
using Topshelf;

namespace theorbo
{
    internal class Program
    {
        private static readonly ILog logger = Log.Get(typeof(Program));

        private static IContainer CreateDiContainer()
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterModule<TheorboModule>();

            return containerBuilder.Build();
        }

        private static void Main(string[] args)
        {
            logger.Info($"Started {DateTime.Now}");

            PathService.EnsurePathExists();

            logger.Info($"Service data resides in `{PathService.AppData}`");

            var container = CreateDiContainer();

            HostFactory.Run(configurator =>
            {
                configurator.UseLog4Net();
                configurator.StartAutomatically();

                configurator.EnableServiceRecovery(rc =>
                {
                    rc.RestartService(1); // restart the service after 1 minute
                });

                configurator.Service<TelegramBotService>(s =>
                {
                    s.ConstructUsing(hostSettings => container.Resolve<TelegramBotService>());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });

                configurator.RunAsLocalSystem();

                configurator.SetDescription("Telegram Theory Bot host");
                configurator.SetDisplayName("Telegram Theory Bot");
                configurator.SetServiceName("Theorbo");
            });
        }
    }
}