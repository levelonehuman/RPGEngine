using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGEngine.Items
{
    public class QuestRewardItem
    {
        public BaseItem Item { get; private set; }
        public int Count { get; private set; }
        public bool IsChosenReward { get; private set; } //differentiate between auto-granted rewards and a single item chosen from a group.

        public QuestRewardItem(int itemID, int count, bool isChosenReward)
        {
            this.Item = GameWorld.GetWorld().ItemList.Single(il => il.ID == itemID);
            this.Count = count;
            this.IsChosenReward = isChosenReward;
        }
    }
}
