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

        /// <summary>
        /// <para>Is used to set the thickness of the stroke in percentages.</para>
        /// <para>If the color is not defined, it will use black.</para>
        /// </summary>
        public double SetStrokeThickness
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

        /// <summary>
        /// Is used to set a fixed size for the wells
        /// </summary>
        public double SetWellSize
        {
            get { return _setWellSize; }
            set
            {
                if (value >= 1)
                {
                    _hasSetWellSize = true;
                    _setWellSize = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Value can't be smaller than 1");
                }
            }
        }

        /// <summary>
        /// <para>Is used to set the color of the wellplate.</para>
        /// </summary>
        public Color SetGridColor
        {
            get { return _setGridColor; }
            set
            {
                _setGridColor = value;
            }
        }

        /// <summary>
        /// Is used for the color of a well when it is clicked
        /// </summary>
        public Color SetClickColor
        {
            get { return _setClickColor; }
            set
            {
                _setClickColor = value;
            }
        }

        /// <summary>
        /// Is used to set a color for the stroke
        /// </summary>
        public Color SetStrokeColor
        {
            get { return _strokeColor; }
            set
            {
                _setStrokeColor = true;
                _strokeColor = value;
            }
        }

        public Color SetLabelColor
        {
            get { return _setLabelColor; }
            set
            {
                _setLabelColor = value;
            }
        }

        /// <summary>
        /// Is used to return a list with every colored well
        /// </summary>
        public List<string> GetColoredCoordinates
        {
            get
            {
                updateColoredList();
                return _coloredCoordinates;
            }
            private set { _coloredCoordinates = value; }
        }

        /// <summary>
        /// Is used to return a list with every well that is not colored
        /// </summary>
        public List<string> GetNotColoredCoordinates
        {
            get
            {
                giveNotColoredList();
                return _notColoredCoordinates;
            }
            private set { _notColoredCoordinates = value; }
        }

        public bool IsRectangle;
        public bool IsEditable;
        public bool TurnCoordinatesOff;

        private double _setWellSize = -1;

        private const string _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private string _createEllipseName;

        private int _widthWellPlate = 12;
        private int _heightWellPlate = 8;
        private int _wellRoundedCorner = 0;
        private const int _widthLeftOver = 16;
        private const int _heightLeftOver = 50;

        private float _shapeDistance = 1;
        private float _lastRectangleWidth;
        private float _letterDistance = 50;
        private float _calcMaxHeight;
        private float _calcMaxWidth;
        private float _recalcMaxWidth;
        private float _recalcMaxHeight;
        private const float _fontSizeModifier = 0.45F;

        private double _strokeThickness = 0.08;
        private double _wellSize;
        private double _setMaxHeight;
        private double _setMaxWidth;

        private bool _setStrokeColor;
        private bool _setTheMaxHeight;
        private bool _hasSetWellSize;

        private Color _setGridColor = Color.FromRgb(209, 232, 247);
        private Color _setClickColor = Color.FromRgb(0, 157, 247);
        private Color _setLabelColor = Colors.Black;
        private Color _strokeColor;

        private List<string> _coordinates = new List<string>();
        private List<string> _coloredCoordinates = new List<string>();
        private List<string> _notColoredCoordinates = new List<string>();

        public WellPlateControl()
        {
            InitializeComponent();
        }

        public void HidePlaceHolder()
        {
            this.Background = new SolidColorBrush(Colors.Transparent);
            imgPlaceHolder.Visibility = Visibility.Hidden;
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
            //double biggerThanSpaceAvailablePercentage = 100;
            double highestTextblock;

            _coordinates.Clear();
            //clears the previous shapes and labels
            gGenerateWellPlate.Children.Clear();
            gCoordinates.Children.Clear();

            //checks if a value is entered. If yes -> set the value to the corresponding variable. If both are no -> maxWidth becomes 600 pixels
            checkControlSize();

            //checks if the user has chosen for rectangles and changes the spacing between the wells after that.
            if (IsRectangle)
            {
                _shapeDistance = 1.05F;
                _wellRoundedCorner = 0;
            }
            else
            {
                _shapeDistance = 1.3F;
                _wellRoundedCorner = 1600;
            }

            //calculates the size of a well and removes a third of the calculated well from the maximum width
            _wellSize = _calcMaxWidth / (_shapeDistance * _widthWellPlate);
            if (_setTheMaxHeight)
            {
                _recalcMaxHeight = (float)((_calcMaxHeight - (_wellSize / 1.5)) * 0.9);
                _lastRectangleWidth = _recalcMaxHeight / (_shapeDistance * _heightWellPlate);
            }
            else
            {
                _recalcMaxWidth = (float)((_calcMaxWidth - (_wellSize / 1.5)) * 0.95);
                _lastRectangleWidth = _recalcMaxWidth / (_shapeDistance * _widthWellPlate);
            }

            if (_hasSetWellSize)
            {
                _lastRectangleWidth = (float)_setWellSize;
            }

            highestTextblock = (_shapeDistance * _lastRectangleWidth) - _lastRectangleWidth + _heightWellPlate * _lastRectangleWidth * _shapeDistance + (_lastRectangleWidth * _fontSizeModifier);

            //in case given height is smaller than needed height
            heightCheck(highestTextblock);
            
        }

        private void checkControlSize()
        {
            if (!double.IsNaN(this.Width))
            {
                if (this.Width < (_widthLeftOver + 4))
                {
                    throw new ArgumentOutOfRangeException("maxWidth can't be smaller than 20");
                }
                else
                {
                    _setMaxWidth = this.Width;
                    _calcMaxWidth = (float)(_setMaxWidth - _widthLeftOver);
                }
            }
            else if (!double.IsNaN(this.Height))
            {
                if (this.Height < (_heightLeftOver + 5))
                {
                    throw new ArgumentOutOfRangeException("SetMaxHeight can't be smaller than 55");
                }
                else
                {
                    _setMaxHeight = this.Height;
                    _calcMaxHeight = (float)(_setMaxHeight - _heightLeftOver);
                    _setTheMaxHeight = true;
                }
            }
            else
            {
                _setMaxWidth = 600; //default width if it doesn't find a width or height is 600 pixels
                _calcMaxWidth = (float)(_setMaxWidth - _widthLeftOver);
            }
        }

        private void heightCheck(double highestTextblock)
        {
            double biggerThanSpaceAvailablePercentage;
            if (highestTextblock > this.Height)
            {
                biggerThanSpaceAvailablePercentage = (highestTextblock - this.Height) / this.Height * 100;

                if (_setTheMaxHeight)
                {
                    _recalcMaxHeight = (float)(_recalcMaxHeight / (biggerThanSpaceAvailablePercentage + 100) * 100);
                }
                else
                {
                    _recalcMaxWidth = (float)(_recalcMaxWidth / (biggerThanSpaceAvailablePercentage + 100) * 100);
                }
            }
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
            rectangle.RadiusX = _wellRoundedCorner;
            rectangle.RadiusY = _wellRoundedCorner;

            //checks if the user has set a maximum height, otherwise it is going to use the maximum width that is set
            //default if the maximum width is not set is 600
            if (_hasSetWellSize)
            {
                rectangle.Width = _setWellSize;
                rectangle.Height = _setWellSize;
            }
            else
            {
                if (_setTheMaxHeight)
                {
                    rectangle.Width = _recalcMaxHeight / (_shapeDistance * _heightWellPlate);
                    rectangle.Height = _recalcMaxHeight / (_shapeDistance * _heightWellPlate);
                }
                else
                {
                    rectangle.Width = _recalcMaxWidth / (_shapeDistance * _widthWellPlate);
                    rectangle.Height = _recalcMaxWidth / (_shapeDistance * _widthWellPlate);
                }
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
            float textBlockSizeModifier = .45F;

            //takes care of the alphabetic labels
            if (width == 0 && !TurnCoordinatesOff)
            {
                TextBlock lblAlphabetic = new TextBlock();
                lblAlphabetic.Text = $"{_alphabet[height]}";
                lblAlphabetic.Foreground = new SolidColorBrush(_setLabelColor);
                lblAlphabetic.HorizontalAlignment = HorizontalAlignment.Left;
                lblAlphabetic.VerticalAlignment = VerticalAlignment.Bottom;
                lblAlphabetic.Width = _lastRectangleWidth;
                lblAlphabetic.Height = _lastRectangleWidth;
                lblAlphabetic.FontSize = _lastRectangleWidth * textBlockSizeModifier;
                lblAlphabetic.TextAlignment = TextAlignment.Center;
                lblAlphabetic.Margin = new Thickness(0, 0, 0,
                    (_shapeDistance * _lastRectangleWidth) - _lastRectangleWidth + (_heightWellPlate * _lastRectangleWidth - (height * _lastRectangleWidth) - _lastRectangleWidth) * _shapeDistance - (_lastRectangleWidth / 4));
                gCoordinates.Children.Add(lblAlphabetic);

            }

            //takes care of the numeric labels
            if (height == 0 && !TurnCoordinatesOff)
            {
                TextBlock lblNumeric = new TextBlock();
                lblNumeric.Text = $"{width + 1}";
                lblNumeric.Foreground = new SolidColorBrush(_setLabelColor);
                lblNumeric.HorizontalAlignment = HorizontalAlignment.Left;
                lblNumeric.VerticalAlignment = VerticalAlignment.Bottom;
                lblNumeric.TextAlignment = TextAlignment.Center;
                lblNumeric.Width = _lastRectangleWidth;
                lblNumeric.FontSize = _lastRectangleWidth * textBlockSizeModifier;

                lblNumeric.Margin = new Thickness(
                    _letterDistance + (_shapeDistance * _lastRectangleWidth) - _lastRectangleWidth + width * _lastRectangleWidth * _shapeDistance,
                    0,
                    0,
                    (_shapeDistance * _lastRectangleWidth) - _lastRectangleWidth + _heightWellPlate * _lastRectangleWidth * _shapeDistance);
                gCoordinates.Children.Add(lblNumeric);

                if (width == 0)
                {
                    Debug.WriteLine($"Recommended height: {(_shapeDistance * _lastRectangleWidth) - _lastRectangleWidth + _heightWellPlate * _lastRectangleWidth * _shapeDistance + lblNumeric.FontSize}");
                    Debug.WriteLine($"Well size: {_lastRectangleWidth}");
                }
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
            rectOutline.Stroke = new SolidColorBrush(Colors.LightGray);
            rectOutline.StrokeThickness = _lastRectangleWidth * 0.1; //the 0.1 makes sure the stroke is 10% of the width of a well
            rectOutline.Width = _lastRectangleWidth * _widthWellPlate * _shapeDistance + (_shapeDistance * _lastRectangleWidth) - _lastRectangleWidth;
            rectOutline.Height = _lastRectangleWidth * _heightWellPlate * _shapeDistance + (_shapeDistance * _lastRectangleWidth) - _lastRectangleWidth;

            if (_lastRectangleWidth > 15 && !IsRectangle)
            {
                rectOutline.RadiusX = 15;
                rectOutline.RadiusY = 15;
            }
        }

        /// <summary>
        /// Generates the wellplate based on the values the user entered. If it doesn't find any values that the user entered, it will use default values:<br></br>
        /// The default values are:<br></br>
        /// SetGridColor = Color.FromRgb(209, 232, 247); (lightblue)<br></br>
        /// SetClickColor = Color.FromRgb(0, 157, 247); (blue)<br></br>
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
                        return true;
                    }
                }
            }
            return false;
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
                    return true;
                }
            }
            return false;
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
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// <para>If you click an rectangle, it will get the color of the 'clickcolor'.</para>
        /// <para>If you click it again, it will change back to the grid color</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clickForColor(object sender, MouseButtonEventArgs e)
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

        /// <summary>
        /// Clears entire wellplate of colored wells.
        /// </summary>
        public void Clear()
        {
            foreach (object child in gGenerateWellPlate.Children)
            {
                Rectangle rectangle = child as Rectangle;
                SolidColorBrush brush = rectangle.Fill as SolidColorBrush;

                if (brush != null && rectangle != null)
                {
                    if (brush.Color != _setGridColor)
                    {
                        rectangle.Fill = new SolidColorBrush(_setGridColor);
                    }
                }

            }
        }

        /// <summary>
        /// Will clear the well on the entered coordinate
        /// </summary>
        /// <param name="coordinate">The coordinate of the well that needs to be cleared</param>
        public void Clear(string coordinate)
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
                        rectangle.Fill = new SolidColorBrush(_setGridColor);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Will clear the well on the entered number
        /// </summary>
        /// <param name="coordinate">The number of the well that needs to be cleared</param>
        public void Clear(int coordinate)
        {
            foreach (object child in gGenerateWellPlate.Children)
            {
                Rectangle rectangle = child as Rectangle;

                if (rectangle != null)
                {
                    //is number
                    if (rectangle.Name.Split("_")[1] == coordinate.ToString())
                    {
                        rectangle.Fill = new SolidColorBrush(_setGridColor);
                        break;
                    }
                }
            }
        }
    }
}
