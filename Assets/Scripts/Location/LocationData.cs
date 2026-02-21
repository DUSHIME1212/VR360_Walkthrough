using System;
using System.Collections.Generic;
using UnityEngine;

namespace VRCampusTour.Location
{
    [Serializable]
    public class LocationData
    {
        public string locationName;
        public string description;
        public Material skyboxMaterial;
        public AudioClip ambienceAudio;
        public List<HotspotData> infoHotspots = new List<HotspotData>();
        public List<NavigationData> navigationPoints = new List<NavigationData>();
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

}