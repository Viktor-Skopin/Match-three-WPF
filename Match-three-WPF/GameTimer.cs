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
        private DispatcherTimer DispatcherTimer;
        /// <summary>
        /// Label для вывода оставшегося времени
        /// </summary>
        private Label GameLabel;
        /// <summary>
        /// Progress Bar для вывода оставшегося времени
        /// </summary>
        ProgressBar PB;
        /// <summary>
        /// Оставшееся время
        /// </summary>
        TimeSpan timeSpan;
        int timeSeconds;

        public GameTimer(int seconds, Label label, ProgressBar progressBar)
        {
            DispatcherTimer = new DispatcherTimer();
            DispatcherTimer.Tick += TimerTick;
            DispatcherTimer.Interval = new TimeSpan(0, 0, 1);

            GameLabel = label;
            PB = progressBar;

            progressBar.Maximum = seconds;
            progressBar.Value = seconds;
            timeSpan = TimeSpan.FromSeconds(seconds);
            timeSeconds = seconds;
            GameLabel.Content = timeSpan.ToString("mm' : 'ss");
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if(timeSeconds > 0)
            {
                timeSeconds--;

                PB.Value = timeSeconds;

                timeSpan = TimeSpan.FromSeconds(timeSeconds);

                GameLabel.Content = timeSpan.ToString("mm' : 'ss");
            }
            else
            {
                Stop();
            }
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
