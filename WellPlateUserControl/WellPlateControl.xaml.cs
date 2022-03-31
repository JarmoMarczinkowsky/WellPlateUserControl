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
        const string _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private string _createEllipseName;
        public string LastClickedCoordinate { get; private set; }
        
        private int _widthWellPlate;
        private int _heightWellPlate;
        public int SetMaxWidth = 600;
        public int SetMaxHeight = 601;

        private float _shapeDistance = 1;
        
        private double _strokeThickness = 0.08;

        private bool _setStrokeColor;
        public bool IsRectangle;
        private bool _setMaxWidth;
        private bool _setMaxHeight;
        
        public bool IsEditable;

        private Color _colorConverter = Colors.Black;
        private Color _clickColorConverter = Colors.Red;
        private Color _strokeColor;
        
        private List<string> _coordinates = new List<string>();
        private List<string> _coloredCoordinates = new List<string>();
        private List<string> _notColoredCoordinates = new List<string>();


        public WellPlateControl()
        {
            InitializeComponent();
            rectPlaceHolder.Visibility = Visibility.Hidden;
            Debug.WriteLine("Before");
        }

        /// <summary>
        /// <para>Generates the wellplate</para>
        /// <para>If you have set the colors, set this <b>after</b> them</para>
        /// <para>Can be used without defining other parameters like colors.</para>
        /// <example>If used without other parameters, a few default values will be used:</example><br></br>
        /// <example>maxWidth = 600</example><br></br>
        /// <example>No stroke</example>
        /// <example>strokeThickness = 16%</example><br></br>
        /// <example>gridColor = black</example><br></br>
        /// <example>clickColor = red</example><br></br>
        /// <example>Use: '8,6' will create a grid that is 8 wide and 6 high</example>
        /// </summary>
        /// <param name="inputWidth">The width that the grid is going to be</param>
        /// <param name="inputHeight">The height that the grid is going to be</param>
        /// <returns>True if method succeeds and an out of range error if a values are higher than 26 or smaller than 1</returns>
        public bool SetWellPlateSize(int inputWidth, int inputHeight)
        {
            if (SetMaxHeight < 1)
            {
                throw new ArgumentOutOfRangeException("SetMaxHeight can't be smaller than 1");
            }
            else if (SetMaxHeight != 601)
            {
                _setMaxHeight = true;
            }

            if (SetMaxWidth < 1)
            {
                throw new ArgumentOutOfRangeException("maxWidth can't be smaller than 1");
            }
            

            if (inputWidth > 0 && inputWidth < _alphabet.Length 
                               && inputHeight > 0 
                               && inputHeight < _alphabet.Length)
            {
                _heightWellPlate = inputHeight;
                _widthWellPlate = inputWidth;

                _coordinates.Clear();

                //clears the previous shapes
                gGenerateWellPlate.Children.Clear();

                SetMaxWidth -= 18;
                SetMaxHeight -= 50;

                //generates the shapes
                for (int height = 0; height < _heightWellPlate; height++)
                {
                    for (int width = 0; width < _widthWellPlate; width++)
                    {
                        //_loopCounter++;

                        //creates rectangle
                        Rectangle rectangle = new Rectangle()
                        {
                            VerticalAlignment = VerticalAlignment.Bottom,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            Name = $"{_alphabet[height]}{width + 1}_{1 + height + _heightWellPlate * width}",
                            Fill = new SolidColorBrush(_colorConverter)
                        };

                        _coordinates.Add(rectangle.Name);
                        
                        //checks if the user wants a rectangle. Otherwise it will round the rectangles so it looks like circles.
                        if (IsRectangle)
                        {
                            rectangle.RadiusX = 0;
                            rectangle.RadiusY = 0;
                            _shapeDistance = 1.05F;
                        }
                        else
                        {
                            rectangle.RadiusX = 150;
                            rectangle.RadiusY = 150;
                            _shapeDistance = 1;
                        }

                        //checks if the user has set a maximum height, otherwise it is going to use the maximum width that is set
                        //default if the maximum width is not set is 600
                        if (_setMaxHeight)
                        {
                            rectangle.Width = SetMaxHeight / (_shapeDistance * _heightWellPlate);
                            rectangle.Height = SetMaxHeight / (_shapeDistance * _heightWellPlate); //(_heightWellPlate + 1)
                        }
                        else
                        {
                            rectangle.Width = SetMaxWidth / (_shapeDistance  * _widthWellPlate);
                            rectangle.Height = SetMaxWidth / (_shapeDistance * _widthWellPlate);
                        }
                        
                        //checks if stroke color is set and sets the stroke afterwards.
                        if (_setStrokeColor)
                        {
                            rectangle.Stroke = new SolidColorBrush(_strokeColor);
                            rectangle.StrokeThickness = rectangle.Width * _strokeThickness;
                        }

                        //takes care of the position of the rectangle
                        //directions: left, up, right, down
                        rectangle.Margin = new Thickness(
                            width * rectangle.Width * _shapeDistance, 
                            0,  
                            0, 
                            (_heightWellPlate * rectangle.Width - (height * rectangle.Width) - rectangle.Height) * _shapeDistance); 

                        _coordinates.Add(rectangle.Name);
                        
                        gGenerateWellPlate.Children.Add(rectangle);
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
        /// <para>Converts the entered gridcolor to a readable format for the code.</para>
        /// <example>Use: 'Colors.Red' (without quotes).</example>
        /// </summary>
        /// <param name="gridColor">the color of the grid</param>
        /// <returns>True if the color succeeds to be put inside the variable and an error if it fails to be put inside the variable</returns>
        public bool SetGridColor(Color gridColor)
        {
            _colorConverter = gridColor; 
            return true;            
        }

        /// <summary>
        /// <para>Converts the entered clickcolor to a readable format for the code.</para>
        /// <example>Use: 'Colors.[wishedColor]' without square brackets</example>
        /// </summary>
        /// <param name="clickColor">the color of the wells that you see when you click on them. Is also used for the coordinate system colors</param>
        /// <returns>True if the color succeeds to be put inside the variable and an error if it fails to be put inside the variable</returns>
        public bool SetClickColor(Color clickColor)
        {
            _clickColorConverter = clickColor;
            return true;
        }

        /// <summary>
        /// <para>Give a number or a coordinate and the coordinate will get the 'click' color.</para>
        /// <para>Use '<b>;</b>' to highlight more coordinates. Works with numbers and coordinates</para>
        /// <para>For the next examples a 8x6 wellplate is being used</para>
        /// <para><b>Example:</b> 'A4' colors the 4th circle on the upper row.</para>
        /// <para><b>Example 2:</b> '4' also colors the 4th circle on the first column</para>
        /// <para><b>Example:</b> 'A1;B3;B5;20' wil highlight all these coordinates</para>
        /// </summary>
        /// <param name="coordinate">the coordinate that is about to get colored. Put an ';' in it if you need to color more coordinates</param>
        /// <returns>True if everything succeeds or false if the code fails</returns>
        public bool ColorCoordinate(string coordinate)
        {
            if (String.IsNullOrWhiteSpace(coordinate))
            {
                throw new ArgumentNullException("coordinate does not take 'null' for an argument.");
            }

            string formattedCoordinate;
            List<string> coordinatesForColor = coordinate.Split(";".Trim()).ToList();

            try
            {
                //coordinate to color system
                foreach (string newCoordinate in coordinatesForColor)
                {
                    if (!string.IsNullOrWhiteSpace(newCoordinate))
                    {
                        foreach (object child in gGenerateWellPlate.Children)
                        {
                            Rectangle rectangle = child as Rectangle;

                            formattedCoordinate = newCoordinate.Trim();

                            if (rectangle != null)
                            {
                                //is number
                                if (char.IsDigit(formattedCoordinate[0]))
                                {
                                    if (rectangle.Name.Split("_")[1] == formattedCoordinate)
                                    {
                                        rectangle.Fill = new SolidColorBrush(_clickColorConverter);
                                    }
                                }

                                //is alphabetic
                                else
                                {
                                    _createEllipseName = $"{formattedCoordinate.ToUpper()}";

                                    if (rectangle.Name.Split("_")[0] == _createEllipseName)
                                    {
                                        rectangle.Fill = new SolidColorBrush(_clickColorConverter);
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
                throw new Exception(ex.ToString());
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
            if (String.IsNullOrWhiteSpace(coordinate))
            {
                throw new ArgumentNullException("coordinate does not take 'null' for an argument.");
            }

            foreach (object child in gGenerateWellPlate.Children)
            {
                Rectangle rectangle = child as Rectangle;
                if (rectangle.Name.Split("_")[0] == coordinate.ToUpper().Trim())
                {
                    rectangle.Fill = new SolidColorBrush(chosenColor);
                }
            }
            return true;
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
            foreach (object child in gGenerateWellPlate.Children)
            {
                Rectangle rectangle = child as Rectangle;
                if (rectangle.Name.Split("_")[1] == coordinate.ToString())
                {
                    rectangle.Fill = new SolidColorBrush(chosenColor);
                }
            }
            return true;
        }


        /// <summary>
        /// <para>If you click an rectangle, it will get the color of the 'clickcolor'.</para>
        /// <para>If you click it again, it will change back to the grid color</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClickForColor(object sender, MouseButtonEventArgs e)
        {
            //loops through each of the rectangles
            foreach (object child in gGenerateWellPlate.Children)
            {
                Rectangle rectangle = child as Rectangle;

                if (rectangle != null)
                {
                    if (((Rectangle)child).IsMouseOver)
                    {
                        SolidColorBrush brush = rectangle.Fill as SolidColorBrush;
                        if (brush != null)
                        {
                            LastClickedCoordinate = $"{rectangle.Name.Split("_")[0]}";
                            if (IsEditable)
                            {
                                //if an rectangle is the first color, convert it to the other color if you click it
                                //turns rectangle on
                                if (brush.Color == _colorConverter)
                                {
                                    rectangle.Fill = new SolidColorBrush(_clickColorConverter);
                                }
                                //if an rectangle is the clicked color, convert it to the first color if you click it
                                //turns rectangle off
                                else
                                {
                                    rectangle.Fill = new SolidColorBrush(_colorConverter);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// <para>Set <b>before</b> the wellplatesize</para>
        /// <para>Used to give an outline color to the circles in the wellplate.</para>
        /// <example>Use: Colors.Green</example><br></br>
        /// </summary>
        /// <param name="strokeColor">Color of the stroke</param>
        /// <returns>True if it succeeds or false if it doesn't succeed</returns>
        public bool SetStroke(Color strokeColor)
        {
            _strokeColor = strokeColor;
            _setStrokeColor = true;
            return true;
        }

        /// <summary>
        /// <para>Set <b>before</b> the wellplatesize</para>
        /// <para>Used to give an outline color to the circles in the wellplate.</para>
        /// <para>Also used to set the thickness of the stroke.</para>
        /// <example>Use: Colors.[wishedColor] without brackets</example>
        /// <exmple>Use: 15 for a 15% thick border in the rectangle</exmple>
        /// </summary>
        /// <param name="strokeColor">The color of the stroke.</param>
        /// <param name="strokeThickness">The thickness of the stroke in percentages.</param>
        /// <returns>True if it succeeds and false if it doesn't succeed.</returns>
        public bool SetStroke(Color strokeColor, double strokeThickness)
        {
            _strokeColor = strokeColor;
            if (strokeThickness >= 0 && strokeThickness <= 100)
            {
                _strokeThickness = strokeThickness / 100 / 2;
                _setStrokeColor = true;
                return true;
            }
            throw new ArgumentOutOfRangeException("Thickness of the stroke can't be smaller than 0 or bigger than 100");
        }



        /// <summary>
        /// <para>Input a number and get the coordinate it belongs to</para>
        /// <example>Example: '4' in a 8x6 returns D1</example>
        /// </summary>
        /// <param name="coordinate">Integer that gets used to convert to the correct coordinate</param>
        /// <returns>String with the coordinate the number is linked to. If it doesn't find anything, it will return "nothing found"</returns>
        public string NumberToCoordinate(int coordinate)
        {
            foreach (string loopedCoordinate in _coordinates)
            {
                if (loopedCoordinate.Split("_")[1].Trim() == coordinate.ToString())
                {
                    return $"{loopedCoordinate.Split("_")[0]}";
                }
            }
            return "";
        }

        /// <summary>
        /// <para>Input a coordinate and get the number it belongs to</para>
        /// <example>Example: 'D1' in a 8x6 grid returns 4</example>
        /// </summary>
        /// <param name="coordinate">String that gets used to convert to the correct number</param>
        /// <returns>Integer with the number the coordinate is linked to. <br>If it doesn't find anything, it will return -1</br>.<br>If it notices a null or whitespace it will return -2.</br></returns>
        public int CoordinateToNumber(string coordinate)
        {
            if (String.IsNullOrWhiteSpace(coordinate))
            {
                throw new ArgumentNullException("coordinate does not take 'null' for an argument.");
            }

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
                Rectangle rectangle = child as Rectangle;

                if (!_coloredCoordinates.Contains(rectangle.Name.Split("_")[0].Trim()))
                {
                    _notColoredCoordinates.Add(rectangle.Name.Split("_")[0]);
                }
            }
            return _notColoredCoordinates;
        }

        /// <summary>
        /// Will clear the list of every colored coordinate and refill them with the colored coordinates.
        /// </summary>
        private void UpdateColoredList()
        {
            _coloredCoordinates.Clear();

            foreach (object child in gGenerateWellPlate.Children)
            {
                Rectangle rectangle = child as Rectangle;
                SolidColorBrush brush = rectangle.Fill as SolidColorBrush;

                if (brush != null)
                {
                    if (brush.Color != _colorConverter)
                    {
                        _coloredCoordinates.Add(rectangle.Name.Split("_")[0]);
                    }
                }
            }
        }

        

        
    }
}
