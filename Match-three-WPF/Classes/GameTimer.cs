using System;
using System.Windows.Threading;
using System.Windows.Controls;

namespace Match_three_WPF
{
    /// <summary>
    /// Таймер
    /// </summary>
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
        /// <summary>
        /// Количество секунд, оставшихся до конца игры
        /// </summary>
        public int timeSeconds;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="seconds">Количество секунд до конца игры</param>
        /// <param name="label">Label для вывода оставшегося времени</param>
        /// <param name="progressBar">Progress Bar для вывода оставшегося времени</param>
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
        /// <summary>
        /// Запуск таймера
        /// </summary>
        public void Start()
        {
            DispatcherTimer.Start();
        }
        /// <summary>
        /// Остановка таймера
        /// </summary>
        public void Stop()
        {
            DispatcherTimer.Stop();
        }
    }
}
