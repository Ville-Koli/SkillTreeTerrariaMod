using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Runeforge.Content.Buffs;
using Terraria.UI;
using System.Collections.Generic;
using Terraria.GameInput;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework.Input;
using Runeforge.Content.UI;

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