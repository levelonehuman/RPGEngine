using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGEngine
{
    public class BaseItem
    {
        public int ID { get; private set; }
        public string Name { get; private set; }        
        public int Value { get; private set; }
        public bool CanEquip { get; private set; }

        public BaseItem(int id, string name, int value, bool canEquip)
        {
            this.ID = id;
            this.Name = name;
            this.Value = value;
            this.CanEquip = canEquip;
        }        
    }
}
