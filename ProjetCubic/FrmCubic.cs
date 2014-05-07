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
using CSCore.CoreAudioAPI;

namespace ProjetCubic
{
    public partial class FrmCubic : Form
    {
        private List<Event> _lstEvents;
        private List<TimingPoint> _lstTimingPoints;
        private KeyHandler Keyhandler;
        public static double _dSliderVelocity;
        private int _iIndexEvent, _iIndexTP, _iNombreEvent, _iNombreTP, _lMax;
        public static double _TempsParBattement, _TempsParBattementBase;
        private string sPathDossierChanson = @"C:\Program Files (x86)\osu!\Songs\";
        private long _lTempsDeChanson, _lTempsDepart, _lPremiereNote, _lTempsDepart1;
        public Rectangle _WindowsRect;
        private BackgroundWorker bwBot = new BackgroundWorker();
        private BackgroundWorker bwRechercheProcessus = new BackgroundWorker();
        private BackgroundWorker bwRechercheDebutChanson = new BackgroundWorker();
        private BackgroundWorker bwRechercheFinChanson = new BackgroundWorker();
        private BackgroundWorker bwDetectionSon = new BackgroundWorker();
        private Process _processosu;
        public FrmCubic()
        {

            InitializeComponent();
            _lstEvents = new List<Event>();
            _lstTimingPoints = new List<TimingPoint>();
            _iIndexEvent = 0;
            _iIndexTP = 0;
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

            bwRechercheFinChanson.WorkerReportsProgress = true;
            bwRechercheFinChanson.WorkerSupportsCancellation = true;
            bwRechercheFinChanson.DoWork += new DoWorkEventHandler(bwRechercheFinChanson_DoWork);
            bwRechercheFinChanson.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwRechercheFinChanson_RunWorkerCompleted);

            bwDetectionSon.WorkerReportsProgress = true;
            bwDetectionSon.WorkerSupportsCancellation = true;
            bwDetectionSon.DoWork += new DoWorkEventHandler(bwDetectionSon_DoWork);
            bwDetectionSon.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwDetectionSon_RunWorkerCompleted);

        }

        #region AnalyseSon
        private byte getOutputSound()
        {
            return (byte)(GetDefaultAudioSessionManager2(DataFlow.Render).GetSessionEnumerator().Where(item => item.DisplayName == "").Max(value => value.QueryInterface<AudioMeterInformation>().GetPeakValue()) * 100);
        }
        private static AudioSessionManager2 GetDefaultAudioSessionManager2(DataFlow dataFlow)
        {
            using (var enumerator = new MMDeviceEnumerator())
            {
                using (var device = enumerator.GetDefaultAudioEndpoint(dataFlow, Role.Multimedia))
                {
                    var sessionManager = AudioSessionManager2.FromMMDevice(device);
                    return sessionManager;
                }
            }
        }
#endregion
        #region BackgroundWorker
        private void bwDetectionSon_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            //_lTempsDepart1 = DateTime.Now.Ticks / 10000;
            //DémarrerBot();
            _lTempsDeChanson = 0;
            _iIndexTP = 0;
            _iIndexEvent = 0;
            _lPremiereNote = 2;
            _lMax = _lstEvents.Max(ev => ev.iTemps);
            _TempsParBattementBase = _lstTimingPoints.ElementAt(_iIndexTP++).TempsParBattement;
            _TempsParBattement = _TempsParBattementBase;
            bwBot.RunWorkerAsync();
            bwDetectionSon.Dispose();
        }

        private void bwDetectionSon_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            byte byVolume = getOutputSound();
            while (byVolume<1)
            {
                Debug.WriteLine(byVolume);
                if ((worker.CancellationPending == true))
                {
                    e.Cancel = true;
                    break;
                }
                byVolume = getOutputSound();
            }
            /*Color CouleurPixel;
            List<int> Durees = new List<int>();
            CouleurPixel = GetPixelColor(_WindowsRect.Left + 4, _WindowsRect.Height - 3);
            while (!(CouleurPixel.R >= 135 && CouleurPixel.R <= 150 && CouleurPixel.G >= 135 && CouleurPixel.G <= 150 && CouleurPixel.B >= 75 && CouleurPixel.B <= 90))
            {
                _lTempsDepart1 = DateTime.Now.Ticks / 10000;
                tslblStatut.Text = CouleurPixel.ToString() + "   " + (_WindowsRect.Left + 3) + (_WindowsRect.Height - 4);
                if ((worker.CancellationPending == true))
                {
                    e.Cancel = true;
                    break;
                }
                CouleurPixel = GetPixelColor(_WindowsRect.Left + 3, _WindowsRect.Height - 4);
                Durees.Add((int)(DateTime.Now.Ticks / 10000 - _lTempsDepart1));
            }
            MessageBox.Show(Durees.Average().ToString());
             */
        }
        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            tslblStatut.Text = "Bot stopped!";
            bwBot.Dispose();
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
            _lTempsDepart1 = _lTempsDepart - _lTempsDepart1;
            _TempsParBattement = 500;
            while (DateTime.Now.Ticks / 10000 - _lTempsDepart <= _lMax)
            {
                if ((worker.CancellationPending == true))
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    _lTempsDeChanson = DateTime.Now.Ticks / 10000 - _lTempsDepart + _lPremiereNote + 13;
                    if (_iIndexTP <= _iNombreTP - 1 && _lstTimingPoints.ElementAt(_iIndexTP).Offset <= _lTempsDeChanson)
                    {
                        Debug.Write(_lstTimingPoints.ElementAt(_iIndexTP).Offset + "    ");
                        if (_lstTimingPoints.ElementAt(_iIndexTP++).TempsParBattement < 0)
                            _TempsParBattement = 100 / _lstTimingPoints.ElementAt(_iIndexTP++).TempsParBattement * -1 * _TempsParBattementBase;
                        else
                            _TempsParBattement = _lstTimingPoints.ElementAt(_iIndexTP++).TempsParBattement;
                    }

                    if (_iIndexEvent <= _iNombreEvent - 1 && ((_lstEvents.ElementAt(_iIndexEvent).GetType().ToString() != "ProjetCubic.Point" && _lstEvents.ElementAt(_iIndexEvent).iTemps <= _lTempsDeChanson) || (_lstEvents.ElementAt(_iIndexEvent).GetType().ToString() == "ProjetCubic.Point" && _lstEvents.ElementAt(_iIndexEvent).iTemps <= _lTempsDeChanson)))
                    {
                        tslblStatut.Text = "Bot running...!   Note en cours : " + (_iIndexEvent + 1);
                        Debug.Write(_lstEvents.ElementAt(_iIndexEvent).iTemps + "=" + _lTempsDeChanson.ToString() + "   ");
                        _lstEvents.ElementAt(_iIndexEvent++).ClickOnMousePosition();
                    }
                    else if (_iIndexEvent >= _iNombreEvent - 1)
                    {
                        //Application.Exit();
                    }
                    Debug.Write(_lTempsDeChanson.ToString() + "\n");
                }
            }
        }
        private void bwRechercheProcessus_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            tslblStatut.Text = "Osu detected!";
            bwRechercheDebutChanson.RunWorkerAsync();
            bwRechercheProcessus.Dispose();
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
            _processosu = Process.GetProcessesByName("osu!").First();
        }

        private void bwRechercheDebutChanson_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //_lTempsDepart1 = DateTime.Now.Ticks / 10000;
            if (tslblStatut.Text.Contains(" à été trouvée!"))
                lblPathChanson.Text = opfParcourirChanson.FileName;
            ChargerListe();
            //getosuWindowSize();
            if (!bwDetectionSon.IsBusy)
            bwDetectionSon.RunWorkerAsync();
            if (!bwRechercheFinChanson.IsBusy)
            bwRechercheFinChanson.RunWorkerAsync();
            bwRechercheDebutChanson.Dispose();
        }

        private void bwRechercheDebutChanson_DoWork(object sender, DoWorkEventArgs e)
        {
            tslblStatut.Text = "Waiting for a song to start";
            BackgroundWorker worker = sender as BackgroundWorker;
            string sTitreFenêtre, sTitreChanson, sDifficultee;

            if (Process.GetProcessesByName("osu!").Count() > 0)
                _processosu = Process.GetProcessesByName("osu!").First();

            sTitreFenêtre = _processosu.MainWindowTitle.Trim();
            while (sTitreFenêtre == "osu!" && Process.GetProcessesByName("osu!").Count() > 0 || sTitreFenêtre == "")
            {
                if ((worker.CancellationPending == true))
                {
                    e.Cancel = true;
                    break;
                }
                _processosu = Process.GetProcessesByName("osu!").First();
                sTitreFenêtre = _processosu.MainWindowTitle.Trim();
            }
            sTitreChanson = sTitreFenêtre.Substring(sTitreFenêtre.IndexOf('-') + 1, sTitreFenêtre.Length - sTitreFenêtre.IndexOf('-') - 1).Trim();
            sDifficultee = sTitreChanson.Substring(sTitreChanson.IndexOf('['), sTitreChanson.Length - sTitreChanson.IndexOf('[')).Trim();
            sTitreChanson = sTitreChanson.Substring(0, sTitreChanson.IndexOf('[') - 1).Trim();
            string[] files = Directory.GetFiles(sPathDossierChanson, sTitreChanson + "*" + sDifficultee + "*", SearchOption.AllDirectories);
            if (files.Count() > 0)
            {
                tslblStatut.Text = "La chanson " + sTitreChanson + " à été trouvée!";
                opfParcourirChanson.FileName = files.First();
                ChargerListe();
            }
            else
                tslblStatut.Text = "La chanson " + sTitreChanson + " est introuvable.";
        }
        private void bwRechercheFinChanson_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bwRechercheDebutChanson.RunWorkerAsync();
            bwRechercheFinChanson.Dispose();
        }

        private void bwRechercheFinChanson_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            if (Process.GetProcessesByName("osu!").Count() > 0)
                _processosu = Process.GetProcessesByName("osu!").First();

            while (Process.GetProcessesByName("osu!").Count() > 0 && _processosu.MainWindowTitle.Trim() != "osu!")
            {
                if ((worker.CancellationPending == true))
                {
                    e.Cancel = true;
                    break;
                }
                _processosu = Process.GetProcessesByName("osu!").First();
            }
        }
        #endregion

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

        #region GetWindowsBounds
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(IntPtr hWnd, out Rectangle rect);

        private void getosuWindowSize()
        {
            GetWindowRect(_processosu.MainWindowHandle
       , out _WindowsRect);
        }
        #endregion

        #region GetPixelColor

        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("user32.dll")]
        static extern Int32 ReleaseDC(IntPtr hwnd, IntPtr hdc);

        [DllImport("gdi32.dll")]
        static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);
        static public System.Drawing.Color GetPixelColor(int x, int y)
        {
            IntPtr hdc = GetDC(IntPtr.Zero);
            uint pixel = GetPixel(hdc, x, y);
            ReleaseDC(IntPtr.Zero, hdc);
            Color color = Color.FromArgb((int)(pixel & 0x000000FF),
                         (int)(pixel & 0x0000FF00) >> 8,
                         (int)(pixel & 0x00FF0000) >> 16);
            return color;
        }
        #endregion

        #region Events
        private void HandleHotkey()
        {
            if (bwBot.IsBusy)
            {
                _lPremiereNote = _lTempsDeChanson;
                bwBot.CancelAsync();
                this.Show();
            }
            else if (lblPathChanson.Text != "" && !bwRechercheProcessus.IsBusy)
            {
                //this.Hide();
                MouseHook.Start();
                MouseHook.MouseAction += new EventHandler(MouseEvent);
            }
            else if (bwRechercheProcessus.IsBusy)
                MessageBox.Show("Osu doit être démarré pour cela ! Il n'est actuellement pas détecté.");
        }

        private void MouseEvent(object sender, EventArgs e)
        {
            MouseHook.stop();
            _lTempsDepart1 = DateTime.Now.Ticks / 10000;
            if (_lTempsDeChanson > 0)
                bwBot.RunWorkerAsync();
            else
                DémarrerBot();
        }

        private void btnParcourir_Click(object sender, EventArgs e)
        {
            opfParcourirChanson.ShowDialog();
            sPathDossierChanson = opfParcourirChanson.FileName;
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
            _iNombreTP = _lstTimingPoints.Count();
            TrierListeOrdre();
        }
        private Event EventDeString(string sLigne)
        {
            string[] sEventParams = sLigne.Split(',');
            int[] iEventParams = new int[sEventParams.Count()];
            double dSliderLongueur=0;
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
                    double.TryParse(sEventParams[7].Replace('.', ','), out dSliderLongueur);
                    evenement = new Slider(iEventParams[0], iEventParams[1], iEventParams[2], (byte)iEventParams[6], dSliderLongueur);
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
            double dTempsParBattement = 0;
            int iCPT = 0;
            foreach (string param in sTimingPointParams)
            {
                if (iCPT == 1)
                    double.TryParse(sTimingPointParams[iCPT].Replace('.', ','), out dTempsParBattement);

                int.TryParse(sTimingPointParams[iCPT], out iTimingPointParams[iCPT++]);
            }
            TimingPoint TimingP = new TimingPoint(iTimingPointParams[0], dTempsParBattement, iTimingPointParams[2], iTimingPointParams[3], iTimingPointParams[5], iTimingPointParams[7]);
            return TimingP;
        }
        private void TrierListeOrdre()
        {
            _lstEvents.OrderBy(evenement => evenement.iTemps);
        }
        private void btnParcourirDossier_Click(object sender, EventArgs e)
        {
            opfParcourirDossierChanson.ShowDialog();
            lblPathChanson.Text = opfParcourirDossierChanson.SelectedPath;
        }
        private void DémarrerBot()
        {
            if (!bwBot.IsBusy)
            {
                _lTempsDeChanson = 0;
                _iIndexTP = 0;
                _iIndexEvent = 0;
                _lPremiereNote = _lstEvents.ElementAt(_iIndexEvent++).iTemps;
                _lMax = _lstEvents.Max(ev => ev.iTemps);
                _TempsParBattementBase = _lstTimingPoints.ElementAt(_iIndexTP++).TempsParBattement;
                _TempsParBattement = _TempsParBattementBase;
                bwBot.RunWorkerAsync();
            }
        }
    }
}
