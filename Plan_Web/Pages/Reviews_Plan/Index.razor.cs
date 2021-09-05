using Facility;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Plan_Apt_Lib;
using Plan_Blazor_Lib;
using Plan_Blazor_Lib.Article;
using Plan_Blazor_Lib.Plan;
using Plan_Blazor_Lib.Review;
using Plan_Lib;
using Plan_Lib.Pund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plan_Web.Pages.Reviews_Plan
{
    public partial class Index
    {
        #region 인스턴스

        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public NavigationManager MyNav { get; set; } // Url
        [Inject] private IJSRuntime JSRuntime { get; set; }
        [Inject] public IAptInfor_Lib aptInfor_Lib { get; set; }
        [Inject] public IPlan_Review_Lib plan_Review_Lib { get; set; }
        [Inject] public IReview_Content_Lib review_Content_Lib { get; set; }
        [Inject] public IFacility_Sort_Lib facility_Sort_Lib { get; set; }
        [Inject] public IRepair_Plan_Lib repair_Plan_Lib { get; set; }
        [Inject] public IRepair_Capital_Lib repair_Capital_Lib { get; set; }
        [Inject] public IUnit_Price_Lib unit_Price_Lib { get; set; }
        [Inject] public IBylaw_Lib bylaw_Lib { get; set; }
        [Inject] public IArticle_Lib article_Lib { get; set; }

        #endregion 인스턴스

        #region 속성

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
        private Bylaw_Entity bpn { get; set; } = new Bylaw_Entity();
        private Repair_Plan_Entity epn { get; set; } = new Repair_Plan_Entity();
        private List<Facility_Sort_Entity> fnnA { get; set; } = new List<Facility_Sort_Entity>();
        private List<Facility_Sort_Entity> fnnB { get; set; } = new List<Facility_Sort_Entity>();
        private List<Join_Article_Cycle_Cost_Entity> jacce { get; set; } = new List<Join_Article_Cycle_Cost_Entity>();//수선항목 목록
        private Join_Article_Cycle_Cost_Entity jacc { get; set; } = new Join_Article_Cycle_Cost_Entity(); //수선항목 상세
        private List<Repair_Plan_Entity> rnn { get; set; } = new List<Repair_Plan_Entity>(); //장기수선계획 조정년도 목록
        private List<Repair_Plan_Entity> rnnA { get; set; } = new List<Repair_Plan_Entity>(); //장기수선계획 코드 목록

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

        #endregion 변수

        #region 페이징

        /// <summary>
        /// 페이징 속성
        /// </summary>
        protected DulPager.DulPagerBase pager = new DulPager.DulPagerBase()
        {
            PageNumber = 1,
            PageIndex = 0,
            PageSize = 15,
            PagerButtonCount = 5
        };

        /// <summary>
        /// 페이징
        /// </summary>
        protected async void PageIndexChanged(int pageIndex)
        {
            pager.PageIndex = pageIndex;
            pager.PageNumber = pageIndex + 1;

            await DisplayViews();

            StateHasChanged();
        }

        #endregion 페이징

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
                await NewDataViews();

                await DisplayViews();
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인하지 않았습니다.");
                MyNav.NavigateTo("/");
            }
        }

        /// <summary>
        /// 처음 로딩
        /// </summary>
        /// <returns></returns>
        private async Task NewDataViews()
        {
            fnnA = await facility_Sort_Lib.GetList_A_FacilitySort(); //대분류 만들기
            dbBalance = await repair_Capital_Lib.BalanceSum(Apt_Code);//잔액가져오기
            //fnnB = new List<Facility_Sort_Entity>();
            //fnnC = new List<Facility_Sort_Entity>();
        }

        /// <summary>
        /// 데이터 뷰
        /// </summary>
        /// <returns></returns>
        private async Task DisplayViews()
        {
            pager.RecordCount = await plan_Review_Lib.GetList_Apt_Page_Count(Apt_Code);
            annA = await plan_Review_Lib.GetList_Apt_Page(pager.PageIndex, Apt_Code);
            StateHasChanged();
        }

        /// <summary>
        /// 입력된 검토 수선항목 수
        /// </summary>
        /// <param name="Plan_Review_Code"></param>
        /// <returns></returns>
        private int Review_Content_Count(string Plan_Review_Code)
        {
            return review_Content_Lib.Review_Content_Count(Apt_Code, Plan_Review_Code);
        }

        /// <summary>
        /// 총론 새로 입력 열기
        /// </summary>
        /// <returns></returns>
        private async Task btnOpen()
        {
            int intCount = await plan_Review_Lib.Non_Complete(Apt_Code);//미완료 존재여부 있으면 0보다 크고 그렇지 않으면 0;
            if (intCount < 1)
            {
                InsertViews = "B";
                NewOpen = "B";
                rnn = await repair_Plan_Lib.Getlist_Adjustment_Year(Apt_Code);
                ann = new Plan_Review_Entity();
                bnn = new Review_Content_Entity();
                ann.PlanReview_Date = DateTime.Now.Date;
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "완료되지 않은 검토가 있습니다. \n 검토를 생성하려면 먼저 기존 검토를 완료하여야 합니다. \n 미완료된 검토를 그대로 두고 새롭게 생성하는 것은 허용되지 않습니다.");
            }
        }

        /// <summary>
        /// 총론 수정 열기
        /// </summary>
        private void btnEidtOpenA()
        {
            InsertViews = "B"; // 수정열기
            //ViewsA = "A"; // 상세보기 닫기
        }

        /// <summary>
        /// 총론 상세보기 닫기
        /// </summary>
        private void btnViewsCloseA()
        {
            ViewsA = "A";
        }

        

        /// <summary>
        /// 총론검토 삭제
        /// </summary>
        private async Task OnRemove(int Aid)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{Aid}번 글을 정말로 삭제하시겠습니까? \n 만일 삭제할 경우에는 저장된 해당 검토의 수선항목 검토도 모두 삭제됩니다. \n 신중하게 판단하여 삭제하시기 바랍니다. \n \n한번 삭제되면 살릴 수 없습니다.");
            if (isDelete)
            {
                int BeingCount = await repair_Plan_Lib.Being_Plan_Review(Apt_Code, Aid.ToString());
                if (BeingCount < 1)
                {
                    await plan_Review_Lib.Remove_PlanReview(Aid.ToString());
                    await review_Content_Lib.Delete_Repair_Review_Content(Aid.ToString());
                    await DisplayViews();
                    ViewsA = "A";
                    ViewsB = "A";
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "이 검토를 토대로 장기수선계획이 조정되었으므로 삭제할 수 없습니다. . \n 이 검토를 삭제하려면 먼저 참조된 장기수선계획을 조정하여 삭제가 가능합니다.");
                }
            }
        }

        /// <summary>
        /// 상세보기
        /// </summary>
        private async Task OnById(int Aid)
        {
            await DetailsView(Aid);
            ViewsA = "B";
        }

        /// <summary>
        /// 상세보기 메서드
        /// </summary>
        private async Task DetailsView(int Aid)
        {
            ann = await plan_Review_Lib.Detail_PlanReview(Aid);
            epn = await repair_Plan_Lib.Detail_Repair_Plan(ann.Apt_Code, ann.Repair_Plan_Code);
            //dbBalance = await repair_Capital_Lib.BalanceSum(Apt_Code);//잔액가져오기
            int Bylaw_Code = await bylaw_Lib.Bylaw_Last_Code(Apt_Code);
            upsn = await unit_Price_Lib.Report_Plan_Cost_Bylaw(Apt_Code, ann.Repair_Plan_Code, Bylaw_Code.ToString());
            upn = await unit_Price_Lib.Detail_Unit_Price_New(Apt_Code, ann.Repair_Plan_Code, upsn.Levy_Rate, upsn.Levy_Period); //단가 관련 정보 가져오기

            bnnA = await review_Content_Lib.GetList_ReviewCode_Sort(ann.Plan_Review_Code.ToString(), Apt_Code, "3");
            bnnB = await review_Content_Lib.GetList_ReviewCode_Sort(ann.Plan_Review_Code.ToString(), Apt_Code, "4");
            bnnC = await review_Content_Lib.GetList_ReviewCode_Sort(ann.Plan_Review_Code.ToString(), Apt_Code, "5");
            bnnD = await review_Content_Lib.GetList_ReviewCode_Sort(ann.Plan_Review_Code.ToString(), Apt_Code, "6");
            bnnE = await review_Content_Lib.GetList_ReviewCode_Sort(ann.Plan_Review_Code.ToString(), Apt_Code, "7");
            bnnF = await review_Content_Lib.GetList_ReviewCode_Sort(ann.Plan_Review_Code.ToString(), Apt_Code, "8");
        }

        /// <summary>
        /// 수선항목 검토 수정열기
        /// </summary>
        private async Task OnByContentEdit(Review_Content_Join_Enity rev)
        {
            jacc = await article_Lib.Details_Repair_Article_Cycle_Sort_Review(rev.Repair_Plan_Code, rev.Repair_Article_Code, Apt_Code);

            bnn.Apt_Code = rev.Apt_Code;
            bnn.Repair_Article_Review = rev.Repair_Article_Review;
            bnn.Repair_Cycle_Review = rev.Repair_Cycle_Review;
            bnn.Repair_Cost_Review = rev.Repair_Cost_Review;
            bnn.Repair_Part_Rate_Review = rev.Repair_Part_Rate_Review;
            bnn.Review_Content = rev.Review_Content;
            bnn.Plan_Review_Code = rev.Plan_Review_Code;
            bnn.Plan_Review_Content_Code = rev.Plan_Review_Content_Code;

            InsertViewsB = "B";
        }

        /// <summary>
        /// 수선항목 검토 내용 삭제
        /// </summary>
        private async Task OnContentRemove(Review_Content_Join_Enity rev)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{rev.Plan_Review_Content_Code}의 수선항목을 정말로 삭제하시겠습니까?");
            if (isDelete)
            {
                await review_Content_Lib.Remove_Repair_Review_Content(rev.Plan_Review_Content_Code);
                int intCode = Convert.ToInt32(rev.Plan_Review_Code);
                await DetailsView(intCode);
            }
        }

        /// <summary>
        /// 수선항목 검토 존재 여부
        /// </summary>
        private int onComplete(string Repair_Article_Code, string Plan_Review_Code)
        {
            return review_Content_Lib.Being_Review_Content(Repair_Article_Code, Plan_Review_Code);
        }

        /// <summary>
        /// 총론 검토 등록 닫기
        /// </summary>
        private void btnClose()
        {
            InsertViews = "A";
        }

        /// <summary>
        /// 조정년도 선택 시 실행
        /// </summary>
        private async Task onYear(ChangeEventArgs a)
        {
            strPlan_Year = a.Value.ToString();
            rnnA = await repair_Plan_Lib.GetList_Repair_Plan_Apt_Year(Apt_Code, strPlan_Year);
        }

        /// <summary>
        /// 장기수선계획 코드 선택 시 실행
        /// </summary>
        private async Task onCode(ChangeEventArgs a)
        {
            epn = await repair_Plan_Lib.Detail_Repair_Plan(Apt_Code, a.Value.ToString());

            int Bylaw_Code = await bylaw_Lib.Bylaw_Last_Code(Apt_Code);
            upsn = await unit_Price_Lib.Report_Plan_Cost_Bylaw(Apt_Code, ann.Repair_Plan_Code, Bylaw_Code.ToString());
            upn = await unit_Price_Lib.Detail_Unit_Price_New(Apt_Code, epn.Repair_Plan_Code, upsn.Levy_Rate, upsn.Levy_Period); //단가 관련 정보 가져오기
        }

        /// <summary>
        /// 장기수선계획 검토 총론 저장
        /// </summary>
        private async Task btnSave()
        {
            ann.Repair_Plan_Code = epn.Repair_Plan_Code;


            if (ann.Levy_Sum == "" || ann.Levy_Sum == null || ann.Levy_Sum == "Z")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "부과금액 검토를 선택하지 않았습니다.");
            }
            else if (ann.Emergency_Expense == "" || ann.Emergency_Expense == null || ann.Emergency_Expense == "Z")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "소액지출 검토를 선택하지 않았습니다.");
            }
            else if (ann.PlanReview_Division == "" || ann.PlanReview_Division == null || ann.PlanReview_Division == "Z")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "검토구분을 선택하지 않았습니다.");
            }
            else if (ann.Plan_Period == "" || ann.Plan_Period == null || ann.Plan_Period == "Z")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "계획기간 검토를 선택하지 않았습니다.");
            }
            else if (ann.Plan_Reviewer == "" || ann.Plan_Reviewer == null || ann.Plan_Reviewer == "Z")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "검토자를 입력하지 않았습니다.");
            }
            else if (ann.Repair_Plan_Code == "" || ann.Repair_Plan_Code == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "계획코드를 입력하지 않았습니다.");
            }
            else if (ann.Saving_Cost == "" || ann.Saving_Cost == null || ann.Saving_Cost == "Z")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "잔액이 적정하지 않았습니다.");
            }
            else if (ann.SmallSum_Expense == "" || ann.SmallSum_Expense == null || ann.SmallSum_Expense == "Z")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "소액지출 검토를 선택하지 않았습니다.");
            }
            else
            {
                if (ann.Plan_Review_Code < 1)
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
                    ann.PostIP = myIPAddress;

                    #endregion 아이피 입력

                    ann.Apt_Code = Apt_Code;
                    ann.Staff_Code = User_Code;
                    ann.PlanReview_Ago = epn.Plan_Review_Date;
                    //ann.Saving_Cost = Convert.ToInt32(upsn.balance_sum.Replace(",", "")).ToString();
                    //ann.Levy_Sum = upsn.Unit_Price;
                    //ann.Plan_Period = epn.Plan_Period.ToString();

                    await plan_Review_Lib.Add_Plan_Review(ann);
                }
                else
                {
                    await plan_Review_Lib.Edit_Plan_Review(ann);
                }

                InsertViews = "A";
                await DisplayViews();
                ViewsA = "B";
                await DetailsView(ann.Plan_Review_Code);
            }
        }

        /// <summary>
        /// 수선항목 검토 열기
        /// </summary>
        private async Task btnContentOpen()
        {
            //fnnA = await facility_Sort_Lib.GetList_A_FacilitySort();
            jacce = await article_Lib.GetLIst_Repair_Article_Cycle_Review(Apt_Code, epn.Repair_Plan_Code);//수선항목 목록
            InsertViewsA = "B";
        }

        /// <summary>
        /// 대분류 선택 중분류 목록 만들기
        /// </summary>
        private async Task onSortA(ChangeEventArgs a)
        {
            strSortA = a.Value.ToString();
            fnnB = await facility_Sort_Lib.GetList_FacilitySortA(a.Value.ToString());
            strSortB = "Z";
            if (strSortA == null || strSortA == "" || strSortA == "Z")
            {
                jacce = await article_Lib.GetLIst_Repair_Article_Cycle_Review(Apt_Code, epn.Repair_Plan_Code);//수선항목 목록
            }
            else
            {
                jacce = await article_Lib.GetLIst_Repair_Article_Cycle_Sort_Review(epn.Repair_Plan_Code, "Sort_A_Code", strSortA, Apt_Code);
            }
        }

        /// <summary>
        /// 중분류 선택 하면 수선항목 만들기
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        private async Task onSortB(ChangeEventArgs a)
        {
            strSortB = a.Value.ToString();
            jacce = await article_Lib.GetLIst_Repair_Article_Cycle_Sort_Review(epn.Repair_Plan_Code, "Sort_B_code", strSortB, Apt_Code);
        }

        /// <summary>
        /// 수선항목 선택 시 실행
        /// </summary>
        /// <param name="an"></param>
        private void OnByContentId(Join_Article_Cycle_Cost_Entity an)
        {
            jacc = an;
            bnn = new Review_Content_Entity();

            if (an.Repair_Rate < 1)
            {
                bnn.Repair_Part_Rate_Review = "해당없음";
            }
            if (an.All_Cycle < 1)
            {
                bnn.Repair_Cycle_Review = "해당없음";
            }
        }

        /// <summary>
        /// 수선항목 검토 내용 저장
        /// </summary>
        /// <returns></returns>
        private async Task btnContentSave()
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



            if (bnn.Plan_Review_Content_Code < 1)
            {
                bnn.Apt_Code = Apt_Code;
                bnn.Plan_Review_Code = ann.Plan_Review_Code.ToString();
                bnn.Sort_A_Code = jacc.Sort_A_Code;
                bnn.Sort_B_Code = jacc.Sort_B_Code;
                bnn.Sort_C_Code = jacc.Sort_C_Code;
                bnn.Repair_Article_Code = jacc.Aid.ToString();
                bnn.Staff_Code = User_Code;

                await review_Content_Lib.Add_Review_Content(bnn);
                //InsertViewsA = "A";
            }
            else
            {
                bnn.Staff_Code = User_Code;
                await review_Content_Lib.Edit_Review_Content(bnn);
                InsertViewsB = "A";
            }

            await DetailsView(ann.Plan_Review_Code);

            jacc = new Join_Article_Cycle_Cost_Entity();
            bnn = new Review_Content_Entity();

        }

        /// <summary>
        /// 수선항목 검토 닫기
        /// </summary>
        private void btnCloseA()
        {
            InsertViewsA = "A";
        }

        /// <summary>
        /// 수선항목 검토 수정 닫기
        /// </summary>
        private void btnCloseB()
        {
            InsertViewsB = "A";
        }
    }
}
