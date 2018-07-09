using _6System;
using AutomaticReleaseBLL;
using System;
using System.Data;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Timers;

namespace TimeService
{
    public partial class PurchaseOrderTrackService : ServiceBase
    {
        Timer aTimer = new Timer();       //System.Timers
        Timer bTimer = new Timer();

        public PurchaseOrderTrackService()
        {
            InitializeComponent();
        }
        protected override void OnStart(string[] args)
        {
            aTimer.Elapsed += new ElapsedEventHandler(TimedEvent);
            aTimer.Interval = 1000;    //配置文件中配置的秒数  
            aTimer.Enabled = true;

            bTimer.Elapsed += new ElapsedEventHandler(TimedEventb);
            bTimer.Interval = 1000;    //配置文件中配置的秒数  
            bTimer.Enabled = true;
        }
        protected override void OnStop()
        {
            aTimer.Enabled =false;
            bTimer.Enabled = false;
        }


       
        private void TimedEvent(object sender, ElapsedEventArgs e)
        {
            int iHour = e.SignalTime.Hour;
            int iMinute = e.SignalTime.Minute;
            int iSecond = e.SignalTime.Second;
            aTimer.Enabled = false;
            try
            {
                var accounts = AccountBLL.GetAllAccount();              
                foreach (DataRow dr in accounts.Rows)
                {
                    Task.Factory.StartNew(() =>
                    {
                        int contentNum = 0;
                        if (AutoWeb.isSend(dr, ref contentNum, iHour))
                        {
                            string useid = dr["id"].ToString();
                            string webGroupId = dr["id1"].ToString();
                            int context = int.Parse(dr["groupCount"].ToString());
                            var task = AutoWeb.GetWebSiteAwaitTask(webGroupId);
                            task.Wait();
                            if (task != null && task.Result != null && task.Result.Rows.Count > 0)
                            {
                                var hostDataTable = task.Result;
                                AutoWeb.AutoWebSit(contentNum, useid, hostDataTable, iHour, webGroupId, context);
                            }
                        }
                    });
                }
                AutoWeb.UpateAccount(iHour);
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

        private void TimedEventb(object sender, ElapsedEventArgs e)
        {
            int iHour = e.SignalTime.Hour;
            int iMinute = e.SignalTime.Minute;
            int iSecond = e.SignalTime.Second;
            aTimer.Enabled = false;
            try
            {              
                if (iHour == 1 && iMinute == 30 && iSecond <=1)
                {
                    AccountBLL.UpdateUseState();
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
    
}
