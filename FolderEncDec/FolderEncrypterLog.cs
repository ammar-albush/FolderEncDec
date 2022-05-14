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
    public partial class FolderEncrypterLog : Form
    {
        public FolderEncrypterLog()
        {
            InitializeComponent();
            if (!log_bgw.IsBusy)
            {
                log_bgw.RunWorkerAsync();

            }

        }
        internal string password = string.Empty;

        internal string[] fileslist = null;

        internal byte[] Salt = null;

        private void log_bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < fileslist.Length; i++)
            {

                try
                {
                    FileEncryption.FileEncrypt(fileslist[i], password, Salt,Configure.Extenchen);
                    log_bgw.ReportProgress(((i + 1) * 100) / fileslist.Length);
                    File.Delete(fileslist[i]);
                    Invoke(new Action(() => { log_textBox.AppendText(Path.GetFileName(fileslist[i] + " $$Successfully Encrypted$$" + "\r\n")); }));
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);

                }

            }

           
        }

        private void log_bgw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void log_bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            this.Close();
        }

    }
}
