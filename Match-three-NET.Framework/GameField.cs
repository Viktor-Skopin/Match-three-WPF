using System;
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
        private int fieldSize;

        /// <summary>
        /// Выбрана ли какая либо ячейка
        /// </summary>
        private bool IsSomeSelected = false;

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
        public Figure GetRandomFigure(Random random)
        {
            Array values = Enum.GetValues(typeof(Figure));
            
            Figure randomFigure = (Figure)values.GetValue(random.Next(1,values.Length));
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
            for (int x = 0; x < cells.GetLength(0); x++)
            {
                for (int y = 0; y < cells.GetLength(1); y++)
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

                if (i <= (fieldSize-1) && cells[X, i].figure == figure)
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

                if (i <= (fieldSize-1) && cells[i, Y].figure == figure)
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
                }
            }
        }

        /// <summary>
        /// Удалить помеченные на удаление ячейки
        /// </summary>
        public void DeleteMarkedCells()
        {
            for (int x = 0; x < cells.GetLength(0); x++)
            {
                for (int y = 0; y < cells.GetLength(1); y++)
                {
                    CheckDeletionMarked(cells[x, y]);
                }
            }
        }

        /// <summary>
        /// Отмечает помеченные ячейки как пустые
        /// </summary>
        public void CheckDeletionMarked(Cell cell)
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
            for (int i = 1; i < cells.GetLength(1); i++)
            {
                for (int x = 0; x < cells.GetLength(0); x++)
                {
                    for (int y = 1; y < cells.GetLength(1); y++)
                    {
                        if (cells[x, y].figure == Figure.Empty && cells[x, y - 1].figure != Figure.Empty)
                        {
                            SwapCells(cells[x, y], cells[x, y - 1]);
                        }
                    }
                }
            }
        }

        public void PutDownFiguresOnes()
        {
            for (int x = 0; x < cells.GetLength(0); x++)
            {
                for (int y = 1; y < cells.GetLength(1); y++)
                {
                    if (cells[x, y].figure == Figure.Empty && cells[x, y - 1].figure != Figure.Empty)
                    {
                        SwapCells(cells[x, y], cells[x, y - 1]);
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

            for (int x = 0; x < cells.GetLength(0); x++)
            {
                for (int y = 0; y < cells.GetLength(1); y++)
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
            for (int x = 0; x < cells.GetLength(0); x++)
            {
                for (int y = 0; y < cells.GetLength(1); y++)
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
    }
}
