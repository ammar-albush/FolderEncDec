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
    public partial class FolderDecrypter : Form
    {
        public FolderDecrypter()
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

        static double[] GetDoublesformBytes(byte[] bytes)
        {
            return Enumerable.Range(0, bytes.Length / sizeof(double))
                .Select(offset => BitConverter.ToDouble(bytes, offset * sizeof(double)))
                .ToArray();
        }

        string getStringformBytes(byte[] bytes) => Encoding.ASCII.GetString(bytes);

        public static byte[] Unprotect(byte[] data)
        {
            try
            {
                //Decrypt the data using DataProtectionScope.CurrentUser.
                return ProtectedData.Unprotect(data, s_additionalEntropy, DataProtectionScope.CurrentUser);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("Data was not decrypted. An error occurred.");
                Console.WriteLine(e.ToString());
                return null;
            }
        }
        private bool chekimage()
        {
             
            ImageInfo imageInfo;

            if (Configure.FileEncrypterPath == String.Empty)
            {

                string FileEncrypterPath = Path.Combine(Target_Folder_button.Text, "FileDecrypter.data");


                if (File.Exists(FileEncrypterPath))
                {
                    using (FileStream stream = new FileStream(FileEncrypterPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {

                        IFormatter formatter = new BinaryFormatter();

                        imageInfo = (ImageInfo)formatter.Deserialize(stream);


                        FaceRecognitionDotNet.Image img = FaceRecognition.LoadImage(toCompareImage.ToBitmap());

                        double[] img_matrix = faceRecognition.FaceEncodings(img).ToArray().First().GetRawEncoding();

                        byte[] un_prot_password;

                        byte[] ing_info_bytes;

                        if (Configure.FileEncrypterWithCurrentUserEncryption)
                        {
                            un_prot_password = Unprotect(imageInfo.Password);

                            ing_info_bytes = Unprotect(imageInfo.Matrix);
                        }
                        else
                        {

                            un_prot_password = imageInfo.Password;
                            ing_info_bytes = imageInfo.Matrix;

                        }
                        string password = getStringformBytes(un_prot_password);

                        this.password = password;

                        this.Salt = imageInfo.Salt;



                        double[] imageMatrix = GetDoublesformBytes(ing_info_bytes);

                        DlibDotNet.Matrix<double> matrix1 = new DlibDotNet.Matrix<double>(imageMatrix, 4, 32);

                        DlibDotNet.Matrix<double> matrix2 = new DlibDotNet.Matrix<double>(img_matrix, 4, 32);

                        if (DlibDotNet.Dlib.Length(matrix1 - matrix2) < 0.6)
                        {

                            return true;

                        } 
                    
                    }

                }
                else
                {

                    MessageBox.Show("cannot Find  FileDecrypter ");

                }
            }
            else
            {

                string FileEncrypterPath = Path.Combine(Configure.FileEncrypterPath, "FileDecrypter.data"); 


                if (File.Exists(FileEncrypterPath))
                {
                    using (FileStream stream = new FileStream(FileEncrypterPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                   
                    {
                        IFormatter formatter = new BinaryFormatter();

                        imageInfo = (ImageInfo)formatter.Deserialize(stream);

                        FaceRecognitionDotNet.Image img = FaceRecognition.LoadImage(toCompareImage.ToBitmap());

                        double[] img_matrix = faceRecognition.FaceEncodings(img).ToArray().First().GetRawEncoding();

                        byte[] un_prot_password;

                        byte[] ing_info_bytes;

                        if (Configure.FileEncrypterWithCurrentUserEncryption)
                        {
                            un_prot_password = Unprotect(imageInfo.Password);

                            ing_info_bytes = Unprotect(imageInfo.Matrix);
                        }
                        else
                        {

                            un_prot_password = imageInfo.Password;
                            ing_info_bytes = imageInfo.Matrix;

                        }
                        string password = getStringformBytes(un_prot_password);

                        this.password = password;

                        this.Salt = imageInfo.Salt;



                        double[] imageMatrix = GetDoublesformBytes(ing_info_bytes);

                        DlibDotNet.Matrix<double> matrix1 = new DlibDotNet.Matrix<double>(imageMatrix, 4, 32);

                        DlibDotNet.Matrix<double> matrix2 = new DlibDotNet.Matrix<double>(img_matrix, 4, 32);

                        if (DlibDotNet.Dlib.Length(matrix1 - matrix2) < 0.6)
                        {

                            return true;

                        }
                    }
                }
                else
                {

                    MessageBox.Show("cannot Find  FileDecrypter ");

                }

            }           

            return false;



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

                        toCompareImage = bgr_img;

                        break;


                    }



                }




                catch (Exception ex)
                {



                }





            }

            if (isDetected)
                Invoke(new Action(() => {
                    MessageBox.Show(" Successfuly  Detected");
                }));

            while (true)
            {

                if (Target_Folder_button.Text != String.Empty)
                {

                    break;

                }

            }

            if (chekimage())
            {


                Invoke(new Action(() => { Decrypt_button.Enabled = true; }));

            }
            else
            {

                Invoke(new Action(() => { MessageBox.Show("Input Face Doesn´t match with the Face in DataBank"); }));

            }

        }

        private void get_img_backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

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





        private void check_img_bgw_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void Decrypt_button_Click(object sender, EventArgs e)
        {
            try
            {

                string[] stringFiles = Directory.GetFiles(this.Target_Folder_button.Text);
                List<string> encreptedFiles = new List<string>();
                foreach (string file in stringFiles)
                {

                    if (Path.GetExtension(file) == Configure.Extenchen)
                    {

                        encreptedFiles.Add(file);


                    }

                }
                FolderDecrypterLog DecryptionLog = new FolderDecrypterLog();
                DecryptionLog.password = this.password;
                DecryptionLog.fileslist = encreptedFiles;
                DecryptionLog.Salt = this.Salt;
                DecryptionLog.ShowDialog();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }


    }
}
