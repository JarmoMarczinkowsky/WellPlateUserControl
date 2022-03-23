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
        private string _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private string _createEllipseName;
        private string _lastClickedCoordinate;

        private int _widthWellPlate;
        private int _heightWellPlate;
        private int _shapeSize = 15;

        private int _distanceFromWall = 5;
        private int _loopCounter;
        private int _amountColored;
        private int _amountNotColored;

        private double _circleSizeMultiplier = 1;
        private float _shapeDistance = 1;

        private bool _setTheGridColor;
        private bool _setTheClickColor;
        private bool _setStrokeColor;
        private bool _setRectangle;
        private bool _getLastCoordinateActive;
        private bool _isWellEditable;

        private Color _colorConverter;
        private Color _clickColorConverter;
        private Color _strokeColor;
        private Color _defaultGridColor;
        private Color _defaultClickColor;
        //private Color _defaultStrokeColor;

        private List<string> _coordinatesForColor;
        private List<string> _coordinates = new List<string>();
        private List<string> _coloredCoordinates = new List<string>();
        private List<string> _notColoredCoordinates = new List<string>();


        public WellPlateControl()
        {
            InitializeComponent();

            _defaultGridColor = Colors.Black;
            _defaultClickColor = Colors.Red;
            _setTheGridColor = false;
            _setTheClickColor = false;

            rectPlaceHolder.Visibility = Visibility.Hidden;


        }

        /// <summary>
        /// <para>Use this method <b>after</b> you've stated the colors</para>
        /// <para>Takes care of the wellplate size</para>
        /// <example>Use: '8,6' will create a grid that is 8 wide and 6 high</example>
        /// </summary>
        /// <param name="inputWidth">The width that the grid is going to be</param>
        /// <param name="inputHeight">The height that the grid is going to be</param>
        /// <returns>True if method succeeds and an out of range error if a values are higher than 26 or smaller than 1</returns>
        public bool SetWellPlateSize(int inputWidth, int inputHeight)
        {
            if (inputWidth > 0 && inputWidth < _alphabet.Length && inputHeight > 0 && inputHeight < _alphabet.Length)
            {
                _heightWellPlate = inputHeight;
                _widthWellPlate = inputWidth;

                _coordinates.Clear();

                //sets the values of the shapes
                //_shapeSize = 15;
                //_shapeDistance = 1;
                //_distanceFromWall = 5;

                //preparation for the coordinate system
                //_alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
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
                        Rectangle ellipse = new Rectangle()
                        {
                            VerticalAlignment = VerticalAlignment.Bottom,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            Width = _shapeSize * _circleSizeMultiplier,
                            Height = _shapeSize * _circleSizeMultiplier,
                            Name = $"{_alphabet[height]}{width + 1}_{1 + height + _heightWellPlate * width}", //$"{_alphabet[height]}{width + 1}_{_loopCounter + 1}"
                            //Stroke = new SolidColorBrush(_strokeColor),
                        };
                        _coordinates.Add(ellipse.Name);
                        //makes it  the color from the combobox
                        if (_setTheGridColor)
                        {
                            ellipse.Fill = new SolidColorBrush(_colorConverter);
                        }
                        else
                        {
                            ellipse.Fill = new SolidColorBrush(_defaultGridColor);
                        }

                        if (_setStrokeColor)
                        {
                            ellipse.Stroke = new SolidColorBrush(_strokeColor);
                            ellipse.StrokeThickness = ellipse.Width * 0.08; //0.08 is around 16% of circle 
                        }

                        //checks if the user wants a rectangle. Otherwise it will round the rectangles so it looks like circles.
                        if (_setRectangle)
                        {
                            ellipse.RadiusX = 0;
                            ellipse.RadiusY = 0;
                            _shapeDistance = 1.05F;
                        }
                        else
                        {
                            ellipse.RadiusX = 150;
                            ellipse.RadiusY = 150;
                        }


                        //takes care of the position of the ellipse
                        ellipse.Margin = new Thickness(
                            (_distanceFromWall + width * _shapeSize * _shapeDistance) * _circleSizeMultiplier, //left
                            0,  //up
                            0, //right
                            (_distanceFromWall + _heightWellPlate * _shapeSize - (_distanceFromWall + height * _shapeSize)) * _circleSizeMultiplier * _shapeDistance); //down

                        _coordinates.Add(ellipse.Name);
                        //_notColoredCoordinates.Add(ellipse.Name);


                        gGenerateWellPlate.Children.Add(ellipse);
                    }
                }

                return true;
            }
            else
            {

                throw new ArgumentOutOfRangeException($"Number can't be bigger than 26 or smaller than 1: length = {inputWidth}, width = {inputHeight}");
            }
        }

        /// <summary>
        /// <para>Converts the inputted gridcolor to a readable format for the code.</para>
        /// <example>Use: 'Colors.[wishedColor]' without square brackets</example>
        /// </summary>
        /// <param name="gridColor">the color of the grid</param>
        /// <returns>True if the color succeeds to be puth inside the variable and an error if it fails to be put inside the variable</returns>
        public bool SetGridColor(Color gridColor)
        {

            try
            {
                _colorConverter = gridColor; //(Color)ColorConverter.ConvertFromString
                _setTheGridColor = true;
                return true;
            }
            catch (Exception ex)
            {
                //ex.Message($"Can't convert color, are you sure you're using a valid color?");
                MessageBox.Show(ex + Environment.NewLine + "Can't convert color, are you sure you're using a valid color?");
                return false;
            }
        }

        /// <summary>
        /// <para>Converts the inputted clickcolor to a readable format for the code.</para>
        /// <example>Use: 'Colors.[wishedColor]' without square brackets</example>
        /// </summary>
        /// <param name="clickColor">the color of the wells that you see when you click on them. Is also used for the coordinate system colors</param>
        /// <returns>True if the color succeeds to be puth inside the variable and an error if it fails to be put inside the variable</returns>
        public bool SetClickColor(Color clickColor)
        {
            try
            {
                _clickColorConverter = clickColor;
                _setTheClickColor = true;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + $"{Environment.NewLine}Can't convert color, are you sure you're using a valid color ?");
                return false;

            }

        }

        /// <summary>
        /// <para>Give a number or a coordinate and the coordinate will get the 'click' color.</para>
        /// <para>Use '<b>;</b>' to highlight more coordinates. Works with numbers and coordinates</para>
        /// <para><b>Example:</b> 'A4' colors the 4th circle on the upper row.</para>
        /// <para><b>Example 2:</b> '4' also colors the 4th circle on the upper row</para>
        /// <para><b>Example:</b> 'A1;B3;B5;20' wil highlight all these coordinates</para>
        /// </summary>
        /// <param name="coordinate">the coordinate that is about to get colored. Put an ';' in it if you need to color more coordinates</param>
        /// <returns>True if everything succeeds or false if the code fails</returns>
        public bool ColorCoordinate(string coordinate)
        {
            string formattedCoordinate;
            _coordinatesForColor = coordinate.Split(";".Trim()).ToList();

            //Debug.WriteLine(_clickColorConverter);

            try
            {
                //coordinate to color system
                foreach (string newCoordinate in _coordinatesForColor)
                {
                    if (newCoordinate != "")
                    {
                        foreach (object child in gGenerateWellPlate.Children)
                        {
                            Rectangle ellipse = child as Rectangle;

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
            catch (Exception ex)
            {
                MessageBox.Show(ex + $"{Environment.NewLine}Something went wrong with the coordinates. Are you sure you are using coordinates or numbers?");
                return false;


            }
        }
        /// <summary>
        /// <para>Set <b>after</b> the WellPlateSize</para>
        /// <para>This function is being used to color a well based on the corresponding coordinate and color.</para>
        /// <example>Example: '"A5",Colors.Aqua' will color coordinate A5 with the color 'Aqua'</example>
        /// </summary>
        /// <param name="coordinate">The coordinate that needs to be colored. For example: "A5" or "D4" </param>
        /// <param name="chosenColor">The color that the coordinate needs to be colored. For example Colors.Aqua to make the color Aqua</param>
        /// <returns>True if it succeeds and false if it doesn't succeed in coloring the correct coordinate</returns>
        public bool ColorCoordinate(string coordinate, Color chosenColor)
        {
            try
            {
                foreach (object child in gGenerateWellPlate.Children)
                {
                    Rectangle ellipse = child as Rectangle;
                    if (ellipse.Name.Split("_")[0] == coordinate.ToUpper().Trim())
                    {
                        ellipse.Fill = new SolidColorBrush(chosenColor);
                        
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + $"{Environment.NewLine}");
                return false;
            }
        }

        /// <summary>
        /// <para>Set <b>after</b> the WellPlateSize</para>
        /// <para>This function is being used to color a well based on the corresponding coordinate and color.</para>
        /// <example>Example: '"A5",Colors.Aqua' will color coordinate A5 with the color 'Aqua'</example>
        /// </summary>
        /// <param name="coordinate">The number that a coordinate has that needs to be colored. For example: 3 or 52 </param>
        /// <param name="chosenColor">The color that the coordinate needs to be colored. For example Colors.Aqua to make the color Aqua</param>
        /// <returns>True if it succeeds and false if it doesn't succeed in coloring the correct coordinate</returns>
        public bool ColorCoordinate(int coordinate, Color chosenColor)
        {
            try
            {
                foreach (object child in gGenerateWellPlate.Children)
                {
                    Rectangle ellipse = child as Rectangle;
                    if (ellipse.Name.Split("_")[1] == coordinate.ToString())
                    {
                        ellipse.Fill = new SolidColorBrush(chosenColor);
                        
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + $"{Environment.NewLine}");
                return false;
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
                Rectangle ellipse = child as Rectangle;

                if (((Rectangle)child).IsMouseOver)
                {
                    SolidColorBrush brush = ellipse.Fill as SolidColorBrush;
                    if (brush != null)
                    {
                        _lastClickedCoordinate = $"{ellipse.Name.Split("_")[0]}";
                        if (_setTheClickColor && _isWellEditable)
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
                        else if (_isWellEditable)
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

        /// <summary>
        /// <para>Set this before the wellplatesize</para>
        /// <para>Sets the size of the circles. It works as a multiplier</para>
        /// <example>So '2' is 2 times as big as the circles are normally and '0.5' is half so big as it is normally</example>
        /// </summary>
        /// <param name="circleSizeMultiplier"></param>
        public bool SetCircleSize(double circleSizeMultiplier)
        {
            try
            {
                _circleSizeMultiplier = circleSizeMultiplier;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + $"{Environment.NewLine}Could not convert the value for circlesize. Are you sure you are using a float?");
                return false;
            }


        }

        /// <summary>
        /// <para>Set <b>before</b> the wellplatesize</para>
        /// <para>Used to give an outline color to the circles in the wellplate</para>
        /// <example>Use: Colors.[wishedColor] without brackets</example>
        /// </summary>
        /// <param name="strokeColor"></param>
        /// <returns>True if succeeds or false if it doesn't succeed</returns>
        public bool SetStrokeColor(Color strokeColor)
        {
            try
            {
                _strokeColor = strokeColor;
                _setStrokeColor = true;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + $"{Environment.NewLine}Could not convert color. Are you sure you are using a valid color?");
                return false;
            }
        }

        /// <summary>
        /// <para>Input a number and get the coordinate it belongs to</para>
        /// <example>Example: '4' in a 8x6 returns D1</example>
        /// </summary>
        /// <param name="coordinate">Integer that gets used to convert to the correct coordinate</param>
        /// <returns>String with the coordinate the number is linked to. If it doesn't find anything, it will return "nothing found"</returns>
        public string CoordinateConverter(int coordinate)
        {
            foreach (string loopedCoordinate in _coordinates)
            {
                if (loopedCoordinate.Split("_")[1].Trim() == coordinate.ToString())
                {
                    return $"{loopedCoordinate.Split("_")[0]}";
                }
            }
            return "nothing found";
        }

        /// <summary>
        /// <para>Input a coordinate and get the number it belongs to</para>
        /// <example>Example: 'D1' in a 8x6 grid returns 4</example>
        /// </summary>
        /// <param name="coordinate">String that gets used to convert to the correct number</param>
        /// <returns>Integer with the number the coordinate is linked to. If it doesn't find anything, it will return '-1'</returns>
        public int CoordinateConverter(string coordinate)
        {
            foreach (string loopedCoordinate in _coordinates)
            {
                if (loopedCoordinate.Split("_")[0] == coordinate.ToUpper())
                {
                    return Convert.ToInt32(loopedCoordinate.Split("_")[1].Trim());
                }
            }
            return -1;
        }

        /// <summary>
        /// <para>Set this function <b>after</b> WellPlateSize.</para>
        /// <para>Gives a list of every colored well.</para>
        /// </summary>
        /// <returns>A list of every colored well</returns>
        public List<string> GiveColoredList()
        {
            UpdateColoredList();

            Debug.WriteLine(_coloredCoordinates.Count);
            return _coloredCoordinates;
        }

        /// <summary>
        /// <para>Set this function <b>after</b> WellPlateSize.</para>
        /// <para>Gives a list of every not-colored well.</para>
        /// </summary>
        /// <returns>A list of every not-colored well</returns>
        public List<string> GiveNotColoredList()
        {
            UpdateColoredList();
            _notColoredCoordinates.Clear();

            foreach (object child in gGenerateWellPlate.Children)
            {
                Rectangle ellipse = child as Rectangle;

                if (!_coloredCoordinates.Contains(ellipse.Name.Split("_")[0].Trim()))
                {
                    _notColoredCoordinates.Add(ellipse.Name.Split("_")[0]);
                }
            }
            
            Debug.WriteLine(_notColoredCoordinates.Count);
            return _notColoredCoordinates;
        }

        private void UpdateColoredList()
        {
            _coloredCoordinates.Clear();

            foreach (object child in gGenerateWellPlate.Children)
            {
                Rectangle ellipse = child as Rectangle;
                SolidColorBrush brush = ellipse.Fill as SolidColorBrush;

                if (brush != null)
                {
                    if (_setTheGridColor)
                    {
                        if (brush.Color != _defaultGridColor)
                        {
                            _coloredCoordinates.Add(ellipse.Name.Split("_")[0]);
                        }

                    }
                    else
                    {
                        if (brush.Color != _colorConverter)
                        {
                            _coloredCoordinates.Add(ellipse.Name.Split("_")[0]);
                        }
                    }

                }
            }
            
        }

        /// <summary>
        /// <para>Set after the WellPlateSize.</para>
        /// <para>Returns the last clicked coordinate in a string.</para>
        /// <example>For example: if you click 'A5' it will return 'A5'.</example>
        /// </summary>
        /// <returns>String with the last clicked coordinate.</returns>
        public string GetLastClickedCoordinate()
        {
            return _lastClickedCoordinate;
        }

        /// <summary>
        /// <para>Set <b>before</b> the wellplatesize</para>
        /// <para>Makes all the wells rectangles instead of ellipses</para>
        /// </summary>
        /// <returns>True</returns>
        public bool IsRectangle()
        {
            _setRectangle = true;
            return true;
        }

        /// <summary>
        /// <para>Makes the wellplate clickable. If you click a well, it will change to the set color you used for the clickcolor.</para>
        /// <para>Default is turned off.</para>
        /// </summary>
        /// <returns>True, because I can't return a void.</returns>
        public bool IsEditable()
        {
            _isWellEditable = true;
            return true;
        }
    }
}
