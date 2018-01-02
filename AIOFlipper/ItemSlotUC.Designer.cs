namespace AIOFlipper
{
    partial class ItemSlotUC
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
            this.pictureBoxItemIcon = new System.Windows.Forms.PictureBox();
            this.labelItemName = new System.Windows.Forms.Label();
            this.labelItemPriceBuy = new System.Windows.Forms.Label();
            this.labelItemPriceSell = new System.Windows.Forms.Label();
            this.labelItemPriceCurrent = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxItemIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxItemIcon
            // 
            this.pictureBoxItemIcon.InitialImage = null;
            this.pictureBoxItemIcon.Location = new System.Drawing.Point(0, 15);
            this.pictureBoxItemIcon.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBoxItemIcon.Name = "pictureBoxItemIcon";
            this.pictureBoxItemIcon.Size = new System.Drawing.Size(257, 75);
            this.pictureBoxItemIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxItemIcon.TabIndex = 0;
            this.pictureBoxItemIcon.TabStop = false;
            // 
            // labelItemName
            // 
            this.labelItemName.Font = new System.Drawing.Font("Kingthings Petrock", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelItemName.ForeColor = System.Drawing.Color.LightGray;
            this.labelItemName.Location = new System.Drawing.Point(0, 100);
            this.labelItemName.Margin = new System.Windows.Forms.Padding(0);
            this.labelItemName.Name = "labelItemName";
            this.labelItemName.Size = new System.Drawing.Size(257, 23);
            this.labelItemName.TabIndex = 1;
            this.labelItemName.Text = "Third-age platebody";
            this.labelItemName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelItemPriceBuy
            // 
            this.labelItemPriceBuy.Font = new System.Drawing.Font("Kingthings Petrock", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelItemPriceBuy.ForeColor = System.Drawing.Color.LightGray;
            this.labelItemPriceBuy.Location = new System.Drawing.Point(31, 186);
            this.labelItemPriceBuy.Margin = new System.Windows.Forms.Padding(0);
            this.labelItemPriceBuy.Name = "labelItemPriceBuy";
            this.labelItemPriceBuy.Size = new System.Drawing.Size(195, 23);
            this.labelItemPriceBuy.TabIndex = 2;
            this.labelItemPriceBuy.Text = "Buy price: 330.000.000";
            this.labelItemPriceBuy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelItemPriceSell
            // 
            this.labelItemPriceSell.Font = new System.Drawing.Font("Kingthings Petrock", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelItemPriceSell.ForeColor = System.Drawing.Color.LightGray;
            this.labelItemPriceSell.Location = new System.Drawing.Point(31, 223);
            this.labelItemPriceSell.Margin = new System.Windows.Forms.Padding(0);
            this.labelItemPriceSell.Name = "labelItemPriceSell";
            this.labelItemPriceSell.Size = new System.Drawing.Size(195, 23);
            this.labelItemPriceSell.TabIndex = 3;
            this.labelItemPriceSell.Text = "Sell price: 332.275.000";
            this.labelItemPriceSell.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelItemPriceCurrent
            // 
            this.labelItemPriceCurrent.Font = new System.Drawing.Font("Kingthings Petrock", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelItemPriceCurrent.ForeColor = System.Drawing.Color.LightGray;
            this.labelItemPriceCurrent.Location = new System.Drawing.Point(31, 149);
            this.labelItemPriceCurrent.Margin = new System.Windows.Forms.Padding(0);
            this.labelItemPriceCurrent.Name = "labelItemPriceCurrent";
            this.labelItemPriceCurrent.Size = new System.Drawing.Size(195, 23);
            this.labelItemPriceCurrent.TabIndex = 4;
            this.labelItemPriceCurrent.Text = "Buying: 330.000.000";
            this.labelItemPriceCurrent.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ItemSlotUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImage = global::AIOFlipper.Properties.Resources.itemFrame;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Controls.Add(this.labelItemPriceCurrent);
            this.Controls.Add(this.labelItemPriceSell);
            this.Controls.Add(this.labelItemPriceBuy);
            this.Controls.Add(this.labelItemName);
            this.Controls.Add(this.pictureBoxItemIcon);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ItemSlotUC";
            this.Size = new System.Drawing.Size(257, 320);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxItemIcon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxItemIcon;
        private System.Windows.Forms.Label labelItemName;
        private System.Windows.Forms.Label labelItemPriceBuy;
        private System.Windows.Forms.Label labelItemPriceSell;
        private System.Windows.Forms.Label labelItemPriceCurrent;
    }
}
