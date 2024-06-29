using Reflex.Injectors;
using UnityEngine;

namespace Reflex.Scripts.Core
{
    [DefaultExecutionOrder(-10001)]
    public class ProjectContext : AContext
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            UnityInjector.BeforeAwakeOfFirstSceneOnly(this);
        }

        public override void InstallBindings(Context context)
        {
            base.InstallBindings(context);
            Debug.Log($"{GetType().Name} Bindings Installed");
        }
    }
}