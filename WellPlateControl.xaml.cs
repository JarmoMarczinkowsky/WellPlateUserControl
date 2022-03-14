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
    /// Interaction logic for WellPlateControl.xaml
    /// </summary>
    public partial class WellPlateControl : UserControl
    {
        
        private string _gridColor;

        private string _clickColor;
        //private string _cboxWellPlateSize;
        private string _wellPlateSize;
        private string _alphabet;
        private string _createEllipseName;
        private string _colorCoordinate;
        private string _victim;
        private int _widthWellPlate;
        private int _heightWellPlate;
        private int _shapeSize;
        private int _shapeDistance;
        private int _distanceFromWall;
        private int _loopCounter;
        private Color _colorConverter;
        private Color _clickColorConverter;
        private List<string> _coordinatesForColor;
        private bool _isValid;

        public WellPlateControl()
        {
            InitializeComponent();

            GetValues();

            if (_isValid)
            {
                //creates the lists for the comboboxes
                //List<string> sizes = new List<string>() { "4x6", "6x8", "8x12", "16x24" };
                //List<string> colors = new List<string>() { "Aqua", "Beige", "Black", "Blue", "Brown", "Gray", "Green", "Pink", "Red", "Yellow" };

                //_wellPlateSize = "6x8";

                //fills the comboboxes
                //cboxWellColor.ItemsSource = colors;
                //cboxWellClickColor.ItemsSource = colors;
                //cboxWellSize.ItemsSource = sizes;

                //makes the comboboxes select an item
                //cboxWellColor.SelectedItem = cboxWellColor.Items[2]; //selects 'Black'
                //cboxWellClickColor.SelectedItem = cboxWellClickColor.Items[8]; //selects 'Red'
                //cboxWellSize.SelectedItem = cboxWellSize.Items[2]; //selects '8x12'

                rectPlaceHolder.Visibility = Visibility.Hidden;

                //takes care of the grid
                //sets the values of the shapes
                _shapeSize = 15;
                _shapeDistance = 1;
                _distanceFromWall = 5;

                //preparation for the coordinate system
                _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                _loopCounter = -1;

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

                //coordinate to color system
                foreach (string coordinate in _coordinatesForColor)
                {
                    if (_colorCoordinate != "")
                    {
                        foreach (object child in gGenerateWellPlate.Children)
                        {
                            Ellipse ellipse = child as Ellipse;

                            //is number
                            if (int.TryParse(coordinate.Trim(), out int checkNumber))
                            {
                                if (ellipse.Name.Split("_")[1] == coordinate)
                                {
                                    ellipse.Fill = new SolidColorBrush(_clickColorConverter);
                                }
                            }

                            //is alphabetic
                            else
                            {
                                _createEllipseName = $"{coordinate.ToUpper()}";

                                if (ellipse.Name.Contains(_createEllipseName))
                                {
                                    ellipse.Fill = new SolidColorBrush(_clickColorConverter);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show($"{_victim} can't be bigger than 26");
            }

        }

        private void GenerateWellPlate(object sender, RoutedEventArgs e)
        {
            

        }
        
        public void GetValues()
        {
            //puts the content of the comboboxes in three different strings
            //_cboxGridColor = cboxWellColor.SelectedItem as string;
            //_cboxClickColor = cboxWellClickColor.SelectedItem as string;
            //_cboxWellPlateSize = cboxWellSize.SelectedItem as string;

            //removes the need for the combobox: wellplateSize 
            _wellPlateSize = "10x12"; //height x width

            //removes the need for the combobox: gridColor
            _gridColor = "Black";

            

            //removes the need for the combobox: clickColor
            _clickColor = "Red";

            //removes the need for the textbox: CoordinateToColor
            _colorCoordinate = "A2";

            ColorValidator(_gridColor, _clickColor);

            //prepares wellplate size
            _heightWellPlate = Convert.ToInt32(_wellPlateSize.Split("x")[0]);
            _widthWellPlate = Convert.ToInt32(_wellPlateSize.Split("x")[1]);

            if (_heightWellPlate < 27 && _widthWellPlate < 27)
            {
                _isValid = true;
            }
            else
            {
                if (_heightWellPlate > 26)
                {
                    _victim = "height";
                }
                else if (_widthWellPlate > 26)
                {
                    _victim = "width";
                    
                }
                _isValid = false;
            }

            //prepares the coordinate to color system
            _coordinatesForColor = _colorCoordinate.Split(";".Trim()).ToList();
            
            
        }

        public void ColorValidator(string wellColor, string clickColor)
        {
            //if (char.IsLetter(wellColor[0]))
            //{

            //}

            try
            {
                _colorConverter = (Color)ColorConverter.ConvertFromString(wellColor);
                _clickColorConverter = (Color)ColorConverter.ConvertFromString(clickColor);

            }
            catch
            {
                MessageBox.Show("Can't convert color");
            }
        }

        

        private void ClickForColor(object sender, MouseButtonEventArgs e)
        {
            

            //loops through each of the ellipses
            foreach (object child in gGenerateWellPlate.Children)
            {
                Ellipse ellipse = child as Ellipse;

                if (((Ellipse)child).IsMouseOver)
                {
                    SolidColorBrush brush = ellipse.Fill as SolidColorBrush;
                    if (brush != null)
                    {
                        //if an ellipse is the first color, convert it to the other color if you click it
                        if (brush.Color == _colorConverter)
                        {
                            ellipse.Fill = new SolidColorBrush(_clickColorConverter);
                            Debug.WriteLine($"{ellipse.Name}: turned on");
                        }
                        //if an ellipse is the clicked color, convert it to the first color if you click it
                        else
                        {
                            ellipse.Fill = new SolidColorBrush(_colorConverter);
                            Debug.WriteLine($"{ellipse.Name}: turned off");
                        }
                    }

                }
            }
        }

        private void CoordinateToColor(object sender, RoutedEventArgs e)
        {
            //gets all the values of the comboboxes
            GetValues();

            

            
            
        }
    }
}
