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
        
        private int _widthWellPlate;
        private int _heightWellPlate;
        private int _shapeSize = 15;
        private int _shapeDistance = 1;
        private int _distanceFromWall = 5;
        private int _loopCounter;

        private float _circleSizeMultiplier = 1;
        
        private bool _setTheGridColor;
        private bool _setTheClickColor;
        private bool _setStrokeColor;
        

        private Color _colorConverter;
        private Color _clickColorConverter;
        private Color _strokeColor;
        private Color _defaultGridColor;
        private Color _defaultClickColor;
        //private Color _defaultStrokeColor;

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
                        Ellipse ellipse = new Ellipse()
                        {
                            VerticalAlignment = VerticalAlignment.Bottom,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            Width = _shapeSize * _circleSizeMultiplier,
                            Height = _shapeSize * _circleSizeMultiplier,
                            Name = $"{_alphabet[height]}{width + 1}_{1 + height + _heightWellPlate * width}", //$"{_alphabet[height]}{width + 1}_{_loopCounter + 1}"
                            //Stroke = new SolidColorBrush(_strokeColor),
                        };

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
                        

                        //takes care of the position of the ellipse
                        ellipse.Margin = new Thickness(
                            (_distanceFromWall + width * _shapeSize * _shapeDistance) * _circleSizeMultiplier, //left
                            0,  //up
                            0, //right
                            (_distanceFromWall + _heightWellPlate * _shapeSize - (_distanceFromWall + height * _shapeSize)) * _circleSizeMultiplier); //down

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
                //ex.Message($"Can't convert color, are you sure you're using a valid color ?").ToString();
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
            catch (Exception ex)
            {
                //ex.Message("Something went wrong with the coordinates. Are you sure you are using coordinates or numbers?{Environment.NewLine}Examples: A5 to color coordinate A5.{Environment.NewLine}5 to color ellipse number 5.{Environment.NewLine}Use ';' to color multiple wells. Example: A5;B5;13 ");
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

        /// <summary>
        /// <para>Set this before the wellplatesize</para>
        /// <para>Sets the size of the circles. It works as a multiplier</para>
        /// <example>So '2' is 2 times as big as the circles are normally and '0.5' is half so big as it is normally</example>
        /// </summary>
        /// <param name="circleSizeMultiplier"></param>
        public bool SetCircleSize(float circleSizeMultiplier)
        {
            try
            {
                _circleSizeMultiplier = circleSizeMultiplier;
                return true;
            }
            catch (Exception ex)
            {
                //ex.Message("Test");
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
                return false;
            }
        }
    }
}
