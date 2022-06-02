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
            //globalWellPlate.GridColor = Colors.Turquoise;
            //globalWellPlate.ClickColor = Colors.Azure;
            //globalWellPlate.StrokeColor = Colors.Tomato;
            //globalWellPlate.StrokeThickness = 20;
            globalWellPlate.SetWellPlateSize(8,10);
            //globalWellPlate.WellSize = 30;
            //globalWellPlate.LabelColor = Colors.Red;
            //globalWellPlate.BorderColor = Colors.Blue;
            globalWellPlate.DrawWellPlate();
            globalWellPlate.ColorCoordinate(4);
            globalWellPlate.ColorCoordinate("A1");
            globalWellPlate.ColorCoordinate("C4", Colors.Red);
            globalWellPlate.ColorCoordinate(5, Colors.Green);
            Debug.WriteLine(globalWellPlate.CoordinateToNumber("A4"));
            Debug.WriteLine(globalWellPlate.NumberToCoordinate(6));
            //Debug.WriteLine(globalWellPlate.ColoredCoordinates);
            //Debug.WriteLine(globalWellPlate.NotColoredCoordinates);

        }
    }
}
