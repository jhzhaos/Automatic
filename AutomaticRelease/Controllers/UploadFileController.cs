using AutomaticReleaseBLL;
using Ionic.Zip;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Caching;
using System.Threading;

namespace AutomaticRelease.Controllers
{
    public class UploadFileController : Controller
    {
        // GET: UploadFile
        [AccountAuthorizeAttribute]
        public ActionResult Index()
        {
            var webCategory = WebsiteBLL.GetwebCategory();
            var sbc = string.Empty;
            if (webCategory != null && webCategory.Rows.Count > 0)
            {
                foreach (DataRow dr in webCategory.Rows)
                {
                    sbc += "<option value='" + dr["webCategoryCode"] + "'>" + dr["webCategoryName"] + "</option>";
                }
            }
            ViewBag.webCategoryid = sbc;
            return View();
        }
        [AccountAuthorizeAttribute]
        public ActionResult UpLoadProcess(string id, string name, string type, string lastModifiedDate, int size, HttpPostedFileBase file)
        {
            var guid = Request["guid"];//前端传来的GUID号
            var fileType = Request["fileType"];
            var category = Request["categoryPath"];
            var webGroupId = Request["webGroupId"];
            string localPath = Path.Combine(HttpRuntime.AppDomainAppPath, "Upload\\" + guid);
            if (Request.Files.Count == 0)
            {
                return Json(new { jsonrpc = 2.0, error = new { code = 102, message = "保存失败" }, id = "id" });
            }
            if (!Directory.Exists(localPath))
            {
                Directory.CreateDirectory(localPath);
            }
            var filePath = Path.Combine(localPath, file.FileName);
            file.SaveAs(filePath);
            string date = DateTime.Now.ToString("yyyyMMdd");
            var path= "d:test\\" + category + "\\" + fileType + "\\" + Session["LoginUser"] + "\\" + date + "\\";
          
            DeCompressRar(filePath, path);
            UploadSaveData.SaveData(path, Session["LoginUser"].ToString(),category,fileType,date, webGroupId);         
            return Json(new
            {
                jsonrpc = "2.0",
                id = id,
                fileType = fileType
            });

        }     

        /// <summary>
        /// 将格式为rar的压缩文件解压到指定的目录
        /// </summary>
        /// <param name="rarFileName">要解压rar文件的路径</param>
        /// <param name="saveDir">解压后要保存到的目录</param>
        [AccountAuthorizeAttribute]
        public static void DeCompressRar(string rarFileName, string saveDir)
        {
            if (".zip".Equals(Path.GetExtension(rarFileName).ToLower()))
            {
                using (ZipFile zip = new ZipFile(rarFileName))
                {
                    zip.ExtractAll(saveDir, ExtractExistingFileAction.OverwriteSilently);
                }
            }
            else {
                string regKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\WinRAR.exe";
                RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(regKey);
                string winrarPath = registryKey.GetValue("").ToString();
                registryKey.Close();
                string winrarDir = System.IO.Path.GetDirectoryName(winrarPath);
                String commandOptions = string.Format("x {0} {1} -y", rarFileName, saveDir);

                ProcessStartInfo processStartInfo = new ProcessStartInfo();
                processStartInfo.FileName = System.IO.Path.Combine(winrarDir, "rar.exe");
                processStartInfo.Arguments = commandOptions;
                processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                Process process = new Process();
                process.StartInfo = processStartInfo;
                process.Start();
                process.WaitForExit();
                process.Close();
            }
        }

    }
}