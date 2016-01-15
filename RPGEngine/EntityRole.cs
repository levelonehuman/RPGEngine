using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGEngine
{
    /// <summary>
    /// references an Entity's RPG class (Warrior, Archer, etc). Currently this only deals with base modifiers,
    /// but may be expanded to include abilities and other role-specific functions.
    /// </summary>
    public class EntityRole
    {
        public string Name { get; set; }
        public double DamageModifier { get; private set; }
        public double HealthModifier { get; private set; }
        public double DefenseModifier { get; private set; }

        public EntityRole(string name, double dmgMod, double hpMod, double defMod)
        {
            this.Name = name;
            this.DamageModifier = dmgMod;
            this.HealthModifier = hpMod;
            this.DefenseModifier = defMod;
        }

        public static EntityRole CreateRole(string roleName)
        {
            switch (roleName)
            {
                case "Archer":
                    return ArcherRole();
                case "Mage":
                    return MageRole();
                case "Warrior":
                    return WarriorRole();
                default:
                    throw new ArgumentException();
            }
        }

        private static EntityRole ArcherRole()
        {
            string name = "Archer";
            double dmgMod = 1.1;
            double hpMod = 0.9;
            double defMod = 0.9;

            return new EntityRole(name, dmgMod, hpMod, defMod);
        }

        private static EntityRole MageRole()
        {
            string name = "Mage";
            double dmgMod = 1.2;
            double hpMod = 0.8;
            double defMod = 0.7;

            return new EntityRole(name, dmgMod, hpMod, defMod);
        }

        private static EntityRole WarriorRole()
        {
            string name = "Warrior";
            double dmgMod = 1.0;
            double hpMod = 1.0;
            double defMod = 1.2;

            return new EntityRole(name, dmgMod, hpMod, defMod);
        }
    }
}
