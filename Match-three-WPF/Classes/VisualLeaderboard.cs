using Match_three_NET.Framework;
using System;
using System.Windows.Controls;

namespace Match_three_WPF
{
    /// <summary>
    /// Визуальное отображение таблицы лидеров
    /// </summary>
    public class VisualLeaderboard
    {
        public Leaderboard board = new Leaderboard();
        public Label LeaderLabel;
        public ProgressBar Progress;
        public Label PointsLeft;
        public Label Percents;
        /// <summary>
        /// Устонавить контролы дляотображения информации
        /// </summary>
        public void SetConrols(Label leaderLabel, ProgressBar progress, Label pointsLeft, Label percent)
        {
            LeaderLabel = leaderLabel;
            Progress = progress;
            PointsLeft = pointsLeft;
            Percents = percent;
        }
        /// <summary>
        /// Обновление информации
        /// </summary>
        public void UpdateInfo(int currentPoints)
        {

            board.FindLeader(currentPoints);
            board.GetLeaderInfo(out string leader, out int leaderPoints);

            LeaderLabel.Content = leader;

            PointsLeft.Content = $"Осталось: {leaderPoints - currentPoints}";

            Progress.Maximum = leaderPoints;
            Progress.Minimum = 0;
            Progress.Value = currentPoints;

            double lp = leaderPoints;
            double cp = currentPoints;
            double result = ((double)currentPoints / (double)leaderPoints) * 100;


            Percents.Content = $"{Math.Round(result,1)}%";
        }
    }
}
