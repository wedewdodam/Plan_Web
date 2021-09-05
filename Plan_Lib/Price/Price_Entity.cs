using System;

namespace Plan_Blazor_Lib.Price
{
    /// <summary>
    /// 폼목 엔터티
    /// </summary>
    public class Repair_DetailKind_Entity
    {
        public int DetailKind_Code { get; set; }

        // 시설물 대분류 코드
        public string Sort_A_Code { get; set; }

        // 시설물 중분류 코드
        public string Sort_B_Code { get; set; }

        // 시설물 공사종별 코드
        public string Sort_C_Code { get; set; }

        // 품명
        public string Product_name { get; set; }

        // 규격
        public string Standard_name { get; set; }

        // 품명 구분
        public string DetailKind_Division { get; set; }

        // 단위
        public string Unit_cd { get; set; }

        // 품명 설명
        public string DetailKind_Etc { get; set; }

        // 사용여부
        public string status_kb { get; set; }

        // 입력일
        public DateTime PostDate { get; set; }

        // 입력자 아이디
        public string Staff_Code { get; set; }
    }

    /// <summary>
    /// 단가 엔터티
    /// </summary>
    public class Repair_Price_Entity
    {
        // 단가 식별코드
        public int Price_Code { get; set; }

        // 세부 품명 코드
        public string DetailKind_Code { get; set; }

        // 품명
        public string Product_name { get; set; }

        // 재료비
        public double Source_amt { get; set; }

        // 노무비
        public double Labor_amt { get; set; }

        // 경비
        public double Cost_amt { get; set; }

        // 일괄단가
        public double Unit_price { get; set; }

        // 기준일
        public DateTime regist_dt { get; set; }

        // 설명
        public string memo_tx { get; set; }

        // 사용 상태 여부
        public string status_kb { get; set; }

        // 입력일
        public DateTime PostDate { get; set; }

        // 입력자
        public string User_Code { get; set; }

        // 구분 코드
        public string pk_Code { get; set; }
    }

    /// <summary>
    /// 수선 폼목 및 단가 포함 엔터티
    /// </summary>
    public class Repair_Price_Kind_Entity
    {
        // 일괄단가 고유 식별번호
        public int Price_Code { get; set; }

        // 시설물 대분류 코드
        public string Sort_A_Code { get; set; }

        // 시설물 중분류 코드
        public string Sort_B_Code { get; set; }

        // 시설물 공사종별 코드
        public string Sort_C_Code { get; set; }

        // 품명
        public string Product_name { get; set; }

        // 규격
        public string Standard_name { get; set; }

        // 재료비
        public double Source_amt { get; set; }

        // 노무비
        public double Labor_amt { get; set; }

        // 경비
        public double Cost_amt { get; set; }

        // 일괄단가
        public double Unit_price { get; set; }

        // 단위
        public string Unit_cd { get; set; }

        // 단가 적용일
        public DateTime regist_dt { get; set; }

        // 설명
        public string memo_tx { get; set; }

        // 현재 사용여부
        public string status_kb { get; set; }

        // 입력일
        public DateTime PostDate { get; set; }

        // 입력자 코드
        public string User_Code { get; set; }

        // 기타
        public string pk_Code { get; set; }

        // 세부 품명 코드
        public string DetailKind_Code { get; set; }

        // 품명 구분
        public string DetailKind_Division { get; set; }

        // 품명 설명
        public string DetailKind_Etc { get; set; }

        // 입력자 아이디
        public string Staff_Code { get; set; }
    }

    /// <summary>
    /// 단가 모음 엔터티
    /// </summary>
    public class Price_Set_Entity
    {
        public int Price_Set_Code { get; set; }
        public string Apt_Code { get; set; }
        public string Repair_Plan_Code { get; set; }
        public string DetailKind_Code { get; set; }
        public string Goods_Name { get; set; }
        public string Product_name { get; set; }
        public string Unit_cd { get; set; }
        public string Repair_Cost_Code { get; set; }
        public string Price_Code { get; set; }
        public string Price_Division { get; set; }
        public int Select_Price { get; set; }
        public double Repair_Cost { get; set; }
        public int Repair_Amount { get; set; }
        public DateTime Regist_dt { get; set; }
        public string Price_Set_Etc { get; set; }
        public string Staff_Code { get; set; }
        public DateTime PostDate { get; set; }
        public int OrderBy { get; set; }
    }

    /// <summary>
    /// 장기수선충당금 사용계획서 엔터티
    /// </summary>
    public class Cost_Using_Plan_Entity
    {
        public int Cost_Use_Plan_Code { get; set; }
        public string Apt_Code { get; set; }
        public string Repair_Plan_Code { get; set; }
        public string Repair_Article_Code { get; set; }
        public string Plan_Year { get; set; }
        public string Repair_Name { get; set; }
        public string Repair_Position { get; set; }
        public string Repair_Range { get; set; }
        public double Repair_Cost_Sum { get; set; }
        public string Repair_Detail { get; set; }
        public string Design_Drawing { get; set; }
        public string Repair_Method { get; set; }
        public DateTime Start_Date { get; set; }
        public DateTime End_Date { get; set; }
        public string Tender_Method_Process { get; set; }
        public DateTime PostDate { get; set; }
        public string PostIP { get; set; }
        public string Staff_Code { get; set; }
    }

    /// <summary>
    /// 원가계산서 할증율 엔터티
    /// </summary>
    public class UnitPrice_Rate_Entity
    {
        public int UnitPrice_Rate_Code { get; set; }
        public string Standard_Year { get; set; }
        public double Source_Rate { get; set; }
        public double Labor_Rate { get; set; }
        public double Cost_Rate { get; set; }
        public double Indirectness_Rate { get; set; }
        public double Industrial_Accident_Rate { get; set; }
        public double Employ_Insurance_Rate { get; set; }
        public double Well_Insurance_Rate { get; set; }
        public double Pension_Insurance_Rate { get; set; }
        public double OldMan_Insurance_Rate { get; set; }
        public double Health_Safe_Insurance_Rate { get; set; }
        public double Environment_Priserve_Rate { get; set; }
        public double Etc_Cost_Rate { get; set; }
        public double Common_Cost_Rate { get; set; }
        public double Profit_Rate { get; set; }
        public double VAT_Rate { get; set; }
        public string Staff_Code { get; set; }
        public DateTime PostDate { get; set; }
        public string PostIP { get; set; }
    }

    /// <summary>
    /// 해당 수선금액의 단가(재료비, 노무비, 경비) 합계 구하기
    /// </summary>
    public class Prime_Cost_Report_Entity
    {
        public double Source_amt { get; set; }
        public double Labor_amt { get; set; }
        public double Cost_amt { get; set; }
        public double Select_Price { get; set; }
        public int Repair_Amount { get; set; }
        public double Repair_Cost { get; set; }
    }

    /// <summary>
    /// 수선금액 엔터티
    /// </summary>
    public class Report_Plan_Cost_Entity
    {
        /// <summary>
        /// 공동주택 식별코드
        /// </summary>
        public string Apt_Code { get; set; }

        /// <summary>
        /// 장기수선계획 식별코드
        /// </summary>
        public string Repair_Plan_Code { get; set; }

        /// <summary>
        /// 해당 장기수선계획의 수선비 총액
        ///
        /// </summary>
        public string plan_total_sum { get; set; }

        /// <summary>
        /// 해당 공동주택의 장기수선충당금 잔액
        /// </summary>
        public string balance_sum { get; set; }

        /// <summary>
        /// 해당 공동주택의 주택공급 총면적
        /// </summary>
        public string supply_total_Area { get; set; }

        /// <summary>
        /// 해당 공동주택의 총 세대 수
        /// </summary>
        public string household { get; set; }

        /// <summary>
        ///  해당 공동주택의 현재 기준 장기수서충당금 ㎡당 부과 단가
        /// </summary>
        public string unit_Price { get; set; }
    }
}