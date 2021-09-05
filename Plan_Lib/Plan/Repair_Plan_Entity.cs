using System;

namespace Plan_Blazor_Lib.Plan
{
    public class Repair_Plan_Entity
    {
        /// <summary>
        /// 일련번호
        /// </summary>
        public int Aid { get; set; }

        /// <summary>
        /// 장기수선계획 식별코드
        /// </summary>
        public string Repair_Plan_Code { get; set; }

        /// <summary>
        /// 공동주택 식별코드
        /// </summary>
        public string Apt_Code { get; set; }

        /// <summary>
        /// 장기수선계획 검토 식별코드
        /// </summary>
        public string Plan_Review_Code { get; set; }

        /// <summary>
        /// 조정일
        /// </summary>
        public DateTime Adjustment_Date { get; set; }

        /// <summary>
        /// 조정년도
        /// </summary>
        public string Adjustment_Year { get; set; }

        /// <summary>
        /// 조정월
        /// </summary>
        public string Adjustment_Month { get; set; }

        /// <summary>
        /// 사용검사일
        /// </summary>
        public DateTime Founding_Date { get; set; }

        /// <summary>
        /// 사업주체
        /// </summary>
        public string Founding_man { get; set; }

        /// <summary>
        /// 조정구분(정기, 수시)
        /// </summary>
        public string Adjustment_Division { get; set; }

        /// <summary>
        /// 계획기간
        /// </summary>
        public int Plan_Period { get; set; }

        /// <summary>
        /// 마지막 조정일
        /// </summary>
        public DateTime LastAdjustment_Date { get; set; }

        /// <summary>
        /// 조정자
        /// </summary>
        public string Adjustment_Man { get; set; }

        /// <summary>
        /// 조정차수
        /// </summary>
        public int Adjustment_Num { get; set; }

        /// <summary>
        /// 검토일
        /// </summary>
        public DateTime Plan_Review_Date { get; set; }

        /// <summary>
        /// 소액지출 구분
        /// </summary>
        public string SmallSum_Unit { get; set; }

        /// <summary>
        /// 소액범위
        /// </summary>
        public int Small_Sum { get; set; }

        /// <summary>
        /// 소액 설명
        /// </summary>
        public string SmallSum_Requirement { get; set; }

        /// <summary>
        /// 소액지출 근거
        /// </summary>
        public string SmallSum_Basis { get; set; }

        /// <summary>
        /// 긴급지출 근거
        /// </summary>
        public string Emergency_Basis { get; set; }

        /// <summary>
        /// 기타 설명
        /// </summary>
        public string Repair_Plan_Etc { get; set; }

        /// <summary>
        /// 삭제여부
        /// </summary>
        public string Del { get; set; }

        /// <summary>
        /// 작성자 아이디
        /// </summary>
        public string User_ID { get; set; }

        /// <summary>
        /// 작성일
        /// </summary>
        public DateTime PostDate { get; set; }

        /// <summary>
        /// 작성자 아이피
        /// </summary>
        public string PostIP { get; set; }

        /// <summary>
        /// 장기수선계획 완료 여부
        /// </summary>
        public string Complete { get; set; }

        /// <summary>
        /// 수시조정 여부(A: 정기 및 수시 조정, B: 간편수시조정)
        /// </summary>
        public string AnyTime { get; set; }
    }

    // 장기수선계획 소액지출 대상 테이블
    public class Repair_SmallSum_Object_Entity
    {
        public int Aid { get; set; }
        public string SmallSum_Object_Code { get; set; }
        public string SmallSum_Object { get; set; }
        public string SmallSum_Object_Detail { get; set; }
        public string Del { get; set; }
        public DateTime PostDate { get; set; }
        public string PostIP { get; set; }
    }

    // 장기수선계획 소액지출 대상 선택 테이블
    public class Repair_SmallSum_Object_Selection_Entity
    {
        public int Aid { get; set; }

        /// <summary>
        /// 소액지출 선택 식별코드
        /// </summary>
        public string SmallSum_Object_Selection_Code { get; set; }

        // 장기수선계획 식별코드
        public string Repair_Plan_Code { get; set; }

        // 공동주택 식별코드
        public string Apt_Code { get; set; }

        // 소액지출 대상 식별코드
        public string SmallSum_Object_Code { get; set; }

        // 각 단지에 입력된 소액지출 대상 내용
        public string SmallSum_Object_Content { get; set; }

        public DateTime PostDate { get; set; }
        public string PostIP { get; set; }
    }

    // 장기수선충당금 소액지출 요건 테이블
    public class Repair_SmallSum_Requirement_Entity
    {
        public int Aid { get; set; }

        // 소액지출 요건 식별 코드
        public string SmallSum_Requirement_Code { get; set; }

        // 소액지출 요건 내용
        public string SmallSum_Requirement { get; set; }

        // 소액지출 요건 설명
        public string SmallSum_Requirement_Etc { get; set; }

        // 입력일
        public DateTime PostDate { get; set; }

        // 입력자 아이피
        public string PostIP { get; set; }
    }

    // 장기수선충당금 소액지출 요건 테이블
    public class Repair_SmallSum_Requirement_Selection_Entity
    {
        public int Aid { get; set; }

        // 소액지출 요건 선택 식별 코드
        public string SmallSum_Requirement_Selection_Code { get; set; }

        //공동주택 식별코드
        public string Apt_Code { get; set; }

        // 장기수선계획 식별 코드
        public string Repair_Plan_Code { get; set; }

        // 소액지출 요건 식별 코드
        public string SmallSum_Requirement_Code { get; set; }

        // 선택되고 수정된 요건 내용
        public string SmallSum_Requirement_Content { get; set; }

        // 입력일
        public DateTime PostDate { get; set; }

        //public string PostIP { get; set; }
    }

    /// <summary>
    /// 장기수선계획 조정 진행 상황
    /// </summary>
    public class Plan_Prosess
    {
        /// <summary>
        /// 식별코드
        /// </summary>
        public int Aid { get; set; }

        /// <summary>
        /// 단지 식별코드
        /// </summary>
        public string Apt_Code { get; set; }

        /// <summary>
        /// 장기수선계획 식별코드
        /// </summary>
        public string Repair_Plan_Code { get; set; }

        /// <summary>
        /// 수선항목 완료 여부
        /// </summary>
        public string Article_Complete { get; set; }

        /// <summary>
        /// 수선항목 완료일
        /// </summary>
        public DateTime Article_Complete_Date { get; set; }

        /// <summary>
        /// 수선주기 완료여부
        /// </summary>
        public string Cycle_Complete { get; set; }

        /// <summary>
        /// 수선주기 완료일
        /// </summary>
        public DateTime Cycle_Complete_Date { get; set; }

        /// <summary>
        /// 수선금액 완료여부
        /// </summary>
        public string Cost_Complete { get; set; }

        /// <summary>
        /// 수선금액 완요일
        /// </summary>
        public DateTime Cost_Complete_Date { get; set; }

        /// <summary>
        /// 장기수선계획 완료여부
        /// </summary>
        public string Plan_Complete { get; set; }

        /// <summary>
        /// 장기수선계획 완료일
        /// </summary>
        public DateTime Plan_Complete_Date { get; set; }

        /// <summary>
        /// 입력일
        /// </summary>
        public string Post_Date { get; set; }

        /// <summary>
        /// 입력자 아이피
        /// </summary>
        public string PostIP { get; set; }
    }

    /// <summary>
    /// 장기수선 항목, 주기, 금액 엔터티
    /// </summary>
    public class Join_Article_Cycle_Cost_EntityA
    {
        public int Aid { get; set; }
        public string Apt_Code { get; set; }
        public string Repair_Plan_Code { get; set; }
        public string Facility_Code { get; set; }
        public string Sort_A_Name { get; set; }
        public string Sort_B_Name { get; set; }
        public string Sort_C_Name { get; set; }
        public string Sort_A_Code { get; set; }
        public string Sort_B_Code { get; set; }
        public string Sort_C_Code { get; set; }
        public string Repair_Article_Name { get; set; }
        public string Division { get; set; }
        public int Amount { get; set; }
        public string Unit { get; set; }
        public int All_Cycle { get; set; }
        public int Part_Cycle { get; set; }
        public double Repair_Rate { get; set; }
        public DateTime Installation { get; set; }
        public DateTime Installation_Part { get; set; }
        public string Repair_Article_Etc { get; set; }

        public int Cycle_Aid { get; set; }
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

        public string Repair_Cost_Code { get; set; }
        public string Repair_Article_Code { get; set; }
        public string Price_Sort { get; set; }
        public int Repair_Amount { get; set; }
        public long Repair_All_Cost { get; set; }
        public long Repair_Part_Cost { get; set; }
        public double Repair_Rate_Cost { get; set; }
        public DateTime PostDate { get; set; }
        public string Staff_Code { get; set; }
        public string Cost_Etc { get; set; }

        public int NextYear { get; set; }
    }

    public class Repair_Plan_practice_total_Entity
    {
        /// <summary>
        /// 연차별 합계 수선금액
        /// </summary>
        public double Plan_cost { get; set; }

        public double All_Cost { get; set; }
        public double Part_Cost { get; set; }
        public int All_Year_Count { get; set; }
        public int Part_Year_Count { get; set; }
    }
}