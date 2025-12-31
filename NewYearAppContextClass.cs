using System;
using System.Drawing;
using System.Windows.Forms;

namespace CSharpNewYearCountdown {
    public class NewYearAppContext : ApplicationContext
    {
        private NotifyIcon trayIcon;

        public NewYearAppContext()
        {
            // Create NotifyIcon
            trayIcon = new NotifyIcon()
            {
                Icon = Properties.Resources.SiteIcon,
                ContextMenuStrip = new ContextMenuStrip(),
                Visible = true,
                Text = "New Year Countdown"
            };

            // Start timer
            NewYearAlarm alarm = new NewYearAlarm();

            // Add the Exit menu item
            trayIcon.ContextMenuStrip.Items.Add("E&xit", null, (s, e) => {
                ExitApplication();
            });

            // Left-click shall close the app
            trayIcon.MouseDoubleClick += new MouseEventHandler(OnDoubleClick);

            // Handle the application exit event
            Application.ApplicationExit += new EventHandler(OnApplicationExit);
        }

        private void OnApplicationExit(object sender, EventArgs e)
        {
            if (trayIcon != null)
            {
                trayIcon.Dispose();
            }
        }

        private void OnDoubleClick(object sender, EventArgs e) {
            OnApplicationExit(sender, e);
            ExitApplication();
        }

        private void ExitApplication()
        {
            trayIcon.Visible = false;
            Application.Exit();
        }
    }
}