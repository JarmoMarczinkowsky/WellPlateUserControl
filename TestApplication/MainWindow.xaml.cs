using System;
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
        //private WellPlateUserControl.Conn _wellPlate = new WellPlateUserControl.Conn();


        public MainWindow()
        {
            InitializeComponent();

            globalWellPlate.IsEditable = true;
            //globalWellPlate.IsRectangle = true;
            //globalWellPlate.SetGridColor = Colors.Black;
            //globalWellPlate.SetClickColor = Colors.Crimson;
            //globalWellPlate.SetStrokeColor = Colors.White;
            //globalWellPlate.SetStrokeThickness = 80;
            globalWellPlate.SetWellPlateSize(1,26);
            //globalWellPlate.SetGridColor = Colors.Transparent;
            //globalWellPlate.SetWellSize = 5;
            //globalWellPlate.SetMaxWidth = 800;
            globalWellPlate.DrawWellPlate();
            globalWellPlate.ColorCoordinate(4);
            globalWellPlate.ColorCoordinate("A1");
            globalWellPlate.ColorCoordinate("C4", Colors.Red);
            globalWellPlate.ColorCoordinate(5, Colors.Green);
            //globalWellPlate.CoordinateToNumber("A4");
            //globalWellPlate.NumberToCoordinate(6);
            //Debug.WriteLine(globalWellPlate.GetColoredCoordinates);
            //Debug.WriteLine(globalWellPlate.GetNotColoredCoordinates);

        }

        private void ConnTest(object sender, RoutedEventArgs e)
        {
            //placeholder
            //globalWellPlate.IsRectangle = false;
            globalWellPlate.SetWellPlateSize(6, 4);
            globalWellPlate.DrawWellPlate();

        }
    }
}
