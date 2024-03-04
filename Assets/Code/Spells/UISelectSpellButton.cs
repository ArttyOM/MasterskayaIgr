using System;
using Code.HUD.Effects;
using Code.PregameShop;
using Code.Upgrades;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Code.Spells
{
    [RequireComponent(typeof(Button), typeof(EventTrigger))]
    public class UISelectSpellButton : MonoBehaviour
    {
        [SerializeField] private SpellType _spellType;

        private Button _thisButton;
        private IObserver<SpellType> _onClick;
        private SpellBalanceConfig _config;
        [SerializeField] private Image _icon;
        
        [SerializeField] private Image _cooldown;
        [SerializeField] private Effect _cooldownEnded;
        [SerializeField] private Effect _clickEffect;
        
        [SerializeField] private SpellShop _spellIcons;
        private UpgradeService _upgradeService;


        public SpellType GetSpellType => _spellType;

        public void Init(SpellType spellType, IObserver<SpellType> onClick, SpellBalanceConfig config, UpgradeService upgradeService)
        {
            _upgradeService = upgradeService;
            _spellType = spellType;
            _config = config;
            _onClick = onClick;
            _thisButton = GetComponent<Button>();
            _icon.sprite = _spellIcons.GetSprite(GetSpellType);
            _thisButton.onClick.RemoveAllListeners();
            _thisButton.onClick.AddListener(SendOnNext);
            _cooldown.fillAmount = 0;
            _cooldown.DOKill(false);
            _thisButton.interactable = true;
        }

        public void SetButtonInteractable(bool isInteractable)
        {
            _thisButton.interactable = isInteractable;
        }

        private void SendOnNext()
        {
            _clickEffect.Play();
            _onClick.OnNext(_spellType);
            var cooldown = _upgradeService.GetUpgradedValue(UpgradeTarget.SpellCooldown, _config.cooldown);
            _cooldown.fillAmount = 1;
            _thisButton.interactable = false;
            _cooldown.DOFillAmount(0, cooldown).OnComplete(CooldownEnded);
        }

        private void CooldownEnded()
        {
            _cooldownEnded.Play();
            _thisButton.interactable = true;
        }
    }
    
}