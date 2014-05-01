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
        private double _dSliderVelocity;
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
            //MessageBox.Show("FINISHED");
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
                        //Debug.Write(_lstEvents.ElementAt(_iIndexEvent).iTemps + "=" + _lTempsDeChanson.ToString() + "   ");
                        _iIndexEvent++;
                        switch (_lstEvents.ElementAt(_iIndexEvent).GetType().ToString())
                        {
                            case "ProjetCubic.Point":
                                Point point = (Point)_lstEvents[_iIndexEvent];
                                point.ClickOnMousePosition();
                                break;
                            case "ProjetCubic.Slider":
                                Slider slider = (Slider)_lstEvents[_iIndexEvent];
                                slider.ClickOnMousePosition();
                                break;
                            case "ProjetCubic.Spiner":
                                Spiner spiner = (Spiner)_lstEvents[_iIndexEvent];
                                spiner.ClickOnMousePosition();
                                break;
                            default:
                                _lstEvents.ElementAt(_iIndexEvent).ClickOnMousePosition();
                                break;
                        }
                    }
                    else if (_iIndexEvent > _iNombreEvent)
                    {
                        this.Close();
                    }
                    //Debug.Write(_lTempsDeChanson.ToString() + "\n");
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

        public const uint MOUSEEVENTF_LEFTDOWN = 0x02;
        public const uint MOUSEEVENTF_LEFTUP = 0x04;
        public const uint MOUSEEVENTF_RIGHTDOWN = 0x08;
        public const uint MOUSEEVENTF_RIGHTUP = 0x10;

        #endregion
        #region Events
        private void HandleHotkey()
        {
            if (bw.IsBusy)
            {
                bw.CancelAsync();
                this.Show();
            }
            else if (lblPathChanson.Text != "")
            {
                this.Hide();
                MouseHook.Start();
                MouseHook.MouseAction += new EventHandler(MouseEvent);
            }
        }

        private void MouseEvent(object sender, EventArgs e)
        {
            MouseHook.stop();
            _lTempsDeChanson = 0;
            _iIndexEvent = 0;
            _lPremiereNote = _lstEvents.ElementAt(_iIndexEvent++).iTemps;
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
                _lstEvents.Clear();
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
                        else if (sLigne.StartsWith("SliderMultiplier:"))
                            double.TryParse(sLigne.Substring(sLigne.IndexOf(':') +1 , sLigne.Length - sLigne.IndexOf(':') -1), out _dSliderVelocity);
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
            string[] sEventParams = sLigne.Split(',');
            int[] iEventParams = new int[sEventParams.Count()];
            int iCPT = 0;
            foreach (string param in sEventParams)
            {
                int.TryParse(sEventParams[iCPT], out iEventParams[iCPT++]);
            }
            Event evenement = new Event();
            switch (iEventParams[3])
            {
                case 1:
                case 5:
                    evenement = new Point(iEventParams[0], iEventParams[1], iEventParams[2]);
                    break;
                case 2:
                case 6:
                    evenement = new Slider(iEventParams[0], iEventParams[1], iEventParams[2], (byte)iEventParams[6], iEventParams[7]);
                    break;
                case 12:
                    evenement = new Spiner(iEventParams[0], iEventParams[1], iEventParams[2], iEventParams[5]);
                    break;
                default:
                    evenement = new Event(iEventParams[0], iEventParams[1], iEventParams[2]);
                    break;
            }
            return evenement;
        }
        private void TrierListeOrdre()
        {
            _lstEvents.OrderBy(evenement => evenement.iTemps);
        }
    }
}
