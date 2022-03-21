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

            globalWellPlate.SetGridColor(Colors.Black); 
            globalWellPlate.SetClickColor(Colors.Red);
            //globalWellPlate.SetStrokeColor(Colors.Blue);
            globalWellPlate.SetCircleSize(2F);
            globalWellPlate.SetWellPlateSize(8, 6); //width, height
            globalWellPlate.ColorCoordinate("A2;5"); //multiple coordinates get split with a ';'
            Debug.WriteLine(globalWellPlate.CoordinateConverter("A5"));

        }

        private void ConnTest(object sender, RoutedEventArgs e)
        {
            //make sure the colors get set before the grid size
            

        }
    }
}
