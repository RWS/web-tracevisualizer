using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace SdlWeb_TraceVisualizer
{
    /// <summary>
    /// Interaction logic for ProcessList.xaml
    /// </summary>
    public partial class ProcessList : Window
    {
        public ProcessList()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ProcessList_OnLoaded(object sender, RoutedEventArgs e)
        {
            string []processeNames = Process.GetProcesses().Select(p => p.ProcessName).OrderBy(p => p).Distinct().ToArray();
            foreach (string processName in processeNames)
            {
                ProcessListBox.Items.Add(processName);
            }
        }
    }
}
