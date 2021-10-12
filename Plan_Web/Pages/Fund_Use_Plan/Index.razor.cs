using Facility;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Plan_Apt_Lib;
using Plan_Blazor_Lib;
using Plan_Blazor_Lib.Article;
using Plan_Blazor_Lib.Plan;
using Plan_Blazor_Lib.Price;
using Plan_Lib.Pund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plan_Web.Pages.Fund_Use_Plan
{
    public partial class Index
    {
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Parameter] public string Aid { get; set; }
        [Inject] public NavigationManager MyNav { get; set; } // Url
        [Inject] private IJSRuntime JSRuntime { get; set; }
        [Inject] public IAptInfor_Lib aptInfor_Lib { get; set; }
        [Inject] public IRepair_Plan_Lib repair_Plan_Lib { get; set; }
        [Inject] public IRepair_Capital_Lib repair_Capital_Lib { get; set; }
        [Inject] public IRepair_Capital_Use_Lib repair_Capital_Use_Lib { get; set; }
        [Inject] public IUnit_Price_Lib unit_Price_Lib { get; set; }
        [Inject] public ILevy_Rate_Lib levy_Rate_Lib { get; set; }
        [Inject] public ICapital_Levy_Lib capital_Levy_Lib { get; set; }
        [Inject] public IFacility_Sort_Lib facility_Sort_Lib { get; set; }
        [Inject] public ICost_Using_Plan_Lib cost_Using_Plan_Lib { get; set; }
        [Inject] public IArticle_Lib article_Lib { get; set; }

        private Repair_Plan_Entity rpn { get; set; } = new Repair_Plan_Entity();
        List<Cost_Using_Plan_Entity> cup { get; set; } = new List<Cost_Using_Plan_Entity>();
        Cost_Using_Plan_Entity dnn { get; set; } = new Cost_Using_Plan_Entity();
        List<Join_Article_Cycle_Cost_EntityA> JACC { get; set; } = new List<Join_Article_Cycle_Cost_EntityA>();
        List<Join_Article_Cycle_Cost_EntityA> JACC1 { get; set; } = new List<Join_Article_Cycle_Cost_EntityA>();
        private List<Facility_Sort_Entity> fnnA { get; set; } = new List<Facility_Sort_Entity>();
        private List<Facility_Sort_Entity> fnnB { get; set; } = new List<Facility_Sort_Entity>();
        private List<Facility_Sort_Entity> fnnC { get; set; } = new List<Facility_Sort_Entity>();
        private List<Article_Entity> Arnn { get; set; } = new List<Article_Entity>();
        private Article_Entity bnn { get; set; } = new Article_Entity();

        private string strSortA;
        private string strSortB;
        private string strSortC;
        private string strSortD;
        public string Apt_Code { get; set; }
        public string User_Code { get; set; }
        public string Apt_Name { get; set; }
        public string User_Name { get; set; }
        public string BuildDate { get; set; }
        public string InsertViews { get; set; } = "A";
        public string strTitle { get; set; }
        public string NowYear { get; private set; }
        public string NextYear { get; private set; }

        private string strCode { get; set; }
        private string strTender { get; set; }
        private string strTenderA { get; set; }
        private string strTenderB { get; set; }
        private string strTenderC { get; set; }


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
            dnn.Plan_Year = DateTime.Now.Year.ToString();
            NowYear = dnn.Plan_Year;
            NextYear = (DateTime.Now.Year + 1).ToString();
            rpn = await repair_Plan_Lib.Detail_Repair_Plan(Apt_Code, strCode);
            cup = await cost_Using_Plan_Lib.GetList_Year(Apt_Code, DateTime.Now.Year.ToString());
            JACC = await repair_Capital_Use_Lib.GetList_Using_Cost(Apt_Code, strCode, NowYear);
            JACC1 = await repair_Capital_Use_Lib.GetList_Using_Cost(Apt_Code, strCode, NextYear);
        }

        /// <summary>
        /// 결정방법 선택 실행
        /// </summary>
        private void OnTenderA(ChangeEventArgs a)
        {
            strTenderA = a.Value.ToString();
            strTender = strTenderA;
            dnn.Tender_Method_Process = strTender + " 등의 방법으로  공동주택관리 제25조 및 같은 법 시행령 제25조 규정에 따라 공사업체 선정";
        }

        /// <summary>
        /// 입찰방법 선택 실행
        /// </summary>
        private void OnTenderB(ChangeEventArgs a)
        {
            strTenderB = a.Value.ToString();
            strTender = strTenderA + " ≫ " + strTenderB;
            dnn.Tender_Method_Process = strTender + " 등의 방법으로 공동주택관리 제25조 및 같은 법 시행령 제25조 규정에 따라 공사업체 선정";
        }

        /// <summary>
        /// 낙찰방법 선택 실행
        /// </summary>
        private void OnTenderC(ChangeEventArgs a)
        {
            strTenderC = a.Value.ToString();
            strTender = strTenderA + " ≫ " + strTenderB + " ≫ " + strTenderC;
            dnn.Tender_Method_Process = strTender + " 등의 방법으로 공동주택관리 제25조 및 같은 법 시행령 제25조 규정에 따라 공사업체 선정";
        }


        /// <summary>
        /// 장기수선충당금 해당 년도별 목록
        /// </summary>
        private async Task OnByYearSelect(ChangeEventArgs a)
        {
            if (a.Value != null)
            {
                NowYear = a.Value.ToString();
                NextYear = (Convert.ToInt32(NowYear) + 1).ToString();
                cup = await cost_Using_Plan_Lib.GetList_Year(Apt_Code, a.Value.ToString());
                JACC = await repair_Capital_Use_Lib.GetList_Using_Cost(Apt_Code, strCode, NowYear);
                JACC1 = await repair_Capital_Use_Lib.GetList_Using_Cost(Apt_Code, strCode, NextYear);
            }
        }

        /// <summary>
        /// 대분류 선택 중분류 목록 만들기
        /// </summary>
        private async Task onSortA(ChangeEventArgs a)
        {
            strSortA = a.Value.ToString();
            fnnB = await facility_Sort_Lib.GetList_FacilitySortA(a.Value.ToString());
            fnnC = await facility_Sort_Lib.GetList_Sort_AA_List(Apt_Code, Aid, strSortA);
        }

        /// <summary>
        /// 중분류 선택 하면 수선항목 만들기
        /// </summary>
        private async Task onSortB(ChangeEventArgs a)
        {
            strSortB = a.Value.ToString();
            fnnC = await facility_Sort_Lib.GetList_FacilitySort(Apt_Code, Aid, strSortB);
        }

        /// <summary>
        /// 중분류 선택 하면 수선항목 만들기
        /// </summary>
        private async Task onSortC(ChangeEventArgs a)
        {
            strSortC = a.Value.ToString();
            if (strSortC != "예외지출")
            {
                Arnn = await article_Lib.GetListArticleSort(strCode, "Sort_C_Code", strSortC, Apt_Code); 
            }
        }

        /// <summary>
        /// 중분류 선택 하면 수선항목 만들기
        /// </summary>
        private async Task onSortD(ChangeEventArgs a)
        {
            strSortD = a.Value.ToString();
            if (strSortD == "소액지출" || strSortD == "긴급공사" || strSortD == "하자비용")
            {
                dnn.Repair_Article_Code = strSortD;
                //bnn = await article_Lib.Detail_RepairArticle(Apt_Code, Convert.ToInt32(strSortD));
                dnn.Repair_Name = await facility_Sort_Lib.DetailName_FacilitySort(strSortB) + " 긴급 보수 공사";
                dnn.Repair_Position = bnn.Repair_Article_Etc;
            }
            else
            {
                dnn.Repair_Article_Code = strSortD;
                bnn = await article_Lib.Detail_RepairArticle(Apt_Code, Convert.ToInt32(strSortD));
                dnn.Repair_Name = await facility_Sort_Lib.DetailName_FacilitySort(strSortB) + " " + await facility_Sort_Lib.DetailName_FacilitySort(strSortC) + " " + bnn.Repair_Article_Name + " 보수 공사";
                dnn.Repair_Position = bnn.Repair_Article_Etc;
            }         
        }

        /// <summary>
        /// 수선항목 명 불러오기
        /// </summary>
        private string Name(string Code)
        {
            string Name = "";
            if (Code == "")
            {
                Name = "없음";
            }
            else if (Code == null)
            {
                Name = "없음";
            }
            else if (Code == "소액지출")
            {
                Name = "소액지출";
            }
            else if (Code == "긴급지출")
            {
                Name = "긴급지출";
            }
            else if (Code == "하자비용")
            {
                Name = "하자비용";
            }
            else if (Code == "긴급공사")
            {
                Name = "긴급공사";
            }
            else
            {
                Name = article_Lib.ArticleName(Code);
            }
            return Name;
        }

        /// <summary>
        /// 수정
        /// </summary>
        private void OnByEdit(Cost_Using_Plan_Entity cde)
        {
            strTitle = "장기수선충당금 징수·적립 수정";
            dnn = cde;
            InsertViews = "B";
        }

        /// <summary>
        /// 삭제
        /// </summary>
        private async Task OnByRemove(int Aid)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{Aid}번 글을 정말로 삭제하시겠습니까?");
            if (isDelete)
            {
                await cost_Using_Plan_Lib.Remove_Repair_Cost(Aid);
                await DetailsView();
            }
        }

        /// <summary>
        /// 입력 열기
        /// </summary>
        private async Task btnOpen()
        {
            fnnA = await facility_Sort_Lib.GetList_A_FacilitySort(); //대분류 만들기
            InsertViews = "B";
            dnn.Repair_Method = "첨부된 시방서 참조";
            dnn.Start_Date = DateTime.Now;
            dnn.End_Date = DateTime.Now;
            dnn.Design_Drawing = "관리사무소 보관중인 설계도면 및 시방서 등 별도 첨부";
            dnn.Plan_Year = dnn.Start_Date.Year.ToString();
        }

        /// <summary>
        /// 계획예년 목록에서 선택 시 실행
        /// </summary>
        /// <param name="ann"></param>
        private async Task OnBySelectA(Join_Article_Cycle_Cost_EntityA ann)
        {
            dnn.Repair_Name = ann.Sort_B_Name + " " + ann.Sort_C_Name + " " + ann.Repair_Article_Name + " 보수 공사";
            dnn.Repair_Article_Code = ann.Repair_Article_Code;
            fnnA = await facility_Sort_Lib.GetList_A_FacilitySort(); //대분류 만들기
            strSortA = ann.Sort_A_Code;
            fnnB = await facility_Sort_Lib.GetList_FacilitySortA(strSortA);
            strSortB = ann.Sort_B_Code;
            fnnC = await facility_Sort_Lib.GetList_FacilitySortA(strSortB);
            strSortC = ann.Sort_C_Code;
            Arnn = await article_Lib.GetListArticleSort(strCode, "Sort_C_Code", strSortC, Apt_Code);
            strSortD = ann.Repair_Article_Code;

            if (NowYear == ann.Repair_Plan_Year_All.ToString() || NextYear == ann.Repair_Plan_Year_All.ToString())
            {
                dnn.Repair_Cost_Sum = ann.Repair_All_Cost;
                dnn.Plan_Year = ann.Repair_Plan_Year_All.ToString();
            }
            else if (NowYear == ann.Repair_Plan_Year_Part.ToString() || NextYear == ann.Repair_Plan_Year_Part.ToString())
            {
                dnn.Repair_Cost_Sum = ann.Repair_Part_Cost;
                dnn.Plan_Year = ann.Repair_Plan_Year_Part.ToString();
            }

           
            dnn.Repair_Method = ann.Repair_Article_Etc;
            dnn.Design_Drawing = "관리사무소 보관중인 설계도면 및 시방서 등 별도 첨부";
            dnn.Start_Date = DateTime.Now;
            dnn.End_Date = DateTime.Now;
            InsertViews = "B";
        }

        /// <summary>
        /// 장기수선충당금 사용계획서 저장
        /// </summary>
        /// <returns></returns>
        private async Task btnSave()
        {
            dnn.Apt_Code = Apt_Code;
            dnn.Repair_Plan_Code = strCode;
            dnn.Staff_Code = User_Code;
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

            if (dnn.Repair_Name == "" || dnn.Repair_Name == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "공사명칭을 입력하지 않았습니다.");
            }
            else if (dnn.Repair_Position == "" || dnn.Repair_Position == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "공사 위치나 부위를 입력하지 않았습니다.");
            }
            else if (dnn.Design_Drawing == "" || dnn.Design_Drawing == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "설계도면에 대한 내용을 입력하지 않았습니다.");
            }
            else if (dnn.Repair_Cost_Sum <= 10000)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "공사 금액을 입력하지 않았습니다.");
            }
            else if (dnn.Repair_Method == "" || dnn.Repair_Method == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "공사 방법을 입력하지 않았습니다.");
            }
            else if (dnn.Repair_Detail == "" || dnn.Repair_Detail == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "공사 내역을 입력하지 않았습니다.");
            }
            else if (dnn.Repair_Range == "" || dnn.Repair_Range == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "공사 범위를 입력하지 않았습니다.");
            }
            else if (dnn.Tender_Method_Process == "" || dnn.Tender_Method_Process == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "발조 방법과 절차를 입력하지 않았습니다.");
            }
            else
            {
                if (dnn.Cost_Use_Plan_Code < 1)
                {
                    await cost_Using_Plan_Lib.Add(dnn);
                }
                else
                {
                    await cost_Using_Plan_Lib.Edit(dnn);
                }
                dnn = new Cost_Using_Plan_Entity();
                await DetailsView();
                InsertViews = "A";
            }
        }

        /// <summary>
        /// 장기수선충당금 사용계획서 입력 닫기
        /// </summary>
        private void btnClose()
        {
            InsertViews = "A";
        }
    }   
}
