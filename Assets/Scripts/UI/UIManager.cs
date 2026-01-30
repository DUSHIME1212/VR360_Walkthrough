using UnityEngine;
using VRCampusTour.Utils;
using VRCampusTour.UI;

namespace VRCampusTour.UI
{
    /// <summary>
    /// Manages all UI panels and displays
    /// </summary>
    public class UIManager : Singleton<UIManager>
    {
        [Header("UI Panels")]
        [SerializeField] private InfoPanel infoPanel;
        [SerializeField] private NavigationMenu navigationMenu;
        [SerializeField] private GameObject pauseMenu;

        [Header("HUD Elements")]
        [SerializeField] private UnityEngine.UI.Text locationNameText;
        [SerializeField] private UnityEngine.UI.Text progressText;

        protected override void Awake()
        {
            base.Awake();
            InitializePanels();
        }

        void Start()
        {
            SubscribeToEvents();
        }

        void OnDestroy()
        {
            UnsubscribeFromEvents();
        }

        void InitializePanels()
        {
            if (infoPanel != null)
            {
                infoPanel.Hide();
            }

            if (navigationMenu != null)
            {
                navigationMenu.Hide();
            }

            if (pauseMenu != null)
            {
                pauseMenu.SetActive(false);
            }
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

        void HandleLocationChanged(Location.LocationData newLocation)
        {
            UpdateLocationDisplay(newLocation);
            UpdateProgress();
        }

        public void ShowInfoPanel(string title, string content)
        {
            if (infoPanel != null)
            {
                infoPanel.Show(title, content);
            }
        }

        public void HideInfoPanel()
        {
            if (infoPanel != null)
            {
                infoPanel.Hide();
            }
        }

        public void ShowNavigationMenu()
        {
            if (navigationMenu != null)
            {
                navigationMenu.Show();
            }
        }

        public void HideNavigationMenu()
        {
            if (navigationMenu != null)
            {
                navigationMenu.Hide();
            }
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

        void UpdateLocationDisplay(Location.LocationData location)
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