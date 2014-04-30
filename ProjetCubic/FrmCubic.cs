using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjetCubic
{
    public partial class FrmCubic : Form
    {
        List<Event> _lstEvents;
        public FrmCubic()
        {
            InitializeComponent();
            _lstEvents = new List<Event>();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
  
        }

        private void btnParcourir_Click(object sender, EventArgs e)
        {
            opfParcourirChanson.ShowDialog();
        }
    }
}
