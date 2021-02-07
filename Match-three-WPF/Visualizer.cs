﻿using System;
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
        /// <summary>
        /// Изображения
        /// </summary>
        Image[,] images;
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
        GameField GameField;
        /// <summary>
        /// Размер игрового поля
        /// </summary>
        int fieldSize;
        /// <summary>
        /// Изображение, которое анимируется в данный момент
        /// </summary>
        Image AnimatedImage;
        /// <summary>
        /// Выбрана ли какая либо ячейка
        /// </summary>
        bool IsSomeSelected;
        /// <summary>
        /// Координата X выбранной ячейки
        /// </summary>
        int selectedX;
        /// <summary>
        /// Координата Y выбранной ячейки
        /// </summary>
        int selectedY;

        /// <summary>
        /// Происходит ли в данный момент анимация
        /// </summary>
        bool isAnimationGoing = false;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="FieldGrid">Игровое поле</param>
        /// <param name="size">Размер игрового поля</param>
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

        /// <summary>
        /// Присвоение всем ячейкам соответствующего изображения
        /// </summary>
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

        /// <summary>
        /// Присвоение ячейки соответствующего изображения
        /// </summary>
        /// <param name="cell">Логическая ячейка</param>
        /// <param name="image">Графическая ячейка</param>
        async public void DefineImage(Cell cell, Image image)
        {
            if(cell.figure == Figure.Empty)
            {                
                StartImageAnimation(image, Animations.Disappearance);
                await Task.Delay(500);
                image.Source = null;
                ImageBehavior.SetAnimatedSource(image, null);
            }
            else
            {
                BitmapImage BMI = new BitmapImage();
                BMI.BeginInit();
                BMI.UriSource = new Uri(GetFigurePath(cell.figure), UriKind.Relative);
                BMI.EndInit();
                image.Source = BMI;

                ImageBehavior.SetAnimatedSource(image, BMI);
                ImageBehavior.SetAnimationSpeedRatio(image, 0.2);

                StartImageAnimation(image, Animations.Appearance);
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
            for (int i = 0; i < fieldSize; i++)
            {
                GameFieldControl.ColumnDefinitions.Add(new ColumnDefinition());
                GameFieldControl.RowDefinitions.Add(new RowDefinition());
            }

            //Присвоение кнопок гриду
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

        /// <summary>
        /// Назатие на графическую ячейку
        /// </summary>
        async public void CellClick(object sender, EventArgs e)
        {
            //Извлечение координат
            int x = Grid.GetColumn(sender as Button);
            int y = Grid.GetRow(sender as Button);
            if(isAnimationGoing == false)
            {
                //Первое нажатие
                if (IsSomeSelected == false)
                {
                    //Логика
                    GameField.SelectCell(x, y);
                    IsSomeSelected = true;

                    selectedX = x;
                    selectedY = y;
                    //Анимация
                    StartImageAnimation(images[x, y], Animations.Selection);
                    AnimatedImage = images[x, y];
                }
                //Второе нажатие
                else
                {
                    if (IsSamePlase(x, y))
                    {
                        GameField.UnselectCell();
                        IsSomeSelected = false;

                        StopImageAnimation(AnimatedImage);
                        AnimatedImage = null;
                    }
                    else
                    {
                        if (IsNeighbors(x, y))
                        {
                            isAnimationGoing = true;

                            //Снятие выделения
                            StopImageAnimation(AnimatedImage);
                            IsSomeSelected = false;

                            //Логическая замена ячеек
                            GameField.SwapCells(GameField.cells[x, y], GameField.cells[selectedX, selectedY]);                

                            //Графическая замена ячеек
                            StartImageAnimation(AnimatedImage, Animations.Disappearance);
                            StartImageAnimation(images[x, y], Animations.Disappearance);
                            await Task.Delay(500);

                            DefineImage(GameField.cells[x, y], images[x, y]);
                            DefineImage(GameField.cells[selectedX, selectedY], images[selectedX, selectedY]);

                            StartImageAnimation(AnimatedImage, Animations.Appearance);
                            StartImageAnimation(images[x, y], Animations.Appearance);
                            await Task.Delay(500);

                            StopImageAnimation(AnimatedImage);
                            StopImageAnimation(images[x, y]);

                            GameField.CheckAllCells();
                            GameField.FillVoids();

                            DefineImages();

                            AnimatedImage = null;

                            

                            isAnimationGoing = false;
                        }
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

        public bool IsSamePlase(int x,int y)
        {

            if (selectedX == x && selectedY == y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsNeighbors(int x, int y)
        {
            bool left = true;
            bool right = true;
            bool up = true;
            bool down = true;

            //Left
            if (x - 1 >= 0)
            {
                if (x - 1 != selectedX) left = false;
            }
            else left = false;
            //Right
            if (x + 1 <= 9)
            {
                if (x + 1 != selectedX) right = false;
            }
            else right = false;
            //Up
            if (y - 1 >= 0)
            {
                if (y - 1 != selectedY) up = false;
            }
            else up = false;
            //Down
            if (y + 1 <= 9)
            {
                if (y + 1 != selectedY) down = false;
            }
            else down = false;

            if ((up && x == selectedX) || (down && x == selectedX) || (left && y == selectedY) || (right && y == selectedY))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        async public void SwitchImages(Image I1, Image I2)
        {

            StartImageAnimation(I1, Animations.Disappearance);
            StartImageAnimation(I2, Animations.Disappearance);
            await Task.Delay(500);
            StartImageAnimation(I1, Animations.Appearance);
            StartImageAnimation(I2, Animations.Appearance);
            await Task.Delay(500);

        }
    }
}
