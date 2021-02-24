using System.Windows;
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
            PointsLabel.Text = $"{points} очков";
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
