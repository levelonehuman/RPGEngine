using RPGEngine.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGEngine.Quests
{
    /// <summary>
    /// Intermittent rewards for completing a quest step. This is separate from the QuestReward given on quest completion
    /// </summary>
    public class QuestStepReward : QuestReward
    {
        /// <summary>
        /// Use this constructor if there is no reward for this quest step
        /// </summary>
        public QuestStepReward() { }

        public QuestStepReward(int rewardXP, int rewardCurrency, List<QuestRewardItem> rewardItems) : base(rewardXP, rewardCurrency, rewardItems)
        {

        }       
    }
}
