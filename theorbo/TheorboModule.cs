using Autofac;
using theorbo.Config;
using theorbo.MusicTheory;
using theorbo.Services;

namespace theorbo
{
    internal class TheorboModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(BotConfiguration.LoadOrCreate(true)).AsSelf().SingleInstance();
            builder.RegisterType<TelegramBotService>().AsSelf().SingleInstance();
            builder.RegisterType<IanringSource>().AsSelf().SingleInstance();

            //Register all telegram command handlers

            builder.RegisterAssemblyTypes(typeof(ITelegramCommandHandler).Assembly)
                .Where(t => !t.IsAbstract && typeof(ITelegramCommandHandler).IsAssignableFrom(t))
                .As<ITelegramCommandHandler>()
                .SingleInstance();
        }
    }
}