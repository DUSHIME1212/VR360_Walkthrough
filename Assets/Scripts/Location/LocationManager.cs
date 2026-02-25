using UnityEngine;
using System.Collections;
using VRCampusTour.Core;
using VRCampusTour.UI;
using VRCampusTour.Utils;
using VRCampusTour.Location;
using VRCampusTour.Interaction;

namespace VRCampusTour.Location
{
    /// <summary>
    /// Manages 360° location display and transitions
    /// </summary>
    public class LocationManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform hotspotContainer;
        [SerializeField] private GameObject infoHotspotPrefab;
        [SerializeField] private GameObject navigationHotspotPrefab;

        [Header("Hotspot Settings")]
        [SerializeField] private float eyeLevelHeight = 1.5f;

        [Header("Audio")]
        [SerializeField] private AudioClip transitionSound;

        private LocationData currentLocation;

        void Start()
        {
            SubscribeToEvents();
            LoadInitialLocation();
        }

        void OnDestroy()
        {
            UnsubscribeFromEvents();
        }

        void SubscribeToEvents()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnLocationChanged += HandleLocationChanged;
            }
        }

        void UnsubscribeFromEvents()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnLocationChanged -= HandleLocationChanged;
            }
        }

        void LoadInitialLocation()
        {
            if (GameManager.Instance != null)
            {
                LocationData initialLocation = GameManager.Instance.CurrentLocation;
                if (initialLocation != null)
                {
                    SetupLocation(initialLocation);
                }
            }
        }

        void HandleLocationChanged(LocationData newLocation)
        {
            StartCoroutine(TransitionToLocation(newLocation));
        }

        IEnumerator TransitionToLocation(LocationData newLocation)
        {
            // Play transition sound
            if (transitionSound != null && AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(transitionSound);
            }

            // Fade out
            if (FadeController.Instance != null)
            {
                yield return FadeController.Instance.FadeOut();
            }

            // Clear current hotspots
            ClearHotspots();

            // Wait a frame
            yield return null;

            // Setup new location
            SetupLocation(newLocation);

            // Fade in
            if (FadeController.Instance != null)
            {
                yield return FadeController.Instance.FadeIn();
            }
        }

        void SetupLocation(LocationData location)
        {
            if (location == null)
            {
                Debug.LogError("[LocationManager] Location data is null!");
                return;
            }

            currentLocation = location;

            // Set skybox
            if (location.skyboxMaterial != null)
            {
                RenderSettings.skybox = location.skyboxMaterial;
                DynamicGI.UpdateEnvironment();
            }

            // Play ambience
            if (location.ambienceAudio != null && AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayAmbience(location.ambienceAudio, true);
            }

            // Create info hotspots
            if (location.infoHotspots != null)
            {
                foreach (var hotspotData in location.infoHotspots)
                {
                    CreateInfoHotspot(hotspotData);
                }
            }

            // Create navigation hotspots
            if (location.navigationPoints != null)
            {
                foreach (var navData in location.navigationPoints)
                {
                    CreateNavigationHotspot(navData);
                }
            }

            Debug.Log($"[LocationManager] Loaded location: {location.locationName}");
        }

        void CreateInfoHotspot(HotspotData data)
        {
            if (infoHotspotPrefab == null || hotspotContainer == null) return;

            GameObject hotspot = Instantiate(infoHotspotPrefab, hotspotContainer);
            
            // Apply eye level if height is 0
            Vector3 spawnPos = data.worldPosition;
            if (Mathf.Approximately(spawnPos.y, 0)) spawnPos.y = eyeLevelHeight;
            hotspot.transform.position = spawnPos;

            // Look at camera
            if (Camera.main != null)
            {
                hotspot.transform.LookAt(Camera.main.transform);
                hotspot.transform.Rotate(0, 180, 0);
            }

            // Configure hotspot
            var infoHotspot = hotspot.GetComponent<InfoHotspot>();
            if (infoHotspot != null)
            {
                infoHotspot.Initialize(data);
            }
        }

        void CreateNavigationHotspot(NavigationData data)
        {
            if (navigationHotspotPrefab == null || hotspotContainer == null) return;

            GameObject hotspot = Instantiate(navigationHotspotPrefab, hotspotContainer);
            
            // Apply eye level if height is 0
            Vector3 spawnPos = data.worldPosition;
            if (Mathf.Approximately(spawnPos.y, 0)) spawnPos.y = eyeLevelHeight;
            hotspot.transform.position = spawnPos;

            // Look at camera
            if (Camera.main != null)
            {
                hotspot.transform.LookAt(Camera.main.transform);
                hotspot.transform.Rotate(0, 180, 0);
            }

            // Configure hotspot
            var navHotspot = hotspot.GetComponent<NavigationHotspot>();
            if (navHotspot != null)
            {
                navHotspot.Initialize(data);
            }
        }

        void ClearHotspots()
        {
            if (hotspotContainer == null) return;

            foreach (Transform child in hotspotContainer)
            {
                Destroy(child.gameObject);
            }
        }
    }
}