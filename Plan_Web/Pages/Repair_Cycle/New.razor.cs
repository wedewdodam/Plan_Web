using Facility;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Plan_Apt_Lib;
using Plan_Blazor_Lib;
using Plan_Blazor_Lib.Article;
using Plan_Blazor_Lib.Cycle;
using Plan_Blazor_Lib.Plan;
using Plan_Blazor_Lib.Review;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plan_Web.Pages.Repair_Cycle
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
        [Inject] public IFacility_Sort_Lib facility_Sort_Lib { get; set; }
        [Inject] public IReview_Content_Lib review_Content_Lib { get; set; }
        //[Inject] public ILogView_Lib logView_Lib { get; set; }
        #endregion

        #region 목록 인스턴스
        Repair_Plan_Entity rpn { get; set; } = new Repair_Plan_Entity();
        Article_Entity art { get; set; } = new Article_Entity();
        Cycle_Entity dnn { get; set; } = new Cycle_Entity();

        Review_Content_Join_Enity rce { get; set; } = new Review_Content_Join_Enity();
        List<Join_Article_Cycle_Cost_EntityA> cnnA { get; set; } = new List<Join_Article_Cycle_Cost_EntityA>();
        List<Join_Article_Cycle_Cost_EntityA> cnnB { get; set; } = new List<Join_Article_Cycle_Cost_EntityA>();
        List<Join_Article_Cycle_Cost_EntityA> cnnC { get; set; } = new List<Join_Article_Cycle_Cost_EntityA>();
        List<Join_Article_Cycle_Cost_EntityA> cnnD { get; set; } = new List<Join_Article_Cycle_Cost_EntityA>();
        List<Join_Article_Cycle_Cost_EntityA> cnnE { get; set; } = new List<Join_Article_Cycle_Cost_EntityA>();
        List<Join_Article_Cycle_Cost_EntityA> cnnF { get; set; } = new List<Join_Article_Cycle_Cost_EntityA>();

        Join_Article_Cycle_Cost_EntityA jace { get; set; } = new Join_Article_Cycle_Cost_EntityA();
        List<Article_Entity> rae { get; set; } = new List<Article_Entity>(); //입력 수선항목 목록 만들기
        private List<Facility_Sort_Entity> fnnA { get; set; } = new List<Facility_Sort_Entity>();
        private List<Facility_Sort_Entity> fnnB { get; set; } = new List<Facility_Sort_Entity>();
        #endregion

        #region 변수
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
        protected ElementReference myDiv;
        protected ElementReference myDivP;
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
                await DetailsView();

            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인하지 않았습니다.");
                MyNav.NavigateTo("/");
            }
        }

        private async Task DetailsView()
        {
            cnnA = await cycle_Lib.GetLIst_RepairCycle_Sort(Aid, "Sort_A_Code", "3", Apt_Code);
            cnnB = await cycle_Lib.GetLIst_RepairCycle_Sort(Aid, "Sort_A_Code", "4", Apt_Code);
            cnnC = await cycle_Lib.GetLIst_RepairCycle_Sort(Aid, "Sort_A_Code", "5", Apt_Code);
            cnnD = await cycle_Lib.GetLIst_RepairCycle_Sort(Aid, "Sort_A_Code", "6", Apt_Code);
            cnnE = await cycle_Lib.GetLIst_RepairCycle_Sort(Aid, "Sort_A_Code", "7", Apt_Code);
            cnnF = await cycle_Lib.GetLIst_RepairCycle_Sort(Aid, "Sort_A_Code", "8", Apt_Code);
        }

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
            MyNav.NavigateTo("Repair_Cycle/New" + rpn.Repair_Plan_Code);
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

        /// <summary>
        /// 수선항목 상세정보 열기
        /// </summary>
        /// <param name="ar"></param>
        private void OnByDetailA(Join_Article_Cycle_Cost_EntityA ar)
        {
            jace = ar;
        }

        /// <summary>
        /// 수선항목 수정 열기
        /// </summary>
        /// <param name="ar"></param>
        public int ReviewCount { get; set; } = 1;
        public object strInsYearA { get; private set; }
        public object strInsYearB { get; private set; }
        public int intAllCount { get; set; } // 전체수선주기 수
        public int intPartCount { get; private set; }
        public int intPartAllCount { get; set; } //총 부분수선주기 수

        private async Task OnByEditA(Join_Article_Cycle_Cost_EntityA ar)
        {
            dnn.Aid = ar.Aid;
            dnn.All_Cycle_Num = ar.All_Cycle_Num;
            dnn.Apt_Code = ar.Apt_Code;
            dnn.Division = ar.Division;
            art.Division = ar.Division;
            art.Repair_Plan_Code = ar.Repair_Plan_Code;
            dnn.Law_Repair_Cycle_All = ar.Law_Repair_Cycle_All;
            dnn.Law_Repair_Cycle_Part = ar.Law_Repair_Cycle_Part;
            dnn.Law_Repair_Rate = ar.Law_Repair_Rate;
            dnn.Part_Cycle_Num = ar.Part_Cycle_Num;
            dnn.Repair_Article_Code = ar.Repair_Article_Code;
            dnn.Repair_Cycle_Etc = ar.Repair_Cycle_Etc;
            dnn.Repair_Last_Year_All = ar.Repair_Last_Year_All;
            dnn.Repair_Last_Year_Part = ar.Repair_Last_Year_Part;
            dnn.Repair_Plan_Code = ar.Repair_Plan_Code;
            dnn.Repair_Plan_Year_All = ar.Repair_Plan_Year_All;
            dnn.Repair_Plan_Year_Part = ar.Repair_Plan_Year_Part;
            dnn.Set_Repair_Cycle_All = ar.Set_Repair_Cycle_All;
            dnn.Set_Repair_Cycle_Part = ar.Set_Repair_Cycle_Part;
            dnn.Set_Repair_Rate = ar.Set_Repair_Rate;
            dnn.Sort_A_Code = ar.Sort_A_Code;
            dnn.Sort_A_Name = ar.Sort_A_Name;
            dnn.Sort_B_Code = ar.Sort_B_Name;
            dnn.Sort_B_Name = ar.Sort_B_Name;
            dnn.Sort_C_Code = ar.Sort_C_Code;
            dnn.Sort_C_Name = ar.Sort_C_Name;
            art.Installation = ar.Installation;
            art.Installation_Part = ar.Installation_Part;
            art.All_Cycle = ar.Law_Repair_Cycle_All;
            art.Part_Cycle = ar.Law_Repair_Cycle_Part;
            art.Sort_A_Code = ar.Sort_A_Code;
            art.Sort_A_Name = ar.Sort_A_Name;
            art.Sort_B_Code = ar.Sort_B_Code;
            art.Sort_B_Name = ar.Sort_B_Name;
            art.Sort_C_Code = ar.Sort_C_Code;
            art.Sort_C_Name = ar.Sort_C_Name;
            art.Repair_Article_Name = ar.Repair_Article_Name;
            art.Aid = Convert.ToInt32(ar.Repair_Article_Code);

            intAllCount = dnn.All_Cycle_Num;
            intPartAllCount = dnn.Part_Cycle_Num;

            if (intPartAllCount > 0)
            {
                if (dnn.Set_Repair_Cycle_All > 0 && dnn.Set_Repair_Cycle_Part >= 0)
                {
                    if (dnn.Repair_Plan_Year_All > dnn.Repair_Plan_Year_Part)
                    {
                        int z = ((dnn.Repair_Plan_Year_All - dnn.Repair_Plan_Year_Part) % dnn.Set_Repair_Cycle_Part);
                        if (z > 0)
                        {
                            intPartCount = ((dnn.Repair_Plan_Year_All - dnn.Repair_Plan_Year_Part) / dnn.Set_Repair_Cycle_Part) + 1;
                            Part_Cycle_Schedule = art.Installation_Part.Year;
                        }
                        else
                        {
                            intPartCount = ((dnn.Repair_Plan_Year_All - dnn.Repair_Plan_Year_Part) / dnn.Set_Repair_Cycle_Part);
                            Part_Cycle_Schedule = art.Installation_Part.Year;
                        }
                    }
                    else if (dnn.Repair_Plan_Year_All < dnn.Repair_Plan_Year_Part)
                    {
                        int z = (dnn.Law_Repair_Cycle_All % dnn.Set_Repair_Cycle_Part);
                        if (z > 0)
                        {
                            intPartCount = ((dnn.Repair_Plan_Year_All + dnn.Law_Repair_Cycle_All) / dnn.Set_Repair_Cycle_Part);
                            Part_Cycle_Schedule = dnn.Repair_Plan_Year_All;
                        }
                        else
                        {
                            intPartCount = ((dnn.Repair_Plan_Year_All + dnn.Law_Repair_Cycle_All) / dnn.Set_Repair_Cycle_Part) - 1;
                            Part_Cycle_Schedule = dnn.Repair_Plan_Year_All;
                        }
                    }
                    else
                    {
                        intPartCount = 0;
                    }
                }
                else if (dnn.Set_Repair_Cycle_All <= 0 && dnn.Set_Repair_Cycle_Part > 0)
                {
                    intPartCount = dnn.Part_Cycle_Num;
                    //intPartAllCount = dnn.Part_Cycle_Num;
                }
            }
            else
            {
                intPartCount = 0;
            }


            strInsYearA = ar.Installation.Year.ToString();
            strInsYearB = ar.Installation_Part.Year.ToString();
            strTitle = "수선항목 수정";
            try
            {
                rce = await review_Content_Lib.View_ReviewCode_ArticleName(rpn.Plan_Review_Code, Apt_Code, ar.Repair_Article_Name);
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
        private async Task OnRemoveA(Join_Article_Cycle_Cost_EntityA ct)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{Aid}번 글을 정말로 삭제하시겠습니까?");
            if (isDelete)
            {
                await cycle_Lib.Delete_RepairCycle(ct.Aid);
                int a = Convert.ToInt32(ct.Repair_Article_Code);
                await article_Lib.Update_Cycle_Count_Minus(a);//오류 수정할 것.
                await DetailsView();
            }
        }

        /// <summary>
        /// 수선주기 새로 등록 열기
        /// </summary>
        /// <returns></returns>
        private async Task btnOpen()
        {
            fnnA = await facility_Sort_Lib.GetList_A_FacilitySort(); //대분류 만들기
            rae = await article_Lib.GetLIst_RepairArticle_Sort(rpn.Repair_Plan_Code, "Sort_A_Code", "3", Apt_Code); //수선항목 목록 만들기
            strTitle = "수선주시 새로 등록";
            InsertViews = "B";
        }

        /// <summary>
        /// 수선주기 새로 등록 닫기
        /// </summary>
        private async Task btnClose()
        {
            InsertViews = "A";
            intAllCount = 0;
            intPartCount = 0;
            await DetailsView();
        }

        /// <summary>
        /// 대분류 선택 중분류 목록 만들기
        /// </summary>
        private async Task onSortA(ChangeEventArgs a)
        {
            strSortA = a.Value.ToString();
            fnnB = await facility_Sort_Lib.GetList_FacilitySortA(a.Value.ToString());
            rae = await article_Lib.GetLIst_RepairArticle_Sort(rpn.Repair_Plan_Code, "Sort_A_Code", strSortA, Apt_Code); //수선항목 목록 만들기;
        }

        /// <summary>
        /// 중분류 선택 하면 수선항목 만들기
        /// </summary>
        private async Task onSortB(ChangeEventArgs a)
        {
            strSortB = a.Value.ToString();
            rae = await article_Lib.GetLIst_RepairArticle_Sort(rpn.Repair_Plan_Code, "Sort_B_Code", strSortB, Apt_Code); //수선항목 목록 만들기;
        }

        /// <summary>
        /// 수선주기 새로 저장 또는 수정
        /// </summary>
        /// <returns></returns>
        private async Task btnSave()
        {

            if (dnn.Set_Repair_Cycle_All == 0 && dnn.Set_Repair_Cycle_Part == 0)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "수선주기가 입력되지 않았습니다.");
                InsertViewsA = "A";
                InsertViews = "A";
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
                dnn.Post_IP = myIPAddress;

                #endregion 아이피 입력
                dnn.Apt_Code = Apt_Code;
                dnn.User_ID = User_Code;
                dnn.Repair_Plan_Code = art.Repair_Plan_Code;
                dnn.Sort_A_Code = art.Sort_A_Code;
                dnn.Sort_A_Name = art.Sort_A_Name;
                dnn.Sort_B_Code = art.Sort_B_Code;
                dnn.Sort_B_Name = art.Sort_B_Name;
                dnn.Sort_C_Code = art.Sort_C_Code;
                dnn.Sort_C_Name = art.Sort_C_Name;
                dnn.Law_Repair_Cycle_All = art.All_Cycle;
                dnn.Law_Repair_Cycle_Part = art.Part_Cycle;
                dnn.Law_Repair_Rate = Convert.ToInt32(art.Repair_Rate);
                dnn.Repair_Last_Year_All = art.Installation.Year;
                dnn.Repair_Last_Year_Part = art.Installation_Part.Year;

                if (intAllCount > 0 && intPartCount > 0)
                {
                    dnn.Division = "A";
                }
                else if (intAllCount > 0 && intPartCount < 1)
                {
                    dnn.Division = "B";
                }
                else
                {
                    dnn.Division = "C";
                }
                dnn.All_Cycle_Num = intAllCount;
                dnn.Part_Cycle_Num = intPartCount;
                dnn.Repair_Article_Code = art.Aid.ToString();
                dnn.Repair_Plan_Year_All = (art.Installation.Year + dnn.Set_Repair_Cycle_All);
                dnn.Repair_Plan_Year_Part = (art.Installation_Part.Year + dnn.Set_Repair_Cycle_Part);

                if (dnn.Aid < 1)
                {


                    int c = await cycle_Lib.be_cycle_code(dnn.Repair_Article_Code);

                    if (c < 1)
                    {
                        await cycle_Lib.Add_RepairCycle(dnn);
                        await article_Lib.Update_Cycle_Count_Add(Convert.ToInt32(dnn.Repair_Article_Code));//수선주기 추가
                        rae = await article_Lib.GetLIst_RepairArticle_Sort(rpn.Repair_Plan_Code, "Sort_A_Code", dnn.Sort_A_Code, Apt_Code);
                        art = new Article_Entity();
                        dnn = new Cycle_Entity();
                        intPartCount = 0;
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
                    await cycle_Lib.Edit_RepairCycle(dnn);
                    InsertViewsA = "A";
                }
                await DetailsView();
            }
        }

        /// <summary>
        /// 수선항목 선택 시 실행
        /// </summary>
        private async Task OnBySelectAsync(Article_Entity ae)
        {
            art = ae;

            dnn.Set_Repair_Cycle_All = ae.All_Cycle;
            dnn.Set_Repair_Cycle_Part = ae.Part_Cycle;
            var fac = await facility_Sort_Lib.Detail_Sort(ae.Sort_C_Code);
            dnn.Law_Repair_Cycle_All = fac.Repair_Cycle;
            dnn.Law_Repair_Cycle_Part = fac.Repair_Cycle_Part;
            dnn.Law_Repair_Rate = fac.Repair_Rate;

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
            int intNest = ((rpn.Founding_Date.Year + rpn.Plan_Period) - (art.Installation.Year + dnn.Set_Repair_Cycle_All) - ((intAllCount - 1) * art.All_Cycle)) / art.Part_Cycle;
            if (intAllCount > 0 && art.Part_Cycle > 0)
            {
                if (Now_Year == dnn.Repair_Plan_Year_All)
                {
                    int Y = (art.All_Cycle % art.Part_Cycle); //전체 수선주기가 부분수선주기로 나눈 나머지
                    if (Y > 0)
                    {
                        Part_Cycle_Schedule = Now_Year;
                        intPartCount = art.All_Cycle / art.Part_Cycle;
                        intPartAllCount = intPartCount + ((intAllCount - 2) * (art.All_Cycle / art.Part_Cycle)) + intNest;
                        dnn.Repair_Plan_Year_Part = dnn.Repair_Plan_Year_All + art.Part_Cycle;
                        dnn.Set_Repair_Cycle_Part = art.Part_Cycle;
                    }
                    else if (Y == 0)
                    {
                        Part_Cycle_Schedule = Now_Year;
                        intPartCount = (art.All_Cycle / art.Part_Cycle) - 1;
                        intPartAllCount = intPartCount + ((intAllCount - 2) * ((art.All_Cycle / art.Part_Cycle) - 1)) + intNest;
                        dnn.Repair_Plan_Year_Part = dnn.Repair_Plan_Year_All + art.Part_Cycle;
                        dnn.Set_Repair_Cycle_Part = art.Part_Cycle;
                    }
                }
                else if (Now_Year < dnn.Repair_Plan_Year_All)
                {
                    int Y = Now_Year - (art.Installation_Part.Year + art.Part_Cycle);
                    if (Y >= 0)
                    {
                        dnn.Repair_Plan_Year_Part = Now_Year;
                        dnn.Set_Repair_Cycle_Part = Now_Year - art.Installation_Part.Year;
                        Part_Cycle_Schedule = art.Installation_Part.Year;
                        intPartCount = ((dnn.Repair_Plan_Year_All - (art.Installation_Part.Year + dnn.Set_Repair_Cycle_Part)) / art.Part_Cycle) + 1;
                        int q = art.Installation_Part.Year + dnn.Set_Repair_Cycle_Part + ((intPartCount - 1) * art.Part_Cycle);
                        if (q >= dnn.Repair_Plan_Year_All)
                        {
                            intPartCount = intPartCount - 1;
                        }
                        intPartAllCount = intPartCount + ((intAllCount - 1) * ((art.All_Cycle / art.Part_Cycle) - 1)) + intNest;
                    }
                    else if (Y < 0)
                    {
                        dnn.Repair_Plan_Year_Part = art.Installation_Part.Year + art.Part_Cycle;
                        dnn.Set_Repair_Cycle_Part = art.Part_Cycle;
                        Part_Cycle_Schedule = art.Installation_Part.Year;

                        int X = ((dnn.Repair_Plan_Year_All - dnn.Repair_Plan_Year_Part) / art.Part_Cycle);
                        int Z = ((dnn.Repair_Plan_Year_All - dnn.Repair_Plan_Year_Part) / art.Part_Cycle);

                        if ((X == 0) && (Z > 0))
                        {
                            intPartCount = (Z - 1) + 1;
                        }
                        else
                        {
                            intPartCount = Z + 1;
                        }

                        int q = art.Installation_Part.Year + dnn.Set_Repair_Cycle_Part + ((intPartCount - 1) * art.Part_Cycle);
                        if (q >= dnn.Repair_Plan_Year_All)
                        {
                            intPartCount = intPartCount - 1;
                        }

                        intPartAllCount = intPartCount + ((intAllCount - 1) * ((art.All_Cycle / art.Part_Cycle) - 1)) + intNest;
                    }
                    else
                    {
                        dnn.Repair_Plan_Year_Part = 0;
                        dnn.Set_Repair_Cycle_Part = 0;
                        Part_Cycle_Schedule = 0;
                        intPartCount = 0;
                        intPartAllCount = 0;
                    }
                }
                else
                {
                    dnn.Repair_Plan_Year_Part = 0;
                    dnn.Set_Repair_Cycle_Part = 0;
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
                    dnn.Repair_Plan_Year_Part = Now_Year;
                    dnn.Set_Repair_Cycle_Part = Now_Year - art.Installation_Part.Year;
                    Part_Cycle_Schedule = art.Installation_Part.Year;
                    intPartCount = (((rpn.Founding_Date.Year + rpn.Plan_Period) - dnn.Repair_Plan_Year_Part) / art.Part_Cycle) + 1;
                    intPartAllCount = (((rpn.Founding_Date.Year + rpn.Plan_Period) - dnn.Repair_Plan_Year_Part) / art.Part_Cycle) + 1;
                }
                else if (Y < 0)
                {
                    dnn.Repair_Plan_Year_Part = art.Installation_Part.Year + art.Part_Cycle;
                    dnn.Set_Repair_Cycle_Part = art.Part_Cycle;
                    Part_Cycle_Schedule = art.Installation_Part.Year;
                    intPartCount = (((rpn.Founding_Date.Year + rpn.Plan_Period) - dnn.Repair_Plan_Year_Part) / art.Part_Cycle) + 1;
                    intPartAllCount = (((rpn.Founding_Date.Year + rpn.Plan_Period) - dnn.Repair_Plan_Year_Part) / art.Part_Cycle) + 1;
                }
                else
                {
                    dnn.Repair_Plan_Year_Part = 0;
                    dnn.Set_Repair_Cycle_Part = 0;
                    Part_Cycle_Schedule = 0;
                    intPartCount = 0;
                    intPartAllCount = 0;
                }
            }
            else
            {
                dnn.Repair_Plan_Year_Part = 0;
                dnn.Set_Repair_Cycle_Part = 0;
                Part_Cycle_Schedule = 0;
                intPartCount = 0;
                intPartAllCount = 0;
            }

        }

        private void Cycle_PartB()
        {
            int Now_Year = DateTime.Now.Year;
            int intNest = ((rpn.Founding_Date.Year + rpn.Plan_Period) - (art.Installation.Year + dnn.Set_Repair_Cycle_All) - ((intAllCount - 1) * art.All_Cycle)) / art.Part_Cycle;
            if (intAllCount > 0 && art.Part_Cycle > 0)
            {
                if (Now_Year == dnn.Repair_Plan_Year_All)
                {
                    int Y = (art.All_Cycle % art.Part_Cycle); //전체 수선주기가 부분수선주기로 나눈 나머지
                    if (Y > 0)
                    {
                        Part_Cycle_Schedule = Now_Year;

                        dnn.Repair_Plan_Year_Part = dnn.Repair_Plan_Year_All + dnn.Set_Repair_Cycle_Part; //전체수선예정년도에 입력된 부분수선주기를 더한 부분수선예정도

                        intPartCount = ((dnn.Repair_Plan_Year_All + art.All_Cycle - dnn.Repair_Plan_Year_Part) / art.Part_Cycle) + 1;

                        int Z = (dnn.Repair_Plan_Year_All + dnn.Set_Repair_Cycle_Part) + ((intPartCount - 1) * art.Part_Cycle) - (dnn.Repair_Plan_Year_All + art.All_Cycle);
                        if (Z >= 0)
                        {
                            intPartCount = intPartCount - 1;
                        }

                        intPartAllCount = intPartCount + ((intAllCount - 2) * (art.All_Cycle / art.Part_Cycle)) + intNest;
                        dnn.Repair_Plan_Year_Part = dnn.Repair_Plan_Year_All + art.Part_Cycle;
                        //dnn.Set_Repair_Cycle_Part = art.Part_Cycle;
                    }
                    else if (Y == 0)
                    {
                        Part_Cycle_Schedule = Now_Year;

                        dnn.Repair_Plan_Year_Part = dnn.Repair_Plan_Year_All + dnn.Set_Repair_Cycle_Part; //전체수선예정년도에 입력된 부분수선주기를 더한 부분수선예정도
                        intPartCount = ((dnn.Repair_Plan_Year_All + art.All_Cycle - dnn.Repair_Plan_Year_Part) / art.Part_Cycle);

                        int Z = (dnn.Repair_Plan_Year_All + dnn.Set_Repair_Cycle_Part) + ((intPartCount - 1) * art.Part_Cycle) - (dnn.Repair_Plan_Year_All + art.All_Cycle);
                        if (Z >= 0)
                        {
                            intPartCount = intPartCount - 1;
                        }

                        intPartAllCount = intPartCount + ((intAllCount - 2) * ((art.All_Cycle / art.Part_Cycle) - 1)) + intNest;
                        dnn.Repair_Plan_Year_Part = dnn.Repair_Plan_Year_All + art.Part_Cycle;
                        //dnn.Set_Repair_Cycle_Part = art.Part_Cycle;
                    }
                }
                else if (Now_Year < dnn.Repair_Plan_Year_All)
                {
                    int Y = Now_Year - (art.Installation_Part.Year + dnn.Set_Repair_Cycle_Part);
                    if (Y >= 0)
                    {
                        dnn.Repair_Plan_Year_Part = art.Installation_Part.Year + dnn.Set_Repair_Cycle_Part;
                        dnn.Set_Repair_Cycle_Part = Now_Year - art.Installation_Part.Year;
                        Part_Cycle_Schedule = art.Installation_Part.Year;
                        intPartCount = ((dnn.Repair_Plan_Year_All - (art.Installation_Part.Year + dnn.Set_Repair_Cycle_Part)) / art.Part_Cycle) + 1;
                        int q = art.Installation_Part.Year + dnn.Set_Repair_Cycle_Part + ((intPartCount - 1) * art.Part_Cycle);
                        if (q >= dnn.Repair_Plan_Year_All)
                        {
                            intPartCount = intPartCount - 1;
                        }
                        intPartAllCount = intPartCount + ((intAllCount - 1) * ((art.All_Cycle / art.Part_Cycle) - 1)) + intNest;
                    }
                    else if (Y < 0)
                    {
                        dnn.Repair_Plan_Year_Part = art.Installation_Part.Year + dnn.Set_Repair_Cycle_Part;
                        //dnn.Set_Repair_Cycle_Part = art.Part_Cycle;
                        Part_Cycle_Schedule = art.Installation_Part.Year;


                        int W = dnn.Repair_Plan_Year_All - dnn.Repair_Plan_Year_Part; //전체수선예정년도에서 부분수선예전년도를 뺀 수

                        if (W > 0)
                        {
                            Part_Cycle_Schedule = art.Installation_Part.Year;
                            intPartCount = ((dnn.Repair_Plan_Year_All - (art.Installation_Part.Year + dnn.Set_Repair_Cycle_Part)) / art.Part_Cycle) + 1;
                            int q = art.Installation_Part.Year + dnn.Set_Repair_Cycle_Part + ((intPartCount - 1) * art.Part_Cycle);
                            if (q >= dnn.Repair_Plan_Year_All)
                            {
                                intPartCount = intPartCount - 1;
                            }
                            intPartAllCount = intPartCount + ((intAllCount - 1) * ((art.All_Cycle / art.Part_Cycle) - 1)) + intNest;
                            //dnn.Set_Repair_Cycle_Part = art.Part_Cycle;
                        }
                        else
                        {
                            int w = dnn.Repair_Plan_Year_All - (art.Installation_Part.Year + dnn.Set_Repair_Cycle_Part);
                            if (w <= 0)
                            {
                                Part_Cycle_Schedule = dnn.Repair_Plan_Year_All;
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
                                dnn.Repair_Plan_Year_Part = dnn.Law_Repair_Cycle_All + art.Part_Cycle;
                                dnn.Set_Repair_Cycle_Part = art.Part_Cycle;
                                if ((intAllCount - 1) <= 0)
                                {
                                    intNest = 0;
                                }
                                else
                                {
                                    intNest = ((rpn.Founding_Date.Year + rpn.Plan_Period) - (art.Installation.Year + dnn.Set_Repair_Cycle_All) - ((intAllCount - 1) * art.All_Cycle)) / art.Part_Cycle;
                                }

                                intPartAllCount = intPartCount + intSubCount + intNest;
                            }
                            else
                            {
                                Part_Cycle_Schedule = dnn.Repair_Plan_Year_All;
                                intPartCount = art.All_Cycle / art.Part_Cycle;
                                intPartAllCount = intPartCount + ((intAllCount - 2) * ((art.All_Cycle / art.Part_Cycle) - 1)) + intNest;
                                dnn.Repair_Plan_Year_Part = dnn.Repair_Plan_Year_All + art.Part_Cycle;
                                dnn.Set_Repair_Cycle_Part = art.Part_Cycle;
                            }
                        }


                        //int X = ((dnn.Repair_Plan_Year_All - dnn.Repair_Plan_Year_Part) / art.Part_Cycle);


                        //if ((X == 0) && (X > 0))
                        //{
                        //    intPartCount = (X - 1) + 1;
                        //}
                        //else
                        //{
                        //    intPartCount = X + 1;
                        //}

                        //int q = art.Installation_Part.Year + dnn.Set_Repair_Cycle_Part + ((intPartCount - 1) * art.Part_Cycle);
                        //if (q >= dnn.Repair_Plan_Year_All)
                        //{
                        //    intPartCount = intPartCount - 1;
                        //}

                        //intPartAllCount = intPartCount + ((intAllCount - 1) * ((art.All_Cycle / art.Part_Cycle) - 1)) + intNest;
                    }
                    else
                    {
                        dnn.Repair_Plan_Year_Part = 0;
                        dnn.Set_Repair_Cycle_Part = 0;
                        Part_Cycle_Schedule = 0;
                        intPartCount = 0;
                        intPartAllCount = 0;
                    }
                }
                else
                {
                    dnn.Repair_Plan_Year_Part = 0;
                    dnn.Set_Repair_Cycle_Part = 0;
                    Part_Cycle_Schedule = 0;
                    intPartCount = 0;
                    intPartAllCount = 0;
                }
            }
            else if (intAllCount == 0 && art.Part_Cycle > 0)
            {
                int Y = Now_Year - (art.Installation_Part.Year + dnn.Set_Repair_Cycle_Part);
                if (Y >= 0)
                {
                    dnn.Repair_Plan_Year_Part = Now_Year;
                    dnn.Set_Repair_Cycle_Part = Now_Year - art.Installation_Part.Year;
                    Part_Cycle_Schedule = art.Installation_Part.Year;
                    intPartCount = (((rpn.Founding_Date.Year + rpn.Plan_Period) - dnn.Repair_Plan_Year_Part) / art.Part_Cycle) + 1;
                    intPartAllCount = (((rpn.Founding_Date.Year + rpn.Plan_Period) - dnn.Repair_Plan_Year_Part) / art.Part_Cycle) + 1;
                }
                else if (Y < 0)
                {
                    dnn.Repair_Plan_Year_Part = art.Installation_Part.Year + dnn.Set_Repair_Cycle_Part;
                    //dnn.Set_Repair_Cycle_Part = art.Part_Cycle;
                    Part_Cycle_Schedule = art.Installation_Part.Year;
                    intPartCount = (((rpn.Founding_Date.Year + rpn.Plan_Period) - dnn.Repair_Plan_Year_Part) / art.Part_Cycle) + 1;
                    intPartAllCount = (((rpn.Founding_Date.Year + rpn.Plan_Period) - dnn.Repair_Plan_Year_Part) / art.Part_Cycle) + 1;
                }
                else
                {
                    dnn.Repair_Plan_Year_Part = 0;
                    dnn.Set_Repair_Cycle_Part = 0;
                    Part_Cycle_Schedule = 0;
                    intPartCount = 0;
                    intPartAllCount = 0;
                }
            }
            else
            {
                dnn.Repair_Plan_Year_Part = 0;
                dnn.Set_Repair_Cycle_Part = 0;
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
            int av = (art.Installation.Year + dnn.Set_Repair_Cycle_All) - now_year; // 전체수선 수선예정년도에서 현재 년도를 뺀 수
            int dvp = now_year - art.Installation_Part.Year; //현재년도에서 최종 부분수선년도를 뺀 수
            //dnn.Set_Repair_Cycle_Part = dvp;
            int dva = now_year - art.Installation.Year; // 현재년도에서 최종 전체수선년도를 뺀 수
            int cycle = 0;
            int inst_AP = 0;
            int To = rpn.Founding_Date.Year + rpn.Plan_Period; // 사용검사년도에 계획기간을 더한 값

            int Za = intRepairYear + dnn.Set_Repair_Cycle_All;
            int Zp = intRepairYearPart + dnn.Set_Repair_Cycle_Part;

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
                    if (cycle >= dnn.Set_Repair_Cycle_Part) //최종수선년도를 현재년도에서 뺀 수가 부분수선년도 보다 큰 경우(부분수선을 하지 않은 경우)(cycle >= art.Part_Cycle)
                    {
                        int pp = (dnn.Repair_Plan_Year_All - intNowYear) / art.Part_Cycle;
                        if (pp >= 1)
                        {
                            intPartCount = (av / art.Part_Cycle);
                        }
                        else
                        {
                            intPartCount = (av / art.Part_Cycle) + 1;
                        }

                        dnn.Set_Repair_Cycle_Part = intNowYear - inst_AP;
                    }
                    else if (cycle < art.Part_Cycle)
                    {
                        av = (art.Installation.Year + dnn.Set_Repair_Cycle_All) - inst_AP;
                        if ((av % art.Part_Cycle) == 0)
                        {
                            intPartCount = (av / art.Part_Cycle) - 1;
                        }
                        else
                        {
                            intPartCount = (av / art.Part_Cycle);
                        }
                        dnn.Set_Repair_Cycle_Part = art.Part_Cycle;
                    }
                    else
                    {
                        intPartCount = 0;
                    }
                    Part_Cycle_Schedule = art.Installation_Part.Year;

                    //int LastYear = (Part_Cycle_Schedule + (art.Part_Cycle * (intPartCount - 1))); //첫 전체주기의 부분 수선주기 수
                    //int Nest = intRepairYearPart + dnn.Set_Repair_Cycle_All;
                    //if (Nest < LastYear)
                    //{

                    //}
                }
                else if (av >= 2 && av < art.Part_Cycle)
                {
                    intPartCount = 1;

                    if (cycle < art.Part_Cycle)
                    {
                        dnn.Set_Repair_Cycle_Part = intNowYear - inst_AP;
                    }
                    else
                    {
                        intPartCount = (art.All_Cycle / art.Part_Cycle) - 1;
                        dnn.Set_Repair_Cycle_Part = intNowYear - inst_AP;
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
                        dnn.Set_Repair_Cycle_Part = intNowYear - inst_AP;
                    }

                    Part_Cycle_Schedule = art.Installation.Year + dnn.Set_Repair_Cycle_All;

                }

                #region 부분수선주기 총계
                int intNest = (To - (art.Installation.Year + dnn.Set_Repair_Cycle_All) - ((intAllCount - 1) * art.All_Cycle)) / art.Part_Cycle;//전체수선주기 해당년도를 제외한 나머지년도에서 부분수선 주기 횟수계산
                if ((intAllCount > 0) && (art.Part_Cycle > 0))
                {
                    int Z = (art.All_Cycle % art.Part_Cycle);
                    if (Z > 0)
                    {
                        if (av >= dnn.Set_Repair_Cycle_Part)
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
                        if (av >= dnn.Set_Repair_Cycle_Part) //첫 전체주기에 부분수선주기가 있는 경우
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
                    int z = (dnn.Set_Repair_Cycle_Part + art.Installation_Part.Year);//최종수선년도에 첫주기를 더한 값
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
                int Pa = Part_inst + dnn.Set_Repair_Cycle_Part; // 부분 수선 최종 수선일에 부분 수선기간을 더한 값
                intRepairYear = Part_inst; // 부분선 최종 수선년도 대입


                if (Pa <= intNowYear)
                {
                    intPartCount = ((To - intNowYear) / art.Part_Cycle) + 1;
                    dnn.Set_Repair_Cycle_Part = intNowYear - inst_AP;
                }
                else if (Pa > intNowYear)
                {
                    intPartCount = ((To - Pa) / art.Part_Cycle) + 1;
                    dnn.Set_Repair_Cycle_Part = Pa - Part_inst;
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
                    int z = (dnn.Set_Repair_Cycle_Part + art.Installation_Part.Year);//최종수선년도에 첫주기를 더한 값
                    if ((now_year - art.Installation_Part.Year) >= dnn.Set_Repair_Cycle_Part)
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
                    int z = (dnn.Set_Repair_Cycle_Part + art.Installation_Part.Year);//최종수선년도에 첫주기를 더한 값
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
            dnn.Repair_Plan_Year_Part = art.Installation_Part.Year + dnn.Set_Repair_Cycle_Part;
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
            if (((intNowYear - intInsA) >= dnn.Set_Repair_Cycle_All) && (dnn.Set_Repair_Cycle_All > 0))//현재 년도에서 최종수선년도를 뺀 수를 전체주기보다 큰 경우
            {
                int rest = intNowYear - intInsA; //현재 년도에서 최종수선년도를 뺀 년도 수
                dnn.Set_Repair_Cycle_All = rest; // ??
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
            else if (((intNowYear - intInsA) < dnn.Set_Repair_Cycle_All) && art.All_Cycle > 0)
            {
                int Z = intInsA + dnn.Set_Repair_Cycle_All; //최종 수선년도에 첫 전체수선주기를 더한 값

                intAllCount = ((ListYear - Z) / art.All_Cycle) + 1;

                int rest = (intInsA + art.All_Cycle) - (intNowYear - intInsA);
            }
            else
            {
                intAllCount = 0;
            }

            int LastYear = (intInsA + dnn.Set_Repair_Cycle_All + (art.All_Cycle * (intAllCount - 1)));

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

            dnn.Repair_Plan_Year_All = art.Installation.Year + dnn.Set_Repair_Cycle_All;
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
                dnn.Set_Repair_Cycle_All = Y;
                dnn.Repair_Plan_Year_All = art.Installation.Year + dnn.Set_Repair_Cycle_All;
                Cycle_all();
                Cycle_PartA();
            }
            else
            {
                dnn.Set_Repair_Cycle_All = dnn.Repair_Plan_Year_All - art.Installation.Year;

                await JSRuntime.InvokeVoidAsync("SetFocusToElement", myDiv);

                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "수선예정년도가 현재 년도 보다 작을 수는 없습니다. \n 수선예정년도는 금년이거나 미래이여야지 과거 일 수는 없기 때문입니다.");

            }
            //StateHasChanged();
        }

        /// <summary>
        /// 부분 설정주기 수정 시에 실행
        /// </summary>
        /// <param name="a"></param>
        private async Task OnChageingPart(ChangeEventArgs a)
        {
            dnn.Set_Repair_Cycle_Part = Convert.ToInt32(a.Value);
            dnn.Repair_Plan_Year_Part = art.Installation_Part.Year + dnn.Set_Repair_Cycle_Part;

            if (dnn.Repair_Plan_Year_All > DateTime.Now.Year)
            {
                int Z = (art.Installation_Part.Year + dnn.Set_Repair_Cycle_Part);

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
                int Z = (dnn.Repair_Plan_Year_All + dnn.Set_Repair_Cycle_Part);


                if ((Z - DateTime.Now.Year) >= 0)
                {
                    dnn.Set_Repair_Cycle_Part = Convert.ToInt32(a.Value);
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
        /// 일괄저장 클래스
        /// </summary>
        /// <returns></returns>
        private async Task OnCycleAllInsert(string Sort_C_Cdoe)
        {

            string strAgo_Plan_Code = await repair_Plan_Lib.BeComplete_Code(Apt_Code); // 직전 정기조정된 장기수선계획 코드
            var lnn = await cycle_Lib.GetList_Cycle(Apt_Code, strAgo_Plan_Code); // 직전 수선주기 
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
            dnn.Post_IP = myIPAddress;
            #endregion 아이피 입력

            foreach (var it in lnn)
            {
                var date = DateTime.Now.Year;
                if (date <= it.Repair_Plan_Year_All || date <= it.Repair_Plan_Year_Part)
                {
                    it.Post_IP = dnn.Post_IP;
                    it.Repair_Plan_Code = Aid;
                    it.User_ID = User_Code;
                    it.Repair_Article_Code = await cycle_Lib.OnArticleCode(Aid, Sort_C_Cdoe, "");
                    await cycle_Lib.Add_RepairCycle(it);
                }

            }
        }
    }
}
