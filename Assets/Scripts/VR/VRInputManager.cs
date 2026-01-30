using UnityEngine;
using UnityEngine.XR;
using VRCampusTour.Utils;

namespace VRCampusTour.VR
{
    /// <summary>
    /// Handles VR input from controllers and HMD
    /// </summary>
    public class VRInputManager : Singleton<VRInputManager>
    {
        [Header("Input Settings")]
        [SerializeField] private XRNode dominantHand = XRNode.RightHand;
        [SerializeField] private float triggerThreshold = 0.7f;

        // Input states
        private bool triggerPressed = false;
        private bool triggerDown = false;
        private bool triggerUp = false;

        // Events
        public System.Action OnTriggerPressed;
        public System.Action OnTriggerReleased;
        public System.Action OnTriggerHeld;

        void Update()
        {
            UpdateInputStates();
            ProcessInput();
        }

        void UpdateInputStates()
        {
            InputDevice device = InputDevices.GetDeviceAtXRNode(dominantHand);
            
            if (device.isValid)
            {
                // Get trigger value
                device.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue);
                
                bool currentTriggerPressed = triggerValue >= triggerThreshold;
                
                // Detect state changes
                triggerDown = currentTriggerPressed && !triggerPressed;
                triggerUp = !currentTriggerPressed && triggerPressed;
                
                triggerPressed = currentTriggerPressed;
            }
        }

        void ProcessInput()
        {
            if (triggerDown)
            {
                OnTriggerPressed?.Invoke();
            }
            else if (triggerUp)
            {
                OnTriggerReleased?.Invoke();
            }
            else if (triggerPressed)
            {
                OnTriggerHeld?.Invoke();
            }
        }

        public bool IsTriggerPressed() => triggerPressed;
        public bool IsTriggerDown() => triggerDown;
        public bool IsTriggerUp() => triggerUp;

        public Vector2 GetPrimaryAxis()
        {
            InputDevice device = InputDevices.GetDeviceAtXRNode(dominantHand);
            
            if (device.isValid)
            {
                device.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 axis);
                return axis;
            }
            
            return Vector2.zero;
        }

        public bool GetPrimaryButtonDown()
        {
            InputDevice device = InputDevices.GetDeviceAtXRNode(dominantHand);
            
            if (device.isValid)
            {
                device.TryGetFeatureValue(CommonUsages.primaryButton, out bool pressed);
                return pressed;
            }
            
            return false;
        }
    }
}