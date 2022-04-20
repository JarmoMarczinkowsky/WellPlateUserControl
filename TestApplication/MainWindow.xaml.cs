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
            //globalWellPlate.GridColor = Colors.Black;
            globalWellPlate.ClickColor = Colors.Crimson;
            //globalWellPlate.StrokeColor = Colors.Black;
            //globalWellPlate.StrokeThickness = 80;
            globalWellPlate.SetWellPlateSize(12,1);
            //globalWellPlate.WellSize = 5;
            //globalWellPlate.LabelColor = Colors.Red;
            //globalWellPlate.BorderColor = Colors.Blue;
            globalWellPlate.DrawWellPlate();
            //globalWellPlate.ColorCoordinate(4);
            //globalWellPlate.ColorCoordinate("A1");
            //globalWellPlate.ColorCoordinate("C4", Colors.Red);
            //globalWellPlate.ColorCoordinate(5, Colors.Green);
            //globalWellPlate.CoordinateToNumber("A4");
            //globalWellPlate.NumberToCoordinate(6);
            //Debug.WriteLine(globalWellPlate.ColoredCoordinates);
            //Debug.WriteLine(globalWellPlate.NotColoredCoordinates);

        }
    }
}
