using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Plan_Apt_Lib;
using Plan_Blazor_Lib.Plan;
using Plan_Lib;
using Plan_Lib.Pund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plan_Web.Pages.Plan_Report
{
    public partial class Fund_Accomulation
    {

        #region 속성
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Parameter] public string Aid { get; set; }
        [Inject] public NavigationManager MyNav { get; set; } // Url
        [Inject] private IJSRuntime JSRuntime { get; set; }
        [Inject] public IAptInfor_Lib aptInfor_Lib { get; set; }
        [Inject] public IRepair_Plan_Lib repair_Plan_Lib { get; set; }
        [Inject] public IRepair_Capital_Lib repair_Capital_Lib { get; set; }
        [Inject] public IUnit_Price_Lib unit_Price_Lib { get; set; }
        [Inject] public IBylaw_Lib bylaw_Lib { get; set; }
        [Inject] public ILevy_Rate_Lib levy_Rate_Lib { get; set; }
        [Inject] public ICapital_Levy_Lib capital_Levy_Lib { get; set; }

        private Repair_Plan_Entity rpn { get; set; } = new Repair_Plan_Entity();
        List<Capital_Levy_Entity> CL_List { get; set; } = new List<Capital_Levy_Entity>();
        private Unit_Price_string_Entity upsn { get; set; } = new Unit_Price_string_Entity();
        Levy_Rate_Entity unn { get; set; } = new Levy_Rate_Entity();
        Levy_Rate_Entity unn_A { get; set; } = new Levy_Rate_Entity();
        Unit_Price_Entity upn { get; set; } = new Unit_Price_Entity();
        private Bylaw_Entity bpn { get; set; } = new Bylaw_Entity();
        public int Bylaw_Code { get; private set; }

        private double dbYearCapital;
        private double dbAptCapital;

        Capital_Levy_Entity dnn { get; set; } = new Capital_Levy_Entity();
        Repair_Capital_Entity bnn { get; set; } = new Repair_Capital_Entity();

        private double dbTotalCapital;

        //List<Bylaw_Entity> bylawList { get; set; } = new List<Bylaw_Entity>();

        public string Apt_Code { get; set; }
        public string User_Code { get; set; }
        public string Apt_Name { get; set; }
        public string User_Name { get; set; }
        public string BuildDate { get; set; }
        public string InsertViews { get; set; } = "A";
        public string strTitle { get; set; }

        private string strCode;
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
                    await DetailsView(DateTime.Now.Year);
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

        private async Task DetailsView(int Year)
        {
            dnn.Levy_Year = Year;
            bnn = await repair_Capital_Lib._detail(Apt_Code);
            CL_List = await capital_Levy_Lib.GetList(Apt_Code, dnn.Levy_Year);

            bpn = await bylaw_Lib.Details_Bylaw(Apt_Code);
            Bylaw_Code = bpn.Bylaw_Code;
            unn = await levy_Rate_Lib.Detail_Year_Levy_Next(Apt_Code, Bylaw_Code.ToString(), dnn.Levy_Year);

            unn_A = await levy_Rate_Lib.Details_Rate_Levy_Cost_Now(Apt_Code, rpn.Repair_Plan_Code, unn.Levy_Rate_Code.ToString(), 0, 0);

            dbYearCapital = await capital_Levy_Lib.Get_Year_Sum(Apt_Code, dnn.Levy_Year);
            dbAptCapital = await capital_Levy_Lib.Get_Apt_Sum(Apt_Code, bnn.PostDate.Year.ToString());
            bnn = await repair_Capital_Lib._detail(Apt_Code);
            dbTotalCapital = dbAptCapital + bnn.Use_Cost + bnn.Balance_Capital;
        }

        /// <summary>
        /// 장기수선충당금 해당 년도별 목록
        /// </summary>
        private async Task OnByYearSelect(ChangeEventArgs a)
        {
            if (a.Value != null)
            {
                int Y = Convert.ToInt32(a.Value);
                await DetailsView(Y);
            }
        }

        /// <summary>
        /// 인쇄로 이동
        /// </summary>
        private void btnPrint()
        {

        }
    }
}
