using Terraria.ModLoader;


namespace Runeforge.Content.SkillTree
{
    public delegate void ModifyPlayer(ModPlayer player);
    public enum NodeType
    {
        Empty,
        Defence,
        MeleeDamage,
        Health,
        HealthRegen,
        Poison,
        FireDamage,
        MovementSpeed,
        BulletDamage,
        RangedDamage,
        MagicDamage,
        CriticalHitChance,
        CriticalHitDamage,
        ShadowDamage,
        SummonDamage,
        LifeSteal,
        ProjectileCount,
        MeleeAttackSpeed,
        RangedAttackSpeed,
        MAJOR_RighteousFire,
        MAJOR_MinionCreator
    }

    public enum UIType
    {
        hoverOver
    }
    public enum ConnectionDirection
    {
        UP,
        DOWN,
        RIGHT,
        LEFT,
        DIAGONAL_LEFT,
        DIAGONAL_RIGHT,
    }
}