using Facility;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Plan_Apt_Lib;
using Plan_Blazor_Lib;
using Plan_Blazor_Lib.Article;
using Plan_Blazor_Lib.Plan;
using Plan_Blazor_Lib.Record;
using Plan_Lib.Company;
using Plan_Lib.Pund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plan_Web.Pages.Record_Infor
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
        [Inject] public IRepair_Record_Lib repair_Record_Lib { get; set; }
        [Inject] public ICompany_Lib company_Lib { get; set; }
        [Inject] public ICompany_Etc_Lib company_Etc_Lib { get; set; }
        [Inject] public IFacility_Sort_Lib facility_Sort_Lib { get; set; }
        [Inject] public IArticle_Lib article_Lib { get; set; }

        List<Repair_Record_Entity> ann { get; set; } = new List<Repair_Record_Entity>();
        Repair_Record_Entity dnn { get; set; } = new Repair_Record_Entity();
        Company_Entity bnn { get; set; } = new Company_Entity();
        Company_Etc_Entity cnn { get; set; } = new Company_Etc_Entity();
        Company_Entity_Etc cnnE { get; set; } = new Company_Entity_Etc();
        private List<Facility_Sort_Entity> fnnA { get; set; } = new List<Facility_Sort_Entity>();
        private List<Facility_Sort_Entity> fnnB { get; set; } = new List<Facility_Sort_Entity>();
        private List<Facility_Sort_Entity> fnnC { get; set; } = new List<Facility_Sort_Entity>();
        private List<Article_Entity> Arnn { get; set; } = new List<Article_Entity>();
        Repair_Plan_Entity rpn { get; set; } = new Repair_Plan_Entity();


        public string Apt_Code { get; private set; }
        public string User_Code { get; private set; }
        public string Apt_Name { get; private set; }
        public string User_Name { get; private set; }
        public string BuildDate { get; private set; }
        public string Work_Year { get; set; }
        public string strCode { get; set; }
        public string InsertRecord { get; set; } = "A";
        public string InsertCompany { get; set; } = "A";
        public string InOutWork { get; set; } = "A";
        public string strTitle { get; set; }
        public string strSortD { get; private set; }
        public string strSortC { get; private set; }
        public string strSortB { get; private set; }
        public string strSortA { get; private set; }
        public string CorporRate_Num { get; private set; }

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
                    await DatailsView(Work_Year);
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
        /// 데이터 로드
        /// </summary>
        private async Task DatailsView(string Year)
        {
            ann = await repair_Record_Lib.list_Year_List_New(Apt_Code, Year);
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
                //dnn.Repair_Name = await facility_Sort_Lib.DetailName_FacilitySort(strSortB) + " 긴급 보수 공사";
                //dnn.Repair_Position = bnn.Repair_Article_Etc;
            }
            else
            {
                dnn.Repair_Article_Code = strSortD;
                //bnn = await article_Lib.Detail_RepairArticle(Apt_Code, Convert.ToInt32(strSortD));
                //dnn.Repair_Name = await facility_Sort_Lib.DetailName_FacilitySort(strSortB) + " " + await facility_Sort_Lib.DetailName_FacilitySort(strSortC) + " " + bnn.Repair_Article_Name + " 보수 공사";
                //dnn.Repair_Position = bnn.Repair_Article_Etc;
            }
        }

        /// <summary>
        /// 년도 선택 시 선택된 년도 정보 보이기
        /// </summary>
        private async Task OnByYearSelect(ChangeEventArgs a)
        {
            Work_Year = a.Value.ToString();
            await DatailsView(Work_Year);
        }

        /// <summary>
        /// 장기수선계획 이력등록 열기
        /// </summary>
        private async Task btnOpen()
        {
            dnn = new Repair_Record_Entity();
            dnn.Repair_Start_Date = DateTime.Now.Date;
            dnn.Repair_End_Date = DateTime.Now.Date;
            fnnA = await facility_Sort_Lib.GetList_A_FacilitySort(); //대분류 만들기
            InsertRecord = "B";
            InOutWork = "A";
            strTitle = "외주작업";
        }

        /// <summary>
        /// 내부작업 등록 열기
        /// </summary>
        private void btnInWork()
        {
            strTitle = "내부작업";
            InOutWork = "B";
        }

        /// <summary>
        /// 외주작업 등록 열기
        /// </summary>
        private void btnOutWork()
        {
            strTitle = "외주작업";
            InOutWork = "A";
        }

        private async Task btnCompanySearch(ChangeEventArgs a)
        {
            if (a.Value != null)
            {
               CorporRate_Num = a.Value.ToString().Replace("-", "").Replace("_", "").Replace(",", "");
                int incn = await company_Lib.CorporRate_Number(CorporRate_Num);
                if (incn > 0)
                {
                    bnn = await company_Lib.ByDetails_Company(CorporRate_Num);
                    dnn.CorporRate_Number = bnn.CorporRate_Number;
                    dnn.Company_Name = bnn.Company_Name;
                    dnn.Company_Code = bnn.Company_Code;
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "그런 업체는 존재하지 않았습니다. \n 사업자 번호를 다시 입력해 보시거나 아래 업체등록 버튼을 클릭하여 \n 업체를 등록해 주시고 다시 시도해 보세요.");
                }
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "사업자 번호를 입력하지 않았습니다.");
            }
        }

        /// <summary>
        /// 이력정보 수정
        /// </summary>
        private void OnByEdit(Repair_Record_Entity ar)
        {
            InsertRecord = "A";
        }

        /// <summary>
        /// 이력 정보 삭제
        /// </summary>
        private async Task OnRemove(int Aid)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{Aid}번 글을 정말로 삭제하시겠습니까?");
            if (isDelete)
            {
                await repair_Record_Lib.Remove(Aid);
                await DatailsView(Work_Year);
            }
        }

        /// <summary>
        /// 이력 정보 저장
        /// </summary>
        private async Task btnSave()
        {
            await repair_Record_Lib.Add(dnn);
            InsertRecord = "A";
        }

        /// <summary>
        /// 이력정보 닫기
        /// </summary>
        private void btnClose()
        {
            InsertRecord = "A";
        }
    }
}
