
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using VRCampusTour.Utils;

namespace VRCampusTour.UI
{
    /// <summary>
    /// Handles screen fade transitions
    /// </summary>
    public class FadeController : Singleton<FadeController>
    {
        [Header("Settings")]
        [SerializeField] private Image fadeImage;
        [SerializeField] private float fadeDuration = 1.5f;
        [SerializeField] private Color fadeColor = Color.black;
        [SerializeField] private AnimationCurve fadeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        private bool isFading = false;
        private Coroutine currentFade;

        protected override void Awake()
        {
            base.Awake();
            InitializeFadeImage();
        }

        void Start()
        {
            // Start with clear screen
            if (fadeImage != null)
            {
                SetAlpha(0f);
            }
        }

        void InitializeFadeImage()
        {
            if (fadeImage == null)
            {
                // Create fade image if not assigned
                GameObject fadeObj = new GameObject("FadeImage");
                fadeObj.transform.SetParent(transform);

                Canvas canvas = fadeObj.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.sortingOrder = 9999;

                CanvasScaler scaler = fadeObj.AddComponent<CanvasScaler>();
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.referenceResolution = new Vector2(1920, 1080);

                GameObject imageObj = new GameObject("Image");
                imageObj.transform.SetParent(fadeObj.transform);

                fadeImage = imageObj.AddComponent<Image>();
                fadeImage.color = fadeColor;
                fadeImage.raycastTarget = false;

                RectTransform rt = fadeImage.GetComponent<RectTransform>();
                rt.anchorMin = Vector2.zero;
                rt.anchorMax = Vector2.one;
                rt.sizeDelta = Vector2.zero;
            }
            else
            {
                fadeImage.color = fadeColor;
            }
        }

        public Coroutine FadeOut()
        {
            return FadeOut(fadeDuration);
        }

        public Coroutine FadeOut(float duration)
        {
            if (currentFade != null)
            {
                StopCoroutine(currentFade);
            }

            currentFade = StartCoroutine(FadeCoroutine(0f, 1f, duration));
            return currentFade;
        }

        public Coroutine FadeIn()
        {
            return FadeIn(fadeDuration);
        }

        public Coroutine FadeIn(float duration)
        {
            if (currentFade != null)
            {
                StopCoroutine(currentFade);
            }

            currentFade = StartCoroutine(FadeCoroutine(1f, 0f, duration));
            return currentFade;
        }

        IEnumerator FadeCoroutine(float startAlpha, float endAlpha, float duration)
        {
            isFading = true;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = fadeCurve.Evaluate(elapsed / duration);
                float alpha = Mathf.Lerp(startAlpha, endAlpha, t);
                SetAlpha(alpha);
                yield return null;
            }

            SetAlpha(endAlpha);
            isFading = false;
        }

        void SetAlpha(float alpha)
        {
            if (fadeImage != null)
            {
                Color color = fadeImage.color;
                color.a = alpha;
                fadeImage.color = color;
            }
        }

        public bool IsFading() => isFading;
    }
}