namespace TRoschinsky.Service.HomeMaticNotification
{
    partial class NotifyItemForm
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBoxDefST = new System.Windows.Forms.GroupBox();
            this.listViewDefST = new System.Windows.Forms.ListView();
            this.ColHeadNotifyItem = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBoxSimResult = new System.Windows.Forms.GroupBox();
            this.richTextBoxSimResult = new System.Windows.Forms.RichTextBox();
            this.buttonSimulate = new System.Windows.Forms.Button();
            this.numericUpDownSteps = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownIntervall = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dateTimePickerTo = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerFrom = new System.Windows.Forms.DateTimePicker();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBoxDefST.SuspendLayout();
            this.groupBoxSimResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSteps)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownIntervall)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(441, 261);
            this.tabControl1.TabIndex = 9;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.propertyGrid1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(417, 229);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Details of NotifyItem";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(3, 3);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(411, 223);
            this.propertyGrid1.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.splitContainer1);
            this.tabPage2.Controls.Add(this.buttonSimulate);
            this.tabPage2.Controls.Add(this.numericUpDownSteps);
            this.tabPage2.Controls.Add(this.numericUpDownIntervall);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.dateTimePickerTo);
            this.tabPage2.Controls.Add(this.dateTimePickerFrom);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(433, 235);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "SilenceTimes-Tester";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(5, 61);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBoxDefST);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBoxSimResult);
            this.splitContainer1.Size = new System.Drawing.Size(424, 170);
            this.splitContainer1.SplitterDistance = 202;
            this.splitContainer1.TabIndex = 18;
            // 
            // groupBoxDefST
            // 
            this.groupBoxDefST.Controls.Add(this.listViewDefST);
            this.groupBoxDefST.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxDefST.Location = new System.Drawing.Point(0, 0);
            this.groupBoxDefST.Name = "groupBoxDefST";
            this.groupBoxDefST.Size = new System.Drawing.Size(202, 170);
            this.groupBoxDefST.TabIndex = 1;
            this.groupBoxDefST.TabStop = false;
            this.groupBoxDefST.Text = "Defined SilenceTimes";
            // 
            // listViewDefST
            // 
            this.listViewDefST.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColHeadNotifyItem});
            this.listViewDefST.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewDefST.Location = new System.Drawing.Point(3, 16);
            this.listViewDefST.Name = "listViewDefST";
            this.listViewDefST.Size = new System.Drawing.Size(196, 151);
            this.listViewDefST.TabIndex = 0;
            this.listViewDefST.UseCompatibleStateImageBehavior = false;
            this.listViewDefST.View = System.Windows.Forms.View.Details;
            // 
            // ColHeadNotifyItem
            // 
            this.ColHeadNotifyItem.Text = "NotifyItem";
            this.ColHeadNotifyItem.Width = 230;
            // 
            // groupBoxSimResult
            // 
            this.groupBoxSimResult.Controls.Add(this.richTextBoxSimResult);
            this.groupBoxSimResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxSimResult.Location = new System.Drawing.Point(0, 0);
            this.groupBoxSimResult.Name = "groupBoxSimResult";
            this.groupBoxSimResult.Size = new System.Drawing.Size(218, 170);
            this.groupBoxSimResult.TabIndex = 2;
            this.groupBoxSimResult.TabStop = false;
            this.groupBoxSimResult.Text = "Simulation result";
            // 
            // richTextBoxSimResult
            // 
            this.richTextBoxSimResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxSimResult.Font = new System.Drawing.Font("Courier New", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxSimResult.Location = new System.Drawing.Point(3, 16);
            this.richTextBoxSimResult.Name = "richTextBoxSimResult";
            this.richTextBoxSimResult.Size = new System.Drawing.Size(212, 151);
            this.richTextBoxSimResult.TabIndex = 0;
            this.richTextBoxSimResult.Text = "";
            this.richTextBoxSimResult.WordWrap = false;
            // 
            // buttonSimulate
            // 
            this.buttonSimulate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSimulate.Location = new System.Drawing.Point(369, 9);
            this.buttonSimulate.Name = "buttonSimulate";
            this.buttonSimulate.Size = new System.Drawing.Size(59, 46);
            this.buttonSimulate.TabIndex = 17;
            this.buttonSimulate.Text = "Simulate";
            this.buttonSimulate.UseVisualStyleBackColor = true;
            this.buttonSimulate.Click += new System.EventHandler(this.buttonSimulate_Click);
            // 
            // numericUpDownSteps
            // 
            this.numericUpDownSteps.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownSteps.Location = new System.Drawing.Point(303, 35);
            this.numericUpDownSteps.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numericUpDownSteps.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownSteps.Name = "numericUpDownSteps";
            this.numericUpDownSteps.Size = new System.Drawing.Size(49, 20);
            this.numericUpDownSteps.TabIndex = 15;
            this.numericUpDownSteps.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // numericUpDownIntervall
            // 
            this.numericUpDownIntervall.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownIntervall.Increment = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.numericUpDownIntervall.Location = new System.Drawing.Point(303, 9);
            this.numericUpDownIntervall.Maximum = new decimal(new int[] {
            1440,
            0,
            0,
            0});
            this.numericUpDownIntervall.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownIntervall.Name = "numericUpDownIntervall";
            this.numericUpDownIntervall.Size = new System.Drawing.Size(49, 20);
            this.numericUpDownIntervall.TabIndex = 16;
            this.numericUpDownIntervall.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Simulate to";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(224, 39);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Steps";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(224, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Intervall (min)";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Simulate from";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // dateTimePickerTo
            // 
            this.dateTimePickerTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickerTo.CustomFormat = "dd.MM.yy HH:mm";
            this.dateTimePickerTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerTo.Location = new System.Drawing.Point(98, 35);
            this.dateTimePickerTo.Name = "dateTimePickerTo";
            this.dateTimePickerTo.Size = new System.Drawing.Size(110, 20);
            this.dateTimePickerTo.TabIndex = 9;
            // 
            // dateTimePickerFrom
            // 
            this.dateTimePickerFrom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickerFrom.CustomFormat = "dd.MM.yy HH:mm";
            this.dateTimePickerFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerFrom.Location = new System.Drawing.Point(98, 9);
            this.dateTimePickerFrom.Name = "dateTimePickerFrom";
            this.dateTimePickerFrom.Size = new System.Drawing.Size(110, 20);
            this.dateTimePickerFrom.TabIndex = 10;
            // 
            // NotifyItemForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 281);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(480, 320);
            this.Name = "NotifyItemForm";
            this.Text = "NotifyItem Details";
            this.Load += new System.EventHandler(this.TestSilenceTimesForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBoxDefST.ResumeLayout(false);
            this.groupBoxSimResult.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSteps)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownIntervall)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBoxDefST;
        private System.Windows.Forms.ListView listViewDefST;
        private System.Windows.Forms.ColumnHeader ColHeadNotifyItem;
        private System.Windows.Forms.GroupBox groupBoxSimResult;
        private System.Windows.Forms.RichTextBox richTextBoxSimResult;
        private System.Windows.Forms.Button buttonSimulate;
        private System.Windows.Forms.NumericUpDown numericUpDownSteps;
        private System.Windows.Forms.NumericUpDown numericUpDownIntervall;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePickerTo;
        private System.Windows.Forms.DateTimePicker dateTimePickerFrom;
    }
}