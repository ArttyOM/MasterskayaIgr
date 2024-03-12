using System;
using Code.Enemies;
using Code.Events;
using Code.Saves;
using Code.Spells;
using Code.Upgrades;

namespace Code.Projectiles
{
    public class OnClickProjectileThrower: IDisposable
    {
        private readonly WeaponRandomGenerator _weaponRandomGenerator;
        private readonly SpellVfxGenerator _spellVfxGenerator;
        private readonly ExplosionHandler _explosionHandler;
        private readonly GridPointSelector _gridPointSelector;
        private readonly ProjectileThrower _projectileThrower;

        public OnClickProjectileThrower(ConfigHolder configHolder, InGameEvents events, PlayerProfile profile, UpgradeSystem upgradeSystem)
        {
            _weaponRandomGenerator = new WeaponRandomGenerator(configHolder.WeaponSpawnChanceConfig);
            _spellVfxGenerator = new SpellVfxGenerator(configHolder.SpellsConfig, events.OnSpellSelected, events.OnSessionStart);
            _explosionHandler = new(events.OnProjectileExploded, events.OnExplosionEnter, configHolder.SpellsConfig, new UpgradeService(profile.GetUpgrades(), upgradeSystem));
            _gridPointSelector = new GridPointSelector(configHolder.AutofireConfig, events.OnProjectileDestinationSelected, events.OnSessionStart);

            var spellPools = _spellVfxGenerator.GetSpellPools;
            _projectileThrower = new(_weaponRandomGenerator, events.OnProjectileDestinationSelected, events.OnProjectileExploded, events.OnSpellSelected, spellPools);

            
        }

        public void Dispose()
        {
            _weaponRandomGenerator?.Dispose();
            _spellVfxGenerator?.Dispose();
            _explosionHandler?.Dispose();
            _gridPointSelector?.Dispose();
            _projectileThrower?.Dispose();
        }
    }

    public class ConfigHolder
    {
        public AutofireConfig AutofireConfig { get; }
        public WeaponSpawnChanceConfig WeaponSpawnChanceConfig { get; }
        public SpellsConfig SpellsConfig { get; }

        public ConfigHolder(AutofireConfig autofireConfig, WeaponSpawnChanceConfig weaponSpawnChanceConfig, SpellsConfig spellsConfig)
        {
            AutofireConfig = autofireConfig;
            WeaponSpawnChanceConfig = weaponSpawnChanceConfig;
            SpellsConfig = spellsConfig;
        }
    }
}