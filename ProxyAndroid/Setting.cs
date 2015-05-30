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
    public partial class Setting : Form
    {
        string settingPath = System.IO.Directory.GetCurrentDirectory() + "\\Setting.txt";
        public static string selectedPath = "";

        public Setting()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            saveInfo();
            this.Close();
        }

        private void btnSelectPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog path = new FolderBrowserDialog();
            path.ShowDialog();
            selectedPath = path.SelectedPath;
        }

        private void saveInfo()
        {
            try
            {
                StreamWriter sw = new StreamWriter(settingPath);
                sw.Write("");
                sw.Close();
                sw = new StreamWriter(settingPath);
                sw.WriteLine("selectedPath:\r\n" + selectedPath);
                if (this.cbSaveLastName.Checked)
                {
                    sw.WriteLine("saveLastName:\r\nYES");
                }
                else
                {
                    sw.WriteLine("saveLastName:\r\nNO");
                }
                sw.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("数据异常", "提示");
            }
        }

        private void Setting_Load(object sender, EventArgs e)
        {
            init();
        }

        private void init()
        {
            try
            {
                FileStream file = File.Open(settingPath, FileMode.Open);
                List<string> txt = new List<string>();
                using (var stream = new StreamReader(file))
                {
                    while (!stream.EndOfStream)
                    {
                        txt.Add(stream.ReadLine());
                    }
                }

                for (int i = 0; i < txt.Count; i++)
                {
                    if (txt[i].IndexOf("selectedPath") > -1)
                    {
                        selectedPath = txt[i + 1];
                    }
                    else if (txt[i].IndexOf("saveLastName") > -1)
                    {
                        if (txt[i + 1].Equals("YES"))
                        {
                            this.cbSaveLastName.Checked = true;
                        }
                        else
                        {
                            this.cbSaveLastName.Checked = false;
                        }
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("数据异常", "提示");
            }
        }
    }
}
