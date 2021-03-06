﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ProjetCubic
{
    public class Slider : Event
    {
        private double _dLongueur;
        private byte _byAllerRetour;
        public Slider(int iTemp, int iPositionX, int iPositionY, byte byAllerRetour, double iLongueur)
            : base(iTemp, iPositionX, iPositionY)
        {
            _dLongueur = iLongueur;
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

        public double Longueur
        {
            get
            {
                return _dLongueur;
            }
            set
            {
                _dLongueur = value;
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
            System.Threading.Thread.Sleep((int)( _byAllerRetour *((10*_dLongueur)/(FrmCubic._dSliderVelocity * 1000/FrmCubic._TempsParBattement))));
            FrmCubic.mouse_event(FrmCubic.MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
        }
        public override void ClickOnMyPosition()
        {
            FrmCubic.mouse_event(FrmCubic.MOUSEEVENTF_LEFTDOWN, (uint)this.PositionX, (uint)this.PositionY, 0, 0);
            System.Threading.Thread.Sleep((int)(_byAllerRetour * ((10 * _dLongueur) / (FrmCubic._dSliderVelocity * 1000 / FrmCubic._TempsParBattement))));
            FrmCubic.mouse_event(FrmCubic.MOUSEEVENTF_LEFTUP, (uint)this.PositionX, (uint)this.PositionY, 0, 0);
        }
    }
}
