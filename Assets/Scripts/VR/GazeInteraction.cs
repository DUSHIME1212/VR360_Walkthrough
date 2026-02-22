// Scripts/VR/GazeInteraction.cs
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using VRCampusTour.Utils;
using VRCampusTour.Interaction;

namespace VRCampusTour.VR
{
    /// <summary>
    /// Handles gaze-based interaction for VR
    /// Uses raycasting from camera to detect interactable objects
    /// </summary>
    public class GazeInteraction : MonoBehaviour
    {
        [Header("Gaze Settings")]
        [SerializeField] private float gazeDistance = 100f;
        [SerializeField] private float gazeDuration = 2f;
        [SerializeField] private LayerMask interactableLayer;

        [Header("Visual Feedback")]
        [SerializeField] private Image gazeReticle;
        [SerializeField] private Image gazeProgress;
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color hoverColor = Color.cyan;
        [SerializeField] private Color activeColor = Color.green;

        [Header("Audio")]
        [SerializeField] private AudioClip hoverSound;
        [SerializeField] private AudioClip selectSound;

        // State
        private IInteractable currentTarget;
        private IInteractable previousTarget;
        private float currentGazeTime = 0f;
        private bool isGazing = false;

        // Components
        private Camera mainCamera;

        void Start()
        {
            mainCamera = Camera.main;
            InitializeReticle();
        }

        void Update()
        {
            // Keep trying to find the camera if it wasn't available at Start
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
                if (mainCamera == null) return;
            }

            PerformGazeRaycast();
            UpdateGazeProgress();
            UpdateReticleVisuals();
        }

        void InitializeReticle()
        {
            if (gazeReticle != null)
            {
                gazeReticle.color = normalColor;
            }

            if (gazeProgress != null)
            {
                gazeProgress.fillAmount = 0f;
                gazeProgress.color = activeColor;
            }
        }

        void PerformGazeRaycast()
        {
            Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, gazeDistance, interactableLayer))
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();

                if (interactable != null && interactable.IsInteractable())
                {
                    if (currentTarget != interactable)
                    {
                        // New target
                        OnTargetChanged(interactable);
                    }

                    currentTarget = interactable;
                    isGazing = true;
                    currentGazeTime += Time.deltaTime;

                    // Check if gaze duration met OR mouse click
                    bool mouseClicked = Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;
                    if (currentGazeTime >= gazeDuration || mouseClicked)
                    {
                        OnGazeComplete();
                    }

                    return;
                }
            }

            // No valid target
            if (currentTarget != null)
            {
                OnTargetLost();
            }

            isGazing = false;
            currentTarget = null;
        }

        void UpdateGazeProgress()
        {
            if (gazeProgress == null) return;

            if (isGazing && currentTarget != null)
            {
                gazeProgress.fillAmount = Mathf.Clamp01(currentGazeTime / gazeDuration);
            }
            else
            {
                gazeProgress.fillAmount = Mathf.Lerp(gazeProgress.fillAmount, 0f, Time.deltaTime * 5f);
            }
        }

        void UpdateReticleVisuals()
        {
            if (gazeReticle == null) return;

            Color targetColor = normalColor;

            if (isGazing && currentTarget != null)
            {
                targetColor = currentGazeTime >= gazeDuration ? activeColor : hoverColor;
            }

            gazeReticle.color = Color.Lerp(gazeReticle.color, targetColor, Time.deltaTime * 10f);
        }

        void OnTargetChanged(IInteractable newTarget)
        {
            // Exit previous target
            if (previousTarget != null)
            {
                previousTarget.OnGazeExit();
            }

            // Enter new target
            newTarget.OnGazeEnter();
            
            // Play hover sound
            if (hoverSound != null && Core.AudioManager.Instance != null)
            {
                Core.AudioManager.Instance.PlaySFX(hoverSound, 0.5f);
            }

            previousTarget = newTarget;
            currentGazeTime = 0f;
        }

        void OnTargetLost()
        {
            if (currentTarget != null)
            {
                currentTarget.OnGazeExit();
            }

            previousTarget = currentTarget;
            currentGazeTime = 0f;
        }

        void OnGazeComplete()
        {
            if (currentTarget != null)
            {
                currentTarget.OnInteract();
                
                // Play select sound
                if (selectSound != null && Core.AudioManager.Instance != null)
                {
                    Core.AudioManager.Instance.PlaySFX(selectSound);
                }
            }

            // Reset
            currentGazeTime = 0f;
            OnTargetLost();
        }

        void OnDrawGizmos()
        {
            if (mainCamera == null) return;

            Gizmos.color = isGazing ? Color.green : Color.red;
            Gizmos.DrawRay(mainCamera.transform.position, mainCamera.transform.forward * gazeDistance);
        }
    }
}