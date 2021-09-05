using Plan_Blazor_Lib.Plan;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plan_Blazor_Lib.Cycle
{
    public interface ICycle_Lib
    {
        Task<Cycle_Entity> Add_RepairCycle(Cycle_Entity cycle);

        Task<Cycle_Entity> Edit_RepairCycle(Cycle_Entity cycle);

        Task<int> Repeat_RepairCycle(string Repair_Article_Code, string Repair_Plan_Code);

        int Be_RepairCycle(string Repair_Article_Code, string Repair_Plan_Code);

        Task Delete_RepairCycle(int Aid);

        Task Remove_RepairCycle(string Repair_Plan_Code, string Repair_Article_Code);

        Task Delete_Article_Cycle(string Repair_Plan_Code, string Repair_Article_Code);

        Task<int> be_cycle_code(string Repair_Article_Code);

        Task Delete_Cycle_PlanCode(string Repair_Plan_Code);

        Task<List<Join_Article_Cycle_Cost_EntityA>> GetList_RepairCycle(string Apt_Code, string Repair_Plan_Code, string Repair_Article_Code);

        Task<List<Join_Article_Cycle_Cost_EntityA>> GetLIst_RepairCycle_Sort(string Repair_Plan_Code, string Sort_Field, string Sort_Code, string Apt_Code);

        Task<List<Join_Article_Cycle_Cost_EntityA>> GetList_Ago(string Apt_Code, string Feild, string Sort_Code, string Adjustment_Date);

        Task<List<Cycle_Entity>> GetList_Cycle(string Apt_Code, string Repair_Plan_Code);

        Task<Cycle_Entity> Detail_RepairCycle(int Aid);

        Task<Cycle_Entity> Detail_RepairCycle_Article(string Repair_Plan_Code, string Repair_Article_Code);

        Task All_Insert_Code(string Apt_Code, string Repair_Plan_Code_A, string Repair_Plan_Code_B, string Repair_Article_Code, string User_ID, string PostIP);

        Task<int> Repair_Part_Num(string Apt_Code, string Repair_Plan_Code);

        Task<int> Repair_All_Num(string Apt_Code, string Repair_Plan_Code);

        Task<int> Being_Article_Cycle(string Apt_Code, string Repair_Plan_Code, string Repair_Article_Code);

        Task<int> Being_Article_Cycle_Code(string Apt_Code, string Repair_Plan_Code, string Repair_Article_Code);

        Task<Join_Article_Cycle_Cost_EntityA> Detail_Cycle_Article(string Repair_Plan_Code, string Sort_C_Code, string Repair_Article_Name);

        Task<int> Being_Detail_Cycle_Article(string Repair_Plan_Code, string Sort_C_Code, string Repair_Article_Name);

        Task<int> Cycle_Num(string Apt_Code, string Repair_Plan_Code);

        Task<int> Being_Cycle_Article_Code(string Repair_Plan_Code, string Repair_Article_Code);
        Task<string> OnArticleCode(string Repair_Plan_Code, string Sort_C_Code, string Repair_Article_Name);
    }
}