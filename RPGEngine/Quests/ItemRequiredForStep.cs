using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGEngine.Quests
{
    public class ItemRequiredForStep
    {
        public BaseItem Item;
        public int Count;

        public ItemRequiredForStep(int itemID, int count)
        {
            this.Item = GameWorld.GetWorld().ItemList.Single(il => il.ID == itemID);
            this.Count = count;
        }
    }
}
