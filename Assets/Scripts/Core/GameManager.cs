using UnityEngine;
using VRCampusTour.Utils;
using VRCampusTour.Location;
using System.Collections.Generic;

namespace VRCampusTour.Core
{
    /// <summary>
    /// Main game controller - manages overall game state and progression
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {
        [Header("Game Configuration")]
        [SerializeField] private LocationDataSO locationDatabase;
        [SerializeField] private bool debugMode = false;

        [Header("Starting Configuration")]
        [SerializeField] private int startingLocationIndex = 0;

        // Game State
        private int currentLocationIndex = 0;
        private List<int> visitedLocations = new List<int>();
        private float sessionStartTime;

        // Properties
        public LocationData CurrentLocation => GetLocation(currentLocationIndex);
        public int TotalLocations => locationDatabase?.locations.Count ?? 0;
        public bool IsFirstLocation => currentLocationIndex == 0;
        public bool IsLastLocation => currentLocationIndex == TotalLocations - 1;
        public float SessionDuration => Time.time - sessionStartTime;

        // Events
        public System.Action<LocationData> OnLocationChanged;
        public System.Action OnTourCompleted;

        protected override void Awake()
        {
            base.Awake();
            ValidateConfiguration();
        }

        void Start()
        {
            InitializeGame();
        }

        void InitializeGame()
        {
            sessionStartTime = Time.time;
            currentLocationIndex = startingLocationIndex;
            visitedLocations.Clear();
            
            if (debugMode)
            {
                Debug.Log($"[GameManager] Game initialized. Total locations: {TotalLocations}");
            }
        }

        public void NavigateToLocation(int locationIndex)
        {
            if (locationIndex < 0 || locationIndex >= TotalLocations)
            {
                Debug.LogError($"[GameManager] Invalid location index: {locationIndex}");
                return;
            }

            // Track visited location
            if (!visitedLocations.Contains(currentLocationIndex))
            {
                visitedLocations.Add(currentLocationIndex);
            }

            currentLocationIndex = locationIndex;
            LocationData newLocation = GetLocation(locationIndex);

            if (debugMode)
            {
                Debug.Log($"[GameManager] Navigating to: {newLocation.locationName}");
            }

            OnLocationChanged?.Invoke(newLocation);

            // Check if tour is complete
            if (visitedLocations.Count >= TotalLocations)
            {
                OnTourCompleted?.Invoke();
            }
        }

        public void NavigateToNextLocation()
        {
            int nextIndex = currentLocationIndex + 1;
            if (nextIndex < TotalLocations)
            {
                NavigateToLocation(nextIndex);
            }
            else
            {
                Debug.LogWarning("[GameManager] Already at last location");
            }
        }

        public void NavigateToPreviousLocation()
        {
            int prevIndex = currentLocationIndex - 1;
            if (prevIndex >= 0)
            {
                NavigateToLocation(prevIndex);
            }
            else
            {
                Debug.LogWarning("[GameManager] Already at first location");
            }
        }

        public LocationData GetLocation(int index)
        {
            if (locationDatabase == null || index < 0 || index >= locationDatabase.locations.Count)
            {
                Debug.LogError($"[GameManager] Cannot get location at index {index}");
                return null;
            }

            return locationDatabase.locations[index];
        }

        public bool HasVisitedLocation(int index)
        {
            return visitedLocations.Contains(index);
        }

        public float GetTourProgress()
        {
            return visitedLocations.Count / (float)TotalLocations;
        }

        void ValidateConfiguration()
        {
            if (locationDatabase == null)
            {
                Debug.LogError("[GameManager] LocationDatabase is not assigned!");
            }

            if (locationDatabase != null && locationDatabase.locations.Count == 0)
            {
                Debug.LogError("[GameManager] LocationDatabase has no locations!");
            }
        }

        // Debug methods
        [ContextMenu("Log Current State")]
        void LogCurrentState()
        {
            Debug.Log($"=== Game Manager State ===");
            Debug.Log($"Current Location: {CurrentLocation?.locationName ?? "None"}");
            Debug.Log($"Location Index: {currentLocationIndex}/{TotalLocations}");
            Debug.Log($"Visited Locations: {visitedLocations.Count}");
            Debug.Log($"Tour Progress: {GetTourProgress() * 100}%");
            Debug.Log($"Session Duration: {SessionDuration}s");
        }
    }
}