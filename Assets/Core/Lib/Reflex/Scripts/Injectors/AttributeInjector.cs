using Reflex.Scripts.Caching;

namespace Reflex.Injectors
{
	internal static class AttributeInjector
	{
		internal static void Inject(object obj, Context context)
		{
			var info = TypeAttributeInfoCache.Get(obj.GetType());
			// FieldInjector.InjectMany(info.InjectableFields, obj, container);
			// PropertyInjector.InjectMany(info.InjectableProperties, obj, container);
			MethodInjector.InjectMany(info.InjectableMethods, obj, context);
		}
	}
}