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
        private IObserver<SpellType> _onClick;

        public SpellType GetSpellType => _spellType;
        
        public void Init(IObserver<SpellType> onClick)
        {
            _onClick = onClick;
            _thisButton = GetComponent<Button>();

            _thisButton.onClick.AddListener(SendOnNext);
            // foreach (var trigger in GetComponent<EventTrigger>().triggers)
            // {
            //     trigger.callback.AddListener((data) =>
            //     {
            //         onClick.OnNext(_spellType);
            //     });
            // }
        }

        private void SendOnNext()
        {
            _onClick.OnNext(_spellType);
        }
    }
}