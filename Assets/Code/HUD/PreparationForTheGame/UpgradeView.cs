using System;
using Code.PregameShop;
using Code.Upgrades;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.HUD
{
    public class UpgradeView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _title;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _price;
        public event Action<UpgradeView> UpgradeBought;

        public void Render(UpgradeDefinition definition, ShopSystem shopSystem)
        {
            if (definition == null)
            {
                gameObject.SetActive(false);
                return;
            }
            gameObject.SetActive(true);
            _icon.sprite = definition.GetIcon();
            _button.gameObject.SetActive(true);
            _title.text = definition.GetTitle();
            _description.text = definition.GetDescription();
            _price.text = definition.GetCost().ToString();
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(() =>
            {
                if (!shopSystem.CanBuy(definition)) return;
                _button.gameObject.SetActive(false);
                _button.onClick.RemoveAllListeners();
                shopSystem.Buy(definition);
                UpgradeBought?.Invoke(this);
            });
        }
    }
}