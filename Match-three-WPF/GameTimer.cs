using System;
using System.Windows.Threading;
using System.Windows.Controls;

namespace Match_three_WPF
{
    public class GameTimer
    {
        /// <summary>
        /// Таймер
        /// </summary>
        public DispatcherTimer DispatcherTimer;
        /// <summary>
        /// Label для вывода оставшегося времени
        /// </summary>
        public Label GameLabel;
        /// <summary>
        /// Progress Bar для вывода оставшегося времени
        /// </summary>
        public ProgressBar PB;
        /// <summary>
        /// Оставшееся время
        /// </summary>
        public TimeSpan timeSpan;
        public int timeSeconds;

        public GameTimer(int seconds, Label label, ProgressBar progressBar)
        {
            DispatcherTimer = new DispatcherTimer();
            DispatcherTimer.Interval = new TimeSpan(0, 0, 1);

            GameLabel = label;
            PB = progressBar;

            progressBar.Maximum = seconds;
            progressBar.Value = seconds;
            timeSpan = TimeSpan.FromSeconds(seconds);
            timeSeconds = seconds;
            GameLabel.Content = timeSpan.ToString("mm' : 'ss");
        }

        public void Start()
        {
            DispatcherTimer.Start();
        }

        public void Stop()
        {
            DispatcherTimer.Stop();
        }
    }
}
