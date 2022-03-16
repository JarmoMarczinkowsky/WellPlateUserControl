using System;
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

            

        }

        private void ConnTest(object sender, RoutedEventArgs e)
        {
            //make sure the colors get set before the grid size
            globalWellPlate.SetGridColor("Black");
            globalWellPlate.SetClickColor("Red");
            globalWellPlate.SetWellPlateSize(8, 6);
            globalWellPlate.ColorCoordinate("A1;A2;B1;600"); //multiple coordinates get split with a ';'
            


        }
    }
}
