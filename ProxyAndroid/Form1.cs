using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using ProxyAndroid.util;

namespace ProxyAndroid
{
    public partial class Form1 : Form
    {
        string version = "1.4";
        //------------------------------路径定义---------------------------------------
        //执行生成文件bat文件
        string batPath = @"E:\GAME_M_Engine_Proxy\BJMEngineProxyTest\proj\\tools\bin\BJMProxySDKUtil\BJMProxySDKUtil.exe";
        string outPutPath = @"E:\GAME_M_Engine_Proxy\BJMEngineProxyTest\proj\tools\template\output";
        string androidPath = @"E:\GAME_M_Engine_Proxy\BJMEngineProxy\BJMEngineProxy\android\proxy_sdk\sdk\proxy";
        string cPlusPath = @"E:\GAME_M_Engine_Proxy\BJMEngineProxy\BJMEngineProxy\classes\thirdparty";
        string resPath = @"E:\GAME_M_Engine_Proxy\BJMEngineProxyTest\proj\sim\package\platform\";
        string resOtherPath = @"E:\GAME_M_Engine_Proxy\sdkres\android\";
        string androidMKPath = @"E:\GAME_M_Engine_Proxy\BJMEngineProxy\BJMEngineProxy\classes\jni\Android.mk";
        string definePath = @"E:\GAME_M_Engine_Proxy\BJMEngineProxy\BJMEngineProxy\classes\core\BJMProxyPluginDefine.h";
        string createPluginPath = @"E:\GAME_M_Engine_Proxy\BJMEngineProxy\BJMEngineProxy\classes\core\BJMProxyCreatePlugin.cpp";
        string sdkConfigPath = @"E:\GAME_M_Engine_Proxy\BJMEngineProxyTest\proj\sim\apphome\res\cn\script\sdk\SDKConfig.lua";
        string functionConfigPath = @"E:\GAME_M_Engine_Proxy\BJMEngineProxyTest\proj\sim\apphome\res\cn\script\config\FunctionConfig.lua";
        string cashConfigPath = @"E:\GAME_M_Engine_Proxy\BJMEngineProxyTest\proj\sim\apphome\res\cn\dataconfig\cash.json";
        string testProPath = @"E:\GAME_M_Engine_Proxy\BJMEngineProxyTest\proj\tools\android";
        string serverResPath = @"\\192.168.0.6\项目资源共享\SDK\手游\渠道SDK";
        string targetResPath = @"E:\GAME_M_Engine_Proxy\sdkcheck\raw\android";
        string baseaPath = @"E:\hwy_android_temp";
        string basebPath = @"E:\hwy_android_package_temp";
        string plaformPath = @"E:\GAME_M_Engine_Proxy\BJMEngineProxyTest\proj\sim\package\platform";
        string pkgPath = @"E:\test\sim\apphome\ProxyM_cn_cn_cn_1_0_0.pkg";
        string targetPkgaPath = @"E:\GAME_M_Engine_Proxy\BJMEngineProxyTest\proj\tools\prebuild\ProxyM_cn_cn_cn_1_0_0.pkg";
        string targetPkgbPath = @"E:\GAME_M_Engine_Proxy\BJMEngineProxyTest\proj\tools\prebuild\android\ProxyM_cn_cn_cn_1_0_0.pkg";
        string infoPath = System.IO.Directory.GetCurrentDirectory() + "\\Info.txt";
        string settingPath = System.IO.Directory.GetCurrentDirectory() + "\\Setting.txt";
        string aboutPath = System.IO.Directory.GetCurrentDirectory() + "\\About.txt";
        string sdkResPath = @"E:\GAME_M_Engine_Proxy\sdkres\android";
        string toolPath = @"E:\GAME_M_Engine_Proxy\BJMEngineProxyTest\proj\tools\android";
        string genApkPath = @"E:\test2";
        string genPkgPath = @"E:\test";
        //-----------------------------------------------------------------------------

        //------------------------------参数定义---------------------------------------
        Boolean isSaveName = false;
        string lastCnName = "";
        string lastEnName = "";
        //-----------------------------------------------------------------------------

        public Form1()
        {
            InitializeComponent();
        }

        private void btnDoGenBat_Click(object sender, EventArgs e)
        {
            if (this.tbProName.Text.Trim().Length > 0)
            {
                Util.recordInfo("初始化环境",this.tbRecordInfo);
                initEnv();
                Util.recordInfo("初始化结束，执行生成",this.tbRecordInfo);
                try
                {
                    if (this.tbProName.Text.Trim().Length != 0)
                    {
                        Util.recordInfo("清空目录",this.tbRecordInfo);
                        //清空文件，再生成文件，无法进行覆盖
                        Util.clearOutPutDir(outPutPath, new string[]{"java","h","cpp"});
                        Util.recordInfo("开始执行",this.tbRecordInfo);
                        Process pro = new Process();
                        String scope = " ";
                        if (this.cbBase.Checked == true)
                        {
                            scope += "base";
                        }
                        if (this.cbAccount.Checked == true)
                        {
                            scope += "account";
                        }
                        if (this.cbIap.Checked == true)
                        {
                            scope += "iap";
                        }
                        if (this.cbBlog.Checked == true)
                        {
                            scope += "blog";
                        }
                        if (scope.Trim().Length == 0)
                        {
                            MessageBox.Show("请勾选所需功能", "提示");
                            return;
                        }
                        ProcessStartInfo pi = new ProcessStartInfo(batPath, "MakeCode " + System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(this.tbProName.Text) + scope);
                        pi.UseShellExecute = false;
                        pi.RedirectStandardOutput = true;
                        pro.StartInfo = pi;
                        pro.Start();
                        pro.WaitForExit();
                        Util.recordInfo("执行结束，开始修改cpp文件",this.tbRecordInfo);
                        //修改android cpp文件
                        editCppFile();
                        Util.recordInfo("修改完成，开始移动安卓文件到安卓目录",this.tbRecordInfo);
                        //创建java文件夹
                        createDir(androidPath + "\\android_" + this.tbProName.Text.Trim().ToLower());
                        //将生成的c++文件和java文件复制到对应的目录下
                        copyFile2Path(Util.GetAllFilesInDirectory(outPutPath), outPutPath, androidPath + "\\android_" + this.tbProName.Text.Trim().ToLower(), "java");
                        //创建c++文件夹
                        createDir(cPlusPath + "\\" + this.tbProName.Text.Trim());
                        copyFile2Path(Util.GetAllFilesInDirectory(outPutPath), outPutPath, cPlusPath + "\\" + this.tbProName.Text.Trim(), "h");
                        copyFile2Path(Util.GetAllFilesInDirectory(outPutPath), outPutPath, cPlusPath + "\\" + this.tbProName.Text.Trim(), "cpp");
                        Util.recordInfo("移动结束",this.tbRecordInfo);
                        initDir();
                    }
                    else
                    {
                        MessageBox.Show("请输入项目名称", "提示");
                    }

                }
                catch (Exception)
                {
                    MessageBox.Show("数据异常", "提示");
                }
            }
            else
            {
                MessageBox.Show("项目名称不能为空", "提示");
            }
        }

        /************************************************************************/
        /* 将文件复制到对应目录中                                                                     */
        /************************************************************************/
        private void copyFile2Path(List<FileInfo> files, String sourcePath, String targetPath, String sign)
        {
            //不拷贝名字带有ios的文件
            if (sourcePath.IndexOf("ios") <= -1)
            {
                foreach (FileInfo f in files)
                {
                    String[] suffix = f.Name.Split('.');
                    if (suffix.Length == 2)
                    {
                        if (suffix[1].Equals(sign))
                        {
                            File.Copy(sourcePath + "\\" + f.Name, targetPath + "\\" + f.Name);
                        }
                    }
                }
            }
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

        private void Form1_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;//使最大化窗口失效
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            setNeed();
            if (isSaveName)
            {
                initName();
            }
        }

        /************************************************************************/
        /* 将文件夹所有内容复制到指定文件夹中                                                                     */
        /************************************************************************/
        private void directoryCopy(string sourceDirectory, string targetDirectory) {
             if (!Directory.Exists(sourceDirectory) || !Directory.Exists(targetDirectory)) {
                 return;
             }
             DirectoryInfo sourceInfo = new DirectoryInfo(sourceDirectory);
             FileInfo[] fileInfo = sourceInfo.GetFiles();
             foreach (FileInfo fiTemp in fileInfo) {
                 File.Copy(sourceDirectory + "\\" + fiTemp.Name, targetDirectory + "\\" + fiTemp.Name, true);
             }
             DirectoryInfo[] diInfo = sourceInfo.GetDirectories();
             foreach (DirectoryInfo diTemp in diInfo) {
                 string sourcePath = diTemp.FullName;
                 string targetPath = diTemp.FullName.Replace(sourceDirectory,targetDirectory);
                 Directory.CreateDirectory(targetPath);
                 directoryCopy(sourcePath,targetPath);
             }
        }

        private void btnEditMK_Click(object sender, EventArgs e)
        {
            if (this.tbProName.Text.Trim().Length != 0)
            {
                Util.recordInfo("开始修改android.mk文件",this.tbRecordInfo);
                FileStream file = File.Open(androidMKPath, FileMode.Open);
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
                    if (txt[i].IndexOf(@"../lua/") > -1)
                    {
                        string str = @"../thirdparty/" + this.tbProName.Text.Trim() + "/BJMProxyPlugin" + System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(this.tbProName.Text) + ".cpp";
                        txt[i-1] += "\r\n" + str + " \\";
                        str = @"../thirdparty/" + this.tbProName.Text.Trim() + "/BJMProxyPlugin" + System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(this.tbProName.Text) + "_android.cpp";
                        txt[i-1] += "\r\n" + str + " \\";
                        break;
                    }
                }
                bool flaga = false;
                for (int i = 0; i < txt.Count; i++)
                {
                    if (txt[i].IndexOf("LOCAL_C_INCLUDES") > -1)
                    {
                        flaga = true;
                    }
                    if (flaga)
                    {
                        if (txt[i].IndexOf(@"../../../../cocos2d/lua/") > -1)
                        {
                            string str = "$(LOCAL_PATH)/../thirdparty/" + this.tbProName.Text;
                            txt[i - 1] += "\r\n" + str + " \\";
                            break;
                        }
                    }
                }
                bool flagb = false;
                for (int i = 0; i < txt.Count; i++)
                {
                    if (txt[i].IndexOf("LOCAL_EXPORT_C_INCLUDES") > -1)
                    {
                        flagb = true;
                    }
                    if (flagb)
                    {
                        if (txt[i].IndexOf(@"../../../../cocos2d/lua/") > -1)
                        {
                            string str = "$(LOCAL_PATH)/../thirdparty/" + this.tbProName.Text;
                            txt[i-1] += "\r\n" + str + " \\";
                            break;
                        }
                    }
                }
                file.Close();
                StreamWriter sw = new StreamWriter(androidMKPath);
                string w = "";
                sw.Write(w);
                sw.Close();
                sw = new StreamWriter(androidMKPath);
                for (int i = 0; i < txt.Count; i++)
                {
                    sw.WriteLine(txt[i]);
                }
                sw.Close();
                Util.recordInfo("结束修改android.mk文件",this.tbRecordInfo);
                openFile(@"E:\GAME_M_Engine_Proxy\BJMEngineProxy\BJMEngineProxy\classes\jni\Android.mk");
            }
            else
            {
                MessageBox.Show("请输入项目名称", "提示");
            }
        }

        private void btnDefineEdit_Click(object sender, EventArgs e)
        {
            if (this.tbProName.Text.Trim().Length != 0||this.tbCNName.Text.Trim().Length!=0)
            {
                Util.recordInfo("开始修改define.h文件",this.tbRecordInfo);
                FileStream file = File.Open(definePath, FileMode.Open);
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
                    if (txt[i].IndexOf("台湾 放手玩金流 plugin type string") > -1)
                    {
                        string str = "	/**";
                        txt[i-1] += "\r\n" + str;
                        str = "	 * 安卓[" + this.tbCNName.Text.Trim() + "]";
                        txt[i - 1] += "\r\n" + str;
                        str = "	 */";
                        txt[i - 1] += "\r\n" + str;
                        str = "	const static string PT_STR_" + this.tbProName.Text.Trim().ToUpper() + "_ANDROID = \"hwy_android_" + this.tbProName.Text.Trim().ToLower() + "\";";
                        txt[i - 1] += "\r\n" + str;
                    }
                }
                file.Close();
                StreamWriter sw = new StreamWriter(definePath);
                string w = "";
                sw.Write(w);
                sw.Close();
                sw = new StreamWriter(definePath);
                for (int i = 0; i < txt.Count; i++)
                {
                    sw.WriteLine(txt[i]);
                }
                sw.Close();
                Util.recordInfo("结束修改define.h文件",this.tbRecordInfo);
                openFile(@"E:\GAME_M_Engine_Proxy\BJMEngineProxy\BJMEngineProxy\classes\core\BJMProxyPluginDefine.h");
            }
            else
            {
                MessageBox.Show("请输入项目名称", "提示");
            }
        }

        private void btnPluginEdit_Click(object sender, EventArgs e)
        {
            if (this.tbProName.Text.Trim().Length != 0 || this.tbCNName.Text.Trim().Length != 0)
            {
                Util.recordInfo("开始修改plugin.cpp文件",this.tbRecordInfo);
                FileStream file = File.Open(createPluginPath, FileMode.Open);
                List<string> txt = new List<string>();
                using (var stream = new StreamReader(file))
                {
                    while (!stream.EndOfStream)
                    {
                        txt.Add(stream.ReadLine());
                    }
                }

                int index = 0;
                for (int i = 0; i < txt.Count; i++)
                {
                    if (txt[i].IndexOf("#include ") > -1)
                    {
                        if (txt[i+1].IndexOf("#include ") <= -1)
                        {
                            index = i;
                        }
                    }
                }
                string defineStr = "\r\n#include \"thirdparty/" + this.tbProName.Text.Trim() + "/BJMProxyPlugin" + System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(this.tbProName.Text.Trim()) + ".h\"";
                txt[index] += defineStr;

                index = 0;
                for (int i = 0; i < txt.Count; i++)
                {
                    if (txt[i].IndexOf("else if (strPluginName") > -1)
                    {
                        index = i;
                    }
                }
                String str = "    }\r\n	else if (strPluginName == PT_STR_" + this.tbProName.Text.ToUpper() + "_ANDROID){";
                str += "\r\n		pPluginBase = new CBJMProxyPlugin" + System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(this.tbProName.Text) + ";";
                str += "\r\n	}";
                if (txt[index + 3].IndexOf("else") > -1)
                {
                    txt[index + 2] = str;
                }
                else
                {
                    txt[index + 3] = str;
                }
                
                file.Close();
                StreamWriter sw = new StreamWriter(createPluginPath);
                string w = "";
                sw.Write(w);
                sw.Close();
                sw = new StreamWriter(createPluginPath);
                for (int i = 0; i < txt.Count; i++)
                {
                    sw.WriteLine(txt[i]);
                }
                sw.Close();
                Util.recordInfo("结束修改plugin.cpp文件",this.tbRecordInfo);
                openFile(@"E:\GAME_M_Engine_Proxy\BJMEngineProxy\BJMEngineProxy\classes\core\BJMProxyCreatePlugin.cpp");
            }
            else
            {
                MessageBox.Show("请输入项目名称", "提示");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Util.recordInfo(sdkConfigPath.Substring(0, sdkConfigPath.LastIndexOf("\\")),this.tbRecordInfo);
            openFile(sdkConfigPath);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Util.recordInfo(functionConfigPath.Substring(0, functionConfigPath.LastIndexOf("\\")),this.tbRecordInfo);
            openFile(functionConfigPath);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Util.recordInfo(cashConfigPath.Substring(0, cashConfigPath.LastIndexOf("\\")),this.tbRecordInfo);
            System.Diagnostics.Process.Start(cashConfigPath.Substring(0, cashConfigPath.LastIndexOf("\\")));
        }

        private void btnCreateTestPro_Click(object sender, EventArgs e)
        {
            if (this.tbProName.Text.Trim().Length > 0)
            {
                try
                {
                    Util.recordInfo("开始创建",this.tbRecordInfo);
                    FileStream myFs = new FileStream(testProPath + "//make_" + this.tbProName.Text.ToLower() + ".bat", FileMode.Create);
                    StreamWriter mySw = new StreamWriter(myFs);
                    string str = "call ant -f build.xml -DTargetPlatform=" + this.tbProName.Text.ToLower() + " -DTargetPlatform2=hwy -DResPlatform=hwy_android_" + this.tbProName.Text.ToLower() + " -DResPlatform2=null";
                    if (this.cbIsLoginWithHwy.Checked)
                    {
                        str = "call ant -f build.xml -DTargetPlatform=" + this.tbProName.Text.ToLower() + " -DTargetPlatform2=null -DResPlatform=hwy_android -DResPlatform2=null";
                        str += "\r\ncall ant -f build.xml -DTargetPlatform=" + this.tbProName.Text.ToLower() + " -DTargetPlatform2=hwy -DResPlatform=hwy_android_" + this.tbProName.Text.ToLower() + " -DResPlatform2=hwy_android";
                    }
                    mySw.WriteLine(str);
                    mySw.Close();
                    myFs.Close();
                    Util.recordInfo("结束创建，开始执行",this.tbRecordInfo);
                    Util.doBat(testProPath, "make_" + this.tbProName.Text.ToLower() + ".bat");
                    Util.recordInfo("结束执行",this.tbRecordInfo);
                }
                catch (Exception)
                {
                    MessageBox.Show("写入异常", "提示");
                }
            }
            else
            {
                MessageBox.Show("项目名称不能为空", "提示");
            }
        }

        private void btnClearRecord_Click(object sender, EventArgs e)
        {
            this.tbRecordInfo.Text = "";
        }

        /************************************************************************/
        /* 打开文件                                                                     */
        /************************************************************************/
        private void openFile(string path)
        {
            try
            {
                System.Diagnostics.Process.Start(@"E:\editplus\EditPlus_x64\x64\EditPlus.exe", path);
            }
            catch (Exception)
            {
                System.Diagnostics.Process.Start(@"Notepad", path);
            }
        }

        private void menuAbout_Click(object sender, EventArgs e)
        {
            FileStream file = File.Open(aboutPath, FileMode.Open);
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
                if (i == 0)
                {
                    this.tbRecordInfo.Text += "当前版本:"+version+"\r\n制作人:shy"+"\r\n\r\n"+txt[i];
                }
                else
                {
                    this.tbRecordInfo.Text += "\r\n" + txt[i]; 
                }
            }
        }

        private void menuSet_Click(object sender, EventArgs e)
        {
            Setting s = new Setting();
            s.ShowDialog();
        }

        private void menuHelp_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(@"E:\GAME_M_Engine_Proxy\BJMEngineProxyTest\proj\tools\doc\安卓接入流程.docx");
            }
            catch (Exception)
            {
                MessageBox.Show("无法打开", "提示");
            }
        }

        /************************************************************************/
        /* 初始化文件夹                                                                     */
        /************************************************************************/
        private void initEnv()
        {
            if (!File.Exists(@"E:\hwy_android_package_temp") && !File.Exists(@"E:\hwy_android_temp"))
            {
                try
                {
                    createDir(@"E:\hwy_android_package_temp");
                    createDir(@"E:\hwy_android_temp");
                    directoryCopy(System.IO.Directory.GetCurrentDirectory() + "\\hwy_android_package_temp", @"E:\hwy_android_package_temp");
                    directoryCopy(System.IO.Directory.GetCurrentDirectory() + "\\hwy_android_temp", @"E:\hwy_android_temp");
                }
                catch (Exception)
                {
                    MessageBox.Show("初始化异常", "提示");
                }
            }
        }

        private void btnTargetFolder_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(targetResPath);
        }

        private void btnOpenServerFolder_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(serverResPath);
        }

        /************************************************************************/
        /* 初始化文件夹                                                                     */
        /************************************************************************/
        private void initDir()
        {
            try
            {
                Util.recordInfo("开始移动所需文件夹",this.tbRecordInfo);
                createDir(resOtherPath + "hwy_android_" + this.tbProName.Text.ToLower());
                directoryCopy(baseaPath, resOtherPath + "hwy_android_" + this.tbProName.Text.ToLower());
                createDir(resPath + "hwy_android_" + this.tbProName.Text.ToLower());
                directoryCopy(basebPath, resPath + "hwy_android_" + this.tbProName.Text.ToLower());
                Util.recordInfo("移动结束",this.tbRecordInfo);
            }
            catch (Exception)
            {
                MessageBox.Show("文件夹异常","提示");
            }
        }

        /************************************************************************/
        /* 修改cpp文件                                                                     */
        /************************************************************************/
        private void editCppFile()
        {
            try
            {
                FileStream file = File.Open(outPutPath + "\\BJMProxyPlugin" + System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(this.tbProName.Text) + "_android.cpp", FileMode.Open);
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
                    if (txt[i].IndexOf("MEDIATOR") > -1)
                    {
                        txt[i] = "#define MEDIATOR \"sdk/proxy/android_" + this.tbProName.Text.ToLower() + "/BJMProxy" + System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(this.tbProName.Text) + "SdkLibMediator\"";
                        break;
                    }
                }
                file.Close();
                StreamWriter sw = new StreamWriter(outPutPath + "\\BJMProxyPlugin" + System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(this.tbProName.Text) + "_android.cpp");
                string w = "";
                sw.Write(w);
                sw.Close();
                sw = new StreamWriter(outPutPath + "\\BJMProxyPlugin" + System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(this.tbProName.Text) + "_android.cpp");
                for (int i = 0; i < txt.Count; i++)
                {
                    sw.WriteLine(txt[i]);
                }
                sw.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("修改异常","提示");
            }
        }

        private void btnAddFunctionConfig_Click(object sender, EventArgs e)
        {
            if (this.tbProName.Text.Trim().Length > 0)
            {
                try
                {
                    Util.recordInfo("开始修改functionConfig",this.tbRecordInfo);
                    FileStream file = File.Open(functionConfigPath, FileMode.Open);
                    List<string> txt = new List<string>();
                    using (var stream = new StreamReader(file))
                    {
                        while (!stream.EndOfStream)
                        {
                            txt.Add(stream.ReadLine());
                        }
                    }
                    int index = 0;
                    for (int i = 0; i < txt.Count; i++)
                    {
                        if (txt[i].IndexOf("else if (strPluginName") > -1)
                        {
                            index = i;
                        }
                    }
                    string str = "\r\n    hwy_android_" + this.tbProName.Text.ToLower() + " = {";
                    if (this.cbUser.Checked)
                    {
                        str += "\r\n        account = true,";
                    }
                    else
                    {
                        str += "\r\n        account = false,";
                    }
                    if (this.cbCash.Checked)
                    {
                        str += "\r\n        cash = true,";
                    }
                    else
                    {
                        str += "\r\n        cash = false,";
                    }
                    if (this.cbShare.Checked)
                    {
                        str += ",\r\n        share = true,";
                    }
                    else
                    {
                        str += "\r\n        share = false,";
                    }
                    if (this.cbAd.Checked)
                    {
                        str += "\r\n        ad = true,";
                    }
                    else
                    {
                        str += "\r\n        ad = false,";
                    }
                    if (this.cbQa.Checked)
                    {
                        str += "\r\n        qa = true,";
                    }
                    else
                    {
                        str += "\r\n        qa = false,";
                    }
                    if (this.cbBbs.Checked)
                    {
                        str += "\r\n        bbs = true,";
                    }
                    else
                    {
                        str += "\r\n        bbs = false,";
                    }
                    if (this.cbUc.Checked)
                    {
                        str += "\r\n        uc = true";
                    }
                    else
                    {
                        str += "\r\n        uc = false";
                    }
                    str += "\r\n    },";
                    for (int i = 0; i < txt.Count; i++)
                    {
                        if (txt[i].IndexOf("return functionConfig") > -1)
                        {
                            txt[i - 3] += str;
                        }
                    }
                    file.Close();
                    StreamWriter sw = new StreamWriter(functionConfigPath);
                    string w = "";
                    sw.Write(w);
                    sw.Close();
                    sw = new StreamWriter(functionConfigPath);
                    for (int i = 0; i < txt.Count; i++)
                    {
                        sw.WriteLine(txt[i]);
                    }
                    sw.Close();
                    Util.recordInfo("结束修改functionConfig",this.tbRecordInfo);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("修改异常", "提示");
                }
            }
            else
            {
                MessageBox.Show("项目名称不能为空", "提示");
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isSaveName)
            {
                saveName();
            }
            Application.Exit();
        }

        private void btnOpenPkg_Click(object sender, EventArgs e)
        {
            try
            {
                Util.clearAllFilesDir(genPkgPath, "test");
            }
            catch (Exception) 
            {
                MessageBox.Show("清空异常", "提示");
            }
            Util.doBat(@"E:\GAME_M_Engine_Proxy\BJMEngineProxyTest\proj\tools", "makepkg_android.bat");
        }

        private void btnOpenApk_Click(object sender, EventArgs e)
        {
            try
            {
                Util.clearAllFilesDir(genApkPath, "test2");
            }
            catch (Exception)
            {
                MessageBox.Show("清空异常", "提示");
            }
            Util.doBat(@"E:\GAME_M_Engine_Proxy\BJMEngineProxyTest\proj\tools", "make_apks.bat");
        }

        private void btnOpenPla_Click(object sender, EventArgs e)
        {
            Util.recordInfo(plaformPath,this.tbRecordInfo);
            System.Diagnostics.Process.Start(plaformPath);
        }

        private void btnReplacePkg_Click(object sender, EventArgs e)
        {
            try
            {
                Util.recordInfo("开始替换PKG文件",this.tbRecordInfo);
                File.Delete(targetPkgaPath);
                File.Copy(pkgPath, targetPkgaPath);
                File.Delete(targetPkgbPath);
                File.Copy(pkgPath, targetPkgbPath);
                Util.recordInfo("结束替换PKG文件",this.tbRecordInfo);
            }
            catch (Exception)
            {
                MessageBox.Show("替换异常", "提示");
            }
        }

        /************************************************************************/
        /* 初始化信息                                                                     */
        /************************************************************************/
        private void initName()
        {
            FileStream file = File.Open(infoPath, FileMode.Open);
            List<string> txt = new List<string>();
            using (var stream = new StreamReader(file))
            {
                while (!stream.EndOfStream)
                {
                    txt.Add(stream.ReadLine());
                }
            }

            string[] names = txt[txt.Count - 1].Split(';');
            lastEnName = names[0];
            lastCnName = names[1];
            this.tbProName.Text = lastEnName;
            this.tbCNName.Text = lastCnName;
        }

        /************************************************************************/
        /* 保存英文名和中文名                                                                     */
        /************************************************************************/
        private void saveName()
        {
            try
            {
                FileStream file = File.Open(infoPath, FileMode.Open);
                List<string> txt = new List<string>();
                using (var stream = new StreamReader(file))
                {
                    while (!stream.EndOfStream)
                    {
                        txt.Add(stream.ReadLine());
                    }
                }

                StreamWriter sw = new StreamWriter(infoPath);
                if (txt.Count != 0)
                {
                    txt[txt.Count - 1] = this.tbProName.Text + ";" + this.tbCNName.Text;
                    for (int i = 0; i < txt.Count; i++)
                    {
                        sw.Write(txt[i]);
                    }
                }
                else
                {
                    sw.Write(this.tbProName.Text + ";" + this.tbCNName.Text);
                }
                sw.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("数据异常", "提示");
            }
        }

        /************************************************************************/
        /* 设置信息                                                                     */
        /************************************************************************/
        private void setNeed()
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
                   if (txt[i].IndexOf("saveLastName") > -1)
                    {
                        if (txt[i + 1].Equals("YES"))
                        {
                            isSaveName = true;
                        }
                        else
                        {
                            isSaveName = false;
                        }
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("数据异常", "提示");
            }
        }

        private void btnOpenSdkRes_Click(object sender, EventArgs e)
        {
            Util.recordInfo(sdkResPath, this.tbRecordInfo);
            try
            {
                if (this.tbProName.Text.Trim().Length != 0)
                {
                    System.Diagnostics.Process.Start(sdkResPath + "\\hwy_android_" + this.tbProName.Text.Trim());
                }
                else
                {
                    System.Diagnostics.Process.Start(sdkResPath);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("打开异常", "提示");
            }
        }

        private void btnOpenTool_Click(object sender, EventArgs e)
        {
            Util.recordInfo(toolPath,this.tbRecordInfo);
            try
            {
                if (this.tbProName.Text.Trim().Length != 0)
                {
                    Util.doBat(toolPath, "make_" + this.tbProName.Text.Trim() + ".bat");
                }
                else
                {
                    System.Diagnostics.Process.Start(toolPath);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("打开异常", "提示");
            }
        }
    }
}
