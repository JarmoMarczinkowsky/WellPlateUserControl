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

        bool SetCircleSize(float circleSizeMultiplier);

        bool SetStrokeColor(Color strokeColor);

        string CoordinateConverter(int coordinate);

        int CoordinateConverter(string coordinate);

        List<string> GiveColoredList();

        List<string> GiveNotColoredList();

        public string GetLastClickedCoordinate();

        public bool IsRectangle();

    }
}
