using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace WellPlateUserControl
{
    interface IwellPlate
    {
        bool SetWellPlateSize(int width, int length);

        bool SetGridColor(string gridColor);

        bool SetClickColor(string clickColor);

        bool ColorCoordinate(string colorToCoordinate);

        //bool ColorCoordinate(int coordinate);

    }
}
