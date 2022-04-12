using System;
using System.Collections.Generic;

using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Media;

namespace WellPlateUserControl
{
    public class Conn : IwellPlate
    {
        private static WellPlateControl _wellPlate = new WellPlateControl();
        
        public bool SetWellPlateSize(int length, int width)
        {
            return _wellPlate.SetWellPlateSize(length, width);
        }

        public bool ColorCoordinate(string colorToCoordinate)
        {
            return _wellPlate.ColorCoordinate(colorToCoordinate);
        }

        public bool ColorCoordinate(string coordinate, Color chosenColor)
        {
            return _wellPlate.ColorCoordinate(coordinate, chosenColor);
        }

        public bool ColorCoordinate(int coordinate, Color chosenColor)
        {
            return _wellPlate.ColorCoordinate(coordinate, chosenColor);
        }

        public string NumberToCoordinate(int coordinate)
        {
            return _wellPlate.NumberToCoordinate(coordinate);
        }

        public int CoordinateToNumber(string coordinate)
        {
            return _wellPlate.CoordinateToNumber(coordinate);
        }

        public bool DrawWellPlate()
        {
            return _wellPlate.DrawWellPlate();
        }

        public void Clear()
        {
            _wellPlate.Clear();
        }
    }
}
