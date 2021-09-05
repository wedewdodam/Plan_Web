using Facility;
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
    public partial class Index
    {
        #region 인스턴스

        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public NavigationManager MyNav { get; set; } // Url
        [Inject] private IJSRuntime JSRuntime { get; set; }
        [Inject] public IAptInfor_Lib aptInfor_Lib { get; set; }
        [Inject] public IApt_Detail_Lib apt_Detail_Lib { get; set; }
        [Inject] public IAdditional_Welfare_Facility_Lib additional_Welfare_Facility_Lib { get; set; }

        #endregion 인스턴스

        #region 속성

        private AptInfor_Entity ann { get; set; } = new AptInfor_Entity();
        public Apt_Detail_Entity bnn { get; set; } = new Apt_Detail_Entity();
        private List<Additional_Welfare_Facility> annA { get; set; } = new List<Additional_Welfare_Facility>();
        private List<Additional_Welfare_Facility> annB { get; set; } = new List<Additional_Welfare_Facility>();
        private Additional_Welfare_Facility FnnA { get; set; } = new Additional_Welfare_Facility();
        private Additional_Welfare_Facility FnnB { get; set; } = new Additional_Welfare_Facility();

        #endregion 속성

        #region 변수

        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public string InsertViews { get; set; } = "A";
        public string InsertViewsA { get; set; } = "A";
        public string InsertViewsB { get; set; } = "A";
        public string InsertViewsC { get; set; } = "A";
        public string InsertViewsD { get; set; } = "A";
        public int EditViews { get; set; } = 0;
        public int EditViewsA { get; set; } = 0;
        public int EditViewsB { get; set; } = 0;
        public int EditViewsC { get; set; } = 0;
        public int EditViewsD { get; set; } = 0;
        public string strTitle { get; set; }
        public string strTitleA { get; set; } = "등록";
        public string strTitleC { get; set; } = "등록";
        public string strTitleD { get; set; } = "등록";
        //public int num { get; set; } = 0;

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
                //num = 0;

                if (EditViews > 0)
                {
                    strTitle = "수정";
                }
                else
                {
                    strTitle = "입력";
                }
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인하지 않았습니다.");
                MyNav.NavigateTo("/");
            }
        }

        private async Task DisplayViews()
        {
            EditViews = await apt_Detail_Lib.Being(Apt_Code);
            EditViewsA = await additional_Welfare_Facility_Lib.BeingCount(Apt_Code, "A");
            EditViewsB = await additional_Welfare_Facility_Lib.BeingCount(Apt_Code, "B");
            if (EditViews > 0)
            {
                bnn = await apt_Detail_Lib.Detail_AptDetail(Apt_Code);
                strTitle = "수정";
            }
            else
            {
                strTitle = "새로 등록";
            }
            
            if (EditViewsA > 0)
            {
                annA = await additional_Welfare_Facility_Lib.GetList_AdditionalWelfareFacility(Apt_Code, "A");
            }
            if (EditViewsB > 0)
            {
                annB = await additional_Welfare_Facility_Lib.GetList_AdditionalWelfareFacility(Apt_Code, "B"); 
            }
        }

        /// <summary>
        /// 저장(공동주택 상세정보 입력)
        /// </summary>
        /// <returns></returns>
        private async Task btnSave()
        {
            if (bnn.Aid < 1)
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

                bnn.Apt_Code = Apt_Code;
                bnn.Apt_Detail_Code = Apt_Code;
                await apt_Detail_Lib.Add_AptDetail(bnn);
            }
            else
            {
                await apt_Detail_Lib.Edit_AptDetail(bnn);
            }

            InsertViews = "A";
            await DisplayViews();
        }

        /// <summary>
        /// 등록 닫기
        /// </summary>
        private void btnClose()
        {
            InsertViews = "A";
        }

        /// <summary>
        /// 등록 및 수정 열기
        /// </summary>
        private void btnOpen()
        {
            InsertViews = "B";
            //strTitle = "등록";
        }

        /// <summary>
        /// 부대시설 수정
        /// </summary>
        private void OnEditA(Additional_Welfare_Facility add)
        {
            FnnA = add;
            InsertViewsA = "B";
            strTitleA = "부대시설 수정";
        }

        /// <summary>
        /// 부대시설 등록 열기
        /// </summary>
        private void btnAddSaveA()
        {
            FnnA = new Additional_Welfare_Facility();
            InsertViewsA = "B";
            FnnA.Unit = "㎡";
            FnnA.Facility_Division = "A";
            strTitleA = "부대시설 등록";
            FnnA.Facility_Etc = "입력하지 않았음.";
        }

        /// <summary>
        /// 복리시설 등록 열기
        /// </summary>
        private void btnAddSaveB()
        {
            FnnA = new Additional_Welfare_Facility();
            InsertViewsA = "B";
            FnnA.Facility_Division = "B";
            FnnA.Unit = "㎡";
            strTitleA = "복리시설 등록";
            FnnA.Facility_Etc = "입력하지 않았음.";
        }

        /// <summary>
        /// 부대시설 수정 열기
        /// </summary>
        private void OnEditB(Additional_Welfare_Facility add)
        {
            FnnA = add;

            InsertViewsA = "B";
            strTitleA = "복리시설 수정";
        }

        /// <summary>
        /// 복리시설 삭제
        /// </summary>
        /// <param name="Aid"></param>
        /// <returns></returns>
        private async Task OnRemoveB(int Aid)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{Aid}번 글을 정말로 삭제하시겠습니까?");
            if (isDelete)
            {
                await additional_Welfare_Facility_Lib.Remove(Aid.ToString());
                await DisplayViews();
            }
        }

        /// <summary>
        /// 부대복리시설 등록
        /// </summary>
        /// <returns></returns>
        private async Task btnSaveA()
        {
            if (FnnA.Facility_Name == "" || FnnA.Facility_Name == null)
            {
                await JSRuntime.InvokeAsync<object>("alert", "시설명이 입력되지 않았습니다.");
            }
            else if (FnnA.Facility_Position == "" || FnnA.Facility_Position == null)
            {
                await JSRuntime.InvokeAsync<object>("alert", "시설물 위치가 입력되지 않았습니다.");
            }
            else if (FnnA.Quantity < 1)
            {
                await JSRuntime.InvokeAsync<object>("alert", "수량이 입력되지 않았습니다.");
            }
            else if (FnnA.Unit == "" || FnnA.Unit == null)
            {
                await JSRuntime.InvokeAsync<object>("alert", "단위가 선택되지 않았습니다.");
            }
            else if (FnnA.Facility_Etc == "" || FnnA.Facility_Etc == null)
            {
                await JSRuntime.InvokeAsync<object>("alert", "해당 시설물에 설명을 간단히 입력해 주세요.");
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
                FnnA.Post_IP = myIPAddress;

                #endregion 아이피 입력

                FnnA.Apt_Code = Apt_Code;
                FnnA.User_ID = User_Code;

                if (FnnA.Aid < 1)
                {
                    await additional_Welfare_Facility_Lib.Add_AdditionalWelfareFacility(FnnA);
                }
                else
                {
                    await additional_Welfare_Facility_Lib.Edit_AdditionalWelfareFacility(FnnA);
                }
                InsertViewsA = "A";
                await DisplayViews();
            }
        }

        private void btnCloseA()
        {
            InsertViewsA = "A";
        }
    }
}
