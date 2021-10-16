using System;

namespace Plan_Lib.Company
{
    /// <summary>
    /// 업체 분류 엔터티
    /// </summary>
    public class Company_Sort_Entity
    {
        public int Aid { get; set; }
        public string Company_Sort_Code { get; set; }
        public string Apt_Code { get; set; }
        public string Staff_Code { get; set; }
        public string Company_Sort_Name { get; set; }
        public string Up_Code { get; set; }
        public string Company_Sort_Step { get; set; }
        public string Company_Division { get; set; }
        public string Company_Etc { get; set; }
        public DateTime PostDate { get; set; }
    }

    /// <summary>
    /// 업체 정보 엔터티
    /// </summary>
    public class Company_Entity
    {
        public int Aid { get; set; }
        public string Apt_Code { get; set; }
        public string Staff_Code { get; set; }
        public string SortA_Code { get; set; }
        public string SortB_Code { get; set; }
        public string SortA_Name { get; set; }
        public string SortB_Name { get; set; }
        public string Company_Code { get; set; }
        public string Company_Name { get; set; }
        public string CorporRate_Number { get; set; }
        public string Company_Etc { get; set; }
        public DateTime PostDate { get; set; }
        public string PostIP { get; set; }
    }

    /// <summary>
    /// 업체 상세 정보 엔터티
    /// </summary>
    public class Company_Etc_Entity
    {
        public int Aid { get; set; }
        public string CompanyEtc_Code { get; set; }
        public string Company_Code { get; set; }
        public string Staff_Code { get; set; }
        public string Corporation { get; set; }
        public string Ceo_Name { get; set; }
        public string Credit_Rate { get; set; }
        public double Capital { get; set; }
        public string Cor_Tel { get; set; }
        public string Cor_Fax { get; set; }
        public string Cor_Email { get; set; }
        public string Cor_Sido { get; set; }
        public string Cor_Gun { get; set; }
        public string Cor_Adress { get; set; }
        public string Ceo_Mobile { get; set; }
        public string Charge_Man { get; set; }
        public string ChargeMan_Mobile { get; set; }
        public string Cor_Etc { get; set; }
        public DateTime PostDate { get; set; }
        public string PostIP { get; set; }
    }

    // 업체 + 업체 상세
    public class Company_Entity_Etc
    {
        public int Aid { get; set; }

        public string Apt_Code { get; set; }
        public string Staff_Code { get; set; }
        
        /// <summary>
        /// 업체 상위 분류 코드
        /// </summary>
        public string SortA_Code { get; set; }

        /// <summary>
        /// 업체 하위 분류 코드
        /// </summary>
        public string SortB_Code { get; set; }

        /// <summary>
        /// 업체 상위 분류명
        /// </summary>
        public string SortA_Name { get; set; }

        /// <summary>
        /// 업체 하위 분류명
        /// </summary>
        public string SortB_Name { get; set; }

        /// <summary>
        /// 업체 식별코드
        /// </summary>
        public string Company_Code { get; set; }

        /// <summary>
        /// 업체 명
        /// </summary>
        public string Company_Name { get; set; }

        /// <summary>
        /// 업체 사업자 등록 번호
        /// </summary>
        public string CorporRate_Number { get; set; }

        /// <summary>
        /// 업체 설명
        /// </summary>
        public string Company_Etc { get; set; }

        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }


        /// <summary>
        /// 업체 상세 식별코드
        /// </summary>
        public string CompanyEtc_Code { get; set; }

        //public string Company_Code { get; set; }
        //public string Staff_Code { get; set; }
        public string Corporation { get; set; }

        /// <summary>
        /// 대표자명
        /// </summary>
        public string Ceo_Name { get; set; }

        /// <summary>
        /// 신용등급
        /// </summary>
        public string Credit_Rate { get; set; }

        /// <summary>
        /// 자본금
        /// </summary>
        public double Capital { get; set; }

        /// <summary>
        /// 대표 전화번호
        /// </summary>
        public string Cor_Tel { get; set; }

        /// <summary>
        /// 팩스
        /// </summary>
        public string Cor_Fax { get; set; }

        /// <summary>
        /// 이메일
        /// </summary>
        public string Cor_Email { get; set; }

        /// <summary>
        /// 업체 주소 시도
        /// </summary>
        public string Cor_Sido { get; set; }

        /// <summary>
        /// 업체 주소 시군구
        /// </summary>
        public string Cor_Gun { get; set; }

        /// <summary>
        /// 업체 주소 나머지 상세
        /// </summary>
        public string Cor_Adress { get; set; }

        /// <summary>
        /// 대표자 모바일
        /// </summary>
        public string Ceo_Mobile { get; set; }

        /// <summary>
        /// 담당자 명
        /// </summary>
        public string Charge_Man { get; set; }

        /// <summary>
        /// 담당자 모바일
        /// </summary>
        public string ChargeMan_Mobile { get; set; }

        /// <summary>
        /// 업체 상세 설명
        /// </summary>
        public string Cor_Etc { get; set; }
    }

    /// <summary>
    /// 업종 테이블
    /// </summary>
    public class BusinessType_Entity
    {
        public int Aid { get; set; }
        public string BusinessType_Code { get; set; }
        public string BusinessType_Name { get; set; }
        public string Staff_Code { get; set; }
        public string BusinessType_Etc { get; set; }
        public DateTime PostDate { get; set; }
        public string PostIP { get; set; }
    }

    /// <summary>
    /// 선택 업종 테이블
    /// </summary>
    public class BusiList_Entity
    {
        public int Aid { get; set; }
        public string BusiList_Code { get; set; }
        public string CompanyEtc_Code { get; set; }
        public string BusinessType_Code { get; set; }
        public string Staff_Code { get; set; }
        public DateTime PostDate { get; set; }
        public string PostIP { get; set; }
    }

    /// <summary>
    /// 단지 선택 업체 테이블
    /// </summary>
    public class Apt_Company_Entity
    {
        public int Aid { get; set; }
        public string Apt_Company_Code { get; set; }
        public string Apt_Code { get; set; }
        public string SortA_Code { get; set; }
        public string SortB_Code { get; set; }
        public string Company_Code { get; set; }
        public string Staff_Code { get; set; }
        public DateTime PostDate { get; set; }
        public string PostIP { get; set; }
    }
}