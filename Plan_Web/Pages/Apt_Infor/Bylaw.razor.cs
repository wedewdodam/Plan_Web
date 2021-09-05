using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Plan_Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plan_Web.Pages.Apt_Infor
{
    public partial class Bylaw
    {
        #region 인스턴스

        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public NavigationManager MyNav { get; set; } // Url
        [Inject] private IJSRuntime JSRuntime { get; set; }
        [Inject] public IBylaw_Lib bylaw_Lib { get; set; }
        [Inject] public IRelation_Law_Lib relation_Law_Lib { get; set; }

        #endregion 인스턴스

        #region 속성

        private Bylaw_Entity ann { get; set; } = new Bylaw_Entity();
        private Relation_Law_Entity bnn { get; set; } = new Relation_Law_Entity();
        private List<Relation_Law_Entity> bnnA { get; set; } = new List<Relation_Law_Entity>();
        private List<Bylaw_Entity> annA { get; set; } = new List<Bylaw_Entity>();

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

        public string strTitleA { get; set; }
        public string strTitleB { get; set; }

        public string ViewsA { get; set; }
        public string ViewsB { get; set; }

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
            annA = await bylaw_Lib.GetList(Apt_Code);
            bnnA = await relation_Law_Lib.GetList_Set("");
        }

        #region 관리규약 관련 메서드

        /// <summary>
        /// 관리규약 정보 입력 열기
        /// </summary>
        private async Task btnOpenA()
        {
            InsertViewsA = "B";
            ann = new Bylaw_Entity();
            ann.Bylaw_Revision_Date = DateTime.Now.Date;
            ann.Bylaw_Revision_Num = (await bylaw_Lib.Bylaw_Revision(Apt_Code)) + 1;
            ann.Approval_Rate = 51;
            strTitleA = "관리규약 개정 정보 등록";
        }

        /// <summary>
        /// 관리규약 정보 수정 열기
        /// </summary>
        /// <param name="bylaw"></param>
        private void OnEditA(Bylaw_Entity bylaw)
        {
            InsertViewsA = "B";
            ann = bylaw;
            strTitleA = "관리규약 개정 정보 수정";
        }

        /// <summary>
        /// 관리규약 정보
        /// </summary>
        private void btnCloseA()
        {
            InsertViewsA = "A";
        }

        /// <summary>
        /// 관리규약 정보 저장 및 수정
        /// </summary>
        /// <returns></returns>
        public async Task btnSaveA()
        {
            if (ann.Bylaw_Revision_Num < 1)
            {
                await JSRuntime.InvokeAsync<object>("alert", "개정차수가 선택되지 않았습니다.");
            }
            else if (ann.Proposer == "" || ann.Proposer == null)
            {
                await JSRuntime.InvokeAsync<object>("alert", "제안자가 선택되지 않았습니다.");
            }
            else if (ann.Approval_Rate < 51 || ann.Approval_Rate > 100)
            {
                await JSRuntime.InvokeAsync<object>("alert", "동의율은 50이상 100이하이어야 합니다.");
            }
            else if (ann.Bylaw_Law_Basis == "")
            {
                await JSRuntime.InvokeAsync<object>("alert", "개정 이유를 입력하지 않았습니다.");
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
                ann.PostIP = myIPAddress;

                #endregion 아이피 입력

                ann.Apt_Code = Apt_Code;
                ann.Staff_Code = User_Code;

                if (ann.Bylaw_Code < 1)
                {
                    await bylaw_Lib.Add_Bylaw(ann);
                }
                else
                {
                    await bylaw_Lib.Edit_Bylaw(ann);
                }

                ann = new Bylaw_Entity();
                InsertViewsA = "A";

                await DisplayViews();
            }
        }

        /// <summary>
        /// 관리규약 정보 삭제
        /// </summary>
        /// <param name="Aid"></param>
        /// <returns></returns>
        private async Task OnRemoveA(int Aid)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{Aid}번 글을 정말로 삭제하시겠습니까?");
            if (isDelete)
            {
                await bylaw_Lib.Remove_Repair_Cost(Aid);
                await DisplayViews();
            }
        }

        #endregion 관리규약 관련 메서드
    }
}
