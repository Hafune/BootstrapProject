using UnityEngine;
using Reflex.Scripts.Core;
using Reflex.Scripts.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting;

[assembly: AlwaysLinkAssembly] // https://docs.unity3d.com/ScriptReference/Scripting.AlwaysLinkAssemblyAttribute.html

namespace Reflex.Injectors
{
#if UNITY_EDITOR
    public static class EditorContextAccess
    {
        public static Context context;
    }
#endif

    internal static class UnityInjector
    {
        internal static void BeforeAwakeOfFirstSceneOnly(ProjectContext projectContext)
        {
            var projectContainer = CreateProjectContainer(projectContext);
            UnityStaticEvents.OnSceneEarlyAwake += scene =>
            {
                var sceneContainer = CreateSceneContainer(scene, projectContainer);
                SceneInjector.Inject(scene, sceneContainer);
            };
#if UNITY_EDITOR
            EditorContextAccess.context = projectContainer;
#endif
        }

        private static Context CreateProjectContainer(ProjectContext projectContext)
        {
            var container = ContextTree.Root = new Context("ProjectContainer");

            Application.quitting += () =>
            {
                ContextTree.Root = null;
                container.Dispose();
            };

            projectContext.InstallBindings(container);

            return container;
        }

        private static Context CreateSceneContainer(Scene scene, Context projectContext)
        {
            var container = projectContext.Scope(scene.name);

            var subscription = scene.OnUnload(() => { container.Dispose(); });

            // If app is being closed, all containers will be disposed by depth first search starting from project container root, see UnityInjector.cs
            Application.quitting += () => { subscription.Dispose(); };

            if (scene.TryFindAtRootObjects<SceneContext>(out var sceneContext))
            {
                sceneContext.InstallBindings(container);
            }

            return container;
        }
    }
}