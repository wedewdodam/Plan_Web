using System;
using System.ComponentModel.DataAnnotations;

namespace Facility
{
    // 시설물 설명 및 보수 방법 테이블 필드 엔터티
    public class Facility_Explanation_Entity
    {
        public int Aid { get; set; }
        public string Facility_Sort_Code { get; set; }
        public string Explanation { get; set; }
        public string Repair_Method { get; set; }
        public DateTime PostDate { get; set; }
        public string PostIP { get; set; }
    }

    // 공동주택 부대 복리 시설 테이블
    public class Additional_Welfare_Facility
    {
        public int Aid { get; set; }
        public string Apt_Code { get; set; }
        public string Facility_Name { get; set; }
        public string Facility_Position { get; set; }
        public string Facility_Division { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; }
        public string Facility_Etc { get; set; }
        public string User_ID { get; set; }
        public DateTime PostDate { get; set; }
        public string Post_IP { get; set; }
    }

    // 시설물
    public class Facility_Entity
    {
        public int Aid { get; set; }
        public string Facility_Code { get; set; }
        public string Apt_Code { get; set; }
        public string Facility_Sort_Code_A { get; set; }
        public string Sort_A_Name { get; set; }
        public string Facility_Sort_Code_B { get; set; }
        public string Sort_B_Name { get; set; }
        public string Facility_Sort_Code_C { get; set; }
        public string Sort_C_Name { get; set; }
        public string Facility_Name { get; set; }
        public string Facility_Position { get; set; }
        public DateTime Facility_Installation_Date { get; set; }
        public string Facility_Etc { get; set; }
        public DateTime PostDate { get; set; }
    }

    // 시설물 상세
    public class Facility_Detail_Entity
    {
        public int Aid { get; set; }
        public string Facility_Code { get; set; }
        public string Apt_Code { get; set; }
        public string Sort_A { get; set; }
        public string Sort_B { get; set; }
        public string Sort_C { get; set; }
        public DateTime Manufacture_Date { get; set; }
        public string Manufacture_Num { get; set; }
        public string Facility_Use { get; set; }
        public string Facility_Form { get; set; }
        public string Manufacture_Corporation { get; set; }
        public string Facility_Standard { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }
        public string Facility_Detail_Etc { get; set; }
        public string User_ID { get; set; }
        public DateTime PostDate { get; set; }
        public string PostIP { get; set; }
    }

    /// <summary>
    /// 시설물 분류
    /// </summary>
    public class Facility_Sort_Entity
    {
        /// <summary>
        /// 일련번호
        /// </summary>
        [Key]
        [Required]
        public int Aid { get; set; }

        /// <summary>
        /// 시설물분류 식별코드
        /// </summary>
        [Required]
        public string Facility_Sort_Code { get; set; }

        /// <summary>
        /// 시설물 분류 명
        /// </summary>
        [Required]
        public string Sort_Name { get; set; }

        /// <summary>
        /// 대분류 코드
        /// </summary>
        public string Sort_A_Code { get; set; }

        public string Sort_A_Name { get; set; }

        /// <summary>
        /// 중분류 코드
        /// </summary>
        public string Sort_B_Code { get; set; }

        public string Sort_B_Name { get; set; }

        /// <summary>
        /// 상위 코드
        /// </summary>
        [Required]
        public string Up_Code { get; set; }

        /// <summary>
        /// 분류단계
        /// </summary>
        [Required]
        public string Sort_Step { get; set; }

        /// <summary>
        /// 분류순서
        /// </summary>
        public string Sort_Order { get; set; }

        /// <summary>
        /// 분류구분
        /// </summary>
        public string Sort_Division { get; set; }

        /// <summary>
        /// 법정 전체주기
        /// </summary>
        public int Repair_Cycle { get; set; }

        /// <summary>
        /// 법정 부분주기
        /// </summary>
        public int Repair_Cycle_Part { get; set; }

        /// <summary>
        /// 법정 부분수선율
        /// </summary>
        public int Repair_Rate { get; set; }

        /// <summary>
        /// 수선항목 설명
        /// </summary>
        public string Sort_Detail { get; set; }

        /// <summary>
        /// 법령 제정일
        /// </summary>
        public string Enforce_Date { get; set; }

        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }

        public int Ar { get; set; }
    }
}