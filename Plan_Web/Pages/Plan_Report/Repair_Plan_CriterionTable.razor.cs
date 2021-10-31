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
    public partial class Repair_Plan_CriterionTable
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
        List<Join_Article_Cycle_Cost_EntityA> ann1 { get; set; } = new List<Join_Article_Cycle_Cost_EntityA>();
        List<Join_Article_Cycle_Cost_EntityA> ann2 { get; set; } = new List<Join_Article_Cycle_Cost_EntityA>();
        List<Join_Article_Cycle_Cost_EntityA> ann3 { get; set; } = new List<Join_Article_Cycle_Cost_EntityA>();
        List<Join_Article_Cycle_Cost_EntityA> ann4 { get; set; } = new List<Join_Article_Cycle_Cost_EntityA>();
        List<Join_Article_Cycle_Cost_EntityA> ann5 { get; set; } = new List<Join_Article_Cycle_Cost_EntityA>();
        List<Join_Article_Cycle_Cost_EntityA> ann6 { get; set; } = new List<Join_Article_Cycle_Cost_EntityA>();
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
            rpn = await repair_Plan_Lib.Detail_Repair_Plan(Apt_Code, strCode);
            ann1 = await cost_Lib.GetLIst_RepairCost_Sort_New(strCode, "Sort_A_Code", "3", Apt_Code);
            ann2 = await cost_Lib.GetLIst_RepairCost_Sort_New(strCode, "Sort_A_Code", "4", Apt_Code);
            ann3 = await cost_Lib.GetLIst_RepairCost_Sort_New(strCode, "Sort_A_Code", "5", Apt_Code);
            ann4 = await cost_Lib.GetLIst_RepairCost_Sort_New(strCode, "Sort_A_Code", "6", Apt_Code);
            ann5 = await cost_Lib.GetLIst_RepairCost_Sort_New(strCode, "Sort_A_Code", "7", Apt_Code);
            ann6 = await cost_Lib.GetLIst_RepairCost_Sort_New(strCode, "Sort_A_Code", "8", Apt_Code);
        }

        private void btnPrint()
        {

        }
    }
}
