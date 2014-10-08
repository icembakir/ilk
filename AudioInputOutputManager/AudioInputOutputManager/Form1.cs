using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using NAudio.Wave;
using NAudio.CoreAudioApi;
using NAudio.Mixer;
using NAudio.Midi;
using System.IO;

namespace AudioInputOutputManager
{
    public partial class Form1 : Form
    {
        delegate void SetMicrophoneValueCallback(double value);

        delegate void SetLineinValueCallback(double value);

<<<<<<< HEAD:AudioInputOutputManager/AudioInputOutputManager/MainForm.cs
        KeyObserver myObserver = new KeyObserver(); // composition
<<<<<<< HEAD:AudioInputOutputManager/AudioInputOutputManager/MainForm.cs


=======
        
       
        
>>>>>>> origin/master:AudioInputOutputManager/AudioInputOutputManager/MainForm.cs

        public MainForm()
=======
        public Form1()
>>>>>>> parent of 9f5708f... Commit:AudioInputOutputManager/AudioInputOutputManager/Form1.cs
        {
            Environment.CurrentDirectory = Path.GetDirectoryName(Application.ExecutablePath);
            
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
<<<<<<< HEAD:AudioInputOutputManager/AudioInputOutputManager/MainForm.cs
            this.KeyPreview = true;
<<<<<<< HEAD:AudioInputOutputManager/AudioInputOutputManager/MainForm.cs

            InitializeComponent();


=======
            
            InitializeComponent();

            
>>>>>>> origin/master:AudioInputOutputManager/AudioInputOutputManager/MainForm.cs

=======
            
            InitializeComponent();

>>>>>>> parent of 9f5708f... Commit:AudioInputOutputManager/AudioInputOutputManager/Form1.cs
            this.microphoneChart.ChartAreas[0].AxisX.Minimum = 0;
            this.microphoneChart.ChartAreas[0].AxisX.Maximum = 200;

            this.microphoneChart.ChartAreas[0].AxisY.Interval = 10;

            this.microphoneChart.ChartAreas[0].AxisY.Minimum = -100;
            this.microphoneChart.ChartAreas[0].AxisY.Maximum = 0;

            this.lineinChart.ChartAreas[0].AxisX.Minimum = 0;
            this.lineinChart.ChartAreas[0].AxisX.Maximum = 200;

            this.lineinChart.ChartAreas[0].AxisY.Interval = 10;

            this.lineinChart.ChartAreas[0].AxisY.Minimum = -100;
            this.lineinChart.ChartAreas[0].AxisY.Maximum = 0;
            
            this.Hide();
            this.Visible = false;

            this.ForceClosing = false;

            eventLog = new EventLog("EventLog.log");

            //KeyboardHook.GetInstance().AddListener('R', this);

            this.audioInputThread = new Thread(new ThreadStart(AudioInputThread));
            try
            {
                this.audioInputThread.Start();
            }
            catch (Exception ex)
            {
                eventLog.WriteEntry(String.Format("{0} {1}",ex.Message,ex.StackTrace));
                this.audioInputThread.Join(1000);
            }
<<<<<<< HEAD:AudioInputOutputManager/AudioInputOutputManager/MainForm.cs
        } // form constructor ends
<<<<<<< HEAD:AudioInputOutputManager/AudioInputOutputManager/MainForm.cs
=======



>>>>>>> origin/master:AudioInputOutputManager/AudioInputOutputManager/MainForm.cs
=======
        }
>>>>>>> parent of 9f5708f... Commit:AudioInputOutputManager/AudioInputOutputManager/Form1.cs

        private void AudioInputThread()
        {
            eventLog.WriteEntry("AudioInputOutputManager application is started.");
            while (true)
            {
                DetectInputDevices(null);
                Thread.Sleep(5000);
            }
        }

        private void DetectInputDevices(object obj)
        {
            StringBuilder log = new StringBuilder();
            int waveInDevices = WaveIn.DeviceCount;
            bool tempMicPluggedin = false, tempLineinPluggedin = false;
            short tempDeviceStatus = 0x00;

            WaveInEvent tempMicWaveIn, tempLineinWaveIn;

            log.Append("*******STARTING TO COUNT INPUT DEVICES*******\r\n");
            log.Append("*******DETECTED INPUT DEVICES*******\r\n");
            log.Append(String.Format("Input Device Count: {0}\r\n", waveInDevices));

            if (waveInDevices == 0)
            {
                log.Append("NO INPUT AUDIO DEVICE IS DETECTED.\r\n");
            }

            for (int waveInDevice = 0; waveInDevice < waveInDevices; waveInDevice++)
            {
                WaveInCapabilities deviceInfo = WaveIn.GetCapabilities(waveInDevice);
                string productName = deviceInfo.ProductName;
                log.Append(String.Format("Device {0}: {1}, {2} channels\r\n", waveInDevice, productName, deviceInfo.Channels));
                productName = productName.ToLower();

                if (productName.StartsWith("mikrofon") || productName.StartsWith("microphone"))
                {
                    tempMicPluggedin = true;

                    tempDeviceStatus |= 0x01;

                    if (microphonePluggedin)
                        continue;

                    log.Append("Microphone is plugged in.\r\n");

                    tempMicWaveIn = new WaveInEvent();

                    try
                    {
                        tempMicWaveIn.DeviceNumber = waveInDevice;
                        tempMicWaveIn.WaveFormat = new WaveFormat(8000, 1);
                        tempMicWaveIn.DataAvailable += Mic_DataAvailable;
                        tempMicWaveIn.StartRecording();
                        this.micWaveIn = tempMicWaveIn;
                    }
                    catch (NAudio.MmException ex)
                    {
                        eventLog.WriteEntry("Error on microphone: " + ex.Message);
                        tempMicWaveIn.Dispose();
                    }
                }
                else if (productName.StartsWith("hat girişi") || productName.StartsWith("line ın") || productName.StartsWith("line in") || productName.StartsWith("line-in") || productName.StartsWith("line-ın"))
                {
                    tempLineinPluggedin = true;

                    tempDeviceStatus |= 0x02;

                    if (lineinPluggedin)
                        continue;

                    log.Append("Line-in is plugged in.\r\n");

                    tempLineinWaveIn = new WaveInEvent();

                    try
                    {
                        tempLineinWaveIn.DeviceNumber = waveInDevice;
                        tempLineinWaveIn.WaveFormat = new WaveFormat(8000, 1);
                        tempLineinWaveIn.DataAvailable += LineIn_DataAvailable;
                        tempLineinWaveIn.StartRecording();
                        this.lineinWaveIn = tempLineinWaveIn;
                    }
                    catch (NAudio.MmException ex)
                    {
                        eventLog.WriteEntry("Error on line-in: " + ex.Message);
                        tempLineinWaveIn.Dispose();
                    }
                }
            }

            if (microphonePluggedin && !tempMicPluggedin)
            {
                //micWaveIn.StopRecording();
                setApplicationsActive();
                micIsInUse = false;

                this.micWaveIn.Dispose();
                log.Append("Microphone is plugged out.\r\n");
            }

            if (lineinPluggedin && !tempLineinPluggedin)
            {
                //lineinWaveIn.StopRecording();
                setApplicationsActive();
                setMicrophoneActive();
                lineinIsInUse = false;
                micPermitted = true;
                this.lineinWaveIn.Dispose();
                log.Append("Linein is plugged out.\r\n");
            }

            microphonePluggedin = tempMicPluggedin;
            lineinPluggedin = tempLineinPluggedin;

            log.Append("*******END*******");

            if (this.deviceStatus != tempDeviceStatus)
            {
                this.deviceStatus = tempDeviceStatus;
                eventLog.WriteEntry(log.ToString());
            }
        }

        private void Mic_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (!micPermitted) return;

            TimeSpan elapsedTime = DateTime.Now - startTime;
            TimeSpan oneSecondPassed = DateTime.Now - micTimeHoldingOneSecond;

            double sum = 0;
            for (int index = 0; index < e.BytesRecorded; index += 2)
            {
                double sample = BitConverter.ToInt16(e.Buffer, index) / 32768.0;
                sum += (sample * sample);
            }

            double rms = Math.Sqrt(sum / (e.BytesRecorded / 2));
            var decibel = 20 * Math.Log10(rms);

            this.SetMicrophoneValue(decibel);

            if (Math.Ceiling(elapsedTime.TotalSeconds) >= WAIT_TIME_FOR_MICROPHONE_TO_APPLICATION)
            {
                setApplicationsActive();
            }

            micIsUsedIn3Seconds |= decibel > THRESHOLD_FOR_MICROPHONE;

            if (micIsInUse && oneSecondPassed.TotalSeconds >= 1f)
            {
                if (!micIsUsedIn3Seconds)
                {
                    micIsInUse = false;
                }
                else
                {
                    setApplicationsMute();
                }
                startTime = DateTime.Now;
                micIsUsedIn3Seconds = false;
            }
            else if (!micIsInUse && decibel > THRESHOLD_FOR_MICROPHONE)
            {
                eventLog.WriteEntry("Microphone is enabled.");
                micIsInUse = true;
                micIsUsedIn3Seconds = false;
                bool muted = setApplicationsMute();
                startTime = DateTime.Now;
            }

            if (oneSecondPassed.TotalSeconds >= 1f)
                micTimeHoldingOneSecond = DateTime.Now;
        }

        private void LineIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            //if (micPermitted)
            //  startTime = DateTime.Now;

            TimeSpan elapsedTime = DateTime.Now - startTime;
            TimeSpan oneSecondPassed = DateTime.Now - lineinTimeHoldingOneSecond;

            double sum = 0;
            for (int index = 0; index < e.BytesRecorded; index += 2)
            {
                double sample = BitConverter.ToInt16(e.Buffer, index) / 32768.0;
                sum += (sample * sample);
            }

            double rms = Math.Sqrt(sum / (e.BytesRecorded / 2));
            var decibel = 20 * Math.Log10(rms);

            this.SetLineinValue(decibel);

            if (elapsedTime.TotalSeconds >= WAIT_TIME_FOR_LINEIN_TO_APPLICATION)
            {
                micPermitted = true;
                setApplicationsActive();
                setMicrophoneActive();
            }
            else if (elapsedTime.TotalSeconds >= WAIT_TIME_FOR_LINEIN_TO_MICROPHONE)
            {
                setMicrophoneActive();
            }

            lineinIsUsedIn3Seconds |= decibel > THRESHOLD_FOR_LINEIN;

            if (oneSecondPassed.TotalSeconds >= 1f)
                lineinTimeHoldingOneSecond = DateTime.Now;

            if (lineinIsInUse && oneSecondPassed.TotalSeconds >= 1.0f)
            {
                if (!lineinIsUsedIn3Seconds)
                {
                    lineinIsInUse = false;
                    //setMicrophoneActive();
                }
                else
                {
                    setApplicationsMute();
                    setMicrophoneMute();
                }

                startTime = DateTime.Now;
                lineinIsUsedIn3Seconds = false;
            }
            else if (!lineinIsInUse && decibel > THRESHOLD_FOR_LINEIN)
            {
                eventLog.WriteEntry("Line-in is enabled.");
                lineinIsInUse = true;
                micPermitted = false;
                micIsInUse = false;
                lineinIsUsedIn3Seconds = false;
                setApplicationsMute();
                setMicrophoneMute();
                startTime = DateTime.Now;
            }
        }

        private void SetMicrophoneValue(double value)
        {
            if (this.microphoneChart.InvokeRequired)
            {
                try
                {
                    SetMicrophoneValueCallback c = new SetMicrophoneValueCallback(SetMicrophoneValue);
                    this.Invoke(c, new object[] { value });
                }
                catch (ObjectDisposedException ex)
                {
                }
            }
            else
            {
                try
                {
                    if (this.microphoneChart.Series["microphone"].Points.Count == 200)
                    {
                        this.microphoneChart.Series["microphone"].Points.RemoveAt(0);
                    }
                    this.microphoneChart.Series["microphone"].Points.AddY(value);
                }
                catch (NullReferenceException ex)
                {
                }
            }
        }

        private void SetLineinValue(double value)
        {
            if (this.lineinChart.InvokeRequired)
            {
                try
                {
                    SetLineinValueCallback c = new SetLineinValueCallback(SetLineinValue);
                    this.Invoke(c, new object[] { value });
                }
                catch (ObjectDisposedException ex)
                {
                }
            }
            else
            {
                try
                {
                    if (this.lineinChart.Series["linein"].Points.Count == 200)
                    {
                        this.lineinChart.Series["linein"].Points.RemoveAt(0);
                    }
                    this.lineinChart.Series["linein"].Points.AddY(value);
                }
                catch (NullReferenceException ex)
                {
                }
            }
        }

<<<<<<< HEAD:AudioInputOutputManager/AudioInputOutputManager/MainForm.cs
<<<<<<< HEAD:AudioInputOutputManager/AudioInputOutputManager/MainForm.cs
        //private static double deger = 5; //OO
        static List<double> micDegerListesi = new List<double>();
        static List<double> lineInDegerListesi = new List<double>();

=======
        
>>>>>>> origin/master:AudioInputOutputManager/AudioInputOutputManager/MainForm.cs
=======
>>>>>>> parent of 9f5708f... Commit:AudioInputOutputManager/AudioInputOutputManager/Form1.cs
        private DateTime firstKeyEventClickedTime = DateTime.Now;

        private short deviceStatus = 0x00;

        private bool microphonePluggedin = false;
        private bool lineinPluggedin = false;

        private WaveInEvent micWaveIn = null;
        private WaveInEvent lineinWaveIn = null;

        private bool micIsInUse = false;
        private bool micIsUsedIn3Seconds = false;
        private bool micPermitted = true;
        //private DateTime micStartTime = DateTime.Now;
        private DateTime micTimeHoldingOneSecond = DateTime.Now;

        private DateTime startTime = DateTime.Now;

        private bool lineinIsInUse = false;
        private bool lineinIsUsedIn3Seconds = false;
        //private DateTime lineinStartTime = DateTime.Now;
        private DateTime lineinTimeHoldingOneSecond = DateTime.Now;
        private EventLog eventLog;

        private Thread audioInputThread;

        private bool ForceClosing { get; set; }

        private static double WAIT_TIME_FOR_MICROPHONE_TO_APPLICATION = Convert.ToDouble(INI.GetInstance().GetValue("WAIT_TIME_FOR_MICROPHONE_TO_APPLICATION"));
        private static double WAIT_TIME_FOR_LINEIN_TO_APPLICATION = Convert.ToDouble(INI.GetInstance().GetValue("WAIT_TIME_FOR_LINEIN_TO_APPLICATION"));
        private static double WAIT_TIME_FOR_LINEIN_TO_MICROPHONE = Convert.ToDouble(INI.GetInstance().GetValue("WAIT_TIME_FOR_LINEIN_TO_MICROPHONE"));
        private static double THRESHOLD_FOR_MICROPHONE = Convert.ToDouble(INI.GetInstance().GetValue("THRESHOLD_FOR_MICROPHONE"));
        private static double THRESHOLD_FOR_LINEIN = Convert.ToDouble(INI.GetInstance().GetValue("THRESHOLD_FOR_LINEIN"));

<<<<<<< HEAD:AudioInputOutputManager/AudioInputOutputManager/MainForm.cs
<<<<<<< HEAD:AudioInputOutputManager/AudioInputOutputManager/MainForm.cs
=======
        //private static char INPUT_KEY = Convert.ToChar(INI.GetInstance().GetValue("INPUT_KEY"));
        
>>>>>>> origin/master:AudioInputOutputManager/AudioInputOutputManager/MainForm.cs
=======
        private static double LOWER_THRESHOLD_FOR_VOLUMEUP = Convert.ToDouble(INI.GetInstance().GetValue("LOWER_THRESHOLD_FOR_VOLUMEUP"));
        private static double UPPER_THRESHOLD_FOR_VOLUMEUP = Convert.ToDouble(INI.GetInstance().GetValue("UPPER_THRESHOLD_FOR_VOLUMEUP"));
        private static double LOWER_THRESHOLD_FOR_VOLUMEDOWN = Convert.ToDouble(INI.GetInstance().GetValue("LOWER_THRESHOLD_FOR_VOLUMEDOWN"));
        private static double UPPER_THRESHOLD_FOR_VOLUMEDOWN = Convert.ToDouble(INI.GetInstance().GetValue("UPPER_THRESHOLD_FOR_VOLUMEDOWN"));
>>>>>>> parent of 9f5708f... Commit:AudioInputOutputManager/AudioInputOutputManager/Form1.cs

        [DllImport("AudioSessionManager.dll")]
        public static extern Boolean setApplicationsMute(); //makes all audio sessions, except microphone, mute.

        [DllImport("AudioSessionManager.dll")]
        public static extern bool setApplicationsActive(); //makes all audio session (except Microphone) active again

        [DllImport("AudioSessionManager.dll")]
        public static extern bool setMicrophoneActive(); //makes microphone audio session active again

        [DllImport("AudioSessionManager.dll")]
        public static extern bool setMicrophoneMute(); // makes microphone audio session mute.

        [DllImport("AudioSessionManager.dll")]
        public static extern bool volumeUp(); // makes microphone audio session mute.

        [DllImport("AudioSessionManager.dll")]
        public static extern bool volumeDown();

<<<<<<< HEAD:AudioInputOutputManager/AudioInputOutputManager/MainForm.cs
<<<<<<< HEAD:AudioInputOutputManager/AudioInputOutputManager/MainForm.cs

=======
        
>>>>>>> origin/master:AudioInputOutputManager/AudioInputOutputManager/MainForm.cs

=======
>>>>>>> parent of 9f5708f... Commit:AudioInputOutputManager/AudioInputOutputManager/Form1.cs
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.Visible)
            {
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
                this.Hide();
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
                this.ShowInTaskbar = true;
                this.Show();
            }
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.ShowInTaskbar = true;
            this.Show();
        }

        private void hideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            this.Hide();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ForceClosing = true;
            this.audioInputThread.Abort();
            this.Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.ForceClosing)
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
                this.Hide();
            }
        }

<<<<<<< HEAD:AudioInputOutputManager/AudioInputOutputManager/MainForm.cs
        private void startButton_Click(object sender, EventArgs e)
        {
            //TODO: Start recording
            if (this.lineinWaveIn != null)
            {
                this.savesWaveToFile = !this.savesWaveToFile;
                if (this.savesWaveToFile == true)
                {
                    this.lineinWaveFileWriter = new WaveFileWriter("linein.wav", this.lineinWaveIn.WaveFormat);
                    this.startButton.Text = "Stop";
                }
                else
                {
                    this.lineinWaveFileWriter.Dispose();
                    this.lineinWaveFileWriter = null;
                    this.startButton.Text = "Start";
                }
            }
        }

<<<<<<< HEAD:AudioInputOutputManager/AudioInputOutputManager/MainForm.cs

        private void MainForm_Load(object sender, EventArgs e)
        {

            textBoxBaselineForLineIn.Text = medyanValueForLineIn.ToString();
            textBoxBaselineForMic.Text = medyanValueForMic.ToString();

            
        }


        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {

            textBox1.Text = e.KeyCode.ToString();
            textBox2.Text = e.KeyCode.ToString("X");
            textBox3.Text = myObserver.KeyPressed('a').ToString(); // Key pressed herhangi bir char değişkeni alabilir.
        }




        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = checkBox1.Checked;

            if (isChecked)
            {
                textBoxBaselineForLineIn.Text = Math.Round(medyanValueForLineIn,2).ToString();
                textBoxBaselineForMic.Text = Math.Round(medyanValueForMic, 2).ToString();

                textBoxBaselineForLineIn.ReadOnly = true;
                textBoxBaselineForMic.ReadOnly = true;
                //saveButton.Visible = false;

            }

            else
            {
                textBoxBaselineForLineIn.ReadOnly = false;
                textBoxBaselineForMic.ReadOnly = false;
                //saveButton.Visible = true;

                
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            double i;

            if (!double.TryParse(textBoxBaselineForLineIn.Text, out i))
            {
                MessageBox.Show("BASELINE_FOR_LINEIN için sayısal bir değer giriniz");
                return;

            }

            if (!double.TryParse(textBoxBaselineForMic.Text, out i))
            {
                MessageBox.Show("BASELINE_FOR_MICROPHONE için sayısal bir değer giriniz");
                return;

            }

             

            

            if (checkBox1.Checked == false) // medyanvalue'lar(Baseline) için atama checkbox false iken textboxlar'dan yapılsın.
            {
                medyanValueForMic = Convert.ToDouble(textBoxBaselineForMic.Text);
                medyanValueForLineIn = Convert.ToDouble(textBoxBaselineForLineIn.Text);
                
            }

            
            MessageBox.Show("Başarıyla kaydedildi.");
            
        }







=======
>>>>>>> parent of 9f5708f... Commit:AudioInputOutputManager/AudioInputOutputManager/Form1.cs
    }
=======
             

       private void MainForm_KeyDown(object sender, KeyEventArgs e)
       {

           textBox1.Text = e.KeyCode.ToString();
           textBox2.Text = e.KeyCode.ToString("X");
           textBox3.Text = myObserver.KeyPressed('a').ToString(); // Key pressed herhangi bir char değişkeni alabilir.

       }
        
       
    } 
    

>>>>>>> origin/master:AudioInputOutputManager/AudioInputOutputManager/MainForm.cs
}
