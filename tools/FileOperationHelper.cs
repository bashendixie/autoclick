using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public class FileOperationHelper
    {
        /// <summary>
        /// 读取文件内容到字符串
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <param name="mode">操作模式</param>
        /// <returns></returns>
        public static string ReadStringFromFile(string filepath, FileMode mode)
        {
            try
            {
                using (FileStream fs = new FileStream(filepath, mode))
                {
                    StreamReader sr = new StreamReader(fs, Encoding.GetEncoding("utf-8"));
                    return sr.ReadToEnd().Trim();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("", ex);// "文件读取失败", ex);
            }
        }

        /// <summary>
        /// 将字符串写入到文件
        /// </summary>
        /// <param name="content">字符串</param>
        /// <param name="filePath">文件路径</param>
        /// <param name="mode">文件操作模式</param>
        /// <returns></returns>
        public static bool WriteStringToFile(string content, string filePath, FileMode mode)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                FileStream fs = new FileStream(filePath, mode);
                byte[] data = System.Text.Encoding.UTF8.GetBytes(content);
                //开始写入
                fs.Write(data, 0, data.Length);
                //清空缓冲区、关闭流
                fs.Flush();
                fs.Close();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("", ex); //("文件写入失败", ex);
            }
        }
        /// <summary>
        /// 获取文件夹下所有Xml文件名
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<string> GetFileList(string path)
        {
            DirectoryInfo folder = new DirectoryInfo(path);
            List<string> files = new List<string>();
            foreach (FileInfo file in folder.GetFiles("*.xml"))
            {
                files.Add(file.Name);
            }
            return files;
        }

        /// <summary>
        /// 获取文件夹下所有文件名
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<string> GetAllFileList(string path)
        {
            DirectoryInfo folder = new DirectoryInfo(path);
            List<string> files = new List<string>();
            foreach (FileInfo file in folder.GetFiles())
            {
                files.Add(file.Name);
            }
            return files;
        }

        /// <summary>
        /// 获取文件夹下所有文件夹名
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<string> GetAllDirectoryList(string path)
        {
            DirectoryInfo folder = new DirectoryInfo(path);
            List<string> files = new List<string>();
            foreach (DirectoryInfo dir in folder.GetDirectories())
            {
                files.Add(dir.Name);
            }
            return files;
        }

        /// <summary>
        /// 文件重命名，直到不重名为止
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetReNameFile(string path)
        {
            string newName;
            int i = 1;
            do
            {
                newName = "";
                newName += Path.GetDirectoryName(path);
                newName += "\\" + Path.GetFileNameWithoutExtension(path) + "-" + i + Path.GetExtension(path);
                i++;
            }
            while (File.Exists(newName));
            return newName;
        }
        /// <summary>
        /// 获取文件夹内
        /// 当前重名文件之后，还有多少个重名的文件
        /// </summary>
        /// <param name="fileNames"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static int GetNumAfterThisFile(string[] fileNames, int i)
        {
            int num = 0;
            for (int y = 0; y < i; y++)
            {
                //if (y>(i - (fileNames.Length - 1))) 
                {
                    if (File.Exists(fileNames[y]))
                    {
                        num++;
                    }
                }
            }
            return num;
        }

        /// <summary>
        /// 目录下所有内容复制
        /// </summary>
        /// <param name="SourcePath">要Copy的文件夹</param>
        /// <param name="DestinationPath">要复制到哪个地方</param>
        /// <param name="overwriteexisting">是否覆盖</param>
        /// <returns></returns>
        public static bool CopyDirectory(string SourcePath, string DestinationPath, bool overwriteexisting)
        {
            bool ret = false;
            try
            {
                SourcePath = SourcePath.EndsWith(@"\") ? SourcePath : SourcePath + @"\";
                DestinationPath = DestinationPath.EndsWith(@"\") ? DestinationPath : DestinationPath + @"\";

                if (Directory.Exists(SourcePath))
                {
                    if (Directory.Exists(DestinationPath) == false)
                        Directory.CreateDirectory(DestinationPath);

                    foreach (string fls in Directory.GetFiles(SourcePath))
                    {
                        FileInfo flinfo = new FileInfo(fls);
                        flinfo.CopyTo(DestinationPath + flinfo.Name, overwriteexisting);
                    }
                    foreach (string drs in Directory.GetDirectories(SourcePath))
                    {
                        DirectoryInfo drinfo = new DirectoryInfo(drs);
                        if (CopyDirectory(drs, DestinationPath + drinfo.Name, overwriteexisting) == false)
                            ret = false;
                    }
                }
                else
                {

                    //复制单个文件用的
                    SourcePath = SourcePath.EndsWith(@"\") ? SourcePath.Substring(0, SourcePath.Length - 1) : SourcePath;
                    if (File.Exists(SourcePath))
                    {
                        FileInfo flinfo = new FileInfo(SourcePath);
                        flinfo.CopyTo(DestinationPath + flinfo.Name, overwriteexisting);
                    }
                }
                ret = true;
            }
            catch (Exception)
            {
                DeleteFolder(DestinationPath);
                ret = false;
            }
            return ret;
        }

        /// <summary>
        /// 删除目录及内部所有文件
        /// </summary>
        /// <param name="dir">要删除的目录</param>
        public static void DeleteFolder(string dir)
        {
            try
            {
                if (System.IO.Directory.Exists(dir))
                {
                    string[] fileSystemEntries = System.IO.Directory.GetFileSystemEntries(dir);
                    for (int i = 0; i < fileSystemEntries.Length; i++)
                    {
                        string text = fileSystemEntries[i];
                        if (System.IO.File.Exists(text))
                        {
                            System.IO.File.Delete(text);
                        }
                        else
                        {
                            DeleteFolder(text);
                        }
                    }
                    System.IO.Directory.Delete(dir);
                }
                if (System.IO.File.Exists(dir))
                {
                    System.IO.File.Delete(dir);
                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 判断指定文件夹及子文件夹内是否有文件正被打开
        /// 1.文件夹内有文件被打开 0.没有 -1.文件夹不存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static int IsFileOpened(string dir)
        {
            if (System.IO.Directory.Exists(dir))
            {
                string[] fileSystemEntries = System.IO.Directory.GetFileSystemEntries(dir);
                for (int i = 0; i < fileSystemEntries.Length; i++)
                {
                    string text = fileSystemEntries[i];
                    if (System.IO.File.Exists(text))
                    {
                        try
                        {
                            System.IO.File.Move(text, text);
                        }
                        catch (Exception)
                        {
                            return 1;
                        }
                    }
                    else
                    {
                        //递归时外层要先接收内层的结果,无法直接多层catch出去
                        if (IsFileOpened(text) == 1)
                            return 1;
                    }
                }
                return 0;
            }
            return -1;
        }

        /// <summary>
        /// 计算文件的大小
        /// </summary>
        /// <param name="lengthOfDocument"></param>
        /// <returns></returns>
        public static string GetLength(long lengthOfDocument)
        {
            if (lengthOfDocument < 1024)
                return string.Format(lengthOfDocument.ToString() + 'B');
            else if (lengthOfDocument > 1024 && lengthOfDocument <= Math.Pow(1024, 2))
                return string.Format(Math.Floor(lengthOfDocument / 1024.0).ToString() + "KB");
            else if (lengthOfDocument > Math.Pow(1024, 2) && lengthOfDocument <= Math.Pow(1024, 3))
                return string.Format(Math.Round(lengthOfDocument / 1024.0 / 1024.0, 2).ToString() + "M");
            else
                return string.Format(Math.Round(lengthOfDocument / 1024.0 / 1024.0 / 1024.0, 2).ToString() + "GB");
        }

        /// <summary>
        /// 判断文件是word还是excel
        /// 1.word; 2.excel; 0.其他
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static int IsFileWordOrExcel(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                string fileExtension = Path.GetExtension(filePath);
                if (fileExtension == ".docx" || fileExtension == ".doc")
                    return 1;
                else if (fileExtension == ".xls" || fileExtension == ".xlsx")
                    return 2;
                else
                    return 0;
            }
            else
                return 0;
        }

        /// <summary>
        /// 获取文件的创建时间
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetFileCreationTime(string filePath)
        {
            FileInfo fi = new FileInfo(filePath);
            if (fi != null)
                return fi.LastWriteTime.ToString();//创建时间同一分钟内可能会相同,不清楚机制,暂时调整为修改时间;
            else
                return "";
        }

        /// <summary>
        /// 判断文件夹内是否有同名文件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool IsFileNameExist(string name, string filePath)
        {
            if (System.IO.Directory.Exists(filePath))
            {
                DirectoryInfo theFolder = new DirectoryInfo(filePath);
                FileInfo[] fileList = theFolder.GetFiles();
                foreach (FileInfo fileItem in fileList)
                {
                    if (Path.GetFileNameWithoutExtension(fileItem.FullName) == name)
                        return true;
                }
            }

            return false;
        }

        public static void RecursionDirector(string dirs, TreeNode node)
        {
            //绑定到指定的文件夹目录
            DirectoryInfo dir = new DirectoryInfo(dirs);
            //检索表示当前目录的文件和子目录
            FileSystemInfo[] fsinfos = dir.GetFileSystemInfos();
            //遍历检索的文件和子目录
            foreach (FileSystemInfo fsinfo in fsinfos)
            {
                //判断是否为空文件夹　　
                if (fsinfo is DirectoryInfo)
                {
                    TreeNode temp = new TreeNode();
                    temp.Name = fsinfo.FullName;
                    temp.Text = fsinfo.FullName;
                    node.Nodes.Add(temp);
                    //递归调用
                    RecursionDirector(fsinfo.FullName, temp);
                }
                /*else
                {
                    //Console.WriteLine(fsinfo.FullName);
                    //将得到的文件全路径放入到集合中
                    //list.Add(fsinfo.FullName);
                }*/
            }
        }

        public static string ReadFileOwner(string path)
        {
            var fs = System.IO.File.GetAccessControl(path);
            var sid = fs.GetOwner(typeof(SecurityIdentifier));
            var ntAccount = sid.Translate(typeof(NTAccount));
            return ntAccount.ToString();
        }

        public static ImageCodecInfo GetImageCodecInfo(ImageFormat format)
        {
            ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();

            foreach (ImageCodecInfo icf in encoders)
            {
                if (icf.FormatID == format.Guid)
                {
                    return icf;
                }
            }

            return null;
        }
    }
}
