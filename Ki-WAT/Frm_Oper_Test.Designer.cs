namespace Ki_WAT
{
    partial class Frm_Oper_Test
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
            this.label9 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.lbl_SWB = new System.Windows.Forms.Label();
            this.lbl_hlt_message = new System.Windows.Forms.Label();
            this.lbl_PEV = new System.Windows.Forms.Label();
            this.GRP_RR_CAM = new RollTester.CGuage();
            this.GRP_RL_TOE = new RollTester.CGuage();
            this.GRP_FR_CAM = new RollTester.CGuage();
            this.GRP_FL_TOE = new RollTester.CGuage();
            this.GRP_RL_CAM = new RollTester.CGuage();
            this.GRP_RR_TOE = new RollTester.CGuage();
            this.GRP_FL_CAM = new RollTester.CGuage();
            this.GRP_FR_TOE = new RollTester.CGuage();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.MidnightBlue;
            this.label9.Font = new System.Drawing.Font("Arial", 30F);
            this.label9.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label9.Location = new System.Drawing.Point(12, 9);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(1876, 53);
            this.label9.TabIndex = 48;
            this.label9.Text = "WHEEL ALIGNMENT DATA";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(227, 427);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(103, 48);
            this.button1.TabIndex = 162;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(103, 442);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(101, 21);
            this.textBox1.TabIndex = 163;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::Ki_WAT.Properties.Resources.KakaoTalk_20250904_133015943_05;
            this.pictureBox2.Location = new System.Drawing.Point(773, 374);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(331, 218);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 161;
            this.pictureBox2.TabStop = false;
            // 
            // lbl_SWB
            // 
            this.lbl_SWB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(55)))), ((int)(((byte)(52)))));
            this.lbl_SWB.Font = new System.Drawing.Font("Arial", 30F);
            this.lbl_SWB.ForeColor = System.Drawing.Color.White;
            this.lbl_SWB.Location = new System.Drawing.Point(919, 494);
            this.lbl_SWB.Name = "lbl_SWB";
            this.lbl_SWB.Size = new System.Drawing.Size(150, 43);
            this.lbl_SWB.TabIndex = 50;
            this.lbl_SWB.Text = "0.0";
            this.lbl_SWB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_hlt_message
            // 
            this.lbl_hlt_message.BackColor = System.Drawing.Color.MediumSlateBlue;
            this.lbl_hlt_message.Font = new System.Drawing.Font("Arial", 30F);
            this.lbl_hlt_message.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lbl_hlt_message.Location = new System.Drawing.Point(12, 62);
            this.lbl_hlt_message.Name = "lbl_hlt_message";
            this.lbl_hlt_message.Size = new System.Drawing.Size(507, 53);
            this.lbl_hlt_message.TabIndex = 164;
            this.lbl_hlt_message.Text = "-";
            this.lbl_hlt_message.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_hlt_message.Visible = false;
            // 
            // lbl_PEV
            // 
            this.lbl_PEV.BackColor = System.Drawing.Color.Gray;
            this.lbl_PEV.Font = new System.Drawing.Font("Arial", 30F);
            this.lbl_PEV.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lbl_PEV.Location = new System.Drawing.Point(-8, 806);
            this.lbl_PEV.Name = "lbl_PEV";
            this.lbl_PEV.Size = new System.Drawing.Size(1876, 85);
            this.lbl_PEV.TabIndex = 165;
            this.lbl_PEV.Text = "PEV";
            this.lbl_PEV.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // GRP_RR_CAM
            // 
            this.GRP_RR_CAM.BgColor = System.Drawing.Color.CornflowerBlue;
            this.GRP_RR_CAM.currentValue = 0F;
            this.GRP_RR_CAM.Location = new System.Drawing.Point(1497, 496);
            this.GRP_RR_CAM.maxValue = 40F;
            this.GRP_RR_CAM.minValue = -40F;
            this.GRP_RR_CAM.Name = "GRP_RR_CAM";
            this.GRP_RR_CAM.NiddleColor = System.Drawing.Color.Magenta;
            this.GRP_RR_CAM.RegionColor = System.Drawing.Color.LightGreen;
            this.GRP_RR_CAM.Size = new System.Drawing.Size(371, 282);
            this.GRP_RR_CAM.SpeedMode = false;
            this.GRP_RR_CAM.TabIndex = 58;
            this.GRP_RR_CAM.Text = "cGuage5";
            this.GRP_RR_CAM.TickInterval = 30F;
            this.GRP_RR_CAM.Title = "CAMBER";
            this.GRP_RR_CAM.TitleColor = System.Drawing.Color.Black;
            this.GRP_RR_CAM.ValueColor = System.Drawing.Color.LightSteelBlue;
            // 
            // GRP_RL_TOE
            // 
            this.GRP_RL_TOE.BgColor = System.Drawing.Color.CornflowerBlue;
            this.GRP_RL_TOE.currentValue = 0F;
            this.GRP_RL_TOE.Location = new System.Drawing.Point(31, 496);
            this.GRP_RL_TOE.maxValue = 40F;
            this.GRP_RL_TOE.minValue = -40F;
            this.GRP_RL_TOE.Name = "GRP_RL_TOE";
            this.GRP_RL_TOE.NiddleColor = System.Drawing.Color.Magenta;
            this.GRP_RL_TOE.RegionColor = System.Drawing.Color.LightGreen;
            this.GRP_RL_TOE.Size = new System.Drawing.Size(371, 282);
            this.GRP_RL_TOE.SpeedMode = false;
            this.GRP_RL_TOE.TabIndex = 55;
            this.GRP_RL_TOE.Text = "cGuage8";
            this.GRP_RL_TOE.TickInterval = 10F;
            this.GRP_RL_TOE.Title = "PARA";
            this.GRP_RL_TOE.TitleColor = System.Drawing.Color.Black;
            this.GRP_RL_TOE.ValueColor = System.Drawing.Color.LightSteelBlue;
            // 
            // GRP_FR_CAM
            // 
            this.GRP_FR_CAM.BgColor = System.Drawing.Color.CornflowerBlue;
            this.GRP_FR_CAM.currentValue = 0F;
            this.GRP_FR_CAM.Location = new System.Drawing.Point(1497, 107);
            this.GRP_FR_CAM.maxValue = 40F;
            this.GRP_FR_CAM.minValue = -40F;
            this.GRP_FR_CAM.Name = "GRP_FR_CAM";
            this.GRP_FR_CAM.NiddleColor = System.Drawing.Color.Magenta;
            this.GRP_FR_CAM.RegionColor = System.Drawing.Color.LightGreen;
            this.GRP_FR_CAM.Size = new System.Drawing.Size(371, 282);
            this.GRP_FR_CAM.SpeedMode = false;
            this.GRP_FR_CAM.TabIndex = 54;
            this.GRP_FR_CAM.Text = "cGuage3";
            this.GRP_FR_CAM.TickInterval = 30F;
            this.GRP_FR_CAM.Title = "CAMBER";
            this.GRP_FR_CAM.TitleColor = System.Drawing.Color.Black;
            this.GRP_FR_CAM.ValueColor = System.Drawing.Color.LightSteelBlue;
            // 
            // GRP_FL_TOE
            // 
            this.GRP_FL_TOE.BgColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.GRP_FL_TOE.currentValue = 0F;
            this.GRP_FL_TOE.Location = new System.Drawing.Point(31, 107);
            this.GRP_FL_TOE.maxValue = 40F;
            this.GRP_FL_TOE.minValue = -40F;
            this.GRP_FL_TOE.Name = "GRP_FL_TOE";
            this.GRP_FL_TOE.NiddleColor = System.Drawing.Color.Magenta;
            this.GRP_FL_TOE.RegionColor = System.Drawing.Color.LightGreen;
            this.GRP_FL_TOE.Size = new System.Drawing.Size(371, 282);
            this.GRP_FL_TOE.SpeedMode = false;
            this.GRP_FL_TOE.TabIndex = 51;
            this.GRP_FL_TOE.Text = "cGuage2";
            this.GRP_FL_TOE.TickInterval = 10F;
            this.GRP_FL_TOE.Title = "PARA";
            this.GRP_FL_TOE.TitleColor = System.Drawing.Color.Black;
            this.GRP_FL_TOE.ValueColor = System.Drawing.Color.LightSteelBlue;
            // 
            // GRP_RL_CAM
            // 
            this.GRP_RL_CAM.BgColor = System.Drawing.Color.CornflowerBlue;
            this.GRP_RL_CAM.currentValue = 0F;
            this.GRP_RL_CAM.Location = new System.Drawing.Point(432, 496);
            this.GRP_RL_CAM.maxValue = 40F;
            this.GRP_RL_CAM.minValue = -40F;
            this.GRP_RL_CAM.Name = "GRP_RL_CAM";
            this.GRP_RL_CAM.NiddleColor = System.Drawing.Color.Magenta;
            this.GRP_RL_CAM.RegionColor = System.Drawing.Color.LightGreen;
            this.GRP_RL_CAM.Size = new System.Drawing.Size(371, 282);
            this.GRP_RL_CAM.SpeedMode = false;
            this.GRP_RL_CAM.TabIndex = 56;
            this.GRP_RL_CAM.Text = "cGuage7";
            this.GRP_RL_CAM.TickInterval = 30F;
            this.GRP_RL_CAM.Title = "CAMBER";
            this.GRP_RL_CAM.TitleColor = System.Drawing.Color.Black;
            this.GRP_RL_CAM.ValueColor = System.Drawing.Color.LightSteelBlue;
            // 
            // GRP_RR_TOE
            // 
            this.GRP_RR_TOE.BgColor = System.Drawing.Color.CornflowerBlue;
            this.GRP_RR_TOE.currentValue = 0F;
            this.GRP_RR_TOE.Location = new System.Drawing.Point(1070, 496);
            this.GRP_RR_TOE.maxValue = 40F;
            this.GRP_RR_TOE.minValue = -40F;
            this.GRP_RR_TOE.Name = "GRP_RR_TOE";
            this.GRP_RR_TOE.NiddleColor = System.Drawing.Color.Magenta;
            this.GRP_RR_TOE.RegionColor = System.Drawing.Color.LightGreen;
            this.GRP_RR_TOE.Size = new System.Drawing.Size(371, 282);
            this.GRP_RR_TOE.SpeedMode = false;
            this.GRP_RR_TOE.TabIndex = 57;
            this.GRP_RR_TOE.Text = "cGuage6";
            this.GRP_RR_TOE.TickInterval = 10F;
            this.GRP_RR_TOE.Title = "PARA";
            this.GRP_RR_TOE.TitleColor = System.Drawing.Color.Black;
            this.GRP_RR_TOE.ValueColor = System.Drawing.Color.LightSteelBlue;
            // 
            // GRP_FL_CAM
            // 
            this.GRP_FL_CAM.BgColor = System.Drawing.Color.CornflowerBlue;
            this.GRP_FL_CAM.currentValue = 0F;
            this.GRP_FL_CAM.Location = new System.Drawing.Point(432, 107);
            this.GRP_FL_CAM.maxValue = 40F;
            this.GRP_FL_CAM.minValue = -40F;
            this.GRP_FL_CAM.Name = "GRP_FL_CAM";
            this.GRP_FL_CAM.NiddleColor = System.Drawing.Color.Magenta;
            this.GRP_FL_CAM.RegionColor = System.Drawing.Color.LightGreen;
            this.GRP_FL_CAM.Size = new System.Drawing.Size(371, 282);
            this.GRP_FL_CAM.SpeedMode = false;
            this.GRP_FL_CAM.TabIndex = 52;
            this.GRP_FL_CAM.Text = "cGuage1";
            this.GRP_FL_CAM.TickInterval = 30F;
            this.GRP_FL_CAM.Title = "CAMBER";
            this.GRP_FL_CAM.TitleColor = System.Drawing.Color.Black;
            this.GRP_FL_CAM.ValueColor = System.Drawing.Color.LightSteelBlue;
            // 
            // GRP_FR_TOE
            // 
            this.GRP_FR_TOE.BgColor = System.Drawing.Color.CornflowerBlue;
            this.GRP_FR_TOE.currentValue = 0F;
            this.GRP_FR_TOE.Location = new System.Drawing.Point(1070, 107);
            this.GRP_FR_TOE.maxValue = 40F;
            this.GRP_FR_TOE.minValue = -40F;
            this.GRP_FR_TOE.Name = "GRP_FR_TOE";
            this.GRP_FR_TOE.NiddleColor = System.Drawing.Color.Magenta;
            this.GRP_FR_TOE.RegionColor = System.Drawing.Color.LightGreen;
            this.GRP_FR_TOE.Size = new System.Drawing.Size(371, 282);
            this.GRP_FR_TOE.SpeedMode = false;
            this.GRP_FR_TOE.TabIndex = 53;
            this.GRP_FR_TOE.Text = "cGuage4";
            this.GRP_FR_TOE.TickInterval = 10F;
            this.GRP_FR_TOE.Title = "PARA";
            this.GRP_FR_TOE.TitleColor = System.Drawing.Color.Black;
            this.GRP_FR_TOE.ValueColor = System.Drawing.Color.LightSteelBlue;
            // 
            // Frm_Oper_Test
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1900, 900);
            this.Controls.Add(this.lbl_PEV);
            this.Controls.Add(this.lbl_hlt_message);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.GRP_RR_CAM);
            this.Controls.Add(this.GRP_RL_TOE);
            this.Controls.Add(this.GRP_FR_CAM);
            this.Controls.Add(this.GRP_FL_TOE);
            this.Controls.Add(this.lbl_SWB);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.GRP_RL_CAM);
            this.Controls.Add(this.GRP_RR_TOE);
            this.Controls.Add(this.GRP_FL_CAM);
            this.Controls.Add(this.GRP_FR_TOE);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Frm_Oper_Test";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "ㄱ";
            this.Load += new System.EventHandler(this.Frm_Oper_Test_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label9;
        private RollTester.CGuage GRP_FL_TOE;
        private RollTester.CGuage GRP_FL_CAM;
        private RollTester.CGuage GRP_FR_CAM;
        private RollTester.CGuage GRP_FR_TOE;
        private RollTester.CGuage GRP_RR_CAM;
        private RollTester.CGuage GRP_RR_TOE;
        private RollTester.CGuage GRP_RL_CAM;
        private RollTester.CGuage GRP_RL_TOE;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label lbl_SWB;
        private System.Windows.Forms.Label lbl_hlt_message;
        private System.Windows.Forms.Label lbl_PEV;
    }
}