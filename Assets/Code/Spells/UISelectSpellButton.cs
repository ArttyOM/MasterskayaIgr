using System;
using Code.HUD;
using Code.HUD.Effects;
using Code.Projectiles;
using Code.Upgrades;
using DG.Tweening;
using UniRx;
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
        private SpellConfig _config;
        [SerializeField] private Image _icon;
        
        [SerializeField] private Image _cooldown;
        [SerializeField] private Effect _cooldownEnded;
        [SerializeField] private Effect _clickEffect;
        
        private UpgradeService _upgradeService;
        private IDisposable _onExplosion;


        public SpellType GetSpellType => _spellType;

        public void Init(SpellDefinition definition, IObserver<SpellType> onClick, Subject<ExplosionData> onExplosion, SpellConfig config, UpgradeService upgradeService)
        {
            _upgradeService = upgradeService;
            _onExplosion = onExplosion.Subscribe(StartCooldown);
            _spellType = definition.GetSpellType();
            _config = config;
            _onClick = onClick;
            _thisButton = GetComponent<Button>();
            _icon.sprite = definition.GetIcon();
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
            
        }

        private void StartCooldown(ExplosionData explosionData)
        {
            var spellType = explosionData.GetSpellType;
            if (spellType != _spellType) return;
            bool isMega = (_config.megaCastWeaponType == explosionData.GetProjectileType) &&
                          (spellType != SpellType.NoSpell);
            var cooldown = _upgradeService.GetUpgradedValue(UpgradeTarget.SpellCooldown, isMega ? _config.megaSpellBalance.cooldown : _config.commonSpellBalance.cooldown);
            _cooldown.fillAmount = 1;
            _thisButton.interactable = false;
            _cooldown.DOFillAmount(0, cooldown).OnComplete(CooldownEnded);
        }

        private void CooldownEnded()
        {
            _cooldownEnded.Play();
            _thisButton.interactable = true;
        }

        private void OnDestroy()
        {
            _onExplosion.Dispose();
        }
    }
    
}