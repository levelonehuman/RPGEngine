using RPGEngine.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGEngine.Quests
{
    /// <summary>
    /// Holds one step to a quest, including the name, description and completion requirements. The quest object contains a list of these.
    /// </summary>
    public class QuestStep
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public List<ItemRequiredForStep> RequiredItems { get; private set; }
        public List<EnemyRequiredForStep> RequiredKills { get; private set; }        
        public QuestStepReward StepReward { get; private set; }
        public bool IsStepCompleted
        {
            get { return PlayerHasRequiredItems() && PlayerHasRequiredKillCount(); }
        }

        public QuestStep(string name, string description, List<ItemRequiredForStep> stepItems = null, List<EnemyRequiredForStep> stepKills = null, QuestStepReward stepReward = null)
        {
            this.Name = name;
            this.Description = description;
            this.RequiredItems = stepItems ?? new List<ItemRequiredForStep>();
            this.RequiredKills = stepKills ?? new List<EnemyRequiredForStep>();
            this.StepReward = stepReward ?? new QuestStepReward();
        }

        private bool PlayerHasRequiredItems()
        {
            //if the RequiredItems list is empty 
            if (RequiredItems.Count == 0)
            {
                return true;
            }

            foreach (var item in RequiredItems)
            {
                //determine if player has the required item and correct number of items                
                bool hasRequiredItem = Player.GetPlayer().Inventory.Any(pi => pi.Item.ID == item.Item.ID && pi.Count >= item.Count);

                if (hasRequiredItem == false)
                {
                    return false;
                }
            }

            return true;
        }

        private bool PlayerHasRequiredKillCount()
        {
            if (RequiredKills.Count == 0)
            {
                return true;
            }

            foreach (var enemy in RequiredKills)
            {
                if (enemy.KilledByPlayer < enemy.RequiredKillCount)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
