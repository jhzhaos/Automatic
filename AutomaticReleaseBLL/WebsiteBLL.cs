using AutoMaticReleaseDAL;
using EmpireCMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticReleaseBLL
{
    public class WebsiteBLL
    {
        public static DataTable GetWebsite(string userId,string webGroupId)
        {
            var sql = string.Empty;
            if (string.IsNullOrEmpty(webGroupId))
            {
                sql = @"select a.* ,b.webPlatformName,c.webCategoryCode  from [webSit] a
left join webPlatform b on a.webPlatformId = b.id
left join webCategory c on c.id=a.webCategoryId
where a.webGroupId =(select top 1 id from [webGroup] where userid=@userId and groupParentCode!=0)";
                var param = new SqlParameter[]
            {
                new SqlParameter("@userId",userId)
            };
                return SqlHelper.ExecuteDataTable(sql, param);
            }
            else
            {
                sql = @"select a.* ,b.webPlatformName,c.webCategoryCode  from [webSit] a
left join webPlatform b on a.webPlatformId = b.id
left join webCategory c on c.id=a.webCategoryId
where a.webGroupId = @webGroupId";
                var param = new SqlParameter[]
            {
                new SqlParameter("@webGroupId",webGroupId)
            };
                return SqlHelper.ExecuteDataTable(sql, param);
            }

        }

        public static DataTable GetWebSiteAwait(string webGroupId)
        {
            var sql = string.Empty;
            sql = @"select a.* ,b.webPlatformName,c.webCategoryCode  from [webSit] a
left join webPlatform b on a.webPlatformId = b.id
left join webCategory c on c.id=a.webCategoryId
left join webGroup d on a.webGroupId=d.id
where d.groupParentCode = @webGroupId and a.webGroupId=d.Id";
            var param = new SqlParameter[]
        {
                new SqlParameter("@webGroupId",webGroupId)
        };
            return SqlHelper.ExecuteDataTable(sql, param);
        }

        public static DataTable GetWebGroup(string userid)
        {
            var sql = @"SELECT *
  FROM [webGroup] where userid= @userId";
            var param = new SqlParameter[]
            {
                new SqlParameter("@userId",userid)
            };
            return SqlHelper.ExecuteDataTable(sql, param);
        }

        public static DataTable GetWebsiteByid(string id)
        {
            var sql = @"select * from [webSit] ws left join  [webKeyWord] wk on ws.id=wk.webId  where ws.id=@id";
            var param = new SqlParameter[]
            {
                new SqlParameter("@id",id)
            };
            return SqlHelper.ExecuteDataTable(sql, param);         
        }

        public static string GetWebsiteByHost(string host)
        {
            var sql = "select count(1) from [dbo].[webSit] where webHost=@webHost";
            var param = new SqlParameter[]
            {
                new SqlParameter("@webHost",host)
            };
            return SqlHelper.ExecuteScalar(sql, param).ToString();
        }

        public static string GetWebsiteGroupByName(string groupName)
        {
            var sql = "select count(1) from [dbo].[webGroup] where groupName=@groupName";
            var param = new SqlParameter[]
            {
                new SqlParameter("@groupName",groupName)
            };
            return SqlHelper.ExecuteScalar(sql, param).ToString();
        }

        public static DataTable GetwebPlatform()
        {
            var sql = "select *from [dbo].[webPlatform] ";
            return SqlHelper.ExecuteDataTable(sql);
        }

        public static DataTable GetwebCategory()
        {
            var sql = "select *from [dbo].[webCategory] ";
            return SqlHelper.ExecuteDataTable(sql);
        }

        public static int AddWebsite(string webName, string loginName, string loginPassword, string webAdminUrl, string webHost, string webPlatformId, string categoryCode, string categoryName, string webCategoryId,string webGroupId,string keyNames,string keyUrls,string imgPath)
        {
            var sql = @"INSERT INTO [dbo].[webSit]([webName],[loginName]
           ,[loginPassword]
           ,[webAdminUrl]
           ,[webHost]
           ,[webPlatformId]          
,[categoryCode]
,[categoryName]
,[webCategoryId],[webGroupId],[webImgPath]) VALUES
           (@webName, @loginName, @loginPassword, @webAdminUrl, @webHost, @webPlatformId,@categoryCode,@categoryName,@webCategoryId,@webGroupId,@imgPath);
DECLARE @webId int;
select @webId=@@identity;";
            var keyNameList = keyNames.Split(',');
            var keyUrlList=keyUrls.Split(',');
            for (int i=0;i<keyNameList.Count();i++)
            {
                sql +=string.Format(@"INSERT INTO [dbo].[webKeyWord] ([keyName],[webId],[webGroupId] ,[keyType],[keyUrl])  VALUES  ('{0}' , @webId
           ,{1},1 ,'{2}');", keyNameList[i], webGroupId, keyUrlList[i]);
                }
            var param = new SqlParameter[]
            {
                new SqlParameter("@webName",webName),
                 new SqlParameter("@loginName",loginName),
                  new SqlParameter("@loginPassword",loginPassword),
                   new SqlParameter("@webAdminUrl",webAdminUrl),
                    new SqlParameter("@webHost",webHost),
                      new SqlParameter("@webPlatformId",webPlatformId),                 
                     new SqlParameter("@categoryCode",categoryCode),
                      new SqlParameter("@categoryName",categoryName),
                       new SqlParameter("@webCategoryId",webCategoryId),
                        new SqlParameter("@webGroupId",webGroupId),
                        new SqlParameter("@imgPath",imgPath)
            };
            return SqlHelper.ExecuteNonQuery(sql, param);
        }

        public static int AddWebsiteGroup(string webName, string groupParentCode, string userId,string groupCount)
        {
            var sql = @"INSERT INTO [dbo].[webGroup]
           ([groupName]
           ,[groupParentCode]
           ,[userId],[groupCount])
     VALUES (@groupName,@groupParentCode,@userId,@groupCount)";
            var param = new SqlParameter[]
            {
                new SqlParameter("@groupName",webName),
                 new SqlParameter("@groupParentCode",groupParentCode),
                  new SqlParameter("@userId",userId),
                  new SqlParameter("@groupCount",groupCount)
            };
            return SqlHelper.ExecuteNonQuery(sql, param);
        }

        public static int UpDateWebsiteGroup(string webName, string id,string groupCount)
        {

            var sql = @"UPDATE [dbo].[webGroup]
   SET [groupCount] = @groupCount
 WHERE groupParentCode=@id;";
            sql += @"UPDATE [dbo].[webGroup]
   SET [groupName] = @groupName ,[groupCount] = @groupCount
 WHERE id=@id;";
            var param = new SqlParameter[]
            {
                new SqlParameter("@groupName",webName),
                  new SqlParameter("@id",id),
                  new SqlParameter("@groupCount",groupCount)
            };
            return SqlHelper.ExecuteNonQuery(sql, param);
        }

        public static int UpdateWebsite(string webName, string id, string loginName, string loginPassword, string webAdminUrl, string webHost, string webPlatformId, string categoryCode, string categoryName, string webCategoryId,string webGroupId, string keyNames, string keyUrls,string imgPath)
        {
            var sql = @"delete from [webKeyWord] where webId=@id;UPDATE [dbo].[webSit]
   SET [webName] = @webName
      ,[loginName] = @loginName
      ,[loginPassword] =@loginPassword
      ,[webAdminUrl] = @webAdminUrl
      ,[webHost] = @webHost
      ,[webPlatformId] = @webPlatformId,[categoryCode]=@categoryCode,[categoryName]=@categoryName,[webCategoryId]=@webCategoryId,[webGroupId]=@webGroupId,[webImgPath]=@imgPath 
 WHERE id=@id;";
            var keyNameList = keyNames.Split(',');
            var keyUrlList = keyUrls.Split(',');
            for (int i = 0; i < keyNameList.Count(); i++)
            {
                sql += string.Format(@"INSERT INTO [dbo].[webKeyWord] ([keyName],[webId],[webGroupId] ,[keyType],[keyUrl])  VALUES  ('{0}' , @id
           ,{1},1 ,'{2}');", keyNameList[i], webGroupId, keyUrlList[i]);
            }
            var param = new SqlParameter[]
            {
                new SqlParameter("@webName",webName),
                 new SqlParameter("@loginName",loginName),
                  new SqlParameter("@loginPassword",loginPassword),
                   new SqlParameter("@webAdminUrl",webAdminUrl),
                    new SqlParameter("@webHost",webHost),
                      new SqlParameter("@webPlatformId",webPlatformId),
                    new SqlParameter("@id",id),
                     new SqlParameter("@categoryCode",categoryCode),
                      new SqlParameter("@categoryName",categoryName),
                       new SqlParameter("@webCategoryId",webCategoryId),
                       new SqlParameter("@webGroupId",webGroupId),
                        new SqlParameter("@imgPath",imgPath)
            };
            return SqlHelper.ExecuteNonQuery(sql, param);
        }

        public static int DeleteWebSite(string id)
        {
            var sql = "delete from [dbo].[webSit] where id=@id;delete from [dbo].[webKeyWord] where webId=@id;";
            var param = new SqlParameter[]
            {
                new SqlParameter("@id",id)
            };
            return SqlHelper.ExecuteNonQuery(sql, param);
        }

        public static bool loginTest(DataRow dr)
        {
            if (dr["webPlatformId"].ToString() == "1")
            {
                var sixHost = string.Empty;
                var result= _6System.SystemSix.LoginTest(dr["loginName"].ToString(), dr["loginPassword"].ToString(), dr["webHost"].ToString(), dr["webAdminUrl"].ToString(),out sixHost);
                var sql = "update [dbo].[webSit] set [sixHost]=@sixHost where id=@id";
                var param = new SqlParameter[]
                {
                new SqlParameter("@id",dr["id"].ToString()),
                new SqlParameter("@sixHost",sixHost)
                };
                SqlHelper.ExecuteNonQuery(sql, param);
                return result;
            }
            else
            {
                return ECMS.LoginTest(dr["loginName"].ToString(), dr["loginPassword"].ToString(), dr["webHost"].ToString(),dr["webAdminUrl"].ToString());
            }            
        }


        public static DataTable GetAllArticleByIsRead(int topNum, string userId,string webGroupId)
        {
            var sql = string.Format(@"select top {0} * from [article] where isRead<>1 and userid=@userId and webGroupid=@webGroupid;update article set isRead=1 where id in (select top {0} id from [article] where isRead<>1 and userid=@userId and webGroupid=@webGroupid)
                ", topNum);
            var param = new SqlParameter[]
            {
                new SqlParameter("@userId",userId),
                 new SqlParameter("@webGroupid",webGroupId)
            };
            return SqlHelper.ExecuteDataTable(sql, param);
        }

        public static DataTable GetAllArticleByUserId(string userId,string webGroupId)
        {
            if (!string.IsNullOrEmpty(webGroupId))
            {
                webGroupId = " and webGroupid=" + webGroupId;
            }
            var sql = string.Format(@"select * from article where isRead<>1 and userid=@userId{0} order by id desc", webGroupId);            
            var param = new SqlParameter[]
            {
                new SqlParameter("@userId",userId)
            };
            return SqlHelper.ExecuteDataTable(sql, param);
        }

        public static int DeleteWebSiteGroup(string id)
        {
            var sql = @"delete from [webSit] where [webGroupId]=(select id from webGroup where groupParentCode=@id);
            delete from webGroup where groupParentCode = @id;
            delete from [webSit] where [webGroupId] = @id;
            delete from [webGroup] where id = @id;
            delete from [webKeyWord] where webGroupId=@id;";
            var param = new SqlParameter[]
            {
                new SqlParameter("@id",id)
            };
            return SqlHelper.ExecuteNonQuery(sql, param);
        }

        public static int DeleteWebKey(string id)
        {
            var sql = "delete from [dbo].[webKeyWord] where id=@id";
            var param = new SqlParameter[]
            {
                new SqlParameter("@id",id)
            };
            return SqlHelper.ExecuteNonQuery(sql, param);
        }

        public static void ArticleBak(string sql)
        {
            SqlHelper.ExecuteNonQuery(sql);

        }

        public static DataTable GetWebKey(string webId)
        {
            var sql = string.Format(@"select * from [dbo].[webKeyWord] where webId=@webId");
            var param = new SqlParameter[]
            {
                new SqlParameter("@webId",webId)               
            };
            return SqlHelper.ExecuteDataTable(sql, param);
        }

        public static string GetWebContext( DataTable dt,string content,string webImgPath,out string webKeyWords)
        {
            Random rnd = new Random();
            webKeyWords = string.Empty;
            if (dt != null && dt.Rows.Count > 0)
            {
                var keyCount = rnd.Next(0, 5);
                for (int i = 0; i <= keyCount; i++)
                {
                    var keyIndex = rnd.Next(0, dt.Rows.Count - 1);
                    content = content.Replace(dt.Rows[keyIndex]["keyName"].ToString(), "<a href='" + dt.Rows[keyIndex]["keyUrl"] + "'>" + dt.Rows[keyIndex]["keyName"] + "</a>");
                }
                content = content.Replace("[IMG]", "<img alt='" + dt.Rows[rnd.Next(0, dt.Rows.Count - 1)]["keyName"].ToString() + "' src='.." + webImgPath + rnd.Next(0, 100) + ".jpg'/>");
                for (int i = 0; i <= keyCount; i++)
                {
                    var keyIndex = rnd.Next(0, dt.Rows.Count - 1);
                    webKeyWords += dt.Rows[keyIndex]["keyName"].ToString() + ",";
                }
            }           
            return content;
        }
    }
}
