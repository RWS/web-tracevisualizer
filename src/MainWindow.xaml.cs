using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Sdl_Web.TraceVisualizer;
using Tridion.Logging;

namespace SdlWeb_TraceVisualizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void NewSession_OnClick(object sender, RoutedEventArgs e)
        {
            TraceInstruction instruction = getTraceInstruction();
            LoadPerfMonitor(instruction);
        }

        private void OpenSession_OnClick(object sender, RoutedEventArgs e)
        {
            var ofd = new Microsoft.Win32.OpenFileDialog() { Filter = "ETL Files (*.etl)|*.etl|All Files (*.*)|*.*" };
            ofd.Multiselect = false;
            var result = ofd.ShowDialog();
            if (result == false) return;
            TraceInstruction instruction = getTraceInstruction(ofd.FileName);
            LoadPerfMonitor(instruction);
        }

        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private TraceInstruction getTraceInstruction(string filePath = null)
        {
            List<TraceChannels> traceChannels = new List<TraceChannels>();
            foreach (var item in TraceChannelMenuItem.Items)
            {
                MenuItem subMenuItem = item as MenuItem;
                if (subMenuItem != null && subMenuItem.IsChecked)
                {
                    switch (subMenuItem.Name)
                    {
                        case "DefaultChannel":
                            traceChannels.Add(TraceChannels.DefaultChannel);
                            break;
                        case "TcmChannel":
                            traceChannels.Add(TraceChannels.TcmChannel);
                            break;
                        case "TtmChannel":
                            traceChannels.Add(TraceChannels.TtmChannel);
                            break;
                    }
                }
            }
            TraceKeywords keywords = 0;
            foreach (var item in KeywordsMenuItem.Items)
            {
                MenuItem subMenuItem = item as MenuItem;
                if (subMenuItem != null && subMenuItem.IsChecked)
                {
                    switch (subMenuItem.Name)
                    {
                        case "PublicMenuItem":
                            keywords |= TraceKeywords.Public;
                            break;
                        case "PublicIndirectMenuItem":
                            keywords |= TraceKeywords.PublicIndirect;
                            break;
                        case "InternalMenuItem":
                            keywords |= TraceKeywords.Internal;
                            break;
                        case "DatabaseMenuItem":
                            keywords |= TraceKeywords.Database;
                            break;
                        case "ExtensionMenuItem":
                            keywords |= TraceKeywords.Extension;
                            break;
                        case "ExternalMenuItem":
                            keywords |= TraceKeywords.External;
                            break;
                    }
                }
            }
            TraceLevels levels = 0;
            foreach (var item in TraceLevelMenuItem.Items)
            {
                MenuItem subMenuItem = item as MenuItem;
                if (subMenuItem != null && subMenuItem.IsChecked)
                {
                    switch (subMenuItem.Name)
                    {
                        case "CriticaMenuItem":
                            levels = TraceLevels.Critical;
                            break;
                        case "ErrorMenuItem":
                            levels = TraceLevels.Error;
                            break;
                        case "WarningMenuItem":
                            levels = TraceLevels.Warning;
                            break;
                        case "InformationalMenuItem":
                            levels = TraceLevels.Informational;
                            break;
                        case "VerboseMenuItem":
                            levels = TraceLevels.Verbose;
                            break;
                    }
                }
            }
            List<string> processNames = null;
            if (ProcessFilterMenuItem.Items.Count > 3)
            {
                processNames = new List<string>();
                for (int i = 3; i < ProcessFilterMenuItem.Items.Count; i++)
                {
                    processNames.Add((string)((MenuItem)ProcessFilterMenuItem.Items[i]).Header);
                }
            }

            return new TraceInstruction { LogPath = filePath, TraceChannels = traceChannels, TraceKeywords = keywords, TraceLevel = levels, ProcessNames = processNames };
        }

        private void LoadPerfMonitor(TraceInstruction instruction)
        {
            MonitorPerformanceDetail form = new MonitorPerformanceDetail(instruction);
            //WindowInteropHelper wih = new WindowInteropHelper(this) { Owner = form.Handle };
            form.Show();
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            EventListenerManager.Instance.StopListening();
        }

        private void Keyword_OnClick(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = e.Source as MenuItem;
            if (menuItem == null || menuItem.Name == "KeywordsMenuItem")
            {
                return;
            }

            if (menuItem.Name == "AllKeywordsMenuItem")
            {
                foreach (var item in KeywordsMenuItem.Items)
                {
                    MenuItem subMenuItem = item as MenuItem;
                    if (subMenuItem != null)
                    {
                        subMenuItem.IsChecked = true;
                        subMenuItem.IsEnabled = true;
                    }
                }
                AllKeywordsMenuItem.IsEnabled = false;
            }
            else
            {
                AllKeywordsMenuItem.IsEnabled = true;
                AllKeywordsMenuItem.IsChecked = false;
            }

        }

        private void TraceLevel_OnClick(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = e.Source as MenuItem;
            if (menuItem == null || menuItem.Name == "TraceLevelMenuItem")
            {
                return;
            }

            foreach (var item in TraceLevelMenuItem.Items)
            {
                MenuItem subMenuItem = item as MenuItem;
                if (subMenuItem != null)
                {
                    subMenuItem.IsChecked = false;
                }
            }

            menuItem.IsChecked = true;
        }

        private void AddProcessFilter_OnClick(object sender, RoutedEventArgs e)
        {
            ProcessList list = new ProcessList();
            if (list.ShowDialog().GetValueOrDefault() == true)
            {
                foreach (var process in list.ProcessListBox.SelectedItems)
                {
                    string processName = process.ToString();
                    if (GetProcessMenuItem(processName) == null)
                    {
                        MenuItem item = new MenuItem();
                        item.Header = processName;
                        item.Click += (o, args) => ProcessFilterMenuItem.Items.Remove(args.Source);
                        ProcessFilterMenuItem.Items.Add(item);
                    }
                }
            }
        }

        private MenuItem GetProcessMenuItem(string processName)
        {
            foreach (var item in ProcessFilterMenuItem.Items)
            {
                MenuItem menuItem = item as MenuItem;
                if (menuItem != null && (string)menuItem.Header == processName)
                {
                    return menuItem;
                }
            }

            return null;
        }

        private void ClearProcessFilter_OnClick(object sender, RoutedEventArgs e)
        {
            while (ProcessFilterMenuItem.Items.Count > 3)
            {
                ProcessFilterMenuItem.Items.RemoveAt(3);
            }
        }
    }
}
