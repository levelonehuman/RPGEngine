using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGEngine
{
    /// <summary>
    /// Hostile entities in the game. On death, they give exp/gold. Items will be added soon.
    /// </summary>
    public class Enemy : Entity
    {
        private const int MIN_XP_GRANTED = 12;
        private const int MAX_XP_GRANTED = 1200;

        private const int MIN_GOLD_GRANTED = 12;
        private const int MAX_GOLD_GRANTED = 1200;        

        public int ExperienceGranted;
        public int GoldGranted;        

        [JsonConstructor]
        public Enemy(string name, int level, string role) : base(name, level, role)
        {
            this.PropertyChanged += Enemy_PropertyChanged;
            this.ExperienceGranted = GameWorld.GetValueForLevel(this.Level, MIN_XP_GRANTED, MAX_XP_GRANTED);
            this.GoldGranted = GameWorld.GetValueForLevel(this.Level, MIN_GOLD_GRANTED, MAX_GOLD_GRANTED);
        }       

        private void Enemy_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CurrentHealth" && this.IsDead)
            {
                this.Die();
            }
        }

        private int GetRandomGoldDrop()
        {
            int minGold = (int)(this.GoldGranted * MIN_RAND_MOD);
            int maxGold = (int)(this.GoldGranted * MAX_RAND_MOD);

            return RandomNumberGenerator.GetNumberBetween(minGold, maxGold);
        }

        private void Die()
        {            
            Player.GetPlayer().Experience += this.ExperienceGranted;
            Player.GetPlayer().Gold += GetRandomGoldDrop();            
        }        
    }
}
