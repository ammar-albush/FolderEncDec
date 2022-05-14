namespace FolderEncDec
{
    partial class Encryption_Options
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
            this.Save = new System.Windows.Forms.Button();
            this.EFEWCU_checkbox = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.open_folder_button = new System.Windows.Forms.Button();
            this.Target_Folder_button = new System.Windows.Forms.Button();
            this.Extention_textBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(2)))), ((int)(((byte)(2)))));
            this.panel1.Controls.Add(this.Save);
            this.panel1.Controls.Add(this.EFEWCU_checkbox);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.open_folder_button);
            this.panel1.Controls.Add(this.Target_Folder_button);
            this.panel1.Controls.Add(this.Extention_textBox);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 450);
            this.panel1.TabIndex = 0;
            // 
            // Save
            // 
            this.Save.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(58)))), ((int)(((byte)(57)))));
            this.Save.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Save.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Save.Font = new System.Drawing.Font("Leelawadee UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.Save.ForeColor = System.Drawing.Color.Teal;
            this.Save.Location = new System.Drawing.Point(537, 314);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(151, 39);
            this.Save.TabIndex = 7;
            this.Save.Text = "Save";
            this.Save.UseVisualStyleBackColor = false;
            this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // EFEWCU_checkbox
            // 
            this.EFEWCU_checkbox.AutoSize = true;
            this.EFEWCU_checkbox.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.EFEWCU_checkbox.ForeColor = System.Drawing.Color.Teal;
            this.EFEWCU_checkbox.Location = new System.Drawing.Point(212, 214);
            this.EFEWCU_checkbox.Name = "EFEWCU_checkbox";
            this.EFEWCU_checkbox.Size = new System.Drawing.Size(377, 29);
            this.EFEWCU_checkbox.TabIndex = 6;
            this.EFEWCU_checkbox.Text = "Encrypt File Encrypter With Current User";
            this.EFEWCU_checkbox.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.ForeColor = System.Drawing.Color.Teal;
            this.label2.Location = new System.Drawing.Point(57, 128);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(149, 25);
            this.label2.TabIndex = 5;
            this.label2.Text = "File Encrypter  : ";
            // 
            // open_folder_button
            // 
            this.open_folder_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(2)))), ((int)(((byte)(2)))));
            this.open_folder_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.open_folder_button.ForeColor = System.Drawing.Color.Teal;
            this.open_folder_button.Location = new System.Drawing.Point(661, 120);
            this.open_folder_button.Name = "open_folder_button";
            this.open_folder_button.Size = new System.Drawing.Size(27, 42);
            this.open_folder_button.TabIndex = 4;
            this.open_folder_button.Text = "...";
            this.open_folder_button.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.open_folder_button.UseVisualStyleBackColor = false;
            this.open_folder_button.Click += new System.EventHandler(this.open_folder_button_Click);
            // 
            // Target_Folder_button
            // 
            this.Target_Folder_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(2)))), ((int)(((byte)(2)))));
            this.Target_Folder_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Target_Folder_button.ForeColor = System.Drawing.Color.Teal;
            this.Target_Folder_button.Location = new System.Drawing.Point(212, 120);
            this.Target_Folder_button.Name = "Target_Folder_button";
            this.Target_Folder_button.Size = new System.Drawing.Size(443, 42);
            this.Target_Folder_button.TabIndex = 3;
            this.Target_Folder_button.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Target_Folder_button.UseVisualStyleBackColor = false;
            // 
            // Extention_textBox
            // 
            this.Extention_textBox.Location = new System.Drawing.Point(212, 43);
            this.Extention_textBox.Name = "Extention_textBox";
            this.Extention_textBox.Size = new System.Drawing.Size(152, 23);
            this.Extention_textBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.ForeColor = System.Drawing.Color.Teal;
            this.label1.Location = new System.Drawing.Point(57, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Extention : ";
            // 
            // Encryption_Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel1);
            this.Name = "Encryption_Options";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Encryption Options";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Panel panel1;
        private CheckBox EFEWCU_checkbox;
        private Label label2;
        private Button open_folder_button;
        private Button Target_Folder_button;
        private TextBox Extention_textBox;
        private Label label1;
        private Button Save;
    }
}