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
        
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _title;
        [SerializeField] private Button _buyButton;
        [SerializeField] private Button _addToSpellBook;
        
        public void Render(SpellType spell, int cost, Sprite icon, ShopSystem shopSystem)
        {
            _price.text = cost.ToString();
            _icon.sprite = icon;
            _title.text = spell.ToString();
            _buyButton.onClick.RemoveAllListeners();
            _buyButton.onClick.AddListener(() =>
            {
                if (!shopSystem.CanBuy(spell)) return;
                shopSystem.Buy(spell);
                _buyButton.gameObject.SetActive(false);
                _buyButton.onClick.RemoveAllListeners();
            });
            _addToSpellBook.gameObject.SetActive(false);
        }

        public void Render(SpellType spell, Sprite icon, SpellBook spellBook)
        {
            _buyButton.gameObject.SetActive(false);
            _icon.sprite = icon;
            _title.text = spell.ToString();
            _addToSpellBook.interactable = spellBook.CanSelect(spell);
            _addToSpellBook.gameObject.SetActive(spellBook.IsUnlocked(spell) && !spellBook.IsSelected(spell));
            _addToSpellBook.onClick.RemoveAllListeners();
            _addToSpellBook.onClick.AddListener(() =>
            {
                spellBook.TrySelectInFirstEmpty(spell);
            });
        }
    }
}