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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_Operator));
            this.NavTop = new System.Windows.Forms.Panel();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.NavBottom = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.NavTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // NavTop
            // 
            this.NavTop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NavTop.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.NavTop.Controls.Add(this.label9);
            this.NavTop.Controls.Add(this.picLogo);
            this.NavTop.Location = new System.Drawing.Point(0, 0);
            this.NavTop.Margin = new System.Windows.Forms.Padding(4);
            this.NavTop.Name = "NavTop";
            this.NavTop.Size = new System.Drawing.Size(1920, 159);
            this.NavTop.TabIndex = 7;
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
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.MidnightBlue;
            this.label9.Font = new System.Drawing.Font("Arial", 30F);
            this.label9.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label9.Location = new System.Drawing.Point(177, 50);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(1157, 53);
            this.label9.TabIndex = 550;
            this.label9.Text = "-";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.NavTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel NavTop;
        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.Panel NavBottom;
        private System.Windows.Forms.Label label9;
    }
}