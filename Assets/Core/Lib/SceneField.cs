using System;
using UnityEngine;
using Object = UnityEngine.Object;


namespace Lib
{
    [Serializable]
    public class SceneField
    {
        [SerializeField] private Object m_SceneAsset;
        [SerializeField] private string m_SceneName = "";

        public string SceneName
        {
            get => m_SceneName;
            set => m_SceneName = value;
        }

        // makes it work with the existing Unity methods (LoadLevel/LoadScene)
        public static implicit operator string(SceneField sceneField)
        {
            return sceneField.SceneName;
        }
    }
}