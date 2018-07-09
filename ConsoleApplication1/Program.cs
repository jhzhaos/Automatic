using AutomaticReleaseBLL;
using EmpireCMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ConsoleApplication1
{
   public class Program
    {
       
        static void Main(string[] args)
        {
            //System.Timers
            //Console.WriteLine("-------主线程启动-------");
            //Task<string> task = GetStrLengthAsync();//1
            //Console.WriteLine("主线程继续执行");
            //Console.WriteLine("Task返回的值" + task.Result);//3
            //Console.WriteLine("-------主线程结束-------");
            //Console.ReadLine();
             DateTime a = DateTime.Now;
             new a().start();
            Console.WriteLine(ExecDateDiff(a,DateTime.Now));
            Console.ReadLine();
        }

        public class a
        {
            Timer aTimer = new Timer();
            public void start()
            {
                var accounts = AccountBLL.GetAllAccount();
              
                AccountBLL.UpdateUseState();
                var iHour = DateTime.Now.Hour;
                foreach (DataRow dr in accounts.Rows)
                {
                    // Task.Factory.StartNew(() =>
                    //  {
                    int contentNum = 0;
                    if (AutoWeb.isSend(dr, ref contentNum, iHour))
                    {
                        string useid = dr["id"].ToString();
                        string webGroupId = dr["id1"].ToString();
                        int context=int.Parse(dr["groupCount"].ToString());
                        var task = AutoWeb.GetWebSiteAwaitTask(webGroupId);
                        task.Wait();
                        if (task != null && task.Result != null&& task.Result.Rows.Count>0)
                        {
                            var hostDataTable = task.Result;
                            AutoWeb.AutoWebSit(contentNum, useid, hostDataTable, iHour, webGroupId, context);
                        }
                    }
                    //  });
                }
                //aTimer.Elapsed += new ElapsedEventHandler(TimedEvent);
                //aTimer.Interval = 1000;    //配置文件中配置的秒数  
                //aTimer.Enabled = true;
            }

            /// <summary>
            /// 售后服务单状态及物流更新
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void TimedEvent(object sender, ElapsedEventArgs e)
            {
                int iHour = e.SignalTime.Hour;
                int iMinute = e.SignalTime.Minute;
                aTimer.Enabled = false;
                try
                {               
                    var accounts = AccountBLL.GetAllAccount();
                    if (iHour == 1&& iMinute==30)
                    {
                        AccountBLL.UpdateUseState();
                    }               
                    foreach (DataRow dr in accounts.Rows)
                    {                    
                       // Task.Factory.StartNew(() =>
                      //  {
                            int contentNum = 0;
                            if (AutoWeb.isSend(dr, ref contentNum, iHour))
                            {                             
                                string useid = dr["id"].ToString();
                                string webGroupId = dr["id1"].ToString();
                                int context = int.Parse(dr["groupCount"].ToString());
                                var task = AutoWeb.GetWebSiteAwaitTask(webGroupId);
                                task.Wait();
                                if (task != null && task.Result != null)
                                {
                                    var hostDataTable = task.Result;
                                    AutoWeb.AutoWebSit(contentNum, useid, hostDataTable,iHour, webGroupId, context);
                                }
                            }
                      //  });
                    }                           
                }
                catch (Exception ex)
                {
                    LogWrite.Write("自动发布" + ex.Message);
                }
                finally
                {
                    aTimer.Enabled = true;
                }
            }
        }
        /// <summary>
        /// 程序执行时间测试
        /// </summary>
        /// <param name="dateBegin">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns>返回(秒)单位，比如: 0.00239秒</returns>
        public static string ExecDateDiff(DateTime dateBegin, DateTime dateEnd)
        {
            TimeSpan ts1 = new TimeSpan(dateBegin.Ticks);
            TimeSpan ts2 = new TimeSpan(dateEnd.Ticks);
            TimeSpan ts3 = ts1.Subtract(ts2).Duration();
            //你想转的格式
            return ts3.TotalMilliseconds.ToString();
        }

        static async Task<string> GetStrLengthAsync()
        {
            Console.WriteLine("GetStrLengthAsync方法开始执行");
            //此处返回的<string>中的字符串类型，而不是Task<string>
            string str = await GetString();//2
            Console.WriteLine("GetStrLengthAsync方法执行结束");
            return str;
        }

        static Task<string> GetString()
        {
            //Console.WriteLine("GetString方法开始执行")
            return Task<string>.Run(() =>
            {
                System.Threading.Thread.Sleep(1000);
                return "GetString的返回值";//4
            });
        }
    }
}
