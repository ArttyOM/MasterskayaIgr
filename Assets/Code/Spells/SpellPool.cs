using UnityEngine;

namespace Code.Spells
{
    public class SpellPool: UniRx.Toolkit.ObjectPool<Spell>
    {
        public SpellPool(Spell prefab)
        {
            _prefab = prefab;
        }
        
        private Spell _prefab;

        protected override Spell CreateInstance()
        {
            return Object.Instantiate(_prefab);
        }
    }
}