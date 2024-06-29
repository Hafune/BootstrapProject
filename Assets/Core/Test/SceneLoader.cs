using Lib;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Test
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private SceneField _scene;

        public void Load() => SceneManager.LoadScene(_scene);
    }
}