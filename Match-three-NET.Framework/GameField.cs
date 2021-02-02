using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_three_NET.Framework
{
    public class GameField
    {
        public Cell[,] cells;
        int fieldSize { get; set; }

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

        public void StartNewGame()
        {
            DefineFigures();
        }

        public Figure GetRandomFigure(Random random)
        {
            Array values = Enum.GetValues(typeof(Figure));
            
            Figure randomFigure = (Figure)values.GetValue(random.Next(1,values.Length));
            return randomFigure;
        }

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
