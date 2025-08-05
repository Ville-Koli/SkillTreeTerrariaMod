using Runeforge.Content.UI;
using Terraria.ModLoader;


namespace Runeforge.Content.SkillTree
{
    public delegate void ModifyPlayer(StatBlock statBlock, NodeUI node);
    public enum NodeType
    {
        Empty,
        Defence,
        MeleeDamage,
        BulletDamage,
        RangedDamage,
        MagicDamage,
        ShadowDamage,
        SummonDamage,
        FireDamage,
        PoisonImbuement,
        MeleeAttackSpeed,
        RangedAttackSpeed,
        LifeSteal,
        MaxHealth,
        MaxMana,
        HealthRegen,
        Healing,
        Poison,
        MovementSpeed,
        ApplyBuff,
        CriticalHitChance,
        CriticalHitDamage,
        ProjectileCount,
        MAJOR_RighteousFire,
        MAJOR_MinionCreator
    }

    public enum UIType
    {
        HoverOver,
        LevelBar,
        SkillPointOrb
    }
    public enum ConnectionDirection
    {
        UP,
        DOWN,
        RIGHT,
        LEFT,
        DIAGONAL_TOP_RIGHT,
        DIAGONAL_TOP_LEFT,
        DIAGONAL_BOTTOM_RIGHT,
        DIAGONAL_BOTTOM_LEFT
    }
}