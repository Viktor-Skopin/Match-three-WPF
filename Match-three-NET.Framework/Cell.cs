using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_three_NET.Framework
{
    /// <summary>
    /// Ячейка
    /// </summary>
    public class Cell
    {
        /// <summary>
        /// Координата X
        /// </summary>
        public int X { get; set; }
        /// <summary>
        /// Координата Y
        /// </summary>
        public int Y { get; set; }
        /// <summary>
        /// Выбрана ли эта ячейка
        /// </summary>
        public bool IsSelected { get; set; }
        /// <summary>
        /// Фигурка, помещённая в эту ячейку
        /// </summary>
        public Figure figure { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public Cell(int x, int y)
        {
            X = x;
            Y = y;
            IsSelected = false;
            figure = Figure.Empty;
        }
    }
}
