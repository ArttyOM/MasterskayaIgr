using System.Collections.Generic;
using Code.Enemies;

namespace Code.Spells
{
    public interface ISpellActingOnEnemy
    {
        void Act(SpellExplosion explosion, SpellBalanceConfig spellConfig);
    }
}