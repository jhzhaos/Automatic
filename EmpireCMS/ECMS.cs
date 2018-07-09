using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsharpHttpHelper;
using PostTool;
using CsharpHttpHelper.Enum;
using System.Text.RegularExpressions;
using System.Threading;

namespace EmpireCMS
{
    public class ECMS
    {
        public static void EmpireCMS()
        {
            string strIMSPhone = System.Web.HttpUtility.UrlEncode("尹华伟");
            string strIMSPwd = "yinhuawei";
            string postData = "enews=login&username=" + strIMSPhone + "&password=yinhuawei&equestion=0&eanswer=&adminwindow=0&imageField.x=36&imageField.y=13";
            HttpHelper http = new HttpHelper();
            HttpItem item = new HttpItem
            {
                URL = "http://www.honghezdh.com/e/admin/ecmsadmin.php",
                Method = "post",
                Postdata = postData,
                Host = "www.honghezdh.com",
                Referer = "http://www.honghezdh.com/e/admin",
                UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:49.0) Gecko/20100101 Firefox/49.0",
                ContentType = "application/x-www-form-urlencoded",
                KeepAlive = true,
                Allowautoredirect = true,
                AutoRedirectCookie = true
            };
            item.Header.Add("Upgrade-Insecure-Requests", "1");
            HttpResult result = http.GetHtml(item);
            string html = result.Html;
            string code = HttpHelper.GetBetweenHtml(html, "LoginSuccess&", "\">");
           // string codeUse = code.Substring(code.IndexOf("=") + 1, code.Length - code.IndexOf("=") - 1);
            string loginUrl = HttpHelper.GetBetweenHtml(html, "url=", "\">");
            string cookies = result.Cookie;
            cookies = cookies.Replace("path=/,guqyscheckkey=deleted; expires=Thu, 01-Jan-1970 00:00:01 GMT; Max-Age=0;", "");
            cookies = cookies.Replace("path=/,guqysecertkeyrnds=deleted; expires=Thu, 01-Jan-1970 00:00:01 GMT; Max-Age=0;", "");
            cookies = cookies.Replace("path=/", "");
            cookies = cookies.Replace(",", "");
            cookies = cookies.Replace(" ", "");
            http = new HttpHelper();
            item = new HttpItem
            {
                URL = "http://www.honghezdh.com/e/admin/" + loginUrl,
                Method = "get",
                Host = "www.honghezdh.com",
                Cookie = cookies,
                Referer = "http://www.honghezdh.com/e/admin/ecmsadmin.php",
                UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:49.0) Gecko/20100101 Firefox/49.0",
                KeepAlive = true,
                Allowautoredirect = true,
                AutoRedirectCookie = true
            };
            item.Header.Add("Upgrade-Insecure-Requests", "1");
            result = http.GetHtml(item);
            string cookiesNew = result.Cookie;
            cookiesNew = cookiesNew.Replace("path=/", "");
            cookies = Tool.updateCookie(cookies, cookiesNew);
            http = new HttpHelper();
            item = new HttpItem
            {
                URL = "http://www.honghezdh.com/e/admin/admin.php?" + code,
                Method = "get",
                Host = "www.honghezdh.com",
                Cookie = cookies,
                Referer = "http://www.honghezdh.com/e/admin/" + loginUrl,
                UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:49.0) Gecko/20100101 Firefox/49.0",
                KeepAlive = true
            };
            item.Header.Add("Upgrade-Insecure-Requests", "1");
            result = http.GetHtml(item);
            string htmlNew = result.Html;
            string code2 = HttpHelper.GetBetweenHtml(htmlNew, "rhash", "'");
            string cookiesNewOne = result.Cookie;
            cookiesNewOne = cookiesNew.Replace("path=/", "");
            cookies = Tool.updateCookie(cookies, cookiesNewOne);
            http = new HttpHelper();
            item = new HttpItem
            {
                URL = "http://www.honghezdh.com/e/admin/DoTimeRepage.php?" + code + "&rhash" + code2,
                Method = "get",
                Host = "www.honghezdh.com",
                Cookie = cookies,
                Referer = "http://www.honghezdh.com/e/admin/admin.php?" + code,
                UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:49.0) Gecko/20100101 Firefox/49.0",
                KeepAlive = true
            };
            item.Header.Add("Upgrade-Insecure-Requests", "1");
            result = http.GetHtml(item);
            string cookiesNewTwo = result.Cookie;
            cookiesNewTwo = cookiesNew.Replace("path=/", "");
            cookies = Tool.updateCookie(cookies, cookiesNewTwo);
            http = new HttpHelper();
            item = new HttpItem
            {
                URL = "http://www.honghezdh.com/e/admin/AddInfoChClass.php?" + code,
                Method = "get",
                Host = "www.honghezdh.com",
                Cookie = cookies,
                Referer = "http://www.honghezdh.com/e/admin/admin.php?" + code,
                UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:49.0) Gecko/20100101 Firefox/49.0",
                KeepAlive = true
            };
            item.Header.Add("Upgrade-Insecure-Requests", "1");
            result = http.GetHtml(item);
            string cookiesNewThree = result.Cookie;
            cookiesNewThree = cookiesNew.Replace("path=/", "");
            cookies = Tool.updateCookie(cookies, cookiesNewThree);
            http = new HttpHelper();
            item = new HttpItem
            {
                URL = "http://www.honghezdh.com/e/admin/AddNews.php?" + code + "&enews=AddNews&classid=39",
                Method = "get",
                Host = "www.honghezdh.com",
                Cookie = cookies,
                Referer = "http://www.honghezdh.com/e/admin/AddInfoChClass.php",
                UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:49.0) Gecko/20100101 Firefox/49.0",
                KeepAlive = true
            };
            item.Header.Add("Upgrade-Insecure-Requests", "1");
            result = http.GetHtml(item);
            string cookiesNewFour = result.Cookie;
            cookiesNewFour = cookiesNewThree.Replace("path=/", "");
            cookies = Tool.updateCookie(cookies, cookiesNewFour);
            //-----------------文章发布----------------------------------------------------------------------------------------------------------------
            string boundary = string.Format("----{0}", DateTime.Now.Ticks.ToString("x"));    // 分界线可以自定义参数
            var rhash = HttpHelper.GetBetweenHtml(result.Html, "ecmsinfo.php?", "&enews=AddInfoToReHtml").Replace("?&" + code + "&", "");
            string format = "--" + boundary + "\r\nContent-Disposition:form-data;name=\"{0}\"\r\n\r\n{1}\r\n";    //自带项目分隔符
            var ehash = code.Split('=');
            var rhashs = rhash.Split('=');
            var filePass = Tool.GetInputVal(result.Html, @"<input type=hidden value=""(?<value>[^""]+)"" name=filepass>");
            StringBuilder paramImg = new StringBuilder();
            paramImg.Append("--" + boundary + "\r\n");
            paramImg.Append(string.Format(@"Content-Disposition: form-data; name=""file""; filename=""test.jpg""" + "\r\n"));
            paramImg.Append(string.Format(@"Content-Type: image/jpeg" + "\r\n\r\n"));
            byte[] HeadBytes = Encoding.ASCII.GetBytes(paramImg.ToString());
            #region 图片数据
            byte[] PicBytes = Tool.ImageToBytesFromFilePath(string.Format(@"E:\\test.jpg"));
            #endregion         
            paramImg.Append(string.Format(format, ehash[0], ehash[1]));
            paramImg.Append(string.Format(format, rhashs[0], rhashs[1]));
            paramImg.Append(string.Format(format, "enews", "TranFile"));
            paramImg.Append(string.Format(format, "classid", "15"));
            paramImg.Append(string.Format(format, "infoid", "0"));
            paramImg.Append(string.Format(format, "filepass", filePass));
            paramImg.Append(string.Format(format, "ecmsfrom", "http://www.honghezdh.com/e/admin/ecmsinfo.php"));
            paramImg.Append(string.Format(format, "type", "1"));
            paramImg.Append(string.Format(format, "doing", "1"));
            paramImg.Append(string.Format(format, "no", "test.jpg"));
            paramImg.Append(string.Format(format, "width", "105"));
            paramImg.Append(string.Format(format, "height", "118"));
            paramImg.Append(string.Format(format, "tranurl", "http://"));
            paramImg.Append(string.Format(format, "modtype", "0"));
            paramImg.Append(string.Format(format, "sinfo", "1"));
            paramImg.Append(string.Format(format, "fstb", "1"));
            paramImg.Append(string.Format(format, "Submit3", "上传"));
            paramImg.Append("\r\n" + boundary + "--\r\n");
            byte[] TailBytesi = Encoding.ASCII.GetBytes(paramImg.ToString());
            var UploadBuffers = Tool.ComposeArrays(HeadBytes, PicBytes);
            UploadBuffers = Tool.ComposeArrays(UploadBuffers, TailBytesi);
            http = new HttpHelper();
            item = new HttpItem
            {
                URL = "http://www.honghezdh.com/e/admin/ecmseditor/ecmseditor.php",
                Method = "post",
                Host = "www.honghezdh.com",
                Cookie = cookies,
                Referer = "http://www.honghezdh.com/e/admin/ecmseditor/file.php?&classid=15&infoid=0&filepass=" + filePass + "&type=1&modtype=0&sinfo=1&doing=1&tranfrom=&field=titlepic&" + code,
                UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:49.0) Gecko/20100101 Firefox/49.0",
                ContentType = "multipart/form-data; boundary=" + boundary,
                PostDataType = PostDataType.Byte,
                PostEncoding = Encoding.UTF8,
                PostdataByte = UploadBuffers,
            };
            item.Header.Add("Upgrade-Insecure-Requests", "1");
            result = http.GetHtml(item);
            item = new HttpItem
            {
                URL = "http://www.honghezdh.com/e/admin/ecmseditor/file.php?&classid=15&infoid=0&filepass=" + filePass + "&type=1&modtype=0&sinfo=1&doing=1&tranfrom=&field=titlepic&" + code,
                Method = "get",
                Host = "www.honghezdh.com",
                Cookie = cookies,
                UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:49.0) Gecko/20100101 Firefox/49.0",
                ContentType = "multipart/form-data; boundary=" + boundary,
                PostDataType = PostDataType.Byte,
                PostEncoding = Encoding.UTF8,
                PostdataByte = UploadBuffers,
            };
            html = http.GetHtml(item).Html;
            var imgPath = HttpHelper.GetBetweenHtml(html, "index.html", "target='_blank'").Replace("?url=", "").Trim();
            //--------------------图片上传结束---------------------------------------------------------------------------------------------------------
            StringBuilder param = new StringBuilder();
            param.Append("--" + boundary + "\r\n");
            param.Append(string.Format(format, ehash[0], ehash[1]));
            param.Append(string.Format(format, rhashs[0], rhashs[1]));
            param.Append(string.Format(format, "enews", "AddNews"));
            param.Append(string.Format(format, "classid", "15"));
            param.Append(string.Format(format, "bclassid", "10"));
            param.Append(string.Format(format, "id", "0"));
            param.Append(string.Format(format, "filepass", filePass));
            param.Append(string.Format(format, "ecmsfrom", "http://www.honghezdh.com/e/admin/ecmsinfo.php"));
            param.Append(string.Format(format, "ecmsnfrom", "1"));
            param.Append(string.Format(format, "title", "2018ttttt啊啊啊啊啊啊啊ttttttt"));
            param.Append(string.Format(format, "ftitle", "seo标题三生三世"));
            param.Append(string.Format(format, "checked", "1"));
            param.Append(string.Format(format, "newstime", DateTime.Now.ToString()));
            param.Append(string.Format(format, "newstext", "文章详情2018"));
            param.Append(string.Format(format, "dokey", "1"));
            param.Append(string.Format(format, "titlepic", imgPath));
            param.Append(string.Format(format, "addnews", "提 交 "));
            param.Append("\r\n--" + boundary + "--\r\n");
            byte[] TailBytes = Encoding.UTF8.GetBytes(param.ToString());
            http = new HttpHelper();
            item = new HttpItem
            {
                URL = "http://www.honghezdh.com/e/admin/ecmsinfo.php",
                Method = "post",
                Host = "www.honghezdh.com",
                Cookie = cookies,
                //  Referer = "http://www.honghezdh.com/e/admin/AddNews.php?enews=AddNews&ecmsnfrom=1&bclassid=10&classid=15&" + code,
                UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:49.0) Gecko/20100101 Firefox/49.0",
                ContentType = "multipart/form-data; boundary=" + boundary,
                PostDataType = PostDataType.Byte,
                PostEncoding = Encoding.UTF8,
                PostdataByte = TailBytes,
            };
            item.Header.Add("Upgrade-Insecure-Requests", "1");
            result = http.GetHtml(item);
            html = result.Html;
        }

        public static bool LoginTest(string username, string passWord, string host,string adminHost)
        {
            string postData = "enews=login&username=" + System.Web.HttpUtility.UrlEncode(username) + "&password=" + passWord + "&equestion=0&eanswer=&adminwindow=0&imageField.x=36&imageField.y=13";
            HttpHelper http = new HttpHelper();
            HttpItem item = new HttpItem
            {
                URL = host + adminHost+"ecmsadmin.php",
                Method = "post",
                Postdata = postData,
                // Host = "www.honghezdh.com",
                Referer = host + adminHost,
                UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:49.0) Gecko/20100101 Firefox/49.0",
                ContentType = "application/x-www-form-urlencoded",
                KeepAlive = true,
                Allowautoredirect = true,
                AutoRedirectCookie = true
            };
            item.Header.Add("Upgrade-Insecure-Requests", "1");
            HttpResult result = http.GetHtml(item);
            string html = result.Html;
            string loginUrl = HttpHelper.GetBetweenHtml(html, "url=", "\">");
            string cookies = result.Cookie;
            cookies = cookies.Replace("path=/,guqyscheckkey=deleted; expires=Thu, 01-Jan-1970 00:00:01 GMT; Max-Age=0;", "");
            cookies = cookies.Replace("path=/,guqysecertkeyrnds=deleted; expires=Thu, 01-Jan-1970 00:00:01 GMT; Max-Age=0;", "");
            cookies = cookies.Replace("path=/", "");
            cookies = cookies.Replace(",", "");
            cookies = cookies.Replace(" ", "");
            item = new HttpItem
            {
                URL = host + adminHost+loginUrl,
                Method = "get",
                Cookie = cookies,
                Referer = host + adminHost+"ecmsadmin.php",
                UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:49.0) Gecko/20100101 Firefox/49.0",
                KeepAlive = true,
                Allowautoredirect = true,
                AutoRedirectCookie = true
            };
            item.Header.Add("Upgrade-Insecure-Requests", "1");
            result = http.GetHtml(item);
            html = result.Html;
            if (html.IndexOf("登录成功") > -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Task<List<string>> GetCookie(string username, string passWord, string host,string adminHost)
        {
            return Task<string>.Run(() =>
            {
                string postData = "enews=login&username=" + System.Web.HttpUtility.UrlEncode(username) + "&password=" + passWord + "&equestion=0&eanswer=&adminwindow=0&imageField.x=36&imageField.y=13";
                HttpHelper http = new HttpHelper();
                HttpItem item = new HttpItem
                {
                    URL = host + adminHost + "ecmsadmin.php",
                    Method = "post",
                    Postdata = postData,
                    // Host = "www.honghezdh.com",
                    Referer = host + adminHost,
                    UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:49.0) Gecko/20100101 Firefox/49.0",
                    ContentType = "application/x-www-form-urlencoded",
                    KeepAlive = true,
                    Allowautoredirect = true,
                    AutoRedirectCookie = true
                };
                item.Header.Add("Upgrade-Insecure-Requests", "1");
                HttpResult result = http.GetHtml(item);             
                string html = result.Html;
                string code = HttpHelper.GetBetweenHtml(html, "LoginSuccess&", "\">");              
                string loginUrl = HttpHelper.GetBetweenHtml(html, "url=", "\">");
                string cookies = result.Cookie;
                cookies = cookies.Replace("path=/,guqyscheckkey=deleted; expires=Thu, 01-Jan-1970 00:00:01 GMT; Max-Age=0;", "");
                cookies = cookies.Replace("path=/,guqysecertkeyrnds=deleted; expires=Thu, 01-Jan-1970 00:00:01 GMT; Max-Age=0;", "");
                cookies = cookies.Replace("path=/", "");
                cookies = cookies.Replace(",", "");
                cookies = cookies.Replace(" ", "");                      
                item = new HttpItem
                {
                    URL = host + adminHost + loginUrl,
                    Method = "get",
                    Cookie = cookies,
                    Referer = host + adminHost+ "ecmsadmin.php",
                    UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:49.0) Gecko/20100101 Firefox/49.0",
                    KeepAlive = true,
                    Allowautoredirect = true,
                    AutoRedirectCookie = true
                };
                item.Header.Add("Upgrade-Insecure-Requests", "1");
                result = http.GetHtml(item);
                html = result.Html;
                if (html.IndexOf("登录成功") > -1)
                {
                    var cookiesNew = cookies.Replace("path=/", "");
                    cookies = Tool.updateCookie(cookies, cookiesNew);
                    http = new HttpHelper();
                    item = new HttpItem
                    {
                        URL = host + adminHost+"admin.php?" + code,
                        Method = "get",
                        Cookie = cookies,
                        UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:49.0) Gecko/20100101 Firefox/49.0",
                        KeepAlive = true
                    };
                    item.Header.Add("Upgrade-Insecure-Requests", "1");
                    result = http.GetHtml(item);
                    string htmlNew = result.Html;
                    string code2 = "rhash"+HttpHelper.GetBetweenHtml(htmlNew, "rhash", "'");
                    string cookiesNewOne = result.Cookie;                 
                    cookiesNewOne = cookiesNew.Replace("path=/", "");
                    cookies = Tool.updateCookie(cookies, cookiesNewOne);
                    http = new HttpHelper();
                    item = new HttpItem
                    {
                        URL = host + adminHost+ "DoTimeRepage.php?" + code + "&"+code2,
                        Method = "get",
                        Cookie = cookies,
                        UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:49.0) Gecko/20100101 Firefox/49.0",
                        KeepAlive = true
                    };
                    item.Header.Add("Upgrade-Insecure-Requests", "1");
                    result = http.GetHtml(item);
                    string cookiesNewTwo = result.Cookie;
                    cookiesNewTwo = cookiesNew.Replace("path=/", "");
                    cookies = Tool.updateCookie(cookies, cookiesNewTwo);
                    http = new HttpHelper();
                    item = new HttpItem
                    {
                        URL = host + adminHost+ "AddInfoChClass.php?" + code,
                        Method = "get",
                        Cookie = cookies,
                        Referer = host + adminHost+"admin.php?" + code,
                        UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:49.0) Gecko/20100101 Firefox/49.0",
                        KeepAlive = true
                    };
                    item.Header.Add("Upgrade-Insecure-Requests", "1");
                    result = http.GetHtml(item);
                    string cookiesNewThree = result.Cookie;
                    cookiesNewThree = cookiesNew.Replace("path=/", "");
                    cookies = Tool.updateCookie(cookies, cookiesNewThree);
                    List<string> cookiesList = new List<string>();
                    cookiesList.Add(cookies);
                    cookiesList.Add(cookiesNewThree);
                    cookiesList.Add(code);
                    cookiesList.Add(code2);                 
                    return cookiesList;
                }
                else
                {
                    return null;
                }
            });

        }

        public static Task<string> postECMS(string cookies,string cookiesNewThree, string code,string code2, string webCategoryCode, string host, string typeId, string title, string keyWord, string context, string adminHost,string imgUrl)
        {
            return Task<string>.Run(() =>
            {
                var ehash = code.Split('=');
                var rhashs =code2.Split('=');                
                var http = new HttpHelper();
               var item = new HttpItem
                {
                    URL = host + adminHost+"AddNews.php?" + code + "&enews=AddNews&classid=" + typeId,
                    Method = "get",
                    Cookie = cookies,
                    Referer = host + adminHost+"AddInfoChClass.php",
                    UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:49.0) Gecko/20100101 Firefox/49.0",
                    KeepAlive = true
                };
                item.Header.Add("Upgrade-Insecure-Requests", "1");
               var result = http.GetHtml(item);
                string cookiesNewFour = result.Cookie;
                cookiesNewFour = cookiesNewThree.Replace("path=/", "");
                cookies = Tool.updateCookie(cookies, cookiesNewFour);
                //-----------------文章发布----------------------------------------------------------------------------------------------------------------
                string boundary = string.Format("----{0}", DateTime.Now.Ticks.ToString("x"));    // 分界线可以自定义参数                  
                var filePass = Tool.GetInputVal(result.Html, @"<input type=hidden value=""(?<value>[^""]+)"" name=filepass>");
                string format = "--" + boundary + "\r\nContent-Disposition:form-data;name=\"{0}\"\r\n\r\n{1}\r\n";    //自带项目分隔符            
                var imgPath = string.Empty;
                if (!string.IsNullOrEmpty(imgUrl))
                {
                    StringBuilder paramImg = new StringBuilder();
                    paramImg.Append("--" + boundary + "\r\n");
                    paramImg.Append(string.Format(@"Content-Disposition: form-data; name=""file""; filename=""test.jpg""" + "\r\n"));
                    paramImg.Append(string.Format(@"Content-Type: image/jpeg" + "\r\n\r\n"));
                    byte[] HeadBytes = Encoding.ASCII.GetBytes(paramImg.ToString());
                    #region 图片数据
                    byte[] PicBytes = Tool.ImageToBytesFromFilePath(string.Format(@"E:\\test.jpg"));
                    #endregion
                    paramImg.Append(string.Format(format, ehash[0], ehash[1]));
                    paramImg.Append(string.Format(format, rhashs[0], rhashs[1]));
                    paramImg.Append(string.Format(format, "enews", "TranFile"));
                    paramImg.Append(string.Format(format, "classid", "15"));
                    paramImg.Append(string.Format(format, "infoid", "0"));
                    paramImg.Append(string.Format(format, "filepass", filePass));
                    paramImg.Append(string.Format(format, "ecmsfrom", host + adminHost+"ecmsinfo.php"));
                    paramImg.Append(string.Format(format, "type", "1"));
                    paramImg.Append(string.Format(format, "doing", "1"));
                    paramImg.Append(string.Format(format, "no", "test.jpg"));
                    paramImg.Append(string.Format(format, "width", "105"));
                    paramImg.Append(string.Format(format, "height", "118"));
                    paramImg.Append(string.Format(format, "tranurl", "http://"));
                    paramImg.Append(string.Format(format, "modtype", "0"));
                    paramImg.Append(string.Format(format, "sinfo", "1"));
                    paramImg.Append(string.Format(format, "fstb", "1"));
                    paramImg.Append(string.Format(format, "Submit3", "上传"));
                    paramImg.Append("\r\n" + boundary + "--\r\n");
                    byte[] TailBytesi = Encoding.ASCII.GetBytes(paramImg.ToString());
                    var UploadBuffers = Tool.ComposeArrays(HeadBytes, PicBytes);
                    UploadBuffers = Tool.ComposeArrays(UploadBuffers, TailBytesi);
                    http = new HttpHelper();
                    item = new HttpItem
                    {
                        URL = host + adminHost+"ecmseditor/ecmseditor.php",
                        Method = "post",
                        Cookie = cookies,                        
                        UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:49.0) Gecko/20100101 Firefox/49.0",
                        ContentType = "multipart/form-data; boundary=" + boundary,
                        PostDataType = PostDataType.Byte,
                        PostEncoding = Encoding.UTF8,
                        PostdataByte = UploadBuffers,
                    };
                    item.Header.Add("Upgrade-Insecure-Requests", "1");
                    result = http.GetHtml(item);
                    item = new HttpItem
                    {
                        URL = host + adminHost+"ecmseditor/file.php?&classid=15&infoid=0&filepass=" + filePass + "&type=1&modtype=0&sinfo=1&doing=1&tranfrom=&field=titlepic&" + code,
                        Method = "get",
                        Cookie = cookies,
                        UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:49.0) Gecko/20100101 Firefox/49.0",
                        ContentType = "multipart/form-data; boundary=" + boundary,
                        PostDataType = PostDataType.Byte,
                        PostEncoding = Encoding.UTF8,
                        PostdataByte = UploadBuffers,
                    };
                    var html = http.GetHtml(item).Html;
                    imgPath = HttpHelper.GetBetweenHtml(html, "index.html", "target='_blank'").Replace("?url=", "").Trim();
                    Thread.Sleep(2000);
                }
                //--------------------图片上传结束---------------------------------------------------------------------------------------------------------               
                StringBuilder param = new StringBuilder();
                param.Append("--" + boundary + "\r\n");
                param.Append(string.Format(format, ehash[0], ehash[1]));
                param.Append(string.Format(format, rhashs[0], rhashs[1]));
                param.Append(string.Format(format, "enews", "AddNews"));
                param.Append(string.Format(format, "classid", typeId));              
                param.Append(string.Format(format, "id", "0"));
                param.Append(string.Format(format, "filepass", filePass));
                param.Append(string.Format(format, "ecmsfrom", host + adminHost+"ecmsinfo.php"));
                param.Append(string.Format(format, "ecmsnfrom", "1"));
                param.Append(string.Format(format, "title", title));
                param.Append(string.Format(format, "ftitle", title+","+keyWord));
                param.Append(string.Format(format, "checked", "1"));
               // param.Append(string.Format(format, "newstime", DateTime.Now.ToString()));
                param.Append(string.Format(format, "newstext", context));
                param.Append(string.Format(format, "dokey", "1"));
                param.Append(string.Format(format, "titlepic", imgPath));
                param.Append(string.Format(format, "addnews", "提 交 "));
                param.Append("\r\n--" + boundary + "--\r\n");
                byte[] TailBytes = Encoding.UTF8.GetBytes(param.ToString());
                http = new HttpHelper();
                item = new HttpItem
                {
                    URL = host + adminHost+"ecmsinfo.php",
                    Method = "post",
                    Cookie = cookies,
                    UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:49.0) Gecko/20100101 Firefox/49.0",
                    ContentType = "multipart/form-data; boundary=" + boundary,
                    PostDataType = PostDataType.Byte,
                    PostEncoding = Encoding.UTF8,
                    PostdataByte = TailBytes,
                };
                item.Header.Add("Upgrade-Insecure-Requests", "1");
                result = http.GetHtml(item);
                Thread.Sleep(2500);
                return result.Html;
            });
        }
    }
}
