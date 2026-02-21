using System.Collections.Generic;
using UnityEngine;

namespace VRCampusTour.Location
{
    [CreateAssetMenu(fileName = "NewLocation", menuName = "VR Campus Tour/Location Data")]
    public class LocationDataSO : ScriptableObject
    {
        public List<LocationData> locations = new List<LocationData>();

        [ContextMenu("Load Default Data")]
        public void LoadDefaultData()
        {
            Debug.Log("Loading default data...");
        }
    }
}

