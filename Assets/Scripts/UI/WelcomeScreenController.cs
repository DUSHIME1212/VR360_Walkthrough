// Scripts/UI/WelcomeSceneController.cs
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using VRCampusTour.Core;

namespace VRCampusTour.UI
{
    /// <summary>
    /// Controls the welcome/intro scene
    /// </summary>
    public class WelcomeScreenController : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI subtitleText;
        [SerializeField] private Button startButton;
        [SerializeField] private Button quitButton;

        [Header("Animation")]
        [SerializeField] private CanvasGroup mainCanvasGroup;
        [SerializeField] private float introDelay = 1f;
        [SerializeField] private float fadeInDuration = 2f;

        [Header("Audio")]
        [SerializeField] private AudioClip welcomeMusic;
        [SerializeField] private AudioClip buttonClickSound;

        void Start()
        {
            SetupButtons();
            StartCoroutine(PlayIntroSequence());
        }

        void SetupButtons()
        {
            if (startButton != null)
            {
                startButton.onClick.AddListener(OnStartClicked);
            }

            if (quitButton != null)
            {
                quitButton.onClick.AddListener(OnQuitClicked);
                
                #if UNITY_WEBGL
                quitButton.gameObject.SetActive(false);
                #endif
            }
        }

        IEnumerator PlayIntroSequence()
        {
            // Start with fade
            if (mainCanvasGroup != null)
            {
                mainCanvasGroup.alpha = 0f;
            }

            // Play welcome music
            if (welcomeMusic != null && AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayMusic(welcomeMusic, true);
            }

            // Wait for intro delay
            yield return new WaitForSeconds(introDelay);

            // Fade in UI
            if (mainCanvasGroup != null)
            {
                float elapsed = 0f;
                while (elapsed < fadeInDuration)
                {
                    elapsed += Time.deltaTime;
                    mainCanvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / fadeInDuration);
                    yield return null;
                }
                mainCanvasGroup.alpha = 1f;
            }
        }

        void OnStartClicked()
        {
            PlayButtonSound();
            
            if (SceneController.Instance != null)
            {
                SceneController.Instance.LoadWalkthroughScene();
            }
        }

        void OnQuitClicked()
        {
            PlayButtonSound();
            
            if (SceneController.Instance != null)
            {
                SceneController.Instance.QuitApplication();
            }
        }

        void PlayButtonSound()
        {
            if (buttonClickSound != null && AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(buttonClickSound);
            }
        }
    }
}