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
        public bool IsSelected;
        /// <summary>
        /// Фигурка, помещённая в эту ячейку
        /// </summary>
        public Figure figure { get; set; }
        /// <summary>
        /// Помечена ли ячейка на удаление
        /// </summary>
        public bool IsMarkedForDeletion { get; set; }
        /// <summary>
        /// Изменена ли ячейка
        /// </summary>
        public bool IsChanged { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public Cell(int x, int y)
        {
            X = x;
            Y = y;
            IsSelected = false;
            IsMarkedForDeletion = false;
            IsChanged = false;
            figure = Figure.Empty;
        }

        /// <summary>
        /// Выбрать ячейку
        /// </summary>
        public void Select()
        {
            IsSelected = true;
        }

        /// <summary>
        /// Снять выбор ячейки
        /// </summary>
        public void UnSelect()
        {
            IsSelected = false;
        }
    }
}
