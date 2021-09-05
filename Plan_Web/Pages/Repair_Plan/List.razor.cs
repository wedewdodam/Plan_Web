using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Plan_Blazor_Lib.Cost;
using Plan_Blazor_Lib.Plan;
using Plan_Lib;
using Plan_Lib.Pund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plan_Web.Pages.Repair_Plan
{
    public partial class List
    {
        #region 인스턴스
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public IRepair_Plan_Lib repair_Plan_Lib { get; set; }
        [Inject] public NavigationManager MyNav { get; set; } // Url
        [Inject] private IJSRuntime JSRuntime { get; set; }
        [Inject] public IUnit_Price_Lib unit_Price_Lib { get; set; }
        [Inject] public IBylaw_Lib bylaw_Lib { get; set; }
        [Inject] public ICost_Lib cost_Lib { get; set; }
        #endregion

        #region 속성
        List<Repair_Plan_Entity> rpe { get; set; } = new List<Repair_Plan_Entity>();
        private Unit_Price_string_Entity upsn { get; set; } = new Unit_Price_string_Entity();
        Unit_Price_Entity upn { get; set; } = new Unit_Price_Entity();
        #endregion

        #region 변수
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public string BuildDate { get; private set; }
        #endregion

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

            await DetailsView();

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
            pager.RecordCount = await repair_Plan_Lib.GetList_Repair_Plan_Apt_Count(Apt_Code);
            rpe = await repair_Plan_Lib.GetList_Repair_Plan_Apt(pager.PageIndex, Apt_Code); // 첫 장기계획 상세 정보
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
        /// <param name="Aid"></param>
        private void OnSelect(string Aid)
        {
            MyNav.NavigateTo("/Repair_Plan/" + Aid);
        }
    }
}
