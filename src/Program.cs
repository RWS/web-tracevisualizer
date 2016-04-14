//using System;
//using System.Windows.Forms;
//using Tridion.ContentManager.TracingClient;
//using Tridion.Logging;

//namespace SdlWeb_TraceVisualizer
//{
//    static class Program
//    {
//        /// <summary>
//        /// The main entry point for the application.
//        /// </summary>
//        [STAThread]
//        static void Main(string[] args)
//        {
//            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
//            Application.EnableVisualStyles();
//            Application.SetCompatibleTextRenderingDefault(false);

//            Application.ApplicationExit += StopListeners;
//            Application.ThreadException += StopListeners;
//            AppDomain.CurrentDomain.UnhandledException += StopListeners;

//            Application.Run(new MonitorPerformanceDetail(ArgumentParser.Parse(args) ?? new TraceInstruction { TraceKeywords = TraceKeywords.All }));
//        }

//        static void StopListeners(object sender, EventArgs e)
//        {
//            EventListenerManager.Instance.StopListening();
//        }
//    }
//}
