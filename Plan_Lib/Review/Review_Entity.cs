using System;

namespace Plan_Blazor_Lib.Review
{
    /// <summary>
    /// 장기수선계획 검토 기본 테이블 엔터티
    /// </summary>
    public class Plan_Review_Entity
    {
        /// <summary>
        /// 검토총론 코드
        /// </summary>
        public int Plan_Review_Code { get; set; }

        /// <summary>
        /// 공동주택 식별코드
        /// </summary>
        public string Apt_Code { get; set; }

        /// <summary>
        /// 장기수선계회 식별코드
        /// </summary>
        public string Repair_Plan_Code { get; set; }

        /// <summary>
        /// 검토구분(정기, 수시)
        /// </summary>
        public string PlanReview_Division { get; set; }

        /// <summary>
        /// 검토일
        /// </summary>
        public DateTime PlanReview_Date { get; set; }

        /// <summary>
        /// 이전 검토일
        /// </summary>
        public DateTime PlanReview_Ago { get; set; }

        /// <summary>
        /// 계획기간
        /// </summary>
        public string Plan_Period { get; set; }

        /// <summary>
        /// 적립잔액
        /// </summary>
        public string Saving_Cost { get; set; }

        /// <summary>
        /// 긴급지출
        /// </summary>
        public string Emergency_Expense { get; set; }

        /// <summary>
        /// 소액지출
        /// </summary>
        public string SmallSum_Expense { get; set; }

        /// <summary>
        /// 부과금액
        /// </summary>
        public string Levy_Sum { get; set; }

        /// <summary>
        /// 검토의견
        /// </summary>
        public string PlanReview_Opinion { get; set; }

        /// <summary>
        /// 검토완료여부
        /// </summary>
        public string PlanReview_Complete { get; set; }

        /// <summary>
        /// 검토자 아이디
        /// </summary>
        public string Staff_Code { get; set; }

        /// <summary>
        /// 검토자
        /// </summary>
        public string Plan_Reviewer { get; set; }

        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }

        /// <summary>
        /// 입력자 아이피
        /// </summary>
        public string PostIP { get; set; }
    }

    /// <summary>
    /// 장기수선계획 및 장기수선계획 총론 검토 엔터티
    /// </summary>
    public class Plan_Review_Plan_Entity
    {
        public int Plan_Review_Code { get; set; }
        public string Apt_Code { get; set; }
        public string Repair_Plan_Code { get; set; }
        public string PlanReview_Division { get; set; }
        public DateTime PlanReview_Date { get; set; }
        public DateTime PlanReview_Ago { get; set; }
        public string Plan_Period { get; set; }
        public string Saving_Cost { get; set; }
        public string Emergency_Expense { get; set; }
        public string SmallSum_Expense { get; set; }
        public string Levy_Sum { get; set; }
        public string PlanReview_Opinion { get; set; }
        public string PlanReview_Complete { get; set; }
        public string Staff_Code { get; set; }
        public string Plan_Reviewer { get; set; }
        public DateTime PostDate { get; set; }
        public string PostIP { get; set; }

        public int Aid { get; set; }
        public DateTime Adjustment_Date { get; set; }
        public string Adjustment_Year { get; set; }
        public string Adjustment_Month { get; set; }
        public DateTime Founding_Date { get; set; }
        public string Founding_man { get; set; }
        public string Adjustment_Division { get; set; }
        public DateTime LastAdjustment_Date { get; set; }
        public string Adjustment_Man { get; set; }
        public int Adjustment_Num { get; set; }
        public DateTime Plan_Review_Date { get; set; }
        public string SmallSum_Unit { get; set; }
        public int Small_Sum { get; set; }
        public string SmallSum_Requirement { get; set; }
        public string SmallSum_Basis { get; set; }
        public string Emergency_Basis { get; set; }
        public string Repair_Plan_Etc { get; set; }
        public string Del { get; set; }
        public string User_ID { get; set; }
        public string Complete { get; set; }
    }

    /// <summary>
    ///  검토 상세 내용 정보 테이블 정보
    /// </summary>
    public class Review_Content_Entity
    {
        public int Aid { get; set; }
        public int Plan_Review_Content_Code { get; set; }
        public string Plan_Review_Code { get; set; }
        public string Sort_A_Code { get; set; }
        public string Sort_B_Code { get; set; }
        public string Sort_C_Code { get; set; }
        public string Sort_A_Name { get; set; }
        public string Sort_B_Name { get; set; }
        public string Sort_C_Name { get; set; }
        public string Repair_Article_Name { get; set; }
        public int All_Cycle_Num { get; set; }
        public int Part_Cycle_Num { get; set; }
        public int Repair_Plan_Year_All { get; set; }
        public int Repair_Plan_Year_Part { get; set; }
        public string Repair_Rate { get; set; }
        public string Repair_All_Cost { get; set; }
        public string Repair_Part_Cost { get; set; }
        public string Repair_Article_Code { get; set; }
        public string Repair_Article_Review { get; set; }
        public string Repair_Cycle_Review { get; set; }
        public string Repair_Part_Rate_Review { get; set; }
        public string Repair_Cost_Review { get; set; }
        public string Review_Content { get; set; }
        public string Staff_Code { get; set; }
        public string Apt_Code { get; set; }
        public DateTime PostDate { get; set; }
        public string PostIP { get; set; }
        public string Apt_Name { get; set; }
    }

    /// <summary>
    /// 조정에서 조인한 수선항목 검토 테이블 속성
    /// </summary>
    public class Review_Content_Join_Enity
    {
        public string Apt_Code { get; set; }
        public string Repair_Plan_Code { get; set; }
        public string Plan_Review_Code { get; set; }
        public int Plan_Review_Content_Code { get; set; }
        public string Repair_Article_Name { get; set; }
        public string Repair_Article_Review { get; set; }
        public string Repair_Cycle_Review { get; set; }
        public string Repair_Cost_Review { get; set; }
        public string Repair_Part_Rate_Review { get; set; }
        public string Review_Content { get; set; }
        public string Repair_Article_Code { get; set; }
        public DateTime PostDate { get; set; }
    }
}