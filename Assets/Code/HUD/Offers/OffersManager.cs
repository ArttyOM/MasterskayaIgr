using System.Collections.Generic;
using Code.Items;
using MyBox;
using UnityEngine;

namespace Code.HUD.Offers
{
    public class OffersManager : MonoBehaviour
    {
        [SerializeField] private List<Item> _items;
        
        public IEnumerable<Offer> GetRandomOffers()
        {
            yield return new Offer("Offer 1", new List<OfferProduct>()
            {
                new OfferProduct(_items.GetRandom(), UnityEngine.Random.Range(10, 1000), Random.Range(0, 3.99f)),
                new OfferProduct(_items.GetRandom(), UnityEngine.Random.Range(10, 1000), Random.Range(0, 3.99f)),
                new OfferProduct(_items.GetRandom(), UnityEngine.Random.Range(10, 1000), Random.Range(0, 3.99f))
            });
            
            yield return new Offer("Offer 2", new List<OfferProduct>()
            {
                new OfferProduct(_items.GetRandom(), UnityEngine.Random.Range(10, 1000), Random.Range(0, 3.99f)),
                new OfferProduct(_items.GetRandom(), UnityEngine.Random.Range(10, 1000), Random.Range(0, 3.99f)),
                new OfferProduct(_items.GetRandom(), UnityEngine.Random.Range(10, 1000), Random.Range(0, 3.99f))
            });
            yield return new Offer("Offer 3", new List<OfferProduct>()
            {
                new OfferProduct(_items.GetRandom(), UnityEngine.Random.Range(10, 1000), Random.Range(0, 3.99f)),
                new OfferProduct(_items.GetRandom(), UnityEngine.Random.Range(10, 1000), Random.Range(0, 3.99f)),
                new OfferProduct(_items.GetRandom(), UnityEngine.Random.Range(10, 1000), Random.Range(0, 3.99f))
            });
        }
    }
}