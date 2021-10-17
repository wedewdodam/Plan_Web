using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plan_Blazor_Lib.Record
{
    public interface IRepair_Record_Lib
    {
        Task<Repair_Record_Entity> Add(Repair_Record_Entity _rr);

        Task<Repair_Record_Entity> Edit(Repair_Record_Entity _rr);

        Task<List<Repair_Record_Entity>> GetList(string Apt_Code);

        Task<List<Repair_Record_Entity>> GetList_Year(string Apt_Code, string Repair_Year);

        Task<List<Repair_Record_Entity>> list_Year_List(string apt_Code, string PostStart, string PostEnd);

        Task<List<Repair_Record_Entity>> list_Year_List_New(string apt_Code, string PostEnd);

        Task<List<Repair_Record_Entity>> list_Year_List_all(string apt_Code);

        Task<double> Year_Sum(string apt_Code, string _Start);

        Task<Repair_Record_Entity> Detail(int Aid);

        Task<List<Repair_Record_Entity>> GetList_Sort(string Apt_Code, string Feild, string Query);

        Task<List<Repair_Record_Entity>> GetList_Sort_Year(string Apt_Code, string Feild, string Query, string Repair_Year);

        Task<double> UseCost_Year(string Apt_Code, string Repair_Year);

        Task<double> UseCost(string Apt_Code, int Year_Use);

        Task<double> UseCost_Year_A(string Apt_Code, string Repair_Year);

        Task<double> UseCost_Year_Between(string Apt_Code, string Feild, string Query, int Repair_Year_a, int Repair_Year_b);

        Task Remove(int Aid);

        Task<double> aUsing_Cost(string Apt_Code, string Start_year, string End_year);

        Task<Balance_Year_Entity> balance_Year(string Apt_Code, string Reset_Year);

        Task<double> Using_Cost(string Apt_Code, string Repair_Reset_Date, string Repair_End_Date);

        /// <summary>
        /// 해당 공동주택의 수선이력 리스트
        /// </summary>
        Task<List<Repair_Record_Entity>> List_Apt_all(string Apt_Code);
    }

    /// <summary>
    /// 장기수선충당금 관련 정보 입력 완료 정보 클래스
    /// </summary>
    public interface IAppropriation_Prosess_Lib
    {
        Task<Appropriation_Prosess_Entity> Add(Appropriation_Prosess_Entity _Entity);

        Task<Appropriation_Prosess_Entity> Edit(Appropriation_Prosess_Entity _Entity);

        Task Edit_Complete(string Apt_Code, string Identifition_Name, string Complete);

        Task Edit_Complete_year(string Apt_Code, string Identifition_Name, string Complete_year, string Complete);

        Task<int> being_year(string Apt_Code, string Identifition_Name, string Complete_year);

        Task<int> being(string Apt_Code, string Identifition_Name);

        Task<string> _Year(string Apt_Code, string Identifition_Name);

        Task<string> year_Year(string Apt_Code, string Identifition_Name, string Complete_year);

        Task<string> Complete(string Apt_Code, string Identifition_Name);

        Task<string> Complete_year(string Apt_Code, string Identifition_Name, string Complete_year);
    }
}