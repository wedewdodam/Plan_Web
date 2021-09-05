using System;

namespace Plan_Lib.Pund
{
    /// <summary>
    /// 장충금 월 부과 금액 엔터티
    /// </summary>
    public class Capital_Levy_Entity
    {
        public int Capital_Levy_Code { get; set; }
        public string Apt_Code { get; set; }
        public string Levy_Rate_Code { get; set; }
        public double Law_Levy_Month_Sum { get; set; }
        public double Levy_Month_Sum { get; set; }
        public string Levy_Account { get; set; }
        public int Levy_Year { get; set; }
        public int Levy_Month { get; set; }
        public int Levy_Day { get; set; }
        public DateTime Levy_Date { get; set; }
        public string Capital_Levy_Etc { get; set; }
        public string Staff_Code { get; set; }
        public DateTime PostDate { get; set; }
        public string PostIP { get; set; }
    }

    /// <summary>
    /// 장충금 적립율 엔터티(관리규약)
    /// </summary>
    public class Levy_Rate_Entity
    {
        public string Num { get; set; }
        public int Levy_Rate_Code { get; set; }
        public string Apt_Code { get; set; }
        public DateTime Bylaw_Date { get; set; }
        public string Bylaw_Code { get; set; }
        public int Levy_Start_Year { get; set; }
        public int Levy_Start_Month { get; set; }
        public int Levy_Start_Day { get; set; }
        public DateTime Levy_Start_Date { get; set; }
        public int Levy_End_Year { get; set; }
        public int Levy_End_Month { get; set; }
        public int Levy_End_Day { get; set; }
        public DateTime Levy_End_Date { get; set; }
        public int Levy_Period { get; set; }
        public int Levy_Period_New { get; set; }
        public double Levy_Rate { get; set; }
        public double Levy_Rate_Accumulate { get; set; }
        public string Staff_Code { get; set; }
        public DateTime PostDate { get; set; }
        public string PostIP { get; set; }
        public double Supply_Area { get; set; }
        public int Area_Family_Num { get; set; }

        public string Family_Month_Rate_Cost { get; set; }

        public string Family_Year_Rate_Cost { get; set; }

        public string Area_Rate_Month { get; set; }
        public string Area_Rate_Year { get; set; }
        public string supply_total_Area { get; set; }
        public string Total_Cost_Month { get; set; }
        public string Total_Cost_Year { get; set; }
        public string Unit_Price { get; set; }
    }

    /// <summary>
    /// 장충금 초기 입력 값 엔터티
    /// </summary>
    public class Repair_Capital_Entity
    {
        public int Repair_Capital_Code { get; set; }
        public string Apt_Code { get; set; }
        public double Balance_Capital { get; set; }
        public double Levy_Capital { get; set; }
        public double Use_Cost { get; set; }
        public DateTime PostDate { get; set; }
        public string Staff_Code { get; set; }
        public string PostIP { get; set; }
    }

    /// <summary>
    /// 장기수선충당금 사용 입력 값 엔터티
    /// </summary>
    public class Repair_Capital_Use_Entity
    {
        public int Repair_Capital_Use_Code { get; set; }
        public string Apt_Code { get; set; }
        public string Repair_Capital_Code { get; set; }
        public string Repair_plan_Code { get; set; }
        public string Repair_Article_Code { get; set; }
        public string Repair_Cycle_Code { get; set; }
        public string Repair_Cost_Code { get; set; }
        public string Use_Division { get; set; }
        public double Plan_Cost { get; set; }
        public double Use_Cost { get; set; }
        public DateTime Use_Date { get; set; }
        public int Use_Year { get; set; }
        public int Use_Month { get; set; }
        public int Use_Day { get; set; }
        public string Staff_Code { get; set; }
        public DateTime PostDate { get; set; }
        public string PostIP { get; set; }
    }

    // 장기수선충당금 사용계획서 엔터티
    public class Capital_Use_Plan_Entity
    {
        public int Capital_Use_Plan_Code { get; set; }
        public string Apt_Code { get; set; }
        public string Repair_Plan_Code { get; set; }
        public string Repair_Article_Code { get; set; }
        public string Repair_Using_Plan_Name { get; set; }
        public string Repair_Position_Part { get; set; }
        public DateTime Repair_Period_Start { get; set; }
        public DateTime Repair_Period_End { get; set; }
        public string Repair_Method { get; set; }
        public string Repair_Range { get; set; }
        public double Repair_Cost { get; set; }
        public string Repair_Process { get; set; }
        public string Staff_Code { get; set; }
        public DateTime PostDate { get; set; }
        public string PostIP { get; set; }
    }

    /// <summary>
    /// 단가(숫자)
    /// </summary>
    public class Unit_Price_Entity
    {
        public double supply_total_Area { get; set; }
        public double plan_total_sum { get; set; }
        public double Repair_Cost_Part { get; set; }
        public double Repair_Cost_All { get; set; }
        public double Balance_sum { get; set; }
        public int HouseHold { get; set; }
        public double Unit_Price { get; set; }
        public int Levy_Period { get; set; }
        public double Levy_Rate { get; set; }
        public string Apt_Name { get; set; }
    }
}