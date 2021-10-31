using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Plan_Apt_Lib;
using Plan_Blazor_Lib;
using Plan_Blazor_Lib.Cost;
using Plan_Blazor_Lib.Cycle;
using Plan_Blazor_Lib.Plan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plan_Web.Pages.Plan_Report
{
    public partial class Year_Practice_Report
    {
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Parameter] public string Aid { get; set; }
        [Inject] public NavigationManager MyNav { get; set; } // Url
        [Inject] private IJSRuntime JSRuntime { get; set; }
        [Inject] public IAptInfor_Lib aptInfor_Lib { get; set; }
        [Inject] public IRepair_Plan_Lib repair_Plan_Lib { get; set; }
        [Inject] public IArticle_Lib article_Lib { get; set; }
        [Inject] public ICycle_Lib cycle_Lib { get; set; }
        [Inject] public ICost_Lib cost_Lib { get; set; }

        private Repair_Plan_Entity rpn { get; set; } = new Repair_Plan_Entity();
        List<Join_Article_Cycle_Cost_EntityA> ann { get; set; } = new List<Join_Article_Cycle_Cost_EntityA>();
        Repair_Plan_practice_total_Entity rpp { get; set; } = new Repair_Plan_practice_total_Entity();
        public string Apt_Code { get; private set; }
        public string User_Code { get; private set; }
        public string Apt_Name { get; private set; }
        public string User_Name { get; private set; }
        public string BuildDate { get; private set; }
        public string strCode { get; private set; }

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
                    string Year = DateTime.Now.Year.ToString();
                    string F_Year = (DateTime.Now.Year + 2).ToString();
                    await DetailsView(Year, F_Year);
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

        /// <summary>
        /// 연차별 계획 정보 불러오기
        /// </summary>
        private async Task DetailsView(string Now_Year, string Future_Year)
        {
            rpn = await repair_Plan_Lib.Detail_Repair_Plan(Apt_Code, strCode);
            ann = await repair_Plan_Lib.Year_Plan_practice(Apt_Code, strCode, Now_Year, Future_Year);            
            rpp = await repair_Plan_Lib.Year_Plan_Cost_Totay(Apt_Code, strCode, Now_Year, Future_Year);
        }

        private void btnPrint()
        {

        }
    }
}
