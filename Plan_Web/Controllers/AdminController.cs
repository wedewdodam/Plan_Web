using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Plan_Apt_Lib;
using Plan_Blazor_Lib;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Plan_Web.Controllers
{
    public class AdminController : Controller
    {
        //[User][6][1]
        private IkhmaInfor_Lib _infor_Lib;

        private IAptInfor_Lib _AInfor_Lib;
        private ILogView _logv;
        private IStaff _staff;

        //private object infor_Lib;
        private IApt_Detail_Lib _apt_Detail;

        //private IUserModelRepository _userRepo;

        public AdminController(
            IkhmaInfor_Lib infor_Lib,
            IAptInfor_Lib Ainfor_Lib,
            //IRepair_Lib_Apt Apt,
            IStaff Staff,
            IApt_Detail_Lib apt_Detail,
            ILogView logView)
        {
            _infor_Lib = infor_Lib;
            _AInfor_Lib = Ainfor_Lib;
            _logv = logView;
            _staff = Staff;
            //_apt = Apt;
            _apt_Detail = apt_Detail;
            //_userRepo = userRepo; // 추가
        }

        // GET: AdminController
        public ActionResult Index()
        {
            return View();
        }

        private Staff_Entity ann = new Staff_Entity();
        private AptInfor_Entity bnn = new AptInfor_Entity();

        [HttpPost]
        public async Task<IActionResult> Index(string mem_id, string mem_pw)
        {
            if (mem_id != null && mem_pw != null)
            {
                var result = await _logv.LogIn(mem_id, mem_pw);
                if (result > 0)
                {
                    string strApt = await _logv.GetDetail_LogView(mem_id);
                    ann = await _staff.Detail_Staff(strApt, mem_id);
                    bnn = await _AInfor_Lib.Detail_Apt(strApt);
                    var claims = new List<Claim>()
                    {
                        // 로그인 아이디 지정
                        //new Claim("UserId", mem_id),

                        new Claim("UserCode", mem_id),
                        new Claim("AptCode", strApt),
                        new Claim(ClaimTypes.Name, ann.Staff_Name),
                        new Claim("Adress", bnn.Apt_Adress_Sido + " " + bnn.Apt_Adress_Gun),
                        new Claim("BuildDate", bnn.AcceptancedOfWork_Date.ToShortDateString()),
                        new Claim("AptName", bnn.Apt_Name),
                        new Claim("LevelCount", ann.LevelCount.ToString())
                    };

                    var ci = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    //var authenticationProperties = new AuthenticationProperties()
                    //{
                    //    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
                    //    IssuedUtc = DateTimeOffset.UtcNow,
                    //    IsPersistent = true
                    //};

                    //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(ci), authenticationProperties); // 옵션
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(ci)); // 옵션

                    HttpContext.Session.SetString("SessionVariable1", "Testing123");
                    //HttpContext.Session.SetString("SessionKeyName", "The Doctor");
                    //HttpContext.Session.SetInt32("SessionKeyAge", 773);

                    return LocalRedirect(Url.Content("~/"));
                }
            }
            return View();
        }

        // GET: AdminController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AdminController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AdminController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}