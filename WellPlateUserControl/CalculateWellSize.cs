using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WellPlateUserControl
{
    class CalculateWellSize
    {
        

        private const int _widthLeftOver = 16;
        private const int _heightLeftOver = 50;
        
        private double _setMaxHeight;
        private double _setMaxWidth;
        
        public float CalcMaxHeight;
        public float CalcMaxWidth;
        public bool SetTheMaxHeight;
        public float _shapeDistance;
        public float RecalcMaxWidth;
        public float RecalcMaxHeight;
        public float LastRectangleWidth;


        public double WellSize;

        public int _wellRoundedCorner;


        /// <summary>
        /// Checks if the width of the usercontrol is set. If it doesn't find a width, it will search for the height, otherwise it will just use a width of 600
        /// </summary>
        public void CheckControlSize(double width, double height)
        {
            if (!double.IsNaN(width))
            {
                if (width < (_widthLeftOver + 4))
                {
                    throw new ArgumentOutOfRangeException("maxWidth can't be smaller than 20");
                }
                else
                {
                    _setMaxWidth = width;
                    CalcMaxWidth = (float)(_setMaxWidth - _widthLeftOver);
                }
            }
            else if (!double.IsNaN(height))
            {
                if (height < (_heightLeftOver + 5))
                {
                    throw new ArgumentOutOfRangeException("SetMaxHeight can't be smaller than 55");
                }
                else
                {
                    _setMaxHeight = height;
                    CalcMaxHeight = (float)(_setMaxHeight - _heightLeftOver);
                    SetTheMaxHeight = true;
                }
            }
            else
            {
                _setMaxWidth = 600; //default width if it doesn't find a width or height is 600 pixels
                CalcMaxWidth = (float)(_setMaxWidth - _widthLeftOver);
            }
        }

        /// <summary>
        /// Sets the distance between 2 wells
        /// </summary>
        public void RectangleDistance(bool IsRectangle)
        {
            if (IsRectangle)
            {
                _shapeDistance = 1.05F; //1.05 is 5% the size of a well between two wells
                _wellRoundedCorner = 0; //0 = no rounded corners
            }
            else
            {
                _shapeDistance = 1.3F; //1.3 is 30% the size of a well between two wells
                _wellRoundedCorner = 1600; //extremely high value so the wells stay rounded even if they are big
            }
        }


        public void CalculateMaxSize(int widthWellPlate, int heightWellPlate)
        {
            SizeHandler sizeHandler = new();

            WellSize = CalcMaxWidth / (_shapeDistance * widthWellPlate);
            if (SetTheMaxHeight)
            {
                RecalcMaxHeight = (float)((CalcMaxHeight - (WellSize / 1.5)) * 0.9); //0.9 is 90% the size of the wellplate
                LastRectangleWidth = RecalcMaxHeight / (_shapeDistance * heightWellPlate);
            }
            else
            {
                RecalcMaxWidth = (float)((CalcMaxWidth - (WellSize / 1.5)) * 0.95); //0.95 is 95% the size of the wellplate
                LastRectangleWidth = RecalcMaxWidth / (_shapeDistance * widthWellPlate);
            }
        }

        /// <summary>
        /// Checks if the size of the highest textblock is going to be bigger than the usercontrol height. 
        /// If it is it will resize the entered height to a more suitable height
        /// </summary>
        /// <param name="highestTextblock"></param>
        public void HeightCheck(double highestTextblock, double thisWidth, double thisHeight)
        {
            double biggerThanSpaceAvailablePercentage;

            if (highestTextblock > thisWidth)
            {
                biggerThanSpaceAvailablePercentage = (highestTextblock - thisHeight) / thisHeight * 100;

                if (SetTheMaxHeight)
                {
                    RecalcMaxHeight = (float)(RecalcMaxHeight / (biggerThanSpaceAvailablePercentage + 100) * 100);
                }
                else
                {
                    RecalcMaxWidth = (float)(RecalcMaxWidth / (biggerThanSpaceAvailablePercentage + 100) * 100);
                }
            }
        }


    }
}
