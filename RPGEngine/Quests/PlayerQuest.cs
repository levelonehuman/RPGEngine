using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGEngine.Quests
{
    /// <summary>
    /// Represents a quest undertaken by the player. Holds data for Quest (along with rewards, steps, etc.) and a bool identifying completion status.
    /// </summary>
    public class PlayerQuest
    {
        public Quest Quest { get; private set; }
        public bool IsCompleted
        {
            get { return Quest.Steps.All(qs => qs.IsStepCompleted == true); }
        }

        public PlayerQuest(Guid questID)
        {
            this.Quest = GameWorld.GetWorld().QuestList.Single(ql => ql.ID == questID);
        }
    }
}
