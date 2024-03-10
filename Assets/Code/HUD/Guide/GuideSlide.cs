using UnityEngine;

namespace Code.HUD.Guide
{
    public class GuideSlide : MonoBehaviour
    {
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }
    }
}