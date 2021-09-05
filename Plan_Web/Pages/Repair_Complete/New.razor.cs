using Facility;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Plan_Apt_Lib;
using Plan_Blazor_Lib;
using Plan_Blazor_Lib.Article;
using Plan_Blazor_Lib.Cost;
using Plan_Blazor_Lib.Cycle;
using Plan_Blazor_Lib.Plan;
using Plan_Blazor_Lib.Price;
using Plan_Blazor_Lib.Review;
using Plan_Lib.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plan_Web.Pages.Repair_Complete
{
    public partial class New
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
        [Inject] public IFacility_Sort_Lib facility_Sort_Lib { get; set; }
        [Inject] public IReview_Content_Lib review_Content_Lib { get; set; }
        [Inject] public ILogView_Lib logView_Lib { get; set; }
        [Inject] public IRepair_Price_Lib repair_Price_Lib { get; set; }
        [Inject] public IPrice_Set_Lib price_Set_Lib { get; set; }
        [Inject] public IPrime_Cost_Report_Lib prime_Cost_Report_Lib { get; set; }
        [Inject] public IUnitPrice_Rate_Lib unitPrice_Rate_Lib { get; set; }
        [Inject] public IPlan_Prosess_Lib plan_Prosess_Lib { get; set; }
        //[Inject] public IAppropriation_Prosess_Lib appropriation_Prosess_Lib { get; set; }
        #endregion

        #region 목록 인스턴스
        Repair_Plan_Entity rpn { get; set; } = new Repair_Plan_Entity();
        Join_Article_Cycle_Cost_Entity art { get; set; } = new Join_Article_Cycle_Cost_Entity();
        Cost_Entity dnn { get; set; } = new Cost_Entity(); //수선금액을 입력하기 위한 수선금액 정보

        Plan_Prosess ppn { get; set; } = new Plan_Prosess(); //장기수선계획 진행 정보

        Review_Content_Join_Enity rce { get; set; } = new Review_Content_Join_Enity();

        Join_Article_Cycle_Cost_EntityA jace { get; set; } = new Join_Article_Cycle_Cost_EntityA();
        List<Join_Article_Cycle_Cost_Entity> rae { get; set; } = new List<Join_Article_Cycle_Cost_Entity>(); //입력 수선항목 목록 만들기
        List<Facility_Sort_Entity> fnnA { get; set; } = new List<Facility_Sort_Entity>();
        List<Repair_Price_Kind_Entity> rpk { get; set; } = new List<Repair_Price_Kind_Entity>();
        Price_Set_Entity pse { get; set; } = new Price_Set_Entity(); //단가 모음 정보
        UnitPrice_Rate_Entity dr { get; set; } = new UnitPrice_Rate_Entity(); //원가계산서 할증율 엔터티
        Prime_Cost_Report_Entity ps { get; set; } = new Prime_Cost_Report_Entity();//해당 수선금액의 단가(재료비, 노무비, 경비) 합계 구하기 목록
        #endregion

        #region 변수
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public string BuildDate { get; private set; }
        public string InsertViews { get; set; } = "A"; //수선금액 입력 모달
        public string InsertViewsA { get; set; } = "A"; //수선금액 수정 모달
        public string strSortA { get; set; }
        public string strSortB { get; set; }
        public string strTitle { get; set; }
        public long intPrice { get; set; }
        public string strCostA { get; set; } = "D"; //단가 기준 값
        public string CostViewsA { get; set; } = "A"; //단가 선택 시 모달 구현
        public string EditViews { get; set; } = "A"; // 수정 여부
        public string DetailViews { get; set; } = "A";
        public string CostViewsB { get; set; } = "A"; //일위단기 단가 선택 시 모달 구현

        #endregion

        #region Url 이동
        private void OnPlanUrl()
        {
            MyNav.NavigateTo("Repair_Plan/New");
        }

        private void OnArticleUrl()
        {
            MyNav.NavigateTo("Repair_Article/New/" + rpn.Repair_Plan_Code);
        }

        private void OnCycleUrl()
        {
            MyNav.NavigateTo("Repair_Cycle/New/" + rpn.Repair_Plan_Code);
        }

        private void OnCostUrl()
        {
            MyNav.NavigateTo("Repair_Cost/New/" + rpn.Repair_Plan_Code);
        }

        private void OnCompleteUrl()
        {
            MyNav.NavigateTo("Repair_Complete/New/" + rpn.Repair_Plan_Code);
        }
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

                rpn = await repair_Plan_Lib.Detail_Repair_Plan(Apt_Code, Aid);
                await DataViews();
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인하지 않았습니다.");
                MyNav.NavigateTo("/");
            }
        }

        private async Task DataViews()
        {
            int intBe = await plan_Prosess_Lib.Being_Complete(Apt_Code, rpn.Repair_Plan_Code);
            if (intBe > 0)
            {
                ppn = await plan_Prosess_Lib.Detail(Apt_Code, rpn.Repair_Plan_Code);
            }
            else
            {
                ppn.Apt_Code = Apt_Code;
                ppn.Repair_Plan_Code = rpn.Repair_Plan_Code;
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
                ppn.PostIP = myIPAddress;
                #endregion 아이피 입력
                await plan_Prosess_Lib.Add(ppn);
                ppn = await plan_Prosess_Lib.Detail(Apt_Code, rpn.Repair_Plan_Code);
            }

        }

        /// <summary>
        /// 완료로
        /// </summary>
        private async Task btnCompleteA()
        {
            await plan_Prosess_Lib.Edit_Complete_Select(Apt_Code, rpn.Repair_Plan_Code, "Plan_Complete", "B");
            await repair_Plan_Lib.Edit_Complete(rpn.Repair_Plan_Code, "B");
            await DataViews();
        }
        private async Task btnCompleteB()
        {
            await plan_Prosess_Lib.Edit_Complete_Select(Apt_Code, rpn.Repair_Plan_Code, "Article_Complete", "B");
            await DataViews();
        }
        private async Task btnCompleteC()
        {
            await plan_Prosess_Lib.Edit_Complete_Select(Apt_Code, rpn.Repair_Plan_Code, "Cycle_Complete", "B");
            await DataViews();
        }
        private async Task btnCompleteD()
        {
            await plan_Prosess_Lib.Edit_Complete_Select(Apt_Code, rpn.Repair_Plan_Code, "Cost_Complete", "B");
            await DataViews();
        }
        private async Task btnCompleteE()
        {
            //await appropriation_Prosess_Lib.Edit_Complete(Apt_Code, "장기수선충당금 초기화", "B");
            await DataViews();
        }
        private async Task btnCompleteF()
        {
            //await 
            await DataViews();
        }
        private async Task btnCompleteG()
        {
            await DataViews();
        }

        /// <summary>
        /// 진행중으로
        /// </summary>
        private async Task btnComplete_A()
        {
            await plan_Prosess_Lib.Edit_Complete_Select(Apt_Code, rpn.Repair_Plan_Code, "Plan_Complete", "A");
            await repair_Plan_Lib.Edit_Complete(rpn.Repair_Plan_Code, "A");
            
            await DataViews();
        }
        private async Task btnComplete_B()
        {
            await plan_Prosess_Lib.Edit_Complete_Select(Apt_Code, rpn.Repair_Plan_Code, "Article_Complete", "A");
            await DataViews();
        }
        private async Task btnComplete_C()
        {
            await plan_Prosess_Lib.Edit_Complete_Select(Apt_Code, rpn.Repair_Plan_Code, "Cycle_Complete", "A");
            await DataViews();
        }
        private async Task btnComplete_D()
        {
            await plan_Prosess_Lib.Edit_Complete_Select(Apt_Code, rpn.Repair_Plan_Code, "Cost_Complete", "A");
            await DataViews();
        }
        private async Task btnComplete_E()
        {
            await DataViews();
        }
        private async Task btnComplete_F()
        {
            await DataViews();
        }
        private async Task btnComplete_G()
        {
            await DataViews();
        }
    }
}
