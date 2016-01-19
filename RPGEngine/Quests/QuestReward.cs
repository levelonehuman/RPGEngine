using RPGEngine.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGEngine.Quests
{
    /// <summary>
    /// Identifies the reward for completing a quest.
    /// </summary>
    public class QuestReward
    {
        public int RewardExperiencePoints { get; private set; }
        public int RewardCurrency { get; private set; }
        public List<QuestRewardItem> RewardItems { get; private set; }

        public QuestReward()
        {
            this.RewardExperiencePoints = 0;
            this.RewardCurrency = 0;
            this.RewardItems = new List<QuestRewardItem>();
        }

        public QuestReward(int rewardXP = 0, int rewardCurrency = 0, List<QuestRewardItem> rewardItems = null)
        {
            this.RewardExperiencePoints = rewardXP;
            this.RewardCurrency = rewardCurrency;
            this.RewardItems = rewardItems ?? new List<QuestRewardItem>();
        }
    }
}
