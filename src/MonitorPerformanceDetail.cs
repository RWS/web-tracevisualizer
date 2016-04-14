using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Sdl_Web.TraceVisualizer;
using Tridion.Logging;

namespace SdlWeb_TraceVisualizer
{
    internal partial class MonitorPerformanceDetail : Form
    {
        private readonly List<StructuredLogEntry> _allEventRoots = new List<StructuredLogEntry>();
        private readonly Dictionary<string, List<StructuredLogEntry>> _eventsByOperation = new Dictionary<string, List<StructuredLogEntry>>();
        private Dictionary<string, OperationSummary> _operationSummaries = new Dictionary<string, OperationSummary>();
        private readonly TraceInstruction _instruction;
        private IObservable<EntryBase> _source;
        private SynchronizationContext _synchronizationContext;
        private bool _layoutSuspended;

        private class OperationSummary
        {
            public long Count { get; set; }
            public TimeSpan TotalDuration { get; set; }
        }

        private string _currentSearchPattern;

        public MonitorPerformanceDetail(TraceInstruction instruction)
        {
            _synchronizationContext = SynchronizationContext.Current;
            _instruction = instruction;
            InitializeComponent();
            InitializeTreeView();
        }

        private void SearchInTree(StructuredLogEntry logEntry)
        {
            if (logEntry.Parent == null)
            {
                var firstNode =
                    _eventsByOperation.FirstOrDefault(root => root.Key == logEntry.OperationName && root.Value.Any(sle => sle.Start.OperationId == logEntry.Start.OperationId));
                if (!firstNode.Equals(default(KeyValuePair<string, List<StructuredLogEntry>>)))
                {
                    lstCallDetails.Focus();
                    lstCallDetails.SelectObject(firstNode, true);
                    lstCallDetails.Expand(firstNode);

                    var rootActivityNode = firstNode.Value.FirstOrDefault(sle => sle.Start.OperationId == logEntry.Start.OperationId);
                    lstCallDetails.SelectObject(rootActivityNode, true);
                    lstCallDetails.Expand(rootActivityNode);
                    lstCallDetails.SelectedItem.EnsureVisible();
                }
            }
            else
            {
                SearchInTree(logEntry.Parent);
                lstCallDetails.SelectObject(logEntry, true);
                lstCallDetails.Expand(logEntry);
            }
        }

        private void InitializeTreeView()
        {
            lstCallDetails.CanExpandGetter += delegate(object model)
            {
                StructuredLogEntry entry = model as StructuredLogEntry;
                if (entry != null && (entry.NestedCalls.Count + entry.Info.Count) > 0)
                {
                    return true;
                }
                if (model is KeyValuePair<string, List<StructuredLogEntry>>)
                {
                    KeyValuePair<string, List<StructuredLogEntry>> entryPair = (KeyValuePair<string, List<StructuredLogEntry>>)model;
                    return entryPair.Value != null && entryPair.Value.Any();
                }
                return false;
            };
            lstCallDetails.ChildrenGetter += delegate(object model)
            {
                StructuredLogEntry entry = model as StructuredLogEntry;
                if (entry != null && (entry.NestedCalls.Count + entry.Info.Count) > 0)
                {
                    return
                        entry.NestedCalls.Concat<object>(entry.Info)
                             .OrderBy(item => item is EntryBase ? ((EntryBase)item).UtcTimeStamp : ((StructuredLogEntry)item).Start.UtcTimeStamp)
                             .ToArray();
                }
                if (model is KeyValuePair<string, List<StructuredLogEntry>>)
                {
                    KeyValuePair<string, List<StructuredLogEntry>> entryPair = (KeyValuePair<string, List<StructuredLogEntry>>)model;
                    if (entryPair.Value != null && entryPair.Value.Any())
                    {
                        return entryPair.Value;
                    }
                }
                return Enumerable.Empty<StructuredLogEntry>();
            };
            olvColumn_Operation.AspectGetter += delegate(object rowObject)
            {
                StructuredLogEntry entry = rowObject as StructuredLogEntry;
                if (entry != null)
                {
                    string signature = entry.ParameterValueDetailed;
                    return string.Format("{0}({1})", entry.OperationName, signature);
                }
                LogEntry logEntry = rowObject as LogEntry;
                if (logEntry != null)
                {
                    return logEntry.Message;
                }
                if (rowObject is KeyValuePair<string, List<StructuredLogEntry>>)
                {
                    KeyValuePair<string, List<StructuredLogEntry>> entryPair = (KeyValuePair<string, List<StructuredLogEntry>>)rowObject;
                    if (entryPair.Value != null && entryPair.Value.Any())
                    {
                        return entryPair.Key;
                    }
                }
                return string.Empty;
            };
            olvColumn_TimeTaken.AspectGetter += delegate(object rowObject)
            {
                StructuredLogEntry entry = rowObject as StructuredLogEntry;
                if (entry != null && entry.Stop != null)
                {
                    return string.Format("{0:F6} ms", entry.Stop.TimeInterval.TotalMilliseconds);
                }
                if (rowObject is KeyValuePair<string, List<StructuredLogEntry>>)
                {
                    KeyValuePair<string, List<StructuredLogEntry>> entryPair = (KeyValuePair<string, List<StructuredLogEntry>>)rowObject;
                    StructuredLogEntry[] entryPairWithStop = entryPair.Value != null && entryPair.Value.Any()
                                                                 ? entryPair.Value.Where(e => e.Stop != null).ToArray()
                                                                 : new StructuredLogEntry[0];
                    if (entryPairWithStop.Any())
                    {
                        return string.Format("{0:F2} ms", entryPairWithStop.Sum(e => e.Stop.TimeInterval.TotalMilliseconds) / entryPairWithStop.Length);
                    }
                }
                return "...";
            };
            olvColumn_NestedCallerCount.AspectGetter += delegate(object rowObject)
            {
                StructuredLogEntry entry = rowObject as StructuredLogEntry;
                if (entry != null)
                {
                    return entry.NestedCalls.Count.ToString();
                }
                if (rowObject is KeyValuePair<string, List<StructuredLogEntry>>)
                {
                    return ((KeyValuePair<string, List<StructuredLogEntry>>)rowObject).Value.Count;
                }
                return string.Empty;
            };
            olvColumn_InvocationTime.AspectGetter += delegate(object rowObject)
            {
                StructuredLogEntry entry = rowObject as StructuredLogEntry;
                if (entry != null && entry.Stop != null)
                {
                    return entry.Stop.UtcTimeStamp.ToString();
                }
                return string.Empty;
            };
            lstCallDetails.CellToolTipShowing += LstCallDetails_CellToolTipShowing;
            timer1.Start();
        }

        private void LstCallDetails_CellToolTipShowing(object sender, BrightIdeasSoftware.ToolTipShowingEventArgs e)
        {
            StructuredLogEntry entry = e.Model as StructuredLogEntry;
            if (entry != null)
            {
                StringBuilder sb = new StringBuilder();
                if (entry.Start != null)
                {
                    sb.Append("Channel: ");
                    sb.AppendLine(entry.Start.Channel.ToString());
                    sb.Append("Keywords: ");
                    sb.AppendLine(entry.Start.Keywords.ToString());
                    sb.Append("Depth: ");
                    sb.AppendLine(entry.Start.Depth.ToString());
                    sb.Append("Operation: ");
                    sb.AppendLine(entry.Start.OperationId.ToString());
                    sb.Append("ActivityId: ");
                    sb.AppendLine(entry.Start.ActivityId.ToString());
                    sb.Append("RelatedActivityId: ");
                    sb.AppendLine(entry.Start.RelatedActivityId.ToString());
                    string[] paramTypes = entry.Start.MethodSignature.Split(',');
                    string[] paramValues = entry.Start.MethodParameters.Split(new[] { "#END#" }, StringSplitOptions.None);
                    for (int i = 0; i < paramTypes.Length; i++)
                    {
                        sb.AppendLine(paramTypes[i] + (i < paramValues.Length ? ": " + paramValues[i] : String.Empty));
                    }
                }
                e.Text = sb.ToString();
            }
        }

        private void MonitorPerformanceDetail_Load(object sender, EventArgs e)
        {
            Reset();
            StartListeningToCmEvents();
        }

        private void Reset()
        {
            _operationSummaries = new Dictionary<string, OperationSummary>();
            chart_Performance.Series.Clear();
            chart_AverageTime.Series.Clear();
        }

        private void StartListeningToCmEvents()
        {
            EnableTsmiConfiguration(true);

            if (string.IsNullOrEmpty(_instruction.LogPath))
            {
                _source = EventListenerManager.Instance.RegisterObservableEventSource(new List<TraceChannels>(_instruction.TraceChannels), Process.GetCurrentProcess().Id,
                    _instruction.TraceKeywords, _instruction.TraceLevel);
            }
            else
            {
                //@"C:\PerfLogs\DataCollector01.etl"
                SuspendAllLayout();
                _source = EventListenerManager.Instance.RegisterObservableEventSource(_instruction.LogPath, new List<TraceChannels>(_instruction.TraceChannels),
                    _instruction.TraceKeywords, _instruction.TraceLevel);
            }

            if (_instruction.ProcessNames != null && _instruction.ProcessNames.Any())
            {
                _source = _source.Where(entry => _instruction.ProcessNames.Contains(entry.ProcessName));
            }

            _source.Where(IsNotIgnored).Subscribe(AllOperationHandler, OnError, OnCompleted);

            _source.OfType<TraceEntryBase>()
                   .Where(entry => entry.OpCode == TraceOpCodes.Stop && entry.Keywords.HasFlag(TraceKeywords.Public) && IsNotIgnored(entry))
                   .ObserveOn(this)
                   .Subscribe(PerfEventOccurred);
        }

        private void SuspendAllLayout()
        {
            _layoutSuspended = true;
            SuspendLayout();
            chart_Performance.SuspendLayout();
            chart_AverageTime.SuspendLayout();
            lstCallDetails.SuspendLayout();
        }

        private void ResumeAllLayout()
        {
            _layoutSuspended = false;
            lstCallDetails.ResumeLayout();
            chart_AverageTime.ResumeLayout();
            chart_Performance.ResumeLayout();
            ResumeLayout();
        }

        private void OnError(Exception obj)
        {
            ResumeAllLayout();
            MessageBox.Show(obj.ToString(), "Attention!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            _synchronizationContext.Post(d => EnableTsmiConfiguration(false), null);
        }

        private void OnCompleted()
        {
            ResumeAllLayout();
            MessageBox.Show(@"Completed reading trace info.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            _synchronizationContext.Post(d => EnableTsmiConfiguration(false), null);
        }

        private void EnableTsmiConfiguration(bool isRunning)
        {
            tsmiStop.Enabled = isRunning;
            tsmiSearch.Enabled = !isRunning;
        }

        private bool IsNotIgnored(EntryBase entry)
        {
            //return true;
            string[] ignoredMembers = { "Dispose", "ImpersonateWithToken", "Impersonate", ".ctor" };
            TraceEntryBase entryBase = entry as TraceEntryBase;
            if (entryBase != null)
            {
                return !ignoredMembers.Contains(entryBase.MethodName);
            }
            return true;
        }

        private void AllOperationHandler(EntryBase entry)
        {
            Guid operationIdGuid = entry.OperationId;

            TraceEntryBase traceEntry = entry as TraceEntryBase;
            int expectedDepth = entry.OpCode == TraceOpCodes.Start ? entry.Depth - 1 : entry.Depth;
            StructuredLogEntry tree = _allEventRoots.SingleOrDefault(t => t.Start != null && t.Start.OperationId == operationIdGuid);
            // TODO: Build tree when unwinding a call stack.
            if (tree == null)
            {
                if (traceEntry != null)
                {
                    if (!traceEntry.Keywords.HasFlag(TraceKeywords.Public))
                    {
                        return;
                    }
                    tree = new StructuredLogEntry(traceEntry);
                    _allEventRoots.Add(tree);
                    if (!_eventsByOperation.ContainsKey(tree.OperationName))
                    {
                        _eventsByOperation[tree.OperationName] = new List<StructuredLogEntry>();
                    }
                    _eventsByOperation[tree.OperationName].Add(tree);
                }
            }
            else
            {
                Guid expectedActivityId = entry.OpCode == TraceOpCodes.Start ? entry.RelatedActivityId : entry.ActivityId;

                LogEntry logEntry = entry as LogEntry;
                StructuredLogEntry unfinishedNode = GetUnfinishedNode(tree, expectedDepth, expectedActivityId);
                if (unfinishedNode == null)
                {
                    return;
                }
                if (traceEntry != null)
                {
                    if (entry.OpCode == TraceOpCodes.Start)
                    {
                        StructuredLogEntry node = new StructuredLogEntry(traceEntry, unfinishedNode);
                        unfinishedNode.NestedCalls.Add(node);
                    }
                    else if (entry.OpCode == TraceOpCodes.Stop)
                    {
                        unfinishedNode.Stop = traceEntry;
                    }
                }
                else if (logEntry != null)
                {
                    unfinishedNode.Info.Add(logEntry);
                }
            }
        }

        private void WriteToFile()
        {
            StringBuilder sb = new StringBuilder();
            foreach (StructuredLogEntry tree in _allEventRoots)
            {
                TraverseAndPrint(tree, sb);
            }
        }

        private void TraverseAndPrint(StructuredLogEntry tree, StringBuilder sb)
        {
            if (tree.Start != null)
            {
                // TODO
                //tree.Start.OriginalTraceEvent.ToXml(sb);
            }
            if (tree.NestedCalls != null && tree.NestedCalls.Any())
            {
                foreach (StructuredLogEntry entry in tree.NestedCalls)
                {
                    TraverseAndPrint(entry, sb);
                }
            }
            if (tree.Stop != null)
            {
                // TODO
                //tree.Stop.OriginalTraceEvent.ToXml(sb);
            }
        }

        private StructuredLogEntry GetUnfinishedNode(StructuredLogEntry currentNode, int expectedDepth, Guid activityId)
        {
            if (currentNode.Start.Depth > expectedDepth)
            {
                return null;
            }

            if (currentNode.Start.ActivityId == activityId)
            {
                return currentNode;
            }

            StructuredLogEntry bestMatch = null;
            foreach (StructuredLogEntry children in currentNode.NestedCalls)
            {
                StructuredLogEntry unfinishedNode = GetUnfinishedNode(children, expectedDepth, activityId);
                if (unfinishedNode != null)
                {
                    if (unfinishedNode.Start.ActivityId == activityId)
                    {
                        return unfinishedNode;
                    }
                    bestMatch = unfinishedNode;
                }
            }

            return bestMatch ?? currentNode;
        }

        private void PerfEventOccurred(TraceEntryBase entry)
        {
            if (!_layoutSuspended)
            {
                chart_Performance.SuspendLayout();
                chart_AverageTime.SuspendLayout();
            }

            string declaringType = entry.DeclaringType.Split('.').LastOrDefault() ?? string.Empty;
            string operationName = declaringType + "." + entry.MethodName;
            if (!_operationSummaries.ContainsKey(operationName))
            {
                OperationSummary summary = new OperationSummary { Count = 0, TotalDuration = new TimeSpan() };
                _operationSummaries[operationName] = summary;

                chart_Performance.Series.Add(operationName);
                chart_AverageTime.Series.Add(operationName);
            }

            _operationSummaries[operationName].Count++;
            _operationSummaries[operationName].TotalDuration += entry.TimeInterval;
            TimeSpan averageTimeSpan = TimeSpan.FromTicks(_operationSummaries[operationName].TotalDuration.Ticks / _operationSummaries[operationName].Count);

            chart_Performance.Series[operationName].Points.Clear();
            chart_Performance.Series[operationName].Points.AddY(_operationSummaries[operationName].Count);
            chart_AverageTime.Series[operationName].Points.Clear();
            chart_AverageTime.Series[operationName].Points.AddY(averageTimeSpan.TotalSeconds);

            if (!_layoutSuspended)
            {
                chart_AverageTime.ResumeLayout();
                chart_Performance.ResumeLayout();
            }
        }

        private void MonitorPerformanceDetail_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_source != null)
            {
                EventListenerManager.Instance.UnregisterObservableEventSource(_source);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!_layoutSuspended)
            {
                lstCallDetails.SuspendLayout();
            }

            if (operationToolStripMenuItem.Checked)
            {
                lstCallDetails.Roots = _eventsByOperation;
            }
            else
            {
                lstCallDetails.Roots = _allEventRoots;
            }
            lstCallDetails.BuildList(true);

            if (!_layoutSuspended)
            {
                lstCallDetails.ResumeLayout();
            }
        }

        private StructuredLogEntry SearchFor(string pattern, StructuredLogEntry rootLevelEntry, StructuredLogEntry startFromEntry, ref bool startEntryReached)
        {
            if (!startEntryReached && rootLevelEntry == startFromEntry)
            {
                startEntryReached = true;
            }
            else if (startEntryReached && rootLevelEntry.OperationName.ToLowerInvariant().Contains(pattern.ToLowerInvariant()))
            {
                return rootLevelEntry;
            }

            foreach (var item in rootLevelEntry.NestedCalls)
            {
                var result = SearchFor(pattern, item, startFromEntry, ref startEntryReached);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        private void MonitorPerformanceDetail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F && tsmiSearch.Enabled)
            {
                PerformSearch();

                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.S && tsmiStop.Enabled)
            {
                EventListenerManager.Instance.StopListening();
            }
            else if (tsmiSearch.Enabled && e.KeyCode == Keys.F3 && !string.IsNullOrEmpty(_currentSearchPattern))
            {
                SearchAndSelectItem(_currentSearchPattern);
            }
            else if (e.Alt)
            {
                mnsMainMenu.Visible = ! mnsMainMenu.Visible;
            }
        }

        private void PerformSearch()
        {
            string pattern = _currentSearchPattern;
            if (DialogResult.OK == ShowInputDialog(ref pattern) && !string.IsNullOrWhiteSpace(pattern))
            {
                _currentSearchPattern = pattern;
                SearchAndSelectItem(pattern);
            }
        }

        private void SearchAndSelectItem(string pattern)
        {
            StructuredLogEntry selectedEntry = null;
            if (lstCallDetails.SelectedItem != null)
            {
                selectedEntry = lstCallDetails.SelectedItem.RowObject as StructuredLogEntry;
            }

            bool searchEntryReached = selectedEntry == null;
            foreach (StructuredLogEntry root in _eventsByOperation.Values.SelectMany(c => c))
            {
                var result = SearchFor(pattern, root, selectedEntry, ref searchEntryReached);
                if (result != null)
                {
                    SearchInTree(result);
                    break;
                }
            }
        }

        private static DialogResult ShowInputDialog(ref string input)
        {
            System.Drawing.Size size = new System.Drawing.Size(200, 70);
            Form inputBox = new Form();

            inputBox.FormBorderStyle = FormBorderStyle.FixedDialog;
            inputBox.ClientSize = size;
            inputBox.Text = "Pattern";

            TextBox textBox = new TextBox();
            textBox.Size = new System.Drawing.Size(size.Width - 10, 23);
            textBox.Location = new System.Drawing.Point(5, 5);
            textBox.Text = input;
            inputBox.Controls.Add(textBox);

            Button okButton = new Button();
            okButton.DialogResult = DialogResult.OK;
            okButton.Name = "okButton";
            okButton.Size = new System.Drawing.Size(75, 23);
            okButton.Text = "&OK";
            okButton.Location = new System.Drawing.Point(size.Width - 80 - 80, 39);
            inputBox.Controls.Add(okButton);

            Button cancelButton = new Button();
            cancelButton.DialogResult = DialogResult.Cancel;
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(75, 23);
            cancelButton.Text = "&Cancel";
            cancelButton.Location = new System.Drawing.Point(size.Width - 80, 39);
            inputBox.Controls.Add(cancelButton);

            inputBox.AcceptButton = okButton;
            inputBox.CancelButton = cancelButton;

            inputBox.StartPosition = FormStartPosition.CenterParent;
            DialogResult result = inputBox.ShowDialog();
            input = textBox.Text;
            return result;
        }

        private void tsmiStop_Click(object sender, EventArgs e)
        {
            EventListenerManager.Instance.StopListening();
        }

        private void tsmiSearch_Click(object sender, EventArgs e)
        {
            PerformSearch();
        }

        private void operationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            operationToolStripMenuItem.Checked = true;
            methodToolStripMenuItem.Checked = false;
        }

        private void methodToolStripMenuItem_Click(object sender, EventArgs e)
        {
            operationToolStripMenuItem.Checked = false;
            methodToolStripMenuItem.Checked = true;
        }
    }
}
