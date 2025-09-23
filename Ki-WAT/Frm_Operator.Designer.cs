namespace Ki_WAT
{
    partial class Frm_Operator
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
            this.NavTop = new System.Windows.Forms.Panel();
            this.lbl_NextPJI = new KI_Controls.RoundLabel();
            this.analogClock1 = new Ki_WAT.AnalogClock();
            this.lbl_Hand = new KI_Controls.RoundLabel();
            this.lbl_Time = new KI_Controls.RoundLabel();
            this.lbl_Message = new KI_Controls.RoundLabel();
            this.lbl_PJI = new KI_Controls.RoundLabel();
            this.NavBottom = new System.Windows.Forms.Panel();
            this.NavTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // NavTop
            // 
            this.NavTop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NavTop.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.NavTop.Controls.Add(this.lbl_NextPJI);
            this.NavTop.Controls.Add(this.analogClock1);
            this.NavTop.Controls.Add(this.lbl_Hand);
            this.NavTop.Controls.Add(this.lbl_Time);
            this.NavTop.Controls.Add(this.lbl_Message);
            this.NavTop.Controls.Add(this.lbl_PJI);
            this.NavTop.Location = new System.Drawing.Point(0, 0);
            this.NavTop.Margin = new System.Windows.Forms.Padding(4);
            this.NavTop.Name = "NavTop";
            this.NavTop.Size = new System.Drawing.Size(1920, 146);
            this.NavTop.TabIndex = 7;
            this.NavTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.NavTop_MouseDown);
            this.NavTop.MouseMove += new System.Windows.Forms.MouseEventHandler(this.NavTop_MouseMove);
            // 
            // lbl_NextPJI
            // 
            this.lbl_NextPJI.BackColor = System.Drawing.Color.MidnightBlue;
            this.lbl_NextPJI.BackgroundColor = System.Drawing.Color.MidnightBlue;
            this.lbl_NextPJI.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.lbl_NextPJI.BorderRadius = 20;
            this.lbl_NextPJI.BorderSize = 0;
            this.lbl_NextPJI.FlatAppearance.BorderSize = 0;
            this.lbl_NextPJI.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbl_NextPJI.Font = new System.Drawing.Font("굴림", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_NextPJI.ForeColor = System.Drawing.Color.White;
            this.lbl_NextPJI.Location = new System.Drawing.Point(1061, 12);
            this.lbl_NextPJI.Name = "lbl_NextPJI";
            this.lbl_NextPJI.Size = new System.Drawing.Size(552, 54);
            this.lbl_NextPJI.TabIndex = 10;
            this.lbl_NextPJI.Text = "-";
            this.lbl_NextPJI.TextColor = System.Drawing.Color.White;
            this.lbl_NextPJI.UseVisualStyleBackColor = false;
            // 
            // analogClock1
            // 
            this.analogClock1.BackColor = System.Drawing.Color.White;
            this.analogClock1.BorderColor = System.Drawing.Color.LightCoral;
            this.analogClock1.BorderThickness = 3;
            this.analogClock1.HourHandColor = System.Drawing.Color.Black;
            this.analogClock1.Location = new System.Drawing.Point(35, 12);
            this.analogClock1.MinuteHandColor = System.Drawing.Color.Black;
            this.analogClock1.Name = "analogClock1";
            this.analogClock1.NumberFont = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.analogClock1.SecondHandColor = System.Drawing.Color.Red;
            this.analogClock1.Size = new System.Drawing.Size(187, 114);
            this.analogClock1.TabIndex = 4;
            this.analogClock1.Text = "analogClock1";
            // 
            // lbl_Hand
            // 
            this.lbl_Hand.BackColor = System.Drawing.Color.MidnightBlue;
            this.lbl_Hand.BackgroundColor = System.Drawing.Color.MidnightBlue;
            this.lbl_Hand.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.lbl_Hand.BorderRadius = 20;
            this.lbl_Hand.BorderSize = 0;
            this.lbl_Hand.FlatAppearance.BorderSize = 0;
            this.lbl_Hand.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbl_Hand.Font = new System.Drawing.Font("Arial", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Hand.ForeColor = System.Drawing.Color.White;
            this.lbl_Hand.Location = new System.Drawing.Point(250, 12);
            this.lbl_Hand.Name = "lbl_Hand";
            this.lbl_Hand.Size = new System.Drawing.Size(215, 114);
            this.lbl_Hand.TabIndex = 3;
            this.lbl_Hand.Text = "0.0";
            this.lbl_Hand.TextColor = System.Drawing.Color.White;
            this.lbl_Hand.UseVisualStyleBackColor = false;
            // 
            // lbl_Time
            // 
            this.lbl_Time.BackColor = System.Drawing.Color.Black;
            this.lbl_Time.BackgroundColor = System.Drawing.Color.Black;
            this.lbl_Time.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.lbl_Time.BorderRadius = 20;
            this.lbl_Time.BorderSize = 0;
            this.lbl_Time.FlatAppearance.BorderSize = 0;
            this.lbl_Time.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbl_Time.Font = new System.Drawing.Font("Arial", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Time.ForeColor = System.Drawing.Color.White;
            this.lbl_Time.Location = new System.Drawing.Point(1676, 12);
            this.lbl_Time.Name = "lbl_Time";
            this.lbl_Time.Size = new System.Drawing.Size(215, 114);
            this.lbl_Time.TabIndex = 2;
            this.lbl_Time.Text = "0";
            this.lbl_Time.TextColor = System.Drawing.Color.White;
            this.lbl_Time.UseVisualStyleBackColor = false;
            // 
            // lbl_Message
            // 
            this.lbl_Message.BackColor = System.Drawing.Color.LightSteelBlue;
            this.lbl_Message.BackgroundColor = System.Drawing.Color.LightSteelBlue;
            this.lbl_Message.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.lbl_Message.BorderRadius = 20;
            this.lbl_Message.BorderSize = 0;
            this.lbl_Message.FlatAppearance.BorderSize = 0;
            this.lbl_Message.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbl_Message.Font = new System.Drawing.Font("굴림", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_Message.ForeColor = System.Drawing.Color.White;
            this.lbl_Message.Location = new System.Drawing.Point(471, 72);
            this.lbl_Message.Name = "lbl_Message";
            this.lbl_Message.Size = new System.Drawing.Size(1142, 54);
            this.lbl_Message.TabIndex = 1;
            this.lbl_Message.Text = "-";
            this.lbl_Message.TextColor = System.Drawing.Color.White;
            this.lbl_Message.UseVisualStyleBackColor = false;
            // 
            // lbl_PJI
            // 
            this.lbl_PJI.BackColor = System.Drawing.Color.MidnightBlue;
            this.lbl_PJI.BackgroundColor = System.Drawing.Color.MidnightBlue;
            this.lbl_PJI.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.lbl_PJI.BorderRadius = 20;
            this.lbl_PJI.BorderSize = 0;
            this.lbl_PJI.FlatAppearance.BorderSize = 0;
            this.lbl_PJI.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbl_PJI.Font = new System.Drawing.Font("굴림", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_PJI.ForeColor = System.Drawing.Color.White;
            this.lbl_PJI.Location = new System.Drawing.Point(471, 12);
            this.lbl_PJI.Name = "lbl_PJI";
            this.lbl_PJI.Size = new System.Drawing.Size(552, 54);
            this.lbl_PJI.TabIndex = 0;
            this.lbl_PJI.Text = "-";
            this.lbl_PJI.TextColor = System.Drawing.Color.White;
            this.lbl_PJI.UseVisualStyleBackColor = false;
            // 
            // NavBottom
            // 
            this.NavBottom.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.NavBottom.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.NavBottom.Location = new System.Drawing.Point(0, 974);
            this.NavBottom.Margin = new System.Windows.Forms.Padding(4);
            this.NavBottom.Name = "NavBottom";
            this.NavBottom.Size = new System.Drawing.Size(1920, 104);
            this.NavBottom.TabIndex = 8;
            // 
            // Frm_Operator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1920, 1080);
            this.Controls.Add(this.NavBottom);
            this.Controls.Add(this.NavTop);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.IsMdiContainer = true;
            this.Name = "Frm_Operator";
            this.Text = "Frm_Operator";
            this.Load += new System.EventHandler(this.Frm_Operator_Load);
            this.Shown += new System.EventHandler(this.Frm_Operator_Shown);
            this.NavTop.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel NavTop;
        private System.Windows.Forms.Panel NavBottom;
        private KI_Controls.RoundLabel lbl_Hand;
        private KI_Controls.RoundLabel lbl_Time;
        private KI_Controls.RoundLabel lbl_Message;
        private KI_Controls.RoundLabel lbl_PJI;
        private AnalogClock analogClock1;
        private KI_Controls.RoundLabel lbl_NextPJI;
    }
}