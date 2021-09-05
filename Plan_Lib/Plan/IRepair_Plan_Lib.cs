using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plan_Blazor_Lib.Plan
{
    public interface IRepair_Plan_Lib
    {
        Task<Repair_Plan_Entity> Add_Repair_Plan(Repair_Plan_Entity Plan);

        Task<Repair_Plan_Entity> Add_Repair_Plan_Add(Repair_Plan_Entity model);

        Task<Repair_Plan_Entity> Edit_Repair_Plan(Repair_Plan_Entity model);

        Task Edit_Complete(string Repair_Plan_Code, string Complete);

        Task<string> Last_Code(string Apt_Code);

        Task<int> Last_Number();

        Task<int> Last_Aid_Apt(string Apt_Code);

        Task<int> New_Code(string Apt_Code);

        Task<int> Plan_Num_Last(string Apt_Code);

        Task<string> Last_Apt_Code(string Apt_Code);

        Task<string> New_Apt_Code(string Apt_Code);

        Task<int> Last_Number_Apt(string Apt_Code);

        Task<int> Regular_Number_Apt(string Apt_Code);

        Task<string> Regular_Code_Apt(string Apt_Code);

        Task<int> Repeat_Number(string Repair_Plan_Code);

        Task<Repair_Plan_Entity> Detail_Repair_Plan(string Apt_Code, string Repair_Plan_Code);

        Task<Repair_Plan_Entity> Detail_Apt_Last_Plan(string Apt_Code);

        Task<Repair_Plan_Entity> Detail_Apt_Regular_Plan(string Apt_Code);

        Task<Repair_Plan_Entity> Detail_Apt_Ferst_Plan(string Apt_Code);

        Task<Repair_Plan_Entity> Detail_RepairPlan_Aid(int Aid);

        Task Remove_AptCompany(int Aid);

        Task Delete_RepairPlan_PlanCode(string Repair_Plan_Code);

        Task<List<Repair_Plan_Entity>> GetList_Repair_Plan_Apt(int Page, string Apt_Code);
        Task<int> GetList_Repair_Plan_Apt_Count(string Apt_Code);

        Task<List<Repair_Plan_Entity>> GetList_Repair_Plan_Apt_New(string Apt_Code);

        Task<List<Repair_Plan_Entity>> GetList_Repair_Plan_Apt_Year(string Apt_Code, string Adjustment_Year);

        Task<List<Repair_Plan_Entity>> GetList_Repair_Plan();

        Task<DateTime> Repair_Plan_AdjustmentDate(string Apt_Code);

        Task<int> BeComplete_Count(string Apt_Code);

        Task<int> BeComplete_Count_A(string Apt_Code);

        Task<string> BeComplete_Code(string Apt_Code);

        Task<string> BeComplete_Code_A(string Apt_Code);

        Task<string> Repair_Plan_Code(string Apt_Code);

        Task<string> Review_Code(string Repair_Plan_Code, string Apt_Code);

        Task<int> Repair_Plan_Period_Code(string Repair_Plan_Code);

        Task<int> Repair_Plan_Period(string Apt_Code, string Repair_Plan_Code);

        Task<DateTime> Plan_Date(string Apt_Code, string Repair_Plan_Code);

        Task<string> Small_Sum(string Apt_Code);

        Task<int> Being_Plan(string Apt_code);

        Task<int> Being_Plan_None_Complete(string Apt_code);

        Task<int> Being_Plan_Review(string Apt_code, string Plan_Review_Code);

        Task<List<Repair_Plan_Entity>> Getlist_Adjustment_Year(string Apt_Code);

        Task<string> Getlist_Adjustment_Year_Plan(string Apt_Code, string Repair_Plan_Code);

        Task<DateTime> Adjustment_Plan_Date(string Apt_Code, string Repair_Plan_Code);

        Task<int> Being_Count(string Apt_Code);

        Task<DateTime> Adjustment_Date(string Apt_Code, string Repair_Plan_Code);

        Task<List<Join_Article_Cycle_Cost_EntityA>> Year_Plan_practice(string Apt_Code, string Repair_Plan_Code, string StartYear, string EndYear);

        Task<Repair_Plan_practice_total_Entity> Year_Plan_Cost_Totay(string Apt_Code, string Repair_Plan_Code, string StartYear, string EndYear);
    }

    public interface IRepair_SmallSum_Object_Lib
    {
        Task<Repair_SmallSum_Object_Entity> Add_SmallSum_Object(Repair_SmallSum_Object_Entity Plan);

        Task<Repair_SmallSum_Object_Entity> Edit_SmallSum_Object(Repair_SmallSum_Object_Entity Plan);

        Task<int> Last_Number();

        Task<Repair_SmallSum_Object_Entity> Detail_SmallSum_Object(string Repair_SmallSum_Object_Code);

        Task Remove_SmallSum_Object(int Aid);

        Task Delete_SmallSum_Object(int Aid, string Del);

        Task<List<Repair_SmallSum_Object_Entity>> GetList_SmallSum_Object(string Del);

        Task<int> BeCount(string Apt_Code);
    }

    public interface IRepair_SmallSum_Requirement_Lib
    {
        Task<Repair_SmallSum_Requirement_Entity> Add_SmallSum_Requirement(Repair_SmallSum_Requirement_Entity RSR);

        Task<Repair_SmallSum_Requirement_Entity> Edit_SmallSum_Requirement(Repair_SmallSum_Requirement_Entity RSR);

        Task<List<Repair_SmallSum_Requirement_Entity>> GetList_SmallSum_Requirement();

        Task Delete_SmallSum_Requirement(int Aid);
    }

    public interface IRepair_Object_Selection_Lib
    {
        Task<Repair_SmallSum_Object_Selection_Entity> Add_RSO_Selection(Repair_SmallSum_Object_Selection_Entity RSOS);

        Task<Repair_SmallSum_Object_Selection_Entity> Edit_RSOS(Repair_SmallSum_Object_Selection_Entity RSOS);

        Task Edit_RSOS_A(string Aid, string SmallSum_Object_Content);

        Task Delete_ObjetSelection_PlanCode(string Repair_Plan_Code);

        Task<int> Last_Number();

        Task<int> BeCount(string Apt_Code);

        Task Remove_SmallSum_Object_Selection(int Aid);

        Task Delete_SmallSum_Object(int Aid, string Del);

        Task<List<Repair_SmallSum_Object_Selection_Entity>> GetList_RSOS(string Repair_Plan_Code, string Del);

        Task<Repair_SmallSum_Requirement_Entity> Detail_SmallSum_Requirement(string SmallSum_Requirement_Code);

        Task<string> SmallSum_Requirement_Name(string SmallSum_Requirement_Code);

        int Being_Code(string Apt_Code, string Repair_Plan_Code, string SmallSum_Object_Code);
        Task Remove(string Apt_Code, string Repair_Plan_Code, string SmallSum_Object_Code);
    }

    public interface ISmallSum_Repuirement_Selection_Lib
    {
        Task<Repair_SmallSum_Requirement_Selection_Entity> Add_RSR_Selection(Repair_SmallSum_Requirement_Selection_Entity RSRS);

        Task<Repair_SmallSum_Requirement_Selection_Entity> Edit_RSOS(Repair_SmallSum_Requirement_Selection_Entity RSOS);

        Task Edit_RSOS_A(string Aid, string SmallSum_Requirement_Content);

        int Being_Code(string Apt_Code, string Repair_Plan_Code, string SmallSum_Requirement_Code);

        Task<int> BeCount(string Apt_Code);

        Task Remove_SmallSum_Requirement_Selection(int Aid);
        Task Remove(string Apt_Code, string Repair_Plan_Code, string SmallSum_Requirement_Code);

        Task Delete_RequirementSelection_PlanCode(string Repair_Plan_Code);

        Task<List<Repair_SmallSum_Requirement_Selection_Entity>> GetList_RSRS(string Repair_Plan_Code);
    }
}