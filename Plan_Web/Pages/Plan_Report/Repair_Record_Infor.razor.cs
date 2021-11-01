using Facility;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Plan_Apt_Lib;
using Plan_Blazor_Lib;
using Plan_Blazor_Lib.Article;
using Plan_Blazor_Lib.Cost;
using Plan_Blazor_Lib.Plan;
using Plan_Blazor_Lib.Record;
using Plan_Lib.Company;
using Plan_Lib.Pund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plan_Web.Pages.Plan_Report
{
    public partial class Repair_Record_Infor
    {
        #region 속성
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
        [Inject] public ICompanySort_Lib companySort_Lib { get; set; }
        [Inject] public IFacility_Sort_Lib facility_Sort_Lib { get; set; }
        [Inject] public IArticle_Lib article_Lib { get; set; }
        [Inject] public ICost_Lib cost_Lib { get; set; }
        [Inject] public ISido_Lib sido_Lib { get; set; }

        List<Repair_Record_Entity> ann { get; set; } = new List<Repair_Record_Entity>();
        Repair_Record_Entity dnn { get; set; } = new Repair_Record_Entity();
        Company_Entity bnn { get; set; } = new Company_Entity();
        Company_Etc_Entity cnn { get; set; } = new Company_Etc_Entity();
        Company_Entity_Etc cnnE { get; set; } = new Company_Entity_Etc();
        List<Company_Sort_Entity> CsnnA { get; set; } = new List<Company_Sort_Entity>();
        List<Company_Sort_Entity> CsnnB { get; set; } = new List<Company_Sort_Entity>();
        private List<Facility_Sort_Entity> fnnA { get; set; } = new List<Facility_Sort_Entity>();
        private List<Facility_Sort_Entity> fnnB { get; set; } = new List<Facility_Sort_Entity>();
        private List<Facility_Sort_Entity> fnnC { get; set; } = new List<Facility_Sort_Entity>();
        private List<Article_Entity> Arnn { get; set; } = new List<Article_Entity>();
        private Cost_Entity Ccnn { get; set; } = new Cost_Entity();
        Repair_Plan_Entity rpn { get; set; } = new Repair_Plan_Entity();
        List<Sido_Entity> sidos { get; set; } = new List<Sido_Entity>();

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
        public string strTitle_Company { get; set; }
        public string strSortD { get; private set; }
        public string strSortC { get; private set; }
        public string strSortB { get; private set; }
        public string strSortA { get; private set; }
        public string CorporRate_Num { get; private set; }
        public int intCN { get; private set; }
        public string strCorporRate_Number { get; set; }
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
        /// 년도 선택 시 선택된 년도 정보 보이기
        /// </summary>
        private async Task OnByYearSelect(ChangeEventArgs a)
        {
            Work_Year = a.Value.ToString();
            await DatailsView(Work_Year);
        }
        /// <summary>
        /// 인쇄로 이동
        /// </summary>
        private void btnPrint()
        {

        }

    }
}
