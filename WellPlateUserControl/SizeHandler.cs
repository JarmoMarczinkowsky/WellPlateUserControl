using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WellPlateUserControl
{
    class SizeHandler
    {
        public int _heightWellPlate;
        public int _widthWellPlate;

        /// <summary>
        /// <para>Is responsible for the size of the wellplate</para>
        /// </summary>
        /// <param name="inputWidth">The width that the grid is going to be</param>
        /// <param name="inputHeight">The height that the grid is going to be</param>
        /// <returns>True if method succeeds and an out of range error if a values are higher than 26 or smaller than 1</returns>
        public bool SetWellPlateSize(int inputWidth, int inputHeight, string _alphabet)
        {
            if (inputWidth > 0 && inputWidth <= _alphabet.Length
                               && inputHeight > 0
                               && inputHeight <= _alphabet.Length)
            {
                _heightWellPlate = inputHeight;
                _widthWellPlate = inputWidth;

                return true;
            }
            else
            {
                throw new ArgumentOutOfRangeException($"Number can't be bigger than 26 or smaller than 1: length = {inputWidth}, width = {inputHeight}");
            }
        }
    }
}
