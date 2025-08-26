namespace Ki_WAT
{
    partial class Frm_Mainfrm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_Mainfrm));
            this.panelNavBar = new System.Windows.Forms.Panel();
            this.Btn_Result = new System.Windows.Forms.Button();
            this.Btn_StaticMaster = new System.Windows.Forms.Button();
            this.Btn_Rolling = new System.Windows.Forms.Button();
            this.Btn_T = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.btnIo = new System.Windows.Forms.Button();
            this.BtnConfig = new System.Windows.Forms.Button();
            this.BtnManual = new System.Windows.Forms.Button();
            this.BtnMain = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Status_Screw_R = new System.Windows.Forms.Label();
            this.Status_Screw_L = new System.Windows.Forms.Label();
            this.Status_PLC = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.button4 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.Status_BAR = new System.Windows.Forms.Label();
            this.panelNavBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // panelNavBar
            // 
            this.panelNavBar.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panelNavBar.Controls.Add(this.Btn_Result);
            this.panelNavBar.Controls.Add(this.Btn_StaticMaster);
            this.panelNavBar.Controls.Add(this.Btn_Rolling);
            this.panelNavBar.Controls.Add(this.Btn_T);
            this.panelNavBar.Controls.Add(this.pictureBox1);
            this.panelNavBar.Controls.Add(this.picLogo);
            this.panelNavBar.Controls.Add(this.btnIo);
            this.panelNavBar.Controls.Add(this.BtnConfig);
            this.panelNavBar.Controls.Add(this.BtnManual);
            this.panelNavBar.Controls.Add(this.BtnMain);
            this.panelNavBar.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelNavBar.Location = new System.Drawing.Point(0, 0);
            this.panelNavBar.Margin = new System.Windows.Forms.Padding(4);
            this.panelNavBar.Name = "panelNavBar";
            this.panelNavBar.Size = new System.Drawing.Size(91, 1003);
            this.panelNavBar.TabIndex = 6;
            // 
            // Btn_Result
            // 
            this.Btn_Result.BackColor = System.Drawing.Color.Gainsboro;
            this.Btn_Result.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Btn_Result.Font = new System.Drawing.Font("Verdana", 12F);
            this.Btn_Result.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Btn_Result.Location = new System.Drawing.Point(4, 371);
            this.Btn_Result.Margin = new System.Windows.Forms.Padding(4);
            this.Btn_Result.Name = "Btn_Result";
            this.Btn_Result.Size = new System.Drawing.Size(83, 60);
            this.Btn_Result.TabIndex = 555;
            this.Btn_Result.Tag = "frmParameter";
            this.Btn_Result.Text = "Result";
            this.Btn_Result.UseVisualStyleBackColor = false;
            this.Btn_Result.Click += new System.EventHandler(this.Btn_Result_Click);
            // 
            // Btn_StaticMaster
            // 
            this.Btn_StaticMaster.BackColor = System.Drawing.Color.Gainsboro;
            this.Btn_StaticMaster.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Btn_StaticMaster.Font = new System.Drawing.Font("Verdana", 12F);
            this.Btn_StaticMaster.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Btn_StaticMaster.Location = new System.Drawing.Point(4, 302);
            this.Btn_StaticMaster.Margin = new System.Windows.Forms.Padding(4);
            this.Btn_StaticMaster.Name = "Btn_StaticMaster";
            this.Btn_StaticMaster.Size = new System.Drawing.Size(83, 60);
            this.Btn_StaticMaster.TabIndex = 554;
            this.Btn_StaticMaster.Tag = "frmParameter";
            this.Btn_StaticMaster.Text = "Static Master";
            this.Btn_StaticMaster.UseVisualStyleBackColor = false;
            this.Btn_StaticMaster.Click += new System.EventHandler(this.Btn_StaticMaster_Click);
            // 
            // Btn_Rolling
            // 
            this.Btn_Rolling.BackColor = System.Drawing.Color.Gainsboro;
            this.Btn_Rolling.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Btn_Rolling.Font = new System.Drawing.Font("Verdana", 12F);
            this.Btn_Rolling.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Btn_Rolling.Location = new System.Drawing.Point(4, 233);
            this.Btn_Rolling.Margin = new System.Windows.Forms.Padding(4);
            this.Btn_Rolling.Name = "Btn_Rolling";
            this.Btn_Rolling.Size = new System.Drawing.Size(83, 60);
            this.Btn_Rolling.TabIndex = 553;
            this.Btn_Rolling.Tag = "frmParameter";
            this.Btn_Rolling.Text = "Rolling master";
            this.Btn_Rolling.UseVisualStyleBackColor = false;
            this.Btn_Rolling.Click += new System.EventHandler(this.BtnCal_Click);
            // 
            // Btn_T
            // 
            this.Btn_T.BackColor = System.Drawing.Color.Gainsboro;
            this.Btn_T.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Btn_T.Font = new System.Drawing.Font("Verdana", 12F);
            this.Btn_T.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Btn_T.Location = new System.Drawing.Point(4, 816);
            this.Btn_T.Margin = new System.Windows.Forms.Padding(4);
            this.Btn_T.Name = "Btn_T";
            this.Btn_T.Size = new System.Drawing.Size(83, 60);
            this.Btn_T.TabIndex = 551;
            this.Btn_T.Tag = "frmSetting";
            this.Btn_T.Text = "TT";
            this.Btn_T.UseVisualStyleBackColor = false;
            this.Btn_T.Click += new System.EventHandler(this.Btn_T_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.InitialImage")));
            this.pictureBox1.Location = new System.Drawing.Point(7, 909);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(77, 63);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 550;
            this.pictureBox1.TabStop = false;
            // 
            // picLogo
            // 
            this.picLogo.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.picLogo.Image = ((System.Drawing.Image)(resources.GetObject("picLogo.Image")));
            this.picLogo.InitialImage = ((System.Drawing.Image)(resources.GetObject("picLogo.InitialImage")));
            this.picLogo.Location = new System.Drawing.Point(12, 12);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(66, 64);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picLogo.TabIndex = 549;
            this.picLogo.TabStop = false;
            // 
            // btnIo
            // 
            this.btnIo.BackColor = System.Drawing.Color.Gainsboro;
            this.btnIo.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnIo.Font = new System.Drawing.Font("Verdana", 12F);
            this.btnIo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnIo.Location = new System.Drawing.Point(4, 439);
            this.btnIo.Margin = new System.Windows.Forms.Padding(4);
            this.btnIo.Name = "btnIo";
            this.btnIo.Size = new System.Drawing.Size(83, 60);
            this.btnIo.TabIndex = 94;
            this.btnIo.Tag = "frmIo";
            this.btnIo.Text = "Digital I/O";
            this.btnIo.UseVisualStyleBackColor = false;
            this.btnIo.Click += new System.EventHandler(this.btnIo_Click);
            // 
            // BtnConfig
            // 
            this.BtnConfig.BackColor = System.Drawing.Color.Gainsboro;
            this.BtnConfig.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtnConfig.Font = new System.Drawing.Font("Verdana", 12F);
            this.BtnConfig.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BtnConfig.Location = new System.Drawing.Point(4, 164);
            this.BtnConfig.Margin = new System.Windows.Forms.Padding(4);
            this.BtnConfig.Name = "BtnConfig";
            this.BtnConfig.Size = new System.Drawing.Size(83, 60);
            this.BtnConfig.TabIndex = 7;
            this.BtnConfig.Tag = "frmSetting";
            this.BtnConfig.Text = "Config";
            this.BtnConfig.UseVisualStyleBackColor = false;
            this.BtnConfig.Click += new System.EventHandler(this.BtnConfig_Click);
            // 
            // BtnManual
            // 
            this.BtnManual.BackColor = System.Drawing.Color.Gainsboro;
            this.BtnManual.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtnManual.Font = new System.Drawing.Font("Verdana", 12F);
            this.BtnManual.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BtnManual.Location = new System.Drawing.Point(4, 507);
            this.BtnManual.Margin = new System.Windows.Forms.Padding(4);
            this.BtnManual.Name = "BtnManual";
            this.BtnManual.Size = new System.Drawing.Size(83, 60);
            this.BtnManual.TabIndex = 4;
            this.BtnManual.Tag = "frmManual";
            this.BtnManual.Text = "Manual";
            this.BtnManual.UseVisualStyleBackColor = false;
            this.BtnManual.Click += new System.EventHandler(this.btnManual_Click);
            // 
            // BtnMain
            // 
            this.BtnMain.BackColor = System.Drawing.Color.Gainsboro;
            this.BtnMain.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtnMain.Font = new System.Drawing.Font("Verdana", 12F);
            this.BtnMain.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BtnMain.Location = new System.Drawing.Point(4, 95);
            this.BtnMain.Margin = new System.Windows.Forms.Padding(4);
            this.BtnMain.Name = "BtnMain";
            this.BtnMain.Size = new System.Drawing.Size(83, 60);
            this.BtnMain.TabIndex = 2;
            this.BtnMain.Tag = "frmMain";
            this.BtnMain.Text = "Main";
            this.BtnMain.UseVisualStyleBackColor = false;
            this.BtnMain.Click += new System.EventHandler(this.BtnMain_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panel1.Controls.Add(this.Status_BAR);
            this.panel1.Controls.Add(this.Status_Screw_R);
            this.panel1.Controls.Add(this.Status_Screw_L);
            this.panel1.Controls.Add(this.Status_PLC);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.button6);
            this.panel1.Location = new System.Drawing.Point(94, 955);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1822, 48);
            this.panel1.TabIndex = 8;
            // 
            // Status_Screw_R
            // 
            this.Status_Screw_R.BackColor = System.Drawing.Color.Gray;
            this.Status_Screw_R.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Status_Screw_R.Location = new System.Drawing.Point(312, 7);
            this.Status_Screw_R.Name = "Status_Screw_R";
            this.Status_Screw_R.Size = new System.Drawing.Size(121, 32);
            this.Status_Screw_R.TabIndex = 554;
            this.Status_Screw_R.Text = "Sensor";
            this.Status_Screw_R.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Status_Screw_L
            // 
            this.Status_Screw_L.BackColor = System.Drawing.Color.Gray;
            this.Status_Screw_L.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Status_Screw_L.Location = new System.Drawing.Point(169, 7);
            this.Status_Screw_L.Name = "Status_Screw_L";
            this.Status_Screw_L.Size = new System.Drawing.Size(137, 32);
            this.Status_Screw_L.TabIndex = 553;
            this.Status_Screw_L.Text = "Screwdriver";
            this.Status_Screw_L.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Status_PLC
            // 
            this.Status_PLC.BackColor = System.Drawing.Color.Gray;
            this.Status_PLC.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Status_PLC.Location = new System.Drawing.Point(42, 7);
            this.Status_PLC.Name = "Status_PLC";
            this.Status_PLC.Size = new System.Drawing.Size(121, 32);
            this.Status_PLC.TabIndex = 552;
            this.Status_PLC.Text = "PLC";
            this.Status_PLC.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.Gainsboro;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button3.Font = new System.Drawing.Font("Verdana", 12F);
            this.button3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button3.Location = new System.Drawing.Point(4, 816);
            this.button3.Margin = new System.Windows.Forms.Padding(4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(83, 60);
            this.button3.TabIndex = 551;
            this.button3.Tag = "frmSetting";
            this.button3.Text = "TT";
            this.button3.UseVisualStyleBackColor = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox2.InitialImage")));
            this.pictureBox2.Location = new System.Drawing.Point(7, 909);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(77, 63);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 550;
            this.pictureBox2.TabStop = false;
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.Gainsboro;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button4.Font = new System.Drawing.Font("Verdana", 12F);
            this.button4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button4.Location = new System.Drawing.Point(4, 748);
            this.button4.Margin = new System.Windows.Forms.Padding(4);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(83, 60);
            this.button4.TabIndex = 94;
            this.button4.Tag = "frmIo";
            this.button4.Text = "Digital I/O";
            this.button4.UseVisualStyleBackColor = false;
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.Color.Gainsboro;
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button6.Font = new System.Drawing.Font("Verdana", 12F);
            this.button6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button6.Location = new System.Drawing.Point(4, 680);
            this.button6.Margin = new System.Windows.Forms.Padding(4);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(83, 60);
            this.button6.TabIndex = 4;
            this.button6.Tag = "frmManual";
            this.button6.Text = "Manual";
            this.button6.UseVisualStyleBackColor = false;
            // 
            // Status_BAR
            // 
            this.Status_BAR.BackColor = System.Drawing.Color.Gray;
            this.Status_BAR.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Status_BAR.Location = new System.Drawing.Point(439, 7);
            this.Status_BAR.Name = "Status_BAR";
            this.Status_BAR.Size = new System.Drawing.Size(121, 32);
            this.Status_BAR.TabIndex = 555;
            this.Status_BAR.Text = "BarCode";
            this.Status_BAR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Frm_Mainfrm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(1904, 1003);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panelNavBar);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Name = "Frm_Mainfrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Parallelism";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Frm_Mainfrm_FormClosing);
            this.Load += new System.EventHandler(this.Frm_Mainfrm_Load);
            this.panelNavBar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panelNavBar;
        private System.Windows.Forms.Button btnIo;
        private System.Windows.Forms.Button BtnConfig;
        private System.Windows.Forms.Button BtnManual;
        private System.Windows.Forms.Button BtnMain;
        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button Btn_T;
        private System.Windows.Forms.Button Btn_Rolling;
        private System.Windows.Forms.Button Btn_StaticMaster;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Label Status_Screw_R;
        private System.Windows.Forms.Label Status_Screw_L;
        private System.Windows.Forms.Label Status_PLC;
        private System.Windows.Forms.Button Btn_Result;
        private System.Windows.Forms.Label Status_BAR;
    }
}

