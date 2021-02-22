using Match_three_NET.Framework;
using System;
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
        private MTImage[,] Images;
        /// <summary>
        /// Кнопки
        /// </summary>
        private Button[,] Buttons;
        /// <summary>
        /// Графическое игровое поле
        /// </summary>
        private Grid GameFieldControl;
        /// <summary>
        /// Логической егровое поле
        /// </summary>
        public GameField GameField;
        /// <summary>
        /// Изображение, которое анимируется в данный момент
        /// </summary>
        private MTImage AnimatedImage;
        /// <summary>
        /// Графический элемент, отображающий кол-во очков
        /// </summary>
        private Label Points;

        private Label PointsCounter;
        /// <summary>
        /// Происходит ли анимация в данный момент
        /// </summary>
        private bool IsAnimationGoing = false;

        public ButtonsController BC;

        public Spells ChosenSpell;
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="FieldGrid">Игровое поле</param>
        /// <param name="size">Размер игрового поля</param>
        public Visualizer(Grid FieldGrid, int size, Label pointsLabel, Label pointsCounterLabel)
        {
            GameFieldControl = FieldGrid;
            GameField = new GameField(size);
            Points = pointsLabel;
            PointsCounter = pointsCounterLabel;
            ChosenSpell = Spells.None;

            Images = new MTImage[size, size];
            Buttons = new Button[size, size];

            BC = new ButtonsController();

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    Images[x, y] = new MTImage()
                    {
                        X = x,
                        Y = y,
                        figure = GameField.cells[x, y].figure
                    };
                    Buttons[x, y] = new Button()
                    {
                        Background = Brushes.Transparent

                    };
                    Buttons[x, y].Content = Images[x, y];
                    Buttons[x, y].Click += CellClick;
                }
            }

            DefineAllImages();
            PrepareGrid();
        }
        /// <summary>
        /// Обновление счётчика очков
        /// </summary>
        private void UpdatePointsLabel()
        {
            Points.Content = GameField.Points;
            PointsCounter.Content = $"(+{GameField.PointAdded})";
            
        }
        /// <summary>
        /// Присвоение всем ячейкам соответствующего изображения
        /// </summary>
        private void DefineAllImages()
        {
            for (int x = 0; x < GameField.fieldSize; x++)
            {
                for (int y = 0; y < GameField.fieldSize; y++)
                {
                    DefineImage(Images[x, y]);
                }
            }
        }
        /// <summary>
        /// Присвоение ячейки соответствующего изображения
        /// </summary>
        /// <param name="cell">Логическая ячейка</param>
        /// <param name="image">Графическая ячейка</param>
        private void DefineImage(MTImage image)
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

            GameField.cells[X, Y].IsChanged = false;
        }
        /// <summary>
        /// Возвращает относительный путь к изображению соответствующей фигурки
        /// </summary>
        /// <param name="figure">Фигурка</param>
        /// <returns></returns>
        private string GetFigurePath(Figure figure)
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
        private void PrepareGrid()
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
                    Grid.SetColumn(Buttons[x, y], x);
                    Grid.SetRow(Buttons[x, y], y);
                    Images[x, y].Stretch = Stretch.Uniform;
                    GameFieldControl.Children.Add(Buttons[x, y]);
                }
            }
        }
        /// <summary>
        /// Первый клик по ячейке
        /// </summary>
        private void FirstClick(int X, int Y)
        {
            //Логика
            GameField.SelectCell(X, Y);
            GameField.IsSomeSelected = true;

            //Анимация
            StartImageAnimation(Images[X, Y], Animations.Selection);
            AnimatedImage = Images[X, Y];
        }
        /// <summary>
        /// Второй клик по ячейке
        /// </summary>
        private async Task SecondClick(int X, int Y)
        {
            UpdatePointsLabel();
            GameField.ResetCounter();

            //Снятие выделения
            StopImageAnimation(AnimatedImage);
            GameField.IsSomeSelected = false;

            await SwapImages(Images[X, Y], AnimatedImage);

            //Проверка на ложное нажатие
            if (GameField.IsFalseMove())
            {
                await SwapImages(Images[X, Y], AnimatedImage);

                StopImageAnimation(AnimatedImage);
                StopImageAnimation(Images[X, Y]);

                GameField.IsSomeSelected = false;
                AnimatedImage = null;

                return;
            }

            StopImageAnimation(AnimatedImage);
            StopImageAnimation(Images[X, Y]);

            GameField.CheckAllCells();
            GameField.DeleteMarkedCells();
            UpdatePointsLabel();
            await PutDownFigures();

            do
            {
                GameField.MakeNewFigures();
                await DefineAllImagesAnim();

                GameField.CheckAllCells();
                GameField.DeleteMarkedCells();
                UpdatePointsLabel();
                await PutDownFigures();

            }
            while (GameField.HaveEmptyFigeres());

            await DefineAllImagesAnim();

            UpdatePointsLabel();
            GameField.ResetCounter();

            BC.UpdateStatus(GameField);

            AnimatedImage = null;
        }
        /// <summary>
        /// Анимированно опустить фигуры вниз
        /// </summary>
        private async Task PutDownFigures()
        {
            for (int i = 1; i < GameField.fieldSize; i++)
            {
                if (GameField.HasChangedCells())
                {
                    await DefineAllImagesAnim();
                    GameField.PutDownFiguresOnes();
                }

            }
        }
        /// <summary>
        /// Второе нажатие на ту же кнопку
        /// </summary>
        private void SameClick()
        {
            GameField.UnselectCell();
            GameField.IsSomeSelected = false;

            StopImageAnimation(AnimatedImage);
            AnimatedImage = null;
        }
        /// <summary>
        /// Нажатие на графическую ячейку
        /// </summary>
        public async void CellClick(object sender, EventArgs e)
        {
            if (IsAnimationGoing)
            {
                return;
            }

            //Извлечение координат
            int x = Grid.GetColumn(sender as Button);
            int y = Grid.GetRow(sender as Button);

            IsAnimationGoing = true;

            switch (ChosenSpell)
            {
                case Spells.HorizontalSlash:
                    await HorizontalSlash(y);
                    IsAnimationGoing = false;
                    ChosenSpell = Spells.None;
                    BC.UpdateStatus(GameField);
                    return;
                case Spells.VerticalSlash:
                    await VerticalSlash(x);
                    IsAnimationGoing = false;
                    ChosenSpell = Spells.None;
                    BC.UpdateStatus(GameField);
                    return;                    
                case Spells.Bomb:
                    await Bomb(GameField.cells[x, y]);
                    IsAnimationGoing = false;
                    ChosenSpell = Spells.None;
                    BC.UpdateStatus(GameField);
                    return;
                case Spells.Diamondization:
                    await Diamondization(GameField.cells[x, y]);
                    IsAnimationGoing = false;
                    ChosenSpell = Spells.None;
                    BC.UpdateStatus(GameField);
                    return;
                case Spells.Pick:
                    await Pick(GameField.cells[x, y]);
                    IsAnimationGoing = false;
                    ChosenSpell = Spells.None;
                    BC.UpdateStatus(GameField);
                    return;
                case Spells.None:
                    IsAnimationGoing = false;
                    break;
            }            

            //Первое нажатие
            if (GameField.IsSomeSelected == false)
            {
                FirstClick(x, y);
            }
            //Второе нажатие
            else
            {
                //Нажатие на ту же ячейку
                if (GameField.IsSamePlase(x, y))
                {
                    SameClick();
                }
                //Нажатие на соседнюю клетку
                else if (GameField.IsNeighbors(x, y))
                {
                    await SecondClick(x, y);
                }
            }

            IsAnimationGoing = false;
        }

        /// <summary>
        /// Начало анимации выбора в указанной ячейке
        /// </summary>
        /// <param name="image">Ячейка</param>
        private void StartImageAnimation(Image image, DoubleAnimation doubleAnimation)
        {
            image.BeginAnimation(UIElement.OpacityProperty, doubleAnimation);
        }
        /// <summary>
        /// Завершает анимацию выбранной ячейки
        /// </summary>
        /// <param name="image">Ячейка</param>
        private void StopImageAnimation(Image image)
        {
            image.BeginAnimation(UIElement.OpacityProperty, null);
        }
        /// <summary>
        /// Плавно меняет два изображение местами
        /// </summary>
        /// <param name="firstImage">Первое изображение</param>
        /// <param name="secondImage">Второе изображение</param>
        private async Task SwapImages(MTImage firstImage, MTImage secondImage)
        {
            StartImageAnimation(firstImage, Animations.Disappearance);
            StartImageAnimation(secondImage, Animations.Disappearance);
            await Task.Delay(500);

            int x1 = firstImage.X;
            int y1 = firstImage.Y;
            int x2 = secondImage.X;
            int y2 = secondImage.Y;

            GameField.SwapCells(GameField.cells[x1, y1], GameField.cells[x2, y2]);

            DefineImage(firstImage);
            DefineImage(secondImage);

            StartImageAnimation(firstImage, Animations.Appearance);
            StartImageAnimation(secondImage, Animations.Appearance);
            await Task.Delay(500);
        }
        /// <summary>
        /// Анимированно меняет все изменённые изображения на актуальные
        /// </summary>
        private async Task DefineAllImagesAnim()
        {
            for (int x = 0; x < GameField.fieldSize; x++)
            {
                for (int y = 0; y < GameField.fieldSize; y++)
                {
                    AnimatedDefineImage(Images[x, y]);
                }
            }
            await Task.Delay(500);
        }
        /// <summary>
        /// Анимированно меняет язображение на актуальное
        /// </summary>
        /// <param name="image">Изображение</param>
        private async void AnimatedDefineImage(MTImage image)
        {
            int X = image.X;
            int Y = image.Y;

            if (GameField.cells[X, Y].figure == Figure.Empty)
            {
                StartImageAnimation(image, Animations.QuickDisappearance);
                await Task.Delay(200);

                image.Source = null;
                ImageBehavior.SetAnimatedSource(image, null);

                StopImageAnimation(image);



                GameField.cells[X, Y].IsChanged = false;
            }
            else if (GameField.cells[X, Y].IsChanged)
            {
                StartImageAnimation(image, Animations.QuickDisappearance);
                await Task.Delay(200);

                BitmapImage BMI = new BitmapImage();
                BMI.BeginInit();
                BMI.UriSource = new Uri(GetFigurePath(GameField.cells[X, Y].figure), UriKind.Relative);
                BMI.EndInit();
                image.Source = BMI;
                ImageBehavior.SetAnimatedSource(image, BMI);
                ImageBehavior.SetAnimationSpeedRatio(image, 0.2);

                StartImageAnimation(image, Animations.QuickAppearance);
                await Task.Delay(200);

                GameField.cells[X, Y].IsChanged = false;
            }
            else
            {

            }
        }

        private async Task MakeNewFigures()
        {
            GameField.MakeNewFigures();
            await DefineAllImagesAnim();
        }

        private async Task DeleteFigures()
        {
            GameField.CheckAllCells();
            GameField.DeleteMarkedCells();
            UpdatePointsLabel();
            await PutDownFigures();
        }

        private async Task UpdateField()
        {
            do
            {
                await MakeNewFigures();
                await DeleteFigures();
            }
            while (GameField.HaveEmptyFigeres());

        }

        public async Task HorizontalSlash(int y)
        {            
            GameField.HorisontalSlash(y);

            UpdatePointsLabel();

            await PutDownFigures();

            await UpdateField();

            await DefineAllImagesAnim();

            UpdatePointsLabel();

            GameField.ResetCounter();
        }

        public async Task VerticalSlash(int x)
        {
            GameField.VerticalSlash(x);

            UpdatePointsLabel();

            await PutDownFigures();

            await UpdateField();

            await DefineAllImagesAnim();

            UpdatePointsLabel();

            GameField.ResetCounter();
        }
        public async Task Diamondization(Cell cell)
        {
            GameField.Diamondization(cell);

            UpdatePointsLabel();

            await PutDownFigures();

            await UpdateField();

            await DefineAllImagesAnim();

            UpdatePointsLabel();

            GameField.ResetCounter();
        }

        public async Task Pick(Cell cell)
        {
            GameField.Pick(cell);

            UpdatePointsLabel();

            await PutDownFigures();

            await UpdateField();

            await DefineAllImagesAnim();

            UpdatePointsLabel();

            GameField.ResetCounter();
        }

        public async Task Mix()
        {
            if (IsAnimationGoing)
            {
                return;
            }

            IsAnimationGoing = true;

            GameField.Mix();

            UpdatePointsLabel();

            await PutDownFigures();

            await UpdateField();

            await DefineAllImagesAnim();

            UpdatePointsLabel();

            GameField.ResetCounter();

            IsAnimationGoing = false;

            BC.UpdateStatus(GameField);
        }

        public async Task Bomb(Cell cell)
        {
            GameField.Bomb(cell);

            UpdatePointsLabel();

            await PutDownFigures();

            await UpdateField();

            await DefineAllImagesAnim();

            UpdatePointsLabel();

            GameField.ResetCounter();
        }
    }
}
