using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Weighted_Randomizer;
using RPGEngine.Quests;

namespace RPGEngine
{
    /// <summary>
    /// Holds the instance of world the game is played in. 
    /// Will build level/xp table, enemylist, quests, etc.
    /// </summary>
    public class GameWorld
    {        
        public Dictionary<int, int> LevelTable = new Dictionary<int, int>();        
        public List<Enemy> EnemyList = new List<Enemy>();        
        public List<BaseItem> ItemList = new List<BaseItem>();
        public List<Quest> QuestList = new List<Quest>();

        public List<Enemy> SpawnedEnemies = new List<Enemy>();

        private static GameWorld _world;

        IWeightedRandomizer<int> RandomEnemy = new DynamicWeightedRandomizer<int>();

        public const int MAX_LEVEL = 50;
        public const string ENEMY_FILE = "Enemy.json";
        public const string ITEM_FILE = "Item.json";
        public const string QUEST_FILE = "Quest.json";
        
        private const int MIN_XP_REQ = 1000;
        private const int MAX_XP_REQ = 1250000;
                
        //internal const int MIN_ITEM_VALUE = 8;
        //internal const int MAX_ITEM_VALUE = 1000;        

        public static GameWorld GetWorld()
        {
            if (_world == null)
            {
                _world = new GameWorld();               
            }

            return _world;
        }

        //When the GameWorld object is first created, read in all game data
        private GameWorld() 
        {
            InitializeWorld();
        }

        private void InitializeWorld()
        {
            BuildLevelTable();
            BuildEnemyList();
            //BuildItemList();
            //BuildQuestList();        
        }

        private void BuildLevelTable()
        {
            for (int i = 1; i <= MAX_LEVEL; i++)
            {
                int xpRequired = GetValueForLevel(i, MIN_XP_REQ, MAX_XP_REQ);

                LevelTable.Add(i, xpRequired);
            }
        }

        private void BuildEnemyList()
        {
            EnemyList = JsonHelpers.ReadFileToList<Enemy>(ENEMY_FILE);
        }

        private void BuildItemList()
        {
            ItemList = JsonHelpers.ReadFileToList<BaseItem>(ITEM_FILE);
        }

        private void BuildQuestList()
        {
            QuestList = JsonHelpers.ReadFileToList<Quest>(QUEST_FILE);
        }

        public static int GetValueForLevel(int level, int minValue, int maxValue)
        {
            double B = Math.Log(maxValue / minValue) / (GameWorld.MAX_LEVEL - 1);
            double A = minValue / (Math.Exp(B) - 1.0);

            int previousLevelValue = (int)Math.Round(A * Math.Exp(B * (level - 1)));
            int currentLevelValue = (int)Math.Round(A * Math.Exp(B * level));

            return currentLevelValue - previousLevelValue;
        }

        public static int GetValueForLevelWithModifier(int level, int minValue, int maxValue, double modifier)
        {
            double B = Math.Log(maxValue / minValue) / (GameWorld.MAX_LEVEL - 1);
            double A = minValue / (Math.Exp(B) - 1.0);

            int previousLevelValue = (int)Math.Round(A * Math.Exp(B * (level - 1)) * modifier);
            int currentLevelValue = (int)Math.Round(A * Math.Exp(B * level) * modifier);

            return currentLevelValue - previousLevelValue;
        }        
    }
}
