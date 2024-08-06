using Lib;
using UnityEngine.SceneManagement;

namespace Reflex.Injectors
{
	internal static class SceneInjector
	{
		internal static void Inject(Scene scene, Context context)
		{
			foreach (var monoConstruct in scene.All<MonoConstruct>())
				MonoConstruct.SetupContext(context, monoConstruct);
		}
	}
}