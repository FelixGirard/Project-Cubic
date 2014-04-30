using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ProjetCubic
{
    public class Event
    {
        private int _iTemps;
        private int _iPositionX;
        private int _iPositionY;
        public Event(int iPositionX, int iPositionY, int iTemp)
        {
            _iPositionX = iPositionX;
            _iPositionY = iPositionY;
            _iTemps = iTemp;

        }
        public int iTemps
        {
            get
            {
                return _iTemps;
            }
            set
            {
                _iTemps = value;
            }
        }

        public int PositionX
        {
            get
            {
                return _iPositionX;
            }
            set
            {
                _iPositionX = value;
            }
        }
        public int PositionY
        {
            get
            {
                return _iPositionY;
            }
            set
            {
                _iPositionY = value;
            }
        }
    }
}
