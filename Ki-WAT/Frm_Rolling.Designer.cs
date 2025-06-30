namespace Ki_WAT
{
    partial class Frm_Rolling
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_Rolling));
            this.lbl_Toe_FL = new System.Windows.Forms.Label();
            this.Btn_Start = new System.Windows.Forms.Button();
            this.lbl_Toe_FR = new System.Windows.Forms.Label();
            this.lbl_Cam_FL = new System.Windows.Forms.Label();
            this.lbl_Toe_RL = new System.Windows.Forms.Label();
            this.lbl_Cam_RL = new System.Windows.Forms.Label();
            this.lbl_Cam_RR = new System.Windows.Forms.Label();
            this.lbl_Toe_RR = new System.Windows.Forms.Label();
            this.lbl_Cam_FR = new System.Windows.Forms.Label();
            this.ResList = new System.Windows.Forms.ListView();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label9 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rd_bw = new System.Windows.Forms.RadioButton();
            this.rd_fw = new System.Windows.Forms.RadioButton();
            this.text_time = new System.Windows.Forms.TextBox();
            this.text_cnt = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lblMessage = new System.Windows.Forms.Label();
            this.Btn_Init = new System.Windows.Forms.Button();
            this.Btn_Save = new System.Windows.Forms.Button();
            this.pgs_runout = new System.Windows.Forms.ProgressBar();
            this.pgs_Idle = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbl_Toe_FL
            // 
            this.lbl_Toe_FL.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lbl_Toe_FL.Font = new System.Drawing.Font("Arial", 30F);
            this.lbl_Toe_FL.Location = new System.Drawing.Point(40, 170);
            this.lbl_Toe_FL.Name = "lbl_Toe_FL";
            this.lbl_Toe_FL.Size = new System.Drawing.Size(190, 53);
            this.lbl_Toe_FL.TabIndex = 18;
            this.lbl_Toe_FL.Text = "0.0";
            this.lbl_Toe_FL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Btn_Start
            // 
            this.Btn_Start.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_Start.Location = new System.Drawing.Point(864, 16);
            this.Btn_Start.Name = "Btn_Start";
            this.Btn_Start.Size = new System.Drawing.Size(126, 46);
            this.Btn_Start.TabIndex = 21;
            this.Btn_Start.Text = "Measure";
            this.Btn_Start.UseVisualStyleBackColor = true;
            this.Btn_Start.Click += new System.EventHandler(this.Btn_Start_Click);
            // 
            // lbl_Toe_FR
            // 
            this.lbl_Toe_FR.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lbl_Toe_FR.Font = new System.Drawing.Font("Arial", 30F);
            this.lbl_Toe_FR.Location = new System.Drawing.Point(779, 172);
            this.lbl_Toe_FR.Name = "lbl_Toe_FR";
            this.lbl_Toe_FR.Size = new System.Drawing.Size(190, 53);
            this.lbl_Toe_FR.TabIndex = 22;
            this.lbl_Toe_FR.Text = "0.0";
            this.lbl_Toe_FR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_Cam_FL
            // 
            this.lbl_Cam_FL.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lbl_Cam_FL.Font = new System.Drawing.Font("Arial", 30F);
            this.lbl_Cam_FL.Location = new System.Drawing.Point(40, 277);
            this.lbl_Cam_FL.Name = "lbl_Cam_FL";
            this.lbl_Cam_FL.Size = new System.Drawing.Size(190, 53);
            this.lbl_Cam_FL.TabIndex = 23;
            this.lbl_Cam_FL.Text = "0.0";
            this.lbl_Cam_FL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_Toe_RL
            // 
            this.lbl_Toe_RL.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lbl_Toe_RL.Font = new System.Drawing.Font("Arial", 30F);
            this.lbl_Toe_RL.Location = new System.Drawing.Point(40, 412);
            this.lbl_Toe_RL.Name = "lbl_Toe_RL";
            this.lbl_Toe_RL.Size = new System.Drawing.Size(190, 53);
            this.lbl_Toe_RL.TabIndex = 24;
            this.lbl_Toe_RL.Text = "0.0";
            this.lbl_Toe_RL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_Cam_RL
            // 
            this.lbl_Cam_RL.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lbl_Cam_RL.Font = new System.Drawing.Font("Arial", 30F);
            this.lbl_Cam_RL.Location = new System.Drawing.Point(40, 519);
            this.lbl_Cam_RL.Name = "lbl_Cam_RL";
            this.lbl_Cam_RL.Size = new System.Drawing.Size(190, 53);
            this.lbl_Cam_RL.TabIndex = 25;
            this.lbl_Cam_RL.Text = "0.0";
            this.lbl_Cam_RL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_Cam_RR
            // 
            this.lbl_Cam_RR.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lbl_Cam_RR.Font = new System.Drawing.Font("Arial", 30F);
            this.lbl_Cam_RR.Location = new System.Drawing.Point(779, 521);
            this.lbl_Cam_RR.Name = "lbl_Cam_RR";
            this.lbl_Cam_RR.Size = new System.Drawing.Size(190, 53);
            this.lbl_Cam_RR.TabIndex = 28;
            this.lbl_Cam_RR.Text = "0.0";
            this.lbl_Cam_RR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_Toe_RR
            // 
            this.lbl_Toe_RR.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lbl_Toe_RR.Font = new System.Drawing.Font("Arial", 30F);
            this.lbl_Toe_RR.Location = new System.Drawing.Point(779, 414);
            this.lbl_Toe_RR.Name = "lbl_Toe_RR";
            this.lbl_Toe_RR.Size = new System.Drawing.Size(190, 53);
            this.lbl_Toe_RR.TabIndex = 27;
            this.lbl_Toe_RR.Text = "0.0";
            this.lbl_Toe_RR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_Cam_FR
            // 
            this.lbl_Cam_FR.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lbl_Cam_FR.Font = new System.Drawing.Font("Arial", 30F);
            this.lbl_Cam_FR.Location = new System.Drawing.Point(779, 279);
            this.lbl_Cam_FR.Name = "lbl_Cam_FR";
            this.lbl_Cam_FR.Size = new System.Drawing.Size(190, 53);
            this.lbl_Cam_FR.TabIndex = 26;
            this.lbl_Cam_FR.Text = "0.0";
            this.lbl_Cam_FR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ResList
            // 
            this.ResList.Activation = System.Windows.Forms.ItemActivation.TwoClick;
            this.ResList.BackColor = System.Drawing.Color.Lavender;
            this.ResList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ResList.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ResList.FullRowSelect = true;
            this.ResList.GridLines = true;
            this.ResList.HideSelection = false;
            this.ResList.Location = new System.Drawing.Point(996, 77);
            this.ResList.MultiSelect = false;
            this.ResList.Name = "ResList";
            this.ResList.Size = new System.Drawing.Size(799, 574);
            this.ResList.TabIndex = 158;
            this.ResList.UseCompatibleStateImageBehavior = false;
            this.ResList.View = System.Windows.Forms.View.Details;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox2.Image = global::Ki_WAT.Properties.Resources.Handle;
            this.pictureBox2.Location = new System.Drawing.Point(332, 657);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(227, 218);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 160;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Ki_WAT.Properties.Resources.Rolling;
            this.pictureBox1.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.InitialImage")));
            this.pictureBox1.Location = new System.Drawing.Point(21, 77);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(969, 574);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 20;
            this.pictureBox1.TabStop = false;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label9.Font = new System.Drawing.Font("Arial", 30F);
            this.label9.Location = new System.Drawing.Point(351, 822);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(190, 50);
            this.label9.TabIndex = 161;
            this.label9.Text = "asdf";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(565, 719);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(126, 46);
            this.button3.TabIndex = 162;
            this.button3.Text = "Zero";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.Location = new System.Drawing.Point(565, 772);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(126, 46);
            this.button4.TabIndex = 163;
            this.button4.Text = "Sapn";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Arial Narrow", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(697, 772);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(142, 46);
            this.textBox1.TabIndex = 164;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rd_bw);
            this.groupBox1.Controls.Add(this.rd_fw);
            this.groupBox1.Font = new System.Drawing.Font("Arial Narrow", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(33, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(289, 50);
            this.groupBox1.TabIndex = 165;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Direction";
            // 
            // rd_bw
            // 
            this.rd_bw.AutoSize = true;
            this.rd_bw.Location = new System.Drawing.Point(152, 23);
            this.rd_bw.Name = "rd_bw";
            this.rd_bw.Size = new System.Drawing.Size(108, 29);
            this.rd_bw.TabIndex = 1;
            this.rd_bw.TabStop = true;
            this.rd_bw.Text = "Backward";
            this.rd_bw.UseVisualStyleBackColor = true;
            // 
            // rd_fw
            // 
            this.rd_fw.AutoSize = true;
            this.rd_fw.Location = new System.Drawing.Point(34, 20);
            this.rd_fw.Name = "rd_fw";
            this.rd_fw.Size = new System.Drawing.Size(96, 29);
            this.rd_fw.TabIndex = 0;
            this.rd_fw.TabStop = true;
            this.rd_fw.Text = "Forward";
            this.rd_fw.UseVisualStyleBackColor = true;
            // 
            // text_time
            // 
            this.text_time.Font = new System.Drawing.Font("Arial Narrow", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.text_time.Location = new System.Drawing.Point(758, 25);
            this.text_time.Multiline = true;
            this.text_time.Name = "text_time";
            this.text_time.Size = new System.Drawing.Size(100, 34);
            this.text_time.TabIndex = 167;
            // 
            // text_cnt
            // 
            this.text_cnt.Font = new System.Drawing.Font("Arial Narrow", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.text_cnt.Location = new System.Drawing.Point(516, 25);
            this.text_cnt.Multiline = true;
            this.text_cnt.Name = "text_cnt";
            this.text_cnt.Size = new System.Drawing.Size(100, 34);
            this.text_cnt.TabIndex = 168;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(354, 28);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(156, 23);
            this.label10.TabIndex = 169;
            this.label10.Text = "Number of repetitions";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.label11.Location = new System.Drawing.Point(629, 28);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(131, 23);
            this.label11.TabIndex = 170;
            this.label11.Text = "Run out time(sec)";
            // 
            // lblMessage
            // 
            this.lblMessage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.lblMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblMessage.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.Location = new System.Drawing.Point(996, 16);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(341, 50);
            this.lblMessage.TabIndex = 171;
            this.lblMessage.Text = "MSG";
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Btn_Init
            // 
            this.Btn_Init.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_Init.Location = new System.Drawing.Point(1686, 16);
            this.Btn_Init.Name = "Btn_Init";
            this.Btn_Init.Size = new System.Drawing.Size(109, 46);
            this.Btn_Init.TabIndex = 172;
            this.Btn_Init.Text = "Initial";
            this.Btn_Init.UseVisualStyleBackColor = true;
            this.Btn_Init.Click += new System.EventHandler(this.Btn_Init_Click);
            // 
            // Btn_Save
            // 
            this.Btn_Save.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_Save.Location = new System.Drawing.Point(1571, 16);
            this.Btn_Save.Name = "Btn_Save";
            this.Btn_Save.Size = new System.Drawing.Size(109, 46);
            this.Btn_Save.TabIndex = 173;
            this.Btn_Save.Text = "Save";
            this.Btn_Save.UseVisualStyleBackColor = true;
            // 
            // pgs_runout
            // 
            this.pgs_runout.Location = new System.Drawing.Point(1343, 39);
            this.pgs_runout.Name = "pgs_runout";
            this.pgs_runout.Size = new System.Drawing.Size(137, 27);
            this.pgs_runout.TabIndex = 174;
            // 
            // pgs_Idle
            // 
            this.pgs_Idle.Location = new System.Drawing.Point(1486, 39);
            this.pgs_Idle.Name = "pgs_Idle";
            this.pgs_Idle.Size = new System.Drawing.Size(79, 26);
            this.pgs_Idle.TabIndex = 175;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(1344, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 20);
            this.label1.TabIndex = 176;
            this.label1.Text = "RunOut";
            this.label1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lbl_1
            // 
            this.lbl_1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lbl_1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_1.Location = new System.Drawing.Point(1485, 16);
            this.lbl_1.Name = "lbl_1";
            this.lbl_1.Size = new System.Drawing.Size(80, 20);
            this.lbl_1.TabIndex = 177;
            this.lbl_1.Text = "Idle";
            this.lbl_1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // Frm_Rolling
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1815, 910);
            this.Controls.Add(this.lbl_1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pgs_Idle);
            this.Controls.Add(this.pgs_runout);
            this.Controls.Add(this.Btn_Save);
            this.Controls.Add(this.Btn_Init);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.text_cnt);
            this.Controls.Add(this.text_time);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.ResList);
            this.Controls.Add(this.lbl_Cam_RR);
            this.Controls.Add(this.lbl_Toe_RR);
            this.Controls.Add(this.lbl_Cam_FR);
            this.Controls.Add(this.lbl_Cam_RL);
            this.Controls.Add(this.lbl_Toe_RL);
            this.Controls.Add(this.lbl_Cam_FL);
            this.Controls.Add(this.lbl_Toe_FR);
            this.Controls.Add(this.Btn_Start);
            this.Controls.Add(this.lbl_Toe_FL);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Frm_Rolling";
            this.Text = " ";
            this.Load += new System.EventHandler(this.Frm_Rolling_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lbl_Toe_FL;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button Btn_Start;
        private System.Windows.Forms.Label lbl_Toe_FR;
        private System.Windows.Forms.Label lbl_Cam_FL;
        private System.Windows.Forms.Label lbl_Toe_RL;
        private System.Windows.Forms.Label lbl_Cam_RL;
        private System.Windows.Forms.Label lbl_Cam_RR;
        private System.Windows.Forms.Label lbl_Toe_RR;
        private System.Windows.Forms.Label lbl_Cam_FR;
        private System.Windows.Forms.ListView ResList;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rd_fw;
        private System.Windows.Forms.TextBox text_time;
        private System.Windows.Forms.TextBox text_cnt;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Button Btn_Init;
        private System.Windows.Forms.RadioButton rd_bw;
        private System.Windows.Forms.Button Btn_Save;
        private System.Windows.Forms.ProgressBar pgs_runout;
        private System.Windows.Forms.ProgressBar pgs_Idle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbl_1;
    }
}