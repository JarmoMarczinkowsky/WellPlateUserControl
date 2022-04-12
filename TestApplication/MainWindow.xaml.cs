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

            globalWellPlate.IsEditable = true;
            //globalWellPlate.SetStrokeColor = Colors.Black;
            //globalWellPlate.SetStrokeThickness = 80;
            globalWellPlate.SetWellPlateSize(4, 4);
            //globalWellPlate.SetWellPlateSize(4,3);
            //globalWellPlate.SetMaxWidth = 800;
            globalWellPlate.DrawWellPlate();
            globalWellPlate.ColorCoordinate(4);
            globalWellPlate.ColorCoordinate("A1");
            globalWellPlate.ColorCoordinate("C4", Colors.Red);
            globalWellPlate.ColorCoordinate(5, Colors.Green);

        }

        private void ConnTest(object sender, RoutedEventArgs e)
        {
            //placeholder


        }
    }
}
