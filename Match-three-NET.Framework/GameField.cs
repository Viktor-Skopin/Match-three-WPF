﻿using System;

namespace Match_three_NET.Framework
{
    /// <summary>
    /// Игровое поле
    /// </summary>
    public class GameField
    {
        /// <summary>
        /// Поле ячеек
        /// </summary>
        public Cell[,] cells;
        /// <summary>
        /// Размер поля
        /// </summary>
        public int fieldSize;
        /// <summary>
        /// Выбрана ли какая либо ячейка
        /// </summary>
        public bool IsSomeSelected = false;
        /// <summary>
        /// Ссылка на выбранную ячейку в массиве
        /// </summary>
        public Cell ChosenCell;
        /// <summary>
        /// Кол-во очков
        /// </summary>
        public int Points { get; set; }
        /// <summary>
        /// Количестко очков, полученных в результате последнего хода
        /// </summary>
        public int PointAdded { get; set; }
        /// <summary>
        /// Активна ли "Бомба"
        /// </summary>
        public bool IsBombAviable { get; set; }
        /// <summary>
        /// Активно ли "Перемешивание"
        /// </summary>
        public bool IsMixAviable { get; set; }
        /// <summary>
        /// Активна ли "Горизонталь"
        /// </summary>
        public bool IsHorizontalSlashAviable { get; set; }
        /// <summary>
        /// Активна ли "Вертикаль"
        /// </summary>
        public bool IsVerticalSlashAviable { get; set; }
        /// <summary>
        /// Активна ли "Кирка"
        /// </summary>
        public bool IsPickAviable { get; set; }
        /// <summary>
        /// Активна ли "Алмазизация"
        /// </summary>
        public bool IsDiamondizationAviable { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="size">Размер поля</param>
        public GameField(int size)
        {
            cells = new Cell[size, size];
            fieldSize = size;
            Points = 0;
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    cells[x, y] = new Cell(x, y);
                }
            }
            StartNewGame();
        }
        /// <summary>
        /// Начало новой игры
        /// </summary>
        public void StartNewGame()
        {
            DefineFigures();
            CheckAllCells();
            DeleteMarkedCells();
            PutDownFigures();
            FillVoids();

            Points = 0;
            PointAdded = 0;

            IsMixAviable = false;
            IsBombAviable = false;
            IsDiamondizationAviable = false;
            IsHorizontalSlashAviable = false;
            IsVerticalSlashAviable = false;
            IsPickAviable = false;
        }
        /// <summary>
        /// Возвращает случайную фигурку
        /// </summary>
        /// <param name="random">Random который должен быть инициализирован вне цикла</param>
        private Figure GetRandomFigure(Random random)
        {
            Array values = Enum.GetValues(typeof(Figure));

            Figure randomFigure = (Figure)values.GetValue(random.Next(1, values.Length));
            return randomFigure;
        }
        /// <summary>
        /// Присвоение всем ячейкам случайной фигурки
        /// </summary>
        private void DefineFigures()
        {
            Random random = new Random();

            for (int x = 0; x < fieldSize; x++)
            {
                for (int y = 0; y < fieldSize; y++)
                {
                    cells[x, y].figure = GetRandomFigure(random);
                }
            }
        }
        /// <summary>
        /// Выделение ячейки по ссылке
        /// </summary>
        /// <param name="cell">Ячейка</param>
        private void SelectCell(Cell cell)
        {
            cell.Select();
            IsSomeSelected = true;
            ChosenCell = cell;
        }
        /// <summary>
        /// Выделение ячейки по указанным координатам
        /// </summary>
        public void SelectCell(int x, int y)
        {
            SelectCell(cells[x, y]);
        }
        /// <summary>
        /// Снятие выделения с выбранной в данный момент ячейки
        /// </summary>
        public void UnselectCell()
        {
            ChosenCell.UnSelect();
            IsSomeSelected = false;
            ChosenCell = null;
        }
        /// <summary>
        /// Обмен фигурами двуг ячеек
        /// </summary>
        /// <param name="firstCell">Первая фигура</param>
        /// <param name="secondCell">Вторая фигура</param>
        public void SwapCells(Cell firstCell, Cell secondCell)
        {
            Figure buffer = firstCell.figure;

            firstCell.figure = secondCell.figure;
            secondCell.figure = buffer;
        }
        /// <summary>
        /// Помечает подходящие ячейки на удаление
        /// </summary>
        public void CheckAllCells()
        {
            for (int x = 0; x < fieldSize; x++)
            {
                for (int y = 0; y < fieldSize; y++)
                {
                    MatchCheckDown(cells[x, y]);
                    MatchCheckRight(cells[x, y]);
                }
            }
        }
        /// <summary>
        /// Проверка совпадений ячеек внизу
        /// </summary>
        /// <param name="cell">Проверяевая ячейка</param>
        private void MatchCheckDown(Cell cell)
        {
            int X = cell.X;
            int Y = cell.Y;

            Figure figure = cell.figure;

            int count = 0;
            int i = Y;

            while (true)
            {

                if (i <= (fieldSize - 1) && cells[X, i].figure == figure)
                {
                    count++;
                    i++;
                }
                else
                {
                    break;
                }
            }

            if (count >= 3)
            {
                for (int j = Y; j < i; j++)
                {
                    cells[X, j].IsMarkedForDeletion = true;
                    cells[X, j].IsChanged = true;
                }
            }

            Random random = new Random();

            if(IsSpellUnloced(random, count))
            {
                UnlockRandomSpell(random);
            }
        }
        /// <summary>
        /// Проверка совпадений ячеек справа
        /// </summary>
        /// <param name="cell">Проверяевая ячейка</param>
        private void MatchCheckRight(Cell cell)
        {
            int X = cell.X;
            int Y = cell.Y;

            Figure figure = cell.figure;

            int count = 0;
            int i = X;

            while (true)
            {

                if (i <= (fieldSize - 1) && cells[i, Y].figure == figure)
                {
                    count++;
                    i++;
                }
                else
                {
                    break;
                }
            }

            if (count >= 3)
            {
                for (int j = X; j < i; j++)
                {
                    cells[j, Y].IsMarkedForDeletion = true;
                    cells[j, Y].IsChanged = true;
                }
            }
        }
        /// <summary>
        /// Удалить помеченные на удаление ячейки
        /// </summary>
        public void DeleteMarkedCells()
        {
            //for (int x = 0; x < fieldSize; x++)
            //{
            //    for (int y = 0; y < fieldSize; y++)
            //    {
            //        DeleteCell(cells[x, y]);
            //    }
            //}

            foreach(Cell cell in cells)
            {
                DeleteCell(cell);
            }
        }
        /// <summary>
        /// Очищает ячейку если она помечена на удаление
        /// </summary>
        private void DeleteCell(Cell cell)
        {
            if (cell.IsMarkedForDeletion)
            {
                cell.IsMarkedForDeletion = false;
                Points += DefineCellPoints(cell);
                PointAdded += DefineCellPoints(cell);
                cell.figure = Figure.Empty;
            }
        }
        /// <summary>
        /// Определить стоимость фигурки ячейки
        /// </summary>
        /// <param name="cell">Ячейка</param>
        /// <returns>Стоимость ячейки</returns>
        public int DefineCellPoints(Cell cell)
        {
            switch (cell.figure)
            {
                case Figure.Empty:
                    return 0;
                case Figure.Amethyst:
                    return 15;
                case Figure.Citrine:
                    return 5;
                case Figure.Diamond:
                    return 30;
                case Figure.Emerald:
                    return 10;
                case Figure.Ruby:
                    return 25;
                case Figure.Topaz:
                    return 20;
                default:
                    return 0;
            }
        }
        /// <summary>
        /// Сброс счётчика полученных очков
        /// </summary>
        public void ResetCounter()
        {
            PointAdded = 0;
        }
        /// <summary>
        /// Поднимает пустые ячейки вверх
        /// </summary>
        private void PutDownFigures()
        {
            for (int i = 1; i < fieldSize; i++)
            {
                for (int x = 0; x < fieldSize; x++)
                {
                    for (int y = 1; y < fieldSize; y++)
                    {
                        if (cells[x, y].figure == Figure.Empty && cells[x, y - 1].figure != Figure.Empty)
                        {
                            SwapCells(cells[x, y], cells[x, y - 1]);

                            cells[x, y].IsChanged = true;
                            cells[x, y - 1].IsChanged = true;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Опустить фигуры вниз один раз
        /// </summary>
        public void PutDownFiguresOnes()
        {
            for (int x = 0; x < fieldSize; x++)
            {
                for (int y = 1; y < fieldSize; y++)
                {
                    if (cells[x, y].figure == Figure.Empty && cells[x, y - 1].figure != Figure.Empty)
                    {
                        SwapCells(cells[x, y], cells[x, y - 1]);

                        cells[x, y].IsChanged = true;
                    }
                }
            }

        }
        /// <summary>
        /// Создаёт новые фигурки на месте пустых
        /// </summary>
        public void MakeNewFigures()
        {
            Random random = new Random();

            for (int x = 0; x < fieldSize; x++)
            {
                for (int y = 0; y < fieldSize; y++)
                {
                    if (cells[x, y].figure == Figure.Empty)
                    {
                        cells[x, y].figure = GetRandomFigure(random);
                        cells[x, y].IsChanged = true;
                    }
                }
            }
        }
        /// <summary>
        /// Есть ли пустые ячейки
        /// </summary>
        public bool HaveEmptyFigeres()
        {
            for (int x = 0; x < fieldSize; x++)
            {
                for (int y = 0; y < fieldSize; y++)
                {
                    if (cells[x, y].figure == Figure.Empty)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Заполняет и опускает ячейки пока не останется совпадений
        /// </summary>
        private void FillVoids()
        {
            while (HaveEmptyFigeres())
            {
                MakeNewFigures();
                CheckAllCells();
                DeleteMarkedCells();
                PutDownFigures();
            }
        }
        /// <summary>
        /// Проверяет является ли ячейка по заданным координатам соседом выбранной ячейки
        /// </summary>
        public bool IsNeighbors(int x, int y)
        {
            bool left = true;
            bool right = true;
            bool up = true;
            bool down = true;

            //Left
            if (x - 1 >= 0)
            {
                if (x - 1 != ChosenCell.X) left = false;
            }
            else left = false;
            //Right
            if (x + 1 <= 9)
            {
                if (x + 1 != ChosenCell.X) right = false;
            }
            else right = false;
            //Up
            if (y - 1 >= 0)
            {
                if (y - 1 != ChosenCell.Y) up = false;
            }
            else up = false;
            //Down
            if (y + 1 <= 9)
            {
                if (y + 1 != ChosenCell.Y) down = false;
            }
            else down = false;

            if ((up && x == ChosenCell.X) || (down && x == ChosenCell.X) || (left && y == ChosenCell.Y) || (right && y == ChosenCell.Y))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Проверка, совпают ли координаты с координатами выбранной ячейки
        /// </summary>
        public bool IsSamePlase(int x, int y)
        {
            if (ChosenCell.X == x && ChosenCell.Y == y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Есть ли комбинации фигур после хода
        /// </summary>
        public bool IsFalseMove()
        {
            for (int x = 0; x < fieldSize; x++)
            {
                for (int y = 0; y < fieldSize; y++)
                {
                    MatchCheckDown(cells[x, y]);
                    MatchCheckRight(cells[x, y]);
                }
            }

            for (int x = 0; x < fieldSize; x++)
            {
                for (int y = 0; y < fieldSize; y++)
                {
                    if (cells[x, y].IsMarkedForDeletion) return false;
                }
            }

            return true;
        }
        /// <summary>
        /// Есть ли ячейки, помеченные как изменённые
        /// </summary>
        public bool HasChangedCells()
        {
            for (int x = 0; x < fieldSize; x++)
            {
                for (int y = 0; y < fieldSize; y++)
                {
                    if (cells[x, y].IsChanged == true)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Удаляет все ячейки на указанной горизонтальной линии
        /// </summary>
        /// <param name="y">Координата горизонтальной линии</param>
        public void HorisontalSlash(int y)
        {
            for (int i = 0; i < fieldSize; i++)
            {
                cells[i, y].IsMarkedForDeletion = true;
                cells[i, y].IsChanged = true;
            }

            DeleteMarkedCells();

            IsHorizontalSlashAviable = false;
        }
        /// <summary>
        /// Удаляет все ячейки на указанной вертикальной линии
        /// </summary>
        /// <param name="x">Координата вертикальной линии</param>
        public void VerticalSlash(int x)
        {
            for (int i = 0; i < fieldSize; i++)
            {
                cells[x, i].IsMarkedForDeletion = true;
                cells[x, i].IsChanged = true;
            }

            DeleteMarkedCells();

            IsVerticalSlashAviable = false;
        }
        /// <summary>
        /// Превращает все ячейки с теой же фигуркой, что и в выбранной ячейки в алмазы
        /// </summary>
        /// <param name="cell">Ячейка</param>
        public void Diamondization(Cell cell)
        {
            Figure figure = cell.figure;

            foreach (Cell element in cells)
            {
                if (element.figure == figure)
                {
                    element.figure = Figure.Diamond;
                    element.IsChanged = true;
                }
            }

            IsDiamondizationAviable = false;
        }
        /// <summary>
        /// Удаляет выбранную ячейку
        /// </summary>
        /// <param name="cell">Ячейка</param>
        public void Pick(Cell cell)
        {
            cell.IsMarkedForDeletion = true;
            cell.IsChanged = true;
            DeleteCell(cell);

            IsPickAviable = false;
        }
        /// <summary>
        /// Перешивает ячейки на поле
        /// </summary>
        public void Mix()
        {
            Random random = new Random();

            foreach (Cell cell in cells)
            {
                cell.figure = GetRandomFigure(random);
                cell.IsChanged = true;
            }

            IsMixAviable = false;
        }
        /// <summary>
        /// Уничтожает выбранную ячейку и ячейки вокруг неё
        /// </summary>
        /// <param name="cell">Ячейка</param>
        public void Bomb(Cell cell)
        {
            int x = cell.X;
            int y = cell.Y;

            cell.IsMarkedForDeletion = true;
            cell.IsChanged = true;
            DeleteCell(cell);

            if (y + 1 < fieldSize)//Низ
            {
                cells[x, y + 1].IsMarkedForDeletion = true;
                cells[x, y + 1].IsChanged = true;
                DeleteCell(cells[x, y + 1]);
            }
            if (y - 1 >= 0)//Верх
            {
                cells[x, y - 1].IsMarkedForDeletion = true;
                cells[x, y - 1].IsChanged = true;
                DeleteCell(cells[x, y - 1]);
            }
            if (x + 1 < fieldSize)//Право
            {
                cells[x + 1, y].IsMarkedForDeletion = true;
                cells[x + 1, y].IsChanged = true;
                DeleteCell(cells[x + 1, y]);
            }
            if (x - 1 >= 0)//Лево
            {
                cells[x - 1, y].IsMarkedForDeletion = true;
                cells[x - 1, y].IsChanged = true;
                DeleteCell(cells[x - 1, y]);
            }
            if (y + 1 < fieldSize && x - 1 >= 0)//Низ - Лево
            {
                cells[x - 1, y + 1].IsMarkedForDeletion = true;
                cells[x - 1, y + 1].IsChanged = true;
                DeleteCell(cells[x - 1, y + 1]);
            }
            if (y + 1 < fieldSize && x + 1 < fieldSize)//Низ - Право
            {
                cells[x + 1, y + 1].IsMarkedForDeletion = true;
                cells[x + 1, y + 1].IsChanged = true;
                DeleteCell(cells[x + 1, y + 1]);
            }
            if (y - 1 >= 0 && x - 1 >= 0)//Верх - Лево
            {
                cells[x - 1, y - 1].IsMarkedForDeletion = true;
                cells[x - 1, y - 1].IsChanged = true;
                DeleteCell(cells[x - 1, y - 1]);
            }
            if (y - 1 >= 0 && x + 1 < fieldSize)//Верх - Право
            {
                cells[x + 1, y - 1].IsMarkedForDeletion = true;
                cells[x + 1, y - 1].IsChanged = true;
                DeleteCell(cells[x + 1, y - 1]);
            }

            IsBombAviable = false;
        }
        /// <summary>
        /// Открывает доступ к случайной способности
        /// </summary>
        /// <param name="random">Random созданный вне цикла</param>
        public void UnlockRandomSpell(Random random)
        {
            int number = random.Next(1, 100);

            if(number >=1 && number <= 30)
            {
                IsPickAviable = true;
            }
            else if (number >= 31 && number <= 50)
            {
                IsMixAviable = true;
            }
            else if (number >= 51 && number <= 65)
            {
                IsHorizontalSlashAviable = true;
            }
            else if (number >=66 && number <= 80)
            {
                IsVerticalSlashAviable = true;
            }
            else if (number >= 81 && number <= 95)
            {
                IsBombAviable = true;
            }
            else if (number >= 96 && number <= 100)
            {
                IsDiamondizationAviable = true;
            }
        }
        /// <summary>
        /// Будет ли разблокирована способность в этот раз. Зависит от длины собранной линии
        /// </summary>
        /// <param name="random">Random созданный вне цикла</param>
        /// <param name="figureCount">Длинна собранной линии</param>
        /// <returns></returns>
        public bool IsSpellUnloced(Random random, int figureCount)
        {
            int number = random.Next(1, 100);

            if(figureCount * 10 > number)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
