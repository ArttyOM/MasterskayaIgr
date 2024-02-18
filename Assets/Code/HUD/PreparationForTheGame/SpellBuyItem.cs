using Code.PregameShop;
using Code.Spells;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.HUD
{
    public class SpellBuyItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text _price;
        [SerializeField] private DraggableSpell _draggable;
        
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _title;
        [SerializeField] private Button _buyButton;
        public void Render(SpellType spell, int cost, Sprite icon, ShopSystem shopSystem)
        {
            _price.text = cost.ToString();
            _icon.sprite = icon;
            _title.text = spell.ToString();
            _draggable.Disable();
            _buyButton.onClick.RemoveAllListeners();
            _buyButton.onClick.AddListener(() =>
            {
                if (!shopSystem.CanBuy(spell)) return;
                shopSystem.Buy(spell);
                _draggable.SetSpell(spell);
                _draggable.Enable();
                _buyButton.gameObject.SetActive(false);
                _buyButton.onClick.RemoveAllListeners();
            });
        }
        
        public void Render(SpellType spell, Sprite icon)
        {
            _draggable.Enable();
            _draggable.SetSpell(spell);
            _buyButton.gameObject.SetActive(false);
            _icon.sprite = icon;
            _title.text = spell.ToString();
        }
    }
}