﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjetCubic
{
    public class Slider : Event
    {
        public Slider(int iTemp, int iPositionX, int iPositionY)
            : base(iTemp, iPositionX, iPositionY)
        {

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

        public int iDuree
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public byte byAllerRetour
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }
    }
}
