using RPGEngine.Items;
using RPGEngine.Quests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGEngine
{
    public class Player : Entity
    {
        public List<InventoryItem> Inventory;
        public List<PlayerQuest> Quests;        
        public int Gold;        
        public int Experience
        {
            get { return _experience; }
            set
            {
                if (value != _experience)
                {
                    _experience = value;
                    OnPropertyChanged("Experience");
                }
            }
        }

        private static Player player;
        private int _experience;          
        
        public static Player GetPlayer()
        {
            if (player == null)
            {
                player = new Player("TestPlayer", "Mage");
            }

            return player;
        }

        private Player(string name, string role) : base(name, 1, role)
        {
            this._experience = GameWorld.GetWorld().LevelTable.First(lt => lt.Key == this.Level).Value;
            this.Gold = 0;
            this.PropertyChanged += Player_PropertyChanged;            
        }        

        private void Player_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Experience")
            {
                this.Level = GetPlayerLevel();
            }            
        }

        public int GetPlayerLevel()
        {
            return GameWorld.GetWorld().LevelTable.LastOrDefault(lt => lt.Value <= this.Experience).Key;
        }            
    }
}
