using System.Windows.Controls;
using System.Windows;
using Match_three_NET.Framework;

namespace Match_three_WPF
{
    /// <summary>
    /// Класс управления кнопками в окне
    /// </summary>
    public class ButtonsController
    {
        /// <summary>
        /// Кнопка бомбы
        /// </summary>
        public Button Bomb { get; set; }
        /// <summary>
        /// Кнопка перемешивания
        /// </summary>
        public Button Mix { get; set; }
        /// <summary>
        /// Кнопка горизонтали
        /// </summary>
        public Button HorizontalSlash { get; set; }
        /// <summary>
        /// Кнопка вертикали
        /// </summary>
        public Button VerticalSlash { get; set; }
        /// <summary>
        /// Кнопка алмазизации
        /// </summary>
        public Button Diamondization { get; set; }
        /// <summary>
        /// Кнопка кирки
        /// </summary>
        public Button Pick { get; set; }
        /// <summary>
        /// Стандартный отступ кнопок
        /// </summary>
        private Thickness DefoltMargin = new Thickness(2, 2, 2, 2);
        /// <summary>
        /// Обновление информации о состоянии кнопок, изходя из данных GameField
        /// </summary>
        public void UpdateStatus(GameField game)
        {
            if (game.IsBombAviable)
            {
                Show(Bomb);
            }
            else
            {
                Hide(Bomb);
            }

            if (game.IsMixAviable)
            {
                Show(Mix);
            }
            else
            {
                Hide(Mix);
            }

            if (game.IsHorizontalSlashAviable)
            {
                Show(HorizontalSlash);
            }
            else
            {
                Hide(HorizontalSlash);
            }

            if (game.IsVerticalSlashAviable)
            {
                Show(VerticalSlash);
            }
            else
            {
                Hide(VerticalSlash);
            }

            if (game.IsPickAviable)
            {
                Show(Pick);
            }
            else
            {
                Hide(Pick);
            }

            if (game.IsDiamondizationAviable)
            {
                Show(Diamondization);
            }
            else
            {
                Hide(Diamondization);
            }
        }
        /// <summary>
        /// Скрытие кнопки
        /// </summary>
        private void Hide(Button button)
        {
            button.IsEnabled = false;
            button.Visibility = Visibility.Hidden;
            button.Margin = DefoltMargin;
        }
        /// <summary>
        /// Отображение кнопки
        /// </summary>
        private void Show(Button button)
        {
            button.IsEnabled = true;
            button.Visibility = Visibility.Visible;
        }
    }
}
