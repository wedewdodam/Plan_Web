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
    public class LogsController : Controller
    {
        //[User][6][1]
        private IkhmaInfor_Lib _infor_Lib;

        private IAptInfor_Lib _AInfor_Lib;
        private ILogView _logv;
        private IStaff _staff;

        //private object infor_Lib;
        private IApt_Detail_Lib _apt_Detail;

        //private IUserModelRepository _userRepo;

        public LogsController(
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

        public IActionResult Index()
        {
            ViewBag.Title = "로그인 뷰";
            return View();
        }

        private khma_AptMemberCareer_Entity ann { get; set; } = new khma_AptMemberCareer_Entity();

        [HttpPost]
        public async Task<IActionResult> Index(string mem_id, string mem_pw)
        {
            if (mem_id != null && mem_pw != null)
            {
                var result = _infor_Lib.logview(mem_id, mem_pw);
                if (result > 0)
                {
                    ann = await _infor_Lib.kmi_detail(mem_id);
                    var claims = new List<Claim>()
                    {
                        // 로그인 아이디 지정
                        //new Claim("UserId", mem_id),

                        new Claim("UserCode", mem_id),
                        new Claim("AptCode", ann.apt_cd),
                        new Claim(ClaimTypes.Name, ann.mem_nm),
                        new Claim("Adress", ann.ADDR_BASE + ann.ADDR_DETAIL),
                        new Claim("BuildDate", ann.BUILD_DATE),
                        new Claim("AptName", ann.APT_NM)
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

        public async Task<IActionResult> LogOut()
        {
            // 로그아웃
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return LocalRedirect("/");
        }

        public IActionResult Login()
        {
            ViewBag.Title = "로그인 뷰";
            return View();
        }

        [Route("Logs/LogViewAd/")]
        [HttpPost]
        public async Task<IActionResult> Login(string mem_id, string mem_pw)
        {
            //LogView_Entity lnn = new LogView_Entity();

            //Staff_Entity st = await

            if (mem_id != null && mem_pw != null)
            {
                var result = await _logv.LogIn(mem_id, mem_pw);

                if (result > 0)
                {
                    string Apt_Code = await _logv.GetDetail_LogView(mem_id);
                    Staff_Entity st = await _staff.Detail_Staff(Apt_Code, mem_id);
                    AptInfor_Entity at = await _AInfor_Lib.Detail_Apt(Apt_Code);
                    Apt_Detail_Entity ad = await _apt_Detail.Detail_AptDetail(Apt_Code);
                    var claims = new List<Claim>()
                    {
                        new Claim("UserCode", mem_id),
                        new Claim("AptCode", Apt_Code),
                        new Claim(ClaimTypes.Name, st.Staff_Name),
                        new Claim("Adress", at.Apt_Adress_Sido + " " + at.Apt_Adress_Gun + " " + at.Apt_Adress_Rest),
                        new Claim("BuildDate", at.AcceptancedOfWork_Date.ToString()),
                        new Claim("AptName", at.Apt_Name),
                        //new Claim("Developer", ad.Developer),
                        //new Claim("Builder", ad.Builder)
                    };

                    var ci = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(ci)); // 옵션

                    HttpContext.Session.SetString("SessionVariable1", "Testing123");

                    return LocalRedirect(Url.Content("~/"));
                }
            }
            return View();
        }
    }
}