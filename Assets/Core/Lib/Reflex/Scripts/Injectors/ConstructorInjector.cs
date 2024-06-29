using System;
using Reflex.Scripts.Caching;

namespace Reflex.Injectors
{
	internal static class ConstructorInjector
	{
		internal static T ConstructAndInject<T>(Context context)
		{
			return (T) ConstructAndInject(typeof(T), context);
		}
		
		internal static object ConstructAndInject(Type concrete, Context context)
		{
			var info = TypeConstructionInfoCache.Get(concrete);
			var objects = ExactArrayPool<object>.Shared.Rent(info.ConstructorParameters.Length);
			GetConstructionObjects(info.ConstructorParameters, context, ref objects);

			try
			{
				return info.ObjectActivator.Invoke(objects);
			}
			catch (Exception e)
			{
				throw new Exception(
					$"Error occurred while instantiating object with type '{concrete.GetFormattedName()}'\n\n{e.Message}");
			}
			finally
			{
				ExactArrayPool<object>.Shared.Return(objects);
			}
		}

		private static void GetConstructionObjects(Type[] parameters, Context context, ref object[] array)
		{
			for (int i = 0; i < parameters.Length; i++)
			{
				array[i] = context.Resolve(parameters[i]);
			}
		}
	}
}