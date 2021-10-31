using Facility;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Plan_Apt_Lib;
using Plan_Blazor_Lib.Cost;
using Plan_Blazor_Lib.Plan;
using Plan_Blazor_Lib.Record;
using Plan_Lib;
using Plan_Lib.Pund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plan_Web.Pages.Plan_Report
{
    public partial class Repair_Plan_GeneralTable
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
        [Inject] public ILevy_Rate_Lib levy_Rate_Lib { get; set; }
        [Inject] public IRepair_Capital_Use_Lib repair_Capital_Use_Lib { get; set; }
        [Inject] public IRepair_Saving_Using_Pund_Lib repair_Using_Saving_Fund_Lib { get; set; }
        [Inject] public IRepair_Record_Lib repair_Record_Lib { get; set; }
        [Inject] public ICapital_Levy_Lib capital_Levy_Lib { get; set; }
        [Inject] public IApt_Detail_Lib apt_Detail_Lib { get; set; }
        [Inject] public IDong_Lib dong_Lib { get; set; }
        [Inject] public IDong_Composition dong_Composition { get; set; }
        [Inject] public IAdditional_Welfare_Facility_Lib additional_Welfare_Facility_Lib { get; set; }
        [Inject] public IRelation_Law_Lib relation_Law_Lib { get; set; }
        [Inject] public ISmallSum_Repuirement_Selection_Lib smallSum_Repuirement_Selection_Lib { get; set; }
        [Inject] public IRepair_Object_Selection_Lib repair_Object_Selection_Lib { get; set; }

        private Repair_Plan_Entity rpn { get; set; } = new Repair_Plan_Entity();
        Apt_Detail_Entity ann { get; set; } = new Apt_Detail_Entity();
        AptInfor_Entity bnn { get; set; } = new AptInfor_Entity();
        Repair_Plan_Entity cnn { get; set; } = new Repair_Plan_Entity();
        List<Additional_Welfare_Facility> awListA { get; set; } = new List<Additional_Welfare_Facility>();
        List<Additional_Welfare_Facility> awListB { get; set; } = new List<Additional_Welfare_Facility>();
        List<Dong_Entity> doList { get; set; } = new List<Dong_Entity>();
        List<Dong_Composition_Entity> dcList { get; set; } = new List<Dong_Composition_Entity>();
        private Useing_Saving_Report_Entity usr { get; set; } = new Useing_Saving_Report_Entity();
        private Unit_Price_string_Entity upsn { get; set; } = new Unit_Price_string_Entity();
        Unit_Price_Entity upn { get; set; } = new Unit_Price_Entity();
        private Bylaw_Entity bpn { get; set; } = new Bylaw_Entity();
        List<Relation_Law_Entity> rll { get; set; } = new List<Relation_Law_Entity>();
        private List<Repair_SmallSum_Object_Selection_Entity> sose = new List<Repair_SmallSum_Object_Selection_Entity>();
        private List<Repair_SmallSum_Requirement_Selection_Entity> srse = new List<Repair_SmallSum_Requirement_Selection_Entity>();


        public string Apt_Code { get; private set; }
        public string User_Code { get; private set; }
        public string Apt_Name { get; private set; }
        public string User_Name { get; private set; }
        public string BuildDate { get; private set; }
        public string Work_Year { get; private set; }
        private string strCode { get; set; }
        public string AptInfor { get; set; } = "A";
        public string DongInfor { get; set; } = "A";
        public string LawInfor { get; set; } = "A";
        public string PlanInfor { get; set; } = "A";

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
                    Work_Year = DateTime.Now.Year.ToString();
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
            bpn = await bylaw_Lib.Details_Bylaw(Apt_Code);
            upsn = await unit_Price_Lib.Report_Plan_Cost_Bylaw(Apt_Code, rpn.Repair_Plan_Code, bpn.Bylaw_Code.ToString()); //장기수선충당 총계 정보
            upn = await unit_Price_Lib.Detail_Unit_Price_New(Apt_Code, rpn.Repair_Plan_Code, upsn.Levy_Rate, upsn.Levy_Period); //단가 관련 정보 가져오기
            awListA = await additional_Welfare_Facility_Lib.GetList_AdditionalWelfareFacility(Apt_Code, "A");
            awListB = await additional_Welfare_Facility_Lib.GetList_AdditionalWelfareFacility(Apt_Code, "B");
            ann = await apt_Detail_Lib.Detail_AptDetail(Apt_Code);
            bnn = await aptInfor_Lib.Detail_Apt(Apt_Code);
            doList = await dong_Lib.GetList_Dong(Apt_Code);
            dcList = await dong_Composition.GetList_Dong_Composition(Apt_Code);
            rll = await relation_Law_Lib.GetList("B10", "B10");
            srse = await smallSum_Repuirement_Selection_Lib.GetList_RSRS(rpn.Repair_Plan_Code);
            sose = await repair_Object_Selection_Lib.GetList_RSOS(rpn.Repair_Plan_Code, "A");
        }

        private void btnFoundation()
        {
            AptInfor = "A";
            DongInfor = "B";
            LawInfor = "B";
            PlanInfor = "B";
        }

        private void btnDongInfor()
        {
            AptInfor = "B";
            DongInfor = "A";
            LawInfor = "B";
            PlanInfor = "B";
        }

        private void btnLawInfor()
        {
            AptInfor = "B";
            DongInfor = "B";
            LawInfor = "A";
            PlanInfor = "B";
        }

        private void btnPlanInfor()
        {
            AptInfor = "B";
            DongInfor = "B";
            LawInfor = "B";
            PlanInfor = "A";
        }
    }
}
