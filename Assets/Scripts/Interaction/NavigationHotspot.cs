using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRCampusTour.Location;
using VRCampusTour.Core;

namespace VRCampusTour.Interaction
{
    /// <summary>
    /// Hotspot that navigates to a different location when interacted with
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class NavigationHotspot : MonoBehaviour, IInteractable
    {
        [Header("References")]
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI labelText;
        [SerializeField] private GameObject visualContainer;

        [Header("Visual Settings")]
        [SerializeField] private float hoverScale = 1.2f;
        [SerializeField] private float pulseSpeed = 2f;
        [SerializeField] private float pulseAmount = 0.1f;
        [SerializeField] private Color normalColor = new Color(0.2f, 0.8f, 1f);
        [SerializeField] private Color hoverColor = Color.green;

        [Header("Audio")]
        [SerializeField] private AudioClip navigateSound;

        // Data
        private NavigationData navigationData;
        private bool isInteractable = true;
        private bool isHovered = false;
        private Vector3 originalScale;

        void Awake()
        {
            if (visualContainer != null)
            {
                originalScale = visualContainer.transform.localScale;
            }
        }

        void Update()
        {
            UpdateVisuals();
        }

        public void Initialize(NavigationData data)
        {
            navigationData = data;

            // Set label
            if (labelText != null)
            {
                labelText.text = data.destinationName;
            }

            // Set preview image if available
            if (iconImage != null && data.previewImage != null)
            {
                iconImage.sprite = data.previewImage;
            }

            // Set tag and layer for raycasting
            gameObject.tag = Utils.Constants.TAG_NAVIGATION;
            gameObject.layer = LayerMask.NameToLayer(Utils.Constants.LAYER_HOTSPOT);

            // Also set children layer to ensure raycast hits them if they have colliders
            foreach (Transform child in transform)
            {
                child.gameObject.layer = gameObject.layer;
            }

            Debug.Log($"[NavigationHotspot] Initialized: {data.destinationName} on layer: {gameObject.layer}");
        }

        public void OnInteract()
        {
            if (!isInteractable || navigationData == null) return;

            // Play sound
            if (navigateSound != null && AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(navigateSound);
            }

            // Navigate to location
            if (GameManager.Instance != null)
            {
                GameManager.Instance.NavigateToLocation(navigationData.destinationLocationIndex);
            }

            Debug.Log($"[NavigationHotspot] Navigating to: {navigationData.destinationName}");
        }

        public void OnGazeEnter()
        {
            isHovered = true;
        }

        public void OnGazeExit()
        {
            isHovered = false;
        }

        public bool IsInteractable()
        {
            return isInteractable;
        }

        void UpdateVisuals()
        {
            if (visualContainer == null) return;

            // Pulse animation when not hovered
            float pulse = isHovered ? 0f : Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
            Vector3 targetScale = isHovered ? 
                originalScale * hoverScale : 
                originalScale * (1f + pulse);

            visualContainer.transform.localScale = Vector3.Lerp(
                visualContainer.transform.localScale,
                targetScale,
                Time.deltaTime * 5f
            );

            // Animate color
            if (iconImage != null)
            {
                Color targetColor = isHovered ? hoverColor : normalColor;
                iconImage.color = Color.Lerp(iconImage.color, targetColor, Time.deltaTime * 5f);
            }

            // Rotate to face camera
            if (Camera.main != null)
            {
                transform.LookAt(Camera.main.transform);
                transform.Rotate(0, 180, 0);
            }
        }
    }
}