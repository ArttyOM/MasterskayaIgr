using Code.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.HUD.Offers
{
    public class ProductView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _count;
        public void Render(OfferProduct product)
        {
            _icon.sprite = product.GetIcon();
            _count.text = product.GetCount();
        }
    }
}