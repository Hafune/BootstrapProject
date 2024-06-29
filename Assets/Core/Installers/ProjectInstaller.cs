using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Services;
using Lib;
using Reflex;
using Reflex.Scripts;
using Reflex.Scripts.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProjectInstaller : Installer
{
    [SerializeField] private Dependencies dependencies;
    [SerializeField] private SceneField _nextScene;
    private InitializableServices _initializableServices;

    public override void InstallBindings(Context context)
    {
        var projectDependencies = context.Instantiate(dependencies);
        projectDependencies.BindInstances(context);
        
        var initializableServices = new InitializableServices();

        context.BindInstanceAs(initializableServices.Add(new MainMenuService()));
        context.BindInstanceAs(new GlobalStateService());
        context.BindInstanceAs(new PlayerInputs().Player);

        initializableServices.Initialize(context);
        EnableProjectDependencies(projectDependencies.gameObject);
        
        SceneManager.LoadScene(_nextScene);
    }

    private void EnableProjectDependencies(GameObject projectDependencies)
    {
        DontDestroyOnLoad(projectDependencies);
        projectDependencies.SetActive(true);

#if UNITY_EDITOR
        int index = 0;
        foreach (var go in gameObject.scene.GetRootGameObjects().OrderBy(i => i.name))
            go.transform.SetSiblingIndex(index++);
#endif
    }

    private class InitializableServices
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
}