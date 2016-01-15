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
        public int ExperienceGranted;
        public int GoldGranted;        

        [JsonConstructor]
        public Enemy(string name, int level, string role) : base(name, level, role)
        {
            this.PropertyChanged += Enemy_PropertyChanged;
            this.ExperienceGranted = GameWorld.GetValueForLevel(this.Level, GameWorld.MIN_XP_GRANTED, GameWorld.MAX_XP_GRANTED);
            this.GoldGranted = GameWorld.GetValueForLevel(this.Level, GameWorld.MIN_GOLD_GRANTED, GameWorld.MAX_GOLD_GRANTED);
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
            int minGold = (int)(GoldGranted * GameWorld.MIN_RAND_MOD);
            int maxGold = (int)(GoldGranted * GameWorld.MAX_RAND_MOD);

            return RandomNumberGenerator.GetNumberBetween(minGold, maxGold);
        }

        private void Die()
        {            
            Player.GetPlayer().Experience += this.ExperienceGranted;
            Player.GetPlayer().Gold += GetRandomGoldDrop();            
        }        
    }
}
