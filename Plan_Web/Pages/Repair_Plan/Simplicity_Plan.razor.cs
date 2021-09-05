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
using Plan_Lib;
using Plan_Lib.Pund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plan_Web.Pages.Repair_Plan
{
    public partial class Simplicity_Plan
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
        [Inject] public IFacility_Sort_Lib facility_Sort_Lib { get; set; }
        [Inject] public IRepair_Price_Lib repair_Price_Lib { get; set; }
        [Inject] public IPrice_Set_Lib price_Set_Lib { get; set; }
        [Inject] public IPrime_Cost_Report_Lib prime_Cost_Report_Lib { get; set; }
        [Inject] public IUnitPrice_Rate_Lib unitPrice_Rate_Lib { get; set; }
        #endregion 인스턴스

        #region 속성
        List<Repair_Plan_Entity> rpe { get; set; } = new List<Repair_Plan_Entity>();
        private Unit_Price_string_Entity upsn { get; set; } = new Unit_Price_string_Entity();
        Unit_Price_Entity upn { get; set; } = new Unit_Price_Entity();
        Join_Article_Cycle_Cost_Entity art1 { get; set; } = new Join_Article_Cycle_Cost_Entity();
        Repair_Plan_Entity pnn { get; set; } = new Repair_Plan_Entity();
        Article_Entity ann { get; set; } = new Article_Entity();
        Cycle_Entity bnn { get; set; } = new Cycle_Entity();
        Cost_Entity cnn { get; set; } = new Cost_Entity();
        Cost_Entity enn { get; set; } = new Cost_Entity(); //일위단가에 표시될 해당 수선항목의 수선금액 정보
        private Repair_Plan_Entity rpn { get; set; } = new Repair_Plan_Entity();
        private List<Facility_Sort_Entity> fnn { get; set; } = new List<Facility_Sort_Entity>();
        private List<Facility_Sort_Entity> fnnA { get; set; } = new List<Facility_Sort_Entity>();
        private List<Facility_Sort_Entity> fnnB { get; set; } = new List<Facility_Sort_Entity>();
        protected ElementReference myDiv;
        //List<Join_Article_Cycle_Cost_Entity> jacc { get; set; } = new List<Join_Article_Cycle_Cost_Entity>(); //입력 수선항목 목록 만들기
        List<Join_Article_Cycle_Cost_Entity> jacc { get; set; } = new List<Join_Article_Cycle_Cost_Entity>(); //입력 수선항목 목록 만들기
        Join_Article_Cycle_Cost_EntityA jace { get; set; } = new Join_Article_Cycle_Cost_EntityA();
        List<Article_Entity> atr { get; set; } = new List<Article_Entity>(); //입력 수선항목 목록 만들기
        List<Article_Entity> rae { get; set; } = new List<Article_Entity>(); //입력 수선항목 목록 만들기(수선주기)


        List<Repair_Price_Kind_Entity> rpk { get; set; } = new List<Repair_Price_Kind_Entity>();
        Price_Set_Entity pse { get; set; } = new Price_Set_Entity(); //단가 모음 정보
        UnitPrice_Rate_Entity dr { get; set; } = new UnitPrice_Rate_Entity(); //원가계산서 할증율 엔터티
        List<Price_Set_Entity> pseList { get; set; } = new List<Price_Set_Entity>();//단가모음 목록
        Prime_Cost_Report_Entity ps { get; set; } = new Prime_Cost_Report_Entity();//해당 수선금액의 단가(재료비, 노무비, 경비) 합계 구하기 목록
        UnitPrice_Rate_Entity ur { get; set; } = new UnitPrice_Rate_Entity(); //원가계산서 할증율 엔터티

        #endregion 

        #region 변수
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public string BuildDate { get; private set; }
        public long intPrice { get; set; }
        public string EditViews { get; set; } = "A"; // 수정 여부
        public string InsertViews { get; set; } = "A";
        public string InsertArticle { get; set; } = "A";
        public string InsertArticleA { get; set; } = "A";
        public string InsertCycle { get; set; } = "A";
        public string InsertCycleA { get; set; } = "A";
        public string InsertCost { get; set; } = "A";
        public string InsertCostA { get; set; } = "A";
        public int EditViews1 { get; set; } = 0;
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
        public string strCostA { get; set; } = "D"; //단가 기준 값
        public string CostViewsA { get; set; } = "A"; //단가 선택 시 모달 구현
        public string CostViewsB { get; set; } = "A"; //일위단기 단가 선택 시 모달 구현
        private int intCostBeing { get; set; } = 0;
        public string Plan_Review_Code { get; set; }
        public string strYear { get; set; }
        public string strYearA { get; set; }
        public int required { get; set; } = 0;

        private string strProduct;
        private double intAll_Cost;

        private int Personal_Price { get; set; } = 0;

        
        protected ElementReference myDivP;
        #endregion 변수

        #region 페이징
        /// <summary>
        /// 페이징 속성
        /// </summary>
        protected DulPager.DulPagerBase pager = new DulPager.DulPagerBase()
        {
            PageNumber = 1,
            PageIndex = 0,
            PageSize = 10,
            PagerButtonCount = 5
        };

        /// <summary>
        /// 페이징
        /// </summary>
        protected async void PageIndexChanged(int pageIndex)
        {
            pager.PageIndex = pageIndex;
            pager.PageNumber = pageIndex + 1;

            await DetailsView();

            StateHasChanged();
        }

        #endregion 페이징

        #region 메인화면 관련
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

                int Plan_Being = await repair_Plan_Lib.BeComplete_Count(Apt_Code);

                if (Plan_Being > 0)
                {
                    await DetailsView();
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "최초 수립 혹은 최초 입력이 완료되어야 간편 수시조정이 가능합니다. \n 먼저 최초 수립 혹은 최초입력을 완료한 후에 시도해 주세요.");
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

            pager.RecordCount = await repair_Plan_Lib.GetList_Repair_Plan_Apt_Any_Count(Apt_Code);
            rpe = await repair_Plan_Lib.GetList_Repair_Plan_Apt_Any(pager.PageIndex, Apt_Code); // 첫 장기계획 상세 정보
        }

        /// <summary>
        /// 전체수선항목 수
        /// </summary>
        private string Article_All(string Repair_Plan_Code)
        {

            int intAll = cost_Lib.Repair_Cost_Article_All_Count(Apt_Code, Repair_Plan_Code, "Repair_All_Cost");
            return intAll.ToString();
        }

        /// <summary>
        /// 전체수선항목 수
        /// </summary>
        private string Article_Part(string Repair_Plan_Code)
        {
            int intAll = cost_Lib.Repair_Cost_Article_All_Count(Apt_Code, Repair_Plan_Code, "Repair_Part_Cost");
            return intAll.ToString();
        }


        /// <summary>
        /// 전체수선항목 수
        /// </summary>
        private string Cost_All(string Repair_Plan_Code)
        {
            double dbAll = cost_Lib.Cost_All_Total(Apt_Code, Repair_Plan_Code);
            return string.Format("{0: ###,###}", dbAll);
        }

        /// <summary>
        /// 전체수선항목 수
        /// </summary>
        private string Cost_Part(string Repair_Plan_Code)
        {
            double dbPart = cost_Lib.Cost_Part_Total(Apt_Code, Repair_Plan_Code);
            return string.Format("{0: ###,###}", dbPart);
        }

        /// <summary>
        /// 상세로 이동
        /// </summary>
        private void OnSelect(string Aid)
        {
            MyNav.NavigateTo("/Repair_Plan/Simplicity_Details/" + Aid);
        }
        #endregion

        #region 총론 관련
        public string strCode { get; set; }

        /// <summary>
        /// 새로 입력 열기
        /// </summary>
        private async Task btnNews()
        {
            rpn = await repair_Plan_Lib.Detail_Apt_Ferst_Plan(Apt_Code); // 첫 장기계획 상세 정보
            rpn.Adjustment_Num = await repair_Plan_Lib.Being_Count_Any(Apt_Code);
            pnn.Adjustment_Date = DateTime.Now;
            pnn.Repair_Plan_Etc = rpn.Repair_Plan_Etc;
            pnn.Adjustment_Man = rpn.Adjustment_Man;
            InsertViews = "B";
            strTitle = "간편 수시 조정 관리";
        }

        /// <summary>
        /// 저장(계획 총론)
        /// </summary>
        private async Task btnSave()
        {
            #region 장기수선계획 입력
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
            pnn.PostIP = myIPAddress;


            #endregion 아이피 입력
            pnn.AnyTime = "B";
            pnn.User_ID = User_Code;
            pnn.Apt_Code = Apt_Code;

            pnn.Adjustment_Year = rpn.Adjustment_Date.Year.ToString();
            pnn.Adjustment_Month = rpn.Adjustment_Date.Month.ToString();
            pnn.Adjustment_Division = "간편조정";
            pnn.AnyTime = "B";
            pnn.Emergency_Basis = "간편조정";
            pnn.Plan_Review_Code = "간편조정";
            pnn.Plan_Review_Date = DateTime.Now;
            pnn.SmallSum_Unit = rpn.SmallSum_Unit;
            pnn.SmallSum_Basis = rpn.SmallSum_Basis;
            pnn.Small_Sum = rpn.Small_Sum;
            pnn.SmallSum_Requirement = rpn.SmallSum_Requirement;
            pnn.Plan_Period = rpn.Plan_Period;
            pnn.LastAdjustment_Date = rpn.Adjustment_Date;
            pnn.Founding_Date = rpn.Founding_Date;
            pnn.Founding_man = rpn.Founding_man;
            string count = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString();

            pnn.Repair_Plan_Code = Apt_Code + (await repair_Plan_Lib.Last_Aid());

            if (pnn.Aid < 1)
            {
                int A = await repair_Plan_Lib.Add_Repair_Plan_Any(pnn);
            }
            else
            {
                await repair_Plan_Lib.Edit_Repair_Plan(pnn);
            }
            #endregion

            rpn = pnn;
            //rpn.Aid = await repair_Plan_Lib.Last_Aid_Apt(Apt_Code);
            pnn = new Repair_Plan_Entity();

            // 저장 후 수선항목으로 이동 전 실행
            Aid = rpn.Repair_Plan_Code;
            strCode = Aid;
            fnnA = await facility_Sort_Lib.GetList_A_FacilitySort(); //대분류 만들기
            fnnB = await facility_Sort_Lib.GetList_Sort_AA_List(Apt_Code, Aid, "3");
            fnn = await facility_Sort_Lib.GetList_Sort_AA_List(Apt_Code, Aid, "3");

            strTitle = "수선항목 추가 입력";
            ann = new Article_Entity();

            InsertArticle = "B"; // 수선항목 관리 열기
        }

        /// <summary>
        /// 입력닫기
        /// </summary>
        private void btnClose()
        {
            InsertViews = "A";
        }

        /// <summary>
        /// 수선항목 추가
        /// </summary>
        private async Task OnbtnAdd(string Aid)
        {
            strCode = Aid;
            Aid = strCode;
            rpn = await repair_Plan_Lib.Detail_Repair_Plan(Apt_Code, Aid);

            fnnA = await facility_Sort_Lib.GetList_A_FacilitySort(); //대분류 만들기
            //fnnB = await facility_Sort_Lib.GetList_FacilitySortA(a.Value.ToString());
            fnnB = await facility_Sort_Lib.GetList_Sort_AA_List(Apt_Code, Aid, "3");
            fnn = fnnB;//await facility_Sort_Lib.GetList_Sort_AA_List(Apt_Code, Aid, "3");

            strTitle = "수선항목 추가 입력";
            ann = new Article_Entity();

            InsertArticle = "B"; // 수선항목 관리 열기
        }
        #endregion

        #region 수선항목 관련
        /// <summary>
        /// 대분류 선택 중분류 목록 만들기
        /// </summary>
        private async Task onSortA(ChangeEventArgs a)
        {
            strSortA = a.Value.ToString();
            fnnB = await facility_Sort_Lib.GetList_FacilitySortA(a.Value.ToString());
            fnn = await facility_Sort_Lib.GetList_Sort_AA_List(Apt_Code, strCode, strSortA);
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
            ann.All_Cycle = aq.Repair_Cycle;
            ann.Part_Cycle = aq.Repair_Cycle_Part;
            ann.Repair_Rate = aq.Repair_Rate;
            ann.Sort_A_Code = aq.Sort_A_Code;
            ann.Sort_A_Name = aq.Sort_A_Name;
            ann.Sort_B_Code = aq.Sort_B_Code;
            ann.Sort_B_Name = aq.Sort_B_Name;
            ann.Sort_C_Code = aq.Aid.ToString();
            ann.Sort_C_Name = aq.Sort_Name;
            ann.Repair_Article_Name = aq.Sort_Name;

            #region 최종수선년도 관련 메서드
            int intY = rpn.Founding_Date.Year;// + Convert.ToInt32(bnn.All_Cycle);
            int intZ = rpn.Founding_Date.Year;// + 
            int intX = DateTime.Now.Year;
            int intA = Convert.ToInt32(ann.All_Cycle);
            int intP = Convert.ToInt32(ann.Part_Cycle);
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
            ann.Installation = Convert.ToDateTime(strInsYearA + "-01-01");
            ann.Installation_Part = Convert.ToDateTime(strInsYearB + "-01-01");
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
            ann.Installation = Convert.ToDateTime(strInsYearA + "-01-01");
            int intA = rpn.Founding_Date.Year;
            int intB = ann.Installation.Year;
            int intC = rpn.Plan_Period;

            int intD = intB - intA;
            int intE = (intA + intC) - intB;

            if (intD < 0)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "최종수선년도가 사용검사년도 보다 이전일 수는 없습니다. \n 사용검사년도로 지정됩니다. \n 다시 지정해 부세요.");
                ann.Installation = rpn.Founding_Date.Date;
            }
            else if (intE < 0)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "최종수선년도가 장기수선계획년도 이후일 수는 없습니다. \n 사용검사년도로 지정됩니다. \n 다시 지정해 부세요.");
                ann.Installation = rpn.Founding_Date.Date;
            }
            else
            {
                ann.Installation = Convert.ToDateTime(strInsYearB + "-01-01");
            }

        }

        private async Task OnYearB(ChangeEventArgs a)
        {
            strInsYearB = a.Value.ToString();
            ann.Installation_Part = Convert.ToDateTime(strInsYearB + "-01-01");
            int intA = rpn.Founding_Date.Year;
            int intB = ann.Installation_Part.Year;
            int intC = rpn.Plan_Period;

            int intD = intB - intA;
            int intE = (intA + intC) - intB;

            if (intD < 0)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "최종수선년도가 사용검사년도 보다 이전일 수는 없습니다. \n 사용검사년도로 지정됩니다. \n 다시 지정해 부세요.");
                ann.Installation_Part = rpn.Founding_Date.Date;
            }
            else if (intE < 0)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "최종수선년도가 장기수선계획년도 이후일 수는 없습니다. \n 사용검사년도로 지정됩니다. \n 다시 지정해 부세요.");
                ann.Installation_Part = rpn.Founding_Date.Date;
            }
            else
            {
                ann.Installation_Part = Convert.ToDateTime(strInsYearB + "-01-01");
            }
        }
        #endregion

        /// <summary>
        /// 수선항목 저장
        /// </summary>
        private async Task btnSaveArticle()
        {
            ann.Apt_Code = Apt_Code;
            ann.User_ID = User_Code;
            ann.Repair_Plan_Code = rpn.Repair_Plan_Code;
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

            int intA = rpn.Founding_Date.Year + rpn.Plan_Period;
            int intB = ann.Installation.Year + Convert.ToInt32(ann.All_Cycle);
            int intC = ann.Installation_Part.Year + Convert.ToInt32(ann.Part_Cycle);
            ann.Facility_Code = "없음";
            if (ann.Repair_Rate >= 1)
            {
                ann.Division = "B";
            }
            else
            {
                ann.Division = "A";
            }

            if (ann.Sort_A_Code == null || ann.Sort_A_Code == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "분류를 입력하지 않았습니다.");
            }
            else if (ann.Repair_Article_Name == null || ann.Repair_Article_Name == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "수선항목 이름을  입력하지 않았습니다.");
            }
            else if (ann.Unit == null || ann.Unit == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "수량의 단위를 선택하지 않았습니다.");
            }
            else if (ann.Apt_Code == null || ann.Apt_Code == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인하지 않았습니다.");
            }
            else if (ann.Repair_Plan_Code == null || ann.Repair_Plan_Code == "")
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
                if (ann.Aid < 1)
                {
                    int intAA = await article_Lib.Being_Article(ann.Repair_Plan_Code, ann.Sort_C_Code, ann.Repair_Article_Name);
                    if (intAA < 1)
                    {
                        int topCount = await article_Lib.Article_Count(ann.Repair_Plan_Code);
                        if (topCount < 8)
                        {
                            await article_Lib.Add_RepairArticle(ann);
                            //InsertArticle = "A";
                            fnn = await facility_Sort_Lib.GetList_Sort_AA_List(Apt_Code, ann.Repair_Plan_Code, "3");
                            ann = new Article_Entity();
                            InsertArticleA = "A";
                            await DetailsView();
                        }
                        else
                        {
                            await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "간편 조정은 수선항목을 7개 이하만 입력할 수 있습니다. \n 추가로 더 입력하려면 메뉴 [2-3 장기수선계획 검토]를 완료하시고, [2-4 장기수선계획 조정]에서 조정하시기 바랍니다.  \n 7개 이상의 수선항목 조정은 계획기간 중 수선비 총액에 많은 영향을 미치게 되기 때문입니다.");
                        }
                    }
                    else
                    {
                        await article_Lib.Edit_RepairArticle(ann);
                        //fnn = await facility_Sort_Lib.GetList_Sort_AA_List(Apt_Code, Aid, "3");
                        await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "새롭게 추가하는 경우에는 수선항목 이름이 같을 수는 없습니다.");
                        await JSRuntime.InvokeVoidAsync("SetFocusToElement", myDiv);
                    }
                }
                else
                {
                    await article_Lib.Edit_RepairArticle(ann);
                    InsertArticleA = "A";
                    await DetailsView();
                }
                
            }
        }

        /// <summary>
        /// 수선항목 관리 닫기
        /// </summary>
        private async Task btnMoveCycle()
        {
            fnnA = await facility_Sort_Lib.GetList_A_FacilitySort(); //대분류 만들기
            rae = await article_Lib.GetLIst_RepairArticle_Sort(rpn.Repair_Plan_Code, "Sort_A_Code", "3", Apt_Code); //수선항목 목록 만들기
            strTitle = "수선주시 새로 등록";
            InsertArticle = "A";
            strSortA = "3";
            InsertCycle = "B";
        }


        #endregion

        #region 수선주기 관련
        public int intAllCount { get; set; } // 전체수선주기 수
        public int intPartCount { get; private set; }
        public int intPartAllCount { get; set; } //총 부분수선주기 수
        Article_Entity art { get; set; } = new Article_Entity();
        /// <summary>
        /// 대분류 선택 중분류 목록 만들기
        /// </summary>
        private async Task onSortAB(ChangeEventArgs a)
        {
            strSortA = a.Value.ToString();
            fnnB = await facility_Sort_Lib.GetList_FacilitySortA(a.Value.ToString());
            rae = await article_Lib.GetLIst_RepairArticle_Sort(rpn.Repair_Plan_Code, "Sort_A_Code", strSortA, Apt_Code); //수선항목 목록 만들기;
        }

        /// <summary>
        /// 중분류 선택 하면 수선항목 만들기
        /// </summary>
        private async Task onSortBB(ChangeEventArgs a)
        {
            strSortB = a.Value.ToString();
            rae = await article_Lib.GetLIst_RepairArticle_Sort(rpn.Repair_Plan_Code, "Sort_B_Code", strSortB, Apt_Code); //수선항목 목록 만들기;
        }


        /// <summary>
        /// 수선항목 선택 시 실행
        /// </summary>
        private async Task OnBySelectAsync(Article_Entity ae)
        {
            art = ae;

            bnn.Set_Repair_Cycle_All = ae.All_Cycle;
            bnn.Set_Repair_Cycle_Part = ae.Part_Cycle;
            bnn.Repair_Rate = Convert.ToInt32(ae.Repair_Rate);
            var fac = await facility_Sort_Lib.Detail_Sort(ae.Sort_C_Code);
            bnn.Law_Repair_Cycle_All = fac.Repair_Cycle;
            bnn.Law_Repair_Cycle_Part = fac.Repair_Cycle_Part;
            bnn.Law_Repair_Rate = fac.Repair_Rate;

            Cycle_all();
            if (art.Part_Cycle > 0)
            {
                Cycle_PartA();
            }
            else
            {
                intPartAllCount = 0;
                intPartCount = 0;
            }
            //intAllCount = 12;
            //intPartCount = 5;
        }

        /// <summary>
        /// 부분 수선주기 횟수 계산
        /// </summary>
        public int Part_Cycle_Schedule { get; set; } = 0;

        private void Cycle_PartA()
        {
            int Now_Year = DateTime.Now.Year;
            int intNest = 0;
            try
            {
                intNest = ((rpn.Founding_Date.Year + rpn.Plan_Period) - (art.Installation.Year + bnn.Set_Repair_Cycle_All) - ((intAllCount - 1) * art.All_Cycle)) / art.Part_Cycle;
            }
            catch (Exception)
            {
                intNest = 0;
            }
            if (intAllCount > 0 && art.Part_Cycle > 0)
            {
                if (Now_Year == bnn.Repair_Plan_Year_All)
                {
                    int Y = (art.All_Cycle % art.Part_Cycle); //전체 수선주기가 부분수선주기로 나눈 나머지
                    if (Y > 0)
                    {
                        Part_Cycle_Schedule = Now_Year;
                        intPartCount = art.All_Cycle / art.Part_Cycle;
                        intPartAllCount = intPartCount + ((intAllCount - 2) * (art.All_Cycle / art.Part_Cycle)) + intNest;
                        bnn.Repair_Plan_Year_Part = bnn.Repair_Plan_Year_All + art.Part_Cycle;
                        bnn.Set_Repair_Cycle_Part = art.Part_Cycle;
                    }
                    else if (Y == 0)
                    {
                        Part_Cycle_Schedule = Now_Year;
                        intPartCount = (art.All_Cycle / art.Part_Cycle) - 1;
                        intPartAllCount = intPartCount + ((intAllCount - 2) * ((art.All_Cycle / art.Part_Cycle) - 1)) + intNest;
                        bnn.Repair_Plan_Year_Part = bnn.Repair_Plan_Year_All + art.Part_Cycle;
                        bnn.Set_Repair_Cycle_Part = art.Part_Cycle;
                    }
                }
                else if (Now_Year < bnn.Repair_Plan_Year_All)
                {
                    int Y = Now_Year - (art.Installation_Part.Year + art.Part_Cycle);
                    if (Y >= 0)
                    {
                        bnn.Repair_Plan_Year_Part = Now_Year;
                        bnn.Set_Repair_Cycle_Part = Now_Year - art.Installation_Part.Year;
                        Part_Cycle_Schedule = art.Installation_Part.Year;
                        intPartCount = ((bnn.Repair_Plan_Year_All - (art.Installation_Part.Year + bnn.Set_Repair_Cycle_Part)) / art.Part_Cycle) + 1;
                        int q = art.Installation_Part.Year + bnn.Set_Repair_Cycle_Part + ((intPartCount - 1) * art.Part_Cycle);
                        if (q >= bnn.Repair_Plan_Year_All)
                        {
                            intPartCount = intPartCount - 1;
                        }
                        intPartAllCount = intPartCount + ((intAllCount - 1) * ((art.All_Cycle / art.Part_Cycle) - 1)) + intNest;
                    }
                    else if (Y < 0)
                    {
                        bnn.Repair_Plan_Year_Part = art.Installation_Part.Year + art.Part_Cycle;
                        bnn.Set_Repair_Cycle_Part = art.Part_Cycle;
                        Part_Cycle_Schedule = art.Installation_Part.Year;

                        int X = ((bnn.Repair_Plan_Year_All - bnn.Repair_Plan_Year_Part) / art.Part_Cycle);
                        int Z = ((bnn.Repair_Plan_Year_All - bnn.Repair_Plan_Year_Part) / art.Part_Cycle);

                        if ((X == 0) && (Z > 0))
                        {
                            intPartCount = (Z - 1) + 1;
                        }
                        else
                        {
                            intPartCount = Z + 1;
                        }

                        int q = art.Installation_Part.Year + bnn.Set_Repair_Cycle_Part + ((intPartCount - 1) * art.Part_Cycle);
                        if (q >= bnn.Repair_Plan_Year_All)
                        {
                            intPartCount = intPartCount - 1;
                        }

                        intPartAllCount = intPartCount + ((intAllCount - 1) * ((art.All_Cycle / art.Part_Cycle) - 1)) + intNest;
                    }
                    else
                    {
                        bnn.Repair_Plan_Year_Part = 0;
                        bnn.Set_Repair_Cycle_Part = 0;
                        Part_Cycle_Schedule = 0;
                        intPartCount = 0;
                        intPartAllCount = 0;
                    }
                }
                else
                {
                    bnn.Repair_Plan_Year_Part = 0;
                    bnn.Set_Repair_Cycle_Part = 0;
                    Part_Cycle_Schedule = 0;
                    intPartCount = 0;
                    intPartAllCount = 0;
                }
            }
            else if (intAllCount == 0 && art.Part_Cycle > 0)
            {
                int Y = Now_Year - (art.Installation_Part.Year + art.Part_Cycle);
                if (Y >= 0)
                {
                    bnn.Repair_Plan_Year_Part = Now_Year;
                    bnn.Set_Repair_Cycle_Part = Now_Year - art.Installation_Part.Year;
                    Part_Cycle_Schedule = art.Installation_Part.Year;
                    intPartCount = (((rpn.Founding_Date.Year + rpn.Plan_Period) - bnn.Repair_Plan_Year_Part) / art.Part_Cycle) + 1;
                    intPartAllCount = (((rpn.Founding_Date.Year + rpn.Plan_Period) - bnn.Repair_Plan_Year_Part) / art.Part_Cycle) + 1;
                }
                else if (Y < 0)
                {
                    bnn.Repair_Plan_Year_Part = art.Installation_Part.Year + art.Part_Cycle;
                    bnn.Set_Repair_Cycle_Part = art.Part_Cycle;
                    Part_Cycle_Schedule = art.Installation_Part.Year;
                    intPartCount = (((rpn.Founding_Date.Year + rpn.Plan_Period) - bnn.Repair_Plan_Year_Part) / art.Part_Cycle) + 1;
                    intPartAllCount = (((rpn.Founding_Date.Year + rpn.Plan_Period) - bnn.Repair_Plan_Year_Part) / art.Part_Cycle) + 1;
                }
                else
                {
                    bnn.Repair_Plan_Year_Part = 0;
                    bnn.Set_Repair_Cycle_Part = 0;
                    Part_Cycle_Schedule = 0;
                    intPartCount = 0;
                    intPartAllCount = 0;
                }
            }
            else
            {
                bnn.Repair_Plan_Year_Part = 0;
                bnn.Set_Repair_Cycle_Part = 0;
                Part_Cycle_Schedule = 0;
                intPartCount = 0;
                intPartAllCount = 0;
            }

        }

        private void Cycle_PartB()
        {
            int Now_Year = DateTime.Now.Year;
            int intNest = ((rpn.Founding_Date.Year + rpn.Plan_Period) - (art.Installation.Year + bnn.Set_Repair_Cycle_All) - ((intAllCount - 1) * art.All_Cycle)) / art.Part_Cycle;
            if (intAllCount > 0 && art.Part_Cycle > 0)
            {
                if (Now_Year == bnn.Repair_Plan_Year_All)
                {
                    int Y = (art.All_Cycle % art.Part_Cycle); //전체 수선주기가 부분수선주기로 나눈 나머지
                    if (Y > 0)
                    {
                        Part_Cycle_Schedule = Now_Year;

                        bnn.Repair_Plan_Year_Part = bnn.Repair_Plan_Year_All + bnn.Set_Repair_Cycle_Part; //전체수선예정년도에 입력된 부분수선주기를 더한 부분수선예정도

                        intPartCount = ((bnn.Repair_Plan_Year_All + art.All_Cycle - bnn.Repair_Plan_Year_Part) / art.Part_Cycle) + 1;

                        int Z = (bnn.Repair_Plan_Year_All + bnn.Set_Repair_Cycle_Part) + ((intPartCount - 1) * art.Part_Cycle) - (bnn.Repair_Plan_Year_All + art.All_Cycle);
                        if (Z >= 0)
                        {
                            intPartCount = intPartCount - 1;
                        }

                        intPartAllCount = intPartCount + ((intAllCount - 2) * (art.All_Cycle / art.Part_Cycle)) + intNest;
                        bnn.Repair_Plan_Year_Part = bnn.Repair_Plan_Year_All + art.Part_Cycle;
                        //bnn.Set_Repair_Cycle_Part = art.Part_Cycle;
                    }
                    else if (Y == 0)
                    {
                        Part_Cycle_Schedule = Now_Year;

                        bnn.Repair_Plan_Year_Part = bnn.Repair_Plan_Year_All + bnn.Set_Repair_Cycle_Part; //전체수선예정년도에 입력된 부분수선주기를 더한 부분수선예정도
                        intPartCount = ((bnn.Repair_Plan_Year_All + art.All_Cycle - bnn.Repair_Plan_Year_Part) / art.Part_Cycle);

                        int Z = (bnn.Repair_Plan_Year_All + bnn.Set_Repair_Cycle_Part) + ((intPartCount - 1) * art.Part_Cycle) - (bnn.Repair_Plan_Year_All + art.All_Cycle);
                        if (Z >= 0)
                        {
                            intPartCount = intPartCount - 1;
                        }

                        intPartAllCount = intPartCount + ((intAllCount - 2) * ((art.All_Cycle / art.Part_Cycle) - 1)) + intNest;
                        bnn.Repair_Plan_Year_Part = bnn.Repair_Plan_Year_All + art.Part_Cycle;
                        //bnn.Set_Repair_Cycle_Part = art.Part_Cycle;
                    }
                }
                else if (Now_Year < bnn.Repair_Plan_Year_All)
                {
                    int Y = Now_Year - (art.Installation_Part.Year + bnn.Set_Repair_Cycle_Part);
                    if (Y >= 0)
                    {
                        bnn.Repair_Plan_Year_Part = art.Installation_Part.Year + bnn.Set_Repair_Cycle_Part;
                        bnn.Set_Repair_Cycle_Part = Now_Year - art.Installation_Part.Year;
                        Part_Cycle_Schedule = art.Installation_Part.Year;
                        intPartCount = ((bnn.Repair_Plan_Year_All - (art.Installation_Part.Year + bnn.Set_Repair_Cycle_Part)) / art.Part_Cycle) + 1;
                        int q = art.Installation_Part.Year + bnn.Set_Repair_Cycle_Part + ((intPartCount - 1) * art.Part_Cycle);
                        if (q >= bnn.Repair_Plan_Year_All)
                        {
                            intPartCount = intPartCount - 1;
                        }
                        intPartAllCount = intPartCount + ((intAllCount - 1) * ((art.All_Cycle / art.Part_Cycle) - 1)) + intNest;
                    }
                    else if (Y < 0)
                    {
                        bnn.Repair_Plan_Year_Part = art.Installation_Part.Year + bnn.Set_Repair_Cycle_Part;
                        //bnn.Set_Repair_Cycle_Part = art.Part_Cycle;
                        Part_Cycle_Schedule = art.Installation_Part.Year;


                        int W = bnn.Repair_Plan_Year_All - bnn.Repair_Plan_Year_Part; //전체수선예정년도에서 부분수선예전년도를 뺀 수

                        if (W > 0)
                        {
                            Part_Cycle_Schedule = art.Installation_Part.Year;
                            intPartCount = ((bnn.Repair_Plan_Year_All - (art.Installation_Part.Year + bnn.Set_Repair_Cycle_Part)) / art.Part_Cycle) + 1;
                            int q = art.Installation_Part.Year + bnn.Set_Repair_Cycle_Part + ((intPartCount - 1) * art.Part_Cycle);
                            if (q >= bnn.Repair_Plan_Year_All)
                            {
                                intPartCount = intPartCount - 1;
                            }
                            intPartAllCount = intPartCount + ((intAllCount - 1) * ((art.All_Cycle / art.Part_Cycle) - 1)) + intNest;
                            //bnn.Set_Repair_Cycle_Part = art.Part_Cycle;
                        }
                        else
                        {
                            int w = bnn.Repair_Plan_Year_All - (art.Installation_Part.Year + bnn.Set_Repair_Cycle_Part);
                            if (w <= 0)
                            {
                                Part_Cycle_Schedule = bnn.Repair_Plan_Year_All;
                                intPartCount = art.All_Cycle / art.Part_Cycle;
                                int intSubCount = 0;
                                if (((intAllCount - 2) * ((art.All_Cycle / art.Part_Cycle) - 1)) < 0)
                                {
                                    intSubCount = 0;
                                }
                                else
                                {
                                    intSubCount = ((intAllCount - 2) * ((art.All_Cycle / art.Part_Cycle) - 1));
                                }
                                bnn.Repair_Plan_Year_Part = bnn.Law_Repair_Cycle_All + art.Part_Cycle;
                                bnn.Set_Repair_Cycle_Part = art.Part_Cycle;
                                if ((intAllCount - 1) <= 0)
                                {
                                    intNest = 0;
                                }
                                else
                                {
                                    intNest = ((rpn.Founding_Date.Year + rpn.Plan_Period) - (art.Installation.Year + bnn.Set_Repair_Cycle_All) - ((intAllCount - 1) * art.All_Cycle)) / art.Part_Cycle;
                                }

                                intPartAllCount = intPartCount + intSubCount + intNest;
                            }
                            else
                            {
                                Part_Cycle_Schedule = bnn.Repair_Plan_Year_All;
                                intPartCount = art.All_Cycle / art.Part_Cycle;
                                intPartAllCount = intPartCount + ((intAllCount - 2) * ((art.All_Cycle / art.Part_Cycle) - 1)) + intNest;
                                bnn.Repair_Plan_Year_Part = bnn.Repair_Plan_Year_All + art.Part_Cycle;
                                bnn.Set_Repair_Cycle_Part = art.Part_Cycle;
                            }
                        }


                        //int X = ((bnn.Repair_Plan_Year_All - bnn.Repair_Plan_Year_Part) / art.Part_Cycle);


                        //if ((X == 0) && (X > 0))
                        //{
                        //    intPartCount = (X - 1) + 1;
                        //}
                        //else
                        //{
                        //    intPartCount = X + 1;
                        //}

                        //int q = art.Installation_Part.Year + bnn.Set_Repair_Cycle_Part + ((intPartCount - 1) * art.Part_Cycle);
                        //if (q >= bnn.Repair_Plan_Year_All)
                        //{
                        //    intPartCount = intPartCount - 1;
                        //}

                        //intPartAllCount = intPartCount + ((intAllCount - 1) * ((art.All_Cycle / art.Part_Cycle) - 1)) + intNest;
                    }
                    else
                    {
                        bnn.Repair_Plan_Year_Part = 0;
                        bnn.Set_Repair_Cycle_Part = 0;
                        Part_Cycle_Schedule = 0;
                        intPartCount = 0;
                        intPartAllCount = 0;
                    }
                }
                else
                {
                    bnn.Repair_Plan_Year_Part = 0;
                    bnn.Set_Repair_Cycle_Part = 0;
                    Part_Cycle_Schedule = 0;
                    intPartCount = 0;
                    intPartAllCount = 0;
                }
            }
            else if (intAllCount == 0 && art.Part_Cycle > 0)
            {
                int Y = Now_Year - (art.Installation_Part.Year + bnn.Set_Repair_Cycle_Part);
                if (Y >= 0)
                {
                    bnn.Repair_Plan_Year_Part = Now_Year;
                    bnn.Set_Repair_Cycle_Part = Now_Year - art.Installation_Part.Year;
                    Part_Cycle_Schedule = art.Installation_Part.Year;
                    intPartCount = (((rpn.Founding_Date.Year + rpn.Plan_Period) - bnn.Repair_Plan_Year_Part) / art.Part_Cycle) + 1;
                    intPartAllCount = (((rpn.Founding_Date.Year + rpn.Plan_Period) - bnn.Repair_Plan_Year_Part) / art.Part_Cycle) + 1;
                }
                else if (Y < 0)
                {
                    bnn.Repair_Plan_Year_Part = art.Installation_Part.Year + bnn.Set_Repair_Cycle_Part;
                    //bnn.Set_Repair_Cycle_Part = art.Part_Cycle;
                    Part_Cycle_Schedule = art.Installation_Part.Year;
                    intPartCount = (((rpn.Founding_Date.Year + rpn.Plan_Period) - bnn.Repair_Plan_Year_Part) / art.Part_Cycle) + 1;
                    intPartAllCount = (((rpn.Founding_Date.Year + rpn.Plan_Period) - bnn.Repair_Plan_Year_Part) / art.Part_Cycle) + 1;
                }
                else
                {
                    bnn.Repair_Plan_Year_Part = 0;
                    bnn.Set_Repair_Cycle_Part = 0;
                    Part_Cycle_Schedule = 0;
                    intPartCount = 0;
                    intPartAllCount = 0;
                }
            }
            else
            {
                bnn.Repair_Plan_Year_Part = 0;
                bnn.Set_Repair_Cycle_Part = 0;
                Part_Cycle_Schedule = 0;
                intPartCount = 0;
                intPartAllCount = 0;
            }

        }

        private void Cycle_Part()
        {
            int intRepairYear = art.Installation.Year; // 전체 최종수선 년도
            int intRepairYearPart = art.Installation_Part.Year; // 부분 최종수선 년도
            int intNowYear = Convert.ToInt32(DateTime.Now.Year);
            int now_year = DateTime.Now.Year;
            int av = (art.Installation.Year + bnn.Set_Repair_Cycle_All) - now_year; // 전체수선 수선예정년도에서 현재 년도를 뺀 수
            int dvp = now_year - art.Installation_Part.Year; //현재년도에서 최종 부분수선년도를 뺀 수
            //bnn.Set_Repair_Cycle_Part = dvp;
            int dva = now_year - art.Installation.Year; // 현재년도에서 최종 전체수선년도를 뺀 수
            int cycle = 0;
            int inst_AP = 0;
            int To = rpn.Founding_Date.Year + rpn.Plan_Period; // 사용검사년도에 계획기간을 더한 값

            int Za = intRepairYear + bnn.Set_Repair_Cycle_All;
            int Zp = intRepairYearPart + bnn.Set_Repair_Cycle_Part;

            if ((art.All_Cycle > art.Part_Cycle) && (art.Part_Cycle > 0))
            {
                #region MyRegion
                if (dvp > dva)
                {
                    cycle = dva;
                    inst_AP = intRepairYear;
                }
                else if (dvp < dva)
                {
                    cycle = dvp;
                    inst_AP = intRepairYearPart;
                }
                else
                {
                    cycle = dva;
                    inst_AP = intRepairYear;
                }
                #endregion

                if (av >= art.Part_Cycle)//전체 수선예정년도가 현재 년도 보다 크고, 그리고 그 큰수가 부분수선년도 보다 같거나 큰 경우
                {
                    if (cycle >= bnn.Set_Repair_Cycle_Part) //최종수선년도를 현재년도에서 뺀 수가 부분수선년도 보다 큰 경우(부분수선을 하지 않은 경우)(cycle >= art.Part_Cycle)
                    {
                        int pp = (bnn.Repair_Plan_Year_All - intNowYear) / art.Part_Cycle;
                        if (pp >= 1)
                        {
                            intPartCount = (av / art.Part_Cycle);
                        }
                        else
                        {
                            intPartCount = (av / art.Part_Cycle) + 1;
                        }

                        bnn.Set_Repair_Cycle_Part = intNowYear - inst_AP;
                    }
                    else if (cycle < art.Part_Cycle)
                    {
                        av = (art.Installation.Year + bnn.Set_Repair_Cycle_All) - inst_AP;
                        if ((av % art.Part_Cycle) == 0)
                        {
                            intPartCount = (av / art.Part_Cycle) - 1;
                        }
                        else
                        {
                            intPartCount = (av / art.Part_Cycle);
                        }
                        bnn.Set_Repair_Cycle_Part = art.Part_Cycle;
                    }
                    else
                    {
                        intPartCount = 0;
                    }
                    Part_Cycle_Schedule = art.Installation_Part.Year;

                    //int LastYear = (Part_Cycle_Schedule + (art.Part_Cycle * (intPartCount - 1))); //첫 전체주기의 부분 수선주기 수
                    //int Nest = intRepairYearPart + bnn.Set_Repair_Cycle_All;
                    //if (Nest < LastYear)
                    //{

                    //}
                }
                else if (av >= 2 && av < art.Part_Cycle)
                {
                    intPartCount = 1;

                    if (cycle < art.Part_Cycle)
                    {
                        bnn.Set_Repair_Cycle_Part = intNowYear - inst_AP;
                    }
                    else
                    {
                        intPartCount = (art.All_Cycle / art.Part_Cycle) - 1;
                        bnn.Set_Repair_Cycle_Part = intNowYear - inst_AP;
                    }

                    Part_Cycle_Schedule = art.Installation_Part.Year;
                }
                else
                {
                    int intBB = (art.All_Cycle % art.Part_Cycle);

                    if (intBB > 0)
                    {
                        intBB = (art.All_Cycle / art.Part_Cycle);
                    }
                    else
                    {
                        intBB = (art.All_Cycle / art.Part_Cycle) - 1;
                    }

                    intPartCount = 1;
                    if (intBB < 1)
                    {
                        intPartCount = 1;
                    }
                    else
                    {
                        intPartCount = intBB;
                    }

                    //부분주기 설정 주기 만들기
                    if (intPartCount == 1)
                    {
                        bnn.Set_Repair_Cycle_Part = intNowYear - inst_AP;
                    }

                    Part_Cycle_Schedule = art.Installation.Year + bnn.Set_Repair_Cycle_All;

                }

                #region 부분수선주기 총계
                int intNest = (To - (art.Installation.Year + bnn.Set_Repair_Cycle_All) - ((intAllCount - 1) * art.All_Cycle)) / art.Part_Cycle;//전체수선주기 해당년도를 제외한 나머지년도에서 부분수선 주기 횟수계산
                if ((intAllCount > 0) && (art.Part_Cycle > 0))
                {
                    int Z = (art.All_Cycle % art.Part_Cycle);
                    if (Z > 0)
                    {
                        if (av >= bnn.Set_Repair_Cycle_Part)
                        {
                            intPartAllCount = ((art.All_Cycle / art.Part_Cycle) * (intAllCount - 1)) + intPartCount;
                        }
                        else
                        {
                            intPartAllCount = ((art.All_Cycle / art.Part_Cycle) * (intAllCount - 1));
                        }
                    }
                    else
                    {
                        if (av >= bnn.Set_Repair_Cycle_Part) //첫 전체주기에 부분수선주기가 있는 경우
                        {
                            intPartAllCount = (((art.All_Cycle / art.Part_Cycle) - 1) * (intAllCount - 1) + intPartCount);
                        }
                        else
                        {
                            intPartAllCount = (((art.All_Cycle / art.Part_Cycle) - 1) * (intAllCount - 1));
                        }
                    }
                    intPartAllCount = intPartAllCount + intNest;
                }
                else if (intAllCount == 0 && art.Part_Cycle > 0)
                {
                    int z = (bnn.Set_Repair_Cycle_Part + art.Installation_Part.Year);//최종수선년도에 첫주기를 더한 값
                    if (dvp > art.Part_Cycle)
                    {

                        intPartAllCount = 1 + ((To - z) / art.Part_Cycle);
                    }
                    else
                    {
                        intPartAllCount = (To - art.Installation_Part.Year) / art.Part_Cycle;
                    }

                    Part_Cycle_Schedule = art.Installation_Part.Year;
                }
                else
                {
                    intPartAllCount = 0;
                }
                #endregion

            }
            else if ((art.All_Cycle < 1) && (art.Part_Cycle >= 1))
            {
                int Part_inst = art.Installation_Part.Year;
                int Pa = Part_inst + bnn.Set_Repair_Cycle_Part; // 부분 수선 최종 수선일에 부분 수선기간을 더한 값
                intRepairYear = Part_inst; // 부분선 최종 수선년도 대입


                if (Pa <= intNowYear)
                {
                    intPartCount = ((To - intNowYear) / art.Part_Cycle) + 1;
                    bnn.Set_Repair_Cycle_Part = intNowYear - inst_AP;
                }
                else if (Pa > intNowYear)
                {
                    intPartCount = ((To - Pa) / art.Part_Cycle) + 1;
                    bnn.Set_Repair_Cycle_Part = Pa - Part_inst;
                }
                else
                {
                    //intPartCount = 0;
                }

                Part_Cycle_Schedule = art.Installation_Part.Year;

                #region 부분수선주기 총계
                if ((intAllCount > 0) && (art.Part_Cycle > 0))
                {
                    int Z = (art.All_Cycle % art.Part_Cycle);
                    if (Z > 0)
                    {
                        intPartAllCount = ((art.All_Cycle / art.Part_Cycle) * (intAllCount - 1)) + intPartCount;
                    }
                    else
                    {
                        intPartAllCount = (((art.All_Cycle / art.Part_Cycle) - 1) * (intAllCount - 1)) + intPartCount;
                    }
                }
                else if (intAllCount == 0 && art.Part_Cycle > 0)
                {
                    int z = (bnn.Set_Repair_Cycle_Part + art.Installation_Part.Year);//최종수선년도에 첫주기를 더한 값
                    if ((now_year - art.Installation_Part.Year) >= bnn.Set_Repair_Cycle_Part)
                    {
                        intPartAllCount = 1 + ((To - z) / art.Part_Cycle);
                    }
                    else
                    {
                        intPartAllCount = ((To - art.Installation_Part.Year) / art.Part_Cycle);
                    }
                }
                else
                {
                    intPartAllCount = 0;
                }
                #endregion
            }
            else
            {
                intPartCount = 0;

                #region 부분수선주기 총계
                if ((intAllCount > 0) && (art.Part_Cycle > 0))
                {
                    int Z = (art.All_Cycle % art.Part_Cycle);
                    if (Z > 0)
                    {
                        intPartAllCount = ((art.All_Cycle / art.Part_Cycle) * (intAllCount - 1)) + intPartCount;
                    }
                    else
                    {
                        intPartAllCount = (((art.All_Cycle / art.Part_Cycle) - 1) * (intAllCount - 1)) + intPartCount;
                    }
                }
                else if (intAllCount == 0 && art.Part_Cycle > 0)
                {
                    int z = (bnn.Set_Repair_Cycle_Part + art.Installation_Part.Year);//최종수선년도에 첫주기를 더한 값
                    if (dvp > art.Part_Cycle)
                    {

                        intPartAllCount = 1 + ((To - z) / art.Part_Cycle);
                    }
                    else
                    {
                        intPartAllCount = (To - art.Installation_Part.Year) / art.Part_Cycle;
                    }
                }
                else
                {
                    intPartAllCount = 0;
                }
                #endregion
            }
            bnn.Repair_Plan_Year_Part = art.Installation_Part.Year + bnn.Set_Repair_Cycle_Part;
        }

        /// <summary>
        /// 전체수선주기 함수
        /// </summary>
        /// <returns></returns>
        private void Cycle_all()
        {
            int intInsA = art.Installation.Year; //전체최종수선년도
            int intInsP = art.Installation_Part.Year;//부분최종수선년도
            int intNowYear = DateTime.Now.Year;
            int intBuildYear = rpn.Founding_Date.Year; //사용검사년도
            int ListYear = intBuildYear + rpn.Plan_Period; //장기수선 종료년도
            int intRestYear = ListYear - intInsA; //종료년도로부터 전체 최종수선년도을 뺀 남은 년도

            //if (art.All_Cycle >= 1 && intRestYear >= 1)
            //{
            //    intAllCount = intRestYear / art.All_Cycle;
            //}
            //else
            //{
            //    intAllCount = 0;
            //}

            // 전체 수선주기 횟수
            if (((intNowYear - intInsA) >= bnn.Set_Repair_Cycle_All) && (bnn.Set_Repair_Cycle_All > 0))//현재 년도에서 최종수선년도를 뺀 수를 전체주기보다 큰 경우
            {
                int rest = intNowYear - intInsA; //현재 년도에서 최종수선년도를 뺀 년도 수
                bnn.Set_Repair_Cycle_All = rest; // ??
                rest = intRestYear - rest; // 종료년도로부터 전체 최종수선년도을 뺀 남은 년도에서 현재년도에서 최종수선년도를 뺀 수를 다시 뺀 수

                try
                {
                    intAllCount = (rest / art.All_Cycle) + 1;
                }
                catch (Exception)
                {
                    intAllCount = 0;
                }
            }
            else if (((intNowYear - intInsA) < bnn.Set_Repair_Cycle_All) && art.All_Cycle > 0)
            {
                int Z = intInsA + bnn.Set_Repair_Cycle_All; //최종 수선년도에 첫 전체수선주기를 더한 값

                intAllCount = ((ListYear - Z) / art.All_Cycle) + 1;

                int rest = (intInsA + art.All_Cycle) - (intNowYear - intInsA);
            }
            else
            {
                intAllCount = 0;
            }

            int LastYear = (intInsA + bnn.Set_Repair_Cycle_All + (art.All_Cycle * (intAllCount - 1)));

            if (intAllCount <= 15)
            {
                if (ListYear < LastYear)
                {
                    intAllCount = intAllCount - 1;
                }
            }
            else
            {
                intAllCount = 15;
                if (ListYear < LastYear)
                {
                    intAllCount = intAllCount - 1;
                }
            }

            bnn.Repair_Plan_Year_All = art.Installation.Year + bnn.Set_Repair_Cycle_All;
        }

        /// <summary>
        /// 전체 설정주기 수정 시에 실행
        /// </summary>
        /// <param name="a"></param>
        private async Task OnChageingAll(ChangeEventArgs a)
        {
            int Y = Convert.ToInt32(a.Value);
            int Z = (art.Installation.Year + Y);

            if ((Z - DateTime.Now.Year) >= 0)
            {
                bnn.Set_Repair_Cycle_All = Y;
                bnn.Repair_Plan_Year_All = art.Installation.Year + bnn.Set_Repair_Cycle_All;
                Cycle_all();
                Cycle_PartA();
            }
            else
            {
                bnn.Set_Repair_Cycle_All = bnn.Repair_Plan_Year_All - art.Installation.Year;

                await JSRuntime.InvokeVoidAsync("SetFocusToElement", myDiv);

                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "수선예정년도가 현재 년도 보다 작을 수는 없습니다. \n 수선예정년도는 금년이거나 미래이여야지 과거 일 수는 없기 때문입니다.");

            }
            //StateHasChanged();
        }

        //protected ElementReference myDivP;

        /// <summary>
        /// 부분 설정주기 수정 시에 실행
        /// </summary>
        /// <param name="a"></param>
        private async Task OnChageingPart(ChangeEventArgs a)
        {
            bnn.Set_Repair_Cycle_Part = Convert.ToInt32(a.Value);
            bnn.Repair_Plan_Year_Part = art.Installation_Part.Year + bnn.Set_Repair_Cycle_Part;

            if (bnn.Repair_Plan_Year_All > DateTime.Now.Year)
            {
                int Z = (art.Installation_Part.Year + bnn.Set_Repair_Cycle_Part);

                if ((Z - DateTime.Now.Year) >= 0)
                {
                    Cycle_PartB();
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "수선예정년도가 현재 년도 보다 작을 수는 없습니다. \n 수선예정년도는 금년이거나 미래이여야지 과거 일 수는 없기 때문입니다.");
                    await JSRuntime.InvokeVoidAsync("SetFocusToElement", myDivP);
                }
            }
            else
            {
                int Z = (bnn.Repair_Plan_Year_All + bnn.Set_Repair_Cycle_Part);


                if ((Z - DateTime.Now.Year) >= 0)
                {
                    bnn.Set_Repair_Cycle_Part = Convert.ToInt32(a.Value);
                    Cycle_PartB();
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "수선예정년도가 현재 년도 보다 작을 수는 없습니다. \n 수선예정년도는 금년이거나 미래이여야지 과거 일 수는 없기 때문입니다.");
                    await JSRuntime.InvokeVoidAsync("SetFocusToElement", myDivP);
                }
            }
        }


        /// <summary>
        /// 수선주기 저장
        /// </summary>
        private async Task btnSaveCycle()
        {
            if (bnn.Set_Repair_Cycle_All == 0 && bnn.Set_Repair_Cycle_Part == 0)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "수선주기가 입력되지 않았습니다.");
                InsertCycleA = "A";
                InsertCycle = "A";
            }
            else
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
                bnn.Post_IP = myIPAddress;

                #endregion 아이피 입력
                bnn.Apt_Code = Apt_Code;
                bnn.User_ID = User_Code;
                bnn.Repair_Plan_Code = art.Repair_Plan_Code;
                bnn.Sort_A_Code = art.Sort_A_Code;
                bnn.Sort_A_Name = art.Sort_A_Name;
                bnn.Sort_B_Code = art.Sort_B_Code;
                bnn.Sort_B_Name = art.Sort_B_Name;
                bnn.Sort_C_Code = art.Sort_C_Code;
                bnn.Sort_C_Name = art.Sort_C_Name;

                if (intAllCount < 1)
                {
                    //bnn.Law_Repair_Cycle_All = art.All_Cycle;
                    bnn.Repair_Last_Year_All = 0;
                    bnn.Repair_Plan_Year_All = 0;
                    bnn.Set_Repair_Cycle_All = 0;

                }
                else
                {
                    //bnn.Law_Repair_Cycle_All = art.All_Cycle;
                    bnn.Repair_Last_Year_All = art.Installation.Year;
                    bnn.Repair_Plan_Year_All = (art.Installation.Year + bnn.Set_Repair_Cycle_All);
                }

                if (intPartAllCount < 1)
                {
                    //bnn.Law_Repair_Cycle_Part = art.Part_Cycle;
                    bnn.Law_Repair_Rate = Convert.ToInt32(art.Repair_Rate);
                    bnn.Set_Repair_Rate = 0;
                    bnn.Set_Repair_Cycle_Part = 0;
                    bnn.Repair_Last_Year_Part = 0;
                    bnn.Repair_Plan_Year_Part = 0;
                }
                else
                {
                    //bnn.Law_Repair_Cycle_Part = art.Part_Cycle;
                    //bnn.Law_Repair_Rate = Convert.ToInt32(art.Repair_Rate);
                    bnn.Set_Repair_Rate = Convert.ToInt32(art.Repair_Rate);
                    bnn.Repair_Last_Year_Part = art.Installation_Part.Year;
                    bnn.Repair_Plan_Year_Part = (art.Installation_Part.Year + bnn.Set_Repair_Cycle_Part);
                }

                if (intAllCount > 0 && intPartCount > 0)
                {
                    bnn.Division = "A";
                }
                else if (intAllCount > 0 && intPartCount < 1)
                {
                    bnn.Division = "B";
                }
                else
                {
                    bnn.Division = "C";
                }
                bnn.All_Cycle_Num = intAllCount;
                bnn.Part_Cycle_Num = intPartAllCount;
                bnn.Repair_Article_Code = art.Aid.ToString();



                if (bnn.Aid < 1)
                {


                    int c = await cycle_Lib.be_cycle_code(bnn.Repair_Article_Code);

                    if (c < 1)
                    {
                        await cycle_Lib.Add_RepairCycle(bnn);
                        await article_Lib.Update_Cycle_Count_Add(Convert.ToInt32(bnn.Repair_Article_Code));//수선주기 추가
                        rae = await article_Lib.GetLIst_RepairArticle_Sort(rpn.Repair_Plan_Code, "Sort_A_Code", bnn.Sort_A_Code, Apt_Code);
                        art = new Article_Entity();
                        bnn = new Cycle_Entity();
                        intPartCount = 0;
                        intPartAllCount = 0;
                        intAllCount = 0;
                        //Cycle_all();
                        //Cycle_Part();
                    }
                    else
                    {
                        await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "이미 입력되었습니다.");
                    }
                }
                else
                {
                    await cycle_Lib.Edit_RepairCycle(bnn);
                    InsertCycleA = "A";
                    intPartCount = 0;
                    intPartAllCount = 0;
                    intAllCount = 0;
                }
                await DetailsView();
            }


            


        }

        /// <summary>
        /// 수선금액 관리로 이동
        /// </summary>
        /// <returns></returns>
        private async Task btnMoveCost()
        {
            fnnA = await facility_Sort_Lib.GetList_A_FacilitySort(); //대분류 만들기 GetLIst_Repair_Article_Cycle_Sort
            strSortA = "3";
            jacc = await article_Lib.GetList_Repair_Article_Cycle_Sort(strCode, "Sort_A_Code", "3", Apt_Code); //수선항목 목록 만들기
            strTitle = "수선금액 새로 등록";
            InsertCost = "B";
            cnn.Cost_Etc = "실재 공사를 실행한 공사금액 전체를 기준으로 수선금액을 선출함.";
            cnn.Sort_A_Name = "선택 안됨";
            art1.Repair_Article_Name = "-";
            strCostA = "D";
            CostViewsA = "D";
        }

        #endregion

        #region 수선금액 관련    

        /// <summary>
        /// 대분류 선택 중분류 목록 만들기
        /// </summary>
        private async Task onSortCostA(ChangeEventArgs a)
        {
            strSortA = a.Value.ToString();
            fnnB = await facility_Sort_Lib.GetList_FacilitySortA(a.Value.ToString());
            strSortB = "";
            jacc = await article_Lib.GetList_Repair_Article_Cycle_Sort(rpn.Repair_Plan_Code, "Sort_A_Code", strSortA, Apt_Code); //수선항목 목록 만들기;
        }

        /// <summary>
        /// 중분류 선택 하면 수선항목 만들기
        /// </summary>
        private async Task onSortCostB(ChangeEventArgs a)
        {
            strSortB = a.Value.ToString();
            jacc = await article_Lib.GetList_Repair_Article_Cycle_Sort(rpn.Repair_Plan_Code, "Sort_B_Code", strSortB, Apt_Code); //수선항목 목록 만들기;
        }

        /// <summary>
        /// 수선금액 새로 저장 또는 수정
        /// </summary>
        private async Task btnSaveCost()
        {

            if (cnn.Repair_All_Cost == 0)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "전체 수선금액이 입력되지 않았습니다.");
            }
            else
            {
                if (cnn.Repair_Cost_Code < 1)
                {
                    cnn.Apt_Code = Apt_Code;
                    cnn.Staff_Code = User_Code;
                    cnn.Repair_Plan_Code = art1.Repair_Plan_Code;
                    cnn.Sort_A_Code = art1.Sort_A_Code;
                    cnn.Sort_A_Name = art1.Sort_A_Name;
                    cnn.Sort_B_Code = art1.Sort_B_Code;
                    cnn.Sort_B_Name = art1.Sort_B_Name;
                    cnn.Sort_C_Code = art1.Sort_C_Code;
                    cnn.Sort_C_Name = art1.Sort_C_Name;
                    cnn.Repair_Rate = art1.Repair_Rate;
                    //dnn.Repair_Cost_Code = art.Repair_Cost_Code;
                    //dnn.Repair_Rate = art.Repair_Rate;
                    //dnn.Repair_Amount = art.Amount;
                    cnn.Price_Sort = "A";
                    cnn.Repair_Article_Code = art1.Repair_Article_Code;

                    int c = await cost_Lib.Being_Article_Cost(cnn.Apt_Code, cnn.Repair_Plan_Code, cnn.Repair_Article_Code);

                    if (c < 1)
                    {
                        try
                        {
                            cnn = await cost_Lib.Add_Cost(cnn);
                            if (cnn != null)
                            {
                                await article_Lib.Update_Cost_Count_Add(Convert.ToInt32(cnn.Repair_Article_Code));//수선주기 추가
                                art1 = new Join_Article_Cycle_Cost_Entity();
                                cnn = new Cost_Entity();
                                jacc = await article_Lib.GetList_Repair_Article_Cycle_Sort(Aid, "Sort_A_Code", strSortA, Apt_Code); //수선항목 목록 만들기 
                            }
                            else
                            {
                                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "저장되지 않았습니다.");
                            }
                        }
                        catch (Exception)
                        {
                            await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "저장되지 않았습니다.");
                        }
                        //await DetailsView();
                    }
                    else
                    {
                        await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "이미 입력되었습니다.");
                    }
                }
                else if (cnn.Repair_Cost_Code > 0)
                {
                    await cost_Lib.Update_Cost(cnn);
                    await article_Lib.Update_Cost_Count_Add(Convert.ToInt32(cnn.Repair_Article_Code));//수선주기 추가
                    await DetailsView();
                    InsertViews = "A";
                }
            }
        }

        /// <summary>
        /// 수선항목 선택 시 실행
        /// </summary>
        private async Task OnBySelectCostAsync(Join_Article_Cycle_Cost_Entity ae)
        {
            cnn = new Cost_Entity();
            art1 = ae;
            cnn.Repair_All_Cost = 0;
            cnn.Repair_Part_Cost = 0;
            intPrice = 0;
            int intCount = await cost_Lib.beCost(Apt_Code, ae.Repair_Article_Name);//수선항목 명으로 존재 여부 확인
            if (intCount > 0)
            {
                var ago_Cost = await cost_Lib._Cost(Apt_Code, ae.Repair_Article_Name);
                cnn.Repair_All_Cost = ago_Cost.Repair_All_Cost;
                cnn.Repair_Part_Cost = Convert.ToInt64(cnn.Repair_All_Cost * (art1.Repair_Rate / 100));
                intPrice = (cnn.Repair_All_Cost / art1.Amount);
            }
            else
            {
                cnn.Repair_All_Cost = 0;
                cnn.Repair_Part_Cost = 0;
                intPrice = 0;
            }

            cnn.Apt_Code = art1.Apt_Code;
            cnn.Repair_Article_Code = art1.Repair_Article_Code;
            cnn.Repair_Plan_Code = art1.Repair_Plan_Code;
            cnn.Repair_Rate = art1.Set_Repair_Rate;
            var fac = await facility_Sort_Lib.Detail_Sort(ae.Sort_C_Code);
            //cnn.Repair_Amount = art1.Amount;
            cnn.Sort_A_Code = art1.Sort_A_Code;
            cnn.Sort_A_Name = art1.Sort_A_Name;
            cnn.Sort_B_Code = art1.Sort_B_Code;
            cnn.Sort_B_Name = art1.Sort_B_Name;
            cnn.Sort_C_Code = art1.Sort_C_Code;
            cnn.Sort_C_Name = art1.Sort_C_Name;

            cnn.Staff_Code = User_Code;
            cnn.Repair_Amount = art1.Amount;
            strCostA = "C";
            CostViewsA = "C";
        }

        /// <summary>
        /// 단가 기준 선택 시 실행
        /// </summary>
        private async Task onCostA(ChangeEventArgs a)
        {
            if (art1.Repair_Article_Name == null || art1.Repair_Article_Name == "" || art1.Repair_Article_Name == "-")
            {
                //strCostA = "D";
                //CostViewsA = "D";
                //StateHasChanged();
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "수선항목을 선택하지 않았습니다.");
                strCostA = "D";
                CostViewsA = "D";
            }
            else
            {
                string Cost_Code = await cost_Lib.GetAid_Division(Aid, art1.Repair_Article_Code); //수선금액 식별코드 구하기
                string Z = a.Value.ToString();
                if (Z == "A")
                {
                    string strName = art1.Sort_C_Name;
                    bool cont = strName.Contains("기타");

                    if (cont == false)
                    {
                        CostViewsA = "A";
                        intCostBeing = await repair_Price_Lib.Price_Count_SortC(cnn.Sort_C_Code, "A");
                        if (intCostBeing > 0)
                        {
                            rpk = await repair_Price_Lib.Price_Drop_List(cnn.Sort_C_Code, a.Value.ToString());
                        }
                        if (Cost_Code == "A" || Cost_Code == null || Cost_Code == "")
                        {
                            enn = new Cost_Entity();
                            pseList = new List<Price_Set_Entity>();
                        }
                        else
                        {
                            enn = await cost_Lib.Detail_Cost(Apt_Code, Cost_Code);
                            pseList = await price_Set_Lib.GetList_A(Cost_Code);
                        }
                        cnn.Cost_Etc = "일위단가는 주택관리협회에서 수집하여 제공된 단가를 기준을 작성된 수선금액임.";
                        CostViewsB = "B";
                        InsertCost = "A";
                    }
                    else
                    {
                        if (EditViews == "A")
                        {
                            await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", " 「공동주택관리법」 「별표1」에 포함되지 않은 공사종별입니다. \n 「별표1」에 포함되지 않은 수선항목은 단가를 제공하지 않습니다. \n 어떤 수선항목일지 알 수 없기 때문입니다. \n 실행금액으로만 입력이 가능합니다.");
                            CostViewsA = "C";
                            CostViewsB = "A";
                            InsertCost = "B";
                            strCostA = "C";
                        }
                        else
                        {
                            await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", " 「공동주택관리법」 「별표1」에 포함되지 않은 공사종별입니다. \n 「별표1」에 포함되지 않은 수선항목은 단가를 제공하지 않습니다. \n 어떤 수선항목일지 알 수 없기 때문입니다. \n 실행금액으로만 입력이 가능합니다.");
                            CostViewsA = "C";
                            CostViewsB = "A";
                            InsertCost = "A";
                            InsertCostA = "B";
                            strCostA = "C";
                        }
                    }
                    pse = new Price_Set_Entity();
                    strProduct = "";
                }
                else if (Z == "C")
                {
                    if (EditViews == "A")
                    {
                        CostViewsA = "C";
                        CostViewsB = "A";
                        InsertViews = "B";
                        strCostA = "C";
                    }
                    else
                    {
                        CostViewsA = "C";
                        CostViewsB = "A";
                        InsertCost = "A";
                        InsertCostA = "B";
                        strCostA = "C";
                    }
                }
                else
                {
                    if (EditViews == "A")
                    {
                        CostViewsA = "D";
                        CostViewsB = "A";
                        InsertCost = "B";
                    }
                    else
                    {
                        CostViewsA = "D";
                        CostViewsB = "A";
                        InsertCost = "A";
                    }
                }
            }
            strAddPrice = "A";
        }

        /// <summary>
        /// 전체수선금액 입력 실행
        /// </summary>
        /// <param name="a"></param>
        private void OnAllCostInput(ChangeEventArgs a)
        {
            long intNum = Convert.ToInt64(a.Value);
            cnn.Repair_All_Cost = intNum;
            cnn.Repair_Part_Cost = Convert.ToInt64(intNum * (cnn.Repair_Rate / 100)); //부분수선금액 계산
            intPrice = intNum / cnn.Repair_Amount; //단가 계산
        }

        /// <summary>
        /// 단가 품목 선택 시 실행
        /// </summary>
        private async Task onPrice(ChangeEventArgs a)
        {
            strProduct = a.Value.ToString();
            try
            {
                if (strAddPrice == "A")
                {
                    var Product = await repair_Price_Lib.GetDetail_RPC(Convert.ToInt32(strProduct), "A");

                    double dbT_Price = Product.Source_amt + Product.Labor_amt + Product.Cost_amt; // 단가에 간접비 미 포함 합산액

                    pse.DetailKind_Code = Product.DetailKind_Code;
                    pse.Apt_Code = Apt_Code;
                    pse.Goods_Name = Product.Product_name;
                    pse.Price_Code = Product.Price_Code.ToString();
                    pse.Price_Division = "A";
                    pse.Price_Set_Etc = "해당없음";
                    pse.Product_name = Product.Product_name;
                    pse.Regist_dt = Product.regist_dt;
                    pse.Repair_Amount = 0;
                    pse.Repair_Cost = 0;
                    Personal_Price = Convert.ToInt32(dbT_Price); //단지 단가(임의)
                    pse.Repair_Plan_Code = strCode;
                    pse.Select_Price = Convert.ToInt32(dbT_Price); //단가
                    pse.Staff_Code = User_Code;
                    pse.Unit_cd = Product.Unit_cd;
                }
            }
            catch (Exception)
            {

            }

        }

        /// <summary>
        /// 일위단가에서 수량 입력하면 실행되는 메서드
        /// </summary>
        private void OnPriceAmount(ChangeEventArgs a)
        {
            int intNum = Convert.ToInt32(a.Value);
            pse.Repair_Amount = intNum;

            if (strAddPrice == "A")
            {
                if (pse.Select_Price == Personal_Price)
                {
                    pse.Repair_Cost = pse.Repair_Amount * pse.Select_Price;
                }
                else
                {
                    pse.Repair_Cost = pse.Repair_Amount * Personal_Price;
                }
            }
            else
            {
                pse.Repair_Cost = pse.Repair_Amount * Personal_Price;
            }
        }

        /// <summary>
        /// 단지 단가 입력 시 실행
        /// </summary>
        private async Task OnPersonalPrice(ChangeEventArgs a)
        {
            int intNum = Convert.ToInt32(a.Value);
            Personal_Price = intNum;
            if (strAddPrice == "B")
            {
                pse.Select_Price = Personal_Price;
            }
            if (pse.Repair_Amount > 0)
            {
                pse.Repair_Cost = pse.Repair_Amount * Personal_Price;
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "수량을 입력하지 않았습니다.");
            }
        }

        /// <summary>
        /// 추가단가 열고 닫기
        /// </summary>
        public string strAddPrice { get; set; } = "A";
        private void btnAddPriceOpen()
        {
            pse = new Price_Set_Entity();
            pse.Regist_dt = DateTime.Today;
            strAddPrice = "B";
            strProduct = "";
        }
        private void btnAddPriceClose()
        {
            strAddPrice = "A";
        }

        /// <summary>
        /// 전체 수선금액 만들기
        /// </summary>
        private async Task Save_Prive_View(string price_Division)
        {
            ps = await prime_Cost_Report_Lib.GetDetail_Set(cnn.Repair_Cost_Code.ToString());
            ur = await unitPrice_Rate_Lib.Detail_New_Year();

            #region 원가 계산서 계산
            //lblSouce_amt.Text = FuncFormat_Cost(ps.Source_amt);
            double db_se = 0;
            // lblSouce_Etc.Text = repair_Cost_Code;

            double db_cst = ps.Source_amt + db_se;
            //lblCost_Souce.Text = FuncFormat_Cost(db_cst);

            // 직집노무비
            //lblLabor_amt.Text = FuncFormat_Cost(ps.Labor_amt);
            //간접노무비
            double db_ilc = ps.Labor_amt * (ur.Indirectness_Rate / 100);
            //lblIndirectness_Labor_Cost.Text = FuncFormat_Cost(db_ilc);

            //노무비 소계
            double db_lt = ps.Labor_amt + (ps.Labor_amt * (ur.Indirectness_Rate / 100));
            //lblLabor_total.Text = FuncFormat_Cost(db_lt);

            // 직접경비
            //lblCost_amt.Text = FuncFormat_Cost(ps.Cost_amt);
            // 산재보험료
            double db_iac = (ps.Labor_amt + db_ilc) * (ur.Industrial_Accident_Rate / 100);
            //lblIndustrail_Accident_Cost.Text = FuncFormat_Cost(db_iac);
            // 고용보험료
            double db_eic = (ps.Labor_amt + db_ilc) * (ur.Employ_Insurance_Rate / 100);
            //lblEmploy_Insurance_Cost.Text = FuncFormat_Cost(db_eic);
            // 건강보험표
            double db_wic = ps.Labor_amt * (ur.Well_Insurance_Rate / 100);
            //lblWell_Insurance_Cost.Text = FuncFormat_Cost(db_wic);
            //연금 보험료
            double db_pic = ps.Labor_amt * (ur.Pension_Insurance_Rate / 100);
            //lblPension_Insurance_Cost.Text = FuncFormat_Cost(db_pic);
            // 노인 장기 요양 보험료
            double db_oic = (db_wic * (ur.OldMan_Insurance_Rate / 100));
            //lblOldman_Insurance_Cost.Text = FuncFormat_Cost(db_oic);
            //산업 안전 보건비
            double db_hsic = (ps.Source_amt + ps.Labor_amt) * (ur.Health_Safe_Insurance_Rate / 100);
            //lblHealth_Safe_Insurance_Cost.Text = FuncFormat_Cost(db_hsic);
            //환경 보전비
            double db_epc = (ps.Source_amt + ps.Labor_amt + ps.Cost_amt) * (ur.Environment_Priserve_Rate / 100);
            //lblEnvironment_Priserve_Cost.Text = FuncFormat_Cost(db_epc);
            // 기타 경비
            double db_ec = (ps.Source_amt + db_lt) * (ur.Etc_Cost_Rate / 100);
            //lblEtc_Cost.Text = FuncFormat_Cost(db_ec);
            // 경비 소계
            double db_ect = ps.Cost_amt + db_iac + db_eic + db_wic + db_pic + db_oic + db_hsic + db_epc + db_ec;
            //lblEtc_Cost_total.Text = FuncFormat_Cost(db_ect);

            // 누계
            double db_tc = db_cst + db_ect + db_lt;
            //lblTotal_Cost.Text = FuncFormat_Cost(db_tc);

            //일반관리비
            double db_cc = db_tc * (ur.Common_Cost_Rate / 100);
            //lblCommon_Cost.Text = FuncFormat_Cost(db_cc);

            // 이윤
            double db_pc = (db_lt + db_ect + db_cc) * (ur.Profit_Rate / 100);
            //lblProift_Cost.Text = FuncFormat_Cost(db_pc);

            // 공급가액
            double db_sc = db_tc + db_cc + db_pc; // + (new Repair_Lib_Price.Price_Set()).Sum_Artcle_Cost(strRepair_Cost_Code, "V");//추가 단가 금액 합산;
                                                  //lblSupply_Cost.Text = FuncFormat_Cost(db_sc);

            // 부가세
            double db_vr = db_sc * (ur.VAT_Rate / 100);
            //lblVat_Cost.Text = FuncFormat_Cost(db_vr);

            // 공사금액
            //string strRepair_Cost_Code = (new Repair_Lib_Price.Repair_Cost()).GetAid_Division(strRepair_Plan_Code, ddlBloomD.SelectedValue);
            double db_sct = db_sc + db_vr; // + intAll_V;
                                           //lblSupply_Total_Cost.Text = FuncFormat_Cost(db_sct);
            #endregion

            intAll_Cost = 0;// + 
            double intAll_V = await price_Set_Lib.Sum_Artcle_Cost(cnn.Repair_Cost_Code.ToString(), "V"); //추가단가
            double intAll_Q = await price_Set_Lib.Sum_Artcle_Cost(cnn.Repair_Cost_Code.ToString(), "Q");//단지 단가

            if (intAll_Q > 0)
            {
                #region MyRegion
                ////////////////////////////////////////////////////////
                Prime_Cost_Report_Entity rp = await prime_Cost_Report_Lib.GetDetail_Select_Price(cnn.Repair_Cost_Code.ToString());

                double db_sra = rp.Source_amt; //재료비 단가
                double db_lba = rp.Labor_amt; //노무비 단가
                double db_cta = rp.Cost_amt;//경비 단가

                double db_s = db_sra / (db_sra + db_lba + db_cta); //재료비 단가 비율
                double db_l = db_lba / (db_sra + db_lba + db_cta); // 노무비 단가 비율
                double db_c = db_cta / (db_sra + db_lba + db_cta); // 경비 단가 비율

                double db_up_s = rp.Repair_Cost * db_s;
                //Response.Write(db_up_s.ToString());
                double db_up_l = rp.Repair_Cost * db_l;
                //Response.Write(db_up_l.ToString());
                double db_up_c = rp.Repair_Cost * db_c;

                #region 원가 계산서 계산
                //lblSouce_amt.Text = FuncFormat_Cost(ps.Source_amt);
                double db_se_q = 0;
                // lblSouce_Etc.Text = repair_Cost_Code;

                double db_cst_q = db_up_s + db_se_q;
                //lblCost_Souce.Text = FuncFormat_Cost(db_cst);

                // 직집노무비
                //lblLabor_amt.Text = FuncFormat_Cost(ps.Labor_amt);
                //간접노무비
                double db_ilc_q = db_up_l * (ur.Indirectness_Rate / 100);
                //lblIndirectness_Labor_Cost.Text = FuncFormat_Cost(db_ilc);

                //노무비 소계
                double db_lt_q = db_up_l + (db_up_l * (ur.Indirectness_Rate / 100));
                //lblLabor_total.Text = FuncFormat_Cost(db_lt);

                // 직접경비
                //lblCost_amt.Text = FuncFormat_Cost(ps.Cost_amt);
                // 산재보험료
                double db_iac_q = (db_up_l + db_ilc_q) * (ur.Industrial_Accident_Rate / 100);
                //lblIndustrail_Accident_Cost.Text = FuncFormat_Cost(db_iac);
                // 고용보험료
                double db_eic_q = (db_up_l + db_ilc_q) * (ur.Employ_Insurance_Rate / 100);
                //lblEmploy_Insurance_Cost.Text = FuncFormat_Cost(db_eic);
                // 건강보험표
                double db_wic_q = db_up_l * (ur.Well_Insurance_Rate / 100);
                //lblWell_Insurance_Cost.Text = FuncFormat_Cost(db_wic);
                //연금 보험료
                double db_pic_q = db_up_l * (ur.Pension_Insurance_Rate / 100);
                //lblPension_Insurance_Cost.Text = FuncFormat_Cost(db_pic);
                // 노인 장기 요양 보험료
                double db_oic_q = (db_wic_q * (ur.OldMan_Insurance_Rate / 100));
                //lblOldman_Insurance_Cost.Text = FuncFormat_Cost(db_oic);
                //산업 안전 보건비
                double db_hsic_q = (db_up_s + db_up_l) * (ur.Health_Safe_Insurance_Rate / 100);
                //lblHealth_Safe_Insurance_Cost.Text = FuncFormat_Cost(db_hsic);
                //환경 보전비
                double db_epc_q = (db_up_s + db_up_l + db_up_c) * (ur.Environment_Priserve_Rate / 100);
                //lblEnvironment_Priserve_Cost.Text = FuncFormat_Cost(db_epc);
                // 기타 경비
                double db_ec_q = (db_up_s + db_lt_q) * (ur.Etc_Cost_Rate / 100);
                //lblEtc_Cost.Text = FuncFormat_Cost(db_ec);
                // 경비 소계
                double db_ect_q = db_up_c + +db_iac_q + db_eic_q + db_wic_q + db_pic_q + db_oic_q + db_hsic_q + db_epc_q + db_ec_q;
                //lblEtc_Cost_total.Text = FuncFormat_Cost(db_ect);

                // 누계
                double db_tc_q = db_cst_q + db_ect_q + db_lt_q;
                //lblTotal_Cost.Text = FuncFormat_Cost(db_tc);

                //일반관리비
                double db_cc_q = db_tc_q * (ur.Common_Cost_Rate / 100);
                //lblCommon_Cost.Text = FuncFormat_Cost(db_cc);

                // 이윤
                double db_pc_q = (db_lt_q + db_ect_q + db_cc_q) * (ur.Profit_Rate / 100);
                //lblProift_Cost.Text = FuncFormat_Cost(db_pc);

                // 공급가액
                double db_sc_q = db_tc_q + db_cc_q + db_pc_q;
                //lblSupply_Cost.Text = FuncFormat_Cost(db_sc);

                // 부가세
                double db_vr_q = db_sc_q * (ur.VAT_Rate / 100);
                //lblVat_Cost.Text = FuncFormat_Cost(db_vr);

                // 공사금액
                double db_sct_q = db_sc_q + db_vr_q;
                //lblSupply_Total_Cost.Text = FuncFormat_Cost(db_sct); 
                #endregion
                #endregion
                intAll_Q = db_sct_q;
            }
            else
            {
                intAll_Q = 0;
            }

            intAll_Cost = db_sct + intAll_V + intAll_Q;// + (new Repair_Lib_Price.Price_Set()).Sum_Artcle_Cost(strRepair_Cost_Code, "V");
        }

        /// <summary>
        /// 수선금액 기본자료 입력
        /// </summary>
        private async Task Cost_Save()
        {
            if (cnn.Repair_Cost_Code < 1)
            {
                int ac = await cost_Lib.Being_Article_Cost_Code(cnn.Apt_Code, cnn.Repair_Plan_Code, cnn.Repair_Article_Code);
                if (ac < 1)
                {
                    cnn = await cost_Lib.Add_Cost(cnn);//저장 
                }
                else
                {
                    await cost_Lib.Update_Cost(cnn);//수정
                }
            }
            else
            {
                await cost_Lib.Update_Cost(cnn);//수정
            }

            enn = await cost_Lib.Detail_Cost(Apt_Code, cnn.Repair_Cost_Code.ToString());
            pseList = await price_Set_Lib.GetList_A(cnn.Repair_Cost_Code.ToString());//GetList_A(dnn.Repair_Article_Code);
        }

        /// <summary>
        /// 일위단가 저장
        /// </summary>
        private async Task btnSaveCostA()
        {
            if (pse.Repair_Amount < 1)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "수량을 입력하지 않았습니다.");
            }
            else if (pse.Select_Price < 1)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "단가를 입력하지 않았습니다.");
            }
            else if (pse.Repair_Cost < 10)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "수선금액이 산출되지 않았습니다.");
            }
            else if (pse.Unit_cd == "" || pse.Unit_cd == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "단위가 입력되지 않았습니다.");
            }
            else
            {
                if (pse.Repair_Amount > 0)
                {
                    if (strAddPrice == "A")
                    {
                        if (pse.Select_Price != Personal_Price)
                        {
                            pse.Repair_Cost = pse.Repair_Amount * Personal_Price;

                            pse.Select_Price = Personal_Price;
                            pse.Price_Division = "Q"; //단지단가로 입력
                            cnn.Price_Sort = "A";
                        }
                        else //pse.Price_Division = "Q"; 추가단가로 입력 한 것임.(아직 구현 안함)
                        {
                            pse.Price_Division = "A"; //제공단가로 입력
                            cnn.Price_Sort = "A";
                            pse.Repair_Cost = pse.Repair_Amount * pse.Select_Price;
                        }
                    }
                    else
                    {
                        pse.Apt_Code = Apt_Code;
                        pse.Price_Set_Etc = "추가 단기로 입력된 수선금액";
                        pse.Repair_Plan_Code = strCode;
                        pse.Staff_Code = User_Code;
                        pse.Goods_Name = pse.Product_name;
                        pse.Price_Division = "V"; //추가단가로 입력
                        cnn.Price_Sort = "A";
                        pse.Repair_Cost = pse.Repair_Amount * pse.Select_Price;
                        pse.Price_Code = "khma";
                        pse.DetailKind_Code = "khma";
                    }

                    #region 수선금액 만들기
                    int intCount = await cost_Lib.Cost_Count(strCode, cnn.Repair_Article_Code); //해당 수선항목에 수선금액 존재 여부 입력

                    if (intCount < 1)
                    {
                        cnn.Repair_All_Cost = 0;
                        cnn.Repair_Part_Cost = 0;
                        cnn = await cost_Lib.Add_Cost(cnn);//저장

                        pse.Repair_Cost_Code = cnn.Repair_Cost_Code.ToString();

                        await price_Set_Lib.Add_PriceSet(pse);
                        pse = new Price_Set_Entity();
                        pse.Regist_dt = DateTime.Today;
                        Personal_Price = 0;
                        //dnn = await cost_Lib.Detail_Cost_Article(Apt_Code, Aid, dnn.Repair_Article_Code);
                        await Save_Prive_View(cnn.Repair_Cost_Code.ToString());
                        //dnn.Repair_All_Cost = dnn.Repair_All_Cost + Convert.ToInt64(intAll_Cost);
                        cnn.Repair_All_Cost = Convert.ToInt64(intAll_Cost);
                        try
                        {
                            cnn.Repair_Part_Cost = Convert.ToInt64(cnn.Repair_All_Cost * (cnn.Repair_Rate / 100)); // 부분수선금액 계산
                        }
                        catch (Exception)
                        {
                            cnn.Repair_Part_Cost = 0;
                        }

                        await Cost_Save();

                    }
                    else
                    {
                        cnn = await cost_Lib.Detail_Cost_Article(Apt_Code, strCode, cnn.Repair_Article_Code);
                        pse.Repair_Cost_Code = cnn.Repair_Cost_Code.ToString();

                        if (pse.Price_Set_Code < 1)
                        {
                            await price_Set_Lib.Add_PriceSet(pse);
                        }
                        else
                        {

                            await price_Set_Lib.Edit_PriceSet(pse);
                        }

                        pse = new Price_Set_Entity();
                        pse.Regist_dt = DateTime.Today;
                        Personal_Price = 0;

                        await Save_Prive_View(cnn.Repair_Cost_Code.ToString());
                        //dnn.Repair_All_Cost = dnn.Repair_All_Cost + Convert.ToInt64(intAll_Cost);
                        cnn.Repair_All_Cost = Convert.ToInt64(intAll_Cost);
                        try
                        {
                            cnn.Repair_Part_Cost = Convert.ToInt64(cnn.Repair_All_Cost * (cnn.Repair_Rate / 100)); // 부분수선금액 계산
                        }
                        catch (Exception)
                        {
                            cnn.Repair_Part_Cost = 0;
                        }
                        await Cost_Save();
                    }
                    #endregion
                    jacc = await article_Lib.GetList_Repair_Article_Cycle_Sort(strCode, "Sort_A_Code", strSortA, Apt_Code);
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "품목의 수량이 입력되지 않았습니다.");
                }
            }
        }

        /// <summary>
        /// 일위단가 수정
        /// </summary>
        private void btnEditCostA(Price_Set_Entity et)
        {
            pse = et;
            if (et.Price_Division == "V")
            {
                strAddPrice = "B";
                pse.Product_name = et.Goods_Name;
                Personal_Price = et.Select_Price;
                pse.Select_Price = 0;
            }
            else
            {
                strAddPrice = "A";
                pse.Product_name = et.Goods_Name;
                Personal_Price = et.Select_Price;
                pse.Select_Price = et.Select_Price;
            }
        }

        /// <summary>
        /// 수선금액 삭제
        /// </summary>
        private async Task OnRemoveCostA(Join_Article_Cycle_Cost_EntityA ct)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{Aid}번 글을 정말로 삭제하시겠습니까?");
            if (isDelete)
            {
                int a = Convert.ToInt32(ct.Repair_Article_Code);
                await cost_Lib.Remove_Repair_Cost(Convert.ToInt32(ct.Repair_Cost_Code));
                await article_Lib.Update_Cost_Count_Minus(a);
                await price_Set_Lib.Remove_CostCode(ct.Repair_Cost_Code);
                await DetailsView();
            }
        }

        /// <summary>
        /// 일위단가 목록에서 삭제
        /// </summary>
        private async Task OnByRemoveCostA(Price_Set_Entity Cde)
        {
            pse = Cde;

            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{Aid}번 글을 정말로 삭제하시겠습니까?");
            if (isDelete)
            {
                cnn.Repair_Cost_Code = Convert.ToInt32(pse.Repair_Cost_Code);
                cnn = await cost_Lib.Detail_Cost(Apt_Code, cnn.Repair_Cost_Code.ToString());

                await price_Set_Lib.Remove(pse.Price_Set_Code);
                await Save_Prive_View("A");
                cnn.Repair_All_Cost = Convert.ToInt64(intAll_Cost);
                if (cnn.Repair_Rate > 0)
                {
                    cnn.Repair_Part_Cost = Convert.ToInt64(cnn.Repair_All_Cost * (cnn.Repair_Rate / 100));
                }
                else
                {
                    cnn.Repair_Part_Cost = 0;
                }
                await Cost_Save();
                enn = await cost_Lib.Detail_Cost(Apt_Code, cnn.Repair_Cost_Code.ToString());
                pseList = await price_Set_Lib.GetList_A(cnn.Repair_Cost_Code.ToString());//GetList_A(dnn.Repair_Article_Code);
            }
        }

        /// <summary>
        /// 일위단가 입력 닫기
        /// </summary>
        /// <returns></returns>
        private async Task btnClosePrice()
        {
            CostViewsB = "A";

            int intCountA = await price_Set_Lib.Being_PriceSet_be(cnn.Repair_Cost_Code.ToString());

            if (EditViews == "A")
            {
                InsertCost = "B";
                InsertCostA = "A";
                CostViewsA = "D";
                strCostA = "D";
            }
            else
            {
                InsertCostA = "A";
                CostViewsA = "C";
                strCostA = "C";
            }

            if (intCountA > 0)
            {
                await article_Lib.Update_Cost_Count_Add(Convert.ToInt32(cnn.Repair_Article_Code)); // 수선항목에 수선금액 있음 입력
            }
            else if (intCountA < 1)
            {
                await article_Lib.Update_Cost_Count_Minus(Convert.ToInt32(cnn.Repair_Article_Code)); // 수선항목에 수선금액 없음 입력
            }
            jacc = await article_Lib.GetList_Repair_Article_Cycle_Sort(Aid, "Sort_A_Code", strSortA, Apt_Code);
            await DetailsView();
        }

        /// <summary>
        /// 수선주기 새로 등록 닫기
        /// </summary>
        private async Task btnCloseCost()
        {
            InsertCost = "A";
            CostViewsA = "A";       
            
            InsertCycle = "A";
            InsertViews = "A";
            InsertArticle = "A";
            await DetailsView();

            StateHasChanged();
        }

        #endregion

        

    }
}
