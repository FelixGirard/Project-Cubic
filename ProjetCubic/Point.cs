using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ProjetCubic
{
    public class Point : Event
    {
        public Point(int iTemp,int iPositionX,int iPositionY): base(iTemp,iPositionX,iPositionY)
        {

        }
        public override void ClickOnMousePosition()
        {
            uint X = (uint)Cursor.Position.X;
            uint Y = (uint)Cursor.Position.Y;
            FrmCubic.mouse_event(FrmCubic.MOUSEEVENTF_LEFTDOWN, X, Y, 0, 0);
            Debug.WriteLine("pressing");
            System.Threading.Thread.Sleep(30);
            FrmCubic.mouse_event(FrmCubic.MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
        }
        public override void ClickOnMyPosition()
        {
            FrmCubic.mouse_event(FrmCubic.MOUSEEVENTF_LEFTDOWN, (uint)this.PositionX, (uint)this.PositionY, 0, 0);
            System.Threading.Thread.Sleep(30);
            FrmCubic.mouse_event(FrmCubic.MOUSEEVENTF_LEFTUP, (uint)this.PositionX, (uint)this.PositionY, 0, 0);
        }
    }
}
