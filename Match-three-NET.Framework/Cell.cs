using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_three_NET.Framework
{
    public class Cell
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsSelected { get; set; }
        public Figure figure { get; set; }

        public Cell(int x, int y)
        {
            X = x;
            Y = y;
            IsSelected = false;
            figure = Figure.Empty;
        }
    }
}
