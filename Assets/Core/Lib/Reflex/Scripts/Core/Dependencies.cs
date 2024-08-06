using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Reflex.Scripts.Core
{
    public class Dependencies : MonoBehaviour
    {
        [SerializeField] private ScriptableObject[] ScriptableObjects = Array.Empty<ScriptableObject>();
        [SerializeField] private Component[] ComponentDependencies = Array.Empty<Component>();
        [SerializeField] private MonoBehaviour[] MonoDependencies = Array.Empty<MonoBehaviour>();

        private void OnValidate()
        {
            HashSet<ScriptableObject> SoSet = new();
            ScriptableObjects = ScriptableObjects.Where(i => i && SoSet.Add(i)).ToArray();

            HashSet<MonoBehaviour> monoSet = new();
            MonoDependencies = MonoDependencies.Where(i => i && monoSet.Add(i)).ToArray();

            HashSet<Component> comSet = new();
            ComponentDependencies = ComponentDependencies.Where(i => i && comSet.Add(i)).ToArray();
        }

        public void BindInstances(Context context)
        {
            for (int i = 0, iMax = ScriptableObjects.Length; i < iMax; i++)
                context.BindInstanceAs(ScriptableObjects[i], ScriptableObjects[i].GetType());

            for (int i = 0, iMax = ComponentDependencies.Length; i < iMax; i++)
                context.BindInstanceAs(ComponentDependencies[i], ComponentDependencies[i].GetType());

            for (int i = 0, iMax = MonoDependencies.Length; i < iMax; i++)
                context.BindInstanceAs(MonoDependencies[i], MonoDependencies[i].GetType());
        }

        [ContextMenu(nameof(Sort))]
        private void Sort()
        {
            ScriptableObjects = ScriptableObjects.Where(i => i).OrderBy(i => i.name).ToArray();
            ComponentDependencies = ComponentDependencies.Where(i => i).OrderBy(i => i.name).ToArray();
            MonoDependencies = MonoDependencies.Where(i => i).OrderBy(i => i.name).ToArray();
        }
    }
}