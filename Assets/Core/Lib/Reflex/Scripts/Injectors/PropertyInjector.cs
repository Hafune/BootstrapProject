using System;
using System.Reflection;

namespace Reflex.Injectors
{
	internal static class PropertyInjector
	{
		public static void InjectMany(PropertyInfo[] properties, object instance, Context context)
		{
			for (var i = 0; i < properties.Length; i++)
			{
				Inject(properties[i], instance, context);
			}
		}
        
		private static void Inject(PropertyInfo property, object instance, Context context)
		{
			try
			{
				property.SetValue(instance, context.Resolve(property.PropertyType));
			}
			catch (Exception e)
			{
				throw new PropertyInjectorException(e);
			}
		}
	}
}