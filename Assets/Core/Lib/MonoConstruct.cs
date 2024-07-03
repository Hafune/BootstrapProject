using System.Runtime.CompilerServices;
using Reflex;
using UnityEngine;

namespace Lib
{
    public abstract class MonoConstruct : MonoBehaviour
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetupContext(Context c, MonoConstruct monoConstruct) => monoConstruct.Context = c;

        protected Context Context { get; private set; }
    }
}