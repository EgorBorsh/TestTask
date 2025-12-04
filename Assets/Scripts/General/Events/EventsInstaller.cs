using Zenject;

public class EventsInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind(typeof(IEventBusNotResult<>)).To(typeof(EventsBusNotResult<>)).AsSingle();
        Container.Bind(typeof(IEventBusResult<,>)).To(typeof(EventBusResult<,>)).AsSingle();
    }
}
