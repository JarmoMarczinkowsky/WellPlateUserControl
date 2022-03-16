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
        
        private int _widthWellPlate;
        private int _heightWellPlate;
        private int _shapeSize;
        private int _shapeDistance;
        private int _distanceFromWall;
        private int _loopCounter;
        private Color _colorConverter;
        private Color _clickColorConverter;
        private List<string> _coordinatesForColor;
        

        public WellPlateControl()
        {
            InitializeComponent();

            GetValues();



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

            //rectPlaceHolder.Visibility = Visibility.Hidden;

            Debug.WriteLine("Run Start");


        }

        private void GenerateWellPlate(object sender, RoutedEventArgs e)
        {


        }

        public void GetValues()
        {
            Debug.WriteLine("GetValues lukt");
            //puts the content of the comboboxes in three different strings
            //_cboxGridColor = cboxWellColor.SelectedItem as string;
            //_cboxClickColor = cboxWellClickColor.SelectedItem as string;
            //_cboxWellPlateSize = cboxWellSize.SelectedItem as string;

            //removes the need for the combobox: wellplateSize 
            //_wellPlateSize = "10x12"; //height x width

            //removes the need for the combobox: gridColor
            //_gridColor = "Black";

            //removes the need for the combobox: clickColor
            //_clickColor = "Red";

            //removes the need for the textbox: CoordinateToColor
            //_colorCoordinate = "A2";

            //ColorValidator(_gridColor);

            //prepares wellplate size
            //_heightWellPlate = Convert.ToInt32(_wellPlateSize.Split("x")[0]);
            //_widthWellPlate = Convert.ToInt32(_wellPlateSize.Split("x")[1]);


            //prepares the coordinate to color system
            //_coordinatesForColor = _colorCoordinate.Split(";".Trim()).ToList();


        }

        public bool SetWellPlateSize(int inputLength, int inputWidth)
        {
            lblTestLabel.Content = "De labeltest is gelukt";

            Debug.WriteLine(lblTestLabel.Content);
            rectPlaceHolder.Fill = new SolidColorBrush(Colors.Red);
            //MessageBox.Show("Test");

            Debug.WriteLine("-----------------------------------\r\nHier\r\n--------------------------------------------");
            if (inputLength > 0 && inputLength < 27 && inputWidth > 0 && inputWidth < 27)
            {
                _heightWellPlate = inputWidth;
                _widthWellPlate = inputLength;
                
                //sets the values of the shapes
                _shapeSize = 15;
                _shapeDistance = 1;
                _distanceFromWall = 5;

                //preparation for the coordinate system
                _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                _loopCounter = -1;

                gButtonControl.Children.Remove(lblTestLabel);
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
                Debug.WriteLine("SetWellPlateSize doorgelopen");
                return true;
            }
            else
            {
                Debug.WriteLine("False");
                throw new ArgumentOutOfRangeException($"Number can't be bigger than 26 or smaller than 1: length = {inputLength}, width = {inputWidth}");
            }
        }

        public bool SetGridColor(string gridColor)
        {
            string wellColor;
            wellColor = ColorValidator(gridColor);
            Debug.WriteLine("SetGridColor doorgelopen");
            try
            {
                _clickColorConverter = (Color)ColorConverter.ConvertFromString(wellColor);
                return true;
            }
            catch
            {
                throw new Exception();
            }
            
        }

        public bool SetClickColor(string clickColor)
        {
            string wellColor = ColorValidator(clickColor);
            //return true;
            Debug.WriteLine("clickColor doorgelopen");
            try
            {
                _clickColorConverter = (Color)ColorConverter.ConvertFromString(wellColor);
                return true;
            }
            catch
            {
                throw new Exception();
            }
            
        }

        public string ColorValidator(string inputColor)
        {
            string wellColor = "";
            if (inputColor.Contains(",")) //Check for rgb
            {
                string[] splitNewColor = inputColor.Split(",");
                int red = int.Parse(splitNewColor[0].Trim());
                int green = int.Parse(splitNewColor[1].Trim());
                int blue = int.Parse(splitNewColor[2].Trim());

                wellColor = $"#{red:X2}{green:X2}{blue:X2}";
                //Debug.WriteLine($"Hex: {inputColor}");

                
            }

            else if (char.IsLetter(inputColor.FirstOrDefault()))
            {
                wellColor = char.ToUpper(inputColor.First()) + inputColor.Substring(1).ToLower();

            }

            else
            {
                wellColor = inputColor;
            }
            Debug.WriteLine("ColorValidator doorgelopen");
            return wellColor;
            
        }

        /// <summary>
        /// <para>Give a number or a coordinate and the coordinate will get the 'click' color.</para>
        /// <para>Example: 'A4' colors the 4th circle on the upper row.</para>
        /// <para>Example 2: '4' also colors the 4th circle on the upper row</para>
        /// <para>Use ';' to highlight more coordinates. Works with numbers and coordinates</para>
        /// <para>Example: 'A1;B3;B5;20' wil highlight all these coordinates</para>
        /// </summary>
        /// <param name="coordinate"></param>
        /// <returns>True or false</returns>
        public bool ColorCoordinate(string coordinate)
        {
            _coordinatesForColor = coordinate.Split(";".Trim()).ToList();

            try
            {
                //coordinate to color system
                foreach (string newCoordinate in _coordinatesForColor)
                {
                    if (newCoordinate != "")
                    {
                        foreach (object child in gGenerateWellPlate.Children)
                        {
                            Ellipse ellipse = child as Ellipse;

                            //is number
                            if (int.TryParse(newCoordinate.Trim(), out int checkNumber))
                            {
                                if (ellipse.Name.Split("_")[1] == newCoordinate)
                                {
                                    ellipse.Fill = new SolidColorBrush(_clickColorConverter);
                                }
                            }

                            //is alphabetic
                            else
                            {
                                _createEllipseName = $"{newCoordinate.ToUpper()}";

                                if (ellipse.Name.Contains(_createEllipseName))
                                {
                                    ellipse.Fill = new SolidColorBrush(_clickColorConverter);
                                }
                            }
                        }
                    }
                }
                return true;
            }
            catch
            {
                throw new Exception();
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
            Debug.WriteLine("Click");
        }

        private void CoordinateToColor(object sender, RoutedEventArgs e)
        {
            //gets all the values of the comboboxes
            GetValues();





        }
    }
}
