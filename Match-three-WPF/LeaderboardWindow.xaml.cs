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

    }
}
