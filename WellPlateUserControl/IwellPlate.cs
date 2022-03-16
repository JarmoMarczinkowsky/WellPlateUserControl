using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Media;

namespace WellPlateUserControl
{
    interface IwellPlate
    {
        bool SetWellPlateSize(int width, int length);

        bool SetGridColor(Color gridColor);

        bool SetClickColor(Color clickColor);

        bool ColorCoordinate(string colorToCoordinate);

        //bool ColorCoordinate(int coordinate);

    }
}
