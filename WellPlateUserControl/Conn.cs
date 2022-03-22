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

        public bool SetGridColor(Color gridColor)
        {
            return _wellPlate.SetGridColor(gridColor);
        }

        public bool SetClickColor(Color clickColor)
        {
            return _wellPlate.SetClickColor(clickColor);
        }

        public bool ColorCoordinate(string colorToCoordinate)
        {
            return _wellPlate.ColorCoordinate(colorToCoordinate);
        }

        public bool SetCircleSize(float circleSizeMultiplier)
        {
            return _wellPlate.SetCircleSize(circleSizeMultiplier);
        }

        public bool SetStrokeColor(Color strokeColor)
        {
            return _wellPlate.SetStrokeColor(strokeColor);
        }

        public string CoordinateConverter(int coordinate)
        {
            return _wellPlate.CoordinateConverter(coordinate);
        }

        public int CoordinateConverter(string coordinate)
        {
            return _wellPlate.CoordinateConverter(coordinate);
        }

        public List<string> GiveColoredList()
        {
            return _wellPlate.GiveColoredList();
        }

        public List<string> GiveNotColoredList()
        {
            return _wellPlate.GiveNotColoredList();
        }

        public string GetLastClickedCoordinate()
        {
            return _wellPlate.GetLastClickedCoordinate();
        }

        public bool IsRectangle()
        {
            return _wellPlate.IsRectangle();
        }
    }
}
