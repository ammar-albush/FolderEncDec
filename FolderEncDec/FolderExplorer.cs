using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Management;
namespace FolderEncDec
{
    public partial class FolderExplorer : Form
    {
        public FolderExplorer()
        {
            InitializeComponent();
            PopulateDriveList();
            listView1.Select();
        }

        string CurrentFolder = string.Empty;
        private void PopulateDriveList()
        {
            TreeNode nodeTreeNode;
            int imageIndex = 0;
            int selectIndex = 0;

            const int Removable = 2;
            const int LocalDisk = 3;
            const int Network = 4;
            const int CD = 5;
            //const int RAMDrive = 6; 

            this.Cursor = Cursors.WaitCursor;

            //clear TreeView 
            treeView1.Nodes.Clear();
            nodeTreeNode = new TreeNode("My Computer", 0, 0);
            treeView1.Nodes.Add(nodeTreeNode);

            //set node collection 
            TreeNodeCollection nodeCollection = nodeTreeNode.Nodes;

            //Get Drive list 
            ManagementObjectCollection queryCollection = getDrives();
            foreach (ManagementObject mo in queryCollection)
            {
                switch (int.Parse(mo["DriveType"].ToString()))
                {
                    case Removable: //removable drives 
                        imageIndex = 4;
                        selectIndex = 4;
                        break;
                    case LocalDisk: //Local drives 
                        imageIndex = 6;
                        selectIndex = 6;
                        break;
                    case CD: //CD rom drives 
                        imageIndex = 3;
                        selectIndex = 3;
                        break;
                    case Network: //Network drives 
                        imageIndex = 2;
                        selectIndex = 2;
                        break;
                    default: //defalut to folder 
                        imageIndex = 1;
                        selectIndex = 7;
                        break;
                }

                //create new drive node
                nodeTreeNode = new TreeNode(mo["Name"].ToString()
                    + "\\", imageIndex, selectIndex);

                //add new node 
                nodeCollection.Add(nodeTreeNode);
            }

            //Init files ListView 
            listView1.Clear();

            this.Cursor = Cursors.Default;
        }

        protected ManagementObjectCollection getDrives()
        {
            //get drive collection 
            ManagementObjectSearcher query = new
                ManagementObjectSearcher("SELECT * From Win32_LogicalDisk ");
            ManagementObjectCollection queryCollection = query.Get();
            return queryCollection;
        }

       
        private void tvFolders_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            //Populate folders and files when a folder is selected
            this.Cursor = Cursors.WaitCursor;

            //get current selected drive or folder
            TreeNode nodeCurrent = e.Node;

            //clear all sub-folders
            nodeCurrent.Nodes.Clear();

            if (nodeCurrent.SelectedImageIndex == 0)
            {
                //Selected My Computer - repopulate drive list
                PopulateDriveList();
            }
            else
            {
                //populate sub-folders and folder files
                PopulateDirectory(nodeCurrent, nodeCurrent.Nodes);
            }
            this.Cursor = Cursors.Default;
        }

        protected void PopulateDirectory(TreeNode nodeCurrent, TreeNodeCollection nodeCurrentCollection)
        {
            TreeNode nodeDir;
            int imageIndex = 1;     //unselected image index
            int selectIndex = 7;	//selected image index

            if (nodeCurrent.SelectedImageIndex != 0)
            {
                //populate treeview with folders
                try
                {
                    //check path
                    if (Directory.Exists(getFullPath(nodeCurrent.FullPath)) == false)
                    {
                        MessageBox.Show("Directory or path " + nodeCurrent.ToString() + " does not exist.");
                    }
                    else
                    {
                      

                        PopulateFilesLargeIcon(nodeCurrent);
                        CurrentFolder = nodeCurrent.FullPath;
                        label1.Text = CurrentFolder;
                        string[] stringDirectories = Directory.GetDirectories(getFullPath(nodeCurrent.FullPath));
                        string stringFullPath = "";
                        string stringPathName = "";

                        //loop throught all directories
                        foreach (string stringDir in stringDirectories)
                        {
                            stringFullPath = stringDir;
                            stringPathName = GetPathName(stringFullPath);

                            //create node for directories
                            nodeDir = new TreeNode(stringPathName.ToString(), imageIndex, selectIndex);
                            nodeCurrentCollection.Add(nodeDir);
                        }
                    }
                }
                catch (IOException e)
                {
                    MessageBox.Show("Error: Drive not ready or directory does not exist.");
                }
                catch (UnauthorizedAccessException e)
                {
                    MessageBox.Show("Error: Drive or directory access denided.");
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error: " + e);
                }
            }
        }

        protected string GetPathName(string stringPath)
        {
            //Get Name of folder
            string[] stringSplit = stringPath.Split('\\');
            int _maxIndex = stringSplit.Length;
            return stringSplit[_maxIndex - 1];
        }

     
        protected void PopulateFilesLargeIcon(TreeNode nodeCurrent)
        {
            
            //clear list
            listView1.Clear();

            if (nodeCurrent.SelectedImageIndex != 0)
            {
                //check path
                if (Directory.Exists((string)getFullPath(nodeCurrent.FullPath)) == false)
                {
                    MessageBox.Show("Directory or path " + nodeCurrent.ToString() + " does not exist.");
                }
                else
                {
                    try
                    {


                        System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(getFullPath(nodeCurrent.FullPath));
                        CurrentFolder=nodeCurrent.FullPath;
                        label1.Text = CurrentFolder;

                        ListViewItem item;


                        // For each file in the c:\ directory, create a ListViewItem
                        // and set the icon to the icon extracted from the file.
                        foreach (System.IO.FileInfo file in dir.GetFiles())
                        {
                            // Set a default icon for the file.
                            Icon? iconForFile = System.Drawing.Icon.ExtractAssociatedIcon(file.FullName);

                            item = new ListViewItem(file.Name, 1);

                            string extension = Path.GetExtension(file.FullName);

                            if (extension == ".exe" || extension == ".lnk")
                            {
                                string exepFile = file.Extension + "_" + file.Name;
                                if (!imageList2.Images.ContainsKey(exepFile))
                                {

                                    imageList2.Images.Add(exepFile, iconForFile.ToBitmap());

                                }

                                item.ImageKey = exepFile;

                            }
                            else
                            {

                                if (!imageList2.Images.ContainsKey(extension))
                                {

                                    imageList2.Images.Add(extension, iconForFile.ToBitmap());


                                }

                                item.ImageKey = extension;

                            }

                            item.Tag = file.FullName;
                            listView1.Items.Add(item);
                        }
                        foreach (System.IO.DirectoryInfo file in dir.GetDirectories())
                        {

                            item = new ListViewItem(file.Name, 0);
                            item.Tag = file.FullName;
                            listView1.Items.Add(item);


                        }


                    }
                    catch (IOException e)
                    {
                        MessageBox.Show("Error: Drive not ready or directory does not exist.");
                    }
                    catch (UnauthorizedAccessException e)
                    {
                        MessageBox.Show("Error: Drive or directory access denided.");
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Error: " + e);
                    }
                }
            }
        }

        protected void PopulateFilesLargeIcon(string pathCurrent)
        {


            //clear list
            listView1.Clear();

            //check path
            if (Directory.Exists((string)getFullPath(pathCurrent)) == false)
            {
                MessageBox.Show("Directory or path " + pathCurrent.ToString() + " does not exist.");
            }
            else
            {
                try
                {


                    System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(getFullPath(pathCurrent));
                    CurrentFolder = pathCurrent;
                    label1.Text = CurrentFolder;

                    ListViewItem item;


                    // For each file in the c:\ directory, create a ListViewItem
                    // and set the icon to the icon extracted from the file.
                    foreach (System.IO.FileInfo file in dir.GetFiles())
                    {
                        // Set a default icon for the file.
                        Icon? iconForFile = System.Drawing.Icon.ExtractAssociatedIcon(file.FullName);

                        item = new ListViewItem(file.Name, 1);

                        string extension = Path.GetExtension(file.FullName);

                        if (extension == ".exe" || extension == ".lnk")
                        {
                            string exepFile = file.Extension + "_" + file.Name;
                            if (!imageList2.Images.ContainsKey(exepFile))
                            {

                                imageList2.Images.Add(exepFile, iconForFile.ToBitmap());

                            }

                            item.ImageKey = exepFile;

                        } else if (extension == Configure.Extenchen)
                        {

                            item.ImageIndex = 1;

                        } else if (extension == ".data")
                        {

                            item.ImageIndex = 2;
                        
                        }
                        else
                        {

                            if (!imageList2.Images.ContainsKey(extension))
                            {

                                imageList2.Images.Add(extension, iconForFile.ToBitmap());


                            }

                            item.ImageKey = extension;

                        }

                        item.Tag = file.FullName;
                        listView1.Items.Add(item);
                    }
                    foreach (System.IO.DirectoryInfo file in dir.GetDirectories())
                    {

                        item = new ListViewItem(file.Name, 0);
                        item.Tag = file.FullName;
                        listView1.Items.Add(item);


                    }




                }
                catch (IOException e)
                {
                    MessageBox.Show("Error: Drive not ready or directory does not exist.");
                }
                catch (UnauthorizedAccessException e)
                {
                    MessageBox.Show("Error: Drive or directory access denided.");
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error: " + e);
                }
            }

        }
    

        protected string getFullPath(string stringPath)
        {
            //Get Full path
            string stringParse = "";
            //remove My Computer from path.
            stringParse = stringPath.Replace("My Computer\\", "");

            return stringParse;
        }
        protected string getFullPathslash(string stringPath)
        {
            //Get Full path
            string stringParse = "";
            //remove My Computer from path.
            stringParse = stringPath.Replace(@"My Computer/", "");

            return stringParse;
        }


        private void listView1_Click(object sender, EventArgs e)
        {

           


        }

        protected string getBackFolder(string path)
        {
            if (path != string.Empty)
            {
                DirectoryInfo dirInfo = new DirectoryInfo(path);
                string dirname = dirInfo.Name;
                return path.Replace(dirname, "");
            }
            return path;
        
        }
        private void listView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Back)
            {
              string path = getBackFolder(CurrentFolder);
                if ((path != string.Empty)|| (path != "\\") || (path != ""))
                PopulateFilesLargeIcon(path); ;
            }
            if (e.KeyChar == (char)Keys.Clear)
            {

                FileAttributes attr = File.GetAttributes(CurrentFolder);

                if (attr.HasFlag(FileAttributes.Directory))
                {
                    Directory.Delete(CurrentFolder);
                    listView1.Refresh();
                }
                else {

                    Directory.Delete(CurrentFolder);
                    listView1.Refresh();

                }
            }

        }

        private void treeView1_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == (char)Keys.Back)
            {
                string path = getBackFolder(CurrentFolder);
                if (path != string.Empty || path != "\\" || path != "")
                    PopulateFilesLargeIcon(path); 
            }

            if (e.KeyChar == (char)Keys.Clear)
            {

                FileAttributes attr = File.GetAttributes(CurrentFolder);

                if (attr.HasFlag(FileAttributes.Directory))
                {
                    Directory.Delete(CurrentFolder);
                    listView1.Refresh();
                }
                else
                {

                    Directory.Delete(CurrentFolder);
                    listView1.Refresh();

                }
            }

        }

        private void decrypt_button_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Back)
            {
                string path = getBackFolder(CurrentFolder);
                if (path != string.Empty || path != "\\" || path != "")
                    PopulateFilesLargeIcon(path); 
            }
            if (e.KeyChar == (char)Keys.Clear)
            {

                FileAttributes attr = File.GetAttributes(CurrentFolder);

                if (attr.HasFlag(FileAttributes.Directory))
                {
                    Directory.Delete(CurrentFolder);
                    listView1.Refresh();
                }
                else
                {

                    Directory.Delete(CurrentFolder);
                    listView1.Refresh();

                }
            }
        }
        private void Encrypt_button_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Back)
            {
                string path = getBackFolder(CurrentFolder);
                if (path != string.Empty || path != "\\" || path != "")
                    PopulateFilesLargeIcon(path); 
            }
            if (e.KeyChar == (char)Keys.Clear)
            {

                FileAttributes attr = File.GetAttributes(CurrentFolder);

                if (attr.HasFlag(FileAttributes.Directory))
                {
                    Directory.Delete(CurrentFolder);
                    string cv = getBackFolder(CurrentFolder);
                    if (cv != string.Empty || cv != "\\" || cv != "")
                        PopulateFilesLargeIcon(cv);
                }
                else
                {

                    Directory.Delete(CurrentFolder);
                    string cv = getBackFolder(CurrentFolder);
                    if (cv != string.Empty || cv != "\\" || cv != "")
                        PopulateFilesLargeIcon(cv);
                }
            }
        }

        private void Encrypt_button_Click(object sender, EventArgs e)
        {
            FolderEncrypter folderEncrypter = new FolderEncrypter();
            folderEncrypter.Target_Folder_button.Text =getFullPath(CurrentFolder);
            folderEncrypter.ShowDialog();
            string cv = getFullPath(CurrentFolder);
            if (cv != string.Empty || cv != "\\" || cv != "")
                PopulateFilesLargeIcon(cv);
        }

        private void encryptionOptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Encryption_Options encryption_Options = new Encryption_Options();
            encryption_Options.ShowDialog();
        }

        private void decrypt_button_Click(object sender, EventArgs e)
        {
            FolderDecrypter folderDecrypter = new FolderDecrypter();
            folderDecrypter.Target_Folder_button.Text = getFullPath(CurrentFolder);
            folderDecrypter.ShowDialog();
            string cv = getFullPath(CurrentFolder);
            if (cv != string.Empty || cv != "\\" || cv != "")
                PopulateFilesLargeIcon(cv);

        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

               

            }
            if (e.Button == MouseButtons.Right)
            {

                contextMenuStrip1.Show(Cursor.Position);
            
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {

            ListViewItem lvi = listView1.SelectedItems[0];
            string path = getFullPathslash((string)lvi.Tag);
              FileAttributes attr = File.GetAttributes(path);

            if (attr.HasFlag(FileAttributes.Directory))
            {
                Directory.Delete(path);
                string cv = getFullPath(CurrentFolder);
                if (cv != string.Empty || cv != "\\" || cv != "")
                    PopulateFilesLargeIcon(cv);
            }
            else
            {

                File.Delete(path);
                string cv = getFullPath(CurrentFolder);
                if (cv != string.Empty || cv != "\\" || cv != "")
                    PopulateFilesLargeIcon(cv);
            }

        }

      

        private void back_button_Click(object sender, EventArgs e)
        {
            string path = getBackFolder(CurrentFolder);
            if (path != string.Empty || path != "\\" || path != "")
                PopulateFilesLargeIcon(path);
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                ListViewItem lvi = listView1.SelectedItems[0];
                string path = getFullPathslash((string)lvi.Tag);
                CurrentFolder = path;
                label1.Text = CurrentFolder;

                FileAttributes attr = File.GetAttributes(path);

                if (attr.HasFlag(FileAttributes.Directory))
                {

                    PopulateFilesLargeIcon(path);
                }
                else
                {
                    var p = new Process();
                    p.StartInfo = new ProcessStartInfo(path)
                    {
                        UseShellExecute = true
                    };
                    p.Start();


                }


            }
            catch (IOException ex)
            {
                MessageBox.Show("Error: Drive not ready or directory does not exist.");


            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show("Error: Drive or directory access denided.");



            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }
        }
    }
}