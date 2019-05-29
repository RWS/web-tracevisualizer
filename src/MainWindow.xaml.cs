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
            InitializeData();
        }

        private void InitializeData()
        {
            foreach (string channelName in Enum.GetNames(typeof(TraceChannels)))
            {
                TraceChannelMenuItem.Items.Add(new MenuItem
                {
                    IsCheckable = true,
                    Header = channelName,
                    Name = channelName,
                    IsChecked = true
                });
            }
            // TODO: Move all at the end with a separator
            foreach (string keyword in Enum.GetNames(typeof(TraceKeywords)))
            {
                KeywordsMenuItem.Items.Add(new MenuItem
                {
                    IsCheckable = true,
                    Header = keyword,
                    Name = keyword,
                    IsChecked = true
                });
            }

            foreach (string level in Enum.GetNames(typeof(TraceLevels)))
            {
                TraceLevelMenuItem.Items.Add(new MenuItem
                {
                    IsCheckable = true,
                    Header = level,
                    Name = level,
                    IsChecked = level == "Verbose"
                });
            }
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
                    Enum.TryParse(subMenuItem.Name, out TraceChannels channel);
                    traceChannels.Add(channel);
                }
            }
            TraceKeywords keywords = 0;
            foreach (var item in KeywordsMenuItem.Items)
            {
                MenuItem subMenuItem = item as MenuItem;
                if (subMenuItem != null && subMenuItem.IsChecked)
                {
                    Enum.TryParse(subMenuItem.Name, out TraceKeywords keyword);
                    keywords |= keyword;
                }
            }
            TraceLevels levels = 0;
            foreach (var item in TraceLevelMenuItem.Items)
            {
                MenuItem subMenuItem = item as MenuItem;
                if (subMenuItem != null && subMenuItem.IsChecked)
                {
                    Enum.TryParse(subMenuItem.Name, out TraceLevels traceLevel);
                    levels = traceLevel;
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
