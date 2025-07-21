
using System;
using System.Collections.Generic;
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
        private float _rangeDamageIncrease = 1;
        private float _bulletDamageIncrease = 1;
        private float _summonDamageIncrease = 1;
        private float _movementSpeedIncrease = 1;
        private float _meleeAttackSpeedIncrease = 1;
        private float _rangedAttackSpeedIncrease = 1;
        private float _extraProjectiles = 0;
        private float _lifeRegenIncrease = 0;
        private float _maxHealthIncrease = 0;
        private float _maxManaIncrease = 0;
        public float DefenceIncrease { get { return _defenceIncrease; } set { _defenceIncrease = value; } }
        public float MeleeDamageIncrease { get { return _meleeDamageIncrease; } set { _meleeDamageIncrease = value; } }
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
    }
}