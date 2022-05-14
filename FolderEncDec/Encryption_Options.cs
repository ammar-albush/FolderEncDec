using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FolderEncDec
{
    public partial class Encryption_Options : Form
    {
        public Encryption_Options()
        {
            InitializeComponent();
            this.Extention_textBox.Text = Configure.Extenchen;
            this.Target_Folder_button.Text = Configure.FileEncrypterPath;
            this.EFEWCU_checkbox.Checked = Configure.FileEncrypterWithCurrentUserEncryption;
        }

        private void Save_Click(object sender, EventArgs e)
        {
            bool validInput = true;

            if (!this.Extention_textBox.Text.Contains(".")||this.Extention_textBox.Text == String.Empty)
            {
                MessageBox.Show("invalid Extenchen");
                validInput = false;
            }
            if (Configure.FileEncrypterPath != String.Empty)
            {

                if (!Directory.Exists(Configure.FileEncrypterPath))
                {

                    MessageBox.Show("target path doesn´t Exist");
                    validInput=false;
                
                }
            
            }
            if (validInput)
            {
                Configure.Extenchen = this.Extention_textBox.Text;
                Configure.FileEncrypterPath = this.Target_Folder_button.Text;
                Configure.FileEncrypterWithCurrentUserEncryption = this.EFEWCU_checkbox.Checked;
                this.Close();
            }
        }

        private void open_folder_button_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {

                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {

                    this.Target_Folder_button.Text = fbd.SelectedPath;

                }
            }
        }
    }
}
