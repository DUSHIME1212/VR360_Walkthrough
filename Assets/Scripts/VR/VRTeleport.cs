using UnityEngine;
using System.Collections;

namespace VRCampusTour.VR
{
    /// <summary>
    /// Handles teleportation in VR (if needed for physical movement)
    /// </summary>
    public class VRTeleport : MonoBehaviour
    {
        [Header("Teleport Settings")]
        [SerializeField] private Transform playerRig;
        [SerializeField] private float teleportDuration = 0.5f;
        [SerializeField] private AnimationCurve teleportCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        [Header("Visual Effects")]
        [SerializeField] private GameObject teleportMarker;
        [SerializeField] private LineRenderer teleportLine;

        private bool isTeleporting = false;

        public void TeleportTo(Vector3 targetPosition)
        {
            if (isTeleporting) return;
            StartCoroutine(TeleportCoroutine(targetPosition));
        }

        IEnumerator TeleportCoroutine(Vector3 targetPosition)
        {
            isTeleporting = true;

            Vector3 startPosition = playerRig.position;
            float elapsed = 0f;

            while (elapsed < teleportDuration)
            {
                elapsed += Time.deltaTime;
                float t = teleportCurve.Evaluate(elapsed / teleportDuration);
                playerRig.position = Vector3.Lerp(startPosition, targetPosition, t);
                yield return null;
            }

            playerRig.position = targetPosition;
            isTeleporting = false;
        }
    }
}