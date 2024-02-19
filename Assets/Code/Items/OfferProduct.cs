using UnityEngine;

namespace Code.Items
{
    public class OfferProduct
    {
        private readonly Item _item;
        private readonly int _count;
        private readonly float _price;

        public OfferProduct(Item item, int count, float price)
        {
            _item = item;
            _count = count;
            _price = price;
        }

        public Sprite GetIcon()
        {
            return _item.Icon;
        }

        public string GetCount() => _count.ToString("N0");

        public string GetPrice() => _price.ToString("C0");

        public float GetCost() => _price;
    }
}