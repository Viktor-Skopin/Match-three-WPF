using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using Match_three_NET.Framework;

namespace Match_three_WPF
{
    public class ButtonsController
    {
        public Button Bomb { get; set; }
        public Button Mix { get; set; }
        public Button HorizontalSlash { get; set; }
        public Button VerticalSlash { get; set; }
        public Button Diamondization { get; set; }
        public Button Pick { get; set; }

        private Thickness DefoltMargin = new Thickness(2, 2, 2, 2);

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

        private void Hide(Button button)
        {
            button.IsEnabled = false;
            button.Visibility = Visibility.Hidden;
            button.Margin = DefoltMargin;
        }

        private void Show(Button button)
        {
            button.IsEnabled = true;
            button.Visibility = Visibility.Visible;
        }
    }
}
