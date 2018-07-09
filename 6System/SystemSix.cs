using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using PostTool;
using CsharpHttpHelper;
using System.Drawing;
using CsharpHttpHelper.Enum;

namespace _6System
{
    public class SystemSix
    {
#region 测试代码
        public static void post66()
        {

            string strIMSPhone = "zhaojihong";
            string strIMSPwd = "zhaojihong";
            string postData = "username=" + strIMSPhone + "&password=" + strIMSPwd;
            HttpHelper http = new HttpHelper();
            HttpItemJi item = new HttpItemJi
            {
                URL = "http://2186.soudedao.net/index.php?m=site&c=index&a=login",
                Method = "post",
                Postdata = postData,
                Host = "2186.soudedao.net",
                ContentType = "application/x-www-form-urlencoded",
                KeepAlive = true,
                Allowautoredirect = true,
                AutoRedirectCookie = true
            };
            item.Header.Add("Upgrade-Insecure-Requests", "1");
            HttpResult result = http.GetHtml(item);
            string html = result.Html;
            if (html.IndexOf("登录成功！") > -1)
            {
                item = new HttpItemJi()
                {
                    URL = "http://2186.soudedao.net/index.php?m=site&c=content&a=product_form",
                    Method = "POST",
                    ContentType = "application/x-www-form-urlencoded",
                    Host = "2186.soudedao.net",
                    PostEncoding = Encoding.UTF8,
                    Cookie = result.Cookie,
                };
                item.Header.Add("Upgrade-Insecure-Requests", "1");
                var addHtml = http.GetHtml(item).Html;
                var tasks = new Task[10];
                Task.Factory.StartNew(() =>
                  {
                      for (int i = 0; i < 10; i++)
                      {
                          post6Task(result.Cookie, addHtml);
                      }
                  });
            }

        }

        public static void post6Task(string cookies, string addHtml)
        {
            HttpHelper http = new HttpHelper();
            var access_key = Regex.Match(addHtml, @"(?is)<input type=""hidden"" name=""access_key"" value=""(?<value>[^""]+)"">").Groups["value"].Value;
            var typeId = Tool.GetHtmlAttr(addHtml, "input", "value", "二级分类8");
            #region 变量
            string boundary = string.Format("----{0}", DateTime.Now.Ticks.ToString("x"));    // 分界线可以自定义参数
            byte[] UploadBuffers = null;
            StringBuilder UploadBuf = new StringBuilder();
            #endregion

            #region 头部数据
            UploadBuf.Append("--" + boundary + "\r\n");
            UploadBuf.Append(@"Content-Disposition: form-data; name=""logo""; filename=""test.jpg""" + "\r\n");
            UploadBuf.Append("Content-Type: image/jpeg\r\n\r\n");
            byte[] HeadBytes = Encoding.ASCII.GetBytes(UploadBuf.ToString());
            #endregion

            #region 图片数据
            byte[] PicBytes = Tool.ImageToBytesFromFilePath(string.Format(@"E:\\test.jpg"));
            #endregion

            #region 尾部数据
            UploadBuf.Clear();
            UploadBuf.Append("\r\n--" + boundary + "--\r\n");
            byte[] TailBytes = Encoding.ASCII.GetBytes(UploadBuf.ToString());
            #endregion

            #region 数组拼接
            UploadBuffers = Tool.ComposeArrays(HeadBytes, PicBytes);
            UploadBuffers = Tool.ComposeArrays(UploadBuffers, TailBytes);
            #endregion

            #region 上传
            var item = new HttpItemJi()
            {
                URL = "http://2186.soudedao.net/index.php?m=site&c=content&a=save_logo&type=product&id=&access_key=" + access_key,
                Method = "POST",
                ContentType = "multipart/form-data; boundary=" + boundary,
                PostDataType = PostDataType.Byte,
                PostEncoding = Encoding.UTF8,
                PostdataByte = UploadBuffers,
                Cookie = cookies
            };
            item.Header.Add("Upgrade-Insecure-Requests", "1");
            #endregion
            http.FastRequest(item);
            var postDataContent = "info[title]=zhaojicccchaha&info[seo_title]=zhaojihonghaha&info[keywords]=zhaojihonghaha&info[description]=zhaojihonghaha&cat_id[]=" + typeId + "&info[listorder]=0&access_key=" + access_key + "&content=<p>zzzzzz</p>";
            item = new HttpItemJi
            {
                URL = "http://2186.soudedao.net/index.php?m=site&c=content&a=product_form",
                Method = "post",
                Postdata = postDataContent,
                Host = "2186.soudedao.net",
                ContentType = "application/x-www-form-urlencoded",
                KeepAlive = true,
                Allowautoredirect = true,
                AutoRedirectCookie = true,
                Cookie = cookies
            };
            item.Header.Add("Upgrade-Insecure-Requests", "1");
            var html = http.FastRequest(item).Html;
        }
#endregion

        public static bool LoginTest(string username, string passWord, string host, string webAdminUrl,out string sixHost)
        {
            string postData = "username=" + username + "&password=" + passWord;
            if (webAdminUrl == "")
            {
                webAdminUrl = "/index.php?m=site&c=index&a=login";
            }        
            HttpHelper http = new HttpHelper();
            HttpItemJi item = new HttpItemJi
            {
                URL = host + webAdminUrl,
                Method = "post",
                Postdata = postData,              
                ContentType = "application/x-www-form-urlencoded",
                KeepAlive = true,
                Allowautoredirect = true,
                AutoRedirectCookie = true
            };
            item.Header.Add("Upgrade-Insecure-Requests", "1");
            HttpResult result = http.GetHtml(item);
            string html = result.Html;  
            var hostTitle=HttpHelper.GetBetweenHtml(html, "http://", webAdminUrl);
            sixHost = "http://"+hostTitle;
            item = new HttpItemJi
            {
                URL = "http://"+hostTitle + "/index.php?m=site&c=index&a=login",
                Method = "post",
                Postdata = postData,             
                ContentType = "application/x-www-form-urlencoded",
                KeepAlive = true,
                Allowautoredirect = true,
                AutoRedirectCookie = true
            };
            item.Header.Add("Upgrade-Insecure-Requests", "1");
            result = http.GetHtml(item);
            html = result.Html;
            if (html.IndexOf("登录成功！") > -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }      

        public static Task<string> GetCookie(string username, string passWord, string host,string webAdminUrl)
        {          
            string postData = "username=" + username + "&password=" + passWord;
            if (webAdminUrl == "")
            {
                webAdminUrl = "/index.php?m=site&c=index&a=login";
            }
            HttpHelper http = new HttpHelper();
            HttpItemJi item = new HttpItemJi
            {
                URL = host + webAdminUrl,
                Method = "post",
                Postdata = postData,
                Host = host.Replace("http://", ""),
                ContentType = "application/x-www-form-urlencoded",
                KeepAlive = true,
                Allowautoredirect = true,
                AutoRedirectCookie = true
            };
            item.Header.Add("Upgrade-Insecure-Requests", "1");
            HttpResult result = http.GetHtml(item);
            string html = result.Html;
            var hostTitle = HttpHelper.GetBetweenHtml(html, "http://", webAdminUrl);
            item = new HttpItemJi
            {
                URL = "http://" + hostTitle + "/index.php?m=site&c=index&a=login",
                Method = "post",
                Postdata = postData,
                ContentType = "application/x-www-form-urlencoded",
                KeepAlive = true,
                Allowautoredirect = true,
                AutoRedirectCookie = true
            };
            item.Header.Add("Upgrade-Insecure-Requests", "1");
            result = http.GetHtml(item);
            html = result.Html;
            return Task<string>.Run(() =>
            {
                if (html.IndexOf("登录成功！") > -1)
            {
                return result.Cookie;
            }
            else
            {
                return string.Empty;
            }
            });

        }

        public static Task<string> post6Auto(string cookie, string webCategoryCode, string host, string typeId, string title, string keyWord, string context, string imgUrl)
        {
            return Task<string>.Run(() =>
            {
                HttpHelper http = new HttpHelper();
                var item = new HttpItemJi()
                {
                    URL = host + "/index.php?m=site&c=content&a=" + webCategoryCode,
                    Method = "POST",
                    ContentType = "application/x-www-form-urlencoded",                 
                    PostEncoding = Encoding.UTF8,
                    Cookie = cookie,
                };
                item.Header.Add("Upgrade-Insecure-Requests", "1");
                var addHtml = http.GetHtml(item).Html;
                var access_key = Regex.Match(addHtml, @"(?is)<input type=""hidden"" name=""access_key"" value=""(?<value>[^""]+)"">").Groups["value"].Value;
                if (!string.IsNullOrEmpty(imgUrl))
                {
                    #region 变量
                    string boundary = string.Format("----{0}", DateTime.Now.Ticks.ToString("x"));    // 分界线可以自定义参数
                    byte[] UploadBuffers = null;
                    StringBuilder UploadBuf = new StringBuilder();
                    #endregion

                    #region 头部数据
                    UploadBuf.Append("--" + boundary + "\r\n");
                    UploadBuf.Append(@"Content-Disposition: form-data; name=""logo""; filename=""test.jpg""" + "\r\n");
                    UploadBuf.Append("Content-Type: image/jpeg\r\n\r\n");
                    byte[] HeadBytes = Encoding.ASCII.GetBytes(UploadBuf.ToString());
                    #endregion

                    #region 图片数据
                    byte[] PicBytes = Tool.ImageToBytesFromFilePath(string.Format(@"E:\\test.jpg"));
                    #endregion

                    #region 尾部数据
                    UploadBuf.Clear();
                    UploadBuf.Append("\r\n--" + boundary + "--\r\n");
                    byte[] TailBytes = Encoding.ASCII.GetBytes(UploadBuf.ToString());
                    #endregion

                    #region 数组拼接
                    UploadBuffers = Tool.ComposeArrays(HeadBytes, PicBytes);
                    UploadBuffers = Tool.ComposeArrays(UploadBuffers, TailBytes);
                    #endregion

                    #region 上传
                    item = new HttpItemJi()
                    {
                        URL = "http://2186.soudedao.net/index.php?m=site&c=content&a=save_logo&type=product&id=&access_key=" + access_key,
                        Method = "POST",
                        ContentType = "multipart/form-data; boundary=" + boundary,
                        PostDataType = PostDataType.Byte,
                        PostEncoding = Encoding.UTF8,
                        PostdataByte = UploadBuffers,
                        Cookie = cookie
                    };
                    item.Header.Add("Upgrade-Insecure-Requests", "1");
                    http.FastRequest(item);
                    #endregion
                    Thread.Sleep(2000);
                }
                var desc = context.Substring(0, 10);
                if (context.Length > 100)
                {
                    desc = context.Substring(0, 100);
                }
                var postDataContent = string.Format(@"info[title]={0}&info[seo_title]={1}&info[keywords]={2}&info[description]={3}
&cat_id[]={4}&info[listorder]=0&access_key={5}&content={6}",
                   title, title+","+keyWord, keyWord, desc, typeId, access_key, context);
                item = new HttpItemJi
                {
                    URL = host + "/index.php?m=site&c=content&a=" + webCategoryCode,
                    Method = "post",
                    Postdata = postDataContent,                 
                    ContentType = "application/x-www-form-urlencoded",
                    KeepAlive = true,
                    Allowautoredirect = true,
                    AutoRedirectCookie = true,
                    Cookie = cookie
                };
                item.Header.Add("Upgrade-Insecure-Requests", "1");
                var html = http.GetHtml(item).Html;
                Thread.Sleep(2000);
                return html;
            });
        }

    }
}
