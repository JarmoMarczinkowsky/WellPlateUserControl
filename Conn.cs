using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace WellPlateUserControl
{
    

    
    public class Conn : IwellPlate
    {
        private static WellPlateControl _wellPlate = new WellPlateControl();
        
        public bool SetWellPlateSize(int length, int width)
        {
            return _wellPlate.SetWellPlateSize(length, width);
        }

        public bool SetGridColor(string gridColor)
        {
            return _wellPlate.SetGridColor(gridColor);
        }

        public bool SetClickColor(string clickColor)
        {
            return _wellPlate.SetClickColor(clickColor);
        }

        public bool ColorCoordinate(string colorToCoordinate)
        {
            return _wellPlate.ColorCoordinate(colorToCoordinate);
        }

        //public bool ColorCoordinate(int coordinate)
        //{
        //    return _wellPlate.ColorCoordinate(coordinate);
        //}
    }
}
