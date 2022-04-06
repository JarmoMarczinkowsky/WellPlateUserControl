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

            globalWellPlate.SetWellPlateSize(24, 16);
            globalWellPlate.DrawWellPlate();
            globalWellPlate.SetWellPlateSize(6, 4);
            globalWellPlate.DrawWellPlate();



            globalWellPlate.SetWellPlateSize(2, 3);
            globalWellPlate.DrawWellPlate();



            globalWellPlate.SetWellPlateSize(10, 10);
            globalWellPlate.DrawWellPlate();

            ////globalWellPlate.SetGridColor = Color.FromRgb(209, 232, 247);
            ////globalWellPlate.SetClickColor = Color.FromRgb(97,172,223);
            ////globalWellPlate.SetStrokeColor = Colors.Black;
            //////globalWellPlate.StrokeThickness = 20;
            //////globalWellPlate.TurnCoordinatesOff = true;
            //globalWellPlate.SetMaxHeight = 700;
            ////globalWellPlate.SetMaxWidth = 600;
            ////globalWellPlate.IsRectangle = true;
            //globalWellPlate.IsEditable = true;
            //globalWellPlate.SetWellPlateSize(16, 24); //width, height
            ////globalWellPlate.ColorCoordinate("A2;3"); //multiple coordinates get split with a ';'
            //globalWellPlate.DrawWellPlate();
            //globalWellPlate.ColorCoordinate("A5", Color.FromRgb(222,233,212));
            //globalWellPlate.ColorCoordinate("B5", Color.FromRgb(251, 209, 205));
            ////Debug.WriteLine(globalWellPlate.NumberToCoordinate(-11));
            ////Debug.WriteLine(globalWellPlate.CoordinateToNumber("a2"));
            ////Debug.WriteLine(globalWellPlate.LastClickedCoordinate);
            ////List<string> colorList2 = globalWellPlate.GiveColoredList();
            //List<string> colorList = globalWellPlate.GetColoredCoordinates;
            //foreach (string myItem in colorList)
            //{
            //    Debug.WriteLine($"colored: {myItem}");
            //}


            //List<string> notcolorList = globalWellPlate.GiveNotColoredCoordinates;


            //globalWellPlate.GiveNotColoredList();



        }

        private void ConnTest(object sender, RoutedEventArgs e)
        {
            //make sure the colors get set before the grid size
            //Debug.WriteLine(globalWellPlate.LastClickedCoordinate);


        }
    }
}
