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
    public partial class FolderDecrypterLog : Form
    {
        public FolderDecrypterLog()
        {
            InitializeComponent();
            if (!log_bgw.IsBusy)
            {
                log_bgw.RunWorkerAsync();

            }
        }

        internal string password = string.Empty;

        internal List<string> fileslist = new List<string>();

        internal byte[] Salt = null;

        int gettoDecryptFilesCount()
        {
        
            int i = 0;
            foreach (string file in fileslist)
            {

                if (Path.GetExtension(fileslist[i]) == Configure.Extenchen)
                {

                    i++;
                
                }
            
            }

            return i;
        
        
        }
        private void log_bgw_DoWork(object sender, DoWorkEventArgs e)
        {

            for (int i = 0; i < fileslist.Count; i++)
            {

                try
                {
                    
                        FileEncryption.FileDecrypt(fileslist[i], fileslist[i].Replace(Path.GetExtension(fileslist[i]), ""), password, this.Salt);
                        log_bgw.ReportProgress(((i + 1) * 100) / fileslist.Count);
                        File.Delete(fileslist[i]);
                        Invoke(new Action(() => { log_textBox.AppendText(Path.GetFileName(fileslist[i] + " $$Successfully Decrypted$$" + "\r\n")); }));
                    
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);

                }

            }

            log_bgw.ReportProgress(100);

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
