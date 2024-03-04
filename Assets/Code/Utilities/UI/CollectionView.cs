using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FMG
{
    public abstract class CollectionView<T> : MonoBehaviour
    {
        [SerializeField] private View<T> _view;
        [SerializeField] private Transform _root;
        
        [SerializeField] private CollectionFilter<T> _filter;
        [SerializeField] private CollectionOrder<T> _order;
        [SerializeField] private int _limit;
        [SerializeField] private int _skip;
        
        
        
        private readonly List<View<T>> _views = new();
        
        public void Render(IEnumerable<T> collection)
        {
            RemoveAllItems();
            if (_filter != null)
            {
                collection = collection.Where(_filter.Satisfy);
            }
            if (_order != null)
            {
                collection = _order.Sort(collection);
            }
            if (_skip > 0)
            {
                collection = collection.Skip(_skip);
            }
            if (_limit > 0)
            {
                collection = collection.Take(_limit);
            }
            foreach (var item in collection)
            {
                var view = Instantiate(_view, _root);
                view.Render(item);
                _views.Add(view);
            }
        }

        private void RemoveAllItems()
        {
            foreach (var view in _views)
            {
                Destroy(view.gameObject);
            }
            _views.Clear();
        }


        protected virtual void OnDestroy()
        {
            RemoveAllItems();
        }
    }
}