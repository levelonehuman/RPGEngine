using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Weighted_Randomizer;

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
        public List<Enemy> SpawnedEnemies = new List<Enemy>();

        private static GameWorld _world;

        IWeightedRandomizer<int> RandomEnemy = new DynamicWeightedRandomizer<int>();

        public const int MAX_LEVEL = 50;
        public const string ENEMY_FILE = "Enemy.json";

        public const double MISS_CHANCE = 0.12;

        private const int MIN_XP_REQ = 1000;
        private const int MAX_XP_REQ = 1250000;

        internal const int MIN_HEALTH = 100;
        internal const int MAX_HEALTH = 12500;

        internal const int MIN_DAMAGE = 5;
        internal const int MAX_DAMAGE = 130;

        internal const int MIN_DEFENSE = 5;
        internal const int MAX_DEFENSE = 130;

        internal const double EFFECTIVE_DEFENSE = 0.8;
        internal const double MIN_RAND_MOD = 0.5;
        internal const double MAX_RAND_MOD = 2.0;

        internal const int MIN_XP_GRANTED = 12;
        internal const int MAX_XP_GRANTED = 1200;

        internal const int MIN_GOLD_GRANTED = 12;
        internal const int MAX_GOLD_GRANTED = 1200;

        internal const int MIN_ITEM_VALUE = 8;
        internal const int MAX_ITEM_VALUE = 1000;

        internal const int HEALTH_PER_VITALITY_POINT = 3;
        internal const int DAMAGE_PER_STRENGTH_POINT = 3;
        internal const int DEFENSE_PER_RESISTANCE_POINT = 3;

        internal const double MAX_RUN_SPEED = 0.25;
        internal const double MAX_ATTACK_SPEED = 1.0;
        internal const double MAX_LIFE_PER_HIT = 0.1;
        internal const double MAX_LIFE_PER_SEC = 0.1;

        
        private const string _path = @"C:\Users\shelms\documents\visual studio 2015\Projects\RPGEngine\RPGEngine\Data\";
        

        public static GameWorld GetWorld()
        {
            if (_world == null)
            {
                _world = new GameWorld();               
            }

            return _world;
        }

        private GameWorld()
        {
            InitializeWorld();
        }        

        private void InitializeWorld()
        {
            BuildLevelTable();
            BuildEnemyList();            
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
