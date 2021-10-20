using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Plan_Apt_Lib;
using Plan_Blazor_Lib.Plan;
using Plan_Lib.Pund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plan_Web.Pages.Fund_Use_Plan
{
    public partial class Repair_Saving_State_Report
    {
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Parameter] public string Aid { get; set; }
        [Inject] public NavigationManager MyNav { get; set; } // Url
        [Inject] private IJSRuntime JSRuntime { get; set; }
        [Inject] public IAptInfor_Lib aptInfor_Lib { get; set; }
        [Inject] public IRepair_Plan_Lib repair_Plan_Lib { get; set; }
        [Inject] public IRepair_Capital_Lib repair_Capital_Lib { get; set; }
        [Inject] public IRepair_Capital_Use_Lib repair_Capital_Use_Lib { get; set; }
        [Inject] public IRepair_Saving_Using_Pund_Lib repair_Using_Saving_Fund_Lib { get; set; }

        private Repair_Plan_Entity rpn { get; set; } = new Repair_Plan_Entity();
        private List<Repair_Saving_Using_Pund_Entity> ann { get; set; } = new List<Repair_Saving_Using_Pund_Entity>();
        private Repair_Saving_Using_Pund_Entity dnn { get; set; } = new Repair_Saving_Using_Pund_Entity();

        public string Apt_Code { get; set; }
        public string User_Code { get; set; }
        public string Apt_Name { get; set; }
        public string User_Name { get; set; }
        public string BuildDate { get; set; }
        public string InsertViews { get; set; } = "A";
        public string DataViews { get; set; } = "A";
        public string strTitle { get; set; }
        public string NowYear { get; private set; }
        public string NextYear { get; private set; }
        public string strCode { get; set; }
        public string Work_Year { get; set; }


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
                BuildDate = authState.User.Claims.FirstOrDefault(c => c.Type == "BuildDate")?.Value;

                strCode = await ProtectedSessionStore.GetAsync<string>("Plan_Code");

                if (strCode != null)
                {
                    await DetailsView();
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "장기수선계획을 선택되지 않았습니다. \n 선택으로 돌아갑니다..");
                    MyNav.NavigateTo("/Repair_Plan/List");
                }
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인하지 않았습니다.");
                MyNav.NavigateTo("/");
            }
        }

        private async Task DetailsView()
        {
            dnn.Report_Year = DateTime.Now.Year;
            NowYear = dnn.Report_Year.ToString();
            NextYear = (DateTime.Now.Year + 1).ToString();
            rpn = await repair_Plan_Lib.Detail_Repair_Plan(Apt_Code, strCode);
            ann = await repair_Using_Saving_Fund_Lib.GetList(Apt_Code);
        }

        /// <summary>
        /// 새로 등록 열기
        /// </summary>
        private void btnOpen()
        {
            InsertViews = "B";
        }

        /// <summary>
        /// 장기수선충당금 사용 및 적립현황 년도별 목록
        /// </summary>
        private async Task OnByYearSelect(ChangeEventArgs a)
        {
            if (a.Value != null)
            {
                NowYear = a.Value.ToString();
                NextYear = (Convert.ToInt32(NowYear) + 1).ToString();
                dnn = await repair_Using_Saving_Fund_Lib.Detail_Year(Apt_Code, Work_Year);
                //cup = await cost_Using_Plan_Lib.GetList_Year(Apt_Code, a.Value.ToString());
                //JACC = await repair_Capital_Use_Lib.GetList_Using_Cost(Apt_Code, strCode, NowYear);
                //JACC1 = await repair_Capital_Use_Lib.GetList_Using_Cost(Apt_Code, strCode, NextYear);
            }
        }

        /// <summary>
        /// 수정열기
        /// </summary>
        /// <param name="ar"></param>
        private void OnByEdit(Repair_Saving_Using_Pund_Entity ar)
        {
            dnn = ar;
            InsertViews = "B";
        }

        /// <summary>
        /// 장기수선충당금 사용 및 적립현황
        /// </summary>
        /// <param name="Aid"></param>
        /// <returns></returns>
        private async Task OnByRemove(int Aid)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{Aid}번 글을 정말로 삭제하시겠습니까?");
            if (isDelete)
            {
                await repair_Using_Saving_Fund_Lib.Delete(Aid);
                await DetailsView();
            }
        }
    }
}
