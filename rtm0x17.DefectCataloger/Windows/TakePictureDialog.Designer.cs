namespace rtm0x17.DefectCataloger.Windows
{
    partial class TakePictureDialog
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
            this.PictureBoxArea = new System.Windows.Forms.PictureBox();
            this.ButtonTakePicture = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxArea)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // PictureBoxArea
            // 
            this.PictureBoxArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PictureBoxArea.Image = global::rtm0x17.DefectCataloger.Properties.Resources.logo;
            this.PictureBoxArea.Location = new System.Drawing.Point(3, 3);
            this.PictureBoxArea.Name = "PictureBoxArea";
            this.PictureBoxArea.Size = new System.Drawing.Size(794, 378);
            this.PictureBoxArea.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.PictureBoxArea.TabIndex = 0;
            this.PictureBoxArea.TabStop = false;
            // 
            // ButtonTakePicture
            // 
            this.ButtonTakePicture.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ButtonTakePicture.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ButtonTakePicture.Location = new System.Drawing.Point(3, 387);
            this.ButtonTakePicture.Name = "ButtonTakePicture";
            this.ButtonTakePicture.Size = new System.Drawing.Size(794, 60);
            this.ButtonTakePicture.TabIndex = 2;
            this.ButtonTakePicture.Text = "📸 Scatta foto";
            this.ButtonTakePicture.UseVisualStyleBackColor = true;
            this.ButtonTakePicture.Click += new System.EventHandler(this.ButtonTakePicture_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.ButtonTakePicture, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.PictureBoxArea, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 85.44601F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.55399F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(800, 450);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // TakePictureDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "TakePictureDialog";
            this.Text = "TakePictureDialog";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TakePictureDialog_FormClosing);
            this.Load += new System.EventHandler(this.TakePictureDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxArea)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox PictureBoxArea;
        private System.Windows.Forms.Button ButtonTakePicture;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}