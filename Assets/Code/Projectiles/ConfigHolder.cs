using Code.Spells;

namespace Code.Projectiles
{
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