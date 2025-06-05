using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using Clearcove.Logging;
using StoryMaker.Helpers;
using StoryMaker.Models;
using StoryMaker.Models.Utility;
using Timeline.Helper;

namespace StoryMaker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Logger _logger = new Logger(typeof(App));

        public App()
        {
//            Test();

            DispatcherUnhandledException += App_DispatcherUnhandledException;
            FrameworkCompatibilityPreferences.KeepTextBoxDisplaySynchronizedWithTextProperty = false;

            var logPath = Paths.DocumentPath + "/Logs.txt";
            Logger.LogToConsole = true;
            Logger.Start(new FileInfo(logPath));
        }

        void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Utils.ShowWarningMessage(e.Exception.Message);
#if DEBUG

            _logger.Log(Logger.ERROR, "error", e.Exception);

            // var lastLog = "";
            // if (File.Exists(logPath))
            //     lastLog = File.ReadAllText(logPath);
            //
            // File.WriteAllText(logPath, lastLog + GetAllExceptions(e.Exception));
#endif
            e.Handled = true;
        }

        public static string GetAllExceptions(Exception ex)
        {
            var x = 0;
            var pattern = "EXCEPTION #{0}:\r\n{1}";
            var message = $"\n Time : {DateTime.Now.ToString(CultureInfo.InvariantCulture)}" +
                          $"\n message : {string.Format(pattern, ++x, ex.Message)}" +
                          $"\n Help Link : {ex.HelpLink}" +
                          $"\n Data : {ex.Data}" +
                          $"\n HResult : {ex.HResult}" +
                          $"\n Source : {ex.Source}" +
                          $"\n StackTrace : {ex.StackTrace}" +
                          $"\n Target Site : {ex.TargetSite}";
            var inner = ex.InnerException;
            while (inner != null)
            {
                message += "\r\n============\r\n" + string.Format(pattern, ++x, inner.Message);
                inner = inner.InnerException;
            }

            return $"{message}\n-------------------------------------------------------------------\n";
        }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            var softwareModel = Core.Save_Load.SoftwareData.Load(Paths.DocumentPath);

            //initialize the splash screen and set it as the application main window
            var splashScreen = new Views.SplashScreen(softwareModel);
            var mainWindow = new Views.MainWindow(softwareModel);
            MainWindow = splashScreen;
            splashScreen.Show();

            //in order to ensure the UI stays responsive, we need to
            //do the work on a different thread
            Task.Factory.StartNew(() =>
            {
                //simulate some work being done
                System.Threading.Thread.Sleep(1000);

                //since we're not on the UI thread
                //once we're done we need to use the Dispatcher
                //to create and show the main window
                Current.Dispatcher?.Invoke(() =>
                {
                    //initialize the main window, set it as the application main window
                    //and close the splash screen
                    MainWindow = mainWindow;
                    mainWindow.Show();
                    splashScreen.Close();
                });
            });
        }
    }
}