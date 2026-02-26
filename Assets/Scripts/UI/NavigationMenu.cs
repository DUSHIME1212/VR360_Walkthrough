using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace VRCampusTour.UI
{
    /// <summary>
    /// Menu for quick navigation between locations
    /// </summary>
    public class NavigationMenu : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private Transform buttonContainer;
        [SerializeField] private GameObject navigationButtonPrefab;
        [SerializeField] private Button closeButton;
        [SerializeField] private CanvasGroup canvasGroup;

        // [Header("Audio")]
        // [SerializeField] private AudioClip openSound;
        // [SerializeField] private AudioClip closeSound;

        private List<Button> navigationButtons = new List<Button>();
        private bool isVisible = false;

        void Awake()
        {
            if (closeButton != null)
            {
                closeButton.onClick.AddListener(Hide);
            }

            if (canvasGroup == null)
            {
                canvasGroup = GetComponent<CanvasGroup>();
            }
        }

        void Start()
        {
            GenerateNavigationButtons();
            Hide();
        }

        void GenerateNavigationButtons()
        {
            if (Core.GameManager.Instance == null || buttonContainer == null || navigationButtonPrefab == null)
            {
                return;
            }

            // Clear existing buttons
            foreach (Transform child in buttonContainer)
            {
                Destroy(child.gameObject);
            }
            navigationButtons.Clear();

            // Create button for each location
            int totalLocations = Core.GameManager.Instance.TotalLocations;

            for (int i = 0; i < totalLocations; i++)
            {
                Location.LocationData location = Core.GameManager.Instance.GetLocation(i);
                if (location == null) continue;

                GameObject buttonObj = Instantiate(navigationButtonPrefab, buttonContainer);
                Button button = buttonObj.GetComponent<Button>();
                Text buttonText = buttonObj.GetComponentInChildren<Text>();

                if (buttonText != null)
                {
                    buttonText.text = location.locationName;
                }

                int locationIndex = i; // Capture for closure
                if (button != null)
                {
                    button.onClick.AddListener(() => OnNavigationButtonClicked(locationIndex));
                    navigationButtons.Add(button);
                }
            }
        }

        void OnNavigationButtonClicked(int locationIndex)
        {
            if (Core.GameManager.Instance != null)
            {
                Core.GameManager.Instance.NavigateToLocation(locationIndex);
            }

            Hide();
        }

        public void Show()
        {
            isVisible = true;

            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1f;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }

            gameObject.SetActive(true);

            // if (openSound != null && Core.AudioManager.Instance != null)
            // {
            //     Core.AudioManager.Instance.PlaySFX(openSound);
            // }
        }

        public void Hide()
        {
            isVisible = false;

            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0f;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }

            gameObject.SetActive(false);

            // if (closeSound != null && Core.AudioManager.Instance != null)
            // {
            //     Core.AudioManager.Instance.PlaySFX(closeSound);
            // }
        }

        public void Toggle()
        {
            if (isVisible)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }
    }
}