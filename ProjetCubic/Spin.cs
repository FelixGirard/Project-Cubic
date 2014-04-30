using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjetCubic
{
    public class Spin : Event
    {
        public Spin(int iTemp, int iPositionX, int iPositionY)
            : base(iTemp, iPositionX, iPositionY)
        {

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
    }
}
