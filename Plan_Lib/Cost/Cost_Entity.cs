using System;

namespace Plan_Blazor_Lib.Cost
{
    public class Cost_Entity
    {
        //public int Aid { get; set; }
        /// <summary>
        /// 수선금액 식별코드
        /// </summary>
        public int Repair_Cost_Code { get; set; }

        /// <summary>
        /// 수선금액 식별코드 문자
        /// </summary>
        public string Repair_Cost_CodeA { get; set; }

        /// <summary>
        /// 공동주택 식별코드
        /// </summary>
        public string Apt_Code { get; set; }

        /// <summary>
        /// 장기수선계획 식별코드
        /// </summary>
        public string Repair_Plan_Code { get; set; }

        /// <summary>
        /// 대분류 코드
        /// </summary>
        public string Sort_A_Code { get; set; }

        /// <summary>
        /// 중분류 코드
        /// </summary>
        public string Sort_B_Code { get; set; }

        /// <summary>
        /// 소분류 코드
        /// </summary>
        public string Sort_C_Code { get; set; }

        /// <summary>
        /// 대분류 명
        /// </summary>
        public string Sort_A_Name { get; set; }

        /// <summary>
        /// 중분류 명
        /// </summary>
        public string Sort_B_Name { get; set; }

        /// <summary>
        /// 소분류 명
        /// </summary>
        public string Sort_C_Name { get; set; }

        /// <summary>
        /// 수선항목 식별코드
        /// </summary>
        public string Repair_Article_Code { get; set; }

        /// <summary>
        /// 단가 기준
        /// </summary>
        public string Price_Sort { get; set; }

        /// <summary>
        /// 수선 기준 수량
        /// </summary>
        public int Repair_Amount { get; set; }

        /// <summary>
        /// 전체수선금액
        /// </summary>
        public long Repair_All_Cost { get; set; }

        /// <summary>
        /// 부분수선 금액
        /// </summary>
        public long Repair_Part_Cost { get; set; }

        /// <summary>
        /// 부분수선율
        /// </summary>
        public double Repair_Rate { get; set; }

        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }

        /// <summary>
        /// 입력자 아이디
        /// </summary>
        public string Staff_Code { get; set; }

        /// <summary>
        /// 기타 설명
        /// </summary>
        public string Cost_Etc { get; set; }

        /// <summary>
        /// 수선항목명
        /// </summary>
        public string Repair_Article_Name { get; set; }

        /// <summary>
        /// 수량의 단위
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 전체주기 횟수
        /// </summary>
        public int All_Cycle_Num { get; set; }

        /// <summary>
        /// 부분주기 횟수
        /// </summary>
        public int Part_Cycle_Num { get; set; }
    }

    /// <summary>
    /// 수선비 총액
    /// </summary>
    public class Plan_Total_Cost_Entity
    {
        /// <summary>
        /// 계획기간 중 수선비 총액
        /// </summary>
        public string plan_total_sum { get; set; }

        /// <summary>
        /// 장기수선충당금 잔액
        /// </summary>
        public string balance_sum { get; set; }

        /// <summary>
        /// 주택공급 총면적
        /// </summary>
        public string supply_total_Area { get; set; }

        /// <summary>
        /// 세대수
        /// </summary>
        public string household { get; set; }

        /// <summary>
        /// 전체수선 총액
        /// </summary>
        public string dbpart { get; set; }

        /// <summary>
        /// 부분수선 총액
        /// </summary>
        public string dball { get; set; }
    }
}