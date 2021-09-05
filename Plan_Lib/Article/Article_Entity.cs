using System;
using System.ComponentModel.DataAnnotations;

namespace Plan_Blazor_Lib.Article
{
    public class Article_Entity
    {
        /// <summary>
        /// 일련번호, 수선항목 식별코드
        /// </summary>
        [Key]
        public int Aid { get; set; }

        /// <summary>
        /// 공동주택 식별코드
        /// </summary>
        [Required]
        public string Apt_Code { get; set; }

        /// <summary>
        /// 장기수선계획 식별코드
        /// </summary>
        [Required]
        public string Repair_Plan_Code { get; set; }

        /// <summary>
        /// 시설물코드
        /// </summary>
        public string Facility_Code { get; set; }

        /// <summary>
        /// 대분류코드
        /// </summary>
        public string Sort_A_Code { get; set; }

        /// <summary>
        /// 중분류코드
        /// </summary>
        public string Sort_B_Code { get; set; }

        /// <summary>
        /// 소분류코드
        /// </summary>
        public string Sort_C_Code { get; set; }

        /// <summary>
        /// 대분류명
        /// </summary>
        public string Sort_A_Name { get; set; }

        /// <summary>
        /// 중분류명
        /// </summary>
        public string Sort_B_Name { get; set; }

        /// <summary>
        /// 소분류명
        /// </summary>
        public string Sort_C_Name { get; set; }

        /// <summary>
        /// 수선항목명
        /// </summary>
        public string Repair_Article_Name { get; set; }

        /// <summary>
        /// 구분
        /// </summary>
        public string Division { get; set; }

        /// <summary>
        /// 수량
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// 단위
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 설정 전체주기
        /// </summary>
        public int All_Cycle { get; set; }

        /// <summary>
        /// 설정 부분주기
        /// </summary>
        public int Part_Cycle { get; set; }

        /// <summary>
        /// 설정부분수선율
        /// </summary>
        public double Repair_Rate { get; set; }

        /// <summary>
        /// 전체 최종수선년도
        /// </summary>
        public DateTime Installation { get; set; }

        /// <summary>
        /// 부분 최종수선년도
        /// </summary>
        public DateTime Installation_Part { get; set; }

        /// <summary>
        /// 수선항목 설명
        /// </summary>
        public string Repair_Article_Etc { get; set; }

        /// <summary>
        /// 입력자 아이디
        /// </summary>
        public string User_ID { get; set; }

        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }

        /// <summary>
        /// 입력자 아이디
        /// </summary>
        public string PostIP { get; set; }

        /// <summary>
        /// 삭제여부
        /// </summary>
        public string Del { get; set; }

        public int f_Aid { get; set; }

        /// <summary>
        /// 입력된 수선주기 수
        /// </summary>
        public int Cycle_Count { get; set; }

        /// <summary>
        /// 입력된 수선금액 수
        /// </summary>
        public int Cost_Count { get; set; }
    }

    /// <summary>
    /// 장기수선 항목, 주기, 금액 엔터티
    /// </summary>
    public class Join_Article_Cycle_Cost_Entity
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
        public int Cycle_Count { get; set; }
        public int Cost_Count { get; set; }

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
        public double Repair_All_Cost { get; set; }
        public double Repair_Part_Cost { get; set; }
        public double Repair_Rate_Cost { get; set; }
        public DateTime PostDate { get; set; }
        public string Staff_Code { get; set; }
        public string Cost_Etc { get; set; }

        public int NextYear { get; set; }
    }
}