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

            globalWellPlate.IsEditable();
            globalWellPlate.SetGridColor(Colors.Black); 
            globalWellPlate.SetClickColor(Colors.Red);
            //globalWellPlate.SetStrokeColor(Colors.Blue);
            globalWellPlate.SetCircleSize(3.9);
            //globalWellPlate.IsRectangle();
            globalWellPlate.SetWellPlateSize(6, 2); //width, height
            //globalWellPlate.ColorCoordinate("A2;5"); //multiple coordinates get split with a ';'
            globalWellPlate.ColorCoordinate(3, Colors.Green);
            globalWellPlate.ColorCoordinate("A5", Colors.DarkBlue);
            globalWellPlate.ColorCoordinate("B1");
            Debug.WriteLine(globalWellPlate.CoordinateConverter(4));
            Debug.WriteLine(globalWellPlate.GetLastClickedCoordinate());
        }

        private void ConnTest(object sender, RoutedEventArgs e)
        {
            //make sure the colors get set before the grid size
            

        }
    }
}
