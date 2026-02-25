using UnityEngine;
using UnityEngine.UI;
using VRCampusTour.Location;
using VRCampusTour.UI;
using VRCampusTour.Core;

namespace VRCampusTour.Interaction
{
    /// <summary>
    /// Hotspot that displays information when interacted with
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class InfoHotspot : MonoBehaviour, IInteractable
    {
        [Header("References")]
        [SerializeField] private Image iconImage;
        [SerializeField] private GameObject visualContainer;

        [Header("Visual Settings")]
        [SerializeField] private float hoverScale = 1.2f;
        [SerializeField] private float animationSpeed = 5f;
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color hoverColor = Color.cyan;

        [Header("Audio")]
        [SerializeField] private AudioClip interactSound;

        // Data
        private HotspotData hotspotData;
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

        public void Initialize(HotspotData data)
        {
            hotspotData = data;

            // Set icon if available
            if (iconImage != null && data.icon != null)
            {
                iconImage.sprite = data.icon;
            }

            // Set tag and layer for raycasting
            gameObject.tag = Utils.Constants.TAG_INFO;
            gameObject.layer = LayerMask.NameToLayer(Utils.Constants.LAYER_HOTSPOT);

            // Also set children layer to ensure raycast hits them if they have colliders
            foreach (Transform child in transform)
            {
                child.gameObject.layer = gameObject.layer;
            }

            Debug.Log($"[InfoHotspot] Initialized: {data.title} on layer: {gameObject.layer}");
        }

        public void OnInteract()
        {
            if (!isInteractable || hotspotData == null) return;

            // Play sound
            if (interactSound != null && AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(interactSound);
            }

            // Show info panel
            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowInfoPanel(hotspotData.title, hotspotData.information);
            }

            Debug.Log($"[InfoHotspot] Interacted: {hotspotData.title}");
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

            // Animate scale
            Vector3 targetScale = isHovered ? originalScale * hoverScale : originalScale;
            visualContainer.transform.localScale = Vector3.Lerp(
                visualContainer.transform.localScale,
                targetScale,
                Time.deltaTime * animationSpeed
            );

            // Animate color
            if (iconImage != null)
            {
                Color targetColor = isHovered ? hoverColor : normalColor;
                iconImage.color = Color.Lerp(iconImage.color, targetColor, Time.deltaTime * animationSpeed);
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