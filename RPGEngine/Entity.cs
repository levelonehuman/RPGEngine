using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGEngine
{
    /// <summary>
    /// Base class for Enemies and Player.
    /// Holds core functionality for both classes, and handles attacking/taking damage/healing/etc.
    /// </summary>
    public class Entity : INotifyPropertyChanged
    {
        internal const double EFFECTIVE_DEFENSE = 0.8;
        internal const double MIN_RAND_MOD = 0.5;
        internal const double MAX_RAND_MOD = 2.0;

        private const double MISS_CHANCE = 0.12;
        
        public int ID { get; set; } //Making this a base property in case merchants/other NPCs inherit from this class
        public string Name { get; set; }
        public Stats Stats { get; set; }
        public string Role { get; set; }
        public int Level
        {
            get { return _level; }
            set
            {
                if (value != _level)
                {
                    _level = value;
                    OnPropertyChanged("Level");
                }
            }
            }
        public int CurrentHealth
        {
            get { return this._currentHealth; }
            set
            {
                if (value != _currentHealth)
                {
                    _currentHealth = value;
                    OnPropertyChanged("CurrentHealth");
                }
            }
        }        

        [JsonIgnore]
        public bool IsDead { get { return this.CurrentHealth <= 0; } }

        private int _currentHealth;
        private int _level;
        private int _minDamage { get { return (int)(this.Stats.Damage * MIN_RAND_MOD); } }
        private int _maxDamage { get { return (int)(this.Stats.Damage * MAX_RAND_MOD); } }
        protected double modifier { get { return Math.Max(this.Level / 2, 1); } }
        protected EntityRole _role;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }        

        [JsonConstructor]
        public Entity(string name, int level, string role)
        {
            this.PropertyChanged += Entity_PropertyChanged;

            this.Name = name;            
            this._role = EntityRole.CreateRole(role);
            this.Stats = new Stats();
            this.Level = level;
            this.Role = role;
            this.CurrentHealth = this.Stats.Health;
                    
        }

        private void Entity_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Level")
            {
                this.Stats.SetStatsForLevel(this.Level, this._role);
                this.HealDamage(this.Stats.Health);
            }            
        }

        public void Attack(Entity enemy)
        {
            int damage = CalculateDamage(enemy);
            enemy.TakeDamage(damage);
        }

        protected void TakeDamage(int damage)
        {
            this.CurrentHealth -= damage;            
        }

        public void HealDamage(int healAmount)
        {
            int totalHealth = this.CurrentHealth + healAmount;
            this.CurrentHealth = Math.Min(totalHealth, this.Stats.Health);
        }

        protected int CalculateDamage(Entity enemy)
        {
            int damage = 0;

            if (Missed() == false)
            {
                int enemyDefense = (int)(enemy.Stats.Defense * EFFECTIVE_DEFENSE);
                int minDmg = Math.Max(_minDamage - enemyDefense, 0);
                int maxDmg = Math.Max(_maxDamage - enemyDefense, 0);

                damage = RandomNumberGenerator.GetNumberBetween(minDmg, maxDmg);                
            }

            return Math.Max(damage, 0);            
        }

        protected bool Missed()
        {
            return RandomNumberGenerator.GetNumberBetween(1, 100) < (MISS_CHANCE * 100);
        }        
    }
}
