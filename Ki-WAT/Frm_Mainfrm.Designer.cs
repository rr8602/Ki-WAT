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
            this.Btn_VEP = new System.Windows.Forms.Button();
            this.Btn_Result = new System.Windows.Forms.Button();
            this.Btn_StaticMaster = new System.Windows.Forms.Button();
            this.Btn_Rolling = new System.Windows.Forms.Button();
            this.Btn_T = new System.Windows.Forms.Button();
            this.btnIo = new System.Windows.Forms.Button();
            this.BtnConfig = new System.Windows.Forms.Button();
            this.BtnManual = new System.Windows.Forms.Button();
            this.BtnMain = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.stcLable = new System.Windows.Forms.Label();
            this.picBK = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.lbl_SEN_RR = new KI_Controls.RoundLabel();
            this.lbl_SEN_RL = new KI_Controls.RoundLabel();
            this.lbl_SEN_FR = new KI_Controls.RoundLabel();
            this.lbl_SEN_FL = new KI_Controls.RoundLabel();
            this.lbl_State_VEP = new KI_Controls.RoundLabel();
            this.lbl_Status_BAR = new KI_Controls.RoundLabel();
            this.lbl_Status_SWB = new KI_Controls.RoundLabel();
            this.Status_Screw_L = new KI_Controls.RoundLabel();
            this.Status_PLC = new KI_Controls.RoundLabel();
            this.panelNavBar.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBK)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // panelNavBar
            // 
            this.panelNavBar.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panelNavBar.Controls.Add(this.Btn_VEP);
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
            // Btn_VEP
            // 
            this.Btn_VEP.BackColor = System.Drawing.Color.Gainsboro;
            this.Btn_VEP.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Btn_VEP.Font = new System.Drawing.Font("Verdana", 12F);
            this.Btn_VEP.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Btn_VEP.Location = new System.Drawing.Point(4, 756);
            this.Btn_VEP.Margin = new System.Windows.Forms.Padding(4);
            this.Btn_VEP.Name = "Btn_VEP";
            this.Btn_VEP.Size = new System.Drawing.Size(83, 60);
            this.Btn_VEP.TabIndex = 556;
            this.Btn_VEP.Tag = "frmSetting";
            this.Btn_VEP.Text = "VEP";
            this.Btn_VEP.UseVisualStyleBackColor = false;
            this.Btn_VEP.Click += new System.EventHandler(this.Btn_VEP_Click);
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
            this.Btn_T.Location = new System.Drawing.Point(4, 575);
            this.Btn_T.Margin = new System.Windows.Forms.Padding(4);
            this.Btn_T.Name = "Btn_T";
            this.Btn_T.Size = new System.Drawing.Size(83, 60);
            this.Btn_T.TabIndex = 551;
            this.Btn_T.Tag = "frmSetting";
            this.Btn_T.Text = "Simul";
            this.Btn_T.UseVisualStyleBackColor = false;
            this.Btn_T.Click += new System.EventHandler(this.Btn_T_Click);
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
            this.panel1.Controls.Add(this.lbl_SEN_RR);
            this.panel1.Controls.Add(this.lbl_SEN_RL);
            this.panel1.Controls.Add(this.lbl_SEN_FR);
            this.panel1.Controls.Add(this.lbl_SEN_FL);
            this.panel1.Controls.Add(this.lbl_State_VEP);
            this.panel1.Controls.Add(this.lbl_Status_BAR);
            this.panel1.Controls.Add(this.lbl_Status_SWB);
            this.panel1.Controls.Add(this.Status_Screw_L);
            this.panel1.Controls.Add(this.Status_PLC);
            this.panel1.Location = new System.Drawing.Point(94, 950);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1822, 55);
            this.panel1.TabIndex = 8;
            // 
            // stcLable
            // 
            this.stcLable.Font = new System.Drawing.Font("Arial", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stcLable.Location = new System.Drawing.Point(102, 737);
            this.stcLable.Name = "stcLable";
            this.stcLable.Size = new System.Drawing.Size(1790, 208);
            this.stcLable.TabIndex = 557;
            this.stcLable.Text = "System Initialize ...";
            this.stcLable.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // picBK
            // 
            this.picBK.BackgroundImage = global::Ki_WAT.Properties.Resources.logo;
            this.picBK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.picBK.Location = new System.Drawing.Point(98, 12);
            this.picBK.Name = "picBK";
            this.picBK.Size = new System.Drawing.Size(1794, 725);
            this.picBK.TabIndex = 556;
            this.picBK.TabStop = false;
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
            // lbl_SEN_RR
            // 
            this.lbl_SEN_RR.BackColor = System.Drawing.Color.Gray;
            this.lbl_SEN_RR.BackgroundColor = System.Drawing.Color.Gray;
            this.lbl_SEN_RR.BorderColor = System.Drawing.Color.Gray;
            this.lbl_SEN_RR.BorderRadius = 5;
            this.lbl_SEN_RR.BorderSize = 0;
            this.lbl_SEN_RR.FlatAppearance.BorderSize = 0;
            this.lbl_SEN_RR.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbl_SEN_RR.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.lbl_SEN_RR.ForeColor = System.Drawing.Color.White;
            this.lbl_SEN_RR.Location = new System.Drawing.Point(1647, 6);
            this.lbl_SEN_RR.Name = "lbl_SEN_RR";
            this.lbl_SEN_RR.Size = new System.Drawing.Size(81, 31);
            this.lbl_SEN_RR.TabIndex = 578;
            this.lbl_SEN_RR.Text = "RR";
            this.lbl_SEN_RR.TextColor = System.Drawing.Color.White;
            this.lbl_SEN_RR.UseVisualStyleBackColor = false;
            // 
            // lbl_SEN_RL
            // 
            this.lbl_SEN_RL.BackColor = System.Drawing.Color.Gray;
            this.lbl_SEN_RL.BackgroundColor = System.Drawing.Color.Gray;
            this.lbl_SEN_RL.BorderColor = System.Drawing.Color.Gray;
            this.lbl_SEN_RL.BorderRadius = 5;
            this.lbl_SEN_RL.BorderSize = 0;
            this.lbl_SEN_RL.FlatAppearance.BorderSize = 0;
            this.lbl_SEN_RL.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbl_SEN_RL.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.lbl_SEN_RL.ForeColor = System.Drawing.Color.White;
            this.lbl_SEN_RL.Location = new System.Drawing.Point(1541, 6);
            this.lbl_SEN_RL.Name = "lbl_SEN_RL";
            this.lbl_SEN_RL.Size = new System.Drawing.Size(81, 31);
            this.lbl_SEN_RL.TabIndex = 577;
            this.lbl_SEN_RL.Text = "RL";
            this.lbl_SEN_RL.TextColor = System.Drawing.Color.White;
            this.lbl_SEN_RL.UseVisualStyleBackColor = false;
            // 
            // lbl_SEN_FR
            // 
            this.lbl_SEN_FR.BackColor = System.Drawing.Color.Gray;
            this.lbl_SEN_FR.BackgroundColor = System.Drawing.Color.Gray;
            this.lbl_SEN_FR.BorderColor = System.Drawing.Color.Gray;
            this.lbl_SEN_FR.BorderRadius = 5;
            this.lbl_SEN_FR.BorderSize = 0;
            this.lbl_SEN_FR.FlatAppearance.BorderSize = 0;
            this.lbl_SEN_FR.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbl_SEN_FR.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.lbl_SEN_FR.ForeColor = System.Drawing.Color.White;
            this.lbl_SEN_FR.Location = new System.Drawing.Point(1435, 6);
            this.lbl_SEN_FR.Name = "lbl_SEN_FR";
            this.lbl_SEN_FR.Size = new System.Drawing.Size(81, 31);
            this.lbl_SEN_FR.TabIndex = 576;
            this.lbl_SEN_FR.Text = "FR";
            this.lbl_SEN_FR.TextColor = System.Drawing.Color.White;
            this.lbl_SEN_FR.UseVisualStyleBackColor = false;
            // 
            // lbl_SEN_FL
            // 
            this.lbl_SEN_FL.BackColor = System.Drawing.Color.Gray;
            this.lbl_SEN_FL.BackgroundColor = System.Drawing.Color.Gray;
            this.lbl_SEN_FL.BorderColor = System.Drawing.Color.Gray;
            this.lbl_SEN_FL.BorderRadius = 5;
            this.lbl_SEN_FL.BorderSize = 0;
            this.lbl_SEN_FL.FlatAppearance.BorderSize = 0;
            this.lbl_SEN_FL.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbl_SEN_FL.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.lbl_SEN_FL.ForeColor = System.Drawing.Color.White;
            this.lbl_SEN_FL.Location = new System.Drawing.Point(1329, 6);
            this.lbl_SEN_FL.Name = "lbl_SEN_FL";
            this.lbl_SEN_FL.Size = new System.Drawing.Size(81, 31);
            this.lbl_SEN_FL.TabIndex = 575;
            this.lbl_SEN_FL.Text = "FL";
            this.lbl_SEN_FL.TextColor = System.Drawing.Color.White;
            this.lbl_SEN_FL.UseVisualStyleBackColor = false;
            // 
            // lbl_State_VEP
            // 
            this.lbl_State_VEP.BackColor = System.Drawing.Color.Gray;
            this.lbl_State_VEP.BackgroundColor = System.Drawing.Color.Gray;
            this.lbl_State_VEP.BorderColor = System.Drawing.Color.Gray;
            this.lbl_State_VEP.BorderRadius = 5;
            this.lbl_State_VEP.BorderSize = 0;
            this.lbl_State_VEP.FlatAppearance.BorderSize = 0;
            this.lbl_State_VEP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbl_State_VEP.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.lbl_State_VEP.ForeColor = System.Drawing.Color.White;
            this.lbl_State_VEP.Location = new System.Drawing.Point(511, 6);
            this.lbl_State_VEP.Name = "lbl_State_VEP";
            this.lbl_State_VEP.Size = new System.Drawing.Size(111, 37);
            this.lbl_State_VEP.TabIndex = 566;
            this.lbl_State_VEP.Text = "VEP";
            this.lbl_State_VEP.TextColor = System.Drawing.Color.White;
            this.lbl_State_VEP.UseVisualStyleBackColor = false;
            // 
            // lbl_Status_BAR
            // 
            this.lbl_Status_BAR.BackColor = System.Drawing.Color.Gray;
            this.lbl_Status_BAR.BackgroundColor = System.Drawing.Color.Gray;
            this.lbl_Status_BAR.BorderColor = System.Drawing.Color.Gray;
            this.lbl_Status_BAR.BorderRadius = 5;
            this.lbl_Status_BAR.BorderSize = 0;
            this.lbl_Status_BAR.FlatAppearance.BorderSize = 0;
            this.lbl_Status_BAR.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbl_Status_BAR.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.lbl_Status_BAR.ForeColor = System.Drawing.Color.White;
            this.lbl_Status_BAR.Location = new System.Drawing.Point(396, 6);
            this.lbl_Status_BAR.Name = "lbl_Status_BAR";
            this.lbl_Status_BAR.Size = new System.Drawing.Size(111, 37);
            this.lbl_Status_BAR.TabIndex = 565;
            this.lbl_Status_BAR.Text = "BARCODE";
            this.lbl_Status_BAR.TextColor = System.Drawing.Color.White;
            this.lbl_Status_BAR.UseVisualStyleBackColor = false;
            // 
            // lbl_Status_SWB
            // 
            this.lbl_Status_SWB.BackColor = System.Drawing.Color.Gray;
            this.lbl_Status_SWB.BackgroundColor = System.Drawing.Color.Gray;
            this.lbl_Status_SWB.BorderColor = System.Drawing.Color.Gray;
            this.lbl_Status_SWB.BorderRadius = 5;
            this.lbl_Status_SWB.BorderSize = 0;
            this.lbl_Status_SWB.FlatAppearance.BorderSize = 0;
            this.lbl_Status_SWB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbl_Status_SWB.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.lbl_Status_SWB.ForeColor = System.Drawing.Color.White;
            this.lbl_Status_SWB.Location = new System.Drawing.Point(281, 6);
            this.lbl_Status_SWB.Name = "lbl_Status_SWB";
            this.lbl_Status_SWB.Size = new System.Drawing.Size(111, 37);
            this.lbl_Status_SWB.TabIndex = 564;
            this.lbl_Status_SWB.Text = "SWB";
            this.lbl_Status_SWB.TextColor = System.Drawing.Color.White;
            this.lbl_Status_SWB.UseVisualStyleBackColor = false;
            // 
            // Status_Screw_L
            // 
            this.Status_Screw_L.BackColor = System.Drawing.Color.Gray;
            this.Status_Screw_L.BackgroundColor = System.Drawing.Color.Gray;
            this.Status_Screw_L.BorderColor = System.Drawing.Color.Gray;
            this.Status_Screw_L.BorderRadius = 5;
            this.Status_Screw_L.BorderSize = 0;
            this.Status_Screw_L.FlatAppearance.BorderSize = 0;
            this.Status_Screw_L.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Status_Screw_L.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.Status_Screw_L.ForeColor = System.Drawing.Color.White;
            this.Status_Screw_L.Location = new System.Drawing.Point(166, 6);
            this.Status_Screw_L.Name = "Status_Screw_L";
            this.Status_Screw_L.Size = new System.Drawing.Size(111, 37);
            this.Status_Screw_L.TabIndex = 563;
            this.Status_Screw_L.Text = "Screwdriver";
            this.Status_Screw_L.TextColor = System.Drawing.Color.White;
            this.Status_Screw_L.UseVisualStyleBackColor = false;
            // 
            // Status_PLC
            // 
            this.Status_PLC.BackColor = System.Drawing.Color.Gray;
            this.Status_PLC.BackgroundColor = System.Drawing.Color.Gray;
            this.Status_PLC.BorderColor = System.Drawing.Color.Gray;
            this.Status_PLC.BorderRadius = 5;
            this.Status_PLC.BorderSize = 0;
            this.Status_PLC.FlatAppearance.BorderSize = 0;
            this.Status_PLC.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Status_PLC.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.Status_PLC.ForeColor = System.Drawing.Color.White;
            this.Status_PLC.Location = new System.Drawing.Point(51, 6);
            this.Status_PLC.Name = "Status_PLC";
            this.Status_PLC.Size = new System.Drawing.Size(111, 37);
            this.Status_PLC.TabIndex = 562;
            this.Status_PLC.Text = "PLC";
            this.Status_PLC.TextColor = System.Drawing.Color.White;
            this.Status_PLC.UseVisualStyleBackColor = false;
            // 
            // Frm_Mainfrm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(1904, 1003);
            this.Controls.Add(this.stcLable);
            this.Controls.Add(this.picBK);
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
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picBK)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
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
        private System.Windows.Forms.Button Btn_Result;
        private System.Windows.Forms.Button Btn_VEP;
        private KI_Controls.RoundLabel lbl_State_VEP;
        private KI_Controls.RoundLabel lbl_Status_BAR;
        private KI_Controls.RoundLabel lbl_Status_SWB;
        private KI_Controls.RoundLabel Status_Screw_L;
        private KI_Controls.RoundLabel Status_PLC;
        private KI_Controls.RoundLabel lbl_SEN_RR;
        private KI_Controls.RoundLabel lbl_SEN_RL;
        private KI_Controls.RoundLabel lbl_SEN_FR;
        private KI_Controls.RoundLabel lbl_SEN_FL;
        private System.Windows.Forms.PictureBox picBK;
        private System.Windows.Forms.Label stcLable;
    }
}

