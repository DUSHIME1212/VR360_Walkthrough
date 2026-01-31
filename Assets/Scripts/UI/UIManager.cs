using UnityEngine;
using TMPro;
using VRCampusTour.Utils;
using VRCampusTour.UI;
using VRCampusTour.Location;

namespace VRCampusTour.UI
{
    /// <summary>
    /// Manages all UI panels and displays
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [Header("UI Panels")]
        [SerializeField] private InfoPanel infoPanel;
        [SerializeField] private NavigationMenu navigationMenu;
        [SerializeField] private GameObject pauseMenu;

        [Header("HUD Elements")]
        [SerializeField] private TextMeshProUGUI locationNameText;
        [SerializeField] private TextMeshProUGUI progressText;

        void Awake()
        {
            // Singleton pattern
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            InitializePanels();
        }

        void Start()
        {
            SubscribeToEvents();
        }

        void OnDestroy()
        {
            UnsubscribeFromEvents();

            // Clear singleton reference
            if (Instance == this)
            {
                Instance = null;
            }
        }

        void InitializePanels()
        {
            if (infoPanel != null)
                infoPanel.Hide();

            if (navigationMenu != null)
                navigationMenu.Hide();

            if (pauseMenu != null)
                pauseMenu.SetActive(false);
        }

        void SubscribeToEvents()
        {
            if (Core.GameManager.Instance != null)
            {
                Core.GameManager.Instance.OnLocationChanged += HandleLocationChanged;
            }
        }

        void UnsubscribeFromEvents()
        {
            if (Core.GameManager.Instance != null)
            {
                Core.GameManager.Instance.OnLocationChanged -= HandleLocationChanged;
            }
        }

        void HandleLocationChanged(LocationData newLocation)
        {
            UpdateLocationDisplay(newLocation);
            UpdateProgress();
        }

        public void ShowInfoPanel(string title, string content)
        {
            if (infoPanel != null)
                infoPanel.Show(title, content);
        }

        public void HideInfoPanel()
        {
            if (infoPanel != null)
                infoPanel.Hide();
        }

        public void ShowNavigationMenu()
        {
            if (navigationMenu != null)
                navigationMenu.Show();
        }

        public void HideNavigationMenu()
        {
            if (navigationMenu != null)
                navigationMenu.Hide();
        }

        public void TogglePauseMenu()
        {
            if (pauseMenu != null)
            {
                bool isActive = pauseMenu.activeSelf;
                pauseMenu.SetActive(!isActive);
                Time.timeScale = isActive ? 1f : 0f;
            }
        }

        void UpdateLocationDisplay(LocationData location)
        {
            if (locationNameText != null && location != null)
            {
                locationNameText.text = location.locationName;
            }
        }

        void UpdateProgress()
        {
            if (progressText != null && Core.GameManager.Instance != null)
            {
                int current = Core.GameManager.Instance.CurrentLocation != null ? 1 : 0;
                int total = Core.GameManager.Instance.TotalLocations;
                progressText.text = $"Location {current}/{total}";
            }
        }
    }
}