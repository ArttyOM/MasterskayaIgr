using System.Collections.Generic;
using System.Linq;

namespace Code.Items
{
    public class Offer
    {
        private readonly List<OfferProduct> _products;
        private string _title;
        private float _totalCost;

        public Offer(string title, IEnumerable<OfferProduct> products)
        {
            _title = title;
            _products = products.ToList();
            _totalCost = 0;
            foreach (var product in _products)
            {
                _totalCost += product.GetCost();
            }
        }

        public IEnumerable<OfferProduct> Products => _products;
        public string GetTotalCost() => _totalCost.ToString("C");
        public string GetTitle() => _title;
    }
}