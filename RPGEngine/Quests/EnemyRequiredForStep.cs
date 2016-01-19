using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGEngine.Quests
{
    public class EnemyRequiredForStep
    {
        public Enemy EnemyToKill { get; private set; }
        public int RequiredKillCount { get; private set; }
        public int KilledByPlayer { get; set; }

        public EnemyRequiredForStep(int enemyID, int killCount)
        {
            this.EnemyToKill = GameWorld.GetWorld().EnemyList.Single(el => el.ID == enemyID);
            this.RequiredKillCount = killCount;
            this.KilledByPlayer = 0;
        }
    }
}
