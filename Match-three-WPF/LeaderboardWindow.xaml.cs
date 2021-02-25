using System.Collections.Generic;
using System.Windows;
using Match_three_NET.Framework;

namespace Match_three_WPF
{
    /// <summary>
    /// Логика взаимодействия для LeaderboardWindow.xaml
    /// </summary>
    public partial class LeaderboardWindow : Window
    {

        List<PlayerAccount> PlayerAccounts;
        public LeaderboardWindow(List<PlayerAccount> accounts)
        {
            InitializeComponent();

            PlayerAccounts = accounts;

            ScoreList.ItemsSource = PlayerAccounts;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
