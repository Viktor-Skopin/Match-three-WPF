using System;
using System.Windows;

namespace Match_three_WPF
{
    /// <summary>
    /// Логика взаимодействия для GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        /// <summary>
        /// Игра
        /// </summary>
        Visualizer MatchThree;
        /// <summary>
        /// Таймер
        /// </summary>
        GameTimer timer;
        public GameWindow()
        {
            InitializeComponent();

            MatchThree = new Visualizer(GameFieldGrid, 10, PointsLabel, AddedLabel);

            MatchThree.ButtonController.Mix = MixSpellButton;
            MatchThree.ButtonController.Pick = PickSpellButton;
            MatchThree.ButtonController.HorizontalSlash = HorizontalSlashSpellButton;
            MatchThree.ButtonController.VerticalSlash = VerticalSlashSpellButton;
            MatchThree.ButtonController.Bomb = BombSpellButton;
            MatchThree.ButtonController.Diamondization = DiamondizationSpellButton;

            MatchThree.ButtonController.UpdateStatus(MatchThree.GameField);

            MatchThree.Leaderboard.SetConrols(LeaderLabel, LeaderProgress, LeaderPointsLabel, PercentLabel);

            timer = new GameTimer(300, TimeLabel, TimePB);
            timer.DispatcherTimer.Tick += TimerTick;
            timer.Start();
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DisableAllSpells()
        {
            Thickness thickness = new Thickness(2, 2, 2, 2);

            VerticalSlashSpellButton.Margin = thickness;
            HorizontalSlashSpellButton.Margin = thickness;
            BombSpellButton.Margin = thickness;
            MixSpellButton.Margin = thickness;
            DiamondizationSpellButton.Margin = thickness;
            PickSpellButton.Margin = thickness;
        }

        private void HorizontalSlashSpellButton_Click(object sender, RoutedEventArgs e)
        {
            if(MatchThree.ChosenSpell == Spells.HorizontalSlash)
            {
                MatchThree.ChosenSpell = Spells.None;
                HorizontalSlashSpellButton.Margin = new Thickness(2, 2, 2, 2);
            }
            else
            {
                MatchThree.ChosenSpell = Spells.HorizontalSlash;
                DisableAllSpells();
                HorizontalSlashSpellButton.Margin = new Thickness(5, 5, 5, 5);
            }
        }

        private void VerticalSlashSpellButton_Click(object sender, RoutedEventArgs e)
        {
            if (MatchThree.ChosenSpell == Spells.VerticalSlash)
            {
                MatchThree.ChosenSpell = Spells.None;
                VerticalSlashSpellButton.Margin = new Thickness(2, 2, 2, 2);
            }
            else
            {
                MatchThree.ChosenSpell = Spells.VerticalSlash;
                DisableAllSpells();
                VerticalSlashSpellButton.Margin = new Thickness(5, 5, 5, 5);
            }
        }

        private async void MixSpellButton_Click(object sender, RoutedEventArgs e)
        {

            if (MatchThree.ChosenSpell != Spells.Mix)
            {
                DisableAllSpells();
            }

            await MatchThree.Mix();
        }

        private void PickSpellButton_Click(object sender, RoutedEventArgs e)
        {
            if (MatchThree.ChosenSpell == Spells.Pick)
            {
                MatchThree.ChosenSpell = Spells.None;
                PickSpellButton.Margin = new Thickness(2, 2, 2, 2);
            }
            else
            {
                MatchThree.ChosenSpell = Spells.Pick;
                DisableAllSpells();
                PickSpellButton.Margin = new Thickness(5, 5, 5, 5);
            }
        }

        private void DiamondizationSpellButton_Click(object sender, RoutedEventArgs e)
        {
            if (MatchThree.ChosenSpell == Spells.Diamondization)
            {
                MatchThree.ChosenSpell = Spells.None;
                DiamondizationSpellButton.Margin = new Thickness(2, 2, 2, 2);
            }
            else
            {
                MatchThree.ChosenSpell = Spells.Diamondization;
                DisableAllSpells();
                DiamondizationSpellButton.Margin = new Thickness(5, 5, 5, 5);
            }
        }

        private void BombSpellButton_Click(object sender, RoutedEventArgs e)
        {
            if (MatchThree.ChosenSpell == Spells.Bomb)
            {
                MatchThree.ChosenSpell = Spells.None;
                BombSpellButton.Margin = new Thickness(2, 2, 2, 2);
            }
            else
            {
                MatchThree.ChosenSpell = Spells.Bomb;
                DisableAllSpells();
                BombSpellButton.Margin = new Thickness(5, 5, 5, 5);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            LeaderboardWindow LW = new LeaderboardWindow(MatchThree.Leaderboard.board.Players);
            LW.ShowDialog();
            timer.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (timer.timeSeconds > 0)
            {
                timer.timeSeconds--;

                timer.PB.Value = timer.timeSeconds;

                timer.timeSpan = TimeSpan.FromSeconds(timer.timeSeconds);

                timer.GameLabel.Content = timer.timeSpan.ToString("mm' : 'ss");
            }
            else
            {
                timer.Stop();
                GameOwerWindow GW = new GameOwerWindow(MatchThree.GameField.Points);
                GW.ShowDialog();

                MatchThree.NewGame();

                MatchThree.ButtonController.Mix = MixSpellButton;
                MatchThree.ButtonController.Pick = PickSpellButton;
                MatchThree.ButtonController.HorizontalSlash = HorizontalSlashSpellButton;
                MatchThree.ButtonController.VerticalSlash = VerticalSlashSpellButton;
                MatchThree.ButtonController.Bomb = BombSpellButton;
                MatchThree.ButtonController.Diamondization = DiamondizationSpellButton;

                MatchThree.ButtonController.UpdateStatus(MatchThree.GameField);

                timer = new GameTimer(300, TimeLabel, TimePB);
                timer.DispatcherTimer.Tick += TimerTick;
                timer.Start();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            MatchThree.NewGame();

            MatchThree.ButtonController.Mix = MixSpellButton;
            MatchThree.ButtonController.Pick = PickSpellButton;
            MatchThree.ButtonController.HorizontalSlash = HorizontalSlashSpellButton;
            MatchThree.ButtonController.VerticalSlash = VerticalSlashSpellButton;
            MatchThree.ButtonController.Bomb = BombSpellButton;
            MatchThree.ButtonController.Diamondization = DiamondizationSpellButton;

            MatchThree.ButtonController.UpdateStatus(MatchThree.GameField);

            timer = new GameTimer(300, TimeLabel, TimePB);
            timer.DispatcherTimer.Tick += TimerTick;
            timer.Start();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            GuideWindow GW = new GuideWindow();
            GW.ShowDialog();
            timer.Start();
        }
    }
}
