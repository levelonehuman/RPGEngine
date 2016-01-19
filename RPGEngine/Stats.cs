using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGEngine
{
    /// <summary>
    /// Stat values for enemies, player, and items
    /// </summary>
    public class Stats// : INotifyPropertyChanged
    {
        public Stats() { }

        //public event PropertyChangedEventHandler PropertyChanged;

        //private void Stats_PropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //            
        //}
        private const int MIN_HEALTH = 100;
        private const int MAX_HEALTH = 12500;
        
        private const int MIN_DAMAGE = 5;
        private const int MAX_DAMAGE = 130;
        
        private const int MIN_DEFENSE = 5;
        private const int MAX_DEFENSE = 130;
        
        private const int HEALTH_PER_VITALITY_POINT = 3;
        private const int DAMAGE_PER_STRENGTH_POINT = 3;
        private const int DEFENSE_PER_RESISTANCE_POINT = 3;
        
        private const double MAX_RUN_SPEED = 0.25;
        private const double MAX_ATTACK_SPEED = 1.0;
        private const double MAX_LIFE_PER_HIT = 0.1;
        private const double MAX_LIFE_PER_SEC = 0.1;

        //Primary stats
        public int Health
        {
            get { return _health; }
            set { _health = value; }
        }
        public int Damage
        {
            get { return _damage; }
            set { _damage = value; }
        }
        public int Defense
        {
            get { return _defense; }
            set { _defense = value; }
        }        

        //Secondary stats
        public int Strength
        {
            get { return _strength; }
            set { _strength = value; }
        }
        public int Vitality
        {
            get { return _vitality; }
            set { _vitality = value; }
        }
        public int Resistance
        {
            get { return _resistance; }
            set { _resistance = value; }
        }        

        //Bonus stats
        public double RunSpeed
        {
            get { return Math.Min(_runSpeed, MAX_RUN_SPEED); }
            set { _runSpeed = value; }
        }
        public double AttackSpeed
        {
            get { return Math.Min(_attackSpeed, MAX_ATTACK_SPEED); }
            set { _attackSpeed = value; }
        }
        public double LifePerHit
        {
            get { return Math.Min(_lifePerHit, MAX_LIFE_PER_HIT); }
            set { _lifePerHit = value; }
        }
        public double LifePerSec
        {
            get { return Math.Min(_lifePerSec, MAX_LIFE_PER_SEC); }
            set { _lifePerSec = value; }
        }

        private int _health;
        private int _damage;
        private int _defense;

        private int _strength;
        private int _vitality;
        private int _resistance;

        private double _runSpeed;
        private double _attackSpeed;
        private double _lifePerHit;
        private double _lifePerSec;        

        internal void SetStatsForLevel(int level, EntityRole role)
        {
            _health = CalculateHealth(level, role);
            _damage = CalculateDamage(level, role);            
            _defense = CalculateDefense(level, role);            
        }        

        private int CalculateHealth(int level, EntityRole role)
        {
            int baseHealth = GameWorld.GetValueForLevelWithModifier(level, MIN_HEALTH, MAX_HEALTH, role.HealthModifier);
            int addedHealth = _vitality * HEALTH_PER_VITALITY_POINT;

            return baseHealth + addedHealth;
        }
        
        private int CalculateDamage(int level, EntityRole role)
        {
            int baseDamage = GameWorld.GetValueForLevelWithModifier(level, MIN_DAMAGE, MAX_DAMAGE, role.DamageModifier);
            int addedDamage = _strength * DAMAGE_PER_STRENGTH_POINT;

            return baseDamage + addedDamage;
        }

        private int CalculateDefense(int level, EntityRole role)
        {
            int baseDefense = GameWorld.GetValueForLevelWithModifier(level, MIN_DEFENSE, MAX_DEFENSE, role.DefenseModifier);
            int addedDefense = _resistance * DEFENSE_PER_RESISTANCE_POINT;

            return baseDefense + addedDefense;
        }
    }
}
