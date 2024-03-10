using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.HUD.Guide
{
    public class SpellGuideSlide : GuideSlide
    {
        [SerializeField] private TMP_Text _title;
        [SerializeField] private TMP_Text _fullDescription;
        [SerializeField] private Image _image;
        [SerializeField] private GameObject _targetsRoot;
        [SerializeField] private Image[] _targets;
        [SerializeField] private SpellDefinition _definition;


        private void OnValidate()
        {
            if(_definition != null)
                Render(_definition);
        }

        public void Render(SpellDefinition definition)
        {
            _title.text = definition.GetTitle();
            _fullDescription.text = definition.GetFullDescription();
            _image.sprite = definition.GetGuideArt();
            var targetItems = definition.GetTargetItems();
            _targetsRoot.SetActive(targetItems.Count > 0);
            for (int i = 0; i < _targets.Length; i++)
            {
                if (targetItems.Count <= i)
                {
                    _targets[i].gameObject.SetActive(false);
                }
                else
                {
                    _targets[i].gameObject.SetActive(true);
                    _targets[i].sprite = targetItems[i];
                }
            }
        }
    }
}