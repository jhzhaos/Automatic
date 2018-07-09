using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeService
{
   public class LogWrite
    {        
        public static void Write(string str)
        {
            string dt =DateTime.Now.ToString()+":"+ str+ "\r\n";
            int len = dt.Length;
            byte[] inputByts = new byte[len];
            inputByts = Encoding.GetEncoding("utf-8").GetBytes(dt);
            System.IO.FileStream fs = new System.IO.FileStream(AppDomain.CurrentDomain.BaseDirectory+GetValue("LogPath"), System.IO.FileMode.OpenOrCreate);
            fs.Seek(0, System.IO.SeekOrigin.End);
            fs.Write(inputByts, 0, inputByts.Length);
            fs.Close();//关闭流
        }

        /// <summary>  
        /// 读取指定key的值  
        /// </summary>  
        /// <param name="key"></param>  
        /// <returns></returns>  
        public static string GetValue(string key)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.AppSettings.Settings[key] == null)
                return "";
            else
                return config.AppSettings.Settings[key].Value;
        }
    }
}
