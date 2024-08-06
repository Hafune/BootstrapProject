using System;
using System.Collections.Generic;
using Lib;
using UnityEngine;

namespace Reflex.Scripts.Utilities
{
    internal static class MonoInstantiate
    {
        private static Transform _hiddenParent;
        private static readonly List<MonoConstruct> _list = new();

        private static Transform HiddenParent
        {
            get
            {
                if (_hiddenParent != null)
                    return _hiddenParent;

                var gameObject = new GameObject("[Reflex] Hidden Parent");
                gameObject.SetActive(false);
                gameObject.hideFlags = HideFlags.DontSave;
                _hiddenParent = gameObject.transform;

                return _hiddenParent;
            }
        }

        internal static T Instantiate<T>(T original, Transform parent, Context context, Func<Transform, T> instantiate) where T : Component
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

            instance.GetComponentsInChildren(true, _list);

            for (int i = 0, iMax = _list.Count; i < iMax; i++)
                MonoConstruct.SetupContext(context, _list[i]);

            if (instance.transform is RectTransform rectTransform)
                rectTransform.RestoreRectTransform(original);
            
            instance.gameObject.SetActive(prefabWasActive);

            return instance;
        }

        internal static void RestoreRectTransform(this RectTransform rectTransform, Component original)
        {
            if (original.TryGetComponent<RectTransform>(out var originalRectTransform))
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