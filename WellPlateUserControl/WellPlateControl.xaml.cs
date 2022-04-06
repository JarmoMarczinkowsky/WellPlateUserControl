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
        public string LastClickedCoordinate { get; private set; }
        
        private int _setMaxHeight = 601;
        public int SetMaxHeight
        {
            get { return _setMaxHeight; }
            set 
            { 
                if (value < 55)
                {
                    throw new ArgumentOutOfRangeException("SetMaxHeight can't be smaller than 55");
                }
                else if (value != 601)
                {
                    _setMaxHeight = value;
                    _setTheMaxHeight = true;
                }
            }
        }

        private int _setMaxWidth = 600;
        public int SetMaxWidth
        {
            get { return _setMaxWidth; }
            set 
            {
                if (value < 20)
                {
                    throw new ArgumentOutOfRangeException("maxWidth can't be smaller than 20");
                }
                else
                {
                    _setMaxWidth = value;
                }
            }
        }

        public bool IsRectangle;
        public bool IsEditable;
        public bool TurnCoordinatesOff;

        private const string _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private string _createEllipseName;

        private int _widthWellPlate = 12;
        private int _heightWellPlate = 8;

        private float _shapeDistance = 1;
        private float _lastRectangleWidth;
        private float _letterDistance = 50;
        private float _calcMaxHeight;
        private float _calcMaxWidth;
        private float _recalcMaxWidth;

        private double _strokeThickness = 0.08;
        private double _wellSize;
        public double StrokeThickness
        {
            get { return _strokeThickness; }
            set
            {
                if (value >= 0 && value <= 100)
                {
                    _strokeThickness = value / 100 / 2;
                    if (!_setStrokeColor)
                    {
                        _strokeColor = Colors.Black;
                        _setStrokeColor = true;
                    }
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Thickness of the stroke can't be smaller than 0 or bigger than 100");
                }
            }
        }
        private bool _setStrokeColor;
        private bool _setTheMaxHeight;

        private Color _setGridColor = Color.FromRgb(209, 232, 247);
        public Color SetGridColor
        {
            get { return _setGridColor; }
            set
            {
                _setGridColor = value;
            }
        }

        private Color _setClickColor = Color.FromRgb(0, 157, 247);
        public Color SetClickColor
        {
            get { return _setClickColor; }
            set
            {
                _setClickColor = value;
            }
        }

        private Color _strokeColor;
        public Color SetStrokeColor
        {
            get { return _strokeColor; }
            set
            {
                _setStrokeColor = true;
                _strokeColor = value;
            }
        }
        private List<string> _coordinates = new List<string>();
        
        private List<string> _coloredCoordinates = new List<string>();
        public List<string> GetColoredCoordinates
        {
            get
            {
                updateColoredList();
                return _coloredCoordinates;
            }
            private set { _coloredCoordinates = value; }
        }

        private List<string> _notColoredCoordinates = new List<string>();
        public List<string> GetNotColoredCoordinates
        {
            get
            {
                giveNotColoredList();
                return _notColoredCoordinates;
            }
            private set { _notColoredCoordinates = value; }
        }


        public WellPlateControl()
        {
            InitializeComponent();
        }

        public void HidePlaceHolder()
        {
            rectPlaceHolder.Visibility = Visibility.Hidden;
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
            if (inputWidth > 0 && inputWidth <= _alphabet.Length
                               && inputHeight > 0
                               && inputHeight <= _alphabet.Length)
            {
                _heightWellPlate = inputHeight;
                _widthWellPlate = inputWidth;

                return true;
            }
            else
            {
                throw new ArgumentOutOfRangeException($"Number can't be bigger than 26 or smaller than 1: length = {inputWidth}, width = {inputHeight}");
            }
        }

        /// <summary>
        /// Prepares the values needed for generating the wellplate
        /// </summary>
        private void prepareValues()
        {
            _coordinates.Clear();
            //clears the previous shapes and labels
            gGenerateWellPlate.Children.Clear();
            gCoordinates.Children.Clear();

            _calcMaxWidth = _setMaxWidth - 16;

            if (_setTheMaxHeight)
            {
                _calcMaxHeight = _setMaxHeight - 50;
            }

            //checks if the user has chosen for rectangles and changes the spacing between the wells after that.
            if (IsRectangle)
            {
                _shapeDistance = 1.05F;
            }
            else
            {
                _shapeDistance = 1.3F;
            }

            //calculates the size of a well and removes a third of the calculated well from the maximum width
            _wellSize = _calcMaxWidth / (_shapeDistance * _widthWellPlate);
            _recalcMaxWidth = (float)(_calcMaxWidth - (_wellSize / 1.5));
        }

        /// <summary>
        /// <para>Is responsible for creating the wells</para>
        /// </summary>
        /// <param name="width">Current width of the for loop</param>
        /// <param name="height">Current height of the for loop</param>
        private void createWells(int width, int height)
        {
            Rectangle rectangle = new Rectangle()
            {
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Left,
                Name = $"{_alphabet[height]}{width + 1}_{1 + height + _heightWellPlate * width}",
                Fill = new SolidColorBrush(_setGridColor)
            };

            _coordinates.Add(rectangle.Name);

            //checks if the user wants a rectangle. Otherwise it will round the rectangles so it looks like circles.
            if (IsRectangle)
            {
                rectangle.RadiusX = 0;
                rectangle.RadiusY = 0;
            }
            else
            {
                rectangle.RadiusX = 1500;
                rectangle.RadiusY = 1500;
            }

            //checks if the user has set a maximum height, otherwise it is going to use the maximum width that is set
            //default if the maximum width is not set is 600
            if (_setTheMaxHeight)
            {
                rectangle.Width = _calcMaxHeight / (_shapeDistance * _heightWellPlate);
                rectangle.Height = _calcMaxHeight / (_shapeDistance * _heightWellPlate);
            }
            else
            {
                rectangle.Width = _recalcMaxWidth / (_shapeDistance * _widthWellPlate);
                rectangle.Height = _recalcMaxWidth / (_shapeDistance * _widthWellPlate);
            }

            //checks if stroke color is set and sets the stroke afterwards.
            if (_setStrokeColor)
            {
                rectangle.Stroke = new SolidColorBrush(_strokeColor);
                rectangle.StrokeThickness = rectangle.Width * _strokeThickness;
            }

            _lastRectangleWidth = (float)rectangle.Width;

            _letterDistance = (float)(rectangle.Width / 1.5);

            //takes care of the position of the rectangle
            rectangle.Margin = new Thickness(
                _letterDistance + (_shapeDistance * rectangle.Width) - rectangle.Width + width * rectangle.Width * _shapeDistance, 0, 0,
                (_shapeDistance * rectangle.Height) - rectangle.Height + (_heightWellPlate * rectangle.Width - (height * rectangle.Width) - rectangle.Height) * _shapeDistance);

            _coordinates.Add(rectangle.Name);

            gGenerateWellPlate.Children.Add(rectangle);
        }

        /// <summary>
        /// <para>Is repsonsible for creating the alphabetic and numeric labels with coordinates.</para>
        /// </summary>
        /// <param name="width">Current width of the for loop</param>
        /// <param name="height">Current height of the for loop</param>
        private void generateLabels(int width, int height)
        {
            //takes care of the alphabetic labels
            if (width == 0 && !TurnCoordinatesOff)
            {
                Label lblAlphabetic = new Label();
                lblAlphabetic.Content = $"{_alphabet[height]}";
                lblAlphabetic.Foreground = new SolidColorBrush(Colors.Black);
                lblAlphabetic.HorizontalAlignment = HorizontalAlignment.Left;
                lblAlphabetic.VerticalAlignment = VerticalAlignment.Bottom;
                lblAlphabetic.Width = _lastRectangleWidth;
                lblAlphabetic.Height = _lastRectangleWidth;
                lblAlphabetic.VerticalContentAlignment = VerticalAlignment.Center;
                lblAlphabetic.FontSize = _lastRectangleWidth / 2.5;
                lblAlphabetic.Margin = new Thickness(0, 0, 0,
                    (_shapeDistance * _lastRectangleWidth) - _lastRectangleWidth + (_heightWellPlate * _lastRectangleWidth - (height * _lastRectangleWidth) - _lastRectangleWidth) * _shapeDistance);
                gCoordinates.Children.Add(lblAlphabetic);

            }

            //takes care of the numeric labels
            if (height == 0 && !TurnCoordinatesOff)
            {
                Label lblNumeric = new Label();
                lblNumeric.Content = $"{width + 1}";
                lblNumeric.Foreground = new SolidColorBrush(Colors.Black);
                //lblNumeric.Background = new SolidColorBrush(Colors.LightGray);
                lblNumeric.HorizontalAlignment = HorizontalAlignment.Left;
                lblNumeric.VerticalAlignment = VerticalAlignment.Bottom;
                lblNumeric.Width = _lastRectangleWidth;

                lblNumeric.HorizontalContentAlignment = HorizontalAlignment.Center;
                lblNumeric.FontSize = _lastRectangleWidth / 2.5;
                lblNumeric.Margin = new Thickness(
                    _letterDistance + (_shapeDistance * _lastRectangleWidth) - _lastRectangleWidth + width * _lastRectangleWidth * _shapeDistance,
                    0,
                    0,
                    (_shapeDistance * _lastRectangleWidth) - _lastRectangleWidth + _heightWellPlate * _lastRectangleWidth /*- (height * rectangle.Width) - rectangle.Height)*/ * _shapeDistance);
                gCoordinates.Children.Add(lblNumeric);
            }
        }
        /// <summary>
        /// Is reponsible for the rounded border around the wellplate
        /// </summary>
        private void generateBorder()
        {
            //rectangle that takes care of the outline of the wellplate
            rectOutline.HorizontalAlignment = HorizontalAlignment.Left;
            rectOutline.VerticalAlignment = VerticalAlignment.Bottom;
            rectOutline.Margin = new Thickness(_letterDistance, 0, 0, 0);
            rectOutline.Fill = new SolidColorBrush(Colors.Transparent);
            rectOutline.Width = _lastRectangleWidth * _widthWellPlate * _shapeDistance + (_shapeDistance * _lastRectangleWidth) - _lastRectangleWidth;
            rectOutline.Height = _lastRectangleWidth * _heightWellPlate * _shapeDistance + (_shapeDistance * _lastRectangleWidth) - _lastRectangleWidth;
            rectOutline.Stroke = new SolidColorBrush(Colors.LightGray);
            rectOutline.StrokeThickness = 3;
            rectOutline.RadiusX = 15;
            rectOutline.RadiusY = 15;
        }

        /// <summary>
        /// Generates the wellplate based on the values the user entered. If it doesn't find any values that the user entered, it will use default values:<br></br>
        /// The default values are:<br></br>
        /// SetGridColor = Color.FromRgb(209, 232, 247);<br></br>
        /// SetClickColor = Color.FromRgb(97, 172, 223);<br></br>
        /// IsEditable = false;<br></br>
        /// IsRectangle = false; <br></br>
        /// </summary><br></br>
        /// <returns>True</returns>
        public bool DrawWellPlate()
        {
            HidePlaceHolder();

            prepareValues();

            //generates the shapes
            for (int height = 0; height < _heightWellPlate; height++)
            {
                for (int width = 0; width < _widthWellPlate; width++)
                {
                    createWells(width, height);

                    generateLabels(width, height);
                }
            }
            generateBorder();
            return true;
        }

        /// <summary>
        /// <para>Enter a coordinate and it will get the 'click' color.</para>
        /// <para>For the next example a 8x6 wellplate is being used</para>
        /// <para><b>Example:</b> "A4" colors the 4th circle on the first row</para>
        /// </summary>
        /// <param name="coordinate">the coordinate that is about to get colored.</param>
        /// <returns>True if everything succeeds or false if the code fails</returns>
        public bool ColorCoordinate(string coordinate)
        {
            string formattedCoordinate;
            
            if (string.IsNullOrWhiteSpace(coordinate))
            {
                throw new ArgumentNullException("coordinate does not take 'null' for an argument.");
            }

            foreach (object child in gGenerateWellPlate.Children)
            {
                Rectangle rectangle = child as Rectangle;

                formattedCoordinate = coordinate.Trim();

                if (rectangle != null)
                {
                    _createEllipseName = $"{formattedCoordinate.ToUpper()}";

                    if (rectangle.Name.Split("_")[0] == _createEllipseName)
                    {
                        rectangle.Fill = new SolidColorBrush(_setClickColor);
                    }
                    
                }
            }
                
            return true;
        }

        /// <summary>
        /// <para>Give a number and the coordinate will get the 'click' color.</para>
        /// <para>For the next example a 8x6 wellplate is being used</para>
        /// <para><b>Example:</b> '4' colors the 4th circle on the first column</para>
        /// </summary>
        /// <param name="coordinate">the coordinate that is about to get colored.</param>
        /// <returns>True if everything succeeds or false if the code fails</returns>
        public bool ColorCoordinate(int coordinate)
        {
            foreach (object child in gGenerateWellPlate.Children)
            {
                Rectangle rectangle = child as Rectangle;

                if (rectangle != null)
                {
                    //is number
                    if (rectangle.Name.Split("_")[1] == coordinate.ToString())
                    {
                        rectangle.Fill = new SolidColorBrush(_setClickColor);
                        return true;
                    }
                }
            }
            return false;
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
            if (string.IsNullOrWhiteSpace(coordinate))
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
                                if (brush.Color == _setGridColor)
                                {
                                    rectangle.Fill = new SolidColorBrush(_setClickColor);
                                }
                                //if an rectangle is the clicked color, convert it to the first color if you click it
                                //turns rectangle off
                                else
                                {
                                    rectangle.Fill = new SolidColorBrush(_setGridColor);
                                }
                            }
                        }
                    }
                }
            }
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
            if (string.IsNullOrWhiteSpace(coordinate))
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

        ///// <summary>
        ///// <para>Set this function <b>after</b> WellPlateSize.</para>
        ///// <para>Gives a list of every colored well.</para>
        ///// </summary>
        ///// <returns>A list of every colored well</returns>
        //public List<string> GiveColoredList()
        //{
        //    UpdateColoredList();
        //    return _coloredCoordinates;
        //}

        /// <summary>
        /// <para>Set this function <b>after</b> WellPlateSize.</para>
        /// <para>Gives a list of every not-colored well.</para>
        /// </summary>
        /// <returns>A list of every not-colored well</returns>
        public void giveNotColoredList()
        {
            updateColoredList();
            _notColoredCoordinates.Clear();

            foreach (object child in gGenerateWellPlate.Children)
            {
                Rectangle rectangle = child as Rectangle;

                if (!_coloredCoordinates.Contains(rectangle.Name.Split("_")[0].Trim()))
                {
                    _notColoredCoordinates.Add(rectangle.Name.Split("_")[0]);
                }
            }
        }

        /// <summary>
        /// Will clear the list of every colored coordinate and refill them with the colored coordinates.
        /// </summary>
        private void updateColoredList()
        {
            _coloredCoordinates.Clear();

            foreach (object child in gGenerateWellPlate.Children)
            {
                Rectangle rectangle = child as Rectangle;
                SolidColorBrush brush = rectangle.Fill as SolidColorBrush;

                if (brush != null)
                {
                    if (brush.Color != _setGridColor)
                    {
                        _coloredCoordinates.Add(rectangle.Name.Split("_")[0]);
                    }
                }
            }
        }

        

        

        
    }
}
