using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Match_three_NET.Framework
{
    public class Leaderboard
    {
        /// <summary>
        /// Список игроков
        /// </summary>
        public List<PlayerAccount> Players { get; set; }
        /// <summary>
        /// Номер текущего лидера
        /// </summary>
        private int LeaderID = 9;
        /// <summary>
        /// Есть ли лидер на данный момент
        /// </summary>
        private bool HaseLeader = true;

        /// <summary>
        /// Конструктор
        /// </summary>
        public Leaderboard()
        {
            Deserialize();
        }
        /// <summary>
        /// Сортирует список игроков по убыванию количества очков
        /// </summary>
        public void Sort()
        {
            Players = Players.OrderByDescending(x => x.Points).ToList();
        }
        /// <summary>
        /// Добавляет нового игрока с заданным именем и количеством набранных очков
        /// </summary>
        /// <param name="name">Имя игрока</param>
        /// <param name="points">Количество очков игрока</param>
        public void Add(string name, int points)
        {
            Players.Add(new PlayerAccount { Name = name, Points = points });
        }
        /// <summary>
        /// Обрезает список лидеров до 10 человек
        /// </summary>
        public void Cut()
        {
            if (Players.Count > 10)
            {
                for (int i = Players.Count - 1; i >= 10; --i)
                {
                    Players.RemoveAt(i);
                }
            }
        }
        /// <summary>
        /// Сохраняет список лидеров в файл
        /// </summary>
        public void Serialize()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<PlayerAccount>));

            using (FileStream stream = new FileStream("Leaderboard.xml", FileMode.Create))
            {
                serializer.Serialize(stream, Players);
            }
        }
        /// <summary>
        /// Загрузка списка лидеров из файла
        /// </summary>
        public void Deserialize()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<PlayerAccount>));

            using (FileStream stream = new FileStream("Leaderboard.xml", FileMode.OpenOrCreate))
            {
                Players = (List<PlayerAccount>)serializer.Deserialize(stream);
            }
        }
        /// <summary>
        /// Возвращает имя и количество очков текущего лидера
        /// </summary>
        /// <param name="name">Имя</param>
        /// <param name="points">Кол-во очков</param>
        public void GetLeaderInfo(out string name, out int points)
        {
            if (HaseLeader)
            {
                name = Players[LeaderID].Name;
                points = Players[LeaderID].Points;
            }
            else
            {
                name = "!!!ВЫ!!!";
                points = int.MaxValue;
            }
        }
        /// <summary>
        /// Определяет текущего лидера исходя из количества набранных очков
        /// </summary>
        /// <param name="points">Количество набранных очков</param>
        public void FindLeader(int points)
        {
            for (int i = Players.Count - 1; i >= 0; i--)
            {
                if (Players[i].Points > points)
                {
                    LeaderID = i;
                    return;
                }                
            }

            LeaderID = -1;
            HaseLeader = false;
        }
    }
}
