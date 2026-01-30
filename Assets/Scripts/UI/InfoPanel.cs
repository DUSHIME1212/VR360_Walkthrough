using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace VRCampusTour.UI
{
    /// <summary>
    /// Manages the information panel display
    /// </summary>
    public class InfoPanel : MonoBehaviour
    {
        [Header("Panel Components")]
        [SerializeField] private GameObject panel;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI contentText;
        [SerializeField] private Button closeButton;

        void Start()
        {
            if (closeButton != null)
            {
                closeButton.onClick.AddListener(Hide);
            }
            
            Hide();
        }

        public void Show(string title, string content)
        {
            if (titleText != null)
            {
                titleText.text = title;
            }
            
            if (contentText != null)
            {
                contentText.text = content;
            }
            
            if (panel != null)
            {
                panel.SetActive(true);
            }
        }

        public void Hide()
        {
            if (panel != null)
            {
                panel.SetActive(false);
            }
        }

        void OnDestroy()
        {
            if (closeButton != null)
            {
                closeButton.onClick.RemoveListener(Hide);
            }
        }
    }
}