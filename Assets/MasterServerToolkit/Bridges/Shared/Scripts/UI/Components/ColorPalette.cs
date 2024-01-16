using MasterServerToolkit.Extensions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MasterServerToolkit.Bridges
{
    public class ColorPalette : MonoBehaviour
    {
        #region INSPECTOR

        [Header("Settings")] [SerializeField] public Color32[] colors = new Color32[]
        {
            new Color(1f, 0.8666667f, 0.7254902f),
            new Color(0.9058824f, 0.7372549f, 0.5686275f),
            new Color(0.7764706f, 0.5529412f, 0.3882353f),
            new Color(0.6431373f, 0.3764706f, 0.2078431f),
            new Color(0.3686275f, 0.2666667f, 0.2196078f),
            new(230, 90, 90, 255),
            new(240, 205, 89, 255),
            new(66, 191, 123, 255),
            new(106, 205, 185, 255),
            new(54, 142, 229, 255),
            new(176, 131, 214, 255),
            new(239, 128, 197, 255),
            Color.white,
            new(47, 47, 47, 255),
            new(125, 53, 53, 255),
            new(137, 89, 67, 255),
            new(198, 156, 108, 255),
            new(54, 118, 83, 255),
            new(58, 106, 108, 255),
            new(54, 69, 98, 255),
            new(87, 66, 107, 255),
            new(171, 171, 171, 255),
            Color.black
        };

        [Header("Components")] [SerializeField]
        private Toggle togglePrefab;

        [SerializeField] private RectTransform container;

        public UnityEvent<Color> OnColorChangeEvent;

        #endregion

        private List<Toggle> toggles = new();

        private void Awake()
        {
            DrawPalette();
        }

        private void DrawPalette()
        {
            togglePrefab.gameObject.SetActive(true);

            toggles.Clear();
            container.RemoveChildren(togglePrefab.transform);

            for (var i = 0; i < colors.Length; i++) CreateColorPicker(colors[i]);

            togglePrefab.gameObject.SetActive(false);
        }

        private void CreateColorPicker(Color newColor)
        {
            var toggle = Instantiate(togglePrefab, container, false);
            toggle.transform.Find("Background").GetComponent<Image>().color = newColor;

            var color = newColor;

            toggle.onValueChanged.AddListener((isOn) =>
            {
                if (isOn)
                    OnColorChangeEvent?.Invoke(color);
            });

            toggles.Add(toggle);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void SelectColor(string value)
        {
            value = !value.StartsWith("#") ? $"#{value}" : value;

            if (ColorUtility.TryParseHtmlString(value, out var color))
                SelectColor(color);
            else
                Debug.LogError($"{value} is invalid color value");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        public void SelectColor(Color color)
        {
            for (var i = 0; i < colors.Length; i++)
                if (colors[i] == color)
                {
                    toggles[i].isOn = true;
                    break;
                }
        }
    }
}