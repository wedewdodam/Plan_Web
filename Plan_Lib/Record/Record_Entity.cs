using System;

namespace Plan_Blazor_Lib.Record
{
    /// <summary>
    /// 수선이력 엔터티
    /// </summary>
    public class Repair_Record_Entity
    {
        /// <summary>
        /// 장기수선충당금 수선이력 식별코드
        /// </summary>
        public int Aid { get; set; }

        /// <summary>
        /// 공동주택 식별코드
        /// </summary>
        public string Apt_Code { get; set; }

        /// <summary>
        /// 공동주택 명
        /// </summary>
        public string Apt_Name { get; set; }

        /// <summary>
        /// 공동주택 시도명
        /// </summary>
        public string Apt_Adress_sido { get; set; }

        
        /// <summary>
        /// 장기수선계획코드
        /// </summary>
        public string Repair_Plan_Code { get; set; }

        /// <summary>
        /// 대분류 코드
        /// </summary>
        public string Sort_A_Code { get; set; } // 대분류 코드

        /// <summary>
        /// 대분류 명
        /// </summary>
        public string Sort_A_Name { get; set; }

        /// <summary>
        /// 중분류 코드
        /// </summary>
        public string Sort_B_Code { get; set; } // 중분류 코드

        /// <summary>
        /// 중분류 명
        /// </summary>
        public string Sort_B_Name { get; set; }

        /// <summary>
        /// 공사종별 식별코드
        /// </summary>
        public string Sort_C_Code { get; set; } // 공사종별 코드

        /// <summary>
        /// 공사종별 이름
        /// </summary>
        public string Sort_C_Name { get; set; }

        /// <summary>
        /// 업체 식별 코드
        /// </summary>
        public string Company_Code { get; set; }//업체코드

        /// <summary>
        /// 업체 명
        /// </summary>
        public string Company_Name { get; set; }

        /// <summary>
        /// 수선항목 코드
        /// </summary>
        public string Repair_Article_Code { get; set; } //수선항목코드

        /// <summary>
        /// 수선항목 명
        /// </summary>
        public string Repair_Article_Name { get; set; }  //수선항목 명

        /// <summary>
        /// 수선구분(전체, 부분)
        /// </summary>
        public string Repair_Division { get; set; }

        /// <summary>
        /// 장기수선계획 금액
        /// </summary>
        public double Repair_Plan_Cost { get; set; } //수선계획금액

        /// <summary>
        /// 계약금액
        /// </summary>
        public double Repair_contract_Cost { get; set; } // 계약금액

        /// <summary>
        /// 공사완료 금액
        /// </summary>
        public double Repair_Cost_Complete { get; set; } // 공사완료 금액

        /// <summary>
        /// 공사 시작일
        /// </summary>
        public DateTime Repair_Start_Date { get; set; } // 공사시작일

        /// <summary>
        /// 공사 종료일
        /// </summary>
        public DateTime Repair_End_Date { get; set; } // 공사종료일

        /// <summary>
        /// 작업 연인월
        /// </summary>
        public int Repair_laver_Count { get; set; } // 작업 연인월        

        /// <summary>
        /// 공사업체 사업자 등록번호
        /// </summary>
        public string CorporRate_Number { get; set; } // 사업자등록번호

        /// <summary>
        /// 입찰방법
        /// </summary>
        public string Tender_Mothed { get; set; } //입찰방법

        /// <summary>
        /// 낙찰 방법
        /// </summary>
        public string Tender_bid { get; set; } //낙찰방법

        /// <summary>
        /// 지출구분(계획지출, 긴급공사, 소액지출, 하자지출)
        /// </summary>
        public string Cost_Division { get; set; }// 지출구분

        /// <summary>
        /// 공사 담당자 명
        /// </summary>
        public string Charge_Man { get; set; } //담당자명

        /// <summary>
        /// 공사 담당자 연락처
        /// </summary>
        public string ChargeMan_mobile { get; set; }//담당자 연락처

        /// <summary>
        /// 작업구분(외부, 내부)
        /// </summary>
        public string Work_Division { get; set; } //

        /// <summary>
        /// 수선이력 기타 정보
        /// </summary>
        public string Repair_Record_Etc { get; set; }

        /// <summary>
        /// 수선년도
        /// </summary>
        public int Repair_Year { get; set; }

        /// <summary>
        /// 공사 월
        /// </summary>
        public int Repair_Month { get; set; }

        /// <summary>
        /// 공사 일
        /// </summary>
        public int Repair_Day { get; set; }

        /// <summary>
        /// 입력자 식별코드
        /// </summary>
        public string Staff_Code { get; set; }

        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }

        /// <summary>
        /// 입력자 아이피
        /// </summary>
        public string PostIP { get; set; }

        /// <summary>
        /// 완료년도
        /// </summary>
        public int Repair_End_Year { get; set; }
    }
}