using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem;

namespace VRCampusTour.VR
{
    /// <summary>
    /// Fallback input handler for desktop (Mouse & Keyboard)
    /// Allows rotating the camera using mouse or arrow keys
    /// </summary>
    public class DesktopInputHandler : MonoBehaviour
    {
        [Header("Rotation Settings")]
        [SerializeField] private float mouseSensitivity = 0.1f;
        [SerializeField] private float keyboardSensitivity = 80f;
        [SerializeField] private float verticalLimit = 85f;

        [Header("Status")]
        [SerializeField] private bool disableInVR = true;

        private float rotationX = 0f;
        private float rotationY = 0f;

        void Start()
        {
            // Initialize rotation with current camera rotation
            Vector3 currentRotation = transform.localEulerAngles;
            rotationY = currentRotation.y;
            rotationX = currentRotation.x;
            
            // Fix for Euler angles jumping around 0/360
            if (rotationX > 180) rotationX -= 360f;
        }

        void Update()
        {
            if (disableInVR && IsVRActive()) return;

            HandleMouseRotation();
            HandleKeyboardRotation();

            ApplyRotation();
        }

        bool IsVRActive()
        {
            return XRSettings.isDeviceActive;
        }

        void HandleMouseRotation()
        {
            if (Mouse.current == null) return;

            // Check if left or right button is held
            if (Mouse.current.leftButton.isPressed || Mouse.current.rightButton.isPressed)
            {
                Vector2 delta = Mouse.current.delta.ReadValue();
                float mouseX = delta.x * mouseSensitivity;
                float mouseY = delta.y * mouseSensitivity;

                rotationY += mouseX;
                rotationX -= mouseY;
                rotationX = Mathf.Clamp(rotationX, -verticalLimit, verticalLimit);
            }
        }

        void HandleKeyboardRotation()
        {
            if (Keyboard.current == null) return;

            float horizontal = 0;
            float vertical = 0;

            if (Keyboard.current.leftArrowKey.isPressed) horizontal = -1;
            else if (Keyboard.current.rightArrowKey.isPressed) horizontal = 1;

            if (Keyboard.current.upArrowKey.isPressed) vertical = 1;
            else if (Keyboard.current.downArrowKey.isPressed) vertical = -1;

            if (horizontal != 0 || vertical != 0)
            {
                rotationY += horizontal * keyboardSensitivity * Time.deltaTime;
                rotationX -= vertical * keyboardSensitivity * Time.deltaTime;
                rotationX = Mathf.Clamp(rotationX, -verticalLimit, verticalLimit);
            }
        }

        void ApplyRotation()
        {
            transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0f);
        }
    }
}
