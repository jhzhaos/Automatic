using AutoMaticReleaseDAL;
using PostTool;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticReleaseBLL
{
   public class UploadSaveData
    {
        public static void SaveData(string txtPath, string userId, string category, string fileType, string date,string webGroupId)
        {
            var tool = new Tool();
            var txtList = new List<FileInfo>();
            tool.GetAllFiles(txtPath, txtList);
            var contentLine = new List<string>();
            var titleList = new List<string>();
            var keyWordList = new List<string>();
            var contentList = new List<string>();
            foreach (var t in txtList)
            {
                if (File.Exists(t.FullName))
                {
                    string[] strs2 = File.ReadAllLines(t.FullName, Encoding.Default);
                    if (strs2.Count() > 3)
                    {
                        titleList.Add(strs2[0].Trim(';').Trim(',').Trim('；').Trim('，'));
                        keyWordList.Add(strs2[1]);
                        var content = string.Empty;
                        for (int i = 0; i < strs2.Count(); i++)
                        {
                            if (i > 2)
                            {
                                content += strs2[i] + "<br/>";
                            }
                        }
                        contentList.Add(content);
                    }                    
                    File.Delete(t.FullName);
                }
            }
            var param = new SqlParameter[]
           {
                new SqlParameter("@userid",userId),
                new SqlParameter("@date",date)
           };
            SqlHelper.ExecuteNonQuery("delete from article where userid=@userid and date=@date", param);
            DataTable dt = SqlHelper.ExecuteDataTable("select * from article where 1=2");
            dt.TableName = "article";
            for (int i = 0; i < titleList.Count(); i++)
            {
                var titles = titleList[i].Replace("；", ";").Replace(",", ";").Replace("，", ";").Trim(';').Split(';').ToList();
                foreach (var t in titles)
                {
                    var row = dt.NewRow();
                    row["title"] = t;
                    row["seoWord"] = keyWordList[i];
                    row["content"] = contentList[i];
                    row["contentCategoryName"] = category;
                    row["userid"] = userId;
                    row["date"] = date;
                    row["webGroupid"] = webGroupId;
                    dt.Rows.Add(row);
                }
            }
            SqlHelper.BulkInsert(dt);
        }

        public static int GetConents(string userId)
        {
            var sql = "select * from article where userid=@userid";
            var param = new SqlParameter[]
            {
                new SqlParameter("@userName",userId)    
            };
            return SqlHelper.ExecuteNonQuery(sql, param);
        }
    }
}
