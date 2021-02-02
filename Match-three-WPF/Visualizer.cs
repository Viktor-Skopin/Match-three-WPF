using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Match_three_NET.Framework;
using WpfAnimatedGif;

namespace Match_three_WPF
{
    public class Visualizer
    {
        Image[,] images;
        Button[,] buttons;

        Grid GameFieldControl;
        GameField GameField;
        int fieldSize;

        public Visualizer(Grid FieldGrid, int size)
        {
            GameFieldControl = FieldGrid;
            GameField = new GameField(size);
            fieldSize = size;

            images = new Image[size, size];
            buttons = new Button[size, size];

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    images[x, y] = new Image();
                    buttons[x, y] = new Button();
                    buttons[x, y].Content = images[x, y];
                    buttons[x, y].Click += CellClick;
                }
            }

            DefineImages();
            PrepareGrid();
        }

        public void DefineImages()
        {
            for (int x = 0; x < fieldSize; x++)
            {
                for (int y = 0; y < fieldSize; y++)
                {
                    DefineImage(GameField.cells[x, y], images[x, y]);                   
                }
            }
        }

        public void DefineImage(Cell cell, Image image)
        {
            BitmapImage BMI = new BitmapImage();
            BMI.BeginInit();
            BMI.UriSource = new Uri(GetFigurePath(cell.figure),UriKind.Relative);
            BMI.EndInit();
            image.Source = BMI;

            ImageBehavior.SetAnimatedSource(image, BMI);
            ImageBehavior.SetAnimationSpeedRatio(image, 0.2);
        }

        public string GetFigurePath(Figure figure)
        {
            switch (figure)
            {
                case Figure.Amethyst:
                    return "/Assets/Figures/Amethyst.gif";
                case Figure.Citrine:
                    return "/Assets/Figures/Citrine.gif";
                case Figure.Diamond:
                    return "/Assets/Figures/Diamond.gif";
                case Figure.Emerald:
                    return "/Assets/Figures/Emerald.gif";
                case Figure.Ruby:
                    return "/Assets/Figures/Ruby.gif";
                case Figure.Topaz:
                    return "/Assets/Figures/Topaz.gif";
                default:
                    throw new Exception("Ссылка на объект не может быть получена");
            }
        }

        public void PrepareGrid()
        {
            for (int i = 0; i < fieldSize; i++)
            {
                GameFieldControl.ColumnDefinitions.Add(new ColumnDefinition());
                GameFieldControl.RowDefinitions.Add(new RowDefinition());
            }

            for (int x = 0; x < fieldSize; x++)
            {
                for (int y = 0; y < fieldSize; y++)
                {
                    Grid.SetColumn(buttons[x, y], x);
                    Grid.SetRow(buttons[x, y], y);
                    images[x, y].Stretch = Stretch.Uniform;
                    GameFieldControl.Children.Add(buttons[x, y]);
                }
            }
        }

        public void CellClick(object sender, EventArgs e)
        {
            int x = Grid.GetColumn(sender as Button);
            int y = Grid.GetRow(sender as Button);

            var animation = new DoubleAnimation();
            animation.From = 1.0;
            animation.To = 0.0;
            animation.Duration = TimeSpan.FromSeconds(0.5);
            animation.AutoReverse = true;
            animation.RepeatBehavior = RepeatBehavior.Forever;
            images[x,y].BeginAnimation(UIElement.OpacityProperty, animation);
        }
    }
}
