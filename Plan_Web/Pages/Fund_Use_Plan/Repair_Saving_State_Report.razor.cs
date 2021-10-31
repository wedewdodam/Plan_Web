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
        [Inject] public ICost_Lib cost_Lib { get; set; }
        [Inject] public IRepair_Capital_Lib repair_Capital_Lib { get; set; }
        [Inject] public IUnit_Price_Lib unit_Price_Lib { get; set; }
        [Inject] public IBylaw_Lib bylaw_Lib { get; set; }
        [Inject] public ILevy_Rate_Lib levy_Rate_Lib { get; set; }
        [Inject] public IRepair_Capital_Use_Lib repair_Capital_Use_Lib { get; set; }
        [Inject] public IRepair_Saving_Using_Pund_Lib repair_Using_Saving_Fund_Lib { get; set; }
        [Inject] public IRepair_Record_Lib repair_Record_Lib { get; set; }
        [Inject] public ICapital_Levy_Lib capital_Levy_Lib { get; set; }

        private Repair_Plan_Entity rpn { get; set; } = new Repair_Plan_Entity();
        private List<Repair_Saving_Using_Pund_Entity> ann { get; set; } = new List<Repair_Saving_Using_Pund_Entity>();
        private Repair_Saving_Using_Pund_Entity dnn { get; set; } = new Repair_Saving_Using_Pund_Entity();
        private Repair_Saving_Using_Pund_Entity fnn { get; set; } = new Repair_Saving_Using_Pund_Entity();
        private Unit_Price_string_Entity upsn { get; set; } = new Unit_Price_string_Entity();
        Unit_Price_Entity upn { get; set; } = new Unit_Price_Entity();
        private Bylaw_Entity bpn { get; set; } = new Bylaw_Entity();
        private Useing_Saving_Report_Entity usr { get; set; } = new Useing_Saving_Report_Entity();
        List<Repair_Record_Entity> rnn { get; set; } = new List<Repair_Record_Entity>();
        public double dbPlanSaveSum { get; private set; }
        public double dbInterestSaveSum { get; private set; }
        public double dbEtcSaveSum { get; private set; }

        //Capital_Levy_Entity cnn { get; set; } = new Capital_Levy_Entity();

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
            dnn.Report_Year = DateTime.Now.Year;
            Work_Year = dnn.Report_Year.ToString();
            NowYear = dnn.Report_Year.ToString();
            NextYear = (DateTime.Now.Year + 1).ToString();
            rpn = await repair_Plan_Lib.Detail_Repair_Plan(Apt_Code, strCode);
            ann = await repair_Using_Saving_Fund_Lib.GetList(Apt_Code);
        }

        /// <summary>
        /// 새로 등록 열기
        /// </summary>
        private async Task btnOpen()
        {            
            strTitle = "등록";
            dnn = new Repair_Saving_Using_Pund_Entity();
            dnn.Report_Date = Convert.ToDateTime(Work_Year + "-12-31");
            bpn = await bylaw_Lib.Details_Bylaw(Apt_Code);
            upsn = await unit_Price_Lib.Report_Plan_Cost_Bylaw(Apt_Code, rpn.Repair_Plan_Code, bpn.Bylaw_Code.ToString()); //장기수선충당 총계 정보
            upn = await unit_Price_Lib.Detail_Unit_Price_New(Apt_Code, rpn.Repair_Plan_Code, upsn.Levy_Rate, upsn.Levy_Period); //단가 관련 정보 가져오기
            dnn.Unit_Price = upn.Unit_Price;
            dnn.Supply_Area = upn.supply_total_Area.ToString();
            dnn.Adjust_Date = rpn.Adjustment_Date;
            dnn.Family_Num = upn.HouseHold;
            dnn.Founding_Date = rpn.Founding_Date;
            dnn.Adress = "-";
            dnn.Month_Impose = Convert.ToInt32(upn.supply_total_Area * dnn.Unit_Price);
            dnn.Plan_Funds = upn.plan_total_sum;
            usr = await repair_Capital_Lib.BalanceSum_Year(Apt_Code, Convert.ToInt32(Work_Year));
            //dnn.Balance_Funds = usr.dbBalanceAgoSum; //전년말까지 잔액
            dnn.Using_Funds_ago = usr.dbUseingAgoAgoSum; //초기화 사용액 포함 전전년도 말까지 사용 총액
            dnn.Using_Funds_now = usr.dbUseingAgoSum; //전년도 사용액
            dnn.Using_Funds = dnn.Using_Funds_ago + dnn.Using_Funds_now;
            dnn.Real_Balance_Funds = usr.dbBalanceAgoSum;
            dnn.Saving_Funds = Math.Round((dnn.Plan_Funds * (usr.LevyRateSum / 100)), 0);
            dnn.Need_Funds = dnn.Plan_Funds - dnn.Saving_Funds;
            dnn.Balance_Funds = dnn.Saving_Funds - dnn.Using_Funds;
            InsertViews = "B";
        }

        /// <summary>
        /// 장기수선충당금 사용 및 적립현황 년도별 목록
        /// </summary>
        private async Task OnByYearSelect(ChangeEventArgs a)
        {
            if (a.Value != null)
            {
                Work_Year = a.Value.ToString();
                
                dnn = await repair_Using_Saving_Fund_Lib.Detail_Year(Apt_Code, Work_Year);
            }
        }

        /// <summary>
        /// 상세보기
        /// </summary>
        private async Task OnByAid(Repair_Saving_Using_Pund_Entity ar)
        {
            dnn = ar;

            int be = await repair_Using_Saving_Fund_Lib.Being(Apt_Code, (dnn.Report_Year - 1).ToString());
            if (be > 0)
            {
                fnn = await repair_Using_Saving_Fund_Lib.Detail_Year(Apt_Code, (dnn.Report_Year - 1).ToString());
            }
            else
            {
                fnn.Balance_Funds = 0;
                fnn.Need_Funds = 0;
                fnn.Plan_Funds = 0;
                fnn.Saving_Funds = 0;
                fnn.Using_Funds = 0;
            }

            rnn = await repair_Record_Lib.list_Year_List_New(Apt_Code, dnn.Report_Year.ToString());
            usr = await repair_Capital_Lib.BalanceSum_Year(Apt_Code, Convert.ToInt32(dnn.Report_Year));
            dbPlanSaveSum = await capital_Levy_Lib._Year_Plan_Sum(Apt_Code, Convert.ToInt32(dnn.Report_Year), "장기수선충당금");
            dbInterestSaveSum = await capital_Levy_Lib._Year_Plan_Sum(Apt_Code, Convert.ToInt32(dnn.Report_Year), "이자");
            dbEtcSaveSum = await capital_Levy_Lib._Year_Etc_Sum(Apt_Code, Convert.ToInt32(dnn.Report_Year), "기타");

            strTitle = "상세보기";
            DataViews = "B";
        }

        /// <summary>
        /// 수정열기
        /// </summary>
        /// <param name="ar"></param>
        private void OnByEdit(Repair_Saving_Using_Pund_Entity ar)
        {
            dnn = ar;           

            strTitle = "수정";
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

        /// <summary>
        /// 계획기간 중 수선비 총액 입력 시 필요액 계산
        /// </summary>
        private void OnPlanFunds(ChangeEventArgs a)
        {
            dnn.Plan_Funds = Convert.ToDouble(a.Value);
            dnn.Need_Funds = dnn.Plan_Funds - dnn.Saving_Funds;
        }

        /// <summary>
        /// 적립율에 의한 부과총액 입력 시 필요액 계산
        /// </summary>
        private void OnSavingFunds(ChangeEventArgs a)
        {
            dnn.Saving_Funds = Convert.ToDouble(a.Value);
            dnn.Need_Funds = dnn.Plan_Funds - dnn.Saving_Funds;
        }

        /// <summary>
        /// 전전년도까지 사용액 입력 시 사용총액 계산
        /// </summary>
        private void OnUsingFundsAgo(ChangeEventArgs a)
        {
            dnn.Using_Funds_ago = Convert.ToDouble(a.Value);
            dnn.Using_Funds = dnn.Using_Funds_ago + dnn.Using_Funds_now;
        }

        /// <summary>
        /// 전년도에 사용액 입력 시 사용총액 계산
        /// </summary>
        private void OnUsingFundsNow(ChangeEventArgs a)
        {
            dnn.Using_Funds_now = Convert.ToDouble(a.Value);
            dnn.Using_Funds = dnn.Using_Funds_ago + dnn.Using_Funds_now;
        }

        /// <summary>
        /// 입력 닫기
        /// </summary>
        private void btnClose()
        {
            InsertViews = "A";
        }

        /// <summary>
        /// 상세 닫기
        /// </summary>
        private void btnCloseV()
        {
            DataViews = "A";
        }

        /// <summary>
        /// 입력 저장
        /// </summary>
        private async Task btnSave()
        {
            dnn.Apt_Code = Apt_Code;
            dnn.Apt_Name = Apt_Name;
            dnn.Staff_Code = User_Code;
            dnn.Staff_Name = User_Name;
            dnn.Report_Year = dnn.Report_Date.Year;
            
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
            dnn.PostIP = myIPAddress;
            #endregion 아이피 입력

            if (dnn.Apt_Code == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인하지 않았습니다.");
            }
            else if (dnn.Balance_Funds < 1000)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "잔액을 입력하지 않았습니다.");
            }
            else if (dnn.Month_Impose < 1000)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "월 적립액을 입력하지 않았습니다.");
            }
            else if (dnn.Need_Funds < 1000)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "필요액을 입력하지 않았습니다.");
            }
            else if (dnn.Plan_Funds < 1000)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "수선비 총액을 입력하지 않았습니다.");
            }
            else if (dnn.Saving_Funds < 1000)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "적립액을 입력하지 않았습니다.");
            }
            else if (dnn.Unit_Price < 1)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "단가를 입력하지 않았습니다.");
            }
            else if (dnn.Supply_Area == null || dnn.Supply_Area == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "공급면적을 입력하지 않았습니다.");
            }            
            else if (dnn.Report_Date == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "작성일을 입력하지 않았습니다.");
            }
            else
            {
                if (dnn.Aid < 1)
                {
                    int be = await repair_Using_Saving_Fund_Lib.Being(Apt_Code, Work_Year);
                    if (be < 1)
                    {
                        await repair_Using_Saving_Fund_Lib.Add(dnn);
                    }
                    else
                    {
                        await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", dnn.Report_Year + "년은 이미 입력되었으므로 입력할 수 없습니다.");
                    }
                }
                else
                {
                    await repair_Using_Saving_Fund_Lib.Edit(dnn);
                }
                dnn = new Repair_Saving_Using_Pund_Entity();
                InsertViews = "A";
                await DetailsView();
            }
        }
    }
}
