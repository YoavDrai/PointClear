using UnityEngine;
using UnityEngine.SceneManagement;

namespace PointClear.Managers
{
    /// <summary>
    /// Single point of responsibility for loading scenes, including the
    /// initial Bootstrap-to-first-scene transition. Future scenes should be
    /// loaded through this manager rather than calling SceneManager directly.
    /// </summary>
    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader Instance { get; private set; }

        [SerializeField]
        private string initialSceneName = "PrototypeScene";

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            if (!string.IsNullOrEmpty(initialSceneName))
            {
                LoadScene(initialSceneName);
            }
        }

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
    }
}
