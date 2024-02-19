using System;
using System.Collections.Generic;
using System.Linq;
using Code.Items;
using UnityEngine;

namespace Code.HUD.Offers
{
    public class OffersList : MonoBehaviour
    {
        [SerializeField] private OfferView _prefab;
        [SerializeField] private Transform _contentRoot;
        
        [SerializeField] private int _maxCount;
        private readonly List<OfferView> _views = new ();

        
        public void Render(IEnumerable<Offer> offers)
        {
            RemoveAllOffers();
            foreach (var offer in offers.Take(_maxCount))
            {
                var offerView = Instantiate(_prefab, _contentRoot);
                offerView.Render(offer);
                _views.Add(offerView);
            }
        }


        private void OnDestroy()
        {
            RemoveAllOffers();
        }

        private void RemoveAllOffers()
        {
            foreach (var view in _views)
            {
                Destroy(view.gameObject);
            }

            _views.Clear();
        }
    }
}