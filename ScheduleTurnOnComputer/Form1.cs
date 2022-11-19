using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScheduleTurnOnComputer
{
    public partial class Form1 : Form
    {


        Timer t = new Timer();
        public Form1()
        {
            InitializeComponent();
            t.Interval = 1000;
            t.Tick += new EventHandler(this.timer1_Tick);
            t.Start();

            
            cbTime.SelectedIndex = 19;
            rbTomorrow.Checked = true;

            string timeNow = DateTime.Now.ToString("HH");
            if (Int32.Parse(timeNow) >=0 && Int32.Parse(timeNow) <= 19)
            {
                rbToday.Checked = true;
            }
        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            lbTime.Text = DateTime.Now.ToString("HH");
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void startSchedule()
        {
            DateTime dateSchedule = DateTime.Today.AddDays(1).Date;
            if (rbToday.Checked)
            {
                dateSchedule = DateTime.Today.Date;
                if (Int32.Parse(cbTime.Text) <= Int32.Parse(lbTime.Text))
                {
                    MessageBox.Show("Time schedule is smaller than time now");
                    return;
                }
            }
            else if (rbTomorrow.Checked)
            {
                dateSchedule = DateTime.Today.AddDays(1).Date;
            }
            else if (rb3.Checked)
            {
                dateSchedule = DateTime.Today.AddDays(2).Date;
            }
            String timeSchedule = cbTime.Text;
            string time = "@echo off\nstart \"\" wosb.exe /run /systray dt=\" " + dateSchedule.ToShortDateString() + "\" tm=\"" + timeSchedule + ":00" + "\"\ntimeout 3\n";
            time += "powercfg -h off\nrundll32.exe powrprof.dll,SetSuspendState 0,1,0\npowercfg - h on";    // Sleep mode
            File.WriteAllText("schedule.bat", time);

            //System.Diagnostics.Process.Start("schedule.bat");
        }

        private void btnTurnOff_Click(object sender, EventArgs e)
        {
            startSchedule();
        }

        private void cbTime_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                startSchedule();
            }
        }
    }
}
