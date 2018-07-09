using AutoMaticReleaseDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticReleaseBLL
{
    public class AccountBLL
    {
        public static string GetAccountByUsername(string username)
        {
            var sql = "select count(1) from [user] where userName=@userName";
            var param = new SqlParameter[]
            {
                new SqlParameter("@userName",username),
            };
            return SqlHelper.ExecuteScalar(sql, param).ToString();
        }
        public static DataRow GetAccountByUsernamePwd(string username, string passWord)
        {
            var sql = "select * from [user] where userName=@userName and passWord=@passWord";
            var param = new SqlParameter[]
            {
                new SqlParameter("@userName",username),
                new SqlParameter("@passWord",passWord)
            };
            return SqlHelper.ExecuteDataRow(sql, param);
        }

        public static int Register(string userName, string passWord,string userCode)
        {
            var sql = "INSERT INTO [dbo].[user] ([userName] ,[passWord],[userCode]) VALUES (@userName , @passWord,@userCode)";
            var param = new SqlParameter[]
            {
                new SqlParameter("@userName",userName),
                new SqlParameter("@passWord",passWord),
                 new SqlParameter("@userCode",userCode)
            };
            return SqlHelper.ExecuteNonQuery(sql, param);
        }

        public static int UpdateUser(string startHour1, string endHour1, string count11,string count12, string startHour2, string endHour2, string count21, string count22, string startHour3, string endHour3, string count31, string count32, string id)
        {
            var sql = @"update [user] set startHour1=@startHour1,endHour1=@endHour1,count11=@count11,count12=@count12,
                startHour2=@startHour2,endHour2=@endHour2,count21=@count21,count22=@count22,
startHour3=@startHour3,endHour3=@endHour3,count31=@count31,count32=@count32,isSend1=0,isSend2=0,isSend3=0 
where id=@id";
            var param = new SqlParameter[]
            {
                new SqlParameter("@startHour1",startHour1),
                new SqlParameter("@endHour1",endHour1),
                 new SqlParameter("@count11",count11),
                  new SqlParameter("@count12",count12),
                    new SqlParameter("@startHour2",startHour2),
                new SqlParameter("@endHour2",endHour2),
                 new SqlParameter("@count21",count21),
                  new SqlParameter("@count22",count22),
                    new SqlParameter("@startHour3",startHour3),
                new SqlParameter("@endHour3",endHour3),
                 new SqlParameter("@count31",count31),
                  new SqlParameter("@count32",count32),
                   new SqlParameter("@id",id)
            };
            return SqlHelper.ExecuteNonQuery(sql, param);
        }

        public static DataRow GetAccount(string id)
        {
            var sql = "select * from [user] where id=@id";
            var param = new SqlParameter[]
            {
                new SqlParameter("@id",id)           
            };
            return SqlHelper.ExecuteDataRow(sql, param);
        }

        public static DataTable GetAllAccount(bool hasWeb=false)
        {          
            var dtCache = CacheHelper.GetCache("AllAccount") as DataTable;
            if (dtCache == null||hasWeb)
            {
                var sql = "select * from [user] u,[webGroup] w where u.id = w.userId and w.groupParentCode=0";
                dtCache = SqlHelper.ExecuteDataTable(sql);
                CacheHelper.SetCache("AllAccount", dtCache,1);
            }
            return dtCache;
        }

        public static void UpdateUseState()
        {
            var sql = "update [user] set isSend1=0,isSend2=0,isSend3=0";         
            SqlHelper.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 唯一订单号生成
        /// </summary>
        /// <returns></returns>
        public static string GenerateOrderNumber()
        {
            string strDateTimeNumber = DateTime.Now.ToString("yyyyMMddHHmmssms");
            string strRandomResult = NextRandom(1000, 1).ToString();
            return strDateTimeNumber + strRandomResult;
        }

        /// <summary>
        /// 参考：msdn上的RNGCryptoServiceProvider例子
        /// </summary>
        /// <param name="numSeeds"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private static int NextRandom(int numSeeds, int length)
        {
            // Create a byte array to hold the random value.  
            byte[] randomNumber = new byte[length];
            // Create a new instance of the RNGCryptoServiceProvider.  
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            // Fill the array with a random value.  
            rng.GetBytes(randomNumber);
            // Convert the byte to an uint value to make the modulus operation easier.  
            uint randomResult = 0x0;
            for (int i = 0; i < length; i++)
            {
                randomResult |= ((uint)randomNumber[i] << ((length - 1 - i) * 8));
            }
            return (int)(randomResult % numSeeds) + 1;
        }


    }
}
