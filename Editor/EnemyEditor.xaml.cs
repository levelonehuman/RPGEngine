using RPGEngine;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Editor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class EnemyEditor : Window
    {
        public ObservableCollection<Enemy> EnemyList;

        public EnemyEditor()
        {
            InitializeComponent();

            var data = JsonHelpers.ReadFileToCollection<Enemy>(GameWorld.ENEMY_FILE);
            EnemyList = data["Enemy"];
            EnemyGrid.DataContext = EnemyList;
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveEnemyList();
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            Enemy enemy = (sender as Button).DataContext as Enemy;
            EnemyList.Remove(enemy);

            SaveEnemyList();
        }        

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            string name = enemyNameTextBox.Text;
            int level = Convert.ToInt32(levelTextBox.Text);
            string role = roleComboBox.Text;

            if (!String.IsNullOrEmpty(name)
                && !String.IsNullOrEmpty(role)
                && (level > 0 && level <= 50))
            {
                Enemy enemy = new Enemy(name, level, role);

                if (IsDupeEnemy(enemy) == false)
                {
                    if (IsValidEnemy(enemy))
                    {
                        EnemyList.Add(enemy);
                        SaveEnemyList();
                    }
                    else
                    {
                        MessageBox.Show("Error creating enemy!");
                    }
                    
                }
                else
                {
                    MessageBox.Show("This enemy has already been added!");
                }                
            }
            else
            {
                MessageBox.Show("Invalid values! Unable to create enemy.");
            }

        }        

        private void EnemyGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //SaveEnemyList();
            Enemy enemy = (sender as DataGrid).SelectedItem as Enemy;

            if (enemy != null && IsValidEnemy(enemy))
            {
                SaveEnemyList();
            }                       
        }

        private bool IsDupeEnemy(Enemy enemy)
        {
            var result = EnemyList.FirstOrDefault(e => e.Name == enemy.Name
                                                    && e.Level == enemy.Level
                                                    && e.Role == enemy.Role);

            return result != null;
        }

        private bool IsValidEnemy(Enemy enemy)
        {
            bool[] validEnemy = new bool[]
            {
                enemy.Stats.Health >= 0,
                enemy.Stats.Damage >= 0,
                enemy.Stats.Defense >= 0,
                enemy.ExperienceGranted >= 0,
                enemy.GoldGranted >= 0,
                enemy.Level > 0 && enemy.Level <= 50,
                !String.IsNullOrEmpty(enemy.Name),
                !String.IsNullOrEmpty(enemy.Role)                
            };


            return !validEnemy.Any(b => b == false);
        }

        private void SaveEnemyList()
        {
            var data = JsonHelpers.ReadFileToCollection<Enemy>(GameWorld.ENEMY_FILE);
            data["Enemy"] = EnemyList;
            JsonHelpers.SaveFile(GameWorld.ENEMY_FILE, data);
        }        
    }    
}
