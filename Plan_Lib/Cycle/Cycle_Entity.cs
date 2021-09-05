using System;

namespace Plan_Blazor_Lib.Cycle
{
    public class Cycle_Entity
    {
        public int Aid { get; set; }
        public string Apt_Code { get; set; }
        public string Repair_Plan_Code { get; set; }
        public string Repair_Article_Code { get; set; }
        public string Division { get; set; }
        public string Sort_A_Code { get; set; }
        public string Sort_B_Code { get; set; }
        public string Sort_C_Code { get; set; }
        public string Sort_A_Name { get; set; }
        public string Sort_B_Name { get; set; }
        public string Sort_C_Name { get; set; }
        public int Law_Repair_Cycle_All { get; set; }
        public int Set_Repair_Cycle_All { get; set; }
        public int Law_Repair_Cycle_Part { get; set; }
        public int Set_Repair_Cycle_Part { get; set; }
        public int Law_Repair_Rate { get; set; }
        public int Set_Repair_Rate { get; set; }
        public int Repair_Last_Year_All { get; set; }
        public int Repair_Last_Year_Part { get; set; }
        public int Repair_Plan_Year_All { get; set; }
        public int Repair_Plan_Year_Part { get; set; }
        public int All_Cycle_Num { get; set; }
        public int Part_Cycle_Num { get; set; }
        public string Repair_Cycle_Etc { get; set; }
        public string User_ID { get; set; }
        public DateTime Post_Date { get; set; }
        public string Post_IP { get; set; }
    }
}