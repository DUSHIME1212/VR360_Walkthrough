using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using VRCampusTour.Utils;
using VRCampusTour.UI;

namespace VRCampusTour.Core
{
    /// <summary>
    /// Handles scene transitions and loading
    /// </summary>
    public class SceneController : Singleton<SceneController>
    {
        [SerializeField] private float minimumLoadTime = 1f;

        private bool isLoading = false;

        public void LoadWelcomeScene()
        {
            LoadSceneWithFade(Constants.SCENE_WELCOME);
        }

        public void LoadWalkthroughScene()
        {
            LoadSceneWithFade(Constants.SCENE_WALKTHROUGH);
        }

        public void LoadOutroScene()
        {
            LoadSceneWithFade(Constants.SCENE_OUTRO);
        }

        public void QuitApplication()
        {
            StartCoroutine(QuitWithFade());
        }

        void LoadSceneWithFade(string sceneName)
        {
            if (isLoading) return;
            StartCoroutine(LoadSceneCoroutine(sceneName));
        }

        IEnumerator LoadSceneCoroutine(string sceneName)
        {
            isLoading = true;

            // Fade out
            if (FadeController.Instance != null)
            {
                yield return FadeController.Instance.FadeOut();
            }

            // Minimum load time for smooth experience
            float loadStartTime = Time.time;

            // Load scene asynchronously
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            asyncLoad.allowSceneActivation = false;

            // Wait for scene to load
            while (!asyncLoad.isDone)
            {
                // Check if minimum time has passed
                float elapsedTime = Time.time - loadStartTime;
                if (asyncLoad.progress >= 0.9f && elapsedTime >= minimumLoadTime)
                {
                    asyncLoad.allowSceneActivation = true;
                }

                yield return null;
            }

            // Fade in
            if (FadeController.Instance != null)
            {
                yield return FadeController.Instance.FadeIn();
            }

            isLoading = false;
        }

        IEnumerator QuitWithFade()
        {
            // Fade out
            if (FadeController.Instance != null)
            {
                yield return FadeController.Instance.FadeOut();
            }

            yield return new WaitForSeconds(0.5f);

            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
    }
}