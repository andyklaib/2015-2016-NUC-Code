namespace Leslie
{
    partial class Form1
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.camLaunchBT = new System.Windows.Forms.Button();
            this.sourceNUD = new System.Windows.Forms.NumericUpDown();
            this.widthNUD = new System.Windows.Forms.NumericUpDown();
            this.heightNUD = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.threshNUD = new System.Windows.Forms.NumericUpDown();
            this.debugCB = new System.Windows.Forms.CheckBox();
            this.connectBT = new System.Windows.Forms.Button();
            this.portCB = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.brefreshBT = new System.Windows.Forms.Button();
            this.videoStream = new System.Windows.Forms.PictureBox();
            this.frametimer = new System.Windows.Forms.Timer(this.components);
            this.updateTM = new System.Windows.Forms.Timer(this.components);
            this.lidarvisPB = new System.Windows.Forms.PictureBox();
            this.calibrateBT = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sourceNUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.widthNUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.heightNUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.threshNUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.videoStream)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lidarvisPB)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3});
            this.statusStrip1.Location = new System.Drawing.Point(0, 685);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1002, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(89, 17);
            this.toolStripStatusLabel1.Text = "Camera Status: ";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(80, 17);
            this.toolStripStatusLabel2.Text = "LIDAR Status: ";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(71, 17);
            this.toolStripStatusLabel3.Text = "Com Status:";
            // 
            // camLaunchBT
            // 
            this.camLaunchBT.Location = new System.Drawing.Point(12, 12);
            this.camLaunchBT.Name = "camLaunchBT";
            this.camLaunchBT.Size = new System.Drawing.Size(132, 23);
            this.camLaunchBT.TabIndex = 1;
            this.camLaunchBT.Text = "Launch Camera";
            this.camLaunchBT.UseVisualStyleBackColor = true;
            this.camLaunchBT.Click += new System.EventHandler(this.camLaunchBT_Click);
            // 
            // sourceNUD
            // 
            this.sourceNUD.Location = new System.Drawing.Point(99, 38);
            this.sourceNUD.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.sourceNUD.Name = "sourceNUD";
            this.sourceNUD.Size = new System.Drawing.Size(120, 20);
            this.sourceNUD.TabIndex = 2;
            // 
            // widthNUD
            // 
            this.widthNUD.Location = new System.Drawing.Point(99, 64);
            this.widthNUD.Maximum = new decimal(new int[] {
            1920,
            0,
            0,
            0});
            this.widthNUD.Name = "widthNUD";
            this.widthNUD.Size = new System.Drawing.Size(120, 20);
            this.widthNUD.TabIndex = 3;
            this.widthNUD.Value = new decimal(new int[] {
            1280,
            0,
            0,
            0});
            // 
            // heightNUD
            // 
            this.heightNUD.Location = new System.Drawing.Point(99, 90);
            this.heightNUD.Maximum = new decimal(new int[] {
            1080,
            0,
            0,
            0});
            this.heightNUD.Name = "heightNUD";
            this.heightNUD.Size = new System.Drawing.Size(120, 20);
            this.heightNUD.TabIndex = 4;
            this.heightNUD.Value = new decimal(new int[] {
            720,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Video Source";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Image Width";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 92);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Image Height";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 117);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Threshold";
            // 
            // threshNUD
            // 
            this.threshNUD.Location = new System.Drawing.Point(99, 115);
            this.threshNUD.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.threshNUD.Name = "threshNUD";
            this.threshNUD.Size = new System.Drawing.Size(120, 20);
            this.threshNUD.TabIndex = 8;
            this.threshNUD.Value = new decimal(new int[] {
            70,
            0,
            0,
            0});
            // 
            // debugCB
            // 
            this.debugCB.AutoSize = true;
            this.debugCB.Checked = true;
            this.debugCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.debugCB.Location = new System.Drawing.Point(12, 144);
            this.debugCB.Name = "debugCB";
            this.debugCB.Size = new System.Drawing.Size(58, 17);
            this.debugCB.TabIndex = 10;
            this.debugCB.Text = "Debug";
            this.debugCB.UseVisualStyleBackColor = true;
            // 
            // connectBT
            // 
            this.connectBT.Location = new System.Drawing.Point(445, 38);
            this.connectBT.Name = "connectBT";
            this.connectBT.Size = new System.Drawing.Size(75, 23);
            this.connectBT.TabIndex = 11;
            this.connectBT.Text = "Connect";
            this.connectBT.UseVisualStyleBackColor = true;
            this.connectBT.Click += new System.EventHandler(this.connectBT_Click);
            // 
            // portCB
            // 
            this.portCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.portCB.FormattingEnabled = true;
            this.portCB.Location = new System.Drawing.Point(318, 12);
            this.portCB.Name = "portCB";
            this.portCB.Size = new System.Drawing.Size(121, 21);
            this.portCB.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(223, 17);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(87, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "LIDAR COM port";
            // 
            // brefreshBT
            // 
            this.brefreshBT.Location = new System.Drawing.Point(445, 12);
            this.brefreshBT.Name = "brefreshBT";
            this.brefreshBT.Size = new System.Drawing.Size(75, 23);
            this.brefreshBT.TabIndex = 14;
            this.brefreshBT.Text = "Refresh";
            this.brefreshBT.UseVisualStyleBackColor = true;
            this.brefreshBT.Click += new System.EventHandler(this.brefreshBT_Click);
            // 
            // videoStream
            // 
            this.videoStream.Location = new System.Drawing.Point(12, 167);
            this.videoStream.Name = "videoStream";
            this.videoStream.Size = new System.Drawing.Size(357, 357);
            this.videoStream.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.videoStream.TabIndex = 15;
            this.videoStream.TabStop = false;
            // 
            // frametimer
            // 
            this.frametimer.Tick += new System.EventHandler(this.frametimer_Tick);
            // 
            // updateTM
            // 
            this.updateTM.Interval = 25;
            this.updateTM.Tick += new System.EventHandler(this.updateTM_Tick_1);
            // 
            // lidarvisPB
            // 
            this.lidarvisPB.Location = new System.Drawing.Point(375, 167);
            this.lidarvisPB.Name = "lidarvisPB";
            this.lidarvisPB.Size = new System.Drawing.Size(512, 512);
            this.lidarvisPB.TabIndex = 16;
            this.lidarvisPB.TabStop = false;
            // 
            // calibrateBT
            // 
            this.calibrateBT.Location = new System.Drawing.Point(539, 12);
            this.calibrateBT.Name = "calibrateBT";
            this.calibrateBT.Size = new System.Drawing.Size(75, 23);
            this.calibrateBT.TabIndex = 17;
            this.calibrateBT.Text = "Calibrate";
            this.calibrateBT.UseVisualStyleBackColor = true;
            this.calibrateBT.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1002, 707);
            this.Controls.Add(this.videoStream);
            this.Controls.Add(this.calibrateBT);
            this.Controls.Add(this.lidarvisPB);
            this.Controls.Add(this.brefreshBT);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.portCB);
            this.Controls.Add(this.connectBT);
            this.Controls.Add(this.debugCB);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.threshNUD);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.heightNUD);
            this.Controls.Add(this.widthNUD);
            this.Controls.Add(this.sourceNUD);
            this.Controls.Add(this.camLaunchBT);
            this.Controls.Add(this.statusStrip1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sourceNUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.widthNUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.heightNUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.threshNUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.videoStream)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lidarvisPB)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.Button camLaunchBT;
        private System.Windows.Forms.NumericUpDown sourceNUD;
        private System.Windows.Forms.NumericUpDown widthNUD;
        private System.Windows.Forms.NumericUpDown heightNUD;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown threshNUD;
        private System.Windows.Forms.CheckBox debugCB;
        private System.Windows.Forms.Button connectBT;
        private System.Windows.Forms.ComboBox portCB;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button brefreshBT;
        private System.Windows.Forms.PictureBox videoStream;
        private System.Windows.Forms.Timer frametimer;
        private System.Windows.Forms.Timer updateTM;
        private System.Windows.Forms.PictureBox lidarvisPB;
        private System.Windows.Forms.Button calibrateBT;
    }
}

