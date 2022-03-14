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
            throw new NotImplementedException();
        }

        public bool SetClickColor(string clickColor)
        {
            throw new NotImplementedException();
        }

        public bool ColorCoordinate(string colorToCoordinate)
        {
            throw new NotImplementedException();
        }

        public bool ColorCoordinate(int coordinate)
        {
            throw new NotImplementedException();
        }
    }
}
