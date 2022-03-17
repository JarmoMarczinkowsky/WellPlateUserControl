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

            //globalWellPlate.SetGridColor(Colors.Green); //https://docs.microsoft.com/en-us/dotnet/api/system.windows.media.colors?view=windowsdesktop-6.0
            //globalWellPlate.SetClickColor(Colors.GreenYellow);
            globalWellPlate.SetWellPlateSize(8, 6); //width, height
            globalWellPlate.ColorCoordinate("C3;C5;E2;F3;F4;F5;E6"); //multiple coordinates get split with a ';'


        }

        private void ConnTest(object sender, RoutedEventArgs e)
        {
            //make sure the colors get set before the grid size
            

        }
    }
}
