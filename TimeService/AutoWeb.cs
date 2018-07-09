using _6System;
using AutomaticReleaseBLL;
using EmpireCMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace TimeService
{
    public class AutoWeb
    {
        public static string AutoWebSit(int contextNum, string userId, DataTable webData, int iHour, string webGroupId, int contextCount)
        {
            if (webData != null && webData.Rows.Count > 0)
            {
                var sql = string.Empty;
                var contextDataTable = WebsiteBLL.GetAllArticleByIsRead(contextNum, userId, webGroupId);
                if (contextDataTable != null && contextDataTable.Rows.Count > 0 && webData != null && webData.Rows.Count > 0)
                {
                    foreach (DataRow context in contextDataTable.Rows)
                    {
                        int i = 0;
                        foreach (DataRow h in webData.Rows)
                        {
                            if (i >= contextCount || i > webData.Rows.Count)
                            {
                                sql += string.Format(@"insert into articleBak([title],[seoWord],[content],[IsImage],[websitCategory],[contentCategoryName]
           ,[contentCategoryId],[userid],[date],[isRead],[webGroupid])   select [title],[seoWord],[content],[IsImage],[websitCategory],[contentCategoryName]
           ,[contentCategoryId],[userid],GETDATE() ,[isRead],[webGroupid] from article  where article.id = {0} ;delete from article where id ={0} ;", context["id"].ToString());
                                break;
                            }
                            i++;

                            Task.Factory.StartNew(() =>
                            {
                                var webKeyWords = WebsiteBLL.GetWebKey(h["id"].ToString());
                                if (h["webPlatformId"].ToString() == "1")
                                {
                                    var cookieTask = GetCookie(h["loginName"].ToString(), h["loginPassword"].ToString(), h["webHost"].ToString(), h["webAdminUrl"].ToString());
                                    cookieTask.Wait();
                                    if (cookieTask.Result != null && cookieTask.Result != "")
                                    {
                                        var webWords = string.Empty;
                                        var content = WebsiteBLL.GetWebContext(webKeyWords, context["content"].ToString(), h["webImgPath"].ToString(), out webWords);
                                        var task = AutoRelease6System(cookieTask.Result, h["webCategoryCode"].ToString(), h["sixHost"].ToString(), h["categoryCode"].ToString(), context["title"].ToString(), webWords, content, null);
                                        if (task.Result.IndexOf("发布成功") < 0)
                                        {
                                            Thread.Sleep(1000);
                                            task = AutoRelease6System(cookieTask.Result, h["webCategoryCode"].ToString(), h["sixHost"].ToString(), h["categoryCode"].ToString(), context["title"].ToString(), webWords, content, null);
                                        }
                                    }


                                }
                                else
                                {
                                    var cookieTask = GetECMSCookie(h["loginName"].ToString(), h["loginPassword"].ToString(), h["webHost"].ToString(), h["webAdminUrl"].ToString());
                                    cookieTask.Wait();
                                    if (cookieTask.Result != null && cookieTask.Result.Count > 0)
                                    {

                                        var cookie = cookieTask.Result;
                                        var webWords = string.Empty;
                                        var content = WebsiteBLL.GetWebContext(webKeyWords, context["content"].ToString(), h["webImgPath"].ToString(), out webWords);
                                        var task = AutoReleaseECMS(cookie[0], cookie[1], cookie[2], cookie[3], h["webCategoryCode"].ToString(), h["webHost"].ToString(), h["categoryCode"].ToString(), context["title"].ToString(), webWords, content, h["webAdminUrl"].ToString(), null);
                                        if (task.Result.IndexOf("增加信息成功") < 0)
                                        {
                                            Thread.Sleep(1000);
                                            task = AutoReleaseECMS(cookie[0], cookie[1], cookie[2], cookie[3], h["webCategoryCode"].ToString(), h["webHost"].ToString(), h["categoryCode"].ToString(), context["title"].ToString(), webWords, content, h["webAdminUrl"].ToString(), null);
                                        }

                                    }

                                }
                            });
                        }
                    }                  
                    WebsiteBLL.ArticleBak(sql);                   
                }
            }
            return string.Empty;
        }

        public static void UpateAccount(int iHour)
        {
          var  sql = string.Format(@"
update [user] set isSend1=1 where {0} between  startHour1 and endHour1;
update [user] set isSend2=1 where {0} between  startHour2 and endHour2;
update [user] set isSend3=1 where {0} between  startHour3 and endHour3;", iHour);
            WebsiteBLL.ArticleBak(sql);
            AccountBLL.GetAllAccount(true);
        }
        public static async Task<string> GetCookie(string username, string password, string host, string webAdminUrl)
        {
            return await SystemSix.GetCookie(username, password, host, webAdminUrl);
        }


        public static async Task<List<string>> GetECMSCookie(string username, string password, string host, string adminHost)
        {
            return await ECMS.GetCookie(username, password, host, adminHost);
        }

        public static async Task<DataTable> GetWebSiteAwaitTask(string webGroupId)
        {
            return await GetWebSiteAwait(webGroupId);
        }
        public static Task<DataTable> GetWebSiteAwait(string webGroupId)
        {
            return Task<DataTable>.Run(() =>
            {
                return WebsiteBLL.GetWebSiteAwait(webGroupId);
            });
        }

        public async static Task<string> AutoRelease6System(string cookie, string webCategoryCode, string host, string categoryCode, string title, string keyWord, string context, string imgUrl)
        {
            return await SystemSix.post6Auto(cookie, webCategoryCode, host, categoryCode, title, keyWord, context, imgUrl);
        }

        public async static Task<string> AutoReleaseECMS(string cookies, string cookiesNewThree, string code, string code2, string webCategoryCode, string host, string categoryCode, string title, string keyWord, string context, string adminHost, string imgUrl)
        {
            return await ECMS.postECMS(cookies, cookiesNewThree, code, code2, webCategoryCode, host, categoryCode, title, keyWord, context, adminHost, imgUrl);
        }

        public static bool isSend(DataRow dr, ref int contentNum, int iHour)
        {
            Random rd = new Random();
            if (dr["startHour1"].ToString() != "" && dr["endHour1"].ToString() != "" && iHour >= int.Parse(dr["startHour1"].ToString()) && iHour < int.Parse(dr["endHour1"].ToString()) && dr["isSend1"].ToString() == "0")
            {
                contentNum = rd.Next(int.Parse(dr["count11"].ToString()), int.Parse(dr["count12"].ToString()));
                return true;
            }
            if (dr["startHour2"].ToString() != "" && dr["endHour2"].ToString() != "" && iHour >= int.Parse(dr["startHour2"].ToString()) && iHour < int.Parse(dr["endHour2"].ToString()) && dr["isSend2"].ToString() == "0")
            {
                contentNum = rd.Next(int.Parse(dr["count21"].ToString()), int.Parse(dr["count22"].ToString()));
                return true;
            }
            if (dr["startHour3"].ToString() != "" && dr["endHour3"].ToString() != "" && iHour >= int.Parse(dr["startHour3"].ToString()) && iHour < int.Parse(dr["endHour3"].ToString()) && dr["isSend3"].ToString() == "0")
            {
                contentNum = rd.Next(int.Parse(dr["count31"].ToString()), int.Parse(dr["count32"].ToString()));
                return true;
            }
            return false;
        }

    }
}
