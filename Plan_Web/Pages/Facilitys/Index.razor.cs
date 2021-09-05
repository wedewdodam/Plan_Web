using Facility;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plan_Web.Pages.Facilitys
{
    public partial class Index
    {
        #region 인스턴스

        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Inject] public NavigationManager MyNav { get; set; } // Url
        [Inject] private IJSRuntime JSRuntime { get; set; }
        [Inject] public IFacility_Lib facility_Lib { get; set; }
        [Inject] public IFacility_Detail_Lib facility_Detail_Lib { get; set; }
        [Inject] public IFacility_Sort_Lib facility_Sort_Lib { get; set; }

        #endregion 인스턴스

        #region 속성

        private Facility_Entity ann { get; set; } = new Facility_Entity();
        private Facility_Detail_Entity bnn { get; set; } = new Facility_Detail_Entity();
        private Facility_Sort_Entity fnn { get; set; } = new Facility_Sort_Entity();
        private List<Facility_Entity> annA { get; set; } = new List<Facility_Entity>();
        private List<Facility_Entity> annB { get; set; } = new List<Facility_Entity>();
        private List<Facility_Entity> annC { get; set; } = new List<Facility_Entity>();
        private List<Facility_Entity> annD { get; set; } = new List<Facility_Entity>();
        private List<Facility_Entity> annE { get; set; } = new List<Facility_Entity>();
        private List<Facility_Entity> annF { get; set; } = new List<Facility_Entity>();
        private List<Facility_Detail_Entity> bnnA { get; set; } = new List<Facility_Detail_Entity>();
        private List<Facility_Sort_Entity> fnnA { get; set; } = new List<Facility_Sort_Entity>();
        private List<Facility_Sort_Entity> fnnB { get; set; } = new List<Facility_Sort_Entity>();
        private List<Facility_Sort_Entity> fnnC { get; set; } = new List<Facility_Sort_Entity>();
        private List<Facility_Sort_Entity> fnnAA { get; set; } = new List<Facility_Sort_Entity>();
        private List<Facility_Sort_Entity> fnnBB { get; set; } = new List<Facility_Sort_Entity>();

        #endregion 속성

        #region 변수

        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public string User_Code { get; set; }
        public string User_Name { get; set; }
        public string BuildDate { get; set; }
        public string InsertViewsA { get; set; } = "A";
        public string InsertViewsB { get; set; } = "A";

        public string strSortA { get; set; }
        public string strSortB { get; set; }
        public string strSortAA { get; set; }
        public string strSortBB { get; set; }

        public string Division { get; set; } = "A";

        public string strTitle { get; set; }
        //public string strTitleB { get; set; }

        public string ViewsA { get; set; }
        public string ViewsB { get; set; }

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

        private async Task NewDataViews()
        {
            //bnn.Manufacture_Date = Convert.ToDateTime(BuildDate).Date;
            fnnAA = await facility_Sort_Lib.GetList_A_FacilitySort(); //대분류 만들기
            fnnBB = new List<Facility_Sort_Entity>();
            fnnA = await facility_Sort_Lib.GetList_A_FacilitySort(); //대분류 만들기
            fnnB = new List<Facility_Sort_Entity>();
            fnnC = new List<Facility_Sort_Entity>();
        }

        /// <summary>
        /// 중분류 만들기
        /// </summary>
        private async Task onSortA(ChangeEventArgs a)
        {
            strSortA = a.Value.ToString();
            ann.Facility_Sort_Code_A = strSortA;
            bnn.Sort_A = strSortA;
            fnnB = await facility_Sort_Lib.GetList_FacilitySortA(strSortA); //중분류 만들기
            fnnC = new List<Facility_Sort_Entity>();
        }

        /// <summary>
        /// 소분류 만들기
        /// </summary>
        private async Task onSortB(ChangeEventArgs a)
        {
            strSortB = a.Value.ToString();
            ann.Facility_Sort_Code_B = strSortB;
            bnn.Sort_B = strSortB;
            fnnC = await facility_Sort_Lib.GetList_FacilitySortA(strSortB); //소분류 만들기
        }

        /// <summary>
        /// 대분류 목록 만들기
        /// </summary>
        private async Task onSortAA(ChangeEventArgs a)
        {
            strSortAA = a.Value.ToString();
            fnnBB = new List<Facility_Sort_Entity>();
            fnnBB = await facility_Sort_Lib.GetList_FacilitySortA(strSortAA); //중분류 만들기
            Division = "B";
            if (strSortAA == "Z")
            {
                Division = "A";
            }
            await DisplayViews();
        }

        /// <summary>
        /// 중분류 목록 만들기
        /// </summary>
        private async Task onSortBB(ChangeEventArgs a)
        {
            strSortBB = a.Value.ToString();
            Division = "C";
            await DisplayViews();
        }

        /// <summary>
        /// 데이터 뷰
        /// </summary>
        /// <returns></returns>
        private async Task DisplayViews()
        {
            if (Division == "A")
            {
                //pager.RecordCount = await facility_Lib.GetList_Apt_Count(Apt_Code);
                annA = await facility_Lib.GetList_Apt_Sort(Apt_Code, "3");
                annB = await facility_Lib.GetList_Apt_Sort(Apt_Code, "4");
                annC = await facility_Lib.GetList_Apt_Sort(Apt_Code, "5");
                annD = await facility_Lib.GetList_Apt_Sort(Apt_Code, "6");
                annE = await facility_Lib.GetList_Apt_Sort(Apt_Code, "7");
                annF = await facility_Lib.GetList_Apt_Sort(Apt_Code, "8");
            }
            else if (Division == "B")
            {
                pager.RecordCount = await facility_Lib.GetList_Apt_Count(Apt_Code);
                annA = await facility_Lib.GetList_Apt_Query(pager.PageIndex, Apt_Code, "Facility_Sort_Code_A", strSortAA);
            }
            else if (Division == "C")
            {
                pager.RecordCount = await facility_Lib.GetList_Apt_Count(Apt_Code);
                annA = await facility_Lib.GetList_Apt_Query(pager.PageIndex, Apt_Code, "Facility_Sort_Code_B", strSortBB);
            }
            else
            {
                //pager.RecordCount = await facility_Lib.GetList_Apt_Count(Apt_Code);
                annA = await facility_Lib.GetList_Apt_Sort(Apt_Code, "3");
                annB = await facility_Lib.GetList_Apt_Sort(Apt_Code, "4");
                annC = await facility_Lib.GetList_Apt_Sort(Apt_Code, "5");
                annD = await facility_Lib.GetList_Apt_Sort(Apt_Code, "6");
                annE = await facility_Lib.GetList_Apt_Sort(Apt_Code, "7");
                annF = await facility_Lib.GetList_Apt_Sort(Apt_Code, "8");
            }
        }

        #region 시설물 입출력 관련

        /// <summary>
        /// 시설물 정보 입력 및 수정 열기
        /// </summary>
        private void btnOpen()
        {
            ann = new Facility_Entity();
            bnn = new Facility_Detail_Entity();
            ann.Facility_Installation_Date = Convert.ToDateTime(BuildDate).Date;
            bnn.Manufacture_Date = Convert.ToDateTime(BuildDate).Date;
            InsertViewsA = "B";
            strTitle = "시설물 정보 입력";
        }

        /// <summary>
        /// 수정열기
        /// </summary>
        private async Task OnEdit(Facility_Entity aa)
        {
            InsertViewsA = "B";
            strTitle = "시설물 정보 수정";
            ann = aa;
            strSortA = ann.Facility_Sort_Code_A;
            fnnB = await facility_Sort_Lib.GetList_FacilitySortA(strSortA);
            strSortB = ann.Facility_Sort_Code_B;
            fnnC = await facility_Sort_Lib.GetList_FacilitySortA(strSortB);
            int iCount = await facility_Detail_Lib.Detail_Facility_Detail_FacilityCode_Count(ann.Facility_Code);
            if (iCount > 0)
            {
                bnn = await facility_Detail_Lib.Detail_Facility_Detail_FacilityCode(ann.Facility_Code);
            }
            else
            {
                bnn = await facility_Detail_Lib.Detail_Facility_Detail_FacilityCode(ann.Aid.ToString());
            }
        }

        /// <summary>
        /// 시설물 삭제하기
        /// </summary>
        private async Task OnRemove(int Aid)
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{Aid}번 글을 정말로 삭제하시겠습니까?");
            if (isDelete)
            {
                await facility_Lib.Remove(Aid.ToString());
                await DisplayViews();
            }
        }

        /// <summary>
        /// 수정 및 등록 닫기
        /// </summary>
        private void btnClose()
        {
            InsertViewsA = "A";
        }

        /// <summary>
        /// 시설물 정보 저장 및 수정 하기
        /// </summary>
        /// <returns></returns>
        public async Task btnSave()
        {
            int Add_id = 0;
            ann.Facility_Code = "F" + DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
            if (ann.Facility_Name == "" || ann.Facility_Name == null)
            {
                await JSRuntime.InvokeAsync<object>("alert", "시설명이 입력되지 않았습니다.");
            }
            else if (ann.Facility_Code == "" || ann.Facility_Code == null || bnn.Facility_Code == "0")
            {
                await JSRuntime.InvokeAsync<object>("alert", "시설물 식별코드가 입력되지 않았습니다.");
            }
            else if (ann.Facility_Position == "" || ann.Facility_Position == null)
            {
                await JSRuntime.InvokeAsync<object>("alert", "시설물 위치가 입력되지 않았습니다.");
            }
            else if (ann.Facility_Sort_Code_A == "" || ann.Facility_Sort_Code_A == null)
            {
                await JSRuntime.InvokeAsync<object>("alert", "대분류가 선택되지 않았습니다.");
            }
            else if (ann.Facility_Sort_Code_B == "" || ann.Facility_Sort_Code_B == null)
            {
                await JSRuntime.InvokeAsync<object>("alert", "중분류가 선택되지 않았습니다.");
            }
            else if (bnn.Facility_Form == "" || bnn.Facility_Form == null)
            {
                await JSRuntime.InvokeAsync<object>("alert", "규격이 입력되지 않았습니다.");
            }
            else if (ann.Facility_Sort_Code_C == "" || ann.Facility_Sort_Code_C == null)
            {
                await JSRuntime.InvokeAsync<object>("alert", "공사종별이 선택되지 않았습니다.");
            }
            else if (bnn.Quantity < 1)
            {
                await JSRuntime.InvokeAsync<object>("alert", "수량이 입력되지 않았습니다.");
            }
            else if (bnn.Unit == "" || bnn.Unit == null || bnn.Unit == "Z")
            {
                await JSRuntime.InvokeAsync<object>("alert", "단위가 선택되지 않았습니다.");
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
                bnn.PostIP = myIPAddress;

                #endregion 아이피 입력

                ann.Apt_Code = Apt_Code;
                ann.Facility_Installation_Date = bnn.Manufacture_Date;
                ann.Facility_Sort_Code_A = strSortA;
                ann.Facility_Sort_Code_B = strSortB;
                //ann.Facility_Code = "F" + DateTimeOffset.Now.ToUnixTimeSeconds().ToString();

                bnn.Apt_Code = Apt_Code;
                bnn.Sort_A = strSortA;
                bnn.Sort_B = strSortB;
                bnn.Sort_C = ann.Facility_Sort_Code_C;
                bnn.User_ID = User_Code;
                bnn.Facility_Detail_Etc = ann.Facility_Etc;
                bnn.Facility_Code = ann.Aid.ToString();

                if (ann.Aid < 1)
                {
                    Add_id = await facility_Lib.Add_Facility(ann); //새로 입력
                    ann = new Facility_Entity();
                    //await DisplayViews();
                    //InsertViewsA = "A";
                }
                else
                {
                    await facility_Lib.Edit_Facility(ann); //수정
                    ann = new Facility_Entity();
                    InsertViewsA = "A";
                }

                if (bnn.Aid < 1)
                {
                    if (Add_id > 0)
                    {
                        bnn.Facility_Code = Add_id.ToString();
                        await facility_Detail_Lib.Add_Facility_Detail(bnn);// 새로 등록
                        await DisplayViews();
                        bnn = new Facility_Detail_Entity();
                        InsertViewsA = "A";
                    }
                    else
                    {
                        await JSRuntime.InvokeAsync<object>("alert", "등록되지 않았습니다.");
                    }
                }
                else
                {
                    await facility_Detail_Lib.Edit_Facility_Detail(bnn);// 수정
                    InsertViewsA = "A";
                    bnn = new Facility_Detail_Entity();
                }
            }
        }

        #endregion 시설물 입출력 관련
    }
}
