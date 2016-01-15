using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGEngine
{
    public class Item
    {
        /// <summary>
        /// Handles items in the game. Each item can have any of the corresponding stats.
        /// </summary>
        public string Name { get; set; }
        public int Value { get; set; }
        public Stats Stats;

        public Item(string name, int value, int health = 0, int damage = 0, int defense = 0, int strength = 0, int vitality = 0, int resistance = 0, double runSpeed = 0, double attackSpeed = 0, double lifePerHit = 0, double lifePerSec = 0)
        {
            this.Name = name;
            this.Value = value;

            this.Stats.Health = health;
            this.Stats.Damage = damage;
            this.Stats.Defense = defense;
            this.Stats.Strength = strength;
            this.Stats.Vitality = vitality;
            this.Stats.Resistance = resistance;
            this.Stats.RunSpeed = runSpeed;
            this.Stats.AttackSpeed = attackSpeed;
            this.Stats.LifePerHit = lifePerHit;
            this.Stats.LifePerSec = lifePerSec;
        }
    }
}
