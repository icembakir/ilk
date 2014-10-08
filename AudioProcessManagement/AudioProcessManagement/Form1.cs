using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace AudioProcessManagement
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;

            this.timer.Tick += new EventHandler(controlProcess); // Everytime timer ticks, timer_Tick will be called
            this.timer.Interval = 5000;              // Timer will tick evert second
            this.timer.Enabled = true;                       // Enable the timer
            this.timer.Start();
        }

        void controlProcess(object sender, EventArgs e)
        {
            Process[] pname = Process.GetProcessesByName("AudioInputOutputManager");
            if (pname.Length == 0)
            {
                //AudioInputOutputManager seems not working, re-execute it
                Process.Start("C:\\TelemedAudio\\AudioInputOutputManager.exe");
            }
        }

        private Timer timer = new Timer();

    }
}
