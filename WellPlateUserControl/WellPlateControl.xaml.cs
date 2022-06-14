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
    //The delegate for the subscribe click event
    public delegate void Notify();


    /// <summary>
    /// Interaction logic for WellPlateControl.xaml
    /// </summary>
    public partial class WellPlateControl : UserControl
    {
        GetCoordinateInfo getCoordinateInfo = new();
        SizeHandler sizeHandler = new();
        CalculateWellSize calculateWellSize = new();
        public event Notify WellClicked;

        public string LastClickedCoordinate { get; private set; }

        /// <summary>
        /// <para>Is used to set the thickness of the stroke in percentages.</para>
        /// <para>If the color is not defined, it will use black</para>
        /// </summary>
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

        /// <summary>
        /// Is used to set a fixed size for the wells
        /// </summary>
        public double WellSize
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
        /// <para>Is used to set the color of the wells in the wellplate</para>
        /// </summary>
        public Color GridColor
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
        public Color ClickColor
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
        public Color StrokeColor
        {
            get { return _strokeColor; }
            set
            {
                _setStrokeColor = true;
                _strokeColor = value;
            }
        }

        /// <summary>
        /// Is used to set a color for the coordinates surrounding the wellplate
        /// </summary>
        public Color LabelColor
        {
            get { return _setLabelColor; }
            set
            {
                _setLabelColor = value;
            }
        }

        /// <summary>
        /// Is used to set a color for the border surrounding the wellplate
        /// </summary>
        public Color BorderColor
        {
            get { return _setBorderColor; }
            set
            {
                _setBorderColor = value;
            }
        }
        /// <summary>
        /// Is used to return a list with every colored well
        /// </summary>
        public List<string> ColoredCoordinates
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
        public List<string> NotColoredCoordinates
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

        private float _letterDistance;
        private const float _fontSizeModifier = 0.45F;

        private double _strokeThickness = 0.08;
        
        private bool _setStrokeColor;
        private bool _hasSetWellSize;

        private Color _setGridColor = Color.FromRgb(209, 232, 247);
        private Color _setClickColor = Colors.CadetBlue;
        private Color _setLabelColor = Colors.Black;
        private Color _setBorderColor = Colors.LightGray;
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
        /// <para>Is responsible for the size of the wellplate</para>
        /// </summary>
        /// <param name="inputWidth">The width that the grid is going to be</param>
        /// <param name="inputHeight">The height that the grid is going to be</param>
        /// <returns>True if method succeeds and an out of range error if a values are higher than 26 or smaller than 1</returns>
        public bool SetWellPlateSize(int inputWidth, int inputHeight)
        {
            bool setPlateSize = sizeHandler.SetWellPlateSize(inputWidth, inputHeight, _alphabet);
            return setPlateSize;
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
            calculateWellSize.CheckControlSize(this.Width, this.Height);

            //checks if the user has chosen for rectangles and changes the spacing between the wells after that.
            calculateWellSize.RectangleDistance(IsRectangle);

            //calculates the size of a well and removes a third of the calculated well from the maximum width
            calculateWellSize.CalculateMaxSize(sizeHandler._widthWellPlate, sizeHandler._heightWellPlate);
            
            //calculates the height of the numeric labels
            highestTextblock = (calculateWellSize._shapeDistance * calculateWellSize.LastRectangleWidth) - calculateWellSize.LastRectangleWidth + sizeHandler._heightWellPlate * calculateWellSize.LastRectangleWidth * calculateWellSize._shapeDistance + (calculateWellSize.LastRectangleWidth * _fontSizeModifier);

            //in case given height is smaller than needed height
            calculateWellSize.HeightCheck(highestTextblock, this.Width, this.Height);

            calculateRectangleSize();

        }

        /// <summary>
        /// Checks if the width of the usercontrol is set. If it doesn't find a width, it will search for the height, otherwise it will just use a width of 600
        /// </summary>
        //private void checkControlSize()
        //{
        //    //if (!double.IsNaN(this.Width))
        //    //{
        //    //    if (this.Width < (_widthLeftOver + 4))
        //    //    {
        //    //        throw new ArgumentOutOfRangeException("maxWidth can't be smaller than 20");
        //    //    }
        //    //    else
        //    //    {
        //    //        _setMaxWidth = this.Width;
        //    //        _calcMaxWidth = (float)(_setMaxWidth - _widthLeftOver);
        //    //    }
        //    //}
        //    //else if (!double.IsNaN(this.Height))
        //    //{
        //    //    if (this.Height < (_heightLeftOver + 5))
        //    //    {
        //    //        throw new ArgumentOutOfRangeException("SetMaxHeight can't be smaller than 55");
        //    //    }
        //    //    else
        //    //    {
        //    //        _setMaxHeight = this.Height;
        //    //        _calcMaxHeight = (float)(_setMaxHeight - _heightLeftOver);
        //    //        _setTheMaxHeight = true;
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    _setMaxWidth = 600; //default width if it doesn't find a width or height is 600 pixels
        //    //    _calcMaxWidth = (float)(_setMaxWidth - _widthLeftOver);
        //    //}
        //    calculateWellSize.CheckControlSize(this.Width, this.Height);
        //}

        /// <summary>
        /// Sets the distance between 2 wells
        /// </summary>
        //private void rectangleDistance()
        //{
        //    //if (IsRectangle)
        //    //{
        //    //    calculateWellSize._shapeDistance = 1.05F; //1.05 is 5% the size of a well between two wells
        //    //    _wellRoundedCorner = 0; //0 = no rounded corners
        //    //}
        //    //else
        //    //{
        //    //    calculateWellSize._shapeDistance = 1.3F; //1.3 is 30% the size of a well between two wells
        //    //    _wellRoundedCorner = 1600; //extremely high value so the wells stay rounded even if they are big
        //    //}
        //    calculateWellSize.RectangleDistance(IsRectangle);
        //}

        //private void calculateMaxSize()
        //{
        //    _wellSize = calculateWellSize.CalcMaxWidth / (calculateWellSize._shapeDistance * sizeHandler._widthWellPlate);
        //    if (calculateWellSize.SetTheMaxHeight)
        //    {
        //        _recalcMaxHeight = (float)((calculateWellSize.CalcMaxHeight - (_wellSize / 1.5)) * 0.9); //0.9 is 90% the size of the wellplate
        //        calculateWellSize.LastRectangleWidth = _recalcMaxHeight / (calculateWellSize._shapeDistance * sizeHandler._heightWellPlate);
        //    }
        //    else
        //    {
        //        _recalcMaxWidth = (float)((calculateWellSize.CalcMaxWidth - (_wellSize / 1.5)) * 0.95); //0.95 is 95% the size of the wellplate
        //        calculateWellSize.LastRectangleWidth = _recalcMaxWidth / (calculateWellSize._shapeDistance * sizeHandler._widthWellPlate);
        //    }
        //}

        ///// <summary>
        ///// Checks if the size of the highest textblock is going to be bigger than the usercontrol height. 
        ///// If it is it will resize the entered height to a more suitable height
        ///// </summary>
        ///// <param name="highestTextblock"></param>
        //private void heightCheck(double highestTextblock)
        //{
        //    double biggerThanSpaceAvailablePercentage;
        //    if (highestTextblock > this.Height)
        //    {
        //        biggerThanSpaceAvailablePercentage = (highestTextblock - this.Height) / this.Height * 100;

        //        if (calculateWellSize.SetTheMaxHeight)
        //        {
        //            calculateWellSize.RecalcMaxHeight = (float)(calculateWellSize.RecalcMaxHeight / (biggerThanSpaceAvailablePercentage + 100) * 100);
        //        }
        //        else
        //        {
        //            calculateWellSize.RecalcMaxWidth = (float)(calculateWellSize.RecalcMaxWidth / (biggerThanSpaceAvailablePercentage + 100) * 100);
        //        }
        //    }
        //}

        /// <summary>
        /// Calculate the size of the wells
        /// </summary>
        private void calculateRectangleSize()
        {
            if (_hasSetWellSize)
            {
                calculateWellSize.LastRectangleWidth = (float)_setWellSize;
            }
            else
            {
                if (calculateWellSize.SetTheMaxHeight)
                {
                    //the size of a well is the maximum height divided by (the distance between the wells * the amount of wells in height)
                    calculateWellSize.LastRectangleWidth = calculateWellSize.RecalcMaxHeight / (calculateWellSize._shapeDistance * sizeHandler._heightWellPlate);
                }
                else
                {
                    //the size of a well is the maximum size divided by (the distance between the wells * the amount of wells in width)
                    calculateWellSize.LastRectangleWidth = calculateWellSize.RecalcMaxWidth / (calculateWellSize._shapeDistance * sizeHandler._widthWellPlate);
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
                Name = $"{_alphabet[height]}{width + 1}_{1 + height + sizeHandler._heightWellPlate * width}",
                Fill = new SolidColorBrush(_setGridColor),
                Effect = null
            };

            _coordinates.Add(rectangle.Name);

            //checks if the user wants a rectangle. Otherwise it will round the rectangles so it looks like circles.
            rectangle.RadiusX = calculateWellSize._wellRoundedCorner;
            rectangle.RadiusY = calculateWellSize._wellRoundedCorner;

            //the size of the wells
            rectangle.Width = calculateWellSize.LastRectangleWidth;
            rectangle.Height = calculateWellSize.LastRectangleWidth;

            //checks if stroke color is set and sets the stroke afterwards.
            if (_setStrokeColor)
            {
                rectangle.Stroke = new SolidColorBrush(_strokeColor);
                rectangle.StrokeThickness = rectangle.Width * _strokeThickness;
            }

            //the distance between the letters and the wells is 5/6 the size of a well
            _letterDistance = (float)(rectangle.Width / 1.2);

            //takes care of the position of the rectangle
            rectangle.Margin = new Thickness(
                _letterDistance + (calculateWellSize._shapeDistance * rectangle.Width) - rectangle.Width + width * rectangle.Width * calculateWellSize._shapeDistance, 0, 0,
                (calculateWellSize._shapeDistance * rectangle.Height) - rectangle.Height + (sizeHandler._heightWellPlate * rectangle.Width - (height * rectangle.Width) - rectangle.Height) * calculateWellSize._shapeDistance);

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
                TextBlock lblAlphabetic = new TextBlock();
                lblAlphabetic.Text = $"{_alphabet[height]}";
                lblAlphabetic.Foreground = new SolidColorBrush(_setLabelColor);
                lblAlphabetic.HorizontalAlignment = HorizontalAlignment.Left;
                lblAlphabetic.VerticalAlignment = VerticalAlignment.Bottom;
                lblAlphabetic.Width = calculateWellSize.LastRectangleWidth;
                lblAlphabetic.Height = calculateWellSize.LastRectangleWidth;
                lblAlphabetic.FontSize = calculateWellSize.LastRectangleWidth * _fontSizeModifier;
                lblAlphabetic.TextAlignment = TextAlignment.Center;
                lblAlphabetic.Margin = new Thickness(0, 0, 0,
                    (calculateWellSize._shapeDistance * calculateWellSize.LastRectangleWidth) - calculateWellSize.LastRectangleWidth + (sizeHandler._heightWellPlate * calculateWellSize.LastRectangleWidth - (height * calculateWellSize.LastRectangleWidth) - calculateWellSize.LastRectangleWidth) * calculateWellSize._shapeDistance - (calculateWellSize.LastRectangleWidth / 4));
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
                lblNumeric.Width = calculateWellSize.LastRectangleWidth;
                lblNumeric.FontSize = calculateWellSize.LastRectangleWidth * _fontSizeModifier;

                lblNumeric.Margin = new Thickness(
                    _letterDistance + (calculateWellSize._shapeDistance * calculateWellSize.LastRectangleWidth) - calculateWellSize.LastRectangleWidth + width * calculateWellSize.LastRectangleWidth * calculateWellSize._shapeDistance,
                    0,
                    0,
                    (calculateWellSize._shapeDistance * calculateWellSize.LastRectangleWidth) - calculateWellSize.LastRectangleWidth + sizeHandler._heightWellPlate * calculateWellSize.LastRectangleWidth * calculateWellSize._shapeDistance);
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
            rectOutline.Stroke = new SolidColorBrush(_setBorderColor);
            rectOutline.StrokeThickness = calculateWellSize.LastRectangleWidth * 0.1; //the 0.1 makes sure the stroke is 10% of the width of a well
            rectOutline.Width = calculateWellSize.LastRectangleWidth * sizeHandler._widthWellPlate * calculateWellSize._shapeDistance + (calculateWellSize._shapeDistance * calculateWellSize.LastRectangleWidth) - calculateWellSize.LastRectangleWidth;
            rectOutline.Height = calculateWellSize.LastRectangleWidth * sizeHandler._heightWellPlate * calculateWellSize._shapeDistance + (calculateWellSize._shapeDistance * calculateWellSize.LastRectangleWidth) - calculateWellSize.LastRectangleWidth;
            rectOutline.Effect = null;

            //if the size of a well is bigger than 15 and not a rectangle it will round the border
            if (calculateWellSize.LastRectangleWidth > 15 && !IsRectangle)
            {
                rectOutline.RadiusX = 15;
                rectOutline.RadiusY = 15;
            }
            else if(!IsRectangle)
            {
                rectOutline.RadiusX = 3;
                rectOutline.RadiusY = 3;
            }
            else
            {
                rectOutline.RadiusX = 0;
                rectOutline.RadiusY = 0;
            }
        }

        /// <summary>
        /// Generates the wellplate based on the values the user entered. If it doesn't find any values that the user entered, it will use default values:<br></br>
        /// The default values are:<br></br>
        /// GridColor = Color.FromRgb(209, 232, 247); (lightblue)<br></br>
        /// ClickColor = Colors.CadetBlue; (blue)<br></br>
        /// LabelColor = Colors.Black<br></br>
        /// BorderColor = Colors.LightGray<br></br>
        /// IsEditable = false;<br></br>
        /// IsRectangle = false; <br></br>
        /// </summary>
        /// <returns>True. If something goes wrong, it will always be somewhere else in the code.<br></br>
        /// Returns True, because at the time of creation I didn't know how to make a void from a function with the conn-class</returns>
        public bool DrawWellPlate()
        {
            HidePlaceHolder();

            prepareValues();

            //generates the shapes
            for (int height = 0; height < sizeHandler._heightWellPlate; height++)
            {
                for (int width = 0; width < sizeHandler._widthWellPlate; width++)
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
        /// <returns>True if everything succeeds or false if the code doesn't find the entered coordinate</returns>
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
        /// <returns>True if everything succeeds or false if the code doesn't find the numbered well</returns>
        public bool ColorCoordinate(int coordinate)
        {
            foreach (object child in gGenerateWellPlate.Children)
            {
                Rectangle rectangle = child as Rectangle;

                if (rectangle != null)
                {
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
        /// <returns>True if everything succeeds or false if the code doesn't find the entered coordinate</returns>
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
        /// <returns>True if everything succeeds or false if the code doesn't find the numbered well</returns>
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
        /// <para>Don't forget to set the IsEditable variable to true to let the user change the color of the wells by clicking</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ClickForColor(object sender, MouseButtonEventArgs e)
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
            WellClicked?.Invoke();
        }

        /// <summary>
        /// <para>Input a number and get the coordinate it belongs to</para>
        /// <example>Example: '4' in a 8x6 returns D1</example>
        /// </summary>
        /// <param name="coordinate">Integer that gets used to convert to the correct coordinate</param>
        /// <returns>String with the coordinate the number is linked to.<br></br>It will return an empty if it doesn't find anything. </returns>
        public string NumberToCoordinate(int coordinate)
        {
            string convertedCoordinate = getCoordinateInfo.NumberToCoordinate(coordinate, _coordinates);
            return convertedCoordinate;
        }

        /// <summary>
        /// <para>Input a coordinate and get the number it belongs to</para>
        /// <example>Example: 'D1' in a 8x6 grid returns 4</example>
        /// </summary>
        /// <param name="coordinate">String that gets used to convert to the correct number</param>
        /// <returns>Integer with the number the coordinate is linked to. <br></br>it will return -1 if it doesn't find anything</returns>
        public int CoordinateToNumber(string coordinate)
        {
            int convertedNumber = getCoordinateInfo.CoordinateToNumber(coordinate, _coordinates);
            return convertedNumber;
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
