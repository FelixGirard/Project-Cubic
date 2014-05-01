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
using System.Diagnostics;

namespace ProjetCubic
{
    public partial class FrmCubic : Form
    {
        List<Event> _lstEvents;
        private KeyHandler Keyhandler;
        private int _iIndexEvent, _iNombreEvent, _lMax;
        private long _lTempsDeChanson, _lTempsDepart, _lPremiereNote;
        private BackgroundWorker bw = new BackgroundWorker();
        public FrmCubic()
        {
            InitializeComponent();
            _lstEvents = new List<Event>();
            _iIndexEvent = 0;
            Keyhandler = new KeyHandler(Keys.S, this);
            Keyhandler.Register();

            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("FINISHED");
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            _lTempsDepart = DateTime.Now.Ticks / 10000;
            while (DateTime.Now.Ticks / 10000 - _lTempsDepart <= _lMax)
            {
                if ((worker.CancellationPending == true))
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    _lTempsDeChanson = DateTime.Now.Ticks / 10000 - _lTempsDepart + _lPremiereNote;
                    if (_iIndexEvent < _iNombreEvent && _lstEvents.ElementAt(_iIndexEvent).iTemps <= _lTempsDeChanson + 3)
                    {
                        Debug.Write(_lstEvents.ElementAt(_iIndexEvent).iTemps + "=" + _lTempsDeChanson.ToString() + "   ");
                        _iIndexEvent++;
                        DoMouseClick();
                    }
                    else if (_iIndexEvent > _iNombreEvent)
                    {
                        this.Close();
                    }
                    Debug.Write(_lTempsDeChanson.ToString() + "\n");
                }
            }
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
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, int dwExtraInfo);

        private const uint MOUSEEVENTF_LEFTDOWN = 0x02;
        private const uint MOUSEEVENTF_LEFTUP = 0x04;
        private const uint MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const uint MOUSEEVENTF_RIGHTUP = 0x10;

        public void DoMouseClick()
        {
             //Call the imported function with the cursor's current position
             uint X = (uint)Cursor.Position.X;
             uint Y = (uint)Cursor.Position.Y;
             /*mouse_event(MOUSEEVENTF_LEFTDOWN, X, Y, 0, 0);
             System.Threading.Thread.Sleep(1);
             mouse_event(MOUSEEVENTF_LEFTUP, X, Y, 0, 0);*/
            SendKeys.SendWait("w");
            //SendKeys.Send("w");
        }
        #endregion
        #region Events
        private void HandleHotkey()
        {
            if (bw.IsBusy)
                bw.CancelAsync();
            else if (lblPathChanson.Text != "")
            {
                //this.Hide();
                MouseHook.Start();
                MouseHook.MouseAction += new EventHandler(MouseEvent);
            }
        }

        private void MouseEvent(object sender, EventArgs e)
        {
            MouseHook.stop();
            _lPremiereNote = _lstEvents.ElementAt(_iIndexEvent++).iTemps;
            _lTempsDepart = DateTime.Now.Ticks / 10000;
            _lMax = _lstEvents.Max(ev => ev.iTemps);
            bw.RunWorkerAsync();
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
