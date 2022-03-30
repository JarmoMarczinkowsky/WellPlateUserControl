﻿using System;
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

        public bool ColorCoordinate(string coordinate, Color chosenColor)
        {
            return _wellPlate.ColorCoordinate(coordinate, chosenColor);
        }

        public bool ColorCoordinate(int coordinate, Color chosenColor)
        {
            return _wellPlate.ColorCoordinate(coordinate, chosenColor);
        }

        //public bool SetCircleSize(double circleSizeMultiplier)
        //{
        //    return _wellPlate.SetCircleSize(circleSizeMultiplier);
        //}

        public bool SetStroke(Color strokeColor, double strokeThickness)
        {
            return _wellPlate.SetStroke(strokeColor, strokeThickness);
        }

        public string NumberToCoordinate(int coordinate)
        {
            return _wellPlate.NumberToCoordinate(coordinate);
        }

        public int CoordinateToNumber(string coordinate)
        {
            return _wellPlate.CoordinateToNumber(coordinate);
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

        public bool SetMaxWidth(int maxWidth)
        {
            return _wellPlate.SetMaxWidth(maxWidth);
        }

        public bool SetMaxHeight(int maxHeight)
        {
            return _wellPlate.SetMaxHeight(maxHeight);
        }
    }
}
