using Facility;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Plan_Apt_Lib;
using Plan_Blazor_Lib;
using Plan_Blazor_Lib.Plan;
using Plan_Blazor_Lib.Record;
using Plan_Lib.Company;
using Plan_Lib.Pund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plan_Web.Pages.Company
{
    public partial class Index
    {
        #region MyRegion
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateRef { get; set; }
        [Parameter] public string Aid { get; set; }
        [Inject] public NavigationManager MyNav { get; set; } // Url
        [Inject] private IJSRuntime JSRuntime { get; set; }
        [Inject] public IAptInfor_Lib aptInfor_Lib { get; set; }
        [Inject] public IRepair_Plan_Lib repair_Plan_Lib { get; set; }
        [Inject] public ICompany_Lib company_Lib { get; set; }
        [Inject] public ICompany_Etc_Lib company_Etc_Lib { get; set; }
        [Inject] public ICompanySort_Lib companySort_Lib { get; set; }
        [Inject] public IUnit_Price_Lib unit_Price_Lib { get; set; }
        [Inject] public ILevy_Rate_Lib levy_Rate_Lib { get; set; }
        [Inject] public ICapital_Levy_Lib capital_Levy_Lib { get; set; }
        [Inject] public IFacility_Sort_Lib facility_Sort_Lib { get; set; }
        [Inject] public IArticle_Lib article_Lib { get; set; }
        [Inject] public ISido_Lib sido_Lib { get; set; }
        [Inject] public IRepair_Record_Lib repair_Record_Lib { get; set; }

        //private Repair_Plan_Entity rpn { get; set; } = new Repair_Plan_Entity();
        List<Company_Sort_Entity> CsnnA { get; set; } = new List<Company_Sort_Entity>();
        List<Company_Sort_Entity> CsnnB { get; set; } = new List<Company_Sort_Entity>();
        //private List<Facility_Sort_Entity> fnnC { get; set; } = new List<Facility_Sort_Entity>();
        //private List<Article_Entity> Arnn { get; set; } = new List<Article_Entity>();
        private List<Company_Entity_Etc> ann { get; set; } = new List<Company_Entity_Etc>();
        private Company_Entity_Etc dnn { get; set; } = new Company_Entity_Etc();
        Company_Entity bnn { get; set; } = new Company_Entity();
        Company_Etc_Entity cnn { get; set; } = new Company_Etc_Entity();
        List<Sido_Entity> sidos { get; set; } = new List<Sido_Entity>();
        List<Repair_Record_Entity> rrnn { get; set; } = new List<Repair_Record_Entity>();

        private string strSortA;
        private string CorporRate_Num;
        private string CompName;
        //private string strSortD;
        public string Apt_Code { get; set; }
        public string User_Code { get; set; }
        public string Apt_Name { get; set; }
        public string User_Name { get; set; }
        public string BuildDate { get; set; }
        public int LevelCount { get; set; } = 0;
        public string InsertViews { get; set; } = "A";
        public string DataViews { get; set; } = "A";
        public string strTitle { get; set; }
        public string NowYear { get; private set; }
        public string NextYear { get; private set; }

        private string strCode { get; set; }
        public string ViewsCompany { get; set; } = "A";
        //private string strTender { get; set; }
        //private string strTenderA { get; set; }
        //private string strTenderB { get; set; }
        //private string strTenderC { get; set; }
        public string Work_Year { get; private set; }

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

            await DatailsView(Work_Year);

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
                LevelCount = Convert.ToInt32(authState.User.Claims.FirstOrDefault(c => c.Type == "LevelCount")?.Value);
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
            pager.RecordCount = await company_Lib.List_Page_Company_Count();
            ann = await company_Lib.List_Page_Company(pager.PageIndex);
            CsnnA = await companySort_Lib.GetList_CompanySort_step("A");
        }

        /// <summary>
        /// 업체정보 입력
        /// </summary>
        private void btnOpen()
        {
            bnn = new Company_Entity();
            cnn = new Company_Etc_Entity();
            InsertViews = "B";
        }

        /// <summary>
        /// 업체 정보 수정
        /// </summary>
        private async Task OnByEdit(Company_Entity_Etc ar)
        {
            if (LevelCount > 5)
            {
                strCompSortA = ar.SortA_Code;
                strCompSortB = ar.SortB_Code;
                #region MyRegion
                if (ar.Cor_Sido == "서울특별시")
                {
                    Sido_Code = "A";
                }
                else if (ar.Cor_Sido == "경기도")
                {
                    Sido_Code = "B";
                }
                else if (ar.Cor_Sido == "부산광역시")
                {
                    Sido_Code = "C";
                }
                else if (ar.Cor_Sido == "대구광역시")
                {
                    Sido_Code = "D";
                }
                else if (ar.Cor_Sido == "인천광역시")
                {
                    Sido_Code = "E";
                }
                else if (ar.Cor_Sido == "광주광역시")
                {
                    Sido_Code = "F";
                }
                else if (ar.Cor_Sido == "대전광역시")
                {
                    Sido_Code = "G";
                }
                else if (ar.Cor_Sido == "울산광역시")
                {
                    Sido_Code = "H";
                }
                else if (ar.Cor_Sido == "세종특별자치시")
                {
                    Sido_Code = "I";
                }
                else if (ar.Cor_Sido == "충청남도")
                {
                    Sido_Code = "J";
                }
                else if (ar.Cor_Sido == "충청북도")
                {
                    Sido_Code = "K";
                }
                else if (ar.Cor_Sido == "경상남도")
                {
                    Sido_Code = "L";
                }
                else if (ar.Cor_Sido == "경상북도")
                {
                    Sido_Code = "M";
                }
                else if (ar.Cor_Sido == "전라남도")
                {
                    Sido_Code = "N";
                }
                else if (ar.Cor_Sido == "전라남도")
                {
                    Sido_Code = "O";
                }
                else if (ar.Cor_Sido == "강원도")
                {
                    Sido_Code = "P";
                }
                else if (ar.Cor_Sido == "제주특별자치도")
                {
                    Sido_Code = "Q";
                }
                #endregion
                CsnnB = await companySort_Lib.GetList_CompanySort(strCompSortA);
                sidos = await sido_Lib.GetList(ar.Cor_Sido);
                cnn.Cor_Gun = ar.Cor_Gun;
                strCorporRate_Number = cnr(ar.CorporRate_Number);

                bnn = await company_Lib.Detail_Company_Detail(ar.Company_Code);
                cnn = await company_Etc_Lib.Detail_Company_Detail(ar.Company_Code);
                InsertViews = "B";
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "수정 권한이 없습니다. \n 수정을 원하시면 관리자에게 문의하세요.");
            }
        }

        /// <summary>
        /// 업체정보 삭제
        /// </summary>
        private async Task OnRemove(int Aid)
        {
            if (LevelCount > 5)
            {
                bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{Aid}번 글을 정말로 삭제하시겠습니까?");
                if (isDelete)
                {
                    await company_Lib.ByDelete_Company(Aid);
                    await DatailsView(Work_Year);
                }
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "삭제 권한이 없습니다. \n 삭제를 원하시면 관리자에게 문의하세요.");
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
        /// 상위분류 선택 시 실행
        /// </summary>
        private async Task OnSortA(ChangeEventArgs a)
        {
            if (a.Value != null && a.Value.ToString() != "")
            {
                strSortA = a.Value.ToString();
                CsnnB = await companySort_Lib.GetList_CompanySort(strSortA);
                pager.RecordCount = await company_Lib.List_Page_Company_Count_Search("SortA_Code", strSortA);
                ann = await company_Lib.List_Page_Company_Search(pager.PageIndex, "SortA_Code", strSortA);
            }
        }

        /// <summary>
        /// 사업자 번호로 검색
        /// </summary>
        private async Task OnCorporRate_Num(ChangeEventArgs a)
        {
            if (a.Value != null && a.Value.ToString() != "")
            {
                CorporRate_Num = a.Value.ToString();
                pager.RecordCount = await company_Lib.List_Page_Company_Count_Search("CorporRate_Number", CorporRate_Num);
                ann = await company_Lib.List_Page_Company_Search(pager.PageIndex, "CorporRate_Number", CorporRate_Num);
            }
        }

        /// <summary>
        /// 업체명으로 검색
        /// </summary>
        private async Task OnCompName(ChangeEventArgs a)
        {
            if (a.Value != null && a.Value.ToString() != "")
            {
                CompName = a.Value.ToString();
                pager.RecordCount = await company_Lib.List_Page_Company_Count_Search("Company_Name", CompName);
                ann = await company_Lib.List_Page_Company_Search(pager.PageIndex, "Company_Name", CompName);
            }
        }

        /// <summary>
        /// 업체정보 입력 닫기
        /// </summary>
        private void btnCloseA()
        {
            InsertViews = "A";
        }

        /// <summary>
        /// 업체정보 입력 닫기
        /// </summary>
        private void btnCloseB()
        {
            ViewsCompany = "A";
        }

        /// <summary>
        /// 업체 정보 상세
        /// </summary>
        private async Task OnByDetails(Company_Entity_Etc ar)
        {
            dnn = ar;
            rrnn = await repair_Record_Lib.List_Apt_all(dnn.Company_Code);
            ViewsCompany = "B";
        }

        /// <summary>
        /// 사업자 번호 바 넣기
        /// </summary>
        private string cnr(string cn)
        {
            string strCorporRate_Number = cn;
            strCorporRate_Number = strCorporRate_Number.Insert(3, "-");
            strCorporRate_Number = strCorporRate_Number.Insert(6, "-");
            return strCorporRate_Number;
        }


        #region 업체정보 새로 등록 메서드 모음
        /// <summary>
        /// 업체 분류 선택
        /// </summary>
        public string strCompSortA { get; set; }
        public string strCompSortB { get; set; }
        public string Sido_Code { get; set; }
        public string SiGunGu { get; set; }
        public string strCorporRate_Number { get; private set; }

        private async Task onCompSortA(ChangeEventArgs a)
        {
            strCompSortA = a.Value.ToString();
            bnn.SortA_Code = strCompSortA;
            bnn.SortA_Name = await companySort_Lib.DetailName_CompanySort(bnn.SortA_Code);
            CsnnB = await companySort_Lib.GetList_CompanySort(bnn.SortA_Code);
        }
        private async Task onCompSortB(ChangeEventArgs a)
        {
            strCompSortB = a.Value.ToString();
            bnn.SortB_Code = strCompSortB;
            bnn.SortB_Name = await companySort_Lib.DetailName_CompanySort(bnn.SortB_Code);
        }

        /// <summary>
        /// 사업자 번호 중복 체크
        /// </summary>
        private async Task btnCompanySearchOn(ChangeEventArgs a)
        {
            bnn.CorporRate_Number = a.Value.ToString().Replace("-", "").Replace(" ", "");
            int intR = await company_Lib.CorporRate_Number(bnn.CorporRate_Number);

            if (intR < 1)
            {
                bool tr = checkCpIdenty(bnn.CorporRate_Number);

                //licities.Add(new SelectListItem { Text = "::분류선택::", Value = "0" });
                if (tr == true)
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", bnn.CorporRate_Number + "는 입력 가능한 사업자 번호 입니다....");
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", bnn.CorporRate_Number + "는 잘못된 사업자 등록 번호 입니다. 다시 입력해 주세요...");
                    bnn.CorporRate_Number = "";
                }
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", bnn.CorporRate_Number + "는 이미 입력된 사업자 등록 번호 입니다. 다시 입력해 주세요...");
                bnn.CorporRate_Number = "";
            }
        }

        /// <summary>
        /// 사업자번호 체크
        /// </summary>
        public bool checkCpIdenty(string cpNum)
        {
            cpNum = cpNum.Replace("-", "");
            if (cpNum.Length != 10)
            {
                return false;
            }
            int sum = 0;
            string checkNo = "137137135";

            // 1
            for (int i = 0; i < checkNo.Length; i++)
            {
                sum += (int)Char.GetNumericValue(cpNum[i]) * (int)Char.GetNumericValue(checkNo[i]);
            }

            // 2
            sum += (int)Char.GetNumericValue(cpNum[8]) * 5 / 10;

            // 3
            sum %= 10;

            // 4
            if (sum != 0)
            {
                sum = 10 - sum;
            }

            // 5
            if (sum != (int)Char.GetNumericValue(cpNum[9]))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 시도 선택 시 실행
        /// </summary>
        private async Task OnSido(ChangeEventArgs a)
        {
            Sido_Code = a.Value.ToString();
            cnn.Cor_Sido = await sido_Lib.SidoName(Sido_Code);
            sidos = await sido_Lib.GetList(cnn.Cor_Sido);
        }

        /// <summary>
        /// 시군구 선택 시 실행
        /// </summary>
        private void onGunGu(ChangeEventArgs a)
        {
            cnn.Cor_Gun = a.Value.ToString();
        }

        /// <summary>
        /// 업체 새로 등록
        /// </summary>
        private async Task btnCompanySave()
        {
            bnn.Apt_Code = Apt_Code;
            bnn.Staff_Code = User_Code;
            cnn.Staff_Code = User_Code;
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
            cnn.PostIP = myIPAddress;
            #endregion 아이피 입력
            bnn.Company_Code = "Cc" + Apt_Code + await company_Lib.Last_Number();

            if (bnn.Apt_Code == null || bnn.Apt_Code == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인되지 않았습니다.");
            }
            else if (bnn.SortA_Code == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "업체 상위분류를 선택하지 않았습니다.");
            }
            else if (bnn.SortB_Code == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "업체 하위분류를 선택하지 않았습니다.");
            }
            else if (bnn.CorporRate_Number == null || bnn.CorporRate_Number == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "사업자 등록 번호가 입력되지 않았습니다.");
            }
            else if (bnn.Company_Name == null || bnn.Company_Name == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "업체명이 입력되지 않았습니다.");
            }
            else if (cnn.Ceo_Name == null || cnn.Ceo_Name == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "대표자 명이 입력되지 않았습니다.");
            }
            else if (cnn.Cor_Sido == null || cnn.Cor_Sido == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "시도가 선택되지 않았습니다.");
            }
            else if (cnn.Cor_Gun == null || cnn.Cor_Gun == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "시군구가 선택되지 않았습니다.");
            }
            else if (cnn.Cor_Adress == null || cnn.Cor_Adress == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "상세주소가 입력되지 않았습니다.");
            }
            else if (cnn.Cor_Tel == null || cnn.Cor_Tel == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "대표 전화가 입력되지 않았습니다.");
            }
            else
            {

                if (bnn.Aid < 1)
                {
                    int intBe = await company_Lib.CorporRate_Number(bnn.CorporRate_Number);
                    if (intBe < 1)
                    {
                        await company_Lib.Add_Company(bnn);
                        cnn.Company_Code = bnn.Company_Code;
                        cnn.Cor_Etc = bnn.Company_Etc;
                        cnn.CompanyEtc_Code = bnn.Company_Code;
                        await company_Etc_Lib.Add_CompanyEtc(cnn);
                    }
                    else
                    {
                        await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "이미 입력된 사업자 번호를 입력하려 했습니다.");
                    }
                }
                else
                {
                    await company_Lib.Edit_Company(bnn);
                    cnn.Cor_Etc = bnn.Company_Etc;

                    await company_Etc_Lib.Edit_CompanyEtc(cnn);
                }
                await DatailsView(Work_Year);
                strCorporRate_Number = bnn.CorporRate_Number;
                InsertViews = "A";

            }
        }
        #endregion

        /// <summary>
        /// 업체 정보 수정
        /// </summary>
        private async Task btnCompanyEdit(Company_Entity_Etc ar)
        {
            if (LevelCount > 5)
            {
                strCompSortA = ar.SortA_Code;
                strCompSortB = ar.SortB_Code;
                #region MyRegion
                if (ar.Cor_Sido == "서울특별시")
                {
                    Sido_Code = "A";
                }
                else if (ar.Cor_Sido == "경기도")
                {
                    Sido_Code = "B";
                }
                else if (ar.Cor_Sido == "부산광역시")
                {
                    Sido_Code = "C";
                }
                else if (ar.Cor_Sido == "대구광역시")
                {
                    Sido_Code = "D";
                }
                else if (ar.Cor_Sido == "인천광역시")
                {
                    Sido_Code = "E";
                }
                else if (ar.Cor_Sido == "광주광역시")
                {
                    Sido_Code = "F";
                }
                else if (ar.Cor_Sido == "대전광역시")
                {
                    Sido_Code = "G";
                }
                else if (ar.Cor_Sido == "울산광역시")
                {
                    Sido_Code = "H";
                }
                else if (ar.Cor_Sido == "세종특별자치시")
                {
                    Sido_Code = "R";
                }
                else if (ar.Cor_Sido == "충청남도")
                {
                    Sido_Code = "J";
                }
                else if (ar.Cor_Sido == "충청북도")
                {
                    Sido_Code = "K";
                }
                else if (ar.Cor_Sido == "경상남도")
                {
                    Sido_Code = "L";
                }
                else if (ar.Cor_Sido == "경상북도")
                {
                    Sido_Code = "M";
                }
                else if (ar.Cor_Sido == "전라남도")
                {
                    Sido_Code = "N";
                }
                else if (ar.Cor_Sido == "전라남도")
                {
                    Sido_Code = "O";
                }
                else if (ar.Cor_Sido == "강원도")
                {
                    Sido_Code = "P";
                }
                else if (ar.Cor_Sido == "제주특별자치도")
                {
                    Sido_Code = "Q";
                }
                #endregion
                CsnnB = await companySort_Lib.GetList_CompanySort(strCompSortA);
                sidos = await sido_Lib.GetList(ar.Cor_Sido);
                cnn.Cor_Gun = ar.Cor_Gun;
                strCorporRate_Number = cnr(ar.CorporRate_Number);

                bnn = await company_Lib.Detail_Company_Detail(ar.Company_Code);
                cnn = await company_Etc_Lib.Detail_Company_Detail(ar.Company_Code);
                InsertViews = "B";
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "수정 권한이 없습니다. \n 수정을 원하시면 관리자에게 문의하세요.");
            }
        }
    }
}
