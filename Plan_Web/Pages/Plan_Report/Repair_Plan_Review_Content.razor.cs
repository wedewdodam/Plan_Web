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

namespace Plan_Web.Pages.Plan_Report
{
    public partial class Repair_Plan_Review_Content
    {
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Parameter] public string Aid { get; set; }
        [Inject] public NavigationManager MyNav { get; set; } // Url
        [Inject] private IJSRuntime JSRuntime { get; set; }
        [Inject] public IAptInfor_Lib aptInfor_Lib { get; set; }
        [Inject] public IRepair_Plan_Lib repair_Plan_Lib { get; set; }
        //[Inject] public IArticle_Lib article_Lib { get; set; }
        //[Inject] public ICycle_Lib cycle_Lib { get; set; }
        [Inject] public ICost_Lib cost_Lib { get; set; }
        [Inject] public IPlan_Review_Lib plan_Review_Lib { get; set; }
        [Inject] public IReview_Content_Lib review_Content_Lib { get; set; }
        [Inject] public IRepair_Capital_Lib repair_Capital_Lib { get; set; }
        [Inject] public IUnit_Price_Lib unit_Price_Lib { get; set; }
        [Inject] public IBylaw_Lib bylaw_Lib { get; set; }

        private Repair_Plan_Entity rpn { get; set; } = new Repair_Plan_Entity();
        private Plan_Review_Entity ann { get; set; } = new Plan_Review_Entity();
        private Review_Content_Entity bnn { get; set; } = new Review_Content_Entity();
        private List<Plan_Review_Plan_Entity> annA { get; set; } = new List<Plan_Review_Plan_Entity>();
        private List<Review_Content_Join_Enity> bnnA { get; set; } = new List<Review_Content_Join_Enity>();
        private List<Review_Content_Join_Enity> bnnB { get; set; } = new List<Review_Content_Join_Enity>();
        private List<Review_Content_Join_Enity> bnnC { get; set; } = new List<Review_Content_Join_Enity>();
        private List<Review_Content_Join_Enity> bnnD { get; set; } = new List<Review_Content_Join_Enity>();
        private List<Review_Content_Join_Enity> bnnE { get; set; } = new List<Review_Content_Join_Enity>();
        private List<Review_Content_Join_Enity> bnnF { get; set; } = new List<Review_Content_Join_Enity>();
        private Unit_Price_string_Entity upsn { get; set; } = new Unit_Price_string_Entity();
        private Unit_Price_Entity upn { get; set; } = new Unit_Price_Entity();
        public double dbBalance { get; private set; }
        private Bylaw_Entity bpn { get; set; } = new Bylaw_Entity();
        public string strPlan_Code { get; private set; }
        private Repair_Plan_Entity epn { get; set; } = new Repair_Plan_Entity();
        private List<Repair_Plan_Entity> rnn { get; set; } = new List<Repair_Plan_Entity>(); //장기수선계획 조정년도 목록
        private List<Repair_Plan_Entity> rnnA { get; set; } = new List<Repair_Plan_Entity>(); //장기수선계획 코드 목록
        private List<Plan_Review_Entity> prnn { get; set; } = new List<Plan_Review_Entity>();

        public string Apt_Code { get; private set; }
        public string User_Code { get; private set; }
        public string Apt_Name { get; private set; }
        public string User_Name { get; private set; }
        public string BuildDate { get; private set; }
        public string strCode { get; private set; }
        public string strReview_Code { get; set; }
        public string strYear { get; set; }
        public string strPlan_Year { get; private set; }

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
                    rpn = await repair_Plan_Lib.Detail_Repair_Plan(Apt_Code, strCode);
                    if (rpn.Plan_Review_Code == "없음" || rpn.Plan_Review_Code == "" || rpn.Plan_Review_Code == null)
                    {
                        
                    }
                    else
                    {
                        await DetailsView(rpn.Plan_Review_Code);
                    }
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
        private async Task DetailsView(string Aid)
        {
            if (Aid != null || Aid != "Z")
            {
                ann = await plan_Review_Lib.Detail_PlanReview(Convert.ToInt32(Aid));
                epn = await repair_Plan_Lib.Detail_Repair_Plan(ann.Apt_Code, ann.Repair_Plan_Code);
                //dbBalance = await repair_Capital_Lib.BalanceSum(Apt_Code);//잔액가져오기
                int Bylaw_Code = await bylaw_Lib.Bylaw_Last_Code(Apt_Code);
                upsn = await unit_Price_Lib.Report_Plan_Cost_Bylaw(Apt_Code, ann.Repair_Plan_Code, Bylaw_Code.ToString());
                upn = await unit_Price_Lib.Detail_Unit_Price_New(Apt_Code, ann.Repair_Plan_Code, upsn.Levy_Rate, upsn.Levy_Period); //단가 관련 정보 가져오기
                dbBalance = await repair_Capital_Lib.BalanceSum(Apt_Code);//잔액가져오기
                rnn = await repair_Plan_Lib.Getlist_Adjustment_Year(Apt_Code);

                bnnA = await review_Content_Lib.GetList_ReviewCode_Sort(ann.Plan_Review_Code.ToString(), Apt_Code, "3");
                bnnB = await review_Content_Lib.GetList_ReviewCode_Sort(ann.Plan_Review_Code.ToString(), Apt_Code, "4");
                bnnC = await review_Content_Lib.GetList_ReviewCode_Sort(ann.Plan_Review_Code.ToString(), Apt_Code, "5");
                bnnD = await review_Content_Lib.GetList_ReviewCode_Sort(ann.Plan_Review_Code.ToString(), Apt_Code, "6");
                bnnE = await review_Content_Lib.GetList_ReviewCode_Sort(ann.Plan_Review_Code.ToString(), Apt_Code, "7");
                bnnF = await review_Content_Lib.GetList_ReviewCode_Sort(ann.Plan_Review_Code.ToString(), Apt_Code, "8"); 
            }
        }

        /// <summary>
        /// 검토코드 선택 시 정보 불러오기
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        private async Task OnSelect(ChangeEventArgs a)
        {
            if (a.Value != null || a.Value.ToString() != "Z")
            {
                strReview_Code = a.Value.ToString();
                await DetailsView(strReview_Code);
            }
        }

        /// <summary>
        /// 조정년도 선택 시 실행
        /// </summary>
        private async Task onYear(ChangeEventArgs a)
        {
            if (a.Value != null || a.Value.ToString() != "Z")
            {
                strPlan_Year = a.Value.ToString();
                rnnA = await repair_Plan_Lib.GetList_Repair_Plan_Apt_Year(Apt_Code, strPlan_Year); 
            }
        }

        /// <summary>
        /// 장기수선계획 코드 선택 시 실행
        /// </summary>
        private async Task onCode(ChangeEventArgs a)
        {
            if (a.Value != null || a.Value.ToString() != "Z")
            {
                strPlan_Code = a.Value.ToString();
                prnn = await plan_Review_Lib.Review_Infor(strPlan_Code);
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
