using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCubic
{
    class TimingPoint
    {
        private int _Offset;

        public int Offset
        {
            get { return _Offset; }
            set { _Offset = value; }
        }
        private double _TempsParBattement;

        public double TempsParBattement
        {
            get { return _TempsParBattement; }
            set { _TempsParBattement = value; }
        }
        private int _TempsParMesure;

        public int TempsParMesure
        {
            get { return _TempsParMesure; }
            set { _TempsParMesure = value; }
        }
        private int _Sample;

        public int Sample
        {
            get { return _Sample; }
            set { _Sample = value; }
        }
        private byte _PourcentageVolume;

        public byte PourcentageVolume
        {
            get { return _PourcentageVolume; }
            set { _PourcentageVolume = value; }
        }
        private byte _Style;

        public byte Style
        {
            get { return _Style; }
            set { _Style = value; }
        }
        public TimingPoint(int Offset, double TempsParBattement, int TempsParMesure, int Sample, int PourcentageVolume, int Style)
        {
            _Offset = Offset;
            _TempsParBattement = TempsParBattement;
            _TempsParMesure = TempsParMesure;
            _Sample = Sample;
            _PourcentageVolume = (byte)PourcentageVolume;
            _Style = (byte)Style;
        
        }
    }
}
