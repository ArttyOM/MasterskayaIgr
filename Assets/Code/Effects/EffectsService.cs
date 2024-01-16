using System.Collections.Generic;
using UnityEngine;

namespace Code.Effects
{
    [DisallowMultipleComponent]
    public class EffectsService : MonoBehaviour
    {
        [field: SerializeField] public List<EffectView> ParticleSystems { get; private set; }
    }
}