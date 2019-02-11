using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Configuration;

namespace SetTimeAndDate
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            updateTimeAndDate();
            syncTimer.Interval = 1000;
            syncTimer.Enabled = true;
            loadPreviousSettings();
        }

        private void syncTimer_Tick(object sender, EventArgs e)
        {
            updateTimeAndDate();
        }

        private void updateTimeAndDate()
        {
            lblCurrentDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            lblCurrentTime.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void setNewSettings()
        {
            RegistrySettings.Write("Date", dtPickerTarget.Value.ToString("yyyy-MM-dd"));
            RegistrySettings.Write("Time", txtTargetTime.Text);
        }

        private void loadPreviousSettings()
        {
            CultureInfo provider = CultureInfo.InvariantCulture;

            try
            {
                string previousDate = RegistrySettings.Read("Date");
                if (!string.IsNullOrEmpty(previousDate))
                    dtPickerTarget.Value = DateTime.ParseExact(previousDate, "yyyy-MM-dd", provider);
                if (!string.IsNullOrEmpty(RegistrySettings.Read("Time")))
                    txtTargetTime.Text = RegistrySettings.Read("Time");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSetDate_Click(object sender, EventArgs e)
        {
            DateTime setDate = dtPickerTarget.Value.Date;
            CultureInfo provider = CultureInfo.InvariantCulture;

            try
            {
                DateTime targetTime = DateTime.ParseExact(txtTargetTime.Text, "HH:mm:ss", provider);
                setDate = setDate.AddHours(targetTime.Hour);
                setDate = setDate.AddMinutes(targetTime.Minute);
                setDate = setDate.AddSeconds(targetTime.Second);

                TimeAndDate.SYSTEMTIME sysTime = new TimeAndDate.SYSTEMTIME();
                sysTime.FromDateTime(setDate);
                TimeAndDate.SetLocalTime(ref sysTime);

                setNewSettings();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnResetDate_Click(object sender, EventArgs e)
        {
            try
            {
                string hostName = "";
                int cnt = 1;
                while (hostName != null)
                {
                    try
                    {
                        hostName = ConfigurationManager.AppSettings["server" + cnt];
                        DateTime atomicTime = TimeAndDate.GetAtomicTime(hostName);

                        //UTC offset and daylight saving time?
                        atomicTime = TimeZone.CurrentTimeZone.ToLocalTime(atomicTime);

                        TimeAndDate.SYSTEMTIME sysTime = new TimeAndDate.SYSTEMTIME();
                        sysTime.FromDateTime(atomicTime);
                        TimeAndDate.SetLocalTime(ref sysTime);
                        MessageBox.Show("Date reset!");
                        return;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        
                    }
                    finally
                    {
                        cnt++;
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
