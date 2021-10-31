using System;

namespace Plan_Lib.Pund
{
    /// <summary>
    /// 장기수선충당금 사용 및 적립현황 엔터티
    /// </summary>
    public class Repair_Saving_Using_Pund_Entity
    {
        /// <summary>
        /// 식별코드
        /// </summary>
        public int Aid { get; set; }

        /// <summary>
        /// 공동주택 식별코드
        /// </summary>
        public string Apt_Code { get; set; }

        /// <summary>
        /// 공동주택명
        /// </summary>
        public string Apt_Name { get; set; }

        /// <summary>
        /// 사용검사일
        /// </summary>
        public DateTime Founding_Date { get; set; }

        /// <summary>
        /// 조정일
        /// </summary>
        public DateTime Adjust_Date { get; set; }

        /// <summary>
        /// 사용 및 적립 게시일
        /// </summary>
        public DateTime Report_Date { get; set; }

        /// <summary>
        /// 기준년도
        /// </summary>
        public int Report_Year { get; set; }

        /// <summary>
        /// 세대수
        /// </summary>
        public int Family_Num { get; set; }

        /// <summary>
        /// 관리소장명
        /// </summary>
        public string Staff_Name { get; set; }

        /// <summary>
        /// 공동주택 주소
        /// </summary>
        public string Adress { get; set; }

        /// <summary>
        /// 수선비 총액
        /// </summary>
        public double Plan_Funds { get; set; }

        /// <summary>
        /// 적립율에 따른 적립액
        /// </summary>
        public double Saving_Funds { get; set; }

        /// <summary>
        /// 전전년도까지 사용액
        /// </summary>
        public double Using_Funds_ago { get; set; }

        /// <summary>
        /// 전년도 사용액
        /// </summary>
        public double Using_Funds_now { get; set; }

        /// <summary>
        /// 사용액 합계
        /// </summary>
        public double Using_Funds { get; set; }

        /// <summary>
        /// 잔액
        /// </summary>
        public double Balance_Funds { get; set; }

        /// <summary>
        /// 실재잔액
        /// </summary>
        public double Real_Balance_Funds { get; set; }

        /// <summary>
        /// 필요액
        /// </summary>
        public double Need_Funds { get; set; }

        /// <summary>
        /// 평방미터당 단가
        /// </summary>
        public double Unit_Price { get; set; }

        /// <summary>
        /// 월부과금액
        /// </summary>
        public int Month_Impose { get; set; }

        /// <summary>
        /// 주택공급면적
        /// </summary>
        public string Supply_Area { get; set; }

        /// <summary>
        /// 기타
        /// </summary>
        public string Etc { get; set; }

        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }

        /// <summary>
        /// 입력자 아이피
        /// </summary>
        public string PostIP { get; set; }

        /// <summary>
        /// 입력자 아이디
        /// </summary>
        public string Staff_Code { get; set; }
    }

    /// <summary>
    /// 단가(문자)
    /// </summary>
    public class Unit_Price_string_Entity
    {
        public string supply_total_Area { get; set; }
        public string plan_total_sum { get; set; }
        public string dbpart { get; set; }
        public string dball { get; set; }

        /// <summary>
        /// 초기화 잔액
        /// </summary>
        public double balanceSum { get; set; }

        /// <summary>
        /// 초기화 잔액 텍스트
        /// </summary>
        public string balance_sum { get; set; }

        /// <summary>
        /// 조정일까지 잔액(텍스트
        /// </summary>
        public string Balance_sum_Adjust { get; set; }

        /// <summary>
        /// 조정일까지 잔액
        /// </summary>
        public double Balance_sum_Adjust_All { get; set; }

        /// <summary>
        /// 세대수
        /// </summary>
        public string household { get; set; }

        /// <summary>
        /// 초기화년도
        /// </summary>
        public string Reset_year { get; set; }

        /// <summary>
        /// 조정일
        /// </summary>
        public string Adjust_Date { get; set; }

        /// <summary>
        /// 단가 (텍스트)
        /// </summary>
        public string Unit_Price { get; set; }

        /// <summary>
        /// 적립기간
        /// </summary>
        public int Levy_Period { get; set; }

        /// <summary>
        /// 적립율
        /// </summary>
        public double Levy_Rate { get; set; }

        /// <summary>
        /// 징수적립 총액
        /// </summary>
        public double Levy_Sum { get; set; }

        /// <summary>
        /// 조정일까지 징수적립액
        /// </summary>
        public double Levy_Sum_Adjust { get; set; }

        /// <summary>
        /// 단지 명
        /// </summary>
        public string Apt_Name { get; set; }

        /// <summary>
        /// 초기화 사용액 텍스트
        /// </summary>
        public string Using_Cost_Sum { get; set; }

        /// <summary>
        /// 초기화 사용액
        /// </summary>
        public double Using_Cost_Sum_All { get; set; }

        /// <summary>
        /// 초기화 이후 사용총액 텍스트
        /// </summary>
        public string Use_Cost { get; set; }

        /// <summary>
        /// 초기화 이후 사용총액
        /// </summary>
        public double User_Cost_All { get; set; }

        /// <summary>
        /// 초기화 이후 조정일 까지 사용액 텍스트
        /// </summary>
        public string Adjust_Using_Cost { get; set; }

        /// <summary>
        /// 초기화 이후 조정일 까지 사용액
        /// </summary>
        public double Ajust_Using_Cost_All { get; set; }

        /// <summary>
        /// 공급면적
        /// </summary>
        public double supply_total_Area_All { get; set; }

        /// <summary>
        /// 단가
        /// </summary>
        public double Unit_Price_All { get; set; }
    }

    /// <summary>
    /// 장기수선충당금 사용 및 적립 현황 엔터티
    /// </summary>
    public class Present_Condition_Entity
    {
        public int Aid { get; set; }
        public int Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public string Apt_Adress_Sido { get; set; }
        public string Apt_Adress_Gun { get; set; }
        public string Apt_Adress_Rest { get; set; }
        public string Write_Year { get; set; }
        public DateTime Write_Date { get; set; }
        public DateTime Standard_Date { get; set; }
        public double PlanCost_Sum { get; set; }
        public double Levy_Saving_Sum { get; set; }
        public double PlanUsing_Sum_ago { get; set; }
        public double PlanUsing_Sum_now { get; set; }
        public double PlanUsing_Sum { get; set; }
        public double Balance_Sum { get; set; }
        public double ReserveSaving_Sum { get; set; }
        public string Founding_Date { get; set; }
        public DateTime Adjustment_Date { get; set; }
        public double Month_Unit_Price { get; set; }
        public double Month_Reverve_Sum { get; set; }
        public int Family_Count_Num { get; set; }
        public double supply_total_Area { get; set; }
        public string Etc { get; set; }
        public string Staff_Name { get; set; }
        public DateTime PostDate { get; set; }
        public string PostIp { get; set; }
    }

    /// <summary>
    /// 장기수선충당금 사용 및 적립 현황 엔터티(문자)
    /// </summary>
    public class Present_Condition_string_Entity
    {
        public int Aid { get; set; }
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public string Apt_Adress_Sido { get; set; }
        public string Apt_Adress_Gun { get; set; }
        public string Apt_Adress_Rest { get; set; }
        public string Write_Year { get; set; }
        public DateTime Write_Date { get; set; }
        public DateTime Standard_Date { get; set; }
        public string PlanCost_Sum { get; set; }
        public string Levy_Saving_Sum { get; set; }
        public string PlanUsing_Sum { get; set; }
        public string Balance_Sum { get; set; }
        public string ReserveSaving_Sum { get; set; }
        public string Founding_Date { get; set; }
        public string Adjustment_Date { get; set; }
        public string Month_Unit_Price { get; set; }
        public string Month_Reverve_Sum { get; set; }
        public string Family_Count_Num { get; set; }
        public string supply_total_Area { get; set; }
        public string Etc { get; set; }
        public string Staff_Name { get; set; }
        public DateTime PostDate { get; set; }
        public string PostIp { get; set; }
    }
}