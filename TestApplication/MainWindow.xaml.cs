﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WellPlateUserControl.Conn _wellPlate = new WellPlateUserControl.Conn();


        public MainWindow()
        {
            InitializeComponent();

            globalWellPlate.IsEditable = true;
            //globalWellPlate.SetStrokeColor = Colors.Black;
            //globalWellPlate.SetStrokeThickness = 80;
            globalWellPlate.SetWellPlateSize(14, 8);
            globalWellPlate.SetWellSize = 30;
            globalWellPlate.DrawWellPlate();
            globalWellPlate.ColorCoordinate(4);
            globalWellPlate.ColorCoordinate("A1");
            globalWellPlate.ColorCoordinate("A4");
            globalWellPlate.ColorCoordinate("C4", Colors.Red);
            


        }

        private void ConnTest(object sender, RoutedEventArgs e)
        {
            //make sure the colors get set before the grid size
            //Debug.WriteLine(globalWellPlate.LastClickedCoordinate);


        }
    }
}
