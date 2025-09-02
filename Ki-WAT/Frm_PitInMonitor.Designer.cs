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
            this.slideBarGauge1 = new RollTester.SlideBarGauge();
            this.SuspendLayout();
            // 
            // slideBarGauge1
            // 
            this.slideBarGauge1.BarAdjustColor = System.Drawing.Color.Green;
            this.slideBarGauge1.BarBackColor = System.Drawing.Color.DeepPink;
            this.slideBarGauge1.BarEvaluationColor = System.Drawing.Color.Yellow;
            this.slideBarGauge1.BarHeight = 50F;
            this.slideBarGauge1.bTarget = true;
            this.slideBarGauge1.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.slideBarGauge1.Location = new System.Drawing.Point(377, 118);
            this.slideBarGauge1.Maximum = 60F;
            this.slideBarGauge1.Minimum = -60F;
            this.slideBarGauge1.Name = "slideBarGauge1";
            this.slideBarGauge1.Size = new System.Drawing.Size(1087, 191);
            this.slideBarGauge1.TabIndex = 0;
            this.slideBarGauge1.Text = "slideBarGauge1";
            this.slideBarGauge1.ThumbBorderColor = System.Drawing.Color.Black;
            this.slideBarGauge1.ThumbColor = System.Drawing.Color.White;
            this.slideBarGauge1.TickColor = System.Drawing.Color.Black;
            this.slideBarGauge1.Tolerance = 10;
            this.slideBarGauge1.Value = 35F;
            // 
            // Frm_PitInMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1904, 1041);
            this.Controls.Add(this.slideBarGauge1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Frm_PitInMonitor";
            this.Text = "Frm_PitInMonitor";
            this.ResumeLayout(false);

        }

        #endregion

        private RollTester.SlideBarGauge slideBarGauge1;
    }
}