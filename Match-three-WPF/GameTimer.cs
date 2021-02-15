using System;
using System.Windows.Threading;
using System.Windows.Controls;

namespace Match_three_WPF
{
    public class GameTimer
    {
        private DispatcherTimer DispatcherTimer;
        private Label GameLabel;
        ProgressBar PB;

        public GameTimer(int seconds, Label label, ProgressBar progressBar)
        {
            DispatcherTimer = new DispatcherTimer();
            DispatcherTimer.Tick += TimerTick;
            DispatcherTimer.Interval = new TimeSpan(0, 0, 1);

            GameLabel = label;
            PB = progressBar;
            progressBar.Maximum = seconds;
            progressBar.Value = seconds;
        }

        private void TimerTick(object sender, EventArgs e)
        {
            PB.Value--;
        }

        public void Start()
        {
            DispatcherTimer.Start();
        }

        public void Stop()
        {

        }
    }
}
