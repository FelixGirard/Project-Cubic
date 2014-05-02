using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ProjetCubic
{
    public class Spiner : Event
    {
        private int _iDuree;
        public Spiner(int iTemp, int iPositionX, int iPositionY,int iTempsFin)
            : base(iTemp, iPositionX, iPositionY)
        {
            _iDuree = iTempsFin - iTemp;
        }
        public int Duree
        {
            get
            {
                return _iDuree;
            }
            set
            {
                _iDuree = value;
            }
        }
        public override void ClickOnMousePosition()
        {
            uint X = (uint)Cursor.Position.X;
            uint Y = (uint)Cursor.Position.Y;
            FrmCubic.mouse_event(FrmCubic.MOUSEEVENTF_LEFTDOWN, X, Y, 0, 0);
            System.Threading.Thread.Sleep(_iDuree);
            FrmCubic.mouse_event(FrmCubic.MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
        }
    }
}
