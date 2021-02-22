using System.Windows;

namespace Match_three_WPF
{
    /// <summary>
    /// Логика взаимодействия для GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        Visualizer MatchThree;
        public GameWindow()
        {
            InitializeComponent();

            MatchThree = new Visualizer(GameFieldGrid, 10, PointsLabel, AddedLabel);

            MatchThree.BC.Mix = MixSpellButton;
            MatchThree.BC.Pick = PickSpellButton;
            MatchThree.BC.HorizontalSlash = HorizontalSlashSpellButton;
            MatchThree.BC.VerticalSlash = VerticalSlashSpellButton;
            MatchThree.BC.Bomb = BombSpellButton;
            MatchThree.BC.Diamondization = DiamondizationSpellButton;

            MatchThree.BC.UpdateStatus(MatchThree.GameField);

            GameTimer timer = new GameTimer(300, TimeLabel, TimePB);
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
    }
}
