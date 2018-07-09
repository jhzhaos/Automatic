using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Drawing;

namespace PostTool
{
  public class Tool
    {

        public static void Write(string filePath,string strCont)
        {
            FileStream fs = new FileStream(filePath, FileMode.Create);
            //获得字节数组
            byte[] data = System.Text.Encoding.Default.GetBytes(strCont);
            //开始写入
            fs.Write(data, 0, data.Length);
            //清空缓冲区、关闭流
            fs.Flush();
            fs.Close();
        }

        /// <summary>
        /// 获取Html字符串中指定标签的指定属性的值 
        /// </summary>
        /// <param name="html">Html字符</param>
        /// <param name="tag">指定标签名</param>
        /// <param name="attr">指定属性名</param>
        /// <returns></returns>
        public static string GetHtmlAttr(string html, string tag, string val, string title)
        {
            var typeid = string.Empty;
            Regex re = new Regex(@"(<" + tag + @"[\w\W].+?>)");
            MatchCollection imgreg = re.Matches(html);
            Regex attrReg = new Regex(@"([a-zA-Z1-9_-]+)\s*=\s*(\x27|\x22)([^\x27\x22]*)(\x27|\x22)", RegexOptions.IgnoreCase);
            for (int i = 0; i < imgreg.Count; i++)
            {
                MatchCollection matchs = attrReg.Matches(imgreg[i].ToString());
                for (int j = 0; j < matchs.Count; j++)
                {
                    GroupCollection groups = matchs[j].Groups;

                    if (matchs.Count == 6 && matchs[5].Value.ToUpper().Contains(title) && val.ToUpper() == groups[1].Value.ToUpper())
                    {
                        typeid = groups[3].Value;
                        break;
                    }
                }
                if (!string.IsNullOrEmpty(typeid))
                {
                    break;
                }

            }

            return typeid;

        }


        public static string updateCookie(string oldcookie, string newcookie)
        {
            List<string> oldcookielist = new List<string>();
            if (oldcookie.Contains(";"))
                oldcookielist = new List<string>(oldcookie.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries));
            else
                oldcookielist.Add(oldcookie);

            List<string> newcookielist = new List<string>();
            if (newcookie.Contains(";"))
                newcookielist = new List<string>(newcookie.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries));
            else
                newcookielist.Add(newcookie);

            foreach (string cookie in newcookielist)
            {
                //Console.WriteLine("cookie:" + cookie);
                if (!string.IsNullOrWhiteSpace(cookie))
                {
                    if (!string.IsNullOrWhiteSpace(cookie.Split('=')[1])) //判断cookie的value值是为空,不为空时才进行操作
                    {
                        bool isFind = false; //判断是否是新值
                        for (int i = 0; i < oldcookielist.Count; i++)
                        {
                            if (cookie.Split('=')[0] == oldcookielist[i].Split('=')[0])
                            {
                                oldcookielist[i] = cookie;
                                isFind = true;
                                break;
                            }
                        }

                        if (!isFind) //如果计算后还是false,则表示newcookie里出现新值了,将新值添加到老cookie里
                            oldcookielist.Add(cookie);
                    }
                }
            }

            oldcookie = string.Empty;

            for (int i = 0; i < oldcookielist.Count; i++)
                oldcookie += oldcookielist[i] + ";";

            return oldcookie;
        }

        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        #region 数组组合
        public static byte[] ComposeArrays(byte[] Array1, byte[] Array2)
        {
            byte[] Temp = new byte[Array1.Length + Array2.Length];
            Array1.CopyTo(Temp, 0);
            Array2.CopyTo(Temp, Array1.Length);
            return Temp;
        }
        #endregion

        #region 图片转Byte数组
        public static byte[] ImageToBytesFromFilePath(string FilePath)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (Image Img = Image.FromFile(FilePath))
                {
                    using (Bitmap Bmp = new Bitmap(Img))
                    {
                        Bmp.Save(ms, Img.RawFormat);
                    }
                }
                return ms.ToArray();
            }
        }
        #endregion

        public static string GetInputVal(string html,string inputHtml)
        {
            return Regex.Match(html, @"(?is)"+inputHtml).Groups["value"].Value;   
        }


        public static void Read(string path)
        {
            StreamReader sr = new StreamReader(path, Encoding.Default);
            String line;
            while ((line = sr.ReadLine()) != null)
            {
                Console.WriteLine(line.ToString());
            }
        }
        /// <summary>  
        /// 获得指定目录下的所有文件  
        /// </summary>  
        /// <param name="path"></param>  
        /// <returns></returns>  
        public List<FileInfo> GetFilesByDir(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);

            //找到该目录下的文件  
            FileInfo[] fi = di.GetFiles("*.txt");

            //把FileInfo[]数组转换为List  
            List<FileInfo> list = fi.ToList<FileInfo>();
            return list;
        }

        public static void DeleteFile(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path);
            }
        }

        public void GetAllFiles(string path, List<FileInfo> tion)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            foreach (FileInfo file in dir.GetFiles("*.txt"))//设置文件类型  
            {
                tion.Add(file); //网StringCollection里面添加文件名  

            }
            foreach (DirectoryInfo subDire in dir.GetDirectories("*")) //操作子目录  

            {
                GetAllFiles(subDire.FullName, tion); //递归  
            }
        }
      
    }
}
