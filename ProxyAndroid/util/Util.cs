using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProxyAndroid.util
{
    class Util
    {
        /************************************************************************/
        /* 记录信息到记录板中                                                                     */
        /************************************************************************/
        public static void recordInfo(String str, TextBox tb)
        {
            tb.Text += str + System.Environment.NewLine + "--------------------------------" + System.Environment.NewLine;
            tb.SelectionStart = tb.TextLength;
            tb.Focus();
        }

        /************************************************************************/
        /* 清空目录下符合规则的所有文件                                                                     */
        /************************************************************************/
        public static void clearOutPutDir(string path, string[] rules)
        {
            try
            {
                List<FileInfo> files = GetAllFilesInDirectory(path);
                foreach (FileInfo f in files)
                {
                    String[] suffix = f.Name.Split('.');
                    for (int i = 0; i < rules.Length; i++)
                    {
                        if (suffix[i].Equals(rules[i]))
                        {
                            File.Delete(path + "\\" + f.Name);
                        }
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("数据异常", "提示");
            }
        }

        /************************************************************************/
        /* 清空目录下所有文件                                                                     */
        /************************************************************************/
        public static void clearOutPutDir(string path)
        {
            try
            {
                List<FileInfo> files = GetAllFilesInDirectory(path);
                foreach (FileInfo f in files)
                {
                    File.Delete(path + "\\" + f.Name);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("数据异常", "提示");
            }
        }

        /************************************************************************/
        /* 清空目录下所有文件（包括文件夹）                                                       */
        /************************************************************************/
        public static void clearAllFilesDir(string path, string key) {
            if (path.Trim() == "" || !Directory.Exists(path))
                return;
            DirectoryInfo dirInfo = new DirectoryInfo(path);

            FileInfo[] fileInfos = dirInfo.GetFiles();
            if (fileInfos != null && fileInfos.Length > 0)
            {
                foreach (FileInfo fileInfo in fileInfos)
                {
                    //DateTime.Compare( fileInfo.LastWriteTime,DateTime.Now);
                    File.Delete(fileInfo.FullName); //删除文件
                }
            }

            DirectoryInfo[] dirInfos = dirInfo.GetDirectories();
            if (dirInfos != null && dirInfos.Length > 0)
            {
                foreach (DirectoryInfo childDirInfo in dirInfos)
                {
                    clearAllFilesDir(childDirInfo.FullName, key); //递归
                }
            }
            if (key.Length != 0) {
                if (!dirInfo.FullName.Contains(key))
                {
                    Directory.Delete(dirInfo.FullName, true); //删除目录
                }
            }
        }

        /************************************************************************/
        /* 获取目录下所有文件                                                                     */
        /************************************************************************/
        public static List<FileInfo> GetAllFilesInDirectory(string strDirectory)
        {
            List<FileInfo> listFiles = new List<FileInfo>(); //保存所有的文件信息  
            DirectoryInfo directory = new DirectoryInfo(strDirectory);
            DirectoryInfo[] directoryArray = directory.GetDirectories();
            FileInfo[] fileInfoArray = directory.GetFiles();
            if (fileInfoArray.Length > 0) listFiles.AddRange(fileInfoArray);
            foreach (DirectoryInfo _directoryInfo in directoryArray)
            {
                DirectoryInfo directoryA = new DirectoryInfo(_directoryInfo.FullName);
                DirectoryInfo[] directoryArrayA = directoryA.GetDirectories();
                FileInfo[] fileInfoArrayA = directoryA.GetFiles();
                if (fileInfoArrayA.Length > 0) listFiles.AddRange(fileInfoArrayA);
                GetAllFilesInDirectory(_directoryInfo.FullName);//递归遍历  
            }
            return listFiles;
        }

        /************************************************************************/
        /* 执行bat文件                                                                     */
        /************************************************************************/
        public static void doBat(string path, string name) 
        {
            Process proc = new Process();
            proc.StartInfo.WorkingDirectory = path;
            proc.StartInfo.FileName = name;
            proc.StartInfo.CreateNoWindow = false;
            proc.Start();
            proc.WaitForExit();
        }
    }
}
