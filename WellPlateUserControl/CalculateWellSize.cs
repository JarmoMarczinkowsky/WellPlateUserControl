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
    }
}
