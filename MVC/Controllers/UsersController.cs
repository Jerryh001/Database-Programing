using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Security.Cryptography;
using System.Text;
using MVC.Models;

namespace MVC.Controllers
{
    public class UsersController : Controller
    {
        private Database1Entities db = new Database1Entities();

        public static String Encrypt(String plaintext)
        {
            SHA256 s = new SHA256CryptoServiceProvider();
            return Convert.ToBase64String(s.ComputeHash(Encoding.Default.GetBytes(plaintext)));
        }

        public ActionResult Login(LoginModel form)
        {
            ViewBag.username = form.in_username;
            ViewBag.password = form.in_password;
            User check = (from a in db.Users
                        where a.UserName == form.in_username
                        select a).FirstOrDefault();
            if (check == null)
            {
                ViewBag.message = "帳號密碼錯誤";
                return View();
            }
            String saltpass = Encrypt(check.Id + form.in_password);
            ViewBag.Epass = saltpass;
            ViewBag.Rpass = check.PassWord;
            if(saltpass==check.PassWord)
            {
                ViewBag.message = "登入成功";
                String eyticket = FormsAuthentication.Encrypt(new FormsAuthenticationTicket(1, form.in_username, DateTime.Now, DateTime.Now.AddMinutes(30), false, "user data", FormsAuthentication.FormsCookiePath));
                HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, eyticket);
                cookie.HttpOnly = true;
                Response.Cookies.Add(cookie);
            }
            else
            {
                ViewBag.message = "帳號密碼錯誤";
            }
            return View();
        }
        [Authorize]
        public ActionResult Authed()
        {
            return View();
        }
        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return View();
        }
        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserName,PassWord")] User user)
        {
            if (ModelState.IsValid)
            {
                user.Id = Guid.NewGuid();
                user.PassWord = Encrypt(user.Id+user.PassWord);
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserName,PassWord")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
