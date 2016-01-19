using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RPGEngine;
using System.Linq;
using System.Diagnostics;
using System.ComponentModel;
using System.Collections.Generic;
using RPGEngine.Quests;
using RPGEngine.Items;

namespace EngineTests
{
    [TestClass]
    public class EngineTesting
    {   
        public static Enemy enemy = new Enemy("TestEnemy", 1, "Warrior");
        public int killCount = 0;
        Dictionary<int, int> KillsToLevelData;

        [TestCategory("Quests")]
        [TestMethod]
        public void ValidateQuestCreation()
        {
            //Creates the different objects required to instantiate a quest object. Will fail if any part of this doesn't work properly.
            Quest quest = CreateTestQuest();
        }

        [TestCategory("Quests")]
        [TestMethod]
        public void IncrementQuestStepKillCounter()
        {
            Quest quest = CreateTestQuest();
            int oldValue = quest.Steps.First().RequiredKills.First().KilledByPlayer;

            quest.Steps.First().RequiredKills.First().KilledByPlayer++;

            int newValue = quest.Steps.First().RequiredKills.First().KilledByPlayer;

            Assert.IsTrue(newValue > oldValue);
        }

        [TestCategory("Quests")]
        [TestMethod]
        public void CompleteQuestStep()
        {
            Quest quest = CreateTestQuest();            
            
            //make required kills match player kills
            foreach (var killCount in quest.Steps[0].RequiredKills)
            {
                killCount.KilledByPlayer = killCount.RequiredKillCount;                   
            }

            //give player all required items
            foreach (var itemRequired in quest.Steps[0].RequiredItems)
            {
                InventoryItem item = new InventoryItem(itemRequired.Item.ID, itemRequired.Count);
                Player.GetPlayer().Inventory.Add(item);
            }

            //verify step has been completed
            Assert.IsTrue(quest.Steps[0].IsStepCompleted);            
        }

        [TestCategory("Quests")]
        [TestMethod]
        public void CompleteQuest()
        {
            Quest quest = CreateTestQuest();

            //PlayerQuest looks for the ID in GameWorld's quest list. This can be removed once we start generating quests with IDs
            GameWorld.GetWorld().QuestList.Add(quest);
            PlayerQuest playerQuest = new PlayerQuest(quest.ID);

            //loop through all steps and give required items/kills
            foreach (var step in playerQuest.Quest.Steps)
            {
                foreach (var killCount in step.RequiredKills)
                {
                    killCount.KilledByPlayer = killCount.RequiredKillCount;
                }

                //give player all required items
                foreach (var itemRequired in step.RequiredItems)
                {
                    InventoryItem item = new InventoryItem(itemRequired.Item.ID, itemRequired.Count);
                    Player.GetPlayer().Inventory.Add(item);
                }
            }

            //Verify player completed the quest
            Assert.IsTrue(playerQuest.IsCompleted);
        }

        public Quest CreateTestQuest()
        {
            int questID = 1;
            string questName = "Test Quest";
            string questDescription = "Save the planet from the evil Voltron!";
            int minLevel = 1;
            bool isRepeatable = false;

            List<QuestStep> steps = CreateQuestStep();
            QuestReward reward = CreateQuestReward();

            return new Quest(questID, questName, questDescription, minLevel, isRepeatable, steps, reward);
        }

        public List<QuestStep> CreateQuestStep()
        {
            List<QuestStep> stepList = new List<QuestStep>();

            //Describe the step
            string stepName = "Kill Voltron!";
            string stepDescription = "Venture out to The Lair of Voltron and kill him!";

            //Set the first enemy in the list to ID 1 (This can be removed once we start populating enemy IDs
            GameWorld.GetWorld().EnemyList.First().ID = 1;

            //Create a step requirement
            List<EnemyRequiredForStep> stepKills = new List<EnemyRequiredForStep>();
            
            EnemyRequiredForStep enemy = new EnemyRequiredForStep(1, 1);
            stepKills.Add(enemy);

            QuestStep step = new QuestStep(stepName, stepDescription, stepKills: stepKills);

            stepList.Add(step);
            //return a QuestStep object
            return stepList;
        }

        public QuestReward CreateQuestReward()
        {
            List<QuestRewardItem> rewardItems = CreateRewardItemList();
            int rewardXP = 10;
            int rewardGold = 25;

            return new QuestReward(rewardXP, rewardGold, rewardItems);
        }

        public List<QuestRewardItem> CreateRewardItemList()
        {
            //Create an item to reward and add it to the GameWorld's item list
            BaseItem rewardItem = new BaseItem(1, "Vorpal Sword of Doom", 10000, true);
            GameWorld.GetWorld().ItemList.Add(rewardItem);

            //Create a QuestRewardItem by ItemID
            QuestRewardItem reward = new QuestRewardItem(1, 1, false);

            //Add the item to a list
            List<QuestRewardItem> rewardItems = new List<QuestRewardItem>();
            rewardItems.Add(reward);

            return rewardItems;
        }

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
