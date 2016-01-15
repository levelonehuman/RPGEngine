using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RPGEngine;
using System.Linq;
using System.Diagnostics;
using System.ComponentModel;
using System.Collections.Generic;

namespace EngineTests
{
    [TestClass]
    public class EngineTesting
    {   
        public static Enemy enemy = new Enemy("TestEnemy", 1, "Warrior");
        public int killCount = 0;
        Dictionary<int, int> KillsToLevelData;

        [TestCategory("Leveling")]
        [TestMethod]
        public void PlayerLevelUp()
        {
            int oldLevel = Player.GetPlayer().Level;
            int oldHP = Player.GetPlayer().Stats.Health;
            int oldDmg = Player.GetPlayer().Stats.Damage;
            int oldDef = Player.GetPlayer().Stats.Defense;

            Player.GetPlayer().Experience += GameWorld.GetWorld().LevelTable[4];

            Assert.IsTrue(oldLevel < Player.GetPlayer().Level);
            Assert.IsTrue(oldHP < Player.GetPlayer().Stats.Health);
            Assert.IsTrue(Player.GetPlayer().CurrentHealth == Player.GetPlayer().Stats.Health);
            Assert.IsTrue(oldDmg < Player.GetPlayer().Stats.Damage);
            Assert.IsTrue(oldDef < Player.GetPlayer().Stats.Defense);
        }

        [TestCategory("Battle")]
        [TestMethod]
        public void PlayerAttackEnemy()
        {
            while (enemy.Stats.Health == enemy.CurrentHealth)
            {
                Player.GetPlayer().Attack(enemy);
            }            
        }

        [TestCategory("Battle")]
        [TestMethod]
        public void PlayerTakeDamage()
        {
            int beforeAttack = Player.GetPlayer().CurrentHealth;            

            enemy.Attack(Player.GetPlayer());

            Assert.IsTrue(Player.GetPlayer().CurrentHealth <= beforeAttack);
        }

        [TestCategory("Battle")]
        [TestMethod]
        public void PlayerHealDamage()
        {
            int beforeHeal = Player.GetPlayer().CurrentHealth; 

            PlayerTakeDamage();            

            Player.GetPlayer().HealDamage(Player.GetPlayer().Stats.Health);

            Assert.IsTrue(beforeHeal <= Player.GetPlayer().CurrentHealth);
            Assert.IsFalse(Player.GetPlayer().CurrentHealth > Player.GetPlayer().Stats.Health);
            
        }

        [TestCategory("Battle")]
        [TestMethod]
        public void EnemyDie()
        {
            Debug.WriteLine(string.Format("Gold: {0}\nXP: {1}", Player.GetPlayer().Gold, Player.GetPlayer().Experience));

            int oldXP = Player.GetPlayer().Experience;
            int oldGold = Player.GetPlayer().Gold;

            while (!enemy.IsDead)
            {
                Player.GetPlayer().Attack(enemy);
            }            
            
            Debug.WriteLine(string.Format("Gold: {0}\nXP: {1}", Player.GetPlayer().Gold, Player.GetPlayer().Experience));

            Assert.IsTrue(enemy.IsDead);
            Assert.IsTrue(Player.GetPlayer().Experience == (oldXP + enemy.ExperienceGranted));
            //Assert.IsTrue(Player.GetPlayer().Gold == (oldGold + enemy.GoldGranted));
        }        

        #region data
        [TestCategory("Data")]
        [TestMethod]
        public void KillsToLevel()
        {
            killCount = 0;
            KillsToLevelData = new Dictionary<int, int>();
            int levelTo = 50;            

            RegisterEntity();
            while (Player.GetPlayer().Level < levelTo)
            {                
                Player.GetPlayer().Attack(enemy);
            }

            foreach (KeyValuePair<int, int> item in KillsToLevelData)
            {
                Debug.WriteLine(string.Format("Kills to level {0}: {1}", item.Key, item.Value));
            }

            Assert.IsTrue(Player.GetPlayer().Level == levelTo);
        }

        [TestCategory("Data")]
        [TestMethod]
        public void NthRootStats()
        {
            int baseHealth = 100;
            int level = 50;

            for (int i = 1; i <= level; i++)
            {
                int oldHealth = (int)(baseHealth + Math.Pow(baseHealth, 1.0 / i - i));
                int newHealth = (int)(baseHealth + Math.Pow(baseHealth, 1.0 / i));

                Debug.WriteLine(newHealth - oldHealth);
            }            
        }

        [TestCategory("Data")]
        [TestMethod]
        public void LevelXP()
        {
            int levels = 50;
            int firstLevelXP = 1250;
            int lastLevelXP = 1250000;

            Dictionary<int, int> Levels = new Dictionary<int, int>();

            double B = Math.Log((double)lastLevelXP / firstLevelXP) / (levels - 1);
            double A = (double)firstLevelXP / (Math.Exp(B) - 1.0);

            for (int i = 2; i <= levels; i++)
            {
                int oldXP = (int)Math.Round(A * Math.Exp(B * (i - 1)));
                int newXP = (int)Math.Round(A * Math.Exp(B * i));

                Levels.Add(i, newXP - oldXP);
                Debug.WriteLine("Level {0}: {1}", i, Levels[i]);
            }

            Debug.WriteLine("");
            for (int i = 3; i <= levels; i++)
            {
                int xpNeeded = Levels[i] - Levels[i - 1];
                Debug.WriteLine("XP Needed for {0}-{1}: {2}", Levels.Keys.First(k => k == i - 1), Levels.Keys.First(k => k == i), xpNeeded);
            }
        }        
        #endregion

        #region helpers
        private void RespawnEnemy()
        {
            enemy.ExperienceGranted = GameWorld.GetValueForLevel(Player.GetPlayer().Level, 12, 1200);
            enemy.HealDamage(enemy.Stats.Health);
        }       
        
        private void RegisterEntity()
        {
            Player.GetPlayer().PropertyChanged += Player_PropertyChanged;
            enemy.PropertyChanged += Enemy_PropertyChanged;
        }

        private void Enemy_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CurrentHealth")
            {
                if (enemy.IsDead)
                {
                    killCount++;
                    RespawnEnemy();                    
                }
            }
        }        

        private void Player_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Level")
            {
                KillsToLevelData.Add(Player.GetPlayer().Level, killCount);
            }
        }
        #endregion
    }
}
