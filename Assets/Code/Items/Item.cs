using UnityEngine;

namespace Code.Items
{
    [CreateAssetMenu(menuName = "Items/New Item")]
    public class Item : ScriptableObject
    {
        [SerializeField] private string _title;
        [SerializeField] private string _description;
        [SerializeField] private Sprite _icon;

        public Sprite Icon => _icon;
        public string Title => _title;
        public string Description => _description;
    }
}