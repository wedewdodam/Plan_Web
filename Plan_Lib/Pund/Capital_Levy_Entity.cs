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

        /// <summary>
        /// 세대 공급면적
        /// </summary>
        public double Supply_Area { get; set; }

        /// <summary>
        /// 면적별 세대수
        /// </summary>
        public int Area_Family_Num { get; set; }

        /// <summary>
        /// 세대 월간 부과금액
        /// </summary>
        public string Family_Month_Rate_Cost { get; set; }

        /// <summary>
        /// 세대 년간 부과금액
        /// </summary>
        public string Family_Year_Rate_Cost { get; set; }

        /// <summary>
        /// 면적별 월간 부과금액
        /// </summary>
        public string Area_Rate_Month { get; set; }

        /// <summary>
        /// 면적별 년간 부과금액
        /// </summary>
        public string Area_Rate_Year { get; set; }

        /// <summary>
        /// 총 공급면적
        /// </summary>
        public double supply_total_Area { get; set; }

        /// <summary>
        /// 월간 부과금액 합계
        /// </summary>
        public string Total_Cost_Month { get; set; }

        /// <summary>
        /// 년간 부과금액 합계
        /// </summary>
        public string Total_Cost_Year { get; set; }

        /// <summary>
        /// 단가
        /// </summary>
        public string Unit_Price { get; set; }

        /// <summary>
        /// 총세대수
        /// </summary>
        public int household { get; set; }
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
        public int Cost_Use_Plan_Code { get; set; }

        /// <summary>
        /// 공동주택 식별코드
        /// </summary>
        public string Apt_Code { get; set; }

        /// <summary>
        /// 장기수선계획 식별코드
        /// </summary>
        public string Repair_Plan_Code { get; set; }

        /// <summary>
        /// 장기수선 항목 식별코드
        /// </summary>
        public string Repair_Article_Code { get; set; }

        /// <summary>
        /// 계획년도
        /// </summary>
        public string Plan_Year { get; set; }

        /// <summary>
        /// 공사 명칭
        /// </summary>
        public string Repair_Name { get; set; }

        /// <summary>
        /// 공사 위치(장소)
        /// </summary>
        public string Repair_Position { get; set; }

        /// <summary>
        /// 공사 범위
        /// </summary>
        public string Repair_Range { get; set; }

        /// <summary>
        /// 공사 금액
        /// </summary>
        public double Repair_Cost_Sum { get; set; }

        /// <summary>
        /// 공사 설명
        /// </summary>
        public string Repair_Detail { get; set; }

        /// <summary>
        /// 설계도면
        /// </summary>
        public string Design_Drawing { get; set; }

        /// <summary>
        /// 공사방법
        /// </summary>
        public string Repair_Method { get; set; }

        /// <summary>
        /// 공사 시작일
        /// </summary>
        public DateTime Start_Date { get; set; }

        /// <summary>
        /// 공사 마감일
        /// </summary>
        public DateTime End_Date { get; set; }

        /// <summary>
        /// 입찰 방법 
        /// </summary>
        public string Tender_Method_Process { get; set; }

        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }

        /// <summary>
        /// 입력 아이피
        /// </summary>
        public string PostIP { get; set; }

        /// <summary>
        /// 입력자 아이디
        /// </summary>
        public string Staff_Code { get; set; }

        /// <summary>
        /// 수선항목 명
        /// </summary>
        public string Repair_Article_Name { get; set; }
    }

    

    /// <summary>
    /// 단가(숫자)
    /// </summary>
    public class Unit_Price_Entity
    {
        /// <summary>
        /// 공급 면적
        /// </summary>
        public double supply_total_Area { get; set; }

        /// <summary>
        /// 수선비 총액
        /// </summary>
        public double plan_total_sum { get; set; }

        /// <summary>
        /// 부분수선비 총액
        /// </summary>
        public double Repair_Cost_Part { get; set; }

        /// <summary>
        /// 전체수선비 총액
        /// </summary>
        public double Repair_Cost_All { get; set; }

        /// <summary>
        /// 잔액
        /// </summary>
        public double Balance_sum { get; set; }

        /// <summary>
        /// 세대수
        /// </summary>
        public int HouseHold { get; set; }

        /// <summary>
        /// 단가
        /// </summary>
        public double Unit_Price { get; set; }

        /// <summary>
        /// 요율 기간
        /// </summary>
        public int Levy_Period { get; set; }

        /// <summary>
        /// 요율
        /// </summary>
        public double Levy_Rate { get; set; }

        /// <summary>
        /// 공동주택 명
        /// </summary>
        public string Apt_Name { get; set; }
    }
}