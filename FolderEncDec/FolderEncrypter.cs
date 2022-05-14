using Emgu.CV;
using Emgu.CV.Structure;
using FaceRecognitionDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FolderEncDec
{
    public partial class FolderEncrypter : Form
    {
        public FolderEncrypter()
        {
            InitializeComponent();
            startVideoCapture();
            get_img_backgroundWorker.RunWorkerAsync();
        }

        private bool captureStarted = false;

        VideoCapture capture;

        Image<Bgr, byte> imgInput;

        Image<Bgr, byte> toCompareImage;

        byte[] Salt = null;


        static byte[] s_additionalEntropy = { 9, 8, 7, 6, 5 };

        string password = string.Empty;
        void startVideoCapture()
        {

            if (capture == null)
            {

                capture = new VideoCapture(0);

            }

            capture.ImageGrabbed += Img_Capture;


            capture.Start();


        }
        void stopVideoCapture()
        {

            if (capture != null)
            {

                capture.Stop();

            }

        }
        private void Img_Capture(object? sender, EventArgs e)
        {
            try
            {


                DetectFaceHaar();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);

            }

        }

        string face_path = Path.GetFullPath(@"./haarcascade_frontalface_default.xml");
        string eyes_path = Path.GetFullPath(@"./haarcascade_eye.xml");

        CascadeClassifier cascadeClassifierface => new CascadeClassifier(face_path);
        CascadeClassifier cascadeClassifiereye => new CascadeClassifier(eyes_path);

        public void DetectFaceHaar()
        {

            try
            {


                if (capture.QueryFrame() != null)
                {
                    imgInput = capture.QueryFrame().ToImage<Bgr, byte>().Resize(capture_imageBox.Width, capture_imageBox.Height, Emgu.CV.CvEnum.Inter.Cubic);

                    var imgGray = capture.QueryFrame().ToImage<Gray, byte>().Resize(capture_imageBox.Width, capture_imageBox.Height, Emgu.CV.CvEnum.Inter.Cubic);

                    Image<Bgr, byte> bgr_img = capture.QueryFrame().ToImage<Bgr, byte>().Resize(capture_imageBox.Width, capture_imageBox.Height, Emgu.CV.CvEnum.Inter.Cubic);


                    Rectangle[] faces = cascadeClassifierface.DetectMultiScale(imgGray, 1.1, 4);

                    foreach (var face in faces)
                    {

                        imgInput.Draw(face, new Bgr(0, 0, 255), 2);

                        imgGray.ROI = face;
                        Rectangle[] eyes = cascadeClassifiereye.DetectMultiScale(imgGray, 1.1, 4);

                    }
                    /*
                    if (faces.Length != 0)
                    {

                        stopVideoCapture();
                        capture_imageBox.Image = bgr_img;
                        MessageBox.Show(" Successfuly  Detected");
                        Invoke(new Action(() => {
                            Encrypt_button.Enabled = true;
                        }));
                        toCompareImage =bgr_img;
                        

                    }
                    else
                    {

                        capture_imageBox.Image = imgInput;
                    }
                }*/
                    capture_imageBox.Image = imgInput;

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);

            }



        }

        private void SaveFileDecrypter(string filename)
        {


            FaceRecognitionDotNet.Image img = FaceRecognition.LoadImage(toCompareImage.ToBitmap());

            string hash = string.Empty;

            double[] img_matrix = faceRecognition.FaceEncodings(img).ToArray().First().GetRawEncoding();

            for (int i = 0; i < img_matrix.Length; i++)
            {

                hash += string.Format("{0:0.00000000}", img_matrix[i]);

            }



            string password = GetHashString(hash);

            this.password = password;
            byte[] bytes;
            byte[] passworbytes;

            if (Configure.FileEncrypterWithCurrentUserEncryption)
            {
                bytes = ProtectdoubleArray(img_matrix);

                passworbytes = ProtectString(password);
            }
            else
            {

                bytes = GetBytes(img_matrix);
                passworbytes = Encoding.ASCII.GetBytes(password);

            }
            this.Salt = FileEncryption.GenerateRandomSalt();

            ImageInfo imageInfo = new ImageInfo(bytes, passworbytes, this.Salt);

            string targetpath = Target_Folder_button.Text + @"\" + filename;

            FileStream stream;

            if (Configure.FileEncrypterPath == String.Empty)
            {

                using (stream = new FileStream(Target_Folder_button.Text + @"\" + filename, FileMode.Create, FileAccess.Write, FileShare.None)) {

                    IFormatter formatter = new BinaryFormatter();

                    formatter.Serialize(stream, imageInfo);

                    stream.Close();

                }
           
            } else {

                using (stream = new FileStream(Configure.FileEncrypterPath + @"\" + filename, FileMode.Create, FileAccess.Write, FileShare.None)) {
                  
                    IFormatter formatter = new BinaryFormatter();

                    formatter.Serialize(stream, imageInfo);

                    stream.Close();

                }

            }
           



        }

        private byte[] GetBytes(double[] values)
        {
            return values.SelectMany(value => BitConverter.GetBytes(value)).ToArray();
        }



        private byte[] ProtectdoubleArray(double[] data)
        {
            try
            {
                // Encrypt the data using DataProtectionScope.CurrentUser. The result can be decrypted
                // only by the same current user.
                return ProtectedData.Protect(GetBytes(data), s_additionalEntropy, DataProtectionScope.CurrentUser);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("Data was not encrypted. An error occurred.");
                Console.WriteLine(e.ToString());
                return null;
            }
        }

        private byte[] ProtectString(string data)
        {

            byte[] bytes = Encoding.ASCII.GetBytes(data);

            try
            {
                // Encrypt the data using DataProtectionScope.CurrentUser. The result can be decrypted
                // only by the same current user.
                return ProtectedData.Protect(bytes, s_additionalEntropy, DataProtectionScope.CurrentUser);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("Data was not encrypted. An error occurred.");
                Console.WriteLine(e.ToString());
                return null;
            }

        }

        public static byte[] GetHash(string inputString)
        {
            using (HashAlgorithm algorithm = SHA256.Create())
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
        private void EncryptFileInfo()
        {



        }

        private void Target_Folder_button_Click(object sender, EventArgs e)
        {

            using (var fbd = new FolderBrowserDialog())
            {

                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {

                    // this.Target_Path_Text.Text = fbd.SelectedPath;

                }
            }
        }

        private void Start_encryption_buttom_Click(object sender, EventArgs e)
        {

        }

        private void start_encreyption_backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {

            // string[] stringFiles = Directory.GetFiles(Target_Path_Text.Text);

            /* foreach (string file in stringFiles)
             {


              //   FileEncryption.FileEncrypt();

             }
            */

        }

        FaceRecognition faceRecognition = FaceRecognition.Create(Application.StartupPath);


        private bool isDetected = false;
        private void get_img_backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {

            while (true)
            {
                try
                {

                    Bitmap bitmap = capture.QueryFrame().ToImage<Bgr, byte>().Resize(capture_imageBox.Width, capture_imageBox.Height, Emgu.CV.CvEnum.Inter.Cubic).ToBitmap();
                    Image<Bgr, byte> bgr_img = capture.QueryFrame().ToImage<Bgr, byte>().Resize(capture_imageBox.Width, capture_imageBox.Height, Emgu.CV.CvEnum.Inter.Cubic);

                    System.Drawing.Image img = bgr_img.ToBitmap();

                    FaceRecognitionDotNet.Image fimg = FaceRecognition.LoadImage(new Bitmap(img));

                    var getImgEn = faceRecognition.FaceEncodings(fimg).ToArray()[0];

                    if (getImgEn != null)
                    {

                        isDetected = true;

                        stopVideoCapture();

                        capture_imageBox.Image = bgr_img;

                        //   Invoke(new Action(() => { button1.Image = global::FaceBasedFileEncrypter.Properties.Resources.icons8_geprüft_16; }));
                        Invoke(new Action(() => {
                            Encrypt_button.Enabled = true;
                        }));
                        toCompareImage = bgr_img;



                        break;


                    }



                }


                catch (Exception ex)
                {



                }

            }
        }

        private void get_img_backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (isDetected)
                MessageBox.Show(" Successfuly  Detected");
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

        private void Encrypt_button_Click(object sender, EventArgs e)
        {
            try
            {

                string[] stringFiles = Directory.GetFiles(this.Target_Folder_button.Text);
                SaveFileDecrypter("FileDecrypter.data");
                FolderEncrypterLog encryptionLog = new FolderEncrypterLog();
                encryptionLog.password = this.password;
                encryptionLog.fileslist = stringFiles;
                encryptionLog.Salt = this.Salt;
                encryptionLog.ShowDialog();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

       
    }
}
