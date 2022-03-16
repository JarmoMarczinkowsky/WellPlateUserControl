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
            _wellPlate.SetWellPlateSize(6, 8);
            _wellPlate.SetGridColor("Black");
            _wellPlate.SetClickColor("Red");

            Label myLabel = new Label();
            myLabel.Content = "Net nieuw";
            gRefreshTest.Children.Add(myLabel);
        
        }
    }
}
