using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ProjetCubic
{
    public class Slider : Event
    {
        private int _iLongueur;
        private byte _byAllerRetour;
        public Slider(int iTemp, int iPositionX, int iPositionY, byte byAllerRetour, int iLongueur)
            : base(iTemp, iPositionX, iPositionY)
        {
            _iLongueur = iLongueur;
            _byAllerRetour = byAllerRetour;
        }
        public List<Point> Positions
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public int Longueur
        {
            get
            {
                return _iLongueur;
            }
            set
            {
                _iLongueur = value;
            }
        }

        public byte AllerRetour
        {
            get
            {
                return _byAllerRetour;
            }
            set
            {
                _byAllerRetour = value;
            }
        }
        public override void ClickOnMousePosition()
        {
            uint X = (uint)Cursor.Position.X;
            uint Y = (uint)Cursor.Position.Y;
            FrmCubic.mouse_event(FrmCubic.MOUSEEVENTF_LEFTDOWN, X, Y, 0, 0);
            System.Threading.Thread.Sleep((int)(_iLongueur * _byAllerRetour * FrmCubic._dSliderVelocity * FrmCubic._TempsParBattement / 100));
            FrmCubic.mouse_event(FrmCubic.MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
        }
        public override void ClickOnMyPosition()
        {
            FrmCubic.mouse_event(FrmCubic.MOUSEEVENTF_LEFTDOWN, (uint)this.PositionX, (uint)this.PositionY, 0, 0);
            System.Threading.Thread.Sleep((int)(_iLongueur * _byAllerRetour * FrmCubic._dSliderVelocity * FrmCubic._TempsParBattement / 100));
            FrmCubic.mouse_event(FrmCubic.MOUSEEVENTF_LEFTUP, (uint)this.PositionX, (uint)this.PositionY, 0, 0);
        }
    }
}
