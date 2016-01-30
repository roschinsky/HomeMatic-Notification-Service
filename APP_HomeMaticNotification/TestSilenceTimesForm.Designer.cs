namespace TRoschinsky.Service.HomeMaticNotification
{
    partial class TestSilenceTimesForm
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
            this.listViewDefST = new System.Windows.Forms.ListView();
            this.ColHeadNotifyItem = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBoxDefST = new System.Windows.Forms.GroupBox();
            this.groupBoxSimResult = new System.Windows.Forms.GroupBox();
            this.richTextBoxSimResult = new System.Windows.Forms.RichTextBox();
            this.dateTimePickerFrom = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerTo = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDownIntervall = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDownSteps = new System.Windows.Forms.NumericUpDown();
            this.buttonSimulate = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBoxDefST.SuspendLayout();
            this.groupBoxSimResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownIntervall)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSteps)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listViewDefST
            // 
            this.listViewDefST.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColHeadNotifyItem});
            this.listViewDefST.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewDefST.Location = new System.Drawing.Point(3, 16);
            this.listViewDefST.Name = "listViewDefST";
            this.listViewDefST.Size = new System.Drawing.Size(234, 145);
            this.listViewDefST.TabIndex = 0;
            this.listViewDefST.UseCompatibleStateImageBehavior = false;
            this.listViewDefST.View = System.Windows.Forms.View.Details;
            // 
            // ColHeadNotifyItem
            // 
            this.ColHeadNotifyItem.Text = "NotifyItem";
            this.ColHeadNotifyItem.Width = 230;
            // 
            // groupBoxDefST
            // 
            this.groupBoxDefST.Controls.Add(this.listViewDefST);
            this.groupBoxDefST.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxDefST.Location = new System.Drawing.Point(0, 0);
            this.groupBoxDefST.Name = "groupBoxDefST";
            this.groupBoxDefST.Size = new System.Drawing.Size(240, 164);
            this.groupBoxDefST.TabIndex = 1;
            this.groupBoxDefST.TabStop = false;
            this.groupBoxDefST.Text = "Defined SilenceTimes";
            // 
            // groupBoxSimResult
            // 
            this.groupBoxSimResult.Controls.Add(this.richTextBoxSimResult);
            this.groupBoxSimResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxSimResult.Location = new System.Drawing.Point(0, 0);
            this.groupBoxSimResult.Name = "groupBoxSimResult";
            this.groupBoxSimResult.Size = new System.Drawing.Size(164, 164);
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
            this.richTextBoxSimResult.Size = new System.Drawing.Size(158, 145);
            this.richTextBoxSimResult.TabIndex = 0;
            this.richTextBoxSimResult.Text = "";
            // 
            // dateTimePickerFrom
            // 
            this.dateTimePickerFrom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickerFrom.CustomFormat = "dd.MM.yy HH:mm";
            this.dateTimePickerFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerFrom.Location = new System.Drawing.Point(89, 12);
            this.dateTimePickerFrom.Name = "dateTimePickerFrom";
            this.dateTimePickerFrom.Size = new System.Drawing.Size(110, 20);
            this.dateTimePickerFrom.TabIndex = 3;
            // 
            // dateTimePickerTo
            // 
            this.dateTimePickerTo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickerTo.CustomFormat = "dd.MM.yy HH:mm";
            this.dateTimePickerTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerTo.Location = new System.Drawing.Point(89, 38);
            this.dateTimePickerTo.Name = "dateTimePickerTo";
            this.dateTimePickerTo.Size = new System.Drawing.Size(110, 20);
            this.dateTimePickerTo.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Simulate from";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Simulate to";
            // 
            // numericUpDownIntervall
            // 
            this.numericUpDownIntervall.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownIntervall.Increment = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.numericUpDownIntervall.Location = new System.Drawing.Point(294, 12);
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
            this.numericUpDownIntervall.TabIndex = 6;
            this.numericUpDownIntervall.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(215, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Intervall (min)";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(215, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Steps";
            // 
            // numericUpDownSteps
            // 
            this.numericUpDownSteps.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownSteps.Location = new System.Drawing.Point(294, 38);
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
            this.numericUpDownSteps.TabIndex = 6;
            this.numericUpDownSteps.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // buttonSimulate
            // 
            this.buttonSimulate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSimulate.Location = new System.Drawing.Point(360, 12);
            this.buttonSimulate.Name = "buttonSimulate";
            this.buttonSimulate.Size = new System.Drawing.Size(59, 46);
            this.buttonSimulate.TabIndex = 7;
            this.buttonSimulate.Text = "Simulate";
            this.buttonSimulate.UseVisualStyleBackColor = true;
            this.buttonSimulate.Click += new System.EventHandler(this.buttonSimulate_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 64);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBoxDefST);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBoxSimResult);
            this.splitContainer1.Size = new System.Drawing.Size(408, 164);
            this.splitContainer1.SplitterDistance = 240;
            this.splitContainer1.TabIndex = 8;
            // 
            // TestSilenceTimesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(432, 240);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.buttonSimulate);
            this.Controls.Add(this.numericUpDownSteps);
            this.Controls.Add(this.numericUpDownIntervall);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dateTimePickerTo);
            this.Controls.Add(this.dateTimePickerFrom);
            this.Name = "TestSilenceTimesForm";
            this.Text = "SVC_HomeMaticNotification - Test SilenceTimes";
            this.Load += new System.EventHandler(this.TestSilenceTimesForm_Load);
            this.groupBoxDefST.ResumeLayout(false);
            this.groupBoxSimResult.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownIntervall)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSteps)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listViewDefST;
        private System.Windows.Forms.GroupBox groupBoxDefST;
        private System.Windows.Forms.GroupBox groupBoxSimResult;
        private System.Windows.Forms.RichTextBox richTextBoxSimResult;
        private System.Windows.Forms.DateTimePicker dateTimePickerFrom;
        private System.Windows.Forms.DateTimePicker dateTimePickerTo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDownIntervall;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDownSteps;
        private System.Windows.Forms.Button buttonSimulate;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ColumnHeader ColHeadNotifyItem;
    }
}