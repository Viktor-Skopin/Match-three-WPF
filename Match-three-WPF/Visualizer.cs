using Match_three_NET.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using WpfAnimatedGif;

namespace Match_three_WPF
{
    public class Visualizer
    {
        /// <summary>
        /// Изображения
        /// </summary>
        MTImage[,] images;
        /// <summary>
        /// Кнопки
        /// </summary>
        Button[,] buttons;
        /// <summary>
        /// Графическое игровое поле
        /// </summary>
        Grid GameFieldControl;
        /// <summary>
        /// Логической егровое поле
        /// </summary>
        private GameField GameField;
        /// <summary>
        /// Изображение, которое анимируется в данный момент
        /// </summary>
        private MTImage AnimatedImage;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="FieldGrid">Игровое поле</param>
        /// <param name="size">Размер игрового поля</param>
        public Visualizer(Grid FieldGrid, int size)
        {
            GameFieldControl = FieldGrid;
            GameField = new GameField(size);

            images = new MTImage[size, size];
            buttons = new Button[size, size];

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    images[x, y] = new MTImage()
                    {
                        X = x,
                        Y = y,
                        figure = GameField.cells[x, y].figure
                    };
                    buttons[x, y] = new Button();
                    buttons[x, y].Content = images[x, y];
                    buttons[x, y].Click += CellClick;
                }
            }

            DefineAllImages();
            PrepareGrid();
        }

        /// <summary>
        /// Присвоение всем ячейкам соответствующего изображения
        /// </summary>
        public void DefineAllImages()
        {
            for (int x = 0; x < GameField.fieldSize; x++)
            {
                for (int y = 0; y < GameField.fieldSize; y++)
                {
                    DefineImage(images[x, y]);
                }
            }
        }

        public void DefineChangedImages()
        {
            for (int x = 0; x < GameField.fieldSize; x++)
            {
                for (int y = 0; y < GameField.fieldSize; y++)
                {
                    if (GameField.cells[x, y].IsChanged)
                    {
                        DefineImage(images[x, y]);
                    }                    
                }
            }
        }

        /// <summary>
        /// Присвоение ячейки соответствующего изображения
        /// </summary>
        /// <param name="cell">Логическая ячейка</param>
        /// <param name="image">Графическая ячейка</param>
        public void DefineImage(MTImage image)
        {
            int X = image.X;
            int Y = image.Y;

            if (GameField.cells[X, Y].figure == Figure.Empty)
            {
                image.Source = null;
                ImageBehavior.SetAnimatedSource(image, null);
            }
            else
            {
                BitmapImage BMI = new BitmapImage();
                BMI.BeginInit();
                BMI.UriSource = new Uri(GetFigurePath(GameField.cells[X, Y].figure), UriKind.Relative);
                BMI.EndInit();
                image.Source = BMI;

                ImageBehavior.SetAnimatedSource(image, BMI);
                ImageBehavior.SetAnimationSpeedRatio(image, 0.2);
            }
        }

        /// <summary>
        /// Возвращает относительный путь к изображению соответствующей фигурки
        /// </summary>
        /// <param name="figure">Фигурка</param>
        /// <returns></returns>
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

        /// <summary>
        /// Подготовка графического игрового поля для дальнейшей работы с ним
        /// </summary>
        public void PrepareGrid()
        {
            //Разбиение грида на столбцы и строки
            for (int i = 0; i < GameField.fieldSize; i++)
            {
                GameFieldControl.ColumnDefinitions.Add(new ColumnDefinition());
                GameFieldControl.RowDefinitions.Add(new RowDefinition());
            }

            //Присвоение кнопок гриду
            for (int x = 0; x < GameField.fieldSize; x++)
            {
                for (int y = 0; y < GameField.fieldSize; y++)
                {
                    Grid.SetColumn(buttons[x, y], x);
                    Grid.SetRow(buttons[x, y], y);
                    images[x, y].Stretch = Stretch.Uniform;
                    GameFieldControl.Children.Add(buttons[x, y]);
                }
            }
        }

        /// <summary>
        /// Нажатие на графическую ячейку
        /// </summary>
        public void CellClick(object sender, EventArgs e)
        {
            //Извлечение координат
            int x = Grid.GetColumn(sender as Button);
            int y = Grid.GetRow(sender as Button);


            //Первое нажатие
            if (GameField.IsSomeSelected == false)
            {
                //Логика
                GameField.SelectCell(x, y);
                GameField.IsSomeSelected = true;

                //Анимация
                StartImageAnimation(images[x, y], Animations.Selection);
                AnimatedImage = images[x, y];
            }
            //Второе нажатие
            else
            {
                if (GameField.IsSamePlase(x, y))
                {
                    GameField.UnselectCell();
                    GameField.IsSomeSelected = false;

                    StopImageAnimation(AnimatedImage);
                    AnimatedImage = null;
                }
                else
                {
                    if (GameField.IsNeighbors(x, y))
                    {
                        //Снятие выделения
                        StopImageAnimation(AnimatedImage);
                        GameField.IsSomeSelected = false;

                        //Логическая замена ячеек
                        GameField.SwapCells(GameField.cells[x, y], GameField.cells[AnimatedImage.X, AnimatedImage.Y]);

                        //Проверка на ложное нажатие
                        if (GameField.IsFalseMove())
                        {
                            GameField.SwapCells(GameField.cells[x, y], GameField.cells[AnimatedImage.X, AnimatedImage.Y]);
                            GameField.IsSomeSelected = false;

                            StopImageAnimation(AnimatedImage);
                            StopImageAnimation(images[x, y]);

                            return;
                        }


                        //Графическая замена ячеек
                        DefineImage(images[x, y]);
                        DefineImage(images[AnimatedImage.X, AnimatedImage.Y]);

                        StopImageAnimation(AnimatedImage);
                        StopImageAnimation(images[x, y]);

                        GameField.CheckAllCells();

                        GameField.FillVoids();
                        DefineChangedImages();

                        AnimatedImage = null;
                    }
                }
            }
        }

        /// <summary>
        /// Начало анимации выбора в указанной ячейке
        /// </summary>
        /// <param name="image">Ячейка</param>
        public void StartImageAnimation(Image image, DoubleAnimation doubleAnimation)
        {
            image.BeginAnimation(UIElement.OpacityProperty, doubleAnimation);
        }

        /// <summary>
        /// Завершает анимацию выбранной ячейки
        /// </summary>
        /// <param name="image">Ячейка</param>
        public void StopImageAnimation(Image image)
        {
            image.BeginAnimation(UIElement.OpacityProperty, null);
        }
    }
}
