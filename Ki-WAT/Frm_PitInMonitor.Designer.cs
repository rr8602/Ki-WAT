namespace Ki_WAT
{
    partial class Frm_PitInMonitor
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
            this.analogClock1 = new Ki_WAT.AnalogClock();
            this.roundLabel9 = new KI_Controls.RoundLabel();
            this.lbl_Time = new KI_Controls.RoundLabel();
            this.lbl_Message = new KI_Controls.RoundLabel();
            this.roundLabel10 = new KI_Controls.RoundLabel();
            this.button1 = new System.Windows.Forms.Button();
            this.Txt_Left = new System.Windows.Forms.TextBox();
            this.roundLabel1 = new KI_Controls.RoundLabel();
            this.lbl_note = new KI_Controls.RoundLabel();
            this.lbl_Right_Value = new KI_Controls.RoundLabel();
            this.Bar_Right = new RollTester.SlideBarGauge();
            this.roundLabel4 = new KI_Controls.RoundLabel();
            this.lbl_Left_Value = new KI_Controls.RoundLabel();
            this.Bar_Left = new RollTester.SlideBarGauge();
            this.NavTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // NavTop
            // 
            this.NavTop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NavTop.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.NavTop.Controls.Add(this.analogClock1);
            this.NavTop.Controls.Add(this.roundLabel9);
            this.NavTop.Controls.Add(this.lbl_Time);
            this.NavTop.Controls.Add(this.lbl_Message);
            this.NavTop.Controls.Add(this.roundLabel10);
            this.NavTop.Location = new System.Drawing.Point(2, 0);
            this.NavTop.Margin = new System.Windows.Forms.Padding(4);
            this.NavTop.Name = "NavTop";
            this.NavTop.Size = new System.Drawing.Size(1920, 146);
            this.NavTop.TabIndex = 11;
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
            // roundLabel9
            // 
            this.roundLabel9.BackColor = System.Drawing.Color.MidnightBlue;
            this.roundLabel9.BackgroundColor = System.Drawing.Color.MidnightBlue;
            this.roundLabel9.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.roundLabel9.BorderRadius = 20;
            this.roundLabel9.BorderSize = 0;
            this.roundLabel9.FlatAppearance.BorderSize = 0;
            this.roundLabel9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.roundLabel9.Font = new System.Drawing.Font("Arial", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.roundLabel9.ForeColor = System.Drawing.Color.White;
            this.roundLabel9.Location = new System.Drawing.Point(250, 12);
            this.roundLabel9.Name = "roundLabel9";
            this.roundLabel9.Size = new System.Drawing.Size(215, 114);
            this.roundLabel9.TabIndex = 3;
            this.roundLabel9.Text = "0.0";
            this.roundLabel9.TextColor = System.Drawing.Color.White;
            this.roundLabel9.UseVisualStyleBackColor = false;
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
            this.lbl_Message.Text = "ENSÁIO PARALELISMO";
            this.lbl_Message.TextColor = System.Drawing.Color.White;
            this.lbl_Message.UseVisualStyleBackColor = false;
            // 
            // roundLabel10
            // 
            this.roundLabel10.BackColor = System.Drawing.Color.MidnightBlue;
            this.roundLabel10.BackgroundColor = System.Drawing.Color.MidnightBlue;
            this.roundLabel10.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.roundLabel10.BorderRadius = 20;
            this.roundLabel10.BorderSize = 0;
            this.roundLabel10.FlatAppearance.BorderSize = 0;
            this.roundLabel10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.roundLabel10.Font = new System.Drawing.Font("굴림", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.roundLabel10.ForeColor = System.Drawing.Color.White;
            this.roundLabel10.Location = new System.Drawing.Point(471, 12);
            this.roundLabel10.Name = "roundLabel10";
            this.roundLabel10.Size = new System.Drawing.Size(1142, 54);
            this.roundLabel10.TabIndex = 0;
            this.roundLabel10.Text = "-";
            this.roundLabel10.TextColor = System.Drawing.Color.White;
            this.roundLabel10.UseVisualStyleBackColor = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(473, 674);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(111, 68);
            this.button1.TabIndex = 14;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Txt_Left
            // 
            this.Txt_Left.Location = new System.Drawing.Point(310, 699);
            this.Txt_Left.Name = "Txt_Left";
            this.Txt_Left.Size = new System.Drawing.Size(100, 21);
            this.Txt_Left.TabIndex = 15;
            this.Txt_Left.Visible = false;
            // 
            // roundLabel1
            // 
            this.roundLabel1.BackColor = System.Drawing.Color.Silver;
            this.roundLabel1.BackgroundColor = System.Drawing.Color.Silver;
            this.roundLabel1.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.roundLabel1.BorderRadius = 20;
            this.roundLabel1.BorderSize = 0;
            this.roundLabel1.FlatAppearance.BorderSize = 0;
            this.roundLabel1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.roundLabel1.Font = new System.Drawing.Font("Arial", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.roundLabel1.ForeColor = System.Drawing.Color.Black;
            this.roundLabel1.Location = new System.Drawing.Point(1008, 404);
            this.roundLabel1.Name = "roundLabel1";
            this.roundLabel1.Size = new System.Drawing.Size(816, 68);
            this.roundLabel1.TabIndex = 13;
            this.roundLabel1.Text = "RIGHT";
            this.roundLabel1.TextColor = System.Drawing.Color.Black;
            this.roundLabel1.UseVisualStyleBackColor = false;
            // 
            // lbl_note
            // 
            this.lbl_note.BackColor = System.Drawing.Color.MidnightBlue;
            this.lbl_note.BackgroundColor = System.Drawing.Color.MidnightBlue;
            this.lbl_note.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.lbl_note.BorderRadius = 20;
            this.lbl_note.BorderSize = 0;
            this.lbl_note.FlatAppearance.BorderSize = 0;
            this.lbl_note.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbl_note.Font = new System.Drawing.Font("Arial", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_note.ForeColor = System.Drawing.Color.White;
            this.lbl_note.Location = new System.Drawing.Point(73, 176);
            this.lbl_note.Name = "lbl_note";
            this.lbl_note.Size = new System.Drawing.Size(1762, 135);
            this.lbl_note.TabIndex = 12;
            this.lbl_note.Text = "AJUSTE PARA. ESQ., APLICAR TORQUÍMETRO";
            this.lbl_note.TextColor = System.Drawing.Color.White;
            this.lbl_note.UseVisualStyleBackColor = false;
            // 
            // lbl_Right_Value
            // 
            this.lbl_Right_Value.BackColor = System.Drawing.Color.Yellow;
            this.lbl_Right_Value.BackgroundColor = System.Drawing.Color.Yellow;
            this.lbl_Right_Value.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.lbl_Right_Value.BorderRadius = 20;
            this.lbl_Right_Value.BorderSize = 0;
            this.lbl_Right_Value.FlatAppearance.BorderSize = 0;
            this.lbl_Right_Value.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbl_Right_Value.Font = new System.Drawing.Font("Arial", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Right_Value.ForeColor = System.Drawing.Color.Black;
            this.lbl_Right_Value.Location = new System.Drawing.Point(1341, 586);
            this.lbl_Right_Value.Name = "lbl_Right_Value";
            this.lbl_Right_Value.Size = new System.Drawing.Size(147, 68);
            this.lbl_Right_Value.TabIndex = 9;
            this.lbl_Right_Value.Text = "99.9\'";
            this.lbl_Right_Value.TextColor = System.Drawing.Color.Black;
            this.lbl_Right_Value.UseVisualStyleBackColor = false;
            // 
            // Bar_Right
            // 
            this.Bar_Right.BarAdjustColor = System.Drawing.Color.Green;
            this.Bar_Right.BarBackColor = System.Drawing.Color.DeepPink;
            this.Bar_Right.BarEvaluationColor = System.Drawing.Color.Yellow;
            this.Bar_Right.BarHeight = 50F;
            this.Bar_Right.bTarget = true;
            this.Bar_Right.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Bar_Right.Location = new System.Drawing.Point(964, 445);
            this.Bar_Right.Maximum = 60F;
            this.Bar_Right.Minimum = -60F;
            this.Bar_Right.Name = "Bar_Right";
            this.Bar_Right.Size = new System.Drawing.Size(904, 181);
            this.Bar_Right.TabIndex = 6;
            this.Bar_Right.Text = "slideBarGauge2";
            this.Bar_Right.ThumbBorderColor = System.Drawing.Color.Black;
            this.Bar_Right.ThumbColor = System.Drawing.Color.White;
            this.Bar_Right.TickColor = System.Drawing.Color.Black;
            this.Bar_Right.Tolerance = 10;
            this.Bar_Right.Value = 0F;
            // 
            // roundLabel4
            // 
            this.roundLabel4.BackColor = System.Drawing.Color.Silver;
            this.roundLabel4.BackgroundColor = System.Drawing.Color.Silver;
            this.roundLabel4.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.roundLabel4.BorderRadius = 20;
            this.roundLabel4.BorderSize = 0;
            this.roundLabel4.FlatAppearance.BorderSize = 0;
            this.roundLabel4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.roundLabel4.Font = new System.Drawing.Font("Arial", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.roundLabel4.ForeColor = System.Drawing.Color.Black;
            this.roundLabel4.Location = new System.Drawing.Point(77, 404);
            this.roundLabel4.Name = "roundLabel4";
            this.roundLabel4.Size = new System.Drawing.Size(816, 68);
            this.roundLabel4.TabIndex = 5;
            this.roundLabel4.Text = "LEFT";
            this.roundLabel4.TextColor = System.Drawing.Color.Black;
            this.roundLabel4.UseVisualStyleBackColor = false;
            // 
            // lbl_Left_Value
            // 
            this.lbl_Left_Value.BackColor = System.Drawing.Color.Lime;
            this.lbl_Left_Value.BackgroundColor = System.Drawing.Color.Lime;
            this.lbl_Left_Value.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.lbl_Left_Value.BorderRadius = 20;
            this.lbl_Left_Value.BorderSize = 0;
            this.lbl_Left_Value.FlatAppearance.BorderSize = 0;
            this.lbl_Left_Value.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbl_Left_Value.Font = new System.Drawing.Font("Arial", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Left_Value.ForeColor = System.Drawing.Color.Black;
            this.lbl_Left_Value.Location = new System.Drawing.Point(408, 600);
            this.lbl_Left_Value.Name = "lbl_Left_Value";
            this.lbl_Left_Value.Size = new System.Drawing.Size(147, 68);
            this.lbl_Left_Value.TabIndex = 4;
            this.lbl_Left_Value.Text = "99.9\'";
            this.lbl_Left_Value.TextColor = System.Drawing.Color.Black;
            this.lbl_Left_Value.UseVisualStyleBackColor = false;
            // 
            // Bar_Left
            // 
            this.Bar_Left.BarAdjustColor = System.Drawing.Color.Green;
            this.Bar_Left.BarBackColor = System.Drawing.Color.DeepPink;
            this.Bar_Left.BarEvaluationColor = System.Drawing.Color.Yellow;
            this.Bar_Left.BarHeight = 50F;
            this.Bar_Left.bTarget = true;
            this.Bar_Left.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Bar_Left.Location = new System.Drawing.Point(31, 445);
            this.Bar_Left.Maximum = 60F;
            this.Bar_Left.Minimum = -60F;
            this.Bar_Left.Name = "Bar_Left";
            this.Bar_Left.Size = new System.Drawing.Size(904, 181);
            this.Bar_Left.TabIndex = 0;
            this.Bar_Left.Text = "slideBarGauge1";
            this.Bar_Left.ThumbBorderColor = System.Drawing.Color.Black;
            this.Bar_Left.ThumbColor = System.Drawing.Color.White;
            this.Bar_Left.TickColor = System.Drawing.Color.Black;
            this.Bar_Left.Tolerance = 10;
            this.Bar_Left.Value = 0F;
            // 
            // Frm_PitInMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1904, 1041);
            this.Controls.Add(this.Txt_Left);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.roundLabel1);
            this.Controls.Add(this.lbl_note);
            this.Controls.Add(this.NavTop);
            this.Controls.Add(this.lbl_Right_Value);
            this.Controls.Add(this.Bar_Right);
            this.Controls.Add(this.roundLabel4);
            this.Controls.Add(this.lbl_Left_Value);
            this.Controls.Add(this.Bar_Left);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Frm_PitInMonitor";
            this.Text = "Frm_PitInMonitor";
            this.NavTop.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private RollTester.SlideBarGauge Bar_Left;
        private KI_Controls.RoundLabel lbl_Left_Value;
        private KI_Controls.RoundLabel roundLabel4;
        private KI_Controls.RoundLabel lbl_Right_Value;
        private RollTester.SlideBarGauge Bar_Right;
        private System.Windows.Forms.Panel NavTop;
        private AnalogClock analogClock1;
        private KI_Controls.RoundLabel roundLabel9;
        private KI_Controls.RoundLabel lbl_Time;
        private KI_Controls.RoundLabel lbl_Message;
        private KI_Controls.RoundLabel roundLabel10;
        private KI_Controls.RoundLabel lbl_note;
        private KI_Controls.RoundLabel roundLabel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox Txt_Left;
    }
}