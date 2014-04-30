using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;

namespace ProjetCubic
{
    public partial class FrmCubic : Form
    {
        List<Event> _lstEvents;
        private KeyHandler Keyhandler;
        private int _iIndexEvent,_iNombreEvent;
        private long _lTempsDeChanson, _lTempsDepart,_lPremiereNote;
        public FrmCubic()
        {
            InitializeComponent();
            _lstEvents = new List<Event>();
            _iIndexEvent = 0;
            Keyhandler = new KeyHandler(Keys.S, this);
            Keyhandler.Register();
        }
        #region keylog
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Constants.WM_HOTKEY_MSG_ID)
                HandleHotkey();
            base.WndProc(ref m);
        }
        #endregion

        #region mouselog
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(long dwFlags, long dx, long dy, long cButtons, long dwExtraInfo);

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        public void DoMouseClick()
        {
           /* //Call the imported function with the cursor's current position
            int X = Cursor.Position.X;
            int Y = Cursor.Position.Y;
            mouse_event(MOUSEEVENTF_LEFTDOWN, X, Y, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, X, Y, 0, 0);*/
            SendKeys.Send("w");
            //SendKeys.Send("w");
        }
        #endregion
        #region Events
        private void HandleHotkey()
        {
            if (lblPathChanson.Text != "")
            {
                //this.Hide();ww
                MouseHook.Start();
                MouseHook.MouseAction += new EventHandler(MouseEvent);
            }
            else
            { }
        }

        private void MouseEvent(object sender, EventArgs e)
        {
            MouseHook.stop();
            _lPremiereNote = _lstEvents.ElementAt(_iIndexEvent++).iTemps;
            _lTempsDepart = DateTime.Now.Ticks / 10000;
            while (DateTime.Now.Ticks / 10000 - _lTempsDepart <= _lstEvents.Max(ev => ev.iTemps))
            {
                _lTempsDeChanson = DateTime.Now.Ticks / 10000 - _lTempsDepart + _lPremiereNote;
                if (_iIndexEvent < _iNombreEvent && _lstEvents.ElementAt(_iIndexEvent).iTemps == _lTempsDeChanson)
                {
                    _iIndexEvent++;
                    DoMouseClick();
                }
                else if (_iIndexEvent > _iNombreEvent)
                {
                    base.Close();
                }
            }
        }

        private void btnParcourir_Click(object sender, EventArgs e)
        {
            opfParcourirChanson.ShowDialog();
            lblPathChanson.Text = opfParcourirChanson.FileName;
            ChargerListe();
        }

        #endregion
        private void ChargerListe()
        {
            if (lblPathChanson.Text != "" && File.Exists(lblPathChanson.Text))
            {
                using (StreamReader sr = File.OpenText(lblPathChanson.Text))
                {
                    bool bEventSectionStarted = false;
                    string sLigne = String.Empty;
                    while ((sLigne = sr.ReadLine()) != null)
                    {
                        if (bEventSectionStarted)
                        {
                            _lstEvents.Add(EventDeString(sLigne));
                        }
                        if (sLigne == "[HitObjects]")
                            bEventSectionStarted = true;
                    }
                }
            }
            _iNombreEvent = _lstEvents.Count();
            TrierListeOrdre();
        }
        private Event EventDeString(string sLigne)
        {
            string[] EventParams = sLigne.Split(',');
            Event evenement = new Event(Convert.ToInt32(EventParams[0]), Convert.ToInt32(EventParams[1]), Convert.ToInt32(EventParams[2]));
            return evenement;
        }
        private void TrierListeOrdre()
        {
            _lstEvents.OrderBy(evenement => evenement.iTemps);
        }
    }
}
