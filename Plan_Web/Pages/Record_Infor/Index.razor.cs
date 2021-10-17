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

namespace Plan_Web.Pages.Record_Infor
{
    public partial class Index
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
        /// 대분류 선택 중분류 목록 만들기
        /// </summary>
        private async Task onSortA(ChangeEventArgs a)
        {
            strSortA = a.Value.ToString();
            dnn.Sort_A_Code = strSortA;
            dnn.Sort_A_Name = await facility_Sort_Lib.DetailName_FacilitySort(strSortA);
            fnnB = await facility_Sort_Lib.GetList_FacilitySortA(a.Value.ToString());
            //fnnC = await facility_Sort_Lib.GetList_Sort_AA_List(Apt_Code, Aid, strSortA);
        }

        /// <summary>
        /// 중분류 선택 하면 수선항목 만들기
        /// </summary>
        private async Task onSortB(ChangeEventArgs a)
        {
            strSortB = a.Value.ToString();
            fnnC = await facility_Sort_Lib.GetList_FacilitySort(Apt_Code, Aid, strSortB);
            dnn.Sort_B_Code = strSortB;
            dnn.Sort_B_Name = await facility_Sort_Lib.DetailName_FacilitySort(strSortB);
        }

        /// <summary>
        /// 중분류 선택 하면 수선항목 만들기
        /// </summary>
        private async Task onSortC(ChangeEventArgs a)
        {
            strSortC = a.Value.ToString();
            if (strSortC != "예외지출")
            {
                //dnn.Sort_C_Name = strSortC;
                dnn.Sort_C_Code = strSortC;
                Arnn = await article_Lib.GetListArticleSort(strCode, "Sort_C_Code", strSortC, Apt_Code);
                dnn.Sort_C_Name = await facility_Sort_Lib.DetailName_FacilitySort(strSortC);
            }
            else
            {
                dnn.Sort_C_Code = strSortC;
                dnn.Sort_C_Name = strSortC;
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
                dnn.Repair_Article_Name = strSortD;
            }
            else
            {
                dnn.Repair_Article_Code = strSortD;
                Ccnn = await cost_Lib.Detail_Cost_Article_Year(Apt_Code, strCode, strSortD, Work_Year);
                if (Ccnn != null)
                {                    
                    if (Ccnn.Repair_All_Cost > 0)
                    {
                        dnn.Repair_Plan_Cost = Ccnn.Repair_All_Cost;
                    }
                    else if (Ccnn.Repair_Part_Cost > 0)
                    {
                        dnn.Repair_Plan_Cost = Ccnn.Repair_Part_Cost;
                    }
                    else
                    {
                        dnn.Repair_Plan_Cost = 0;
                    }
                }
                else
                {
                    dnn.Repair_Plan_Cost = 0;
                }
                dnn.Repair_Article_Name = await article_Lib.Article_Name(Convert.ToInt32(strSortD));
                dnn.Repair_Article_Code = strSortD;
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
            strCorporRate_Number = "";
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

        /// <summary>
        /// 업체 검색
        /// </summary>
        private async Task btnCompanySearch(ChangeEventArgs a)
        {
            if (a.Value != null)
            {
               CorporRate_Num = a.Value.ToString().Replace("-", "").Replace("_", "").Replace(",", "");
                intCN = await company_Lib.CorporRate_Number(CorporRate_Num);
                if (intCN > 0)
                {
                    bnn = await company_Lib.ByDetails_Company(CorporRate_Num);
                    cnnE = await company_Etc_Lib.Detail_Company_Etc_Detail(bnn.Company_Code);
                    dnn.CorporRate_Number = bnn.CorporRate_Number;
                    dnn.Company_Name = bnn.Company_Name;
                    dnn.Company_Code = bnn.Company_Code;
                    dnn.Charge_Man = cnnE.Charge_Man;
                    dnn.ChargeMan_mobile = cnnE.ChargeMan_Mobile;
                    strCorporRate_Number = dnn.CorporRate_Number;
                    strCorporRate_Number = strCorporRate_Number.Insert(3, "-");
                    strCorporRate_Number = strCorporRate_Number.Insert(6, "-");
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
        private async Task OnByEdit(Repair_Record_Entity ar)
        {
            dnn = ar;
            strSortA = dnn.Sort_A_Code;
            fnnB = await facility_Sort_Lib.GetList_FacilitySortA(strSortA);
            strSortB = dnn.Sort_B_Code;
            fnnC = await facility_Sort_Lib.GetList_FacilitySort(Apt_Code, Aid, strSortB);
            strSortC = dnn.Sort_C_Code;
            if (strSortC != "예외지출")
            {                
                Arnn = await article_Lib.GetListArticleSort(strCode, "Sort_C_Code", strSortC, Apt_Code);
                strSortD = dnn.Repair_Article_Code;
            }
            else
            {
                strSortD = dnn.Repair_Article_Code;
            }
            InsertRecord = "B";
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
            dnn.Apt_Code = Apt_Code;
            dnn.Repair_Plan_Code = strCode;
            dnn.Staff_Code = User_Code;
            dnn.Repair_Year = dnn.Repair_Start_Date.Year;
            dnn.Repair_Month = dnn.Repair_Start_Date.Month;
            dnn.Repair_Day = dnn.Repair_Start_Date.Day;
            dnn.Repair_End_Year = dnn.Repair_Year;
            dnn.Repair_Division = dnn.Cost_Division;
            if (InOutWork == "A")
            {
                dnn.Work_Division = "외주공사";
            }
            else
            {
                dnn.Work_Division = "내부공사";
            }
            //dnn.Repair_Plan_Cost = await cost_Lib.Being_Article_Cost_Code(Apt_Code, strCode, dnn.Repair_Article_Code);
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

            if (dnn.Sort_A_Code == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "대분류를 선택하지 않았습니다.");
            }
            else if (dnn.Apt_Code == null || dnn.Apt_Code == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "로그인되지 않았습니다.");
                MyNav.NavigateTo("/");
            }
            else if (dnn.Sort_B_Code == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "중분류를 선택하지 않았습니다.");
            }
            else if (dnn.Sort_C_Code == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "소분류를 선택하지 않았습니다.");
            }
            else if (dnn.Repair_Article_Code == null)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "수선항목을 선택하지 않았습니다.");
            }
            else if (dnn.Repair_contract_Cost < 10)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "계약 금액을 입력하지 않았습니다.");
            }
            else if (dnn.Repair_Cost_Complete < 10)
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "공사 완료 금액을 입력하지 않았습니다.");
            }
            else if (dnn.Repair_Division == null || dnn.Repair_Division == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "수선구분을 선택하지 않았습니다.");
            }
            else if (dnn.Cost_Division == null || dnn.Cost_Division == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "지출구분을 선택하지 않았습니다.");
            }
            else if (dnn.CorporRate_Number == null || dnn.CorporRate_Number == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "사업자 번호를 입력하지 않았습니다.");
            }
            else if (dnn.Company_Name == null || dnn.Company_Name == "")
            {
                await JSRuntime.InvokeVoidAsync("exampleJsFunctions.ShowMsg", "업체명을 입력하지 않았습니다.");
            }
            else
            {
                if (dnn.Aid < 1)
                {
                    await repair_Record_Lib.Add(dnn);
                }
                else
                {
                    await repair_Record_Lib.Edit(dnn);
                }
                InsertRecord = "A";
                dnn = new Repair_Record_Entity();
                cnnE = new Company_Entity_Etc();
                await DatailsView(Work_Year);
            }
        }

        /// <summary>
        /// 이력정보 닫기
        /// </summary>
        private void btnClose()
        {
            InsertRecord = "A";
        }

        /// <summary>
        /// 업체정보 등록 열기
        /// </summary>
        private async Task btnCompanyOpen()
        {
            InsertCompany = "B";
            InsertRecord = "A";
            strTitle_Company = "업체정보 등록";
            CsnnA = await companySort_Lib.GetList_CompanySort_step("A");
            strCorporRate_Number = "";
        }

        /// <summary>
        /// 업체정보 입력 닫기
        /// </summary>
        private void btnCloseA()
        {
            InsertCompany = "A";
        }

        /// <summary>
        /// 업체 분류 선택
        /// </summary>
        public string strCompSortA { get; set; }
        public string strCompSortB { get; set; }
        public string Sido_Code { get; set; }
        public string SiGunGu { get; set; }
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
                //ann.CorporateResistration_Num = "";
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
                    await company_Lib.Add_Company(bnn);
                    cnn.Company_Code = bnn.Company_Code;
                    cnn.Cor_Etc = bnn.Company_Etc;
                    cnn.CompanyEtc_Code = bnn.Company_Code;
                    await company_Etc_Lib.Add_CompanyEtc(cnn);
                }
                else
                {
                    await company_Lib.Edit_Company(bnn);
                    cnn.Cor_Etc = bnn.Company_Etc;
                    
                    await company_Etc_Lib.Edit_CompanyEtc(cnn);
                }

                dnn.Company_Code = bnn.Company_Code;
                dnn.Company_Name = bnn.Company_Name;
                dnn.Charge_Man = cnn.Charge_Man;
                dnn.ChargeMan_mobile = cnn.ChargeMan_Mobile;
                
                dnn.CorporRate_Number = bnn.CorporRate_Number;
                strCorporRate_Number = bnn.CorporRate_Number;

                #region 업체정보 로드
                cnnE.CorporRate_Number = bnn.CorporRate_Number;
                cnnE.Capital = cnn.Capital;
                cnnE.Ceo_Mobile = cnn.Ceo_Mobile;
                cnnE.Ceo_Name = cnn.Ceo_Name;
                cnnE.ChargeMan_Mobile = cnn.ChargeMan_Mobile;
                cnnE.Charge_Man = cnn.Charge_Man;
                cnnE.CompanyEtc_Code = cnn.CompanyEtc_Code;
                cnnE.Company_Code = bnn.Company_Code;
                cnnE.Company_Etc = bnn.Company_Etc;
                cnnE.Company_Name = bnn.Company_Name;
                cnnE.Corporation = cnn.Corporation;
                cnnE.Cor_Adress = cnn.Cor_Adress;
                cnnE.Cor_Email = cnn.Cor_Email;
                cnnE.Cor_Etc = cnn.Cor_Etc;
                cnnE.Cor_Fax = cnn.Cor_Fax;
                cnnE.Cor_Gun = cnn.Cor_Gun;
                cnnE.Cor_Sido = cnn.Cor_Sido;
                cnnE.Cor_Tel = cnn.Cor_Tel;
                cnnE.Credit_Rate = cnn.Credit_Rate;
                cnnE.SortA_Code = bnn.SortA_Code;
                cnnE.SortA_Name = bnn.SortA_Name;
                cnnE.SortB_Code = bnn.SortB_Code;
                cnnE.SortB_Name = bnn.SortB_Name;             
                #endregion

                InsertCompany = "A";
                InsertRecord = "B";
            }
        }
    }
}
