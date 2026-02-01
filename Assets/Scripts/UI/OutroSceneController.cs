using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using VRCampusTour.Core;

namespace VRCampusTour.UI
{
    /// <summary>
    /// Controls the outro/end scene
    /// </summary>
    public class OutroSceneController : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI summaryText;
        [SerializeField] private TextMeshProUGUI statsText;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private Button quitButton;

        [Header("Animation")]
        [SerializeField] private CanvasGroup mainCanvasGroup;
        [SerializeField] private float fadeInDuration = 2f;

        // [Header("Audio")]
        // [SerializeField] private AudioClip outroMusic;
        // [SerializeField] private AudioClip buttonClickSound;

        void Start()
        {
            SetupButtons();
            DisplayStats();
            StartCoroutine(PlayOutroSequence());
        }

        void SetupButtons()
        {
            if (restartButton != null)
            {
                restartButton.onClick.AddListener(OnRestartClicked);
            }

            if (mainMenuButton != null)
            {
                mainMenuButton.onClick.AddListener(OnMainMenuClicked);
            }

            if (quitButton != null)
            {
                quitButton.onClick.AddListener(OnQuitClicked);
                
                #if UNITY_WEBGL
                quitButton.gameObject.SetActive(false);
                #endif
            }
        }

        void DisplayStats()
        {
            if (GameManager.Instance == null) return;

            if (summaryText != null)
            {
                summaryText.text = "Thank you for exploring the ALU Rwanda Campus!";
            }

            if (statsText != null)
            {
                int locationsVisited = GameManager.Instance.TotalLocations;
                float sessionTime = GameManager.Instance.SessionDuration;
                int minutes = Mathf.FloorToInt(sessionTime / 60f);
                int seconds = Mathf.FloorToInt(sessionTime % 60f);

                statsText.text = $"Locations Explored: {locationsVisited}\n" +
                                $"Time Spent: {minutes}m {seconds}s\n" +
                                $"Tour Completion: 100%";
            }
        }

        IEnumerator PlayOutroSequence()
        {
            // Start with fade
            if (mainCanvasGroup != null)
            {
                mainCanvasGroup.alpha = 0f;
            }

            // Play outro music
            // if (outroMusic != null && AudioManager.Instance != null)
            // {
            //     AudioManager.Instance.PlayMusic(outroMusic, true);
            // }

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

        void OnRestartClicked()
        {
            // PlayButtonSound();
            
            if (SceneController.Instance != null)
            {
                SceneController.Instance.LoadWalkthroughScene();
            }
        }

        void OnMainMenuClicked()
        {
            // PlayButtonSound();
            
            if (SceneController.Instance != null)
            {
                SceneController.Instance.LoadWelcomeScene();
            }
        }

        void OnQuitClicked()
        {
            // PlayButtonSound();
            
            if (SceneController.Instance != null)
            {
                SceneController.Instance.QuitApplication();
            }
        }

        // void PlayButtonSound()
        // {
        //     if (buttonClickSound != null && AudioManager.Instance != null)
        //     {
        //         AudioManager.Instance.PlaySFX(buttonClickSound);
        //     }
        // }
    }
}