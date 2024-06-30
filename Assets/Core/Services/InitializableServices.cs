using System.Collections.Generic;
using Core.Services;
using Reflex;

public class InitializableServices
{
    private readonly List<IInitializableService> _list = new(16);

    public T Add<T>(T service) where T : IInitializableService
    {
        _list.Add(service);
        return service;
    }

    public void Initialize(Context context)
    {
        foreach (var service in _list)
            service.InitializeService(context);
    }
}