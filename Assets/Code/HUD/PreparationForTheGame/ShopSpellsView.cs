using System;
using System.Collections.Generic;
using Code.PregameShop;
using Code.Spells;
using UnityEngine;

namespace Code.HUD
{
    public class ShopSpellsView : MonoBehaviour
    {
        [SerializeField] private SpellBuyItem _itemPrefab;
        [SerializeField] private Transform _contentRoot;
        private List<SpellBuyItem> _items = new();
        
        public void Render(SpellBook spellBook, SpellDefinitions spellDefinitions, ShopSystem shopSystem)
        {
            RemoveAllItems();
            foreach (var spell in shopSystem.GetSpellOffers())
            {
                var definition = spellDefinitions.Get(spell);
                if (definition == null) continue;
                var spellItem = Instantiate(_itemPrefab, _contentRoot);
                _items.Add(spellItem);
                if (spellBook.IsUnlocked(spell))
                {
                    spellItem.Render(spell, definition.GetIcon(), spellBook);
                }
                else
                {
                    spellItem.Render(spell, definition.GetCost(), definition.GetIcon(), shopSystem);    
                }
            }
        }

        private void RemoveAllItems()
        {
            foreach (var item in _items)
            {
                Destroy(item.gameObject);
            }
            _items.Clear();
        }


        private void OnDestroy()
        {
            RemoveAllItems();
        }
    }
}