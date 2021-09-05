using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Plan_Apt_Lib;
using Plan_Blazor_Lib;
using Plan_Blazor_Lib.Cost;
using Plan_Blazor_Lib.Cycle;
using Plan_Blazor_Lib.Plan;
using Plan_Blazor_Lib.Review;
using Plan_Lib;
using Plan_Lib.Pund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plan_Web.Pages.Repair_Plan
{
    public partial class Index
    {
        #region 인스턴스

        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Parameter] public string Aid { get; set; }
        [Inject] public NavigationManager MyNav { get; set; } // Url
        [Inject] private IJSRuntime JSRuntime { get; set; }
        [Inject] public IAptInfor_Lib aptInfor_Lib { get; set; }
        [Inject] public IPlan_Review_Lib plan_Review_Lib { get; set; }
        [Inject] public IRepair_Plan_Lib repair_Plan_Lib { get; set; }
        [Inject] public IArticle_Lib article_Lib { get; set; }
        [Inject] public ICycle_Lib cycle_Lib { get; set; }
        [Inject] public ICost_Lib cost_Lib { get; set; }
        [Inject] public IRepair_Capital_Lib repair_Capital_Lib { get; set; }
        [Inject] public IUnit_Price_Lib unit_Price_Lib { get; set; }
        [Inject] public IBylaw_Lib bylaw_Lib { get; set; }
        [Inject] public ISmallSum_Repuirement_Selection_Lib smallSum_Repuirement_Selection_Lib { get; set; }
        [Inject] public IRepair_Object_Selection_Lib repair_Object_Selection_Lib { get; set; }
        [Inject] public IRepair_SmallSum_Object_Lib repair_SmallSum_Object_Lib { get; set; }
        [Inject] public IRepair_SmallSum_Requirement_Lib repair_SmallSum_Requirement_Lib { get; set; }

        #endregion 인스턴스

        #region 속성
        
        //private Plan_Review_Entity ann { get; set; } = new Plan_Review_Entity();
        //private Review_Content_Entity bnn { get; set; } = new Review_Content_Entity();

        List<Plan_Review_Entity> rnnA { get; set; } = new List<Plan_Review_Entity>();
        private Unit_Price_string_Entity upsn { get; set; } = new Unit_Price_string_Entity();

        Unit_Price_Entity upn { get; set; } = new Unit_Price_Entity();
        private Bylaw_Entity bpn { get; set; } = new Bylaw_Entity();
        private Repair_Plan_Entity rpn { get; set; } = new Repair_Plan_Entity();
        private List<Repair_SmallSum_Requirement_Selection_Entity> srse = new List<Repair_SmallSum_Requirement_Selection_Entity>();
        private Repair_SmallSum_Requirement_Selection_Entity srs = new Repair_SmallSum_Requirement_Selection_Entity();
        private List<Repair_SmallSum_Object_Selection_Entity> sose = new List<Repair_SmallSum_Object_Selection_Entity>();
        private Repair_SmallSum_Object_Selection_Entity sos = new Repair_SmallSum_Object_Selection_Entity();
        private List<Repair_SmallSum_Object_Entity> rso = new List<Repair_SmallSum_Object_Entity>();
        private List<Repair_SmallSum_Requirement_Entity> rsr = new List<Repair_SmallSum_Requirement_Entity>();
        #endregion 속성

        #region 변수

        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public string BuildDate { get; private set; }
        public string InsertViews { get; set; } = "A";
        public string InsertViewsA { get; set; } = "A";
        public string InsertViewsB { get; set; } = "A";
        public int EditViews { get; set; } = 0;
        //public int Aid { get; set; } = 0;
        public string strTitle { get; set; }
        public string strPlan_Year { get; set; }
        public string strPlan_Code { get; set; }

        public string ViewsA { get; set; } = "A";
        public string ViewsB { get; set; } = "A";
        public string strSortA { get; set; }
        public string strSortB { get; set; }
        public int Article_Count { get; set; }
        public double dbBalance { get; set; } = 0;
        public string NewOpen { get; set; } = "A";

        public string Plan_Review_Code { get; set; }
        public string strYear { get; set; }
        public string strYearA { get; set; }
        public int required { get; set; } = 0;
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
                BuildDate = authState.User.Claims.FirstOrDefault(c => c.Type == "BuildDate")?.Value;
                string strV = await repair_Plan_Lib.Repair_Plan_Code(Apt_Code);
                if (strV == Aid)
                {
                    MyNav.NavigateTo("/Repair_Plan/New");
                }
                else
                {
                    await DetailsView();
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
            string intY = Aid;
            if (intY != null)
            {
                rpn = await repair_Plan_Lib.Detail_Repair_Plan(Apt_Code, intY); // 첫 장기계획 상세 정보
                //upn = await unit_Price_Lib.Detail_Unit_Price_New(Apt_Code, rpn.Repair_Plan_Code, upsn.Levy_Rate, upsn.Levy_Period); //단가 관련 정보 가져오기
                int Bylaw_Code = await bylaw_Lib.Bylaw_Last_Code(Apt_Code);
                upsn = await unit_Price_Lib.Report_Plan_Cost_Bylaw(Apt_Code, rpn.Repair_Plan_Code, Bylaw_Code.ToString());
                upn = await unit_Price_Lib.Detail_Unit_Price_New(Apt_Code, rpn.Repair_Plan_Code, upsn.Levy_Rate, upsn.Levy_Period); //단가 관련 정보 가져오기
                srse = await smallSum_Repuirement_Selection_Lib.GetList_RSRS(rpn.Repair_Plan_Code);
                sose = await repair_Object_Selection_Lib.GetList_RSOS(rpn.Repair_Plan_Code, "A");
            }
            else
            {
                strTitle = "장기수선게획 총론 새로 등록";
                rpn = new Repair_Plan_Entity();

                rpn = await repair_Plan_Lib.Detail_Apt_Last_Plan(Apt_Code);

                //rpn.PostDate = DateTime.Now;
                rpn.Aid = 0;
                strYearA = (Convert.ToDateTime(BuildDate).Year + rpn.Plan_Period).ToString();
                strYear = strYearA + "년";
                strPeriodYear = "60년간";
                strSmall_Sum = string.Format("{0: ###,###.#}", rpn.Small_Sum);
                //rpn.Plan_Period = 60;
                //rpn.Founding_Date = Convert.ToDateTime(BuildDate).Date;
                rpn.Adjustment_Date = DateTime.Now.Date;
                rpn.Plan_Review_Date = await plan_Review_Lib.Last_Plan_ReviewDate_Apt(Apt_Code);
                rpn.Adjustment_Num = (rpn.Adjustment_Num) + 1;
                rnnA = await plan_Review_Lib.GetList_Apt_Code(Apt_Code);
                rpn.Emergency_Basis = "변압기의 폭팔 등과 같은 예상할 수 없는 긴급상황이 발생하여 2차 피해가 심각하거나 입주민의 불편이 진행되고 있어서 긴급히 보수를 하지 않으면 불가피한 사유가 발생한 경우에 장기수선계획에 반영되어 있지 않거나 수선주기가 이르지 않았을 경우, 계획된 수선금액이 부족한 경우에 있어서 먼저 수선공사를 실행하고 추후에 장기수선계획에 반영할 수 있다. 이 경우 입대의 의결을 거쳐한다. 다만 입주자대표회의의 의결을  받을 수없은 긴급성이 있는 경우에는 실행 후 입대의 의결할 수 있다.";
                rpn.SmallSum_Basis = "주택법 시행령 제66조 제2항은 “장기수선충당금은 관리주체가 다음 각 호의 사항이 포함된 장기수선충당금 사용계획서를 장기수선계획에 따라 작성하고 제51조제1항에 따른 입주자대표회의의 의결을 거쳐 사용한다.” 라고 규정하고 있으나 갑작스런 배관의 누수, 배수펌프의 고장, 승강기의 안전상의 위험이나 고장, 법령에 따른 사용금지의 우려, 그 외 누수나 고장 등 예기치 못한 사정으로 장기수선 계획의 주기를 기다려 수선이나 보수를 할 수 없는 경우 예외적으로 “국토교통부 민원회신 2014년 3월 28일”의 경우와 같이 예외적인 장기수선충당금 소액 사용을 말한다.";
                rpn.Repair_Plan_Etc = "장기수선계획을 검토나 조정을 위해 외부에 전문가의 자문을 받거나 외부 전문가에게 의뢰하여 검토나 조정을 하려는 경우에 그 비용을 장기수선충당금에서 사용할 수 있다. 또한 수선항목(공사종별)에 감리나 설계가 필요한 경우에는 수선금액에 감리나 설계비용을 포함하여 산출하였다.";
                InsertViews = "B";
            }
        }

        #region Url 이동
        private void OnPlanUrl()
        {

        }

        private void OnArticleUrl()
        {
            MyNav.NavigateTo("Repair_Article/" + rpn.Repair_Plan_Code);
        }

        private void OnCycleUrl()
        {
            MyNav.NavigateTo("Repair_Cycle/" + rpn.Repair_Plan_Code);
        }

        private void OnCostUrl()
        {
            MyNav.NavigateTo("Repair_Cost/" + rpn.Repair_Plan_Code);
        }

        private void OnCompleteUrl()
        {
            MyNav.NavigateTo("Repair_Complete/" + rpn.Repair_Plan_Code);
        }
        #endregion

        /// <summary>
        /// 수정 열기
        /// </summary>
        private void btnOpen(Repair_Plan_Entity aq)
        {
            InsertViews = "B";
            rpn = aq;
            strYearA = (Convert.ToDateTime(BuildDate).Year + aq.Plan_Period).ToString();
            strYear = strYearA + "년";
            strPeriodYear = rpn.Plan_Period.ToString() + "년간";
            strSmall_Sum = string.Format("{0: ###,###.#}", rpn.Small_Sum);
        }

        /// <summary>
        /// 장기수선계획 새로등록 열기
        /// </summary>
        private async Task btnNews()
        {
            int intS = await repair_Plan_Lib.Being_Plan_None_Complete(Apt_Code);//미완료 존재여부 확인

            if (intS < 1)
            {
                strTitle = "장기수선게획 총론 새로 등록";
                rpn = new Repair_Plan_Entity();
                rpn.PostDate = DateTime.Now;

                strYearA = (Convert.ToDateTime(BuildDate).Year + 60).ToString();
                strYear = strYearA + "년";
                strPeriodYear = "60년간";


                rpn.Founding_Date = Convert.ToDateTime(BuildDate).Date;
                rpn.Adjustment_Date = DateTime.Now.Date;
                rpn.Plan_Review_Date = DateTime.Now.Date;
                InsertViews = "B";
                rnnA = await plan_Review_Lib.GetList_Apt_Code(Apt_Code);
                rpn.Adjustment_Num = (await repair_Plan_Lib.Plan_Num_Last(Apt_Code)) + 1;

                rpn.Emergency_Basis = "변압기의 폭팔 등과 같은 예상할 수 없는 긴급상황이 발생하여 2차 피해가 심각하거나 입주민의 불편이 진행되고 있어서 긴급히 보수를 하지 않으면 불가피한 사유가 발생한 경우에 장기수선계획에 반영되어 있지 않거나 수선주기가 이르지 않았을 경우, 계획된 수선금액이 부족한 경우에 있어서 먼저 수선공사를 실행하고 추후에 장기수선계획에 반영할 수 있다. 이 경우 입대의 의결을 거쳐한다. 다만 입주자대표회의의 의결을  받을 수없은 긴급성이 있는 경우에는 실행 후 입대의 의결할 수 있다.";
                rpn.SmallSum_Basis = "주택법 시행령 제66조 제2항은 “장기수선충당금은 관리주체가 다음 각 호의 사항이 포함된 장기수선충당금 사용계획서를 장기수선계획에 따라 작성하고 제51조제1항에 따른 입주자대표회의의 의결을 거쳐 사용한다.” 라고 규정하고 있으나 갑작스런 배관의 누수, 배수펌프의 고장, 승강기의 안전상의 위험이나 고장, 법령에 따른 사용금지의 우려, 그 외 누수나 고장 등 예기치 못한 사정으로 장기수선 계획의 주기를 기다려 수선이나 보수를 할 수 없는 경우 예외적으로 “국토교통부 민원회신 2014년 3월 28일”의 경우와 같이 예외적인 장기수선충당금 소액 사용을 말한다.";
                rpn.Repair_Plan_Etc = "장기수선계획을 검토나 조정을 위해 외부에 전문가의 자문을 받거나 외부 전문가에게 의뢰하여 검토나 조정을 하려는 경우에 그 비용을 장기수선충당금에서 사용할 수 있다. 또한 수선항목(공사종별)에 감리나 설계가 필요한 경우에는 수선금액에 감리나 설계비용을 포함하여 산출하였다.";
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "완료되지 않은 장기수선계획이 있습니다.\n 먼저 장기수선계획을 완료한 후에 새로운 장기수선계획을 생성하세요.");
            }
        }
        /// <summary>
        /// 장기수선계획 삭제
        /// </summary>
        private async Task btnRemove(string Code)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{Code}의 장기수선계획을 정말로 삭제하시겠습니까? \n 삭제하면 총론과 수선항목, 수선주기, 수선금액이 모두 삭제됩니다.\n 삭제되면 다시는 되살릴 수 없으므로 주의하여야 합니다. \n \n \n 주의 하세요!");
            if (isDelete)
            {
                await repair_Plan_Lib.Delete_RepairPlan_PlanCode(Code);
                await smallSum_Repuirement_Selection_Lib.Delete_RequirementSelection_PlanCode(Code);
                await repair_Object_Selection_Lib.Delete_ObjetSelection_PlanCode(Code);
                await article_Lib.Delete_Article_PlanCode(Code);
                await cycle_Lib.Delete_Cycle_PlanCode(Code);
                await cost_Lib.Delete_RepairCost_PlanCode(Code);

                MyNav.NavigateTo("/Repair_Plan/List");
            }
        }

        private void onBasis(ChangeEventArgs a)
        {
            rpn.Emergency_Basis = a.Value.ToString();
        }

        /// <summary>
        /// 검토일 선택
        /// </summary>
        private async Task OnReview(ChangeEventArgs a)
        {
            var df = await plan_Review_Lib.Detail_PlanReview(Convert.ToInt32(a.Value));
            rpn.Plan_Review_Code = df.Plan_Review_Code.ToString();
            rpn.Plan_Review_Date = df.PlanReview_Date;
            if (df.PlanReview_Division == "정기검토")
            {
                rpn.Adjustment_Division = "정기조정";
            }
            else
            {
                rpn.Adjustment_Division = "수시조정";
            }
        }

        /// <summary>
        /// 소액 요건 선택 수정
        /// </summary>
        /// <param name="av"></param>
        public string InsertViewsAA { get; set; } = "A";
        public string InsertViewsBB { get; set; } = "A";
        private void OnByEditA(Repair_SmallSum_Requirement_Selection_Entity av)
        {
            srs = av;
            InsertViewsAA = "B";
            strTitleA = "소액지출 대상 수정";
        }

        /// <summary>
        /// 소액 요건 선택 삭제
        /// </summary>
        /// <param name="sv"></param>
        private async Task OnRemoveA(int Aid)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{Aid}번 글을 정말로 삭제하시겠습니까?");
            if (isDelete)
            {
                await smallSum_Repuirement_Selection_Lib.Remove_SmallSum_Requirement_Selection(Aid);
                //rsr = await repair_SmallSum_Requirement_Lib.GetList_SmallSum_Requirement();
                srse = await smallSum_Repuirement_Selection_Lib.GetList_RSRS(rpn.Repair_Plan_Code);
                //sose = await repair_Object_Selection_Lib.GetList_RSOS(rpn.Repair_Plan_Code, "A");
            }

        }

        /// <summary>
        /// 소액 대상 선택 수정
        /// </summary>
        /// <param name="av"></param>
        public string strTitleA { get; set; }
        public string strTitleB { get; set; }
        private void OnByEditB(Repair_SmallSum_Object_Selection_Entity av)
        {
            sos = av;
            InsertViewsBB = "B";
            strTitleB = "소액지출 대상 수정";
        }

        /// <summary>
        /// 소액 대상 선택 삭제
        /// </summary>
        /// <param name="sv"></param>
        private async Task OnRemoveB(int Aid)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{Aid}번 글을 정말로 삭제하시겠습니까?");
            if (isDelete)
            {
                //await repair_Object_Selection_Lib.Remove(Apt_Code, rpn.Repair_Plan_Code, Aid.ToString());
                await repair_Object_Selection_Lib.Remove_SmallSum_Object_Selection(Aid);
                //rso = await repair_SmallSum_Object_Lib.GetList_SmallSum_Object("A");
                //srse = await smallSum_Repuirement_Selection_Lib.GetList_RSRS(rpn.Repair_Plan_Code);
                sose = await repair_Object_Selection_Lib.GetList_RSOS(rpn.Repair_Plan_Code, "A");
            }
        }

        /// <summary>
        /// 소액지출 대상 새로 등록
        /// </summary>
        private async Task OnSmallSum_Object_Insert()
        {
            rso = await repair_SmallSum_Object_Lib.GetList_SmallSum_Object("A");
            InsertViewsB = "B";
            strTitleB = "소액지출 대상 등록";
        }

        /// <summary>
        /// 소액지출 요건 새로 등록
        /// </summary>
        private async Task OnSmallSum_Requirement_Insert()
        {
            rsr = await repair_SmallSum_Requirement_Lib.GetList_SmallSum_Requirement();
            InsertViewsA = "B";
            strTitleA = "소액지출 요건 등록";
        }

        /// <summary>
        /// 소액지출 요건 입력 여부 확인
        /// </summary>
        private int In_Being_A(string Aid)
        {
            return smallSum_Repuirement_Selection_Lib.Being_Code(Apt_Code, rpn.Repair_Plan_Code, Aid);
        }
        /// <summary>
        /// 소액지출 대상 입력 여부 확인
        /// </summary>
        private int In_Being_O(string Aid)
        {
            return repair_Object_Selection_Lib.Being_Code(Apt_Code, rpn.Repair_Plan_Code, Aid);
        }


        /// <summary>
        /// 총론 입력 닫기
        /// </summary>
        private void btnClose()
        {
            if (Aid == null || Aid == "")
            {
                MyNav.NavigateTo("/Repair_Plan/List");
            }
            else
            {
                InsertViews = "A";
            }
        }

        /// <summary>
        /// 총론 입력하기
        /// </summary>
        /// <returns></returns>
        public async Task btnSave()
        {
            rpn.Apt_Code = Apt_Code;
            if (rpn.Emergency_Basis == null || rpn.Emergency_Basis == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "긴급지출 근거를 입력하지 않았습니다.");
            }
            else if (rpn.Plan_Period < 5)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "계획기간을 입력하지 않았습니다.");
            }
            else if (rpn.Founding_man == "" || rpn.Adjustment_Man == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "사업주체를 입력하지 않았습니다.");
            }
            else if (rpn.Adjustment_Division == "" || rpn.Adjustment_Division == null || rpn.Adjustment_Division == "Z")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "조정구분을 선택하지 않았습니다.");
            }
            else if (rpn.SmallSum_Basis == "" || rpn.SmallSum_Basis == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "소액지출 근거를 입력하지 않았습니다.");
            }
            else if (rpn.SmallSum_Unit == "" || rpn.SmallSum_Unit == null || rpn.SmallSum_Unit == "Z")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "소액구분을 입력하지 않았습니다.");
            }
            else if (rpn.Apt_Code == "" || rpn.Apt_Code == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인하지 않았습니다.");
                MyNav.NavigateTo("/");
            }
            else
            {
                if (rpn.Aid < 1)
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
                    rpn.PostIP = myIPAddress;
                    #endregion 아이피 입력
                    rpn.Small_Sum = Convert.ToInt32(strSmall_Sum.Replace(",", ""));
                    rpn.Adjustment_Year = rpn.Adjustment_Date.Year.ToString();
                    rpn.Adjustment_Month = rpn.Adjustment_Date.Month.ToString();
                    //rpn.Plan_Review_Code = "없음";
                    rpn.Repair_Plan_Code = Apt_Code + (await repair_Plan_Lib.Last_Aid());
                    rpn.User_ID = User_Code;

                    int intbe = await repair_Plan_Lib.Repeat_Code(Apt_Code, rpn.Repair_Plan_Code);

                    if (intbe < 1)
                    {
                        int a = await repair_Plan_Lib.Add_Repair_Plan_Add(rpn);
                        string strCode = await repair_Plan_Lib.Last_Apt_Code(Apt_Code);
                        await article_Lib.All_Insert_Code(Apt_Code, strCode, rpn.Repair_Plan_Code, User_Code, rpn.PostIP);    
                    }                 
                }
                else
                {
                    await repair_Plan_Lib.Edit_Repair_Plan(rpn);
                }

                await DetailsView();

                InsertViews = "A";
            }

        }

        /// <summary>
        /// 계획기간 설정
        /// </summary>
        public string strPeriodYear { get; set; } = "0년간";

        /// <summary>
        /// 계획기간 선택 시 실행
        /// </summary>
        private async Task OnPeriodYear(ChangeEventArgs a)
        {
            int intYearA = Convert.ToInt32(a.Value);
            int intYearB = Convert.ToDateTime(BuildDate).Year;
            int Y = intYearA - intYearB;
            if (Y > 5)
            {
                strYear = a.Value.ToString() + "년";
                rpn.Plan_Period = (intYearA - intYearB);
                strPeriodYear = "(" + rpn.Plan_Period + "년간)";
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "계획기간 5년간 이하 일 수 없습니다.");
            }
        }

        /// <summary>
        /// 소액지출 요건 선택
        /// </summary>
        /// <param name="aq"></param>
        /// <returns></returns>
        private async Task OnBySelectR(Repair_SmallSum_Requirement_Entity aq)
        {
            srs.Apt_Code = Apt_Code;
            srs.Repair_Plan_Code = rpn.Repair_Plan_Code;
            srs.SmallSum_Requirement_Code = aq.SmallSum_Requirement_Code;
            srs.SmallSum_Requirement_Content = aq.SmallSum_Requirement;
            srs.SmallSum_Requirement_Selection_Code = rpn.Repair_Plan_Code + (await smallSum_Repuirement_Selection_Lib.BeCount(Apt_Code));

            await smallSum_Repuirement_Selection_Lib.Add_RSR_Selection(srs);
            rsr = await repair_SmallSum_Requirement_Lib.GetList_SmallSum_Requirement();
        }

        /// <summary>
        /// 소액지출 요건 선택 삭제
        /// </summary>
        /// <param name="Aid"></param>
        /// <returns></returns>
        private async Task OnByRemoveR(string Aid)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{Aid}번 글을 정말로 삭제하시겠습니까?");
            if (isDelete)
            {
                await smallSum_Repuirement_Selection_Lib.Remove(Apt_Code, rpn.Repair_Plan_Code, Aid);
                rsr = await repair_SmallSum_Requirement_Lib.GetList_SmallSum_Requirement();
            }
        }

        /// <summary>
        /// 소액지출 금액 입력 시행
        /// </summary>
        public string strSmall_Sum { get; set; }
        private async Task OnSmall_Sum(ChangeEventArgs a)
        {
            string strA = a.Value.ToString();
            strA = strA.Replace(",", "").Replace(" ", "");
            try
            {
                rpn.Small_Sum = Convert.ToInt32(strA);
                strSmall_Sum = string.Format("{0: ###,###.#}", rpn.Small_Sum);
            }
            catch (Exception)
            {
                rpn.Small_Sum = 0;
                strSmall_Sum = "0";
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "소액지출 금액을 입력하지 않았습니다.");
            }
        }

        /// <summary>
        /// 소액지출 요건 입력 닫기
        /// </summary>
        private async Task btnCloseR()
        {
            InsertViewsA = "A";
            //rsr = await repair_SmallSum_Requirement_Lib.GetList_SmallSum_Requirement();
            //await DetailsView();
            srse = await smallSum_Repuirement_Selection_Lib.GetList_RSRS(rpn.Repair_Plan_Code);
            //sose = await repair_Object_Selection_Lib.GetList_RSOS(rpn.Repair_Plan_Code, "A");
        }

        /// <summary>
        /// 소액지출 요건 입력 닫기
        /// </summary>
        private async Task btnCloseO()
        {
            InsertViewsB = "A";
            //await DetailsView();
            sose = await repair_Object_Selection_Lib.GetList_RSOS(rpn.Repair_Plan_Code, "A");
        }

        /// <summary>
        /// 소액지출 대상 선택
        /// </summary>
        /// <param name="aq"></param>
        /// <returns></returns>
        private async Task OnBySelectO(Repair_SmallSum_Object_Entity aq)
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
            sos.PostIP = myIPAddress;

            #endregion 아이피 입력
            sos.Apt_Code = Apt_Code;
            if (sos.Apt_Code == null || sos.Apt_Code == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인하지 않았습니다.");
                MyNav.NavigateTo("/");
            }
            else
            {
                sos.Repair_Plan_Code = rpn.Repair_Plan_Code;
                sos.SmallSum_Object_Code = aq.SmallSum_Object_Code;
                sos.SmallSum_Object_Content = aq.SmallSum_Object;
                sos.SmallSum_Object_Selection_Code = rpn.Repair_Plan_Code + (await repair_Object_Selection_Lib.BeCount(Apt_Code));

                await repair_Object_Selection_Lib.Add_RSO_Selection(sos);
                rso = await repair_SmallSum_Object_Lib.GetList_SmallSum_Object("A");
            }
        }

        /// <summary>
        /// 소액지출 요건 선택 삭제
        /// </summary>
        /// <param name="Aid"></param>
        /// <returns></returns>
        private async Task OnByRemoveO(string Aid)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{Aid}번 글을 정말로 삭제하시겠습니까?");
            if (isDelete)
            {
                await repair_Object_Selection_Lib.Remove(Apt_Code, rpn.Repair_Plan_Code, Aid);
                //rsr = await repair_SmallSum_Requirement_Lib.GetList_SmallSum_Requirement();
                rso = await repair_SmallSum_Object_Lib.GetList_SmallSum_Object("A");
            }
        }

        /// <summary>
        /// 소액지출 요건 수정 닫기
        /// </summary>
        private void btnCloseOR()
        {
            InsertViewsAA = "A";
        }

        /// <summary>
        /// 소액지출 대상 수정하기
        /// </summary>
        /// <returns></returns>
        private async Task btnSaveOR()
        {
            await smallSum_Repuirement_Selection_Lib.Edit_RSOS_A(srs.Aid.ToString(), srs.SmallSum_Requirement_Content);
            srse = await smallSum_Repuirement_Selection_Lib.GetList_RSRS(rpn.Repair_Plan_Code);
            InsertViewsAA = "A";
        }

        /// <summary>
        /// 소액지출 대상 수정 닫기
        /// </summary>
        private void btnCloseOB()
        {
            InsertViewsBB = "A";
        }

        /// <summary>
        /// 소액지출 대상 수정하기
        /// </summary>
        private async Task btnSaveOB()
        {
            await repair_Object_Selection_Lib.Edit_RSOS_A(sos.Aid.ToString(), sos.SmallSum_Object_Content);
            sose = await repair_Object_Selection_Lib.GetList_RSOS(rpn.Repair_Plan_Code, "A");
            InsertViewsBB = "A";
        }

        private async Task btnSaveR()
        {
            await smallSum_Repuirement_Selection_Lib.Add_RSR_Selection(srs);
        }
    }
}
