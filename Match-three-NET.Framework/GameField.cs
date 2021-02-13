﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private Cell ChosenCell;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="size">Размер поля</param>
        public GameField(int size)
        {
            cells = new Cell[size, size];
            fieldSize = size;

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
            FillVoids();
        }

        /// <summary>
        /// Возвращает случайную фигурку
        /// </summary>
        /// <param name="random">Random который должен быть инициализирован вне цикла</param>
        /// <returns></returns>
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
            DeleteMarkedCells();
        }

        /// <summary>
        /// Проверка совпадений ячеек внизу
        /// </summary>
        /// <param name="cell">Проверяевая ячейка</param>
        public void MatchCheckDown(Cell cell)
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
        }
        /// <summary>
        /// Проверка совпадений ячеек справа
        /// </summary>
        /// <param name="cell">Проверяевая ячейка</param>
        public void MatchCheckRight(Cell cell)
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
            for (int x = 0; x < fieldSize; x++)
            {
                for (int y = 0; y < fieldSize; y++)
                {
                    DeleteCell(cells[x, y]);
                }
            }
        }

        /// <summary>
        /// Очищает ячейку если она помечена на удаление
        /// </summary>
        public void DeleteCell(Cell cell)
        {
            if (cell.IsMarkedForDeletion)
            {
                cell.figure = Figure.Empty;
                cell.IsMarkedForDeletion = false;
            }
        }

        /// <summary>
        /// Поднимает пустые ячейки вверх
        /// </summary>
        public void PutDownFigures()
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
        public void FillVoids()
        {
            CheckAllCells();
            PutDownFigures();

            while (HaveEmptyFigeres())
            {
                MakeNewFigures();
                CheckAllCells();
                PutDownFigures();
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

        public void RemoveAllChanges()
        {
            for (int x = 0; x < fieldSize; x++)
            {
                for (int y = 0; y < fieldSize; y++)
                {
                    if (cells[x, y].IsChanged)
                    {
                        cells[x, y].IsChanged = false;
                    }
                }
            }
        }

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
    }
}
