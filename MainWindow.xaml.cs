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
        private string _createEllipseName;
        private int _widthWellPlate;
        private int _heightWellPlate;
        private int _shapeSize;
        private int _shapeDistance;
        private int _distanceFromWall;
        private int _loopCounter;
        
        
        public MainWindow()
        {
            InitializeComponent();

            //creates the lists for the comboboxes
            List<string> sizes = new List<string>() { "4x6", "6x8", "8x12", "16x24" };
            List<string> colors = new List<string>() { "Aqua", "Beige", "Black", "Blue", "Brown", "Gray", "Green", "Pink", "Red", "Yellow" };

            //fills the comboboxes
            cboxWellColor.ItemsSource = colors;
            cboxWellClickColor.ItemsSource = colors;
            cboxWellSize.ItemsSource = sizes;

            //makes the comboboxes select an item
            cboxWellColor.SelectedItem = cboxWellColor.Items[2]; //selects 'Black'
            cboxWellClickColor.SelectedItem = cboxWellClickColor.Items[8]; //selects 'Red'
            cboxWellSize.SelectedItem = cboxWellSize.Items[2]; //selects '8x12'

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
            _distanceFromWall = 5;

            //preparation for the coordinate system
            _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            _loopCounter = -1;

            //converts the colors
            var _colorConverter = (Color)ColorConverter.ConvertFromString(_cboxGridColor);
            var _clickColorConverter = (Color)ColorConverter.ConvertFromString(_cboxClickColor);

            //clears the previous shapes
            gGenerateWellPlate.Children.Clear();

            //generates the shapes
            for (int height = 0; height < _heightWellPlate; height++)
            {
                for (int width = 0; width < _widthWellPlate; width++)
                {
                    _loopCounter++;

                    //creates ellipse
                    Ellipse ellipse = new Ellipse();

                    //takes care of the alignment
                    ellipse.VerticalAlignment = VerticalAlignment.Bottom;
                    ellipse.HorizontalAlignment = HorizontalAlignment.Left;

                    //makes it  the color from the combobox
                    ellipse.Fill = new SolidColorBrush(_colorConverter);
                    
                    //makes the ellipse a certain size
                    ellipse.Width = _shapeSize;
                    ellipse.Height = _shapeSize;

                    //gives the ellipse a name. It starts with the coordinates followed by a underscore with the number of the ellipse
                    ellipse.Name = $"{_alphabet[height]}{width + 1}_{_loopCounter + 1}"; //example 'a5_5'                    
                    
                    //takes care of the position of the ellipse
                    ellipse.Margin = new Thickness(
                        _distanceFromWall + width * _shapeSize * _shapeDistance, //left
                        0,  //up
                        0, //right
                        _distanceFromWall + _heightWellPlate * _shapeSize - (_distanceFromWall + height * _shapeSize)); //down

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

            //prepares wellplate size
            _heightWellPlate = Convert.ToInt32(_cboxWellPlateSize.Split("x")[0]);
            _widthWellPlate = Convert.ToInt32(_cboxWellPlateSize.Split("x")[1]);
        }

        private void ClickForColor(object sender, MouseButtonEventArgs e)
        {
            //gets the values of the comboboxes
            GetComboBoxes();

            //converts the colors
            var colorConverter = (Color)ColorConverter.ConvertFromString(_cboxGridColor);
            var clickColorConverter = (Color)ColorConverter.ConvertFromString(_cboxClickColor);

            //loops through each of the ellipses
            foreach (object child in gGenerateWellPlate.Children)
            {
                Ellipse ellipse = child as Ellipse;
                
                if (((Ellipse)child).IsMouseOver)
                {
                    Debug.WriteLine(ellipse.Name);

                    SolidColorBrush brush = ellipse.Fill as SolidColorBrush;
                    if (brush != null)
                    {

                        //if an ellipse is the first color, convert it to the other color if you click it
                        if (brush.Color == colorConverter)
                        {
                            ellipse.Fill = new SolidColorBrush(clickColorConverter);
                        }

                        //if an ellipse is the clicked color, convert it to the first color if you click it
                        else
                        {
                            ellipse.Fill = new SolidColorBrush(colorConverter);
                        }
                    }

                }
            }
        }

        private void CoordinateToColor(object sender, RoutedEventArgs e)
        {
            //gets all the values of the comboboxes
            GetComboBoxes();

            var clickColorConverter = (Color)ColorConverter.ConvertFromString(_cboxClickColor);

            if (txbCoordinateToColor.Text != "")
            {
                foreach (object child in gGenerateWellPlate.Children)
                {
                    Ellipse ellipse = child as Ellipse;

                    //is number
                    if (int.TryParse(txbCoordinateToColor.Text.Trim(), out int checkNumber))
                    {
                        if (ellipse.Name.Split("_")[1] == txbCoordinateToColor.Text)
                        {
                            ellipse.Fill = new SolidColorBrush(clickColorConverter);
                        }
                    }

                    //is alphabetic
                    else
                    {
                        _createEllipseName = $"{txbCoordinateToColor.Text.ToUpper()}";

                        if (ellipse.Name.Contains(_createEllipseName))
                        {
                            ellipse.Fill = new SolidColorBrush(clickColorConverter);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Input field can't be empty", MessageBoxButton.OK.ToString());
            }

            
        }
    }
}
