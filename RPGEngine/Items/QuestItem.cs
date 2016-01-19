using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGEngine.Items
{
    public class QuestItem : BaseItem
    {
        public QuestItem(int id, string name) : base(id, name, value: 0, canEquip: false) { }
    }
}
