using System.Reflection;
using Lib;
using UnityEngine.SceneManagement;

namespace Reflex.Injectors
{
	internal static class SceneInjector
	{
		private static readonly MethodInfo _method;
		private static object[] _params = new object[1];
		
		static SceneInjector() => _method = MonoConstruct.GetMethod();
		
		internal static void Inject(Scene scene, Context context)
		{
			_params[0] = context;
			
			foreach (var monoBehaviour in scene.All<MonoConstruct>())
				_method.Invoke(monoBehaviour, _params);
		}
	}
}