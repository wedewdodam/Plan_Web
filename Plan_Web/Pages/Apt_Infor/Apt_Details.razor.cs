using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Plan_Apt_Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plan_Web.Pages.Apt_Infor
{
    public partial class Apt_Details
    {
        #region 인스턴스

        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public NavigationManager MyNav { get; set; } // Url
        [Inject] private IJSRuntime JSRuntime { get; set; }
        [Inject] public IAptInfor_Lib aptInfor_Lib { get; set; }
        [Inject] public IApt_Detail_Lib apt_Detail_Lib { get; set; }
        [Inject] public IDong_Lib dong_Lib { get; set; }
        [Inject] public IDong_Composition dong_Composition { get; set; }

        #endregion 인스턴스

        #region 속성

        private AptInfor_Entity ann { get; set; } = new AptInfor_Entity();
        private Dong_Entity bnn { get; set; } = new Dong_Entity();
        private List<Dong_Entity> bnnA { get; set; } = new List<Dong_Entity>();
        private List<Dong_Composition_Entity> bnnB { get; set; } = new List<Dong_Composition_Entity>();
        private Dong_Composition_Entity FnnA { get; set; } = new Dong_Composition_Entity();
        private Dong_Composition_Entity FnnB { get; set; } = new Dong_Composition_Entity();

        #endregion 속성

        #region 변수

        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public string InsertViewsA { get; set; } = "A";
        public string InsertViewsB { get; set; } = "A";

        public int EditViewsA { get; set; } = 0;
        public int EditViewsB { get; set; } = 0;

        public string strTitle { get; set; }
        public int num { get; set; } = 0;

        public string PArea { get; set; }
        public string TArea { get; set; }

        #endregion 변수

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateRef;
            if (authState.User.Identity.IsAuthenticated)
            {
                //로그인 정보
                Apt_Code = authState.User.Claims.FirstOrDefault(c => c.Type == "AptCode")?.Value;
                User_Code = authState.User.Claims.FirstOrDefault(c => c.Type == "UserCode")?.Value;
                Apt_Name = authState.User.Claims.FirstOrDefault(c => c.Type == "AptName")?.Value;
                User_Name = authState.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;

                await DisplayViews();
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인하지 않았습니다.");
                MyNav.NavigateTo("/");
            }
        }

        /// <summary>
        /// 데이터 뷰
        /// </summary>
        /// <returns></returns>
        private async Task DisplayViews()
        {
            bnnA = await dong_Lib.GetList_Dong(Apt_Code);
            bnnB = await dong_Composition.GetList_Dong_Composition(Apt_Code);

            num = await dong_Lib.Being_Family_Count(Apt_Code);

            FnnB = await dong_Composition.Total_Infor(Apt_Code);
        }

        /// <summary>
        /// 동면적 계산
        /// </summary>
        private void onWidthA(ChangeEventArgs a)
        {
            int intA = Convert.ToInt32(a.Value);
            bnn.Width = intA;
            if (bnn.Width > 0 && bnn.Length > 0)
            {
                bnn.Dong_Area = intA * bnn.Length;
            }
            else
            {
                bnn.Dong_Area = 0;
            }
        }

        /// <summary>
        /// 동면적 계산
        /// </summary>
        private void onWidthB(ChangeEventArgs a)
        {
            int intA = Convert.ToInt32(a.Value);
            bnn.Length = intA;
            if (bnn.Length > 0 && bnn.Width > 0)
            {
                bnn.Dong_Area = intA * bnn.Length;
            }
            else
            {
                bnn.Dong_Area = 0;
            }
        }

        /// <summary>
        /// 구성면적 합계 계산
        /// </summary>
        private void onTAreaA(ChangeEventArgs a)
        {
            double dbA = Convert.ToDouble(a.Value);
            FnnA.Supply_Area = dbA;
            PArea = string.Format("{0: ###,###.#}", dbA);

            if (FnnA.Supply_Area > 0 && FnnA.Area_Family_Num > 0)
            {
                FnnA.Total_Area = FnnA.Supply_Area * FnnA.Area_Family_Num;
            }
            else
            {
                FnnA.Total_Area = 0;
            }
        }

        /// <summary>
        /// 구성면적 합계 계산
        /// </summary>
        private void onTAreaB(ChangeEventArgs a)
        {
            int dbA = Convert.ToInt32(a.Value);
            FnnA.Area_Family_Num = dbA;
            if (FnnA.Supply_Area > 0 && FnnA.Area_Family_Num > 0)
            {
                FnnA.Total_Area = FnnA.Supply_Area * FnnA.Area_Family_Num;
            }
            else
            {
                FnnA.Total_Area = 0;
            }
        }

        /// <summary>
        /// 동현황 정보 입력 열기
        /// </summary>
        private void btnOpenA()
        {
            bnn = new Dong_Entity();
            InsertViewsA = "B";

            strTitle = "동현황 정보 입력";
        }

        /// <summary>
        /// 동구성정보 입력 열기
        /// </summary>
        private void btnOpenB()
        {
            FnnA = new Dong_Composition_Entity();
            InsertViewsB = "B";

            strTitle = "동구성 정보 입력";
        }

        /// <summary>
        /// 동현황정보 입력 닫기
        /// </summary>
        private void btnCloseA()
        {
            InsertViewsA = "A";
        }

        /// <summary>
        /// 동구성정보 입력 닫기
        /// </summary>
        private void btnCloseB()
        {
            InsertViewsB = "A";
        }

        /// <summary>
        /// 동현황 정보 수정 열기
        /// </summary>
        private void OnEditA(Dong_Entity dong)
        {
            InsertViewsA = "B";
            bnn = dong;
            strTitle = "동현황 정보 수정";
        }

        /// <summary>
        /// 동구성 정보 수정 열기
        /// </summary>
        private void OnEditB(Dong_Composition_Entity dong)
        {
            FnnA = dong;
            double db = FnnA.Supply_Area / 3.3;
            PArea = string.Format("{0: ###,###.#}", db);
            InsertViewsB = "B";
            strTitle = "동구성 정보 수정";
        }

        /// <summary>
        /// 동정보 삭제
        /// </summary>
        /// <param name="Aid"></param>
        /// <returns></returns>
        private async Task OnRemoveA(int Aid)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{Aid}번 글을 정말로 삭제하시겠습니까?");
            if (isDelete)
            {
                await dong_Lib.Remeove_Dong(Aid);
                await DisplayViews();
            }
        }

        /// <summary>
        /// 동구성정보 삭제
        /// </summary>
        /// <param name="Aid"></param>
        /// <returns></returns>
        private async Task OnRemoveB(int Aid)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{Aid}번 글을 정말로 삭제하시겠습니까?");
            if (isDelete)
            {
                await dong_Composition.Remeove_Dong_Composition(Aid);
                await DisplayViews();
            }
        }

        /// <summary>
        /// 동현황 정보 저장 및 수정
        /// </summary>
        /// <returns></returns>
        private async Task btnSaveA()
        {
            bnn.Apt_Code = Apt_Code;
            if (bnn.Apt_Code == null || bnn.Apt_Code == "")
            {
                await JSRuntime.InvokeAsync<object>("alert", "로그인되지 않았습니다.");
            }
            else if (bnn.Length <= 0)
            {
                await JSRuntime.InvokeAsync<object>("alert", "동이를 입력하지 않았습니다.");
            }
            else if (bnn.Width <= 0)
            {
                await JSRuntime.InvokeAsync<object>("alert", "동폭을 입력하지 않았습니다.");
            }
            else if (bnn.Dong_Area <= 0)
            {
                await JSRuntime.InvokeAsync<object>("alert", "동면적이 입력되지 않았습니다.");
            }
            else if (bnn.Dong_Name == null || bnn.Dong_Name == "")
            {
                await JSRuntime.InvokeAsync<object>("alert", "동이름이 입력되지 않았습니다.");
            }
            else if (bnn.Exit_Num <= 0)
            {
                await JSRuntime.InvokeAsync<object>("alert", "출구수가 입력되지 않았습니다.");
            }
            else if (bnn.Hall_Form == null || bnn.Dong_Name == "Z")
            {
                await JSRuntime.InvokeAsync<object>("alert", "복도형태가 선택되지 않았습니다.");
            }
            else if (bnn.Roof_Form == null || bnn.Roof_Form == "Z")
            {
                await JSRuntime.InvokeAsync<object>("alert", "지붕형태가 선택되지 않았습니다.");
            }
            else if (bnn.Elevator_Num <= 0)
            {
                await JSRuntime.InvokeAsync<object>("alert", "승강기 수가 입력되지 않았습니다.");
            }
            else if (bnn.Family_Num <= 0)
            {
                await JSRuntime.InvokeAsync<object>("alert", "세대수가 입력되지 않았습니다.");
            }
            else if (bnn.Floor_Num <= 0)
            {
                await JSRuntime.InvokeAsync<object>("alert", "층수가 입력되지 않았습니다.");
            }
            else
            {
                #region 아이피 입력

                string myIPAddress = "";
                var ipentry = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
                foreach (var ip in ipentry.AddressList)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        myIPAddress = ip.ToString();
                        break;
                    }
                }
                bnn.PostIP = myIPAddress;

                #endregion 아이피 입력

                bnn.Dong_Code = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
                if (bnn.Dong_Etc == "" || bnn.Dong_Etc == null)
                {
                    bnn.Dong_Etc = "기재 안함.";
                }

                if (bnn.Aid < 1)
                {
                    await dong_Lib.Add_Dong(bnn);
                }
                else
                {
                    await dong_Lib.Edit_Dong(bnn);
                }

                bnn = new Dong_Entity();
                await DisplayViews();
                InsertViewsA = "A";
            }
        }

        /// <summary>
        /// 동구성 정보 저장 및 수정
        /// </summary>
        /// <returns></returns>
        private async Task btnSaveB()
        {
            FnnA.Apt_Code = Apt_Code;
            if (FnnA.Apt_Code == "" || FnnA.Apt_Code == null)
            {
                await JSRuntime.InvokeAsync<object>("alert", "로그인되지 않았습니다.");
            }
            else if (FnnA.Area_Family_Num <= 0)
            {
                await JSRuntime.InvokeAsync<object>("alert", "세대수가 입력되지 않았습니다.");
            }
            else if (FnnA.Supply_Area <= 0)
            {
                await JSRuntime.InvokeAsync<object>("alert", "공급면적이 입력되지 않았습니다.");
            }
            else if (FnnA.Total_Area <= 0)
            {
                await JSRuntime.InvokeAsync<object>("alert", "합계면적이 입력되지 않았습니다.");
            }
            else
            {
                #region 아이피 입력

                string myIPAddress = "";
                var ipentry = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
                foreach (var ip in ipentry.AddressList)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        myIPAddress = ip.ToString();
                        break;
                    }
                }
                FnnA.PostIP = myIPAddress;

                #endregion 아이피 입력

                FnnA.Dong_Code = "A";

                FnnA.Dong_Composition_Code = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();

                if (FnnA.Dong_Etc == "" || FnnA.Dong_Etc == null)
                {
                    FnnA.Dong_Etc = "기재 안함.";
                }

                if (FnnA.Aid < 1)
                {
                    await dong_Composition.Add_Dong_Composition(FnnA);
                    await DisplayViews();
                }
                else
                {
                    await dong_Composition.Edit_Dong_Composition(FnnA);
                }

                FnnA = new Dong_Composition_Entity();
                InsertViewsB = "A";
            }
        }
    }
}
