namespace AIOFlipper
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.buttonMinimizeForm = new System.Windows.Forms.Button();
            this.buttonCloseForm = new System.Windows.Forms.Button();
            this.buttonSettings = new System.Windows.Forms.Button();
            this.buttonDashboard = new System.Windows.Forms.Button();
            this.buttonAccounts = new System.Windows.Forms.Button();
            this.buttonMenu = new System.Windows.Forms.Button();
            this.timerMenuSlideInOut = new System.Windows.Forms.Timer(this.components);
            this.pictureBoxNavTab1 = new System.Windows.Forms.PictureBox();
            this.pictureBoxNavTab2 = new System.Windows.Forms.PictureBox();
            this.accountBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxNavTab1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxNavTab2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.accountBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonMinimizeForm
            // 
            this.buttonMinimizeForm.BackColor = System.Drawing.Color.Transparent;
            this.buttonMinimizeForm.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonMinimizeForm.FlatAppearance.BorderSize = 0;
            this.buttonMinimizeForm.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonMinimizeForm.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonMinimizeForm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonMinimizeForm.Location = new System.Drawing.Point(1773, 12);
            this.buttonMinimizeForm.Name = "buttonMinimizeForm";
            this.buttonMinimizeForm.Size = new System.Drawing.Size(30, 35);
            this.buttonMinimizeForm.TabIndex = 0;
            this.buttonMinimizeForm.UseVisualStyleBackColor = false;
            this.buttonMinimizeForm.Click += new System.EventHandler(this.buttonMinimizeForm_Click);
            // 
            // buttonCloseForm
            // 
            this.buttonCloseForm.BackColor = System.Drawing.Color.Transparent;
            this.buttonCloseForm.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonCloseForm.FlatAppearance.BorderSize = 0;
            this.buttonCloseForm.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonCloseForm.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonCloseForm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCloseForm.Location = new System.Drawing.Point(1811, 12);
            this.buttonCloseForm.Name = "buttonCloseForm";
            this.buttonCloseForm.Size = new System.Drawing.Size(30, 35);
            this.buttonCloseForm.TabIndex = 1;
            this.buttonCloseForm.UseVisualStyleBackColor = false;
            this.buttonCloseForm.Click += new System.EventHandler(this.buttonCloseForm_Click);
            // 
            // buttonSettings
            // 
            this.buttonSettings.AutoSize = true;
            this.buttonSettings.BackColor = System.Drawing.Color.Transparent;
            this.buttonSettings.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonSettings.FlatAppearance.BorderSize = 0;
            this.buttonSettings.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonSettings.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSettings.Image = global::AIOFlipper.Properties.Resources.Settings;
            this.buttonSettings.Location = new System.Drawing.Point(-100, 1314);
            this.buttonSettings.Name = "buttonSettings";
            this.buttonSettings.Size = new System.Drawing.Size(200, 121);
            this.buttonSettings.TabIndex = 2;
            this.buttonSettings.Tag = "814;1314";
            this.buttonSettings.UseVisualStyleBackColor = false;
            this.buttonSettings.Click += new System.EventHandler(this.buttonSettings_Click);
            // 
            // buttonDashboard
            // 
            this.buttonDashboard.AutoSize = true;
            this.buttonDashboard.BackColor = System.Drawing.Color.Transparent;
            this.buttonDashboard.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonDashboard.FlatAppearance.BorderSize = 0;
            this.buttonDashboard.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonDashboard.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonDashboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDashboard.Image = global::AIOFlipper.Properties.Resources.Dashboard;
            this.buttonDashboard.Location = new System.Drawing.Point(-100, 1187);
            this.buttonDashboard.Name = "buttonDashboard";
            this.buttonDashboard.Size = new System.Drawing.Size(200, 121);
            this.buttonDashboard.TabIndex = 3;
            this.buttonDashboard.Tag = "687;1187";
            this.buttonDashboard.UseVisualStyleBackColor = false;
            // 
            // buttonAccounts
            // 
            this.buttonAccounts.AutoSize = true;
            this.buttonAccounts.BackColor = System.Drawing.Color.Transparent;
            this.buttonAccounts.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonAccounts.FlatAppearance.BorderSize = 0;
            this.buttonAccounts.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonAccounts.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonAccounts.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAccounts.Image = global::AIOFlipper.Properties.Resources.Accounts;
            this.buttonAccounts.Location = new System.Drawing.Point(-100, 1060);
            this.buttonAccounts.Name = "buttonAccounts";
            this.buttonAccounts.Size = new System.Drawing.Size(200, 121);
            this.buttonAccounts.TabIndex = 4;
            this.buttonAccounts.Tag = "560;1060";
            this.buttonAccounts.UseVisualStyleBackColor = false;
            this.buttonAccounts.Click += new System.EventHandler(this.buttonAccounts_Click);
            // 
            // buttonMenu
            // 
            this.buttonMenu.BackColor = System.Drawing.Color.Transparent;
            this.buttonMenu.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonMenu.FlatAppearance.BorderSize = 0;
            this.buttonMenu.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonMenu.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonMenu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonMenu.Location = new System.Drawing.Point(14, 941);
            this.buttonMenu.Name = "buttonMenu";
            this.buttonMenu.Size = new System.Drawing.Size(100, 90);
            this.buttonMenu.TabIndex = 5;
            this.buttonMenu.UseVisualStyleBackColor = false;
            this.buttonMenu.Click += new System.EventHandler(this.buttonMenu_Click);
            // 
            // timerMenuSlideInOut
            // 
            this.timerMenuSlideInOut.Interval = 1;
            this.timerMenuSlideInOut.Tag = "0";
            this.timerMenuSlideInOut.Tick += new System.EventHandler(this.timerMenuSlideInOut_Tick);
            // 
            // pictureBoxNavTab1
            // 
            this.pictureBoxNavTab1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxNavTab1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxNavTab1.Image = global::AIOFlipper.Properties.Resources.radio_checked;
            this.pictureBoxNavTab1.Location = new System.Drawing.Point(890, 995);
            this.pictureBoxNavTab1.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBoxNavTab1.Name = "pictureBoxNavTab1";
            this.pictureBoxNavTab1.Size = new System.Drawing.Size(36, 36);
            this.pictureBoxNavTab1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxNavTab1.TabIndex = 6;
            this.pictureBoxNavTab1.TabStop = false;
            this.pictureBoxNavTab1.Tag = "1";
            this.pictureBoxNavTab1.Visible = false;
            this.pictureBoxNavTab1.Click += new System.EventHandler(this.pictureBoxNavTab1_Click);
            // 
            // pictureBoxNavTab2
            // 
            this.pictureBoxNavTab2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxNavTab2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxNavTab2.Image = global::AIOFlipper.Properties.Resources.radio;
            this.pictureBoxNavTab2.Location = new System.Drawing.Point(995, 995);
            this.pictureBoxNavTab2.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBoxNavTab2.Name = "pictureBoxNavTab2";
            this.pictureBoxNavTab2.Size = new System.Drawing.Size(36, 36);
            this.pictureBoxNavTab2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxNavTab2.TabIndex = 7;
            this.pictureBoxNavTab2.TabStop = false;
            this.pictureBoxNavTab2.Tag = "0";
            this.pictureBoxNavTab2.Visible = false;
            this.pictureBoxNavTab2.Click += new System.EventHandler(this.pictureBoxNavTab2_Click);
            // 
            // accountBindingSource
            // 
            this.accountBindingSource.DataSource = typeof(AIOFlipper.Account);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::AIOFlipper.Properties.Resources.FrameTitle___Copy;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1920, 1050);
            this.Controls.Add(this.pictureBoxNavTab2);
            this.Controls.Add(this.pictureBoxNavTab1);
            this.Controls.Add(this.buttonMenu);
            this.Controls.Add(this.buttonAccounts);
            this.Controls.Add(this.buttonDashboard);
            this.Controls.Add(this.buttonSettings);
            this.Controls.Add(this.buttonCloseForm);
            this.Controls.Add(this.buttonMinimizeForm);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1920, 1050);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1918, 1048);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AIOFlipper";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxNavTab1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxNavTab2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.accountBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonMinimizeForm;
        private System.Windows.Forms.Button buttonCloseForm;
        private System.Windows.Forms.Button buttonSettings;
        private System.Windows.Forms.Button buttonDashboard;
        private System.Windows.Forms.Button buttonAccounts;
        private System.Windows.Forms.Button buttonMenu;
        private System.Windows.Forms.Timer timerMenuSlideInOut;
        private System.Windows.Forms.PictureBox pictureBoxNavTab1;
        private System.Windows.Forms.PictureBox pictureBoxNavTab2;
        private System.Windows.Forms.BindingSource accountBindingSource;
    }
}

