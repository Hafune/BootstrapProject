using System;
using System.Reflection;
using Lib;
using Reflex.Scripts.Enums;
using Reflex.Scripts.Extensions;
using UnityEngine;

namespace Reflex.Scripts.Utilities
{
    internal static class MonoInstantiate
    {
        private static Transform _hiddenParent;
        private static readonly MethodInfo _method;
        private static object[] _params = new object[1];

        private static Transform HiddenParent
        {
            get
            {
                if (_hiddenParent != null)
                    return _hiddenParent;

                var gameObject = new GameObject("[Reflex] Hidden Parent");
                gameObject.SetActive(false);
                _hiddenParent = gameObject.transform;

                return _hiddenParent;
            }
        }

        static MonoInstantiate() => _method = MonoConstruct.GetMethod();

        internal static T Instantiate<T>(T original, Transform parent, Context context, Func<Transform, T> instantiate,
            MonoInjectionMode injectionMode) where T : Component
        {
            var root = parent;
            var prefabWasActive = original.gameObject.activeSelf;

            if (prefabWasActive)
                root = HiddenParent;

            var instance = instantiate.Invoke(root);

            if (prefabWasActive)
                instance.gameObject.SetActive(false);

            if (instance.transform.parent != parent)
                instance.transform.SetParent(parent, false);

            var list = instance.GetInjectables(injectionMode);
            _params[0] = context;
            for (int i = 0, count = list.Count; i < count; i++)
                _method.Invoke(list[i], _params);

            instance.gameObject.RestoreRectTransform(original);
            instance.gameObject.SetActive(prefabWasActive);

            return instance;
        }

        internal static void RestoreRectTransform(this GameObject gameObject, Component original)
        {
            if (gameObject.TryGetComponent<RectTransform>(out var rectTransform) &&
                original.TryGetComponent<RectTransform>(out var originalRectTransform))
            {
                rectTransform.offsetMax = originalRectTransform.offsetMax;
                rectTransform.offsetMin = originalRectTransform.offsetMin;
                rectTransform.anchorMax = originalRectTransform.anchorMax;
                rectTransform.anchorMin = originalRectTransform.anchorMin;
                rectTransform.localScale = originalRectTransform.localScale;
            }
        }
    }
}