using Plan_Blazor_Lib.Plan;
using Plan_Blazor_Lib.Price;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plan_Blazor_Lib.Cost
{
    public interface ICost_Lib
    {
        /// <summary>
        /// 수선금액 입력하기
        /// </summary>     
        Task<Cost_Entity> Add_Cost(Cost_Entity Cost);

        /// <summary>
        /// 수선금액 수정하기
        /// </summary>
        Task<Cost_Entity> Update_Cost(Cost_Entity Cost);

        /// <summary>
        /// 수선금액 수정하기
        /// </summary>
        Task<Cost_Entity> Update_Cost_A(Cost_Entity Cost);

        /// <summary>
        /// 부분수선금액과 부분수선율만 수정하기
        /// </summary>
        Task Edit_Part_Price(double Repair_Part_Cost, double Repair_Rate, int Repair_Cost_Code);

        /// <summary>
        /// 수선금액 리스트 (수선항목 코드로)
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="Repair_Plan_Code"></param>
        /// <param name="Repair_Article_Code"></param>
        /// <returns></returns>
        Task<Cost_Entity> GetDetail_Cost_RAC(string Apt_Code, string Repair_Plan_Code, string Repair_Article_Code);

        Task<List<Join_Article_Cycle_Cost_EntityA>> GetLIst_RepairCost_Sort(string Repair_Plan_Code, string Sort_Field, string Sort_Code, string Apt_Code);

        Task<List<Join_Article_Cycle_Cost_EntityA>> GetLIst_RepairCost_Sort_New(string Repair_Plan_Code, string Sort_Field, string Sort_Code, string Apt_Code);

        Task<List<Join_Article_Cycle_Cost_EntityA>> GetLIst_RepairCost_Sort_A(string Repair_Plan_Code, string Sort_Field, string Sort_Code, string Apt_Code);

        Task<Cost_Entity> _Cost(string Apt_Code, string Repair_Article_Name);

        Task<int> beCost(string Apt_Code, string Repair_Article_Name);

        Task<Cost_Entity> _Cost_one(string Apt_Code, string Repair_Article_Name);

        Task Remove_Repair_Cost(int Repair_Cost_Code);

        Task Delete_RepairCost_PlanCode(string Repair_Plan_Code);

        Task Remove_Article_Cost(string Repair_Plan_Code, string Repair_Article_Code);

        Task<int> Cost_Count(string Repair_Plan_Code, string Repair_Article_Code);

        Task<int> Being_Cost_Count(string Repair_Plan_Code);

        Task<int> Cost_Total_Article(string Repair_Plan_Code, string Repair_Article_Code);

        double Cost_All_Total(string Apt_Code, string Repair_Plan_Code);

        double Cost_Part_Total(string Apt_Code, string Repair_Plan_Code);

        Task<double> Cost_All_Total_Sort(string Sort_A_Code, string Apt_Code, string Repair_Plan_Code);

        Task<double> Cost_All_Total_Sort_ABC(string Feild, string Query, string Apt_Code, string Repair_Plan_Code);

        Task<double> Cost_Part_Total_Sort_ABC(string Feild, string Query, string Apt_Code, string Repair_Plan_Code);

        Task<int> Cost_Complete_Article_All(string Apt_Code, string Repair_Plan_Code);

        Task<int> Cost_Complete_Article_Sort(string Apt_Code, string Repair_Plan_Code, string Feild, string Query);

        Task<double> Cost_Part_Total_Sort(string Sort_A_Code);

        Task<string> GetAid_Division(string Repair_Plan_Code, string Repair_Article_Code);

        Task<int> GetAid_Last();

        Task All_Insert_Code(string Apt_Code, string Repair_Plan_Code_A, string Repair_Plan_Code_B, string Staff_Code);

        Task<Cost_Entity> Cost_Detail_ArticleCode(string Apt_Code, string Repair_Article_Code);

        Task<int> Being_Cost_Detail_ArticleCode(string Apt_Code, string Repair_Article_Code);

        Task<Join_Article_Cycle_Cost_EntityA> Detail_Cycle_Cost(string Ago_Repair_Plan_Code, string Sort_C_Code, string Repair_Article_Name);

        Task<Join_Article_Cycle_Cost_EntityA> Detail_Artile_Cycle_Cost(string Apt_Code, string Repair_Plan_Code, string Repair_Article_Code);

        Task<Join_Article_Cycle_Cost_EntityA> Detail_Artile_Cycle_Cost_Year(string Apt_Code, string Repair_Plan_Code, string Repair_Article_Code, string Repair_Year);

        Task<Cost_Entity> Detail_Cost(string Apt_Code, string Repair_Cost_Code);

        Task<int> Detail_Cycle_Cost_Count(string Ago_Repair_Plan_Code, string Sort_C_Code, string Repair_Article_Name);

        Task<int> Being_Article_Cost(string Apt_Code, string Repair_Plan_Code, string Repair_Article_Code);

        Task<int> Being_Article_Cost_Code(string Apt_Code, string Repair_Plan_Code, string Repair_Article_Code);

        Task<int> Last_Count(string Apt_Code);

        Task<List<Join_Article_Cycle_Cost_EntityA>> Plan_Repair(string Apt_Code, string Repair_Plan_Code, string Repair_Year);

        Task<Join_Article_Cycle_Cost_EntityA> Detail_Cost_Article_AllCost(string Apt_Code, string Repair_Article_Name);

        Task<List<Join_Article_Cycle_Cost_EntityA>> GetPrint(string Apt_Code, string Repair_Plan_Code);

        Task<Join_Article_Cycle_Cost_EntityA> Detail_Cost_Article_Cost(string Repair_Plan_Code, string Repair_Article_Code, string Repair_Year);

        Task<Cost_Entity> Detail_Cost_Article(string Apt_Code, string Repair_Plan_Code, string Repair_Article_Code);

        Task<Report_Plan_Cost_Entity> Detail_Report_Plan_Cost(string Apt_Code, string Repair_Plan_Code);

        Task<string> Price_Sort_Detail(string Repair_Article_Code);

        Task<double> Sort_total_All_cost(string Apt_Code, string Repair_Plan_Code, string Sort_A_Code);

        Task<double> Sort_total_Part_cost(string Apt_Code, string Repair_Plan_Code, string Sort_A_Code);

        int Repair_Cost_Article_All_Count(string Apt_Code, string Repair_Plan_Code, string Field);

        Task<Plan_Total_Cost_Entity> PlanTotalCost(string Apt_Code, string Repair_Plan_Code);

        /// <summary>
        /// 수선금액 리스트(시설물 분류 코드)
        /// </summary>
        Task<List<Cost_Entity>> GetList(string Apt_Code, string Repair_Plan_Code);

        /// <summary>
        /// 수선금액 리스트
        /// </summary>
        Task<List<Cost_Entity>> GetList_Cost_Ago(string Apt_Code, string Now_Code, string Ago_Code);

        /// <summary>
        ///  해당 장기수선계획에서 수선항목코드로 수선금액 정보 불러오기
        /// </summary>
        Task<Cost_Entity> Detail_Cost_Article_Year(string Apt_Code, string Repair_Plan_Code, string Repair_Article_Code, string Year);
    }
}