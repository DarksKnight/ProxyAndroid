using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProxyAndroid
{
    public partial class InitWindow : Form
    {
        public InitWindow()
        {
            InitializeComponent();
        }

        private void InitWindow_Load(object sender, EventArgs e)
        {
            if (initEnv())
            {
                Form1 f = new Form1();
                this.Hide();
                f.ShowDialog();
            }
        }

        /************************************************************************/
        /* 初始化文件夹                                                                     */
        /************************************************************************/
        private Boolean initEnv()
        {
            if (!File.Exists(@"E:\hwy_android_package_temp") && !File.Exists(@"E:\hwy_android_temp"))
            {
                try
                {
                    recordInfo("检查路径：\r\nE:\\hwy_android_package_temp");
                    createDir(@"E:\hwy_android_package_temp");
                    directoryCopy(System.IO.Directory.GetCurrentDirectory() + "\\hwy_android_package_temp", @"E:\hwy_android_package_temp");
                    recordInfo("检查完毕，检查路径：\r\nE:\\hwy_android_temp");
                    createDir(@"E:\hwy_android_temp");
                    directoryCopy(System.IO.Directory.GetCurrentDirectory() + "\\hwy_android_temp", @"E:\hwy_android_temp");
                    recordInfo("检查完毕");
                    return true;
                }
                catch (Exception)
                {
                    MessageBox.Show("初始化异常", "提示");
                    return false;
                }
            }
            return true;
        }

        /************************************************************************/
        /* 创建文件夹                                                                     */
        /************************************************************************/
        private void createDir(String sPath)
        {
            try
            {
                Directory.CreateDirectory(sPath);
            }
            catch (Exception)
            {
                MessageBox.Show("创建失败", "提示");
            }
        }

        /************************************************************************/
        /* 将文件夹所有内容复制到指定文件夹中                                                                     */
        /************************************************************************/
        private void directoryCopy(string sourceDirectory, string targetDirectory)
        {
            if (!Directory.Exists(sourceDirectory) || !Directory.Exists(targetDirectory))
            {
                return;
            }
            DirectoryInfo sourceInfo = new DirectoryInfo(sourceDirectory);
            FileInfo[] fileInfo = sourceInfo.GetFiles();
            foreach (FileInfo fiTemp in fileInfo)
            {
                File.Copy(sourceDirectory + "\\" + fiTemp.Name, targetDirectory + "\\" + fiTemp.Name, true);
            }
            DirectoryInfo[] diInfo = sourceInfo.GetDirectories();
            foreach (DirectoryInfo diTemp in diInfo)
            {
                string sourcePath = diTemp.FullName;
                string targetPath = diTemp.FullName.Replace(sourceDirectory, targetDirectory);
                Directory.CreateDirectory(targetPath);
                directoryCopy(sourcePath, targetPath);
            }
        }

        /************************************************************************/
        /* 记录信息到记录板中                                                                     */
        /************************************************************************/
        private void recordInfo(String str)
        {
            this.tbInitInfo.Text += str + System.Environment.NewLine + "--------------------------------" + System.Environment.NewLine;
            this.tbInitInfo.SelectionStart = this.tbInitInfo.TextLength;
            this.tbInitInfo.Focus();
        }
    }
}
