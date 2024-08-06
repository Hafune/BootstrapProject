using System;

namespace Reflex
{
	public interface IResolver : IDisposable
	{
		Type Concrete { get; }
		int Resolutions { get; }
		object Resolve(Context context);
	}
}