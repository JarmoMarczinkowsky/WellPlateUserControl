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

        public bool ColorCoordinate(string coordinate, Color chosenColor);

        public bool ColorCoordinate(int coordinate, Color chosenColor);

        bool SetStroke(Color strokeColor, double strokeThickness);

        string NumberToCoordinate(int coordinate);

        int CoordinateToNumber(string coordinate);

        List<string> GiveColoredList();

        List<string> GiveNotColoredList();

        public string GetLastClickedCoordinate();

        public bool IsRectangle();

    }
}
