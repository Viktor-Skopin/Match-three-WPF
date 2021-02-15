﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Match_three_WPF
{
    /// <summary>
    /// Логика взаимодействия для GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        Visualizer MatchThree;
        public GameWindow()
        {
            InitializeComponent();

            MatchThree = new Visualizer(GameFieldGrid, 10, PointsLabel);
            GameTimer timer = new GameTimer(500, TimeLabel, TimePB);
            timer.Start();

        }
    }
}
