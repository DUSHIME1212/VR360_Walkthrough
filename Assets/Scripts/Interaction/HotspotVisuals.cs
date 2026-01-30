using UnityEngine;

namespace VRCampusTour.Interaction
{
    /// <summary>
    /// Handles visual effects for hotspots (particles, glow, etc.)
    /// </summary>
    public class HotspotVisuals : MonoBehaviour
    {
        [Header("Glow Effect")]
        [SerializeField] private Light glowLight;
        [SerializeField] private float glowIntensity = 2f;
        [SerializeField] private Color glowColor = Color.cyan;

        [Header("Particle Effect")]
        [SerializeField] private ParticleSystem particles;

        [Header("Animation")]
        [SerializeField] private bool rotateConstantly = true;
        [SerializeField] private float rotationSpeed = 30f;
        [SerializeField] private Vector3 rotationAxis = Vector3.up;

        void Start()
        {
            SetupGlow();
            SetupParticles();
        }

        void Update()
        {
            if (rotateConstantly)
            {
                transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime, Space.Self);
            }
        }

        void SetupGlow()
        {
            if (glowLight == null) return;

            glowLight.color = glowColor;
            glowLight.intensity = glowIntensity;
            glowLight.range = 5f;
        }

        void SetupParticles()
        {
            if (particles == null) return;

            var main = particles.main;
            main.startColor = glowColor;
        }

        public void SetGlowActive(bool active)
        {
            if (glowLight != null)
            {
                glowLight.enabled = active;
            }
        }

        public void PlayParticles()
        {
            if (particles != null)
            {
                particles.Play();
            }
        }

        public void StopParticles()
        {
            if (particles != null)
            {
                particles.Stop();
            }
        }
    }
}