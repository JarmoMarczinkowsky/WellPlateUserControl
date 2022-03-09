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

namespace WellPlateUserControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _cboxGridColor;
        private string _cboxClickColor;
        private string _cboxWellPlateSize;
        private string _alphabet;
        private int _widthWellPlate;
        private int _heightWellPlate;
        private int _shapeSize;
        private int _shapeDistance;
        private int _distanceFromWall;
        private int _loopCounter;
        
        public MainWindow()
        {
            InitializeComponent();

            List<string> sizes = new List<string>() { "4x6", "6x8", "8x12", "16x24" };
            List<string> colors = new List<string>() { "Aqua", "Beige", "Black", "Blue", "Brown", "Gray", "Green", "Pink", "Red", "Yellow" };

            cboxWellColor.ItemsSource = colors;
            cboxWellClickColor.ItemsSource = colors;
            cboxWellSize.ItemsSource = sizes;

            cboxWellColor.SelectedItem = cboxWellColor.Items[3];
            cboxWellClickColor.SelectedItem = cboxWellClickColor.Items[8];
            cboxWellSize.SelectedItem = cboxWellSize.Items[2];
            
        }

        private void cboxWellSize_Selected(object sender, RoutedEventArgs e)
        {
            
        }

        private void GenerateWellPlate(object sender, RoutedEventArgs e)
        {
            //gets al the values from the comboboxes
            GetComboBoxes();

            //sets the values of the shapes
            _shapeSize = 15;
            _shapeDistance = 1;
            _distanceFromWall = 15;

            //preparation for the coordinate system
            _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            //clears the previous shapes
            gGenerateWellPlate.Children.Clear();

            //generates the shapes
            for (int height = 0; height < _heightWellPlate; height++)
            {
                for (int width = 0; width < _widthWellPlate; width++)
                {
                    _loopCounter++;

                    Ellipse ellipse = new Ellipse();
                    ellipse.VerticalAlignment = VerticalAlignment.Bottom;
                    ellipse.HorizontalAlignment = HorizontalAlignment.Left;
                    ellipse.Fill = new SolidColorBrush(Colors.Black);
                    ellipse.Width = _shapeSize;
                    ellipse.Height = _shapeSize;
                    ellipse.Name = $"{_alphabet[height]}{width + 1}_{_loopCounter + 1}"; //example 'a5_5'                    
                    ellipse.Margin = new Thickness(_distanceFromWall + width * _shapeSize * _shapeDistance, 0, 0, _distanceFromWall + height * _shapeSize);

                    Debug.WriteLine(ellipse.Name);

                    gGenerateWellPlate.Children.Add(ellipse);
                }
            } 

        }

        public void GetComboBoxes()
        {
            //puts the content of the comboboxes in three different strings
            _cboxGridColor = cboxWellColor.SelectedItem as string;
            _cboxClickColor = cboxWellClickColor.SelectedItem as string;
            _cboxWellPlateSize = cboxWellSize.SelectedItem as string;

            //Debug tijd
            Debug.WriteLine("size is: " + _cboxWellPlateSize);

            //prepares wellplate size
            _widthWellPlate = Convert.ToInt32(_cboxWellPlateSize.Split("x")[0]);
            _heightWellPlate = Convert.ToInt32(_cboxWellPlateSize.Split("x")[1]);
        }
    }
}
