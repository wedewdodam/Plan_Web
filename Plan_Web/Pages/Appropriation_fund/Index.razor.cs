using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Plan_Apt_Lib;
using Plan_Blazor_Lib.Cost;
using Plan_Blazor_Lib.Plan;
using Plan_Lib;
using Plan_Lib.Pund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using BlazorSessionExample.Data

namespace Plan_Web.Pages.Appropriation_fund
{
    public partial class Index
    {
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Parameter] public string Aid { get; set; }
        [Inject] public NavigationManager MyNav { get; set; } // Url
        [Inject] private IJSRuntime JSRuntime { get; set; }
        [Inject] public IAptInfor_Lib aptInfor_Lib { get; set; }
        [Inject] public IRepair_Plan_Lib repair_Plan_Lib { get; set; }
        [Inject] public ICost_Lib cost_Lib { get; set; }
        [Inject] public IRepair_Capital_Lib repair_Capital_Lib { get; set; }
        [Inject] public IUnit_Price_Lib unit_Price_Lib { get; set; }
        [Inject] public IBylaw_Lib bylaw_Lib { get; set; }
        //public Blazored.LocalStorage.ILocalStorageService session;

        private Unit_Price_string_Entity upsn { get; set; } = new Unit_Price_string_Entity();
        Unit_Price_Entity upn { get; set; } = new Unit_Price_Entity();
        private Bylaw_Entity bpn { get; set; } = new Bylaw_Entity();
        List<Bylaw_Entity> bylawList { get; set; } = new List<Bylaw_Entity>();
        private Repair_Plan_Entity rpn { get; set; } = new Repair_Plan_Entity();
        Levy_Rate_Entity bnn { get; set; } = new Levy_Rate_Entity();

        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public string BuildDate { get; private set; }
        public string strCode { get; set; }

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

                //int Plan_Being = await repair_Plan_Lib.BeComplete_Count(Apt_Code);

                strCode = await session.GetItemAsync<string>("Plan_Code");

                if (strCode != null)
                {
                    await DetailsView();
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "식별코드가 선택되지 않았습니다. \n 처음으로 돌아갑니다..");
                    MyNav.NavigateTo("/");
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
            //strCode = await session.GetItemAsync<string>("Plan_Code");
            bylawList = await bylaw_Lib.GetList(Apt_Code);
            rpn = await repair_Plan_Lib.Detail_Repair_Plan(Apt_Code, strCode); // 첫 장기계획 상세 정보
            int Bylaw_Code = await bylaw_Lib.Bylaw_Last_Code(Apt_Code);
            upsn = await unit_Price_Lib.Report_Plan_Cost_Bylaw(Apt_Code, rpn.Repair_Plan_Code, Bylaw_Code.ToString());
            upn = await unit_Price_Lib.Detail_Unit_Price_New(Apt_Code, rpn.Repair_Plan_Code, upsn.Levy_Rate, upsn.Levy_Period); //단가 관련 정보 가져오기
        }

        private void btnFundOpen()
        {

        }

        private void btnRateOpen()
        {

        }
    }
}
