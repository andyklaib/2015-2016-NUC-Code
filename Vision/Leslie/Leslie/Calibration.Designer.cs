namespace Leslie
{
    partial class Calibration
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
            this.saveBT = new System.Windows.Forms.Button();
            this.cancelBT = new System.Windows.Forms.Button();
            this.instTB = new System.Windows.Forms.Label();
            this.nextBT = new System.Windows.Forms.Button();
            this.lidarvisPB = new System.Windows.Forms.PictureBox();
            this.backBT = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.updateTM = new System.Windows.Forms.Timer(this.components);
            this.videoPB = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.lidarvisPB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.videoPB)).BeginInit();
            this.SuspendLayout();
            // 
            // saveBT
            // 
            this.saveBT.Location = new System.Drawing.Point(334, 12);
            this.saveBT.Name = "saveBT";
            this.saveBT.Size = new System.Drawing.Size(97, 23);
            this.saveBT.TabIndex = 0;
            this.saveBT.Text = "Save and Exit";
            this.saveBT.UseVisualStyleBackColor = true;
            // 
            // cancelBT
            // 
            this.cancelBT.Location = new System.Drawing.Point(334, 41);
            this.cancelBT.Name = "cancelBT";
            this.cancelBT.Size = new System.Drawing.Size(97, 23);
            this.cancelBT.TabIndex = 1;
            this.cancelBT.Text = "Cancel";
            this.cancelBT.UseVisualStyleBackColor = true;
            // 
            // instTB
            // 
            this.instTB.AutoSize = true;
            this.instTB.Location = new System.Drawing.Point(12, 9);
            this.instTB.Name = "instTB";
            this.instTB.Size = new System.Drawing.Size(61, 13);
            this.instTB.TabIndex = 2;
            this.instTB.Text = "Instructions";
            // 
            // nextBT
            // 
            this.nextBT.Location = new System.Drawing.Point(110, 65);
            this.nextBT.Name = "nextBT";
            this.nextBT.Size = new System.Drawing.Size(75, 23);
            this.nextBT.TabIndex = 3;
            this.nextBT.Text = "Next";
            this.nextBT.UseVisualStyleBackColor = true;
            this.nextBT.Click += new System.EventHandler(this.nextBT_Click);
            // 
            // lidarvisPB
            // 
            this.lidarvisPB.Location = new System.Drawing.Point(12, 94);
            this.lidarvisPB.Name = "lidarvisPB";
            this.lidarvisPB.Size = new System.Drawing.Size(512, 512);
            this.lidarvisPB.TabIndex = 4;
            this.lidarvisPB.TabStop = false;
            // 
            // backBT
            // 
            this.backBT.Location = new System.Drawing.Point(12, 65);
            this.backBT.Name = "backBT";
            this.backBT.Size = new System.Drawing.Size(75, 23);
            this.backBT.TabIndex = 5;
            this.backBT.Text = "Back";
            this.backBT.UseVisualStyleBackColor = true;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(1118, 94);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(181, 342);
            this.listBox1.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1115, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Test Data:";
            // 
            // updateTM
            // 
            this.updateTM.Tick += new System.EventHandler(this.updateTM_Tick);
            // 
            // videoPB
            // 
            this.videoPB.Location = new System.Drawing.Point(530, 94);
            this.videoPB.Name = "videoPB";
            this.videoPB.Size = new System.Drawing.Size(512, 512);
            this.videoPB.TabIndex = 8;
            this.videoPB.TabStop = false;
            // 
            // Calibration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1385, 749);
            this.Controls.Add(this.videoPB);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.backBT);
            this.Controls.Add(this.lidarvisPB);
            this.Controls.Add(this.nextBT);
            this.Controls.Add(this.instTB);
            this.Controls.Add(this.cancelBT);
            this.Controls.Add(this.saveBT);
            this.Name = "Calibration";
            this.Text = "Calibration";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Calibration_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.lidarvisPB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.videoPB)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button saveBT;
        private System.Windows.Forms.Button cancelBT;
        private System.Windows.Forms.Label instTB;
        private System.Windows.Forms.Button nextBT;
        private System.Windows.Forms.PictureBox lidarvisPB;
        private System.Windows.Forms.Button backBT;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer updateTM;
        private System.Windows.Forms.PictureBox videoPB;
    }
}