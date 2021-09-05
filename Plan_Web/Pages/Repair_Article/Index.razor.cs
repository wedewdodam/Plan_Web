using Facility;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Plan_Apt_Lib;
using Plan_Blazor_Lib;
using Plan_Blazor_Lib.Article;
using Plan_Blazor_Lib.Plan;
using Plan_Blazor_Lib.Review;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plan_Web.Pages.Repair_Article
{
    public partial class Index
    {
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Parameter] public string Aid { get; set; }
        [Inject] public NavigationManager MyNav { get; set; } // Url
        [Inject] private IJSRuntime JSRuntime { get; set; }
        [Inject] public IAptInfor_Lib aptInfor_Lib { get; set; }
        //[Inject] public IPlan_Review_Lib plan_Review_Lib { get; set; }
        [Inject] public IRepair_Plan_Lib repair_Plan_Lib { get; set; }
        [Inject] public IArticle_Lib article_Lib { get; set; }
        [Inject] public IFacility_Sort_Lib facility_Sort_Lib { get; set; }
        [Inject] public IReview_Content_Lib review_Content_Lib { get; set; }

        private Repair_Plan_Entity rpn { get; set; } = new Repair_Plan_Entity();
        List<Article_Entity> annA { get; set; } = new List<Article_Entity>();
        List<Article_Entity> annB { get; set; } = new List<Article_Entity>();
        List<Article_Entity> annC { get; set; } = new List<Article_Entity>();
        List<Article_Entity> annD { get; set; } = new List<Article_Entity>();
        List<Article_Entity> annE { get; set; } = new List<Article_Entity>();
        List<Article_Entity> annF { get; set; } = new List<Article_Entity>();
        private List<Facility_Sort_Entity> fnnA { get; set; } = new List<Facility_Sort_Entity>();
        private List<Facility_Sort_Entity> fnnB { get; set; } = new List<Facility_Sort_Entity>();
        private List<Facility_Sort_Entity> fnn { get; set; } = new List<Facility_Sort_Entity>();
        Article_Entity bnn { get; set; } = new Article_Entity();
        Review_Content_Join_Enity rce { get; set; } = new Review_Content_Join_Enity();
        protected ElementReference myDiv;

        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public string BuildDate { get; private set; }
        public string InsertViews { get; set; } = "A";
        public string InsertViewsA { get; set; } = "A";
        public string InsertViewsB { get; set; } = "A";
        public string strSortA { get; set; }
        public string strSortB { get; set; }
        public string strTitle { get; set; }

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
            rpn = await repair_Plan_Lib.Detail_Repair_Plan(Apt_Code, Aid);
            annA = await article_Lib.GetLIst_RepairArticle_Sort(Aid, "Sort_A_Code", "3", Apt_Code);
            annB = await article_Lib.GetLIst_RepairArticle_Sort(Aid, "Sort_A_Code", "4", Apt_Code);
            annC = await article_Lib.GetLIst_RepairArticle_Sort(Aid, "Sort_A_Code", "5", Apt_Code);
            annD = await article_Lib.GetLIst_RepairArticle_Sort(Aid, "Sort_A_Code", "6", Apt_Code);
            annE = await article_Lib.GetLIst_RepairArticle_Sort(Aid, "Sort_A_Code", "7", Apt_Code);
            annF = await article_Lib.GetLIst_RepairArticle_Sort(Aid, "Sort_A_Code", "8", Apt_Code);
        }

        #region Url 이동
        private void OnPlanUrl()
        {
            MyNav.NavigateTo("Repair_Plan/" + rpn.Repair_Plan_Code);
        }

        private void OnArticleUrl()
        {
            //MyNav.NavigateTo("Repair_Article/" + rpn.Repair_Plan_Code);
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
        /// 수선항목 상세정보 열기
        /// </summary>
        /// <param name="ar"></param>
        private void OnByDetailA(Article_Entity ar)
        {
            bnn = ar;
        }

        /// <summary>
        /// 수선항목 수정 열기
        /// </summary>
        /// <param name="ar"></param>
        public int ReviewCount { get; set; } = 1;
        private async Task OnByEditA(Article_Entity ar)
        {
            bnn = ar;
            strInsYearA = bnn.Installation.Year.ToString();
            strInsYearB = bnn.Installation_Part.Year.ToString();
            strTitle = "수선항목 수정";
            try
            {
                rce = await review_Content_Lib.View_ReviewCode_ArticleName(rpn.Plan_Review_Code, Apt_Code, bnn.Repair_Article_Name);
                if (rce != null)
                {
                    ReviewCount = 1;
                }
                else
                {
                    ReviewCount = 0;
                }
            }
            catch (Exception)
            {
                ReviewCount = 0;
            }
            InsertViewsA = "B";
        }

        /// <summary>
        /// 수선항목 삭제
        /// </summary>
        /// <param name="Aid"></param>
        /// <returns></returns>
        private async Task OnRemoveA(int Aid)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{Aid}번 글을 정말로 삭제하시겠습니까?");
            if (isDelete)
            {
                await article_Lib.Remove_Repair_Article(Aid);
                await DetailsView();
            }
        }

        /// <summary>
        /// 수선항목 새로등록 열기
        /// </summary>
        private async Task btnOpen()
        {
            fnnA = await facility_Sort_Lib.GetList_A_FacilitySort(); //대분류 만들기
            fnn = await facility_Sort_Lib.GetList_Sort_AA_List(Apt_Code, Aid, "3");
            strTitle = "수선항목 추가 입력";

            bnn = new Article_Entity();
            InsertViews = "B";
        }


        /// <summary>
        /// 대분류 선택 중분류 목록 만들기
        /// </summary>
        private async Task onSortA(ChangeEventArgs a)
        {
            strSortA = a.Value.ToString();
            fnnB = await facility_Sort_Lib.GetList_FacilitySortA(a.Value.ToString());
            fnn = await facility_Sort_Lib.GetList_Sort_AA_List(Apt_Code, Aid, strSortA);
        }

        /// <summary>
        /// 중분류 선택 하면 수선항목 만들기
        /// </summary>
        private async Task onSortB(ChangeEventArgs a)
        {
            strSortB = a.Value.ToString();
            fnn = await facility_Sort_Lib.GetList_FacilitySort(Apt_Code, Aid, strSortB);
        }

        /// <summary>
        /// 수선 공사종별 선택 시 실행
        /// </summary>
        /// <param name="aq"></param>
        private void OnByArticleId(Facility_Sort_Entity aq)
        {
            bnn.All_Cycle = aq.Repair_Cycle;
            bnn.Part_Cycle = aq.Repair_Cycle_Part;
            bnn.Repair_Rate = aq.Repair_Rate;
            bnn.Sort_A_Code = aq.Sort_A_Code;
            bnn.Sort_A_Name = aq.Sort_A_Name;
            bnn.Sort_B_Code = aq.Sort_B_Code;
            bnn.Sort_B_Name = aq.Sort_B_Name;
            bnn.Sort_C_Code = aq.Aid.ToString();
            bnn.Sort_C_Name = aq.Sort_Name;
            bnn.Repair_Article_Name = aq.Sort_Name;

            #region 최종수선년도 관련 메서드
            int intY = rpn.Founding_Date.Year;// + Convert.ToInt32(bnn.All_Cycle);
            int intZ = rpn.Founding_Date.Year;// + 
            int intX = DateTime.Now.Year;
            int intA = Convert.ToInt32(bnn.All_Cycle);
            int intP = Convert.ToInt32(bnn.Part_Cycle);
            int intSa = 0;
            try
            {
                intSa = (intX - intY) / intA;
            }
            catch (Exception)
            {
                intSa = 0;
            }
            int intSp = 0;
            try
            {
                intSp = (intX - intY) / intP;
            }
            catch (Exception)
            {
                intSp = 0;
            }

            strInsYearA = (intY + (intA * intSa)).ToString();
            strInsYearB = (intY + (intP * intSp)).ToString();
            bnn.Installation = Convert.ToDateTime(strInsYearA + "-01-01");
            bnn.Installation_Part = Convert.ToDateTime(strInsYearB + "-01-01");
            #endregion
        }

        #region 최종수선년도
        /// <summary>
        /// 전체 최종수선년도 선택 시 실행
        /// </summary>
        public string strInsYearB { get; set; }
        public string strInsYearA { get; set; }
        private async Task OnYearA(ChangeEventArgs a)
        {
            strInsYearA = a.Value.ToString();
            bnn.Installation = Convert.ToDateTime(strInsYearA + "-01-01");
            int intA = rpn.Founding_Date.Year;
            int intB = bnn.Installation.Year;
            int intC = rpn.Plan_Period;

            int intD = intB - intA;
            int intE = (intA + intC) - intB;

            if (intD < 0)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "최종수선년도가 사용검사년도 보다 이전일 수는 없습니다. \n 사용검사년도로 지정됩니다. \n 다시 지정해 부세요.");
                bnn.Installation = rpn.Founding_Date.Date;
            }
            else if (intE < 0)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "최종수선년도가 장기수선계획년도 이후일 수는 없습니다. \n 사용검사년도로 지정됩니다. \n 다시 지정해 부세요.");
                bnn.Installation = rpn.Founding_Date.Date;
            }
            else
            {
                bnn.Installation = Convert.ToDateTime(strInsYearB + "-01-01");
            }

        }

        private async Task OnYearB(ChangeEventArgs a)
        {
            strInsYearB = a.Value.ToString();
            bnn.Installation_Part = Convert.ToDateTime(strInsYearB + "-01-01");
            int intA = rpn.Founding_Date.Year;
            int intB = bnn.Installation_Part.Year;
            int intC = rpn.Plan_Period;

            int intD = intB - intA;
            int intE = (intA + intC) - intB;

            if (intD < 0)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "최종수선년도가 사용검사년도 보다 이전일 수는 없습니다. \n 사용검사년도로 지정됩니다. \n 다시 지정해 부세요.");
                bnn.Installation_Part = rpn.Founding_Date.Date;
            }
            else if (intE < 0)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "최종수선년도가 장기수선계획년도 이후일 수는 없습니다. \n 사용검사년도로 지정됩니다. \n 다시 지정해 부세요.");
                bnn.Installation_Part = rpn.Founding_Date.Date;
            }
            else
            {
                bnn.Installation_Part = Convert.ToDateTime(strInsYearB + "-01-01");
            }
        }
        #endregion

        /// <summary>
        /// 수선항목 저장 및 수정
        /// </summary>
        /// <returns></returns>
        private async Task btnSave()
        {
            bnn.Apt_Code = Apt_Code;
            bnn.User_ID = User_Code;
            bnn.Repair_Plan_Code = Aid;
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

            int intA = rpn.Founding_Date.Year + rpn.Plan_Period;
            int intB = bnn.Installation.Year + Convert.ToInt32(bnn.All_Cycle);
            int intC = bnn.Installation_Part.Year + Convert.ToInt32(bnn.Part_Cycle);
            bnn.Facility_Code = "없음";
            if (bnn.Repair_Rate >= 1)
            {
                bnn.Division = "B";
            }
            else
            {
                bnn.Division = "A";
            }

            if (bnn.Sort_A_Code == null || bnn.Sort_A_Code == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "분류를 입력하지 않았습니다.");
            }
            else if (bnn.Repair_Article_Name == null || bnn.Repair_Article_Name == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "수선항목 이름을  입력하지 않았습니다.");
            }
            else if (bnn.Unit == null || bnn.Unit == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "수선항목 이름을  입력하지 않았습니다.");
            }
            else if (bnn.Apt_Code == null || bnn.Apt_Code == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인하지 않았습니다.");
            }
            else if (bnn.Repair_Plan_Code == null || bnn.Repair_Plan_Code == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인하지 않았습니다.");
            }
            else if (intB > intA)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "전체수선예정년도(최종전체수선년도 + 전체수선주기)가 \n 계획기간 종료년도(사업검사년도 + 계획기간)를 넘을 수는 없습니다. \n 전체수선주기를 수정해 주세요.");
            }
            else if (intC > intA)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "부분수선예정년도(최종부분수선년도 + 부분수선주기)가 \n 계획기간 종료년도(사업검사년도 + 계획기간)를 넘을 수는 없습니다. \n 전체수선주기를 수정해 주세요.");
            }
            else
            {
                if (bnn.Aid < 1)
                {
                    int intAA = await article_Lib.Being_Article(bnn.Repair_Plan_Code, bnn.Sort_C_Code, bnn.Repair_Article_Name);
                    if (intAA < 1)
                    {
                        await article_Lib.Add_RepairArticle(bnn);
                        InsertViews = "A";
                        InsertViewsA = "A";
                        await DetailsView();
                    }
                    else
                    {
                        //await article_Lib.Edit_RepairArticle(bnn);
                        await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "새롭게 추가하는 경우에는 수선항목 이름이 같을 수는 없습니다.");
                        await JSRuntime.InvokeVoidAsync("SetFocusToElement", myDiv);
                    }
                }
                else
                {
                    await article_Lib.Edit_RepairArticle(bnn);
                    InsertViews = "A";
                    InsertViewsA = "A";
                    await DetailsView();
                }
            }
        }

        /// <summary>
        /// 수선항목 입력 모달 닫기
        /// </summary>
        private void btnClose()
        {
            InsertViews = "A";
            InsertViewsA = "A";
        }

        /// <summary>
        /// 수선항목에 입력 시 실행
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        private async Task OnArticleName(ChangeEventArgs a)
        {
            rce = await review_Content_Lib.View_ReviewCode_ArticleName(rpn.Plan_Review_Code, Apt_Code, bnn.Repair_Article_Name);
        }
    }
}
