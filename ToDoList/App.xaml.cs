using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Timers;

namespace ToDoList
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private System.Windows.Forms.NotifyIcon notifyicon;
        private bool exit;
        private bool exitNotify;
        private static Timer timer;
        private NotificationWindow nwindow;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Model.DbDataAccess.CreateTableIfNotExists();
            MainWindow = new MainWindow();
            nwindow = new NotificationWindow();

            MainWindow.Closing += MainWindow_Closing;
            nwindow.Closing += NotificationWindow_Closing;

            notifyicon = new System.Windows.Forms.NotifyIcon();
            notifyicon.DoubleClick += (s, args) => showMainWindow();
            notifyicon.Icon = ToDoList.Properties.Resources.appIcon;
            notifyicon.Visible = true;

            createContextMenu();
            timer = new Timer();
            timer.Interval = 120000;
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;
        }

        private void NotificationWindow_Closing(object sender, CancelEventArgs e)
        {
            if (!exitNotify)
            {
                e.Cancel = true;
                nwindow.Hide();
                timer.Enabled = true;
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke((Action)delegate ()
            {
                showNotificationWindow();
            });
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (!exit)
            {
                e.Cancel = true;
                MainWindow.Hide();
            }
        }

        private void createContextMenu()
        {
            notifyicon.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            notifyicon.ContextMenuStrip.Items.Add("ToDoList").Click += (s, e) => showMainWindow();
            notifyicon.ContextMenuStrip.Items.Add("Wyjdź").Click += (s, e) => exitApp();
        }

        private void exitApp()
        {
            exit = true;
            exitNotify = true;
            MainWindow.Close();
            nwindow.Close();
            notifyicon.Dispose();
            notifyicon = null;
        }

        private void showMainWindow()
        {
            if (MainWindow.IsVisible)
            {
                if (MainWindow.WindowState == WindowState.Minimized)
                {
                    MainWindow.WindowState = WindowState.Normal;
                }
                MainWindow.Activate();
            }
            else
            {
                MainWindow.Show();
            }
        }

        private void showNotificationWindow()
        {
            if (nwindow.IsVisible)
            {
                if (nwindow.WindowState == WindowState.Minimized)
                {
                    nwindow.WindowState = WindowState.Normal;
                }
                nwindow.Activate();
            }
            else
            {
                if (Model.DbDataAccess.LoadTasksFromDB(DateTime.Today) != null && !nwindow.IsVisible)
                {
                    nwindow.LoadTasksToDo(DateTime.Today);
                    nwindow.Show();
                    timer.Enabled = false;
                }
            }

        }
    }
}
