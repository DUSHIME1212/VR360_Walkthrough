using System;
using System.Collections.Generic;
using UnityEngine;

namespace VRCampusTour.Location
{
    [CreateAssetMenu(fileName = "LocationData", menuName = "Campus Walkthrough/Location Data")]
    [Serializable]
    public class LocationData
    {
        public string locationName;
        public string description;
        public Material skyboxMaterial;
        public AudioClip ambienceAudio;
        public List<HotspotData> infoHotspots;
        public List<NavigationData> navigationPoints;
    }

    [Serializable]
    public class HotspotData
    {
        public string title;
        [TextArea(3, 6)]
        public string information;
        public Vector3 worldPosition;
        public Sprite icon;
    }

    [Serializable]
    public class NavigationData
    {
        public string destinationName;
        public int destinationLocationIndex;
        public Vector3 worldPosition;
        public Sprite previewImage;
    }

    [CreateAssetMenu(fileName = "NewLocation", menuName = "VR Campus Tour/Location Data")]
    public class LocationDataSO : ScriptableObject
    {
        public List<LocationData> locations = new List<LocationData>();
    }
}
// using System;
// using System.Collections.Generic;
// using UnityEngine;

// [Serializable]
// public class LocationData
// {
//     public string locationName;
//     public string description;
//     public Material skyboxMaterial;
//     public AudioClip ambienceAudio;
//     public List<HotspotData> infoHotspots;
//     public List<NavigationData> navigationPoints;
// }

// [Serializable]
// public class HotspotData
// {
//     public string title;
//     [TextArea(3, 6)]
//     public string information;
//     public Vector3 worldPosition;
//     public Sprite icon;
// }

// [Serializable]
// public class NavigationData
// {
//     public string destinationName;
//     public int destinationLocationIndex;
//     public Vector3 worldPosition;
//     public Sprite previewImage;
// }

// [CreateAssetMenu(fileName = "NewLocation", menuName = "VR Campus Tour/Location Data")]
// public class LocationDataSO : ScriptableObject
// {
//     public List<LocationData> locations = new List<LocationData>();
// }