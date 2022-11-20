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

            

            string timeNow = DateTime.Now.ToString("HH");
            if (Int32.Parse(timeNow) >=0 && Int32.Parse(timeNow) <= 17) // 0h -> 17h: Hen mo luc 19h
            {
                rbToday.Checked = true;
                cbTime.SelectedIndex = 19;
            }
            else // 17 -> 23: Hen mo luc 8h tomorrow
            {
                rbTomorrow.Checked = true;
                cbTime.SelectedIndex = 8;
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
                if (!cbTime.Text.Contains(":"))
                {
                    if (Int32.Parse(cbTime.Text) <= Int32.Parse(lbTime.Text))
                    {
                        MessageBox.Show("Time schedule is smaller than time now");
                        return;
                    }
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
            if (!timeSchedule.Contains(":"))
            {
                timeSchedule += ":00";
            }
            string time = "@echo off\nstart \"\" wosb.exe /run /systray dt=\" " + dateSchedule.ToShortDateString() + "\" tm=\"" + timeSchedule + "\"";
            // time += "rundll32.exe powrprof.dll,SetSuspendState 0,1,0";    // Sleep mode
            File.WriteAllText("schedule.bat", time);

            System.Diagnostics.Process.Start("schedule.bat");

            System.Threading.Thread.Sleep(2000);
            // Hibernate
            //Application.SetSuspendState(PowerState.Hibernate, true, true);
            // Standby

            // Windows + X   -> U -> S

            if (rbToday.Checked)
            {
                System.Diagnostics.Process.Start("sleep.exe");

            }
            else
            {
                System.Diagnostics.Process.Start("hibernate.exe");

            }

            Application.Exit();

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

        private void rbToday_CheckedChanged(object sender, EventArgs e)
        {
            if (rbToday.Checked)
            {
                cbTime.SelectedIndex = 19;
            }
            cbTime.Focus();
        }

        private void rbTomorrow_CheckedChanged(object sender, EventArgs e)
        {
            if (rbTomorrow.Checked)
            {
                cbTime.SelectedIndex = 8;
            }
            cbTime.Focus();

        }

        private void rb3_CheckedChanged(object sender, EventArgs e)
        {
            if (rb3.Checked)
            {
                cbTime.SelectedIndex = 8;
            }
            cbTime.Focus();

        }
    }
}
