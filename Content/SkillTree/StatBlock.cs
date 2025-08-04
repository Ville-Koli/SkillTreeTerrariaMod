
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Runeforge.Content.UI;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Runeforge.Content.SkillTree
{
    public class StatBlock
    {
        private List<int> buffIDs = new(); // list of buffs that the statblock provides to SBP
        private float _defenceIncrease = 0;
        private float _meleeDamageIncrease = 1;
        private float _lifestealIncrease = 1;
        private float _rangeDamageIncrease = 1;
        private float _bulletDamageIncrease = 1;
        private float _summonDamageIncrease = 1;
        private float _magicDamageIncrease = 1;
        private float _movementSpeedIncrease = 1;
        private float _meleeAttackSpeedIncrease = 1;
        private float _rangedAttackSpeedIncrease = 1;
        private float _extraProjectiles = 0; // not yet implemented
        private float _lifeRegenIncrease = 0;
        private float _maxHealthIncrease = 0;
        private float _maxManaIncrease = 0;
        private float _critChanceIncrease = 0;
        private float _critDamageIncrease = 1;
        private float _currentExperience = 0;
        private float _requiredExperienceForLevel = 45;
        private float _currentLevel = 0;
        private int _skillPoints = 0;
        public float DefenceIncrease { get { return _defenceIncrease; } set { _defenceIncrease = value; } }
        public float MeleeDamageIncrease { get { return _meleeDamageIncrease; } set { _meleeDamageIncrease = value; } }
        public float LifestealIncrease { get { return _lifestealIncrease; } set { _lifestealIncrease = value; } }
        public float RangeDamageIncrease { get { return _rangeDamageIncrease; } set { _rangeDamageIncrease = value; } }
        public float BulletDamageIncrease { get { return _bulletDamageIncrease; } set { _bulletDamageIncrease = value; } }
        public float SummonDamageIncrease { get { return _summonDamageIncrease; } set { _summonDamageIncrease = value; } }
        public float MovementSpeedIncrease { get { return _movementSpeedIncrease; } set { _movementSpeedIncrease = value; } }
        public float MeleeAttackSpeedIncrease { get { return _meleeAttackSpeedIncrease; } set { _meleeAttackSpeedIncrease = value; } }
        public float RangedAttackSpeedIncrease { get { return _rangedAttackSpeedIncrease; } set { _rangedAttackSpeedIncrease = value; } }
        public float ExtraProjectiles { get { return _extraProjectiles; } set { _extraProjectiles = value; } }
        public float LifeRegenIncrease { get { return _lifeRegenIncrease; } set { _lifeRegenIncrease = value; } }
        public float MaxHealthIncrease { get { return _maxHealthIncrease; } set { _maxHealthIncrease = value; } }
        public float MaxManaIncrease { get { return _maxManaIncrease; } set { _maxManaIncrease = value; } }
        public float MagicDamageIncrease { get { return _magicDamageIncrease; } set { _magicDamageIncrease = value; } }
        public float CritChanceIncrease { get { return _critChanceIncrease; } set { _critChanceIncrease = value; } }
        public float CritDamageIncrease { get { return _critDamageIncrease; } set { _critDamageIncrease = value; } }
        public float CurrentExperience { get { return _currentExperience; } set { _currentExperience = value; } }
        public float RequiredExperienceForLevel { get { return _requiredExperienceForLevel; } set { _requiredExperienceForLevel = value; } }
        public float CurrentLevel { get { return _currentLevel; } set { _currentLevel = value; } }
        public int SkillPoints { get { return _skillPoints; } set { _skillPoints = value; } }
        public List<int> GetBuffIDs()
        {
            return buffIDs;
        }
        public void SetBuffIDs(List<int> newBuffIDs)
        {
            buffIDs = newBuffIDs;
        }
        public bool ApplyBuff(int buffid)
        {
            if (!buffIDs.Contains(buffid))
            {
                buffIDs.Add(buffid);
                return true;
            }
            return false;
        }
        public bool RemoveBuff(int buffid)
        {
            return buffIDs.Remove(buffid);
        }
        public void AddMeleeDamageIncrease(float amount)
        {
            MeleeDamageIncrease += amount;
        }
        public void AddDefenceIncrease(float amount)
        {
            DefenceIncrease += amount;
        }
        public void AddRangeDamageIncrease(float amount)
        {
            RangeDamageIncrease += amount;
        }
        public void AddBulletDamageIncrease(float amount)
        {
            BulletDamageIncrease += amount;
        }
        public void AddSummonDamageIncrease(float amount)
        {
            SummonDamageIncrease += amount;
        }
        public void AddMovementSpeedIncreasee(float amount)
        {
            MovementSpeedIncrease += amount;
        }
        public void AddMeleeAttackSpeedIncrease(float amount)
        {
            MeleeAttackSpeedIncrease += amount;
        }
        public void AddRangedAttackSpeedIncrease(float amount)
        {
            RangedAttackSpeedIncrease += amount;
        }
        public void AddExtraProjectiles(float amount)
        {
            ExtraProjectiles += amount;
        }
        public void AddLifeRegenIncrease(float amount)
        {
            LifeRegenIncrease += amount;
        }
        public void AddMaxHealthIncrease(float amount)
        {
            MaxHealthIncrease += amount;
        }
        public void AddMaxManaIncrease(float amount)
        {
            MaxManaIncrease += amount;
        }
        public void AddMagicDamageIncrease(float amount)
        {
            MagicDamageIncrease += amount;
        }
        public void AddCritChanceIncrease(float amount)
        {
            CritChanceIncrease += amount;
        }
        public void AddCritDamageIncrease(float amount)
        {
            CritDamageIncrease += amount;
        }
        public void AddLifeStealIncrease(float amount)
        {
            LifestealIncrease += amount;
        }
        public void AddExperience(float amount)
        {
            CurrentExperience += amount;

            if (CurrentExperience > RequiredExperienceForLevel)
            {
                CurrentLevel += 1;
                CurrentExperience -= RequiredExperienceForLevel;
                RequiredExperienceForLevel *= 1.1902668536131113f; // holy magic numbah 
                // (explanation: sum of most bosses max health then multiply that by 35 and take the 80th root of it. We want lvl 80 to be the really end game level so that there is room for post moonlord content like calamity)
                ChangeSkillPointAmount(+1);
                LevelUI.SetLevel((int)CurrentLevel);
                ModContent.GetInstance<Runeforge>().Logger.Info($"LEVELED UP TO {CurrentLevel} {CurrentExperience} {RequiredExperienceForLevel}!");
                if (CurrentExperience > RequiredExperienceForLevel)
                {
                    AddExperience(0); // causes it to recursively level up until no more excess experience is left
                }
            }
            LevelUI.SetExp(CurrentExperience, RequiredExperienceForLevel);
        }

        public string GetStat(NodeType type)
        {
            switch (type)
            {
                case NodeType.Defence:
                    return DefenceIncrease.ToString() + " Defence";
                case NodeType.RangedDamage:
                    return RangeDamageIncrease.ToString() + "x Ranged damage";
                case NodeType.MeleeDamage:
                    return MeleeDamageIncrease.ToString() + "x Melee damage";
                case NodeType.SummonDamage:
                    return SummonDamageIncrease.ToString() + "x Summon damage";
                case NodeType.RangedAttackSpeed:
                    return RangedAttackSpeedIncrease.ToString() + "x Ranged attack speed";
                case NodeType.MeleeAttackSpeed:
                    return MeleeAttackSpeedIncrease.ToString() + "x Melee attack speed";
                case NodeType.BulletDamage:
                    return BulletDamageIncrease.ToString() + "x Bullet damage";
                case NodeType.MovementSpeed:
                    return MovementSpeedIncrease.ToString() + "% Movement speed";
                case NodeType.MaxHealth:
                    return MaxHealthIncrease.ToString() + " Max health";
                case NodeType.MaxMana:
                    return MaxManaIncrease.ToString() + " Max mana";
                case NodeType.MagicDamage:
                    return MagicDamageIncrease.ToString() + " Magic damage";
                case NodeType.HealthRegen:
                    return LifeRegenIncrease.ToString() + " Health regen";
                case NodeType.ProjectileCount:
                    return ExtraProjectiles.ToString() + " Projectile count";
                case NodeType.CriticalHitChance:
                    return CritChanceIncrease.ToString() + "% Crit chance";
                case NodeType.CriticalHitDamage:
                    return CritDamageIncrease.ToString() + "x Crit damage";
                case NodeType.LifeSteal:
                    return LifestealIncrease.ToString() + "x Lifesteal";
            }
            return "0";
        }

        public void UpdateLevelUI()
        {
            LevelUI.SetLevel((int)CurrentLevel);
            LevelUI.SetExp(CurrentExperience, RequiredExperienceForLevel);
        }

        public void UpdateSkillPointAmount()
        {
            SkillPointDisplay.EditSkillPointAmount(SkillPoints);
        }

        public void ChangeSkillPointAmount(int amount)
        {
            SkillPoints += amount;
            UpdateSkillPointAmount();
        }
    }
}