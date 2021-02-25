using System;

namespace Match_three_NET.Framework
{
    /// <summary>
    /// Запись о игроке
    /// </summary>
    [Serializable]
    public class PlayerAccount
    {
        /// <summary>
        /// Имя игрока
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Кол-во очков, набранных игроком
        /// </summary>
        public int Points { get; set; }
    }
}
