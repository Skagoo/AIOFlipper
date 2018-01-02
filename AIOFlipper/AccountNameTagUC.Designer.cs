namespace AIOFlipper
{
    partial class AccountNameTagUC
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelAccountName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelAccountName
            // 
            this.labelAccountName.Font = new System.Drawing.Font("Kingthings Petrock", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAccountName.ForeColor = System.Drawing.Color.Black;
            this.labelAccountName.Image = global::AIOFlipper.Properties.Resources.thBack;
            this.labelAccountName.Location = new System.Drawing.Point(0, 0);
            this.labelAccountName.Name = "labelAccountName";
            this.labelAccountName.Size = new System.Drawing.Size(300, 60);
            this.labelAccountName.TabIndex = 0;
            this.labelAccountName.Text = "Skagoo";
            this.labelAccountName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AccountNameTagUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Controls.Add(this.labelAccountName);
            this.DoubleBuffered = true;
            this.Name = "AccountNameTagUC";
            this.Size = new System.Drawing.Size(300, 60);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelAccountName;
    }
}
