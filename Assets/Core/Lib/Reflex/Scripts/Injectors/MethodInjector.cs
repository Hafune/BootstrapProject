using System;
using Reflex.Scripts.Caching;

namespace Reflex.Injectors
{
	internal static class MethodInjector
	{
		public static void InjectMany(InjectedMethodInfo[] methods, object instance, Context context)
		{
			for (var i = 0; i < methods.Length; i++)
			{
				Inject(methods[i], instance, context);
			}
		}
        
		private static void Inject(InjectedMethodInfo method, object instance, Context context)
		{
			var arguments = ExactArrayPool<object>.Shared.Rent(method.Parameters.Length);

			for (int i = 0; i < method.Parameters.Length; i++)
			{
				arguments[i] = context.Resolve(method.Parameters[i]);
			}

			try
			{
				method.Invoke(instance, arguments);
			}
			catch (Exception e)
			{
				throw new MethodInjectorException(instance, method, e);
			}
			finally
			{
				ExactArrayPool<object>.Shared.Return(arguments);
			}
		}
	}
}