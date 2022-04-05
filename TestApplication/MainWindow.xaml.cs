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
        private WellPlateUserControl.Conn _wellPlate = new WellPlateUserControl.Conn();


        public MainWindow()
        {
            InitializeComponent();

            globalWellPlate.SetGridColor = Color.FromRgb(209, 232, 247);
            globalWellPlate.SetClickColor = Color.FromRgb(97,172,223);
            //globalWellPlate.SetStrokeColor = Colors.Black;
            //globalWellPlate.StrokeThickness = 20;
            globalWellPlate.SetMaxHeight = 700;
            //globalWellPlate.TurnCoordinatesOff = true;
            //globalWellPlate.SetMaxWidth = 800;
            //globalWellPlate.IsRectangle = true;
            globalWellPlate.IsEditable = true;
            globalWellPlate.SetWellPlateSize(16,24); //width, height
            //globalWellPlate.ColorCoordinate("A2;3"); //multiple coordinates get split with a ';'
            globalWellPlate.ColorCoordinate("A5", Color.FromRgb(222,233,212));
            globalWellPlate.ColorCoordinate("B5", Color.FromRgb(251, 209, 205));
            //Debug.WriteLine(globalWellPlate.NumberToCoordinate(-11));
            //Debug.WriteLine(globalWellPlate.CoordinateToNumber("a2"));
            //Debug.WriteLine(globalWellPlate.LastClickedCoordinate);

            //List<string> colorList = globalWellPlate.GiveColoredList();

            //globalWellPlate.GiveNotColoredList();



        }

        private void ConnTest(object sender, RoutedEventArgs e)
        {
            //make sure the colors get set before the grid size
            //Debug.WriteLine(globalWellPlate.LastClickedCoordinate);


        }
    }
}
