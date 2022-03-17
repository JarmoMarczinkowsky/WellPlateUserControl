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

        //private string _gridColor;

        //private string _clickColor;
        //private string _cboxWellPlateSize;
        //private string _wellPlateSize;
        private string _alphabet;
        private string _createEllipseName;
        //private string _colorCoordinate;
        
        private int _widthWellPlate;
        private int _heightWellPlate;
        private int _shapeSize;
        private int _shapeDistance;
        private int _distanceFromWall;
        private int _loopCounter;

        public bool _setTheGridColor;
        public bool _setTheClickColor;

        private Color _colorConverter;
        private Color _clickColorConverter;
        private Color _defaultGridColor;
        private Color _defaultClickColor;

        private List<string> _coordinatesForColor;
        

        public WellPlateControl()
        {
            InitializeComponent();

            _defaultGridColor = Colors.Black;
            _defaultClickColor = Colors.Red;
            _setTheGridColor = false;
            _setTheClickColor = false;

            rectPlaceHolder.Visibility = Visibility.Hidden;
        }

        private void GenerateWellPlate(object sender, RoutedEventArgs e)
        {


        }

        /// <summary>
        /// <para>Use this method <b>after</b> you've stated the colors</para>
        /// <para>Takes care of the wellplate size</para>
        /// <para>Use: '8,6' will create a grid that is 8 wide and 6 high</para>
        /// </summary>
        /// <param name="inputLength"></param>
        /// <param name="inputWidth"></param>
        /// <returns>True if method succeeds and an out of range error if the values are higher than 26 or smaller than 1</returns>
        public bool SetWellPlateSize(int inputLength, int inputWidth)
        {
            
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

                //gButtonControl.Children.Remove(lblTestLabel);
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
                        if (_setTheGridColor)
                        {
                            ellipse.Fill = new SolidColorBrush(_colorConverter);
                        }
                        else
                        {
                            ellipse.Fill = new SolidColorBrush(_defaultGridColor);
                        }

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

        /// <summary>
        /// <para>Converts the inputted gridcolor to a readable format for the code.</para>
        /// <para>Use: 'Color.[wishedColor]' without square brackets</para>
        /// </summary>
        /// <param name="clickColor"></param>
        /// <returns>True if the color succeeds to be puth inside the variable and an error if it fails to be put inside the variable</returns>
        public bool SetGridColor(Color gridColor)
        {
            Debug.WriteLine("SetGridColor doorgelopen");
            try
            {
                _colorConverter = gridColor; //(Color)ColorConverter.ConvertFromString
                _setTheGridColor = true;
                return true;
            }
            catch
            {
                
                throw new FormatException($"Can't convert color, are you sure you're using a valid color?{Environment.NewLine}https://docs.microsoft.com/en-us/dotnet/api/system.windows.media.colors?view=windowsdesktop-6.0");
            }
        }

        /// <summary>
        /// <para>Converts the inputted clickcolor to a readable format for the code.</para>
        /// <para>Use: 'Color.[wishedColor]' without square brackets</para>
        /// </summary>
        /// <param name="clickColor"></param>
        /// <returns>True if the color succeeds to be puth inside the variable and an error if it fails to be put inside the variable</returns>
        public bool SetClickColor(Color clickColor)
        {
            
            Debug.WriteLine("clickColor doorgelopen");
            try
            {
                _clickColorConverter = clickColor;
                _setTheClickColor = true;
                return true;
            }
            catch
            {
                throw new FormatException($"Can't convert color, are you sure you're using a valid color?{Environment.NewLine}https://docs.microsoft.com/en-us/dotnet/api/system.windows.media.colors?view=windowsdesktop-6.0");
            }
            
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
            string formattedCoordinate;
            _coordinatesForColor = coordinate.Split(";".Trim()).ToList();

            Debug.WriteLine(_clickColorConverter);

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

                            formattedCoordinate = newCoordinate.Trim();

                            //is number
                            if (int.TryParse(formattedCoordinate.Trim(), out int checkNumber))
                            {
                                if (ellipse.Name.Split("_")[1] == formattedCoordinate)
                                {
                                    if (_setTheClickColor)
                                    {
                                        ellipse.Fill = new SolidColorBrush(_clickColorConverter);
                                    }
                                    else
                                    {
                                        ellipse.Fill = new SolidColorBrush(_defaultClickColor);
                                    }
                                    
                                }
                            }

                            //is alphabetic
                            else
                            {
                                _createEllipseName = $"{formattedCoordinate.ToUpper()}";

                                if (ellipse.Name.Split("_")[0] == _createEllipseName)
                                {
                                    if (_setTheClickColor)
                                    {
                                        ellipse.Fill = new SolidColorBrush(_clickColorConverter);
                                    }
                                    else
                                    {
                                        ellipse.Fill = new SolidColorBrush(_defaultClickColor);
                                    }
                                }
                            }
                        }
                    }
                }
                return true;
            }
            catch
            {
                throw new Exception($"Something went wrong with the coordinates. Are you sure you are using coordinates or numbers?{Environment.NewLine}Examples: A5 to color coordinate A5.{Environment.NewLine}5 to color ellipse number 5.{Environment.NewLine}Use ';' to color multiple wells. Example: A5;B5;13 ");
            }
        }


        /// <summary>
        /// <para>If you click an ellipse, it will get the color of the 'clickcolor'.</para>
        /// <para>If you click it again, it will change back to the grid color</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                        if (_setTheClickColor)
                        {
                            //if an ellipse is the first color, convert it to the other color if you click it
                            if (brush.Color == _colorConverter) //_colorConverter
                            {
                                ellipse.Fill = new SolidColorBrush(_clickColorConverter); //_clickColorConverter
                                Debug.WriteLine($"{ellipse.Name}: turned on");
                            }
                            //if an ellipse is the clicked color, convert it to the first color if you click it
                            else
                            {
                                ellipse.Fill = new SolidColorBrush(_colorConverter); //_colorConverter
                                Debug.WriteLine($"{ellipse.Name}: turned off");
                            }
                        }
                        else
                        {
                            //if an ellipse is the first color, convert it to the other color if you click it
                            if (brush.Color == _defaultGridColor) //_colorConverter
                            {
                                ellipse.Fill = new SolidColorBrush(_defaultClickColor); //_clickColorConverter
                                Debug.WriteLine($"{ellipse.Name}: turned on");
                            }
                            //if an ellipse is the clicked color, convert it to the first color if you click it
                            else
                            {
                                ellipse.Fill = new SolidColorBrush(_defaultGridColor); //_colorConverter
                                Debug.WriteLine($"{ellipse.Name}: turned off");
                            }
                        }
                        
                    }

                }
            }
            
        }
    }
}
