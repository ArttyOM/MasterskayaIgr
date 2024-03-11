using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.HUD.Gameplay
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image _fill;
        [SerializeField] private TMP_Text _text;


        public void Render(float current, float max)
        {
            _text.text = $"{Mathf.CeilToInt(current)}\\{Mathf.CeilToInt(max)}";
            _fill.fillAmount = (current / Mathf.Max(max, 1));
        }
        
    }
}