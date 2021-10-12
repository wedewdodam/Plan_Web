using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Plan_Apt_Lib;
using Plan_Blazor_Lib.Cost;
using Plan_Blazor_Lib.Plan;
using Plan_Lib;
using Plan_Lib.Pund;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plan_Web.Pages.Appropriation_fund
{
    public partial class List
    {
        #region 속성
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
        [Inject] public IDong_Composition dong_Composition { get; set; }



        private Unit_Price_string_Entity upsn { get; set; } = new Unit_Price_string_Entity();
        Unit_Price_Entity upn { get; set; } = new Unit_Price_Entity();
        public int LevyCount { get; private set; }
        private Bylaw_Entity bpn { get; set; } = new Bylaw_Entity();
        List<Bylaw_Entity> bylawList { get; set; } = new List<Bylaw_Entity>();
        private Repair_Plan_Entity rpn { get; set; } = new Repair_Plan_Entity();
        Levy_Rate_Entity bnn1 { get; set; } = new Levy_Rate_Entity();
        Levy_Rate_Entity bnn2 { get; set; } = new Levy_Rate_Entity();
        Levy_Rate_Entity bnn3 { get; set; } = new Levy_Rate_Entity();
        Levy_Rate_Entity bnn4 { get; set; } = new Levy_Rate_Entity();
        Levy_Rate_Entity bnn5 { get; set; } = new Levy_Rate_Entity();
        Levy_Rate_Entity bnn6 { get; set; } = new Levy_Rate_Entity();
        Levy_Rate_Entity bnn7 { get; set; } = new Levy_Rate_Entity();
        Levy_Rate_Entity bnn8 { get; set; } = new Levy_Rate_Entity();
        Levy_Rate_Entity bnn9 { get; set; } = new Levy_Rate_Entity();
        Levy_Rate_Entity bnn10 { get; set; } = new Levy_Rate_Entity();
        Levy_Rate_Entity bnn11 { get; set; } = new Levy_Rate_Entity();
        Levy_Rate_Entity bnn12 { get; set; } = new Levy_Rate_Entity();
        Levy_Rate_Entity bnn13 { get; set; } = new Levy_Rate_Entity();
        Levy_Rate_Entity bnn14 { get; set; } = new Levy_Rate_Entity();
        Levy_Rate_Entity bnn15 { get; set; } = new Levy_Rate_Entity();

        private List<Levy_Rate_Entity> List_1 { get; set; } = new List<Levy_Rate_Entity>();
        private List<Levy_Rate_Entity> List_2 { get; set; } = new List<Levy_Rate_Entity>();
        private List<Levy_Rate_Entity> List_3 { get; set; } = new List<Levy_Rate_Entity>();
        private List<Levy_Rate_Entity> List_4 { get; set; } = new List<Levy_Rate_Entity>();
        private List<Levy_Rate_Entity> List_5 { get; set; } = new List<Levy_Rate_Entity>();
        private List<Levy_Rate_Entity> List_6 { get; set; } = new List<Levy_Rate_Entity>();
        private List<Levy_Rate_Entity> List_7 { get; set; } = new List<Levy_Rate_Entity>();
        private List<Levy_Rate_Entity> List_8 { get; set; } = new List<Levy_Rate_Entity>();
        private List<Levy_Rate_Entity> List_9 { get; set; } = new List<Levy_Rate_Entity>();
        private List<Levy_Rate_Entity> List_10 { get; set; } = new List<Levy_Rate_Entity>();
        private List<Levy_Rate_Entity> List_11 { get; set; } = new List<Levy_Rate_Entity>();
        private List<Levy_Rate_Entity> List_12 { get; set; } = new List<Levy_Rate_Entity>();
        private List<Levy_Rate_Entity> List_13 { get; set; } = new List<Levy_Rate_Entity>();
        private List<Levy_Rate_Entity> List_14 { get; set; } = new List<Levy_Rate_Entity>();
        private List<Levy_Rate_Entity> List_15 { get; set; } = new List<Levy_Rate_Entity>();

        //List<Levy_Rate_Entity> lrList { get; set; } = new List<Levy_Rate_Entity>();
        private Repair_Capital_Entity capital_en { get; set; } = new Repair_Capital_Entity();

        public double dbUseSum { get; private set; } //초기화 부과총액
        public double dbLevySum_Year { get; private set; }
        public double dbLevysum_Month { get; private set; }
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public string BuildDate { get; private set; }
        public string strCode { get; set; }
        public double dbUse_Cost { get; set; } = 0;
        public string InsertViewsA { get; set; } = "A";
        public string InsertViewsB { get; set; } = "A";
        public string strTitleA { get; set; }
        public double dbLevySum { get; set; } = 0;
        public double dbUseCode { get; set; } = 0;
        public double dbBalance { get; set; } = 0;
        public string strLevySum { get; set; }
        public string strUseSum { get; set; }
        public string strBalance { get; set; }
        public int Bylaw_Code { get; private set; }
        public int intLevy_Period_New { get; set; }
        public double dbLevyRateSum { get; set; } = 0;
        public int dbLevyMonthSum { get; set; } = 0;

        #endregion

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
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "장기수선계획을 선택되지 않았습니다. \n 처음으로 돌아갑니다..");
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
            bylawList = await bylaw_Lib.GetList(Apt_Code);
            rpn = await repair_Plan_Lib.Detail_Repair_Plan(Apt_Code, strCode); // 첫 장기계획 상세 정보
            bpn = await bylaw_Lib.Details_Bylaw(Apt_Code);
            Bylaw_Code = bpn.Bylaw_Code;
            upsn = await unit_Price_Lib.Report_Plan_Cost_Bylaw(Apt_Code, rpn.Repair_Plan_Code, Bylaw_Code.ToString()); //장기수선충당그 총계 정보
            upn = await unit_Price_Lib.Detail_Unit_Price_New(Apt_Code, rpn.Repair_Plan_Code, upsn.Levy_Rate, upsn.Levy_Period); //단가 관련 정보 가져오기

            LevyCount = await levy_Rate_Lib.Bylaw_Levy_Count(Apt_Code, Bylaw_Code.ToString());


            await Levy_Rate_Lib(Apt_Code, Bylaw_Code, LevyCount);


            dbUseSum = upsn.balanceSum + upsn.Using_Cost_Sum_All;
            dbLevySum_Year = upsn.supply_total_Area_All * upsn.Unit_Price_All * 12;
            dbLevysum_Month = upsn.supply_total_Area_All * upsn.Unit_Price_All;
            dbLevyMonthSum = await levy_Rate_Lib.Levy_Period_Total_New(Apt_Code, Bylaw_Code);
            dbLevyRateSum = await levy_Rate_Lib.Levy_Rate_Accumulate(Apt_Code, Bylaw_Code);
        }


        // 누적적립율에 따른 년도별 누적적립율 리스트 만들기 메서드
        private async Task Levy_Rate_Lib(string strApt_Code, int intBylaw_Code, int i)
        {
            //try
            //{
            if (i == 1)
            {
                string Levy_Rate_Code = await levy_Rate_Lib.Levy_Rate_Code_Search(strApt_Code, intBylaw_Code.ToString(), "1");
                bnn1 = await levy_Rate_Lib.Detail_Levy_Rate_Code(Levy_Rate_Code);
                List_1 = await levy_Rate_Lib.GetList_Rate_Levy_Cost_New(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code.ToString(), bnn1.Levy_Rate, bnn1.Levy_Period_New);
            }
            else if (i == 2)
            {
                string Levy_Rate_Code1 = await levy_Rate_Lib.Levy_Rate_Code_Search(strApt_Code, intBylaw_Code.ToString(), "1");
                bnn1 = await levy_Rate_Lib.Details_Rate_Levy_Cost_Now(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code1.ToString(), 0, 0);
                List_1 = await levy_Rate_Lib.GetList_Rate_Levy_Cost_New(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code1.ToString(), bnn1.Levy_Rate, bnn1.Levy_Period_New);

                string Levy_Rate_Code2 = await levy_Rate_Lib.Levy_Rate_Code_Search(strApt_Code, intBylaw_Code.ToString(), "2");
                bnn2 = await levy_Rate_Lib.Details_Rate_Levy_Cost_Now(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code2.ToString(), 0, 0);
                List_2 = await levy_Rate_Lib.GetList_Rate_Levy_Cost_New(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code2.ToString(), bnn2.Levy_Rate, bnn2.Levy_Period_New);
            }
            else if (i == 3)
            {
                string Levy_Rate_Code1 = await levy_Rate_Lib.Levy_Rate_Code_Search(strApt_Code, intBylaw_Code.ToString(), "1");
                bnn1 = await levy_Rate_Lib.Details_Rate_Levy_Cost_Now(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code1.ToString(), 0, 0);
                List_1 = await levy_Rate_Lib.GetList_Rate_Levy_Cost_New(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code1.ToString(), bnn1.Levy_Rate, bnn1.Levy_Period_New);

                string Levy_Rate_Code2 = await levy_Rate_Lib.Levy_Rate_Code_Search(strApt_Code, intBylaw_Code.ToString(), "2");
                bnn2 = await levy_Rate_Lib.Details_Rate_Levy_Cost_Now(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code2.ToString(), 0, 0);
                List_2 = await levy_Rate_Lib.GetList_Rate_Levy_Cost_New(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code2.ToString(), bnn2.Levy_Rate, bnn2.Levy_Period_New);

                string Levy_Rate_Code3 = await levy_Rate_Lib.Levy_Rate_Code_Search(strApt_Code, intBylaw_Code.ToString(), "3");
                bnn3 = await levy_Rate_Lib.Details_Rate_Levy_Cost_Now(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code3.ToString(), 0, 0);
                List_3 = await levy_Rate_Lib.GetList_Rate_Levy_Cost_New(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code3.ToString(), bnn3.Levy_Rate, bnn3.Levy_Period_New);
            }
            else if (i == 4)
            {
                string Levy_Rate_Code1 = await levy_Rate_Lib.Levy_Rate_Code_Search(strApt_Code, intBylaw_Code.ToString(), "1");
                bnn1 = await levy_Rate_Lib.Details_Rate_Levy_Cost_Now(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code1.ToString(), 0, 0);
                List_1 = await levy_Rate_Lib.GetList_Rate_Levy_Cost_New(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code1.ToString(), bnn1.Levy_Rate, bnn1.Levy_Period_New);

                string Levy_Rate_Code2 = await levy_Rate_Lib.Levy_Rate_Code_Search(strApt_Code, intBylaw_Code.ToString(), "2");
                bnn2 = await levy_Rate_Lib.Details_Rate_Levy_Cost_Now(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code2.ToString(), 0, 0);
                List_2 = await levy_Rate_Lib.GetList_Rate_Levy_Cost_New(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code2.ToString(), bnn2.Levy_Rate, bnn2.Levy_Period_New);

                string Levy_Rate_Code3 = await levy_Rate_Lib.Levy_Rate_Code_Search(strApt_Code, intBylaw_Code.ToString(), "3");
                bnn3 = await levy_Rate_Lib.Details_Rate_Levy_Cost_Now(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code3.ToString(), 0, 0);
                List_3 = await levy_Rate_Lib.GetList_Rate_Levy_Cost_New(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code3.ToString(), bnn3.Levy_Rate, bnn3.Levy_Period_New);

                string Levy_Rate_Code4 = await levy_Rate_Lib.Levy_Rate_Code_Search(strApt_Code, intBylaw_Code.ToString(), "4");
                bnn4 = await levy_Rate_Lib.Details_Rate_Levy_Cost_Now(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code4.ToString(), 0, 0);
                List_4 = await levy_Rate_Lib.GetList_Rate_Levy_Cost_New(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code4.ToString(), bnn4.Levy_Rate, bnn4.Levy_Period_New);

            }
            else if (i == 5)
            {
                string Levy_Rate_Code1 = await levy_Rate_Lib.Levy_Rate_Code_Search(strApt_Code, intBylaw_Code.ToString(), "1");
                bnn1 = await levy_Rate_Lib.Details_Rate_Levy_Cost_Now(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code1.ToString(), 0, 0);
                List_1 = await levy_Rate_Lib.GetList_Rate_Levy_Cost_New(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code1.ToString(), bnn1.Levy_Rate, bnn1.Levy_Period_New);

                string Levy_Rate_Code2 = await levy_Rate_Lib.Levy_Rate_Code_Search(strApt_Code, intBylaw_Code.ToString(), "2");
                bnn2 = await levy_Rate_Lib.Details_Rate_Levy_Cost_Now(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code2.ToString(), 0, 0);
                List_2 = await levy_Rate_Lib.GetList_Rate_Levy_Cost_New(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code2.ToString(), bnn2.Levy_Rate, bnn2.Levy_Period_New);

                string Levy_Rate_Code3 = await levy_Rate_Lib.Levy_Rate_Code_Search(strApt_Code, intBylaw_Code.ToString(), "3");
                bnn3 = await levy_Rate_Lib.Details_Rate_Levy_Cost_Now(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code3.ToString(), 0, 0);
                List_3 = await levy_Rate_Lib.GetList_Rate_Levy_Cost_New(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code3.ToString(), bnn3.Levy_Rate, bnn3.Levy_Period_New);

                string Levy_Rate_Code4 = await levy_Rate_Lib.Levy_Rate_Code_Search(strApt_Code, intBylaw_Code.ToString(), "4");
                bnn4 = await levy_Rate_Lib.Details_Rate_Levy_Cost_Now(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code4.ToString(), 0, 0);
                List_4 = await levy_Rate_Lib.GetList_Rate_Levy_Cost_New(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code4.ToString(), bnn4.Levy_Rate, bnn4.Levy_Period_New);

                string Levy_Rate_Code5 = await levy_Rate_Lib.Levy_Rate_Code_Search(strApt_Code, intBylaw_Code.ToString(), "5");
                bnn5 = await levy_Rate_Lib.Details_Rate_Levy_Cost_Now(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code5.ToString(), 0, 0);
                List_5 = await levy_Rate_Lib.GetList_Rate_Levy_Cost_New(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code5.ToString(), bnn5.Levy_Rate, bnn5.Levy_Period_New);
            }
            else if (i == 6)
            {
                string Levy_Rate_Code1 = await levy_Rate_Lib.Levy_Rate_Code_Search(strApt_Code, intBylaw_Code.ToString(), "1");
                bnn1 = await levy_Rate_Lib.Details_Rate_Levy_Cost_Now(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code1.ToString(), 0, 0);
                List_1 = await levy_Rate_Lib.GetList_Rate_Levy_Cost_New(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code1.ToString(), bnn1.Levy_Rate, bnn1.Levy_Period_New);

                string Levy_Rate_Code2 = await levy_Rate_Lib.Levy_Rate_Code_Search(strApt_Code, intBylaw_Code.ToString(), "2");
                bnn2 = await levy_Rate_Lib.Details_Rate_Levy_Cost_Now(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code2.ToString(), 0, 0);
                List_2 = await levy_Rate_Lib.GetList_Rate_Levy_Cost_New(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code2.ToString(), bnn2.Levy_Rate, bnn2.Levy_Period_New);

                string Levy_Rate_Code3 = await levy_Rate_Lib.Levy_Rate_Code_Search(strApt_Code, intBylaw_Code.ToString(), "3");
                bnn3 = await levy_Rate_Lib.Details_Rate_Levy_Cost_Now(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code3.ToString(), 0, 0);
                List_3 = await levy_Rate_Lib.GetList_Rate_Levy_Cost_New(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code3.ToString(), bnn3.Levy_Rate, bnn3.Levy_Period_New);

                string Levy_Rate_Code4 = await levy_Rate_Lib.Levy_Rate_Code_Search(strApt_Code, intBylaw_Code.ToString(), "4");
                bnn4 = await levy_Rate_Lib.Details_Rate_Levy_Cost_Now(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code4.ToString(), 0, 0);
                List_4 = await levy_Rate_Lib.GetList_Rate_Levy_Cost_New(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code4.ToString(), bnn4.Levy_Rate, bnn4.Levy_Period_New);

                string Levy_Rate_Code5 = await levy_Rate_Lib.Levy_Rate_Code_Search(strApt_Code, intBylaw_Code.ToString(), "5");
                bnn5 = await levy_Rate_Lib.Details_Rate_Levy_Cost_Now(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code5.ToString(), 0, 0);
                List_5 = await levy_Rate_Lib.GetList_Rate_Levy_Cost_New(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code5.ToString(), bnn5.Levy_Rate, bnn5.Levy_Period_New);

                string Levy_Rate_Code6 = await levy_Rate_Lib.Levy_Rate_Code_Search(strApt_Code, intBylaw_Code.ToString(), "6");
                bnn6 = await levy_Rate_Lib.Details_Rate_Levy_Cost_Now(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code6.ToString(), 0, 0);
                List_6 = await levy_Rate_Lib.GetList_Rate_Levy_Cost_New(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code6.ToString(), bnn6.Levy_Rate, bnn6.Levy_Period_New);
            }
            else if (i == 7)
            {
                string Levy_Rate_Code1 = await levy_Rate_Lib.Levy_Rate_Code_Search(strApt_Code, intBylaw_Code.ToString(), "1");
                bnn1 = await levy_Rate_Lib.Details_Rate_Levy_Cost_Now(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code1.ToString(), 0, 0);
                List_1 = await levy_Rate_Lib.GetList_Rate_Levy_Cost_New(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code1.ToString(), bnn1.Levy_Rate, bnn1.Levy_Period_New);

                string Levy_Rate_Code2 = await levy_Rate_Lib.Levy_Rate_Code_Search(strApt_Code, intBylaw_Code.ToString(), "2");
                bnn2 = await levy_Rate_Lib.Details_Rate_Levy_Cost_Now(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code2.ToString(), 0, 0);
                List_2 = await levy_Rate_Lib.GetList_Rate_Levy_Cost_New(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code2.ToString(), bnn2.Levy_Rate, bnn2.Levy_Period_New);

                string Levy_Rate_Code3 = await levy_Rate_Lib.Levy_Rate_Code_Search(strApt_Code, intBylaw_Code.ToString(), "3");
                bnn3 = await levy_Rate_Lib.Details_Rate_Levy_Cost_Now(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code3.ToString(), 0, 0);
                List_3 = await levy_Rate_Lib.GetList_Rate_Levy_Cost_New(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code3.ToString(), bnn3.Levy_Rate, bnn3.Levy_Period_New);

                string Levy_Rate_Code4 = await levy_Rate_Lib.Levy_Rate_Code_Search(strApt_Code, intBylaw_Code.ToString(), "4");
                bnn4 = await levy_Rate_Lib.Details_Rate_Levy_Cost_Now(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code4.ToString(), 0, 0);
                List_4 = await levy_Rate_Lib.GetList_Rate_Levy_Cost_New(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code4.ToString(), bnn4.Levy_Rate, bnn4.Levy_Period_New);

                string Levy_Rate_Code5 = await levy_Rate_Lib.Levy_Rate_Code_Search(strApt_Code, intBylaw_Code.ToString(), "5");
                bnn5 = await levy_Rate_Lib.Details_Rate_Levy_Cost_Now(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code5.ToString(), 0, 0);
                List_5 = await levy_Rate_Lib.GetList_Rate_Levy_Cost_New(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code5.ToString(), bnn5.Levy_Rate, bnn5.Levy_Period_New);

                string Levy_Rate_Code6 = await levy_Rate_Lib.Levy_Rate_Code_Search(strApt_Code, intBylaw_Code.ToString(), "6");
                bnn6 = await levy_Rate_Lib.Details_Rate_Levy_Cost_Now(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code6.ToString(), 0, 0);
                List_6 = await levy_Rate_Lib.GetList_Rate_Levy_Cost_New(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code6.ToString(), bnn6.Levy_Rate, bnn6.Levy_Period_New);

                string Levy_Rate_Code7 = await levy_Rate_Lib.Levy_Rate_Code_Search(strApt_Code, intBylaw_Code.ToString(), "7");
                bnn7 = await levy_Rate_Lib.Details_Rate_Levy_Cost_Now(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code7.ToString(), 0, 0);
                List_7 = await levy_Rate_Lib.GetList_Rate_Levy_Cost_New(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code7.ToString(), bnn7.Levy_Rate, bnn7.Levy_Period_New);
            }
            else if (i == 8)
            {
                string Levy_Rate_Code1 = await levy_Rate_Lib.Levy_Rate_Code_Search(strApt_Code, intBylaw_Code.ToString(), "1");
                bnn1 = await levy_Rate_Lib.Details_Rate_Levy_Cost_Now(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code1.ToString(), 0, 0);
                List_1 = await levy_Rate_Lib.GetList_Rate_Levy_Cost_New(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code1.ToString(), bnn1.Levy_Rate, bnn1.Levy_Period_New);

                string Levy_Rate_Code2 = await levy_Rate_Lib.Levy_Rate_Code_Search(strApt_Code, intBylaw_Code.ToString(), "2");
                bnn2 = await levy_Rate_Lib.Details_Rate_Levy_Cost_Now(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code2.ToString(), 0, 0);
                List_2 = await levy_Rate_Lib.GetList_Rate_Levy_Cost_New(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code2.ToString(), bnn2.Levy_Rate, bnn2.Levy_Period_New);

                string Levy_Rate_Code3 = await levy_Rate_Lib.Levy_Rate_Code_Search(strApt_Code, intBylaw_Code.ToString(), "3");
                bnn3 = await levy_Rate_Lib.Details_Rate_Levy_Cost_Now(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code3.ToString(), 0, 0);
                List_3 = await levy_Rate_Lib.GetList_Rate_Levy_Cost_New(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code3.ToString(), bnn3.Levy_Rate, bnn3.Levy_Period_New);

                string Levy_Rate_Code4 = await levy_Rate_Lib.Levy_Rate_Code_Search(strApt_Code, intBylaw_Code.ToString(), "4");
                bnn4 = await levy_Rate_Lib.Details_Rate_Levy_Cost_Now(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code4.ToString(), 0, 0);
                List_4 = await levy_Rate_Lib.GetList_Rate_Levy_Cost_New(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code4.ToString(), bnn4.Levy_Rate, bnn4.Levy_Period_New);

                string Levy_Rate_Code5 = await levy_Rate_Lib.Levy_Rate_Code_Search(strApt_Code, intBylaw_Code.ToString(), "5");
                bnn5 = await levy_Rate_Lib.Details_Rate_Levy_Cost_Now(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code5.ToString(), 0, 0);
                List_5 = await levy_Rate_Lib.GetList_Rate_Levy_Cost_New(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code5.ToString(), bnn5.Levy_Rate, bnn5.Levy_Period_New);

                string Levy_Rate_Code6 = await levy_Rate_Lib.Levy_Rate_Code_Search(strApt_Code, intBylaw_Code.ToString(), "6");
                bnn6 = await levy_Rate_Lib.Details_Rate_Levy_Cost_Now(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code6.ToString(), 0, 0);
                List_6 = await levy_Rate_Lib.GetList_Rate_Levy_Cost_New(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code6.ToString(), bnn6.Levy_Rate, bnn6.Levy_Period_New);

                string Levy_Rate_Code7 = await levy_Rate_Lib.Levy_Rate_Code_Search(strApt_Code, intBylaw_Code.ToString(), "7");
                bnn7 = await levy_Rate_Lib.Details_Rate_Levy_Cost_Now(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code7.ToString(), 0, 0);
                List_7 = await levy_Rate_Lib.GetList_Rate_Levy_Cost_New(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code7.ToString(), bnn7.Levy_Rate, bnn7.Levy_Period_New);

                string Levy_Rate_Code8 = await levy_Rate_Lib.Levy_Rate_Code_Search(strApt_Code, intBylaw_Code.ToString(), "8");
                bnn8 = await levy_Rate_Lib.Details_Rate_Levy_Cost_Now(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code8.ToString(), 0, 0);
                List_8 = await levy_Rate_Lib.GetList_Rate_Levy_Cost_New(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code8.ToString(), bnn8.Levy_Rate, bnn8.Levy_Period_New);
            }
            else if (i == 9)
            {
                string Levy_Rate_Code1 = await levy_Rate_Lib.Levy_Rate_Code_Search(strApt_Code, intBylaw_Code.ToString(), "1");
                bnn1 = await levy_Rate_Lib.Details_Rate_Levy_Cost_Now(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code1.ToString(), 0, 0);
                List_1 = await levy_Rate_Lib.GetList_Rate_Levy_Cost_New(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code1.ToString(), bnn1.Levy_Rate, bnn1.Levy_Period_New);

                string Levy_Rate_Code2 = await levy_Rate_Lib.Levy_Rate_Code_Search(strApt_Code, intBylaw_Code.ToString(), "2");
                bnn2 = await levy_Rate_Lib.Details_Rate_Levy_Cost_Now(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code2.ToString(), 0, 0);
                List_2 = await levy_Rate_Lib.GetList_Rate_Levy_Cost_New(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code2.ToString(), bnn2.Levy_Rate, bnn2.Levy_Period_New);

                string Levy_Rate_Code3 = await levy_Rate_Lib.Levy_Rate_Code_Search(strApt_Code, intBylaw_Code.ToString(), "3");
                bnn3 = await levy_Rate_Lib.Details_Rate_Levy_Cost_Now(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code3.ToString(), 0, 0);
                List_3 = await levy_Rate_Lib.GetList_Rate_Levy_Cost_New(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code3.ToString(), bnn3.Levy_Rate, bnn3.Levy_Period_New);

                string Levy_Rate_Code4 = await levy_Rate_Lib.Levy_Rate_Code_Search(strApt_Code, intBylaw_Code.ToString(), "4");
                bnn4 = await levy_Rate_Lib.Details_Rate_Levy_Cost_Now(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code4.ToString(), 0, 0);
                List_4 = await levy_Rate_Lib.GetList_Rate_Levy_Cost_New(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code4.ToString(), bnn4.Levy_Rate, bnn4.Levy_Period_New);

                string Levy_Rate_Code5 = await levy_Rate_Lib.Levy_Rate_Code_Search(strApt_Code, intBylaw_Code.ToString(), "5");
                bnn5 = await levy_Rate_Lib.Details_Rate_Levy_Cost_Now(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code5.ToString(), 0, 0);
                List_5 = await levy_Rate_Lib.GetList_Rate_Levy_Cost_New(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code5.ToString(), bnn5.Levy_Rate, bnn5.Levy_Period_New);

                string Levy_Rate_Code6 = await levy_Rate_Lib.Levy_Rate_Code_Search(strApt_Code, intBylaw_Code.ToString(), "6");
                bnn6 = await levy_Rate_Lib.Details_Rate_Levy_Cost_Now(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code6.ToString(), 0, 0);
                List_6 = await levy_Rate_Lib.GetList_Rate_Levy_Cost_New(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code6.ToString(), bnn6.Levy_Rate, bnn6.Levy_Period_New);

                string Levy_Rate_Code7 = await levy_Rate_Lib.Levy_Rate_Code_Search(strApt_Code, intBylaw_Code.ToString(), "7");
                bnn7 = await levy_Rate_Lib.Details_Rate_Levy_Cost_Now(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code7.ToString(), 0, 0);
                List_7 = await levy_Rate_Lib.GetList_Rate_Levy_Cost_New(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code7.ToString(), bnn7.Levy_Rate, bnn7.Levy_Period_New);

                string Levy_Rate_Code8 = await levy_Rate_Lib.Levy_Rate_Code_Search(strApt_Code, intBylaw_Code.ToString(), "8");
                bnn8 = await levy_Rate_Lib.Details_Rate_Levy_Cost_Now(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code8.ToString(), 0, 0);
                List_8 = await levy_Rate_Lib.GetList_Rate_Levy_Cost_New(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code8.ToString(), bnn8.Levy_Rate, bnn8.Levy_Period_New);

                string Levy_Rate_Code9 = await levy_Rate_Lib.Levy_Rate_Code_Search(strApt_Code, intBylaw_Code.ToString(), "9");
                bnn9 = await levy_Rate_Lib.Details_Rate_Levy_Cost_Now(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code9.ToString(), 0, 0);
                List_9 = await levy_Rate_Lib.GetList_Rate_Levy_Cost_New(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code9.ToString(), bnn9.Levy_Rate, bnn9.Levy_Period_New);
            }
            else if (i == 10)
            {

            }
            else if (i == 11)
            {

            }
            else if (i == 12)
            {

            }
            else if (i == 13)
            {

            }
            else if (i == 14)
            {

            }
            else if (i == 15)
            {

            }
            else if (i == 16)
            {

            }
            else if (i == 17)
            {

            }
            else if (i == 18)
            {

            }
            else if (i == 19)
            {

            }
            else
            {

            }
            //}
            //catch (Exception)
            //{

            //}
        }

        /// <summary>
        /// 요율별 정보 목록 만들기
        /// </summary>
        /// <param name="Levy_Rate_Code">요율 식별코드</param>
        /// <param name="Levy_Rate">요율</param>
        /// <param name="Levy_Period">요율 적용기간</param>
        private List<Levy_Rate_Entity> Levy_Rate(int Levy_Rate_Code, double Levy_Rate, int Levy_Period)
        {
            return levy_Rate_Lib.GetList_Rate_Levy_Cost_A(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate_Code.ToString(), Levy_Rate, Levy_Period);
        }

        private Unit_Price_Entity Rate_Infor(double Levy_Rate, int Levy_Period)
        {
            return unit_Price_Lib.Detail_Unit_Price_Up(Apt_Code, rpn.Repair_Plan_Code, Levy_Rate, Levy_Period);
        }
    }
}
