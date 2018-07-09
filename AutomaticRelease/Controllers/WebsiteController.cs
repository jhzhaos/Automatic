using AutomaticReleaseBLL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutomaticRelease.Controllers
{
    public class WebsiteController : Controller
    {
        // GET: Website
        [AccountAuthorizeAttribute]
        public ActionResult Index()
        {
            return View();
        }
        [AccountAuthorizeAttribute]
        public ActionResult GetWebSite()
        {
            var id = Request["id"]== "undefined"? null : Request["id"];
            if (Request["wid"] != null)
            {
                id = Request["wid"];
            }          
           var data= WebsiteBLL.GetWebsite(Session["LoginUser"].ToString(),id);           
           var JsonString = JsonConvert.SerializeObject(data);
            return Json(JsonString);
        }

        public ActionResult GetWebGroup()
        {
            var data = WebsiteBLL.GetWebGroup(Session["LoginUser"].ToString());
            var JsonString = JsonConvert.SerializeObject(data);
            return Json(JsonString);
        }

        [AccountAuthorizeAttribute]
        public ActionResult AddWebSite()
        {
            var id = Request["id"];
            ViewBag.webGroupId = Request["Gid"];
            var webPlatform = WebsiteBLL.GetwebPlatform();
            var webCategory= WebsiteBLL.GetwebCategory();
            if (id!= null)
            {
                var dt = WebsiteBLL.GetWebsiteByid(id);          
                if (dt != null && dt.Rows.Count > 0)
                {
                    var keyNames = string.Join(",", dt.AsEnumerable().Select(d => d.Field<string>("keyName")).ToArray());
                    var keyUrls = string.Join(",", dt.AsEnumerable().Select(d => d.Field<string>("keyUrl")).ToArray());
                    var webData = dt.Rows[0];                             
                    ViewBag.webName = webData["webName"];
                    ViewBag.loginName = webData["loginName"];
                    ViewBag.loginPassword = webData["loginPassword"];
                    ViewBag.webHost = webData["webHost"];
                    ViewBag.categoryCode = webData["categoryCode"];
                    ViewBag.categoryName = webData["categoryName"];
                    ViewBag.id = webData["id"];
                    ViewBag.webAdminUrl = webData["webAdminUrl"];          
                    ViewBag.WordKey = keyNames;
                    ViewBag.WordUrl = keyUrls;
                    ViewBag.webGroupId= webData["webGroupId"];
                    ViewBag.webImgPath = webData["webImgPath"];
                    var wu = string.Empty;
                    if (keyNames != "")
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            wu += "<tr><td>" + dr["keyName"] + "</td><td>" + dr["keyUrl"] + "</td><td><a href='#' onclick='DeleteWebKey(this)'>删除</a></td></tr>";
                        }
                        ViewBag.keylist = wu;
                    }
                    var sb = string.Empty;
                    if (webPlatform != null && webPlatform.Rows.Count > 0)
                    {
                        foreach(DataRow dr in webPlatform.Rows)
                        {
                            if (dr["id"].ToString() == webData["webPlatformId"].ToString())
                            {
                                sb += "<option value='" + dr["id"] + "' selected='selected'>" + dr["webPlatformName"] + "</option>";
                                continue;
                            }
                            sb += "<option value='" + dr["id"] + "'>" + dr["webPlatformName"] + "</option>";
                        }
                    }
                    ViewBag.webPlatformId = sb;
                    var sbc = string.Empty;
                    if (webCategory != null && webCategory.Rows.Count > 0)
                    {
                        foreach (DataRow dr in webCategory.Rows)
                        {
                            if (dr["id"].ToString() == webData["webCategoryId"].ToString())
                            {
                                sbc += "<option value='" + dr["id"] + "' selected='selected'>" + dr["webCategoryName"] + "</option>";
                                continue;
                            }
                            sbc += "<option value='" + dr["id"] + "'>" + dr["webCategoryName"] + "</option>";
                        }
                    }
                    ViewBag.webCategoryid = sbc;

                }                
            }
            else
            {
                var sb = string.Empty;
                if (webPlatform != null && webPlatform.Rows.Count > 0)
                {
                    foreach (DataRow dr in webPlatform.Rows)
                    {
                        sb += "<option value='" + dr["id"] + "'>" + dr["webPlatformName"] + "</option>";
                    }
                }
                ViewBag.webPlatformId = sb;
                var sbc = string.Empty;
                if (webCategory != null && webCategory.Rows.Count > 0)
                {
                    foreach (DataRow dr in webCategory.Rows)
                    {
                        sbc += "<option value='" + dr["id"] + "'>" + dr["webCategoryName"] + "</option>";
                    }
                }
                ViewBag.webCategoryid = sbc;
            }
            return View();
        }
        [AccountAuthorizeAttribute]
        public ActionResult SaveWebSite(FormCollection form)
        {
            var host = "http://" + (form["webHost"].Replace("http://", "").Replace("/", ""));
            var webAdminUrl = "/"+form["webAdminUrl"].Replace("http://", "").TrimEnd('/').TrimStart('/')+"/";
            if (form["webPlatformId"] == "1")
            {
                webAdminUrl = webAdminUrl.TrimEnd('/');
            }
            var imgPath= "/" + form["webImgPath"].Replace("http://", "").TrimEnd('/').TrimStart('/') + "/";
            var msg = string.Empty;
            var id = form["id"];
            JsonResult json = new JsonResult();
            if ( id=="")
            {
                var rows = WebsiteBLL.GetWebsiteByHost(host);              
                if (!"0".Equals(rows))
                {
                    msg = "该网站已经存在";
                    return JavaScript("alert('" + msg + "')");
                }
                else
                {
                    var row = WebsiteBLL.AddWebsite(form["webName"], form["username"], form["password"], webAdminUrl, host, form["webPlatformId"], form["categoryCode"],form["categoryName"],form["webCategoryid"], form["webGroupId"], form["WordKey"], form["kerUrls"],imgPath);
                    if (row > 0)
                    {
                        json.Data = new
                        {
                            result = "true",
                            id=id
                        };
                    }
                }
            }
            else
            {
                var row = WebsiteBLL.UpdateWebsite(form["webName"],id, form["username"], form["password"], webAdminUrl, host, form["webPlatformId"], form["categoryCode"], form["categoryName"], form["webCategoryid"], form["webGroupId"], form["WordKey"], form["kerUrls"], imgPath);
                if (row > 0)
                {
                    json.Data = new
                    {
                        result = "true",
                        id = id
                    };
                }
            }
            return json;
        }

        [AccountAuthorizeAttribute]
        public ActionResult SaveWebSiteGroup(FormCollection form)
        {        
            var msg = string.Empty;
            var id = form["id"];
            var name = form["txtGroupName"];
            var count = form["txtCount"];
            var pareId = form["pareId"]==null?"0" : form["pareId"];
            JsonResult json = new JsonResult();
            if (id == "")
            {
                var rows = WebsiteBLL.GetWebsiteGroupByName(name);
                if (!"0".Equals(rows))
                {
                    msg = "该网站分组已经存在";
                    return JavaScript("alert('" + msg + "')");
                }
                else
                {
                    var row = WebsiteBLL.AddWebsiteGroup(name, pareId, Session["LoginUser"].ToString(),count);
                    if (row > 0)
                    {
                        json.Data = new
                        {
                            result = "true"
                        };
                    }
                }
            }
            else
            {
                var row = WebsiteBLL.UpDateWebsiteGroup(name,id,count);
                if (row > 0)
                {
                    json.Data = new
                    {
                        result = "true"
                    };
                }
            }
            return json;
        }
        [AccountAuthorizeAttribute]
        public ActionResult DeleteWebSite(string id)
        {
            var data = WebsiteBLL.DeleteWebSite(id);         
            JsonResult json = new JsonResult();
            if (data > 0)
            {
                json.Data = new
                {
                    result = "true"
                };
            }
            else
            {
                json.Data = new
                {
                    result = "false"
                };
            }          
            return json;
        }

        [AccountAuthorizeAttribute]
        public ActionResult DeleteWebKey(string id)
        {
            var data = WebsiteBLL.DeleteWebKey(id);
            JsonResult json = new JsonResult();
            if (data > 0)
            {
                json.Data = new
                {
                    result = "true"
                };
            }
            else
            {
                json.Data = new
                {
                    result = "false"
                };
            }
            return json;
        }

        [AccountAuthorizeAttribute]
        public ActionResult DeleteWebSiteGroup(string id)
        {
            var data = WebsiteBLL.DeleteWebSiteGroup(id);
            JsonResult json = new JsonResult();
            if (data > 0)
            {
                json.Data = new
                {
                    result = "true"
                };
            }

            else
            {
                json.Data = new
                {
                    result = "false"
                };
            }
            return json;
        }
        [AccountAuthorizeAttribute]
        public ActionResult MyArticle()
        {
            var webGroupId = Request["webGroupId"];
            var data = WebsiteBLL.GetAllArticleByUserId(Session["LoginUser"].ToString(),webGroupId);
            string body = string.Empty;
            foreach(DataRow dr in data.Rows)
            {
                body +=string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>",dr["title"].ToString(),dr["seoWord"].ToString(),dr["date"].ToString());
            }
            ViewBag.body = body;
            ViewBag.bodyCount = data.Rows.Count;
            return View();
        }
        [AccountAuthorizeAttribute]
        public ActionResult loginTest(string id)
        {
            var data = WebsiteBLL.GetWebsiteByid(id);
            var login = WebsiteBLL.loginTest(data.Rows[0]);
            JsonResult json = new JsonResult();
            if (login)
            {
                json.Data = new
                {
                    result = "true"
                };
            }
            else
            {
                json.Data = new
                {
                    result = "false"
                };
            }
            return json;
        }
    }
}