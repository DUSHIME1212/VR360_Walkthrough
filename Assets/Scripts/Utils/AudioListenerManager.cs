using UnityEngine;

namespace VRCampusTour.Utils
{
    /// <summary>
    /// Ensures only one AudioListener is enabled in the scene
    /// </summary>
    public class AudioListenerManager : MonoBehaviour
    {
        private void Awake()
        {
            EnforceSingleListener();
        }

        public void EnforceSingleListener()
        {
            AudioListener[] listeners = FindObjectsByType<AudioListener>(FindObjectsSortMode.None);
            
            if (listeners.Length > 1)
            {
                Debug.Log($"[AudioListenerManager] Found {listeners.Length} AudioListeners. Enforcing one.");
                
                // Keep the one on the Main Camera if possible, otherwise keep the first one
                Camera mainCam = Camera.main;
                bool keptOne = false;

                foreach (var listener in listeners)
                {
                    if (!keptOne && (mainCam == null || listener.gameObject == mainCam.gameObject))
                    {
                        listener.enabled = true;
                        keptOne = true;
                        Debug.Log($"[AudioListenerManager] Kept AudioListener on {listener.gameObject.name}");
                    }
                    else
                    {
                        listener.enabled = false;
                        Debug.Log($"[AudioListenerManager] Disabled extra AudioListener on {listener.gameObject.name}");
                    }
                }

                // If none were on main camera or we still need one
                if (!keptOne && listeners.Length > 0)
                {
                    listeners[0].enabled = true;
                }
            }
        }
    }
}
