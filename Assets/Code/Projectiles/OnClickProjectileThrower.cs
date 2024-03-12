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
            _weaponRandomGenerator.GenerateWeapon(out _, out _);
            _weaponRandomGenerator.GenerateWeapon(out Weapon loadedWeapon,out _);
            _weaponRandomGenerator.SendWeaponToLoadedPositionInstantly(loadedWeapon);
            
            _gridPointSelector = new GridPointSelector(configHolder.AutofireConfig, events.OnProjectileDestinationSelected, events.OnSessionStart);
            
            _spellVfxGenerator = new SpellVfxGenerator(configHolder.SpellsConfig, events.OnSpellSelected, events.OnSessionStart);
            
            var spellPools = _spellVfxGenerator.GetSpellPools;
            _projectileThrower = new(_weaponRandomGenerator, events.OnProjectileDestinationSelected, events.OnProjectileExploded, events.OnSpellSelected, spellPools);
            _explosionHandler = new(events.OnProjectileExploded, events.OnExplosionEnter, configHolder.SpellsConfig, new UpgradeService(profile.GetUpgrades(), upgradeSystem));
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
}