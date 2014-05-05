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
        private List<Event> _lstEvents;
        private List<TimingPoint> _lstTimingPoints;
        private KeyHandler Keyhandler;
        public static double _dSliderVelocity;
        private int _iIndexEvent, _iNombreEvent, _lMax;
        private long _lTempsDeChanson, _lTempsDepart, _lPremiereNote;
        private BackgroundWorker bwBot = new BackgroundWorker();
        private BackgroundWorker bwRechercheProcessus = new BackgroundWorker();
        private BackgroundWorker bwRechercheDebutChanson = new BackgroundWorker();
        Process processosu;
        public FrmCubic()
        {
            InitializeComponent();
            //To get files from a directory : Directory.GetFiles(path,searchPattern);
            //Directory.GetFiles(@"C:\Program Files (x86)\osu!\Songs\","*");
            _lstEvents = new List<Event>();
            _lstTimingPoints= new List<TimingPoint>();
            _iIndexEvent = 0;
            Keyhandler = new KeyHandler(Keys.S, this);
            Keyhandler.Register();

            bwBot.WorkerReportsProgress = true;
            bwBot.WorkerSupportsCancellation = true;
            bwBot.DoWork += new DoWorkEventHandler(bw_DoWork);
            bwBot.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            bwBot.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);

            bwRechercheProcessus.WorkerReportsProgress = true;
            bwRechercheProcessus.WorkerSupportsCancellation = true;
            bwRechercheProcessus.DoWork += new DoWorkEventHandler(bwRechercheProcessus_DoWork);
            bwRechercheProcessus.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwRechercheProcessus_RunWorkerCompleted);
            bwRechercheProcessus.RunWorkerAsync();

            bwRechercheDebutChanson.WorkerReportsProgress = true;
            bwRechercheDebutChanson.WorkerSupportsCancellation = true;
            bwRechercheDebutChanson.DoWork += new DoWorkEventHandler(bwRechercheDebutChanson_DoWork);
            bwRechercheDebutChanson.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwRechercheDebutChanson_RunWorkerCompleted);
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            tslblStatut.Text = "Bot stopped!";
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            tslblStatut.Text = "Bot running...!";
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
                    if (_iIndexEvent < _iNombreEvent - 1 && _lstEvents.ElementAt(_iIndexEvent).iTemps <= _lTempsDeChanson + 3)
                    {
                        tslblStatut.Text = "Bot running...!   Note en cours : " + _iIndexEvent;
                        _lstEvents.ElementAt(_iIndexEvent++).ClickOnMousePosition();
                        //Debug.Write(_lstEvents.ElementAt(_iIndexEvent).iTemps + "=" + _lTempsDeChanson.ToString() + "   ");
                    }
                    else if (_iIndexEvent >= _iNombreEvent - 1)
                    {
                        this.Close();
                    }
                    //Debug.Write(_lTempsDeChanson.ToString() + "\n");
                }
            }
        }
        private void bwRechercheProcessus_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            tslblStatut.Text = "Osu detected!";
            bwRechercheDebutChanson.RunWorkerAsync();
        }

        private void bwRechercheProcessus_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            while (Process.GetProcessesByName("osu!").Count() == 0)
            {
                tslblStatut.Text = "Waiting for Osu! to start.";
                if ((worker.CancellationPending == true))
                {
                    e.Cancel = true;
                    break;
                }
            }
            processosu = Process.GetProcessesByName("osu!").First();
        }

        private void bwRechercheDebutChanson_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            tslblStatut.Text = "song found";
        }

        private void bwRechercheDebutChanson_DoWork(object sender, DoWorkEventArgs e)
        {
            tslblStatut.Text = "Waiting for a song to start";
            BackgroundWorker worker = sender as BackgroundWorker;
            string sTitreFenêtre, sTitreChanson;

            if(Process.GetProcessesByName("osu!").Count() > 0)
                processosu = Process.GetProcessesByName("osu!").First();

            sTitreFenêtre = processosu.MainWindowTitle.Trim();
            while (sTitreFenêtre=="osu!" && Process.GetProcessesByName("osu!").Count() > 0)
            {
                if ((worker.CancellationPending == true))
                {
                    e.Cancel = true;
                    break;
                }
                processosu = Process.GetProcessesByName("osu!").First();
                sTitreFenêtre = processosu.MainWindowTitle.Trim();
            }
            sTitreChanson = sTitreFenêtre.Substring(sTitreFenêtre.IndexOf('-') + 1, sTitreFenêtre.Length - sTitreFenêtre.IndexOf('-') - 1).Trim();
            Directory.GetFiles(lblPathChanson.Text, "*" + sTitreChanson + "*",
                                         SearchOption.AllDirectories);
            opfParcourirChanson.FileName = sTitreChanson;
            lblPathChanson.Text = opfParcourirChanson.FileName;
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
            if (bwBot.IsBusy)
            {
                bwBot.CancelAsync();
                this.Show();
            }
            else if (lblPathChanson.Text != "" && !bwRechercheProcessus.IsBusy)
            {
                this.Hide();
                MouseHook.Start();
                MouseHook.MouseAction += new EventHandler(MouseEvent);
            }
            else if (bwRechercheProcessus.IsBusy)
                MessageBox.Show("Osu doit être démarré pour cela ! Il n'est actuellement pas détecté.");
        }

        private void MouseEvent(object sender, EventArgs e)
        {
            MouseHook.stop();
            _lTempsDeChanson = 0;
            _iIndexEvent = 0;
            _lPremiereNote = _lstEvents.ElementAt(_iIndexEvent++).iTemps;
            _lMax = _lstEvents.Max(ev => ev.iTemps);
            bwBot.RunWorkerAsync();
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
                    bool bTimingPointsSectionStarted = false;
                    string sLigne = String.Empty;
                    while ((sLigne = sr.ReadLine()) != null)
                    {
                        if (bTimingPointsSectionStarted && sLigne == "")
                            bTimingPointsSectionStarted = false;

                        if (bEventSectionStarted)
                        {
                            _lstEvents.Add(EventDeString(sLigne));
                        }
                        else if (bTimingPointsSectionStarted)
                        {
                            _lstTimingPoints.Add(TPDeString(sLigne));
                        }
                        else if (sLigne.StartsWith("SliderMultiplier:"))
                            double.TryParse(sLigne.Substring(sLigne.IndexOf(':') + 1, sLigne.Length - sLigne.IndexOf(':') - 1).Replace('.', ','), out _dSliderVelocity);

                        if (sLigne == "[TimingPoints]")
                            bTimingPointsSectionStarted = true;

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
        private TimingPoint TPDeString(string sLigne)
        {
            string[] sTimingPointParams = sLigne.Split(',');
            int[] iTimingPointParams = new int[sTimingPointParams.Count()];
            int iCPT = 0;
            foreach (string param in sTimingPointParams)
            {
                int.TryParse(sTimingPointParams[iCPT], out iTimingPointParams[iCPT++]);
            }
            TimingPoint TimingP = new TimingPoint(iTimingPointParams[0], iTimingPointParams[1], iTimingPointParams[2], iTimingPointParams[3], iTimingPointParams[5], iTimingPointParams[7]);
            return TimingP;
        }
        private void TrierListeOrdre()
        {
            _lstEvents.OrderBy(evenement => evenement.iTemps);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            opfParcourirDossierChanson.ShowDialog();
            lblPathChanson.Text = opfParcourirDossierChanson.SelectedPath;
        }
    }
}
