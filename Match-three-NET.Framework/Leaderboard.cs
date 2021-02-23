using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Match_three_NET.Framework
{
    public class Leaderboard
    {
        public List<PlayerAccount> Players { get; set; }

        private int LeaderID = 9;
        private bool HaseLeader = true;

        public Leaderboard()
        {
            Deserialize();
        }

        public void Sort()
        {
            //Players.Sort((a, b) => a.Points.CompareTo(b.Points));

            Players = Players.OrderByDescending(x => x.Points).ToList();
        }

        public void Add(string name, int points)
        {
            Players.Add(new PlayerAccount { Name = name, Points = points });
        }

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

        public void Serialize()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<PlayerAccount>));

            using (FileStream stream = new FileStream("Leaderboard.xml", FileMode.OpenOrCreate))
            {
                serializer.Serialize(stream, Players);
            }
        }

        public void Deserialize()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<PlayerAccount>));

            using (FileStream stream = new FileStream("Leaderboard.xml", FileMode.OpenOrCreate))
            {
                Players = (List<PlayerAccount>)serializer.Deserialize(stream);
            }
        }

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
