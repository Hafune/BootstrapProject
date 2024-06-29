using System;
using System.Collections.Generic;
using Lib;
using Reflex.Scripts.Enums;
using UnityEngine;

namespace Reflex.Scripts.Extensions
{
    internal static class ComponentExtensions
    {
        private static readonly List<MonoConstruct> _list = new();

        internal static List<MonoConstruct> GetInjectables<T>(this T component, MonoInjectionMode injectionMode)
            where T : Component
        {
            switch (injectionMode)
            {
                case MonoInjectionMode.Single:
                {
                    _list.Clear();
                    if (component.TryGetComponent<MonoConstruct>(out var c))
                        _list.Add(c);

                    return _list;
                }
                case MonoInjectionMode.Object:
                {
                    component.GetComponents(_list);
                    return _list;
                }
                case MonoInjectionMode.Recursive:
                {
                    component.GetComponentsInChildren(true, _list);
                    return _list;
                }
                default: throw new ArgumentOutOfRangeException(nameof(injectionMode), injectionMode, null);
            }
        }
    }
}