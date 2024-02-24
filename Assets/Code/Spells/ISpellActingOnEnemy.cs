using System;
using System.Collections.Generic;
using Code.Enemies;
using Code.Upgrades;

namespace Code.Spells
{
    public interface ISpellActingOnEnemy
    {
        void Act(SpellExplosion explosion, SpellBalanceConfig spellConfig);
        void Init(IObservable<(CommonEnemy, SpellExplosion)> onEnemyExploded,
            SpellBalanceConfig commonSpellBalance, SpellBalanceConfig megaSpellConfig, UpgradeService upgradeService);
    }
}