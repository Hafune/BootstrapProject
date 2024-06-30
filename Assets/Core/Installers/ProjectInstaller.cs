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
        
        DontDestroyOnLoad(projectDependencies);
        projectDependencies.gameObject.SetActive(true);
        
        SceneManager.LoadScene(_nextScene);
    }
}