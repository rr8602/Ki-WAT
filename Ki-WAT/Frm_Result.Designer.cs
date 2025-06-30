namespace Ki_WAT
{
    partial class Frm_Result
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.seqList = new System.Windows.Forms.ListView();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(32, 23);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1654, 126);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // seqList
            // 
            this.seqList.Activation = System.Windows.Forms.ItemActivation.TwoClick;
            this.seqList.BackColor = System.Drawing.Color.SeaShell;
            this.seqList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.seqList.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.seqList.FullRowSelect = true;
            this.seqList.GridLines = true;
            this.seqList.HideSelection = false;
            this.seqList.Location = new System.Drawing.Point(32, 172);
            this.seqList.MultiSelect = false;
            this.seqList.Name = "seqList";
            this.seqList.Size = new System.Drawing.Size(554, 675);
            this.seqList.TabIndex = 158;
            this.seqList.UseCompatibleStateImageBehavior = false;
            this.seqList.View = System.Windows.Forms.View.Details;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Ki_WAT.Properties.Resources.Res;
            this.pictureBox1.Location = new System.Drawing.Point(592, 172);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1095, 675);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(618, 257);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(165, 50);
            this.label1.TabIndex = 159;
            this.label1.Text = "label1";
            // 
            // Frm_Result
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1762, 903);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.seqList);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Frm_Result";
            this.Text = "Frm_Result";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView seqList;
        private System.Windows.Forms.Label label1;
    }
}