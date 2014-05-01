using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace ProjetCubic
{
    public class Event
    {
        private int _iTemps;
        private int _iPositionX;
        private int _iPositionY;
        public Event()
        {

        }
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
        public virtual void ClickOnMousePosition()
        {
            uint X = (uint)Cursor.Position.X;
            uint Y = (uint)Cursor.Position.Y;
            FrmCubic.mouse_event(FrmCubic.MOUSEEVENTF_LEFTDOWN, X, Y, 0, 0);
            System.Threading.Thread.Sleep(5);
            FrmCubic.mouse_event(FrmCubic.MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
        }
        public virtual void ClickOnMyPosition()
        {
            FrmCubic.mouse_event(FrmCubic.MOUSEEVENTF_LEFTDOWN, (uint)_iPositionX, (uint)_iPositionY, 0, 0);
            System.Threading.Thread.Sleep(5);
            FrmCubic.mouse_event(FrmCubic.MOUSEEVENTF_LEFTUP, (uint)_iPositionX, (uint)_iPositionY, 0, 0);
        }
    }
}
