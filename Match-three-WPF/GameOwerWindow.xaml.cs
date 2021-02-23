using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Match_three_NET.Framework;

namespace Match_three_WPF
{
    /// <summary>
    /// Логика взаимодействия для GameOwerWindow.xaml
    /// </summary>
    public partial class GameOwerWindow : Window
    {
        int Points;
        public GameOwerWindow(int points)
        {
            InitializeComponent();
            Points = points;
        }

        private void PushButton_Click(object sender, RoutedEventArgs e)
        {
            Leaderboard lb = new Leaderboard();
            lb.Add(NameTextBox.Text, Points);
            lb.Sort();
            lb.Cut();
            lb.Serialize();
            Close();
        }
    }
}
