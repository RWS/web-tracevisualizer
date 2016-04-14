namespace SdlWeb_TraceVisualizer
{
    partial class MonitorPerformanceDetail
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title2 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.chart_Performance = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chart_AverageTime = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.lstCallDetails = new BrightIdeasSoftware.TreeListView();
            this.olvColumn_Operation = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn_TimeTaken = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn_NestedCallerCount = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn_InvocationTime = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.mnsMainMenu = new System.Windows.Forms.MenuStrip();
            this.tsmiStop = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSearch = new System.Windows.Forms.ToolStripMenuItem();
            this.groupByToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.operationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.methodToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.chart_Performance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_AverageTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lstCallDetails)).BeginInit();
            this.mnsMainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // chart_Performance
            // 
            chartArea1.Name = "ChartArea1";
            this.chart_Performance.ChartAreas.Add(chartArea1);
            this.chart_Performance.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chart_Performance.Legends.Add(legend1);
            this.chart_Performance.Location = new System.Drawing.Point(0, 0);
            this.chart_Performance.Margin = new System.Windows.Forms.Padding(4);
            this.chart_Performance.Name = "chart_Performance";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart_Performance.Series.Add(series1);
            this.chart_Performance.Size = new System.Drawing.Size(716, 387);
            this.chart_Performance.TabIndex = 0;
            title1.Name = "CM Public Operation";
            title1.Text = "CM Public Operation";
            this.chart_Performance.Titles.Add(title1);
            // 
            // chart_AverageTime
            // 
            chartArea2.Name = "ChartArea1";
            this.chart_AverageTime.ChartAreas.Add(chartArea2);
            this.chart_AverageTime.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.Name = "Legend1";
            this.chart_AverageTime.Legends.Add(legend2);
            this.chart_AverageTime.Location = new System.Drawing.Point(0, 0);
            this.chart_AverageTime.Margin = new System.Windows.Forms.Padding(4);
            this.chart_AverageTime.Name = "chart_AverageTime";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chart_AverageTime.Series.Add(series2);
            this.chart_AverageTime.Size = new System.Drawing.Size(748, 387);
            this.chart_AverageTime.TabIndex = 1;
            title2.Name = "Title1";
            title2.Text = "CM Average Time/Operation";
            this.chart_AverageTime.Titles.Add(title2);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.chart_Performance);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.chart_AverageTime);
            this.splitContainer1.Size = new System.Drawing.Size(1469, 387);
            this.splitContainer1.SplitterDistance = 716;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 2;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 28);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.lstCallDetails);
            this.splitContainer2.Size = new System.Drawing.Size(1469, 774);
            this.splitContainer2.SplitterDistance = 387;
            this.splitContainer2.TabIndex = 4;
            // 
            // lstCallDetails
            // 
            this.lstCallDetails.AllColumns.Add(this.olvColumn_Operation);
            this.lstCallDetails.AllColumns.Add(this.olvColumn_TimeTaken);
            this.lstCallDetails.AllColumns.Add(this.olvColumn_NestedCallerCount);
            this.lstCallDetails.AllColumns.Add(this.olvColumn_InvocationTime);
            this.lstCallDetails.CellEditUseWholeCell = false;
            this.lstCallDetails.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn_Operation,
            this.olvColumn_TimeTaken,
            this.olvColumn_NestedCallerCount,
            this.olvColumn_InvocationTime});
            this.lstCallDetails.Cursor = System.Windows.Forms.Cursors.Default;
            this.lstCallDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstCallDetails.FullRowSelect = true;
            this.lstCallDetails.SelectedBackColor = System.Drawing.Color.Empty;
            this.lstCallDetails.SelectedBackColor = System.Drawing.Color.Empty;
            this.lstCallDetails.Location = new System.Drawing.Point(0, 0);
            this.lstCallDetails.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lstCallDetails.MultiSelect = false;
            this.lstCallDetails.Name = "lstCallDetails";
            this.lstCallDetails.ShowGroups = false;
            this.lstCallDetails.Size = new System.Drawing.Size(1469, 383);
            this.lstCallDetails.TabIndex = 0;
            this.lstCallDetails.UseCompatibleStateImageBehavior = false;
            this.lstCallDetails.View = System.Windows.Forms.View.Details;
            this.lstCallDetails.VirtualMode = true;
            // 
            // olvColumn_Operation
            // 
            this.olvColumn_Operation.AspectName = "";
            this.olvColumn_Operation.MinimumWidth = 300;
            this.olvColumn_Operation.Text = "Operation";
            this.olvColumn_Operation.Width = 713;
            // 
            // olvColumn_TimeTaken
            // 
            this.olvColumn_TimeTaken.Text = "Time (ms)";
            this.olvColumn_TimeTaken.Width = 148;
            // 
            // olvColumn_NestedCallerCount
            // 
            this.olvColumn_NestedCallerCount.Text = "Nester Callers";
            this.olvColumn_NestedCallerCount.Width = 99;
            // 
            // olvColumn_InvocationTime
            // 
            this.olvColumn_InvocationTime.Text = "Invocation Time";
            this.olvColumn_InvocationTime.Width = 190;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // mnsMainMenu
            // 
            this.mnsMainMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mnsMainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiStop,
            this.tsmiSearch,
            this.groupByToolStripMenuItem});
            this.mnsMainMenu.Location = new System.Drawing.Point(0, 0);
            this.mnsMainMenu.Name = "mnsMainMenu";
            this.mnsMainMenu.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.mnsMainMenu.Size = new System.Drawing.Size(1469, 28);
            this.mnsMainMenu.TabIndex = 1;
            this.mnsMainMenu.Text = "mnsMain";
            this.mnsMainMenu.Visible = false;
            // 
            // tsmiStop
            // 
            this.tsmiStop.Name = "tsmiStop";
            this.tsmiStop.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.tsmiStop.Size = new System.Drawing.Size(52, 24);
            this.tsmiStop.Text = "Stop";
            this.tsmiStop.ToolTipText = "Stop";
            this.tsmiStop.Click += new System.EventHandler(this.tsmiStop_Click);
            // 
            // tsmiSearch
            // 
            this.tsmiSearch.Name = "tsmiSearch";
            this.tsmiSearch.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.tsmiSearch.Size = new System.Drawing.Size(65, 24);
            this.tsmiSearch.Text = "Search";
            this.tsmiSearch.Click += new System.EventHandler(this.tsmiSearch_Click);
            // 
            // groupByToolStripMenuItem
            // 
            this.groupByToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.operationToolStripMenuItem,
            this.methodToolStripMenuItem});
            this.groupByToolStripMenuItem.Name = "groupByToolStripMenuItem";
            this.groupByToolStripMenuItem.Size = new System.Drawing.Size(82, 24);
            this.groupByToolStripMenuItem.Text = "Group By";
            // 
            // operationToolStripMenuItem
            // 
            this.operationToolStripMenuItem.Checked = true;
            this.operationToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.operationToolStripMenuItem.Name = "operationToolStripMenuItem";
            this.operationToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.operationToolStripMenuItem.Text = "Operation";
            this.operationToolStripMenuItem.Click += new System.EventHandler(this.operationToolStripMenuItem_Click);
            // 
            // methodToolStripMenuItem
            // 
            this.methodToolStripMenuItem.Name = "methodToolStripMenuItem";
            this.methodToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.methodToolStripMenuItem.Text = "Method";
            this.methodToolStripMenuItem.Click += new System.EventHandler(this.methodToolStripMenuItem_Click);
            // 
            // MonitorPerformanceDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1469, 802);
            this.Controls.Add(this.splitContainer2);
            this.Controls.Add(this.mnsMainMenu);
            this.KeyPreview = true;
            this.MainMenuStrip = this.mnsMainMenu;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "MonitorPerformanceDetail";
            this.Text = "MonitorPerformance";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MonitorPerformanceDetail_FormClosing);
            this.Load += new System.EventHandler(this.MonitorPerformanceDetail_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MonitorPerformanceDetail_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.chart_Performance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_AverageTime)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lstCallDetails)).EndInit();
            this.mnsMainMenu.ResumeLayout(false);
            this.mnsMainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart_Performance;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_AverageTime;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private BrightIdeasSoftware.TreeListView lstCallDetails;
        private System.Windows.Forms.Timer timer1;
        private BrightIdeasSoftware.OLVColumn olvColumn_Operation;
        private BrightIdeasSoftware.OLVColumn olvColumn_TimeTaken;
        private BrightIdeasSoftware.OLVColumn olvColumn_NestedCallerCount;
        private BrightIdeasSoftware.OLVColumn olvColumn_InvocationTime;
        private System.Windows.Forms.MenuStrip mnsMainMenu;
        private System.Windows.Forms.ToolStripMenuItem tsmiStop;
        private System.Windows.Forms.ToolStripMenuItem tsmiSearch;
        private System.Windows.Forms.ToolStripMenuItem groupByToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem operationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem methodToolStripMenuItem;
    }
}