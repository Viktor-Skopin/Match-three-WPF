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
        int fieldSize { get; set; }

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
        public void DefineFigures()
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
    }
}
