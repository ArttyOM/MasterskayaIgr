using System;
using System.Collections.Generic;
using Code.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.HUD.Offers
{
    public class OfferView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _title;
        
        [SerializeField] private ProductView _productPrefab;
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private Button _buyButton;
        [SerializeField] private Transform _productListRoot;
        private Dictionary<OfferProduct, ProductView> _views = new Dictionary<OfferProduct, ProductView>();
        
        
        public void Render(Offer offer)
        {
            _title.text = offer.GetTitle();
            _buyButton.onClick.RemoveAllListeners();
            _buyButton.onClick.AddListener(BuyItem);
            _priceText.text = offer.GetTotalCost();
            
            //@todo: Cache existing offer views
            RemoveAllViews();
            CreateAllViews(offer);
        }

        private void CreateAllViews(Offer offer)
        {
            foreach (var product in offer.Products)
            {
                var productView = GameObject.Instantiate(_productPrefab, _productListRoot);
                productView.Render(product);
                _views.Add(product, productView);
            }
        }

        private void OnDestroy()
        {
            RemoveAllViews();
            _buyButton.onClick.RemoveAllListeners();
        }

        private void RemoveAllViews()
        {
            foreach (var view in _views)
            {
                Destroy(view.Value.gameObject);
            }

            _views.Clear();
        }

        private void BuyItem()
        {
            Debug.LogError("Can not Buy Offers in this build");
        }
    }
}