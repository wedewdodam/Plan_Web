using System.Collections.Generic;
using System.Threading.Tasks;

namespace Facility
{
    public interface IFacility_Lib
    {
        Task<int> Add_Facility(Facility_Entity Facility);

        Task<Facility_Entity> Edit_Facility(Facility_Entity Facility);

        Task<int> Last_Number();

        Task<List<Facility_Entity>> GetList_Facility(string Apt_Code);

        Task<List<Facility_Entity>> GetList_A(string Apt_Code);

        Task<List<Facility_Entity>> GetList_Facility_Sort_A(string Apt_Code, string Facility_Sort_Code_A);

        Task<List<Facility_Entity>> GetList_Facility_Sort_B(string Apt_Code, string Facility_Sort_Code_B);

        Task<List<Facility_Entity>> GetList_Facility_Sort_C(string Apt_Code, string Facility_Sort_Code_C);

        Task<Facility_Entity> Detail_Facility(string Facility_Code);

        Task<List<Facility_Entity>> GetList_Apt_Sort(string Apt_Code, string Sort);

        Task<int> Repeat_Article(string Facility_Name);

        Task Remove(string Aid);

        Task<List<Facility_Entity>> GetList_Apt(int Page, string Apt_Code);

        Task<int> GetList_Apt_Count(string Apt_Code);

        Task<List<Facility_Entity>> GetList_Apt_Query(int Page, string Apt_Code, string Feild, string Query);

        Task<int> GetList_Apt_Query_Count(string Apt_Code, string Feild, string Query);
    }

    public interface IFacility_Detail_Lib
    {
        Task<Facility_Detail_Entity> Add_Facility_Detail(Facility_Detail_Entity Fac_D);

        Task<Facility_Detail_Entity> Edit_Facility_Detail(Facility_Detail_Entity Fac_D);

        Task<Facility_Detail_Entity> Detail_Facility_Detail(int Aid);

        Task<List<Facility_Detail_Entity>> GetList_Facility_Detail(string Facility_Code);

        Task<Facility_Detail_Entity> Detail_Facility_Detail_FacilityCode(string Facility_Code);

        Task<int> Detail_Facility_Detail_FacilityCode_Count(string Facility_Code);

        Task<int> Last_Number();

        Task<int> Repeat_Check(string Facility_Code);
    }

    public interface IFacility_Explanation_Lib
    {
        Task<Facility_Explanation_Entity> Add_Facility_Explanation(Facility_Explanation_Entity Facility);

        Task<Facility_Explanation_Entity> Edit_Facility_Explanation(Facility_Explanation_Entity Facility);

        Task<Facility_Explanation_Entity> Detail_Facility_Explanation(string Facility_Sort_Code);

        Task Remove_Facility(int Aid);

        Task<int> Repeat_Facility(string Facility_Sort_Code);
    }

    public interface IAdditional_Welfare_Facility_Lib
    {
        Task<Additional_Welfare_Facility> Add_AdditionalWelfareFacility(Additional_Welfare_Facility AWF);

        Task<Additional_Welfare_Facility> Edit_AdditionalWelfareFacility(Additional_Welfare_Facility AWF);

        Task<List<Additional_Welfare_Facility>> GetList_AdditionalWelfareFacility(string Apt_Code, string Facility_Division);

        Task<int> Being_Repeat(string Facility_Name, string Facitlty_Position, string Apt_Code);

        Task<Additional_Welfare_Facility> Detail_AdditionalWelfareFacility(string Aid);

        Task Remove(string Aid);

        Task<int> Being(int Aid);

        Task<int> BeingCount(string Apt_Code, string Division);
    }

    public interface IFacility_Sort_Lib
    {
        Task<Facility_Sort_Entity> Add_FacilitySort(Facility_Sort_Entity model);

        Task<Facility_Sort_Entity> Edit_FacilitySort(Facility_Sort_Entity model);

        Task<Facility_Sort_Entity> Details_FacilitySort(string Aid);

        Task<int> Last_Number();

        Task<int> Sort_Code_Last_Number(string Facility_Sort_Code);

        Task<List<Facility_Sort_Entity>> GetList_A_FacilitySort();

        Task<List<Facility_Sort_Entity>> GetList_C_FacilitySort();

        Task<List<Facility_Sort_Entity>> GetList_C_FacilitySortA(string Plan_Code, string Sort);

        Task<List<Facility_Sort_Entity>> GetList_C_FacilitySort_A(string Sort);

        Task<List<Facility_Sort_Entity>> GetList_C_SortB(string Up_Code);

        Task<List<Facility_Sort_Entity>> GetList_C_Sort_B(string Up_Code, string Plan_Code);

        Task<List<Facility_Sort_Entity>> GetList_FacilitySort(string Apt_Code, string Repair_Plan_Code, string Up_Code);
        Task<List<Facility_Sort_Entity>> GetList_FacilitySortA(string Up_Code);

        Task<List<Facility_Sort_Entity>> GetList_Sort_A_List(string Sort_A_Cod);
        Task<List<Facility_Sort_Entity>> GetList_Sort_AA_List(string Apt_Code, string Repair_Plan_Code, string Sort_A_Code);

        Task<string> DetailName_FacilitySort(string Aid);

        Task<string> Detail_FacilitySort_Division(string Aid);

        Task<Facility_Sort_Entity> Detail_Sort(string Aid);

        Task<string> DetailCode_FacilitySort(string Aid);

        Task<string> FacilitySort_Order(string Aid);
    }
}