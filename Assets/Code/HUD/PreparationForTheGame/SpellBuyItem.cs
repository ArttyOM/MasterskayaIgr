using Code.PregameShop;
using Code.Spells;
using MyBox;
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
        [SerializeField] private TMP_Text _shortDescription;
        
        [SerializeField] private Button _buyButton;
        [SerializeField] private Button _addToSpellBook;
        [SerializeField] private SpellDefinitions _spellDefinitions;
        
        public void Render(SpellType spell, int cost, Sprite icon, ShopSystem shopSystem)
        {
            var definition = _spellDefinitions.Get(spell);
            _price.text = cost.ToString();
            _icon.sprite = icon;
            _title.text = definition.GetTitle();
            _shortDescription.text = definition.GetShortDescription();
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
            var definition = _spellDefinitions.Get(spell);
            _icon.sprite = icon;
            _title.text = definition.GetTitle();
            _shortDescription.text = definition.GetShortDescription();
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