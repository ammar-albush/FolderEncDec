namespace FolderEncDec
{
    partial class FolderDecrypterLog
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.log_textBox = new System.Windows.Forms.TextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.log_bgw = new System.ComponentModel.BackgroundWorker();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(2)))), ((int)(((byte)(2)))));
            this.panel1.Controls.Add(this.log_textBox);
            this.panel1.Controls.Add(this.progressBar1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(548, 450);
            this.panel1.TabIndex = 3;
            // 
            // log_textBox
            // 
            this.log_textBox.Location = new System.Drawing.Point(88, 119);
            this.log_textBox.Multiline = true;
            this.log_textBox.Name = "log_textBox";
            this.log_textBox.Size = new System.Drawing.Size(371, 233);
            this.log_textBox.TabIndex = 1;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(88, 67);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(371, 23);
            this.progressBar1.TabIndex = 0;
            // 
            // log_bgw
            // 
            this.log_bgw.WorkerReportsProgress = true;
            this.log_bgw.DoWork += new System.ComponentModel.DoWorkEventHandler(this.log_bgw_DoWork);
            this.log_bgw.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.log_bgw_ProgressChanged);
            this.log_bgw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.log_bgw_RunWorkerCompleted);
            // 
            // FolderDecrypterLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 450);
            this.Controls.Add(this.panel1);
            this.Name = "FolderDecrypterLog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FolderDecrypterLog";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Panel panel1;
        private TextBox log_textBox;
        private ProgressBar progressBar1;
        private System.ComponentModel.BackgroundWorker log_bgw;
    }
}