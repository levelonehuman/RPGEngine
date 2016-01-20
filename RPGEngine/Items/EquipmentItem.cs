using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGEngine
{
    public class EquipmentItem : BaseItem
    {
        public Stats Stats;
        public Enums.ArmorLocations Location;

        //Add stats to constructor
        public EquipmentItem(Guid id, string name, int value, string armorLocation) : base(id, name, value, canEquip: true)
        {
            Location = GetArmorLocation(armorLocation);
        }

        private Enums.ArmorLocations GetArmorLocation(string armorLocation)
        {
            try
            {
                 return (Enums.ArmorLocations)Enum.Parse(typeof(Enums.ArmorLocations), armorLocation);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
