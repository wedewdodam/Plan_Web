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

namespace Plan_Web.Pages.Repair_Cost
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
        #endregion

        #region 목록 인스턴스
        Repair_Plan_Entity rpn { get; set; } = new Repair_Plan_Entity();
        Join_Article_Cycle_Cost_Entity art { get; set; } = new Join_Article_Cycle_Cost_Entity();
        Cost_Entity dnn { get; set; } = new Cost_Entity(); //수선금액을 입력하기 위한 수선금액 정보
        Cost_Entity enn { get; set; } = new Cost_Entity(); //일위단가에 표시될 해당 수선항목의 수선금액 정보

        Review_Content_Join_Enity rce { get; set; } = new Review_Content_Join_Enity();
        List<Join_Article_Cycle_Cost_EntityA> cnnA { get; set; } = new List<Join_Article_Cycle_Cost_EntityA>();
        List<Join_Article_Cycle_Cost_EntityA> cnnB { get; set; } = new List<Join_Article_Cycle_Cost_EntityA>();
        List<Join_Article_Cycle_Cost_EntityA> cnnC { get; set; } = new List<Join_Article_Cycle_Cost_EntityA>();
        List<Join_Article_Cycle_Cost_EntityA> cnnD { get; set; } = new List<Join_Article_Cycle_Cost_EntityA>();
        List<Join_Article_Cycle_Cost_EntityA> cnnE { get; set; } = new List<Join_Article_Cycle_Cost_EntityA>();
        List<Join_Article_Cycle_Cost_EntityA> cnnF { get; set; } = new List<Join_Article_Cycle_Cost_EntityA>();

        Join_Article_Cycle_Cost_EntityA jace { get; set; } = new Join_Article_Cycle_Cost_EntityA();
        List<Join_Article_Cycle_Cost_Entity> rae { get; set; } = new List<Join_Article_Cycle_Cost_Entity>(); //입력 수선항목 목록 만들기
        List<Facility_Sort_Entity> fnnA { get; set; } = new List<Facility_Sort_Entity>();
        List<Facility_Sort_Entity> fnnB { get; set; } = new List<Facility_Sort_Entity>();
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
        public string InsertViews { get; set; } = "A"; //수선금액 입력 모달
        public string InsertViewsA { get; set; } = "A"; //수선금액 수정 모달

        public string strSortA { get; set; }
        public string strSortB { get; set; }
        public string strTitle { get; set; }
        public long intPrice { get; set; }
        public string strCostA { get; set; } = "D"; //단가 기준 값
        public string CostViewsA { get; set; } = "A"; //단가 선택 시 모달 구현
        public string EditViews { get; set; } = "A"; // 수정 여부
        private int intCostBeing { get; set; } = 0;
        public string DetailViews { get; set; } = "A";
        public string CostViewsB { get; set; } = "A"; //일위단기 단가 선택 시 모달 구현
        //protected ElementReference myDiv;
        //protected ElementReference myDivP;
        private string strProduct;
        private double intAll_Cost;

        private int Personal_Price { get; set; } = 0;
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
                string strV = await repair_Plan_Lib.Repair_Plan_Code(Apt_Code);
                rpn = await repair_Plan_Lib.Detail_Repair_Plan(Apt_Code, Aid);

                await DetailsView();
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인하지 않았습니다.");
                MyNav.NavigateTo("/");
            }
        }

        /// <summary>
        /// 목록 정보 만들기
        /// </summary>
        private async Task DetailsView()
        {
            cnnA = await cost_Lib.GetLIst_RepairCost_Sort_New(Aid, "Sort_A_Code", "3", Apt_Code);
            cnnB = await cost_Lib.GetLIst_RepairCost_Sort_New(Aid, "Sort_A_Code", "4", Apt_Code);
            cnnC = await cost_Lib.GetLIst_RepairCost_Sort_New(Aid, "Sort_A_Code", "5", Apt_Code);
            cnnD = await cost_Lib.GetLIst_RepairCost_Sort_New(Aid, "Sort_A_Code", "6", Apt_Code);
            cnnE = await cost_Lib.GetLIst_RepairCost_Sort_New(Aid, "Sort_A_Code", "7", Apt_Code);
            cnnF = await cost_Lib.GetLIst_RepairCost_Sort_New(Aid, "Sort_A_Code", "8", Apt_Code);
        }

        /// <summary>
        /// 수선금액 새로 등록 열기
        /// </summary>
        private async Task btnOpen()
        {
            fnnA = await facility_Sort_Lib.GetList_A_FacilitySort(); //대분류 만들기 GetLIst_Repair_Article_Cycle_Sort
            strSortA = "3";
            rae = await article_Lib.GetList_Repair_Article_Cycle_Sort(Aid, "Sort_A_Code", "3", Apt_Code); //수선항목 목록 만들기
            strTitle = "수선금액 새로 등록";
            InsertViews = "B";
            dnn.Cost_Etc = "실재 공사를 실행한 공사금액 전체를 기준으로 수선금액을 선출함.";
            dnn.Sort_A_Name = "선택 안됨";
            art.Repair_Article_Name = "-";
            strCostA = "D";
            CostViewsA = "D";
        }

        /// <summary>
        /// 수선주기 새로 등록 닫기
        /// </summary>
        private async Task btnClose()
        {
            InsertViews = "A";
            //intAllCount = 0;
            //intPartCount = 0;
            await DetailsView();
        }

        /// <summary>
        /// 수선항목 상세정보 열기
        /// </summary>
        private async Task OnByDetailA(Join_Article_Cycle_Cost_EntityA ar)
        {
            jace = ar;
            pseList = await price_Set_Lib.GetList_A(jace.Repair_Cost_Code);
            DetailViews = "B";
        }

        /// <summary>
        /// 수선항목 수정 열기
        /// </summary>     
        private async Task OnByEditA(Join_Article_Cycle_Cost_EntityA ar)
        {
            dnn.Apt_Code = ar.Apt_Code;
            dnn.Cost_Etc = ar.Cost_Etc;
            dnn.Price_Sort = ar.Price_Sort;
            dnn.Repair_All_Cost = ar.Repair_All_Cost;
            dnn.Repair_Amount = ar.Repair_Amount;
            dnn.Repair_Article_Code = ar.Repair_Article_Code;
            dnn.Repair_Cost_Code = Convert.ToInt32(ar.Repair_Cost_Code);
            dnn.Repair_Cost_CodeA = ar.Repair_Cost_Code;
            dnn.Repair_Part_Cost = ar.Repair_Part_Cost;
            dnn.Repair_Plan_Code = ar.Repair_Plan_Code;
            dnn.Repair_Rate = ar.Repair_Rate;

            art.Repair_All_Cost = ar.Repair_All_Cost;
            art.Repair_Amount = ar.Repair_Amount;
            art.Repair_Rate = ar.Repair_Rate;
            art.Repair_Rate_Cost = ar.Repair_Rate_Cost;
            art.Repair_Part_Cost = ar.Repair_Part_Cost;

            art.Repair_Article_Code = ar.Repair_Article_Code;
            art.Repair_Article_Name = ar.Repair_Article_Name;
            art.Repair_Plan_Year_All = ar.Repair_Plan_Year_All;
            art.Repair_Plan_Year_Part = ar.Repair_Plan_Year_Part;
            art.Set_Repair_Cycle_All = ar.Set_Repair_Cycle_All;
            art.Set_Repair_Cycle_Part = ar.Set_Repair_Cycle_Part;
            art.Set_Repair_Rate = Convert.ToInt32(ar.Repair_Rate);

            art.Sort_A_Code = ar.Sort_A_Code;
            art.Sort_A_Name = ar.Sort_A_Name;
            art.Sort_B_Code = ar.Sort_B_Name;
            art.Sort_B_Name = ar.Sort_B_Name;
            art.Sort_C_Code = ar.Sort_C_Code;
            art.Sort_C_Name = ar.Sort_C_Name;
            dnn.Sort_A_Code = ar.Sort_A_Code;
            dnn.Sort_A_Name = ar.Sort_A_Name;
            dnn.Sort_B_Code = ar.Sort_B_Name;
            dnn.Sort_B_Name = ar.Sort_B_Name;
            dnn.Sort_C_Code = ar.Sort_C_Code;
            dnn.Sort_C_Name = ar.Sort_C_Name;

            fnnA = await facility_Sort_Lib.GetList_A_FacilitySort(); //대분류 만들기 GetLIst_Repair_Article_Cycle_Sort
            strSortA = dnn.Sort_A_Code;
            fnnB = await facility_Sort_Lib.GetList_FacilitySortA(strSortA);
            strSortB = dnn.Sort_B_Code;

            dnn.Staff_Code = User_Code;

            strCostA = "C";
            CostViewsA = "C";
            intPrice = dnn.Repair_All_Cost / dnn.Repair_Amount;
            InsertViewsA = "B";
            EditViews = "B";
        }

        /// <summary>
        /// 수선금액 수정 닫기
        /// </summary>
        private void btnCloseB()
        {
            InsertViewsA = "A";
            EditViews = "A";
        }

        /// <summary>
        /// 수선금액 삭제
        /// </summary>
        private async Task OnRemoveA(Join_Article_Cycle_Cost_EntityA ct)
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
        /// 대분류 선택 중분류 목록 만들기
        /// </summary>
        private async Task onSortA(ChangeEventArgs a)
        {
            strSortA = a.Value.ToString();
            fnnB = await facility_Sort_Lib.GetList_FacilitySortA(a.Value.ToString());
            strSortB = "";
            rae = await article_Lib.GetList_Repair_Article_Cycle_Sort(rpn.Repair_Plan_Code, "Sort_A_Code", strSortA, Apt_Code); //수선항목 목록 만들기;
        }

        /// <summary>
        /// 중분류 선택 하면 수선항목 만들기
        /// </summary>
        private async Task onSortB(ChangeEventArgs a)
        {
            strSortB = a.Value.ToString();
            rae = await article_Lib.GetList_Repair_Article_Cycle_Sort(rpn.Repair_Plan_Code, "Sort_B_Code", strSortB, Apt_Code); //수선항목 목록 만들기;
        }

        /// <summary>
        /// 수선금액 새로 저장 또는 수정
        /// </summary>
        private async Task btnSave()
        {

            if (dnn.Repair_All_Cost == 0)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "전체 수선금액이 입력되지 않았습니다.");
            }
            else
            {
                if (dnn.Repair_Cost_Code < 1)
                {
                    #region 아이피 입력
                    //string myIPAddress = "";
                    //var ipentry = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
                    //foreach (var ip in ipentry.AddressList)
                    //{
                    //    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    //    {
                    //        myIPAddress = ip.ToString();
                    //        break;
                    //    }
                    //}
                    //dnn. = myIPAddress;

                    #endregion 아이피 입력
                    dnn.Apt_Code = Apt_Code;
                    dnn.Staff_Code = User_Code;
                    dnn.Repair_Plan_Code = art.Repair_Plan_Code;
                    dnn.Sort_A_Code = art.Sort_A_Code;
                    dnn.Sort_A_Name = art.Sort_A_Name;
                    dnn.Sort_B_Code = art.Sort_B_Code;
                    dnn.Sort_B_Name = art.Sort_B_Name;
                    dnn.Sort_C_Code = art.Sort_C_Code;
                    dnn.Sort_C_Name = art.Sort_C_Name;
                    //dnn.Repair_Rate = art.Repair_Rate;
                    //dnn.Repair_Amount = art.Amount;
                    dnn.Price_Sort = "A";
                    dnn.Repair_Article_Code = art.Repair_Article_Code;

                    int c = await cost_Lib.Being_Article_Cost(dnn.Apt_Code, dnn.Repair_Plan_Code, dnn.Repair_Article_Code);

                    if (c < 1)
                    {
                        await cost_Lib.Add_Cost(dnn);
                        await article_Lib.Update_Cost_Count_Add(Convert.ToInt32(dnn.Repair_Article_Code));//수선주기 추가
                        art = new Join_Article_Cycle_Cost_Entity();
                        dnn = new Cost_Entity();
                        rae = await article_Lib.GetList_Repair_Article_Cycle_Sort(Aid, "Sort_A_Code", strSortA, Apt_Code); //수선항목 목록 만들기
                        //await DetailsView();
                    }
                    else
                    {
                        await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "이미 입력되었습니다.");
                    }
                }
                else if (dnn.Repair_Cost_Code > 0)
                {
                    await cost_Lib.Update_Cost(dnn);
                    await article_Lib.Update_Cost_Count_Add(Convert.ToInt32(dnn.Repair_Article_Code));//수선주기 추가
                    await DetailsView();
                    InsertViews = "A";
                }
            }
        }

        /// <summary>
        /// 수선항목 선택 시 실행
        /// </summary>
        private async Task OnBySelectAsync(Join_Article_Cycle_Cost_Entity ae)
        {
            dnn = new Cost_Entity();
            art = ae;
            dnn.Repair_All_Cost = 0;
            dnn.Repair_Part_Cost = 0;
            intPrice = 0;
            try
            {
                var ago_Cost = await cost_Lib._Cost(Apt_Code, ae.Repair_Article_Name);
                dnn.Repair_All_Cost = ago_Cost.Repair_All_Cost;
                dnn.Repair_Part_Cost = Convert.ToInt64(dnn.Repair_All_Cost * (art.Repair_Rate / 100));
                intPrice = (dnn.Repair_All_Cost / art.Amount);
            }
            catch (Exception)
            {
                dnn.Repair_All_Cost = 0;
                dnn.Repair_Part_Cost = 0;
                intPrice = 0;
            }

            dnn.Apt_Code = art.Apt_Code;
            dnn.Repair_Article_Code = art.Repair_Article_Code;
            dnn.Repair_Plan_Code = Aid;
            dnn.Repair_Rate = art.Set_Repair_Rate;
            var fac = await facility_Sort_Lib.Detail_Sort(ae.Sort_C_Code);

            dnn.Sort_A_Code = art.Sort_A_Code;
            dnn.Sort_A_Name = art.Sort_A_Name;
            dnn.Sort_B_Code = art.Sort_B_Code;
            dnn.Sort_B_Name = art.Sort_B_Name;
            dnn.Sort_C_Code = art.Sort_C_Code;
            dnn.Sort_C_Name = art.Sort_C_Name;

            dnn.Staff_Code = User_Code;
            dnn.Repair_Amount = art.Amount;
            strCostA = "C";
            CostViewsA = "C";
        }

        /// <summary>
        /// 단가 기준 선택 시 실행
        /// </summary>
        private async Task onCostA(ChangeEventArgs a)
        {
            if (art.Repair_Article_Name == null || art.Repair_Article_Name == "" || art.Repair_Article_Name == "-")
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
                string Cost_Code = await cost_Lib.GetAid_Division(Aid, art.Repair_Article_Code); //수선금액 식별코드 구하기
                string Z = a.Value.ToString();
                if (Z == "A")
                {
                    string strName = art.Sort_C_Name;
                    bool cont = strName.Contains("기타");

                    if (cont == false)
                    {
                        CostViewsA = "A";
                        intCostBeing = await repair_Price_Lib.Price_Count_SortC(dnn.Sort_C_Code, "A");
                        if (intCostBeing > 0)
                        {
                            rpk = await repair_Price_Lib.Price_Drop_List(dnn.Sort_C_Code, a.Value.ToString());
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
                        dnn.Cost_Etc = "일위단가는 주택관리협회에서 수집하여 제공된 단가를 기준을 작성된 수선금액임.";
                        CostViewsB = "B";
                        InsertViews = "A";
                    }
                    else
                    {
                        if (EditViews == "A")
                        {
                            await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", " 「공동주택관리법」 「별표1」에 포함되지 않은 공사종별입니다. \n 「별표1」에 포함되지 않은 수선항목은 단가를 제공하지 않습니다. \n 어떤 수선항목일지 알 수 없기 때문입니다. \n 실행금액으로만 입력이 가능합니다.");
                            CostViewsA = "C";
                            CostViewsB = "A";
                            InsertViews = "B";
                            strCostA = "C";
                        }
                        else
                        {
                            await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", " 「공동주택관리법」 「별표1」에 포함되지 않은 공사종별입니다. \n 「별표1」에 포함되지 않은 수선항목은 단가를 제공하지 않습니다. \n 어떤 수선항목일지 알 수 없기 때문입니다. \n 실행금액으로만 입력이 가능합니다.");
                            CostViewsA = "C";
                            CostViewsB = "A";
                            InsertViews = "A";
                            InsertViewsA = "B";
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
                        InsertViews = "A";
                        InsertViewsA = "B";
                        strCostA = "C";
                    }
                }
                else
                {
                    if (EditViews == "A")
                    {
                        CostViewsA = "D";
                        CostViewsB = "A";
                        InsertViews = "B";
                    }
                    else
                    {
                        CostViewsA = "D";
                        CostViewsB = "A";
                        InsertViews = "A";
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
            dnn.Repair_All_Cost = intNum;
            dnn.Repair_Part_Cost = Convert.ToInt64(intNum * (dnn.Repair_Rate / 100)); //부분수선금액 계산
            intPrice = intNum / dnn.Repair_Amount; //단가 계산
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
                    pse.Repair_Plan_Code = Aid;
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
        /// 일위단가 입력 닫기
        /// </summary>
        private async Task btnCloseA()
        {
            CostViewsB = "A";

            int intCountA = await price_Set_Lib.Being_PriceSet_be(dnn.Repair_Cost_Code.ToString());

            if (EditViews == "A")
            {
                InsertViews = "B";
                InsertViewsA = "A";
                CostViewsA = "D";
                strCostA = "D";
            }
            else
            {
                InsertViewsA = "A";
                CostViewsA = "C";
                strCostA = "C";
            }

            if (intCountA > 0)
            {
                await article_Lib.Update_Cost_Count_Add(Convert.ToInt32(dnn.Repair_Article_Code)); // 수선항목에 수선금액 있음 입력
            }
            else if (intCountA < 1)
            {
                await article_Lib.Update_Cost_Count_Minus(Convert.ToInt32(dnn.Repair_Article_Code)); // 수선항목에 수선금액 없음 입력
            }

            await DetailsView();
            rae = await article_Lib.GetList_Repair_Article_Cycle_Sort(Aid, "Sort_A_Code", strSortA, Apt_Code); //수선항목 목록 만들기
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
        /// 일위단가 저장
        /// </summary>
        private async Task btnSaveA()
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
                            dnn.Price_Sort = "A";
                        }
                        else //pse.Price_Division = "Q"; 추가단가로 입력 한 것임.(아직 구현 안함)
                        {
                            pse.Price_Division = "A"; //제공단가로 입력
                            dnn.Price_Sort = "A";
                            pse.Repair_Cost = pse.Repair_Amount * pse.Select_Price;
                        }
                    }
                    else
                    {
                        pse.Apt_Code = Apt_Code;
                        pse.Price_Set_Etc = "추가 단기로 입력된 수선금액";
                        pse.Repair_Plan_Code = Aid;
                        pse.Staff_Code = User_Code;
                        pse.Goods_Name = pse.Product_name;
                        pse.Price_Division = "V"; //추가단가로 입력
                        dnn.Price_Sort = "A";
                        pse.Repair_Cost = pse.Repair_Amount * pse.Select_Price;
                        pse.Price_Code = "khma";
                        pse.DetailKind_Code = "khma";
                    }

                    #region 수선금액 만들기
                    int intCount = await cost_Lib.Cost_Count(Aid, dnn.Repair_Article_Code); //해당 수선항목에 수선금액 존재 여부 입력

                    if (intCount < 1)
                    {
                        dnn.Repair_All_Cost = 0;
                        dnn.Repair_Part_Cost = 0;
                        dnn = await cost_Lib.Add_Cost(dnn);//저장

                        pse.Repair_Cost_Code = dnn.Repair_Cost_Code.ToString();

                        await price_Set_Lib.Add_PriceSet(pse);
                        pse = new Price_Set_Entity();
                        pse.Regist_dt = DateTime.Today;
                        Personal_Price = 0;
                        //dnn = await cost_Lib.Detail_Cost_Article(Apt_Code, Aid, dnn.Repair_Article_Code);
                        await Save_Prive_View(dnn.Repair_Cost_Code.ToString());

                        dnn.Repair_All_Cost = Convert.ToInt64(intAll_Cost);
                        try
                        {
                            dnn.Repair_Part_Cost = Convert.ToInt64(dnn.Repair_All_Cost * (dnn.Repair_Rate / 100)); // 부분수선금액 계산
                        }
                        catch (Exception)
                        {
                            dnn.Repair_Part_Cost = 0;
                        }

                        await Cost_Save();

                    }
                    else
                    {
                        //await Save_Prive_View();

                        dnn = await cost_Lib.Detail_Cost_Article(Apt_Code, Aid, dnn.Repair_Article_Code);
                        pse.Repair_Cost_Code = dnn.Repair_Cost_Code.ToString();

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

                        await Save_Prive_View(dnn.Repair_Cost_Code.ToString());
                        dnn.Repair_All_Cost = Convert.ToInt64(intAll_Cost);
                        try
                        {
                            dnn.Repair_Part_Cost = Convert.ToInt64(dnn.Repair_All_Cost * (dnn.Repair_Rate / 100)); // 부분수선금액 계산
                        }
                        catch (Exception)
                        {
                            dnn.Repair_Part_Cost = 0;
                        }
                        await Cost_Save();
                    }
                    #endregion

                    //dnn = new Cost_Entity();
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
        private void btnEditA(Price_Set_Entity et)
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
        /// 수선금액 기본자료 입력
        /// </summary>
        private async Task Cost_Save()
        {
            if (dnn.Repair_Cost_Code < 1)
            {
                dnn = await cost_Lib.Add_Cost(dnn);//저장
            }
            else
            {
                await cost_Lib.Update_Cost(dnn);//수정
            }

            enn = await cost_Lib.Detail_Cost(Apt_Code, dnn.Repair_Cost_Code.ToString());
            pseList = await price_Set_Lib.GetList_A(dnn.Repair_Cost_Code.ToString());//GetList_A(dnn.Repair_Article_Code);
        }

        /// <summary>
        /// 수선금액 저장
        /// </summary>
        private async Task btnSaveAll()
        {
            if (dnn.Repair_All_Cost < 1000)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "전체 수선금액을 입력하지 않았거나, \n 전체 수선금액을 입력하지 않습니다.");
            }
            else
            {
                if (dnn.Repair_Cost_Code < 1)
                {
                    await cost_Lib.Add_Cost(dnn);
                }
                else
                {
                    await cost_Lib.Update_Cost(dnn);
                }

                CostViewsB = "A";
                CostViewsA = "B";
                strCostA = "C";
                InsertViews = "B";
            }
        }

        /// <summary>
        /// 전체 수선금액 만들기
        /// </summary>
        private async Task Save_Prive_View(string price_Division)
        {
            ps = await prime_Cost_Report_Lib.GetDetail_Set(dnn.Repair_Cost_Code.ToString());
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
            double intAll_V = await price_Set_Lib.Sum_Artcle_Cost(dnn.Repair_Cost_Code.ToString(), "V"); //추가단가
            double intAll_Q = await price_Set_Lib.Sum_Artcle_Cost(dnn.Repair_Cost_Code.ToString(), "Q");//단지 단가

            if (intAll_Q > 0)
            {
                #region MyRegion
                ////////////////////////////////////////////////////////
                Prime_Cost_Report_Entity rp = await prime_Cost_Report_Lib.GetDetail_Select_Price(dnn.Repair_Cost_Code.ToString());

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
        /// 일위단가 목록에서 삭제
        /// </summary>
        private async Task OnByRemoveA(Price_Set_Entity Cde)
        {
            pse = Cde;

            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{Aid}번 글을 정말로 삭제하시겠습니까?");
            if (isDelete)
            {
                dnn.Repair_Cost_Code = Convert.ToInt32(pse.Repair_Cost_Code);
                dnn = await cost_Lib.Detail_Cost(Apt_Code, dnn.Repair_Cost_Code.ToString());

                await price_Set_Lib.Remove(pse.Price_Set_Code);
                await Save_Prive_View("A");
                dnn.Repair_All_Cost = Convert.ToInt64(intAll_Cost);
                if (dnn.Repair_Rate > 0)
                {
                    dnn.Repair_Part_Cost = Convert.ToInt64(dnn.Repair_All_Cost * (dnn.Repair_Rate / 100));
                }
                else
                {
                    dnn.Repair_Part_Cost = 0;
                }
                await Cost_Save();
                enn = await cost_Lib.Detail_Cost(Apt_Code, dnn.Repair_Cost_Code.ToString());
                pseList = await price_Set_Lib.GetList_A(dnn.Repair_Cost_Code.ToString());//GetList_A(dnn.Repair_Article_Code);
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
        /// 상세보기 닫기
        /// </summary>
        private void btnCloseD()
        {
            DetailViews = "A";
        }
    }
}
