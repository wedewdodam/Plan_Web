using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plan_Lib.Company
{
    public interface ICompanySort_Lib
    {
        Task<Company_Sort_Entity> Add_CompanySort(Company_Sort_Entity Company);

        Task<Company_Sort_Entity> Edit_CompanySort(Company_Sort_Entity Sort);

        Task<int> Last_Number();

        Task<List<Company_Sort_Entity>> GetList_CompanySort(string Up_Code);

        Task<List<Company_Sort_Entity>> GetList_CompanySort_All();

        Task<List<Company_Sort_Entity>> GetList_CompanySort_step(string Company_Sort_Step);

        Task<string> DetailName_CompanySort(string Company_Sort_Code);

        Task<int> CompanySort_Being(string Company_Sort_Code);

        Task<string> DetailCode_CompnaySort(string Company_Sort_Code);

        Task<Company_Sort_Entity> Detail_CompanySort_Detail(string Company_Sort_Code);

        Task Remove(int Aid);
    }

    /// <summary>
    /// 업체 정보
    /// </summary>
    public interface ICompany_Lib
    {
        Task<Company_Entity> Add_Company(Company_Entity Company);

        Task<Company_Entity> Edit_Company(Company_Entity Sort);

        Task<int> Last_Number();

        Task<int> totalCount();

        Task<int> CorporRate_Number(string CorporRate_Number);

        Task<List<Company_Entity>> GetList_Company(string SortA_Code, string SortB_Code);

        Task<List<Company_Entity>> List_Company();

        Task<List<Company_Entity_Etc>> List_Detail_Company();

        Task<List<Company_Entity_Etc>> Search_Name_List(string Name);

        Task<List<Company_Entity_Etc>> Search_Cor_Num_List(string CorNum);

        Task<string> Detail_Company_Name(string Company_Code);

        Task<string> Detail_Company_CorNum(string Company_Code);

        Task<int> Detail_Company_CorNum_Number(string Company_Code);

        Task<Company_Entity> Detail_Company_Detail(string Company_Code);

        Task<Company_Entity_Etc> Company_View(string CorporRate_Number);
    }

    /// <summary>
    /// 업체 상세 정보
    /// </summary>
    public interface ICompany_Etc_Lib
    {
        Task<Company_Etc_Entity> Add_CompanyEtc(Company_Etc_Entity Company);

        Task<Company_Etc_Entity> Edit_CompanyEtc(Company_Etc_Entity Sort);

        Task<int> Last_Number();

        Task<List<Company_Etc_Entity>> GetList_CompanyEtc(string Company_Code);

        Task<Company_Entity> Detail_Company_Detail(string CompanyEtc_Code);

        Task<Company_Entity_Etc> Detail_Company_Etc_Detail(string Company_Code);

        Task<Company_Entity_Etc> Detail_Company_Etc_Num(string CorporRate_Number);
    }

    /// <summary>
    /// 업종정보
    /// </summary>
    public interface IBusinessType_Lib
    {
        Task<BusinessType_Entity> Add_BusinessType(BusinessType_Entity BusiType);

        Task<BusinessType_Entity> Edit_BusinessType(BusinessType_Entity Sort);

        Task<int> Last_Number();

        Task<List<BusinessType_Entity>> GetList_BusinessType();

        Task<string> Detail_BusinessType_Name(string BusinessType_Code);

        Task<BusiList_Entity> Detail_BusiList(string BusiList_Code);
    }

    /// <summary>
    /// 선택 업종 정보
    /// </summary>
    public interface IBusiList_Lib
    {
        Task<BusiList_Entity> Add_BusiList(BusiList_Entity BusiType);

        Task Remove_BusiList(int Aid);

        Task<int> Last_Number();

        Task<List<BusiList_Entity>> GetList_BusiList(string CompanyEtc_Code);

        Task<string> Detail_BusinessType_Name(string BusinessType_Code);

        Task<BusinessType_Entity> Detail_BusinessType_Detail(string BusinessType_Code);
    }

    /// <summary>
    /// 공동주택에서 선택한 업체 정보
    /// </summary>
    public interface IApt_Company_Lib
    {
        Task<Apt_Company_Entity> Add_AptCompany(Apt_Company_Entity Apt);

        Task Remove_AptCompany(int Aid);

        Task<int> Last_Number();

        Task<int> Saved_Company(string Apt_Code, string Company_Code);

        Task<List<Apt_Company_Entity>> GetList_Apt_Company(string Apt_Code);

        Task<List<Company_Entity_Etc>> GetList_Apt_Company_CompanyEtc(string Apt_Code);

        Task<List<Apt_Company_Entity>> GetList_Apt_Company_Sort(string Apt_Code, string SortA_Code, string SortB_Code);

        Task<Apt_Company_Entity> Detail_Apt_Company(string Apt_Company_Code);
    }
}