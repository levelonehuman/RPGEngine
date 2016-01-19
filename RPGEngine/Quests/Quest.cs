using RPGEngine.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGEngine.Quests
{
    /// <summary>
    /// Parent Quest object. Holds values for required level, prerequisites, steps, and rewards.
    /// A quest is completed when all steps are complete.
    /// </summary>
    public class Quest
    {
        public int ID { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }    
        public int MinimumLevel { get; private set; }
        public bool IsRepeatable { get; private set; }
        public List<QuestStep> Steps { get; private set; }
        public QuestReward Reward { get; private set; }
                
        public Quest(int id)
        {
            Quest _quest = GameWorld.GetWorld().QuestList.Single(ql => ql.ID == id);

            this.ID = _quest.ID;
            this.Name = _quest.Name;
            this.Description = _quest.Description;
            this.MinimumLevel = _quest.MinimumLevel;
            this.IsRepeatable = _quest.IsRepeatable;
            this.Steps = _quest.Steps;
            this.Reward = _quest.Reward;
        }

        public Quest(int id, string name, string description, int minimumLevel, bool isRepeatable, List<QuestStep> steps, QuestReward reward)
        {
            this.ID = id;
            this.Name = name;
            this.Description = description;
            this.MinimumLevel = minimumLevel;
            this.IsRepeatable = isRepeatable;
            this.Steps = steps;
            this.Reward = reward;
        }
    }
}
