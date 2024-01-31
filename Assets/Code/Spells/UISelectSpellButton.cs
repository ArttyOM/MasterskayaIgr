using System;
using TMPro;
using UnityEditor.AddressableAssets.Build.Layout;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Code.Spells
{
    [RequireComponent(typeof(Button), typeof(EventTrigger))]
    public class UISelectSpellButton: MonoBehaviour
    {
        [SerializeField] private SpellType _spellType;
        
        private Button _thisButton;

        public SpellType GetSpellType => _spellType;
        
        public void Init(IObserver<SpellType> onClick)
        {
            _thisButton = GetComponent<Button>();

            foreach (var trigger in GetComponent<EventTrigger>().triggers)
            {
                trigger.callback.AddListener((data) =>
                {
                    onClick.OnNext(_spellType);
                });
            }
        }
    }
}