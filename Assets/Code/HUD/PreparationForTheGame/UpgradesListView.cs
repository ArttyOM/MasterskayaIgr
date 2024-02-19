using System;
using System.Collections.Generic;
using Code.PregameShop;
using UnityEngine;

namespace Code.HUD
{
    public class UpgradesListView : MonoBehaviour
    {
        [SerializeField] private UpgradeView _prefab;
        [SerializeField] private Transform _contentRoot;

        public event Action UpgradeBought;
        private readonly List<UpgradeView> _views = new();
        
        public void Render(ShopSystem shop)
        {
            var offers = shop.GetUpgradeOffers();
            RemoveAllItems();
            foreach (var offer in offers)
            {
                var offerView = Instantiate(_prefab, _contentRoot);
                offerView.Render(offer, shop);
                offerView.UpgradeBought += OnUpgradeBought;
                _views.Add(offerView);
            }
        }

        private void OnUpgradeBought(UpgradeView view) => UpgradeBought?.Invoke();

        private void RemoveAllItems()
        {
            foreach (var item in _views)
            {
                item.UpgradeBought -= OnUpgradeBought;
                Destroy(item.gameObject);
            }
            _views.Clear();
        }


        private void OnDestroy()
        {
            RemoveAllItems();
        }
    }
}