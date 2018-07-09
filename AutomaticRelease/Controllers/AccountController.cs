using AutomaticReleaseBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutomaticRelease.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {        
            return View();
        }

        public ActionResult Register()
        {

            return View();
        }

        public ActionResult LoginIndex()
        {
            return View();
        }

        public ActionResult LoginIn(FormCollection form)
        {
            var username = form["username"];
            var password = form["password"];
            var rows = AccountBLL.GetAccountByUsernamePwd(username,password);
            if (rows != null)
            {            
                HttpContext.Session["userName"] = rows["userName"];
                HttpContext.Session["LoginUser"] = rows["id"];
                return Redirect("/Home/Index/");
            }
            else
            {
                return JavaScript("alert('用户名或密码错误')");
            }
        }
        public ActionResult Manage(FormCollection form)
        {           
            if (HttpContext.Session["userName"] != null)
            {        
                return View(Session["userName"]);
            }
            else
            {
                return Content("1") ;
            }
        }

        public ActionResult CancelUser()
        {
            Session["LoginUser"] = null;
            return Redirect("/Account/LoginIndex/");
        }



        [HttpPost]
        public ActionResult RegisterAccount(FormCollection form)
        {
            var username =form["username"];
            var password = form["password"];           
            var usercode = form["usercode"]==""?AccountBLL.GenerateOrderNumber(): form["usercode"];
           var rows=AccountBLL.GetAccountByUsername(username);
            var msg = string.Empty;
            if (!"0".Equals(rows))
            {
                msg = "用户名已存在";
            }
            else
            {
                var row = AccountBLL.Register(username,password,usercode);
                if (row > 0)
                {
                    msg = "用户注册成功";
                }
            }
            return JavaScript("alert('"+msg+"')");
        }

        [AccountAuthorizeAttribute]
        public ActionResult EditAccountCount(FormCollection form)
        {
            var dr = AccountBLL.GetAccount(Session["LoginUser"].ToString());
            ViewBag.startHour1 = dr["startHour1"];
            ViewBag.endHour1 = dr["endHour1"];
            ViewBag.count11 = dr["count11"];
            ViewBag.count12 = dr["count12"];

            ViewBag.startHour2 = dr["startHour2"];
            ViewBag.endHour2 = dr["endHour2"];
            ViewBag.count21 = dr["count21"];
            ViewBag.count22 = dr["count22"];

            ViewBag.startHour3 = dr["startHour3"];
            ViewBag.endHour3 = dr["endHour3"];
            ViewBag.count31 = dr["count31"];
            ViewBag.count32 = dr["count32"];
            return View();
        }

        [AccountAuthorizeAttribute]
        public ActionResult SaveUser(FormCollection form)
        {
            JsonResult json = new JsonResult();

            var row = AccountBLL.UpdateUser(form["startHour1"], form["endHour1"], form["count11"], form["count12"], form["startHour2"], form["endHour2"], form["count21"], form["count22"], form["startHour3"], form["endHour3"], form["count31"], form["count32"], Session["LoginUser"].ToString());
            if (row > 0)
            {
                json.Data = new
                {
                    result = "true"
                };
            }

            return json;
        }
    }
}