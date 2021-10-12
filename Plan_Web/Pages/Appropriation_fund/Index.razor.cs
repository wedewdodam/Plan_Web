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
        private Bylaw_Entity bpn { get; set; } = new Bylaw_Entity();
        List<Bylaw_Entity> bylawList { get; set; } = new List<Bylaw_Entity>();
        private Repair_Plan_Entity rpn { get; set; } = new Repair_Plan_Entity();
        Levy_Rate_Entity bnn { get; set; } = new Levy_Rate_Entity();
        private List<Dong_Composition_Entity> dong_ce { get; set; } = new List<Dong_Composition_Entity>();
        List<Levy_Rate_Entity> lrList { get; set; } = new List<Levy_Rate_Entity>();
        private Repair_Capital_Entity capital_en { get; set; } = new Repair_Capital_Entity();

        public int MonthSum { get; set; }
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

                //int Plan_Being = await repair_Plan_Lib.BeComplete_Count(Apt_Code);

                strCode = await ProtectedSessionStore.GetAsync<string>("Plan_Code");
                //strCode = await session.GetItem<string>("Plan_Code");

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
            upsn = await unit_Price_Lib.Report_Plan_Cost_Bylaw(Apt_Code, rpn.Repair_Plan_Code, Bylaw_Code.ToString()); //장기수선충당 총계 정보
            upn = await unit_Price_Lib.Detail_Unit_Price_New(Apt_Code, rpn.Repair_Plan_Code, upsn.Levy_Rate, upsn.Levy_Period); //단가 관련 정보 가져오기
            //dbUse_Cost = upsn.Balance_sum_Adjust_All;
            lrList = await levy_Rate_Lib.GetList_Levy_Rate(Apt_Code, Bylaw_Code);//해당 관리규약 코드로 적립요율 정보 불러오기

            dong_ce = await dong_Composition.GetList_Dong_Composition(Apt_Code);
            MonthSum = await levy_Rate_Lib.Levy_Month(Apt_Code, Bylaw_Code);//적립율 정보에 의한 총 개월 수
            capital_en = await repair_Capital_Lib._detail(Apt_Code);
            
            dbUseSum = upsn.balanceSum + upsn.Using_Cost_Sum_All;
            dbLevySum_Year = upsn.supply_total_Area_All * upsn.Unit_Price_All * 12;
            dbLevysum_Month = upsn.supply_total_Area_All * upsn.Unit_Price_All;
            dbLevyMonthSum = await levy_Rate_Lib.Levy_Period_Total_New(Apt_Code, Bylaw_Code);
            dbLevyRateSum = await levy_Rate_Lib.Levy_Rate_Accumulate(Apt_Code, Bylaw_Code);
        }

        /// <summary>
        /// 관리규약 선택 실행
        /// </summary>
        private async Task OnBylawSelect(ChangeEventArgs a)
        {
            Bylaw_Code = Convert.ToInt32(a.Value);
            lrList = await levy_Rate_Lib.GetList_Levy_Rate(Apt_Code, Bylaw_Code);//해당 관리규약 코드로 적립요율 정보 불러오기
        }

        /// <summary>
        /// 장기수선충당금 초기화 열기
        /// </summary>
        private void btnFundOpen()
        {
            InsertViewsA = "B";            
            strTitleA = "장기수선충당금 초기화";
            strLevySum = string.Format("{0: ###,###}", capital_en.Levy_Capital);
            dbLevySum = capital_en.Levy_Capital;
            strUseSum = string.Format("{0: ###,###}", capital_en.Use_Cost);
            dbUse_Cost = capital_en.Use_Cost;
            strBalance = string.Format("{0: ###,###}", capital_en.Balance_Capital);
            dbBalance = capital_en.Balance_Capital;
        }

        /// <summary>
        /// 사용액 입력 시행
        /// </summary>
        private void OnUseCost(ChangeEventArgs a)
        {
            dbUse_Cost = Convert.ToDouble(a.Value);
            capital_en.Use_Cost = dbUse_Cost;
            strUseSum = string.Format("{0: ###,###}", dbUse_Cost);
            dbLevySum = dbUse_Cost + dbBalance;
            strLevySum = string.Format("{0: ###,###}", dbLevySum);
            capital_en.Levy_Capital = dbLevySum;
        }

        /// <summary>
        /// 사용액 입력 시행
        /// </summary>
        private void OnBalance(ChangeEventArgs a)
        {
            dbBalance = Convert.ToDouble(a.Value);
            capital_en.Balance_Capital = dbBalance;
            strBalance = string.Format("{0: ###,###}", dbBalance);
            dbLevySum = dbUse_Cost + dbBalance;
            strLevySum = string.Format("{0: ###,###}", dbLevySum);
            capital_en.Levy_Capital = dbLevySum;
        }

        /// <summary>
        /// 장기수선충당금 초기화 저장
        /// </summary>
        private async Task btnSaveA()
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
            capital_en.PostIP = myIPAddress;
            #endregion 아이피 입력
            capital_en.Apt_Code = Apt_Code;
            capital_en.Staff_Code = User_Code;

            if (capital_en.Balance_Capital < 1)
            {
                await JSRuntime.InvokeAsync<object>("alert", "잔액을 입력되지 않았습니다.");
            }
            else if (capital_en.Levy_Capital < 1)
            {
                await JSRuntime.InvokeAsync<object>("alert", "부과총액이 입력되지 않았습니다.");
            }
            else if (capital_en.Use_Cost < 1)
            {
                await JSRuntime.InvokeAsync<object>("alert", "사용액이 입력되지 않았습니다.");
            }
            else
            {                
                if (capital_en.Repair_Capital_Code < 1)
                {
                    await repair_Capital_Lib.Add_Repair_Capital(capital_en);                    
                }
                else
                {
                    await repair_Capital_Lib.Edit_Repair_Capital(capital_en);
                }                
                await DetailsView();
                InsertViewsA = "A";
            }
            
        }

        /// <summary>
        /// 장기수선충당금 초기화 닫기
        /// </summary>
        private void btnCloseA()
        {
            InsertViewsA = "A";
        }


        /// <summary>
        /// 장기수선충당금 요율 열기
        /// </summary>
        private async Task btnRateOpen()
        {
            InsertViewsB = "B";
            if (lrList.Count < 1)
            {
                bnn.Levy_Start_Year = rpn.Founding_Date.Year - 1;
                bnn.Levy_Start_Month = rpn.Founding_Date.Month;
                bnn.Levy_End_Month = rpn.Founding_Date.Month;
                bnn.Levy_End_Year = DateTime.Now.Year;
                int aa = 13 - bnn.Levy_Start_Month;
                int yy = bnn.Levy_End_Year - bnn.Levy_Start_Year;
                bnn.Levy_Period = ((yy - 1) * 12) + aa + bnn.Levy_End_Month;
            }
            else
            {
                var rate = await levy_Rate_Lib.Detail_New(Apt_Code, Bylaw_Code.ToString());
                                
                if (rate.Levy_End_Month == 12)
                {
                    bnn.Levy_Start_Month = 1;
                    bnn.Levy_Start_Year = rate.Levy_End_Year + 1;
                    bnn.Levy_End_Year = rate.Levy_End_Year + 1;
                    bnn.Levy_End_Month = 12;
                    bnn.Levy_Rate_Accumulate = rate.Levy_Rate_Accumulate;
                    bnn.Levy_Period_New = rate.Levy_Period_New;

                    int aa = 13 - bnn.Levy_Start_Month;
                    int yy = bnn.Levy_End_Year - bnn.Levy_Start_Year;
                    bnn.Levy_Period = ((yy - 1) * 12) + aa + bnn.Levy_End_Month;
                }
                else
                {
                    bnn.Levy_Start_Year = rate.Levy_End_Year;
                    bnn.Levy_Start_Month = rate.Levy_End_Month + 1;
                    bnn.Levy_End_Year = rate.Levy_End_Year + 1;
                    bnn.Levy_End_Month = 12;
                    bnn.Levy_Rate_Accumulate = rate.Levy_Rate_Accumulate;
                    bnn.Levy_Period_New = rate.Levy_Period_New;

                    int aa = 13 - bnn.Levy_Start_Month;
                    int yy = bnn.Levy_End_Year - bnn.Levy_Start_Year;
                    bnn.Levy_Period = ((yy - 1) * 12) + aa + bnn.Levy_End_Month;
                }
                dbLevySum = rate.Levy_Rate_Accumulate;
            }
            
            strTitleA = "관리규약 적립요율 입력";
        }

        /// <summary>
        /// 적립요율 저장
        /// </summary>
        private async Task btnSaveB()
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
            bnn.Staff_Code = User_Code;
            bnn.Bylaw_Code = Bylaw_Code.ToString();
            bnn.Bylaw_Date = bpn.Bylaw_Revision_Date;
            bnn.Levy_Start_Date = Convert.ToDateTime(bnn.Levy_Start_Year + "-" + bnn.Levy_Start_Month + "-" + "01");
            bnn.Levy_End_Date = Convert.ToDateTime(bnn.Levy_End_Year + "-" + bnn.Levy_End_Month + "-" + "21");
            bnn.Levy_Period_New = intLevy_Period_New;

            if (bnn.Levy_Period_New < 0)
            {
                await JSRuntime.InvokeAsync<object>("alert", "누적 적립기간을 입력되지 않았습니다.");
            }
            else if (bnn.Levy_Rate <= 0)
            {
                await JSRuntime.InvokeAsync<object>("alert", "적립율을 입력되지 않았습니다.");
            }
            else if (bnn.Levy_Period < 3)
            {
                await JSRuntime.InvokeAsync<object>("alert", "적립기간을 입력되지 않았습니다.");
            }
            else
            {
                if (bnn.Levy_Period > 1)
                {
                    await levy_Rate_Lib.Add_Levy_Rate(bnn);
                    MonthSum = MonthSum + bnn.Levy_Period_New;
                    await DetailsView();
                }
                else
                {
                    await JSRuntime.InvokeAsync<object>("alert", "적립요율 적용기간이 2보다 작을 수는 없습니다.");
                }
            }

            if (bnn.Levy_End_Month == 12)
            {
                bnn.Levy_Start_Month = 1;
                bnn.Levy_Start_Year = bnn.Levy_End_Year + 1;
                bnn.Levy_End_Year = bnn.Levy_End_Year + 1;
                bnn.Levy_End_Month = 12;
                bnn.Levy_Rate = 0;
                int aa = 13 - bnn.Levy_Start_Month;
                int yy = bnn.Levy_End_Year - bnn.Levy_Start_Year;
                bnn.Levy_Period = ((yy - 1) * 12) + aa + bnn.Levy_End_Month;
            }
            else
            {
                bnn.Levy_Start_Year = bnn.Levy_End_Year;
                bnn.Levy_Start_Month = bnn.Levy_End_Month + 1;
                bnn.Levy_End_Year = bnn.Levy_End_Year + 1;
                bnn.Levy_End_Month = 12;
                bnn.Levy_Rate = 0;
                int aa = 13 - bnn.Levy_Start_Month;
                int yy = bnn.Levy_End_Year - bnn.Levy_Start_Year;
                bnn.Levy_Period = ((yy - 1) * 12) + aa + bnn.Levy_End_Month;
            }
        }

        /// <summary>
        /// 적립율 입력 시 실행
        /// </summary>
        private void OnRateSum(ChangeEventArgs a)
        {
            bnn.Levy_Rate = Convert.ToDouble(a.Value);
            
            bnn.Levy_Rate_Accumulate = dbLevyRateSum + bnn.Levy_Rate;
        }

        /// <summary>
        /// 시작년도 변경시 실행(첫 주기)
        /// </summary>
        private async Task OnLevyStartYear(ChangeEventArgs a)
        {
            bnn.Levy_Start_Year = Convert.ToInt32(a.Value);
            int aa = 13 - bnn.Levy_Start_Month;
            int yy = bnn.Levy_End_Year - bnn.Levy_Start_Year;
            int bYear = rpn.Founding_Date.Year;
            if (bnn.Levy_Start_Year >= bYear)
            {
                bnn.Levy_Period = yy + 1;

                intLevy_Period_New = ((yy - 1) * 12) + aa + bnn.Levy_End_Month;
            }
            else
            {
                bnn.Levy_Start_Year = rpn.Founding_Date.Year;
                await JSRuntime.InvokeAsync<object>("alert", "시작년도가 사용검사 년도 보다 적을 수는 없습니다.");
            }
        }

        /// <summary>
        /// 종료년도 변경시 실행
        /// </summary>
        private async Task OnLevyEndYear(ChangeEventArgs a)
        {
            bnn.Levy_End_Year = Convert.ToInt32(a.Value);
            int aa = 13 - bnn.Levy_Start_Month;
            int yy = bnn.Levy_End_Year - bnn.Levy_Start_Year;
            bnn.Levy_Period = yy + 1;
            intLevy_Period_New = ((yy - 1) * 12) + aa + bnn.Levy_End_Month;

            int re = (rpn.Plan_Period - 1) * 12;

            if (bnn.Levy_Start_Year < bnn.Levy_End_Year)
            {
                if (intLevy_Period_New > re)
                {
                    bnn.Levy_End_Year = bnn.Levy_Start_Year;
                    await JSRuntime.InvokeAsync<object>("alert", "적립요율의 적립종료년도가 계획기간을 넘을 수는 없습니다.");
                }
            }
            else
            {
                bnn.Levy_End_Year = DateTime.Now.Year;
                await JSRuntime.InvokeAsync<object>("alert", "종료년도가 시작년도 보다 적을 수는 없습니다.");
            }
        }

        /// <summary>
        /// 시작월 변경시 실행
        /// </summary>
        private async Task onStartMonth(ChangeEventArgs a)
        {
            bnn.Levy_Start_Month = Convert.ToInt32(a.Value);
            int aa = 13 - bnn.Levy_Start_Month;
            int yy = bnn.Levy_End_Year - bnn.Levy_Start_Year;

            bnn.Levy_Period = (yy + 1);

            intLevy_Period_New = ((yy - 1) * 12) + aa + bnn.Levy_End_Month;

            if (bnn.Levy_Period < 2)
            {
                bnn.Levy_End_Month = bnn.Levy_End_Month + 2;
                await JSRuntime.InvokeAsync<object>("alert", "적립요율 적용기간이 2보다 작을 수는 없습니다.");
            }
        }

        /// <summary>
        /// 종료월 선택 시 실행
        /// </summary>
        private async Task onEndMonth(ChangeEventArgs a)
        {
            bnn.Levy_End_Month = Convert.ToInt32(a.Value);
            int aa = 13 - bnn.Levy_Start_Month;
            int yy = bnn.Levy_End_Year - bnn.Levy_Start_Year;
            bnn.Levy_Period = yy + 1;

            intLevy_Period_New = ((yy - 1) * 12) + aa + bnn.Levy_End_Month;

            int re = (rpn.Plan_Period - 1) * 12;
            if (intLevy_Period_New > re)
            {
                bnn.Levy_End_Month = bnn.Levy_Start_Month;
                await JSRuntime.InvokeAsync<object>("alert", "적립요율의 적립종료년가 계획기간을 넘을 수는 없습니다.");
            }
        }

        /// <summary>
        /// 적립율 전체 삭제
        /// </summary>
        /// <returns></returns>
        private async Task btnRemove()
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{Aid}번 글을 정말로 삭제하시겠습니까?");
            if (isDelete)
            {
                await levy_Rate_Lib.Delete_All_Levy_Rate(Apt_Code, Bylaw_Code);
                await DetailsView();
            }
        }

        /// <summary>
        /// 장기수선충당금 적립요율 닫기
        /// </summary>
        private void btnCloseB()
        {
            InsertViewsB = "A";
        }
    }
}
