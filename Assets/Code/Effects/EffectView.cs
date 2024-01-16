using UnityEngine;

namespace Code.Effects
{
    [DisallowMultipleComponent]
    public class EffectView : MonoBehaviour
    {
        [field: SerializeField] public EffectType Type { get; private set; }
    }
}