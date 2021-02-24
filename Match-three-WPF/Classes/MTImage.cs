using System.Windows.Controls;
using Match_three_NET.Framework;

namespace Match_three_WPF
{
    public class MTImage : Image
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
        /// Фигурка изображения
        /// </summary>
        public Figure figure {get;set;}
    }
}
