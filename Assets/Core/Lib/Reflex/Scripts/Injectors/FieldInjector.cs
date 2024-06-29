using System;
using System.Reflection;

namespace Reflex.Injectors
{
	internal static class FieldInjector
	{
		public static void InjectMany(FieldInfo[] fields, object instance, Context context)
		{
			for (var i = 0; i < fields.Length; i++)
			{
				Inject(fields[i], instance, context);
			}
		}
        
		private static void Inject(FieldInfo field, object instance, Context context)
		{
			try
			{
				field.SetValue(instance, context.Resolve(field.FieldType));
			}
			catch (Exception e)
			{
				throw new FieldInjectorException(e);
			}
		}
	}
}