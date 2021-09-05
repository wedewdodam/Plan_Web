using System;

namespace Plan_Apt_Lib
{
    /// <summary>
    /// 공동주택와 협회 회원정보 기본정보
    /// </summary>
    public class Apt_memverinfor_v_Entity
    {
        public int Aid { get; set; }
        public string Apt_Code { get; set; }
        public string apt_cd { get; set; }
        public string Apt_Name { get; set; }
        public string Apt_Form { get; set; }
        public string Apt_Adress_Sido { get; set; }
        public string Apt_Adress_Gun { get; set; }
        public string Apt_Adress_Rest { get; set; }
        public string CorporateResistration_Num { get; set; }
        public DateTime AcceptancedOfWork_Date { get; set; }
        public int LevelCount { get; set; }
        public string Staff_code { get; set; }
        public DateTime PostDate { get; set; }
        public string PostIP { get; set; }
        public string combile { get; set; }

        public string mem_id { get; set; }
        public string mem_nm { get; set; }
    }

    /// <summary>
    /// 공동주택 스테프 정보
    /// </summary>
    public class Apt_Staff_Join_Entity
    {
        public int Aid { get; set; }
        public string Apt_Name { get; set; }
        public string Apt_Form { get; set; }
        public string Apt_Adress_Sido { get; set; }
        public string Apt_Adress_Gun { get; set; }
        public string Apt_Adress_Rest { get; set; }
        public string CorporateResistration_Num { get; set; }
        public DateTime AcceptancedOfWork_Date { get; set; }
        public int LevelCount { get; set; }
        public DateTime PostDate { get; set; }
        public string PostIP { get; set; }

        public int Staff_Aid { get; set; }
        public string Apt_Code { get; set; }
        public string Staff_Division { get; set; }
        public string Staff_Code { get; set; }
        public string Staff_Name { get; set; }
        public DateTime BirthDate { get; set; }

        // 직원 권한 등급
        public int Staff_LevelCount { get; set; }

        public string Staff_Etc { get; set; }
    }

    //부서 및 직책
    public class Staff_PostDuty_Entity
    {
        public int Aid { get; set; }
        public string Sort_Division { get; set; }
        public string PostDuty_Division { get; set; }
        public string PostDuty_Code { get; set; }
        public string PostDuty_Name { get; set; }
        public string Up_Code { get; set; }
        public string PostDuty_Step { get; set; }
        public string PostDuty_Etc { get; set; }
        public string Staff_Code { get; set; }
        public DateTime PostDate { get; set; }
        public string PostIP { get; set; }
    }

    // 관리자 정보
    public class Staff_Entity
    {
        public int Aid { get; set; }
        public string Apt_Code { get; set; }
        public string Staff_Division { get; set; }
        public string Staff_Code { get; set; }
        public string Staff_Name { get; set; }
        public DateTime BirthDate { get; set; }

        public string License_Number { get; set; }
        public string License_Order { get; set; }

        // 직원 권한 등급
        public int LevelCount { get; set; }

        public string Staff_Etc { get; set; }
        public DateTime PostDate { get; set; }
        public string PostIP { get; set; }
    }

    // 직원 정보
    public class Apt_Staff_Entity
    {
        public int Aid { get; set; }
        public string Apt_Code { get; set; }
        public string Staff_Division { get; set; }
        public string Apt_Staff_Code { get; set; }
        public string Staff_Name { get; set; }
        public DateTime BirthDate { get; set; }

        // 직원 권한 등급
        public int LevelCount { get; set; }

        public string Staff_Etc { get; set; }
        public DateTime PostDate { get; set; }
        public string PostIP { get; set; }
    }

    // 로그인 정보
    public class Present_Entity
    {
        public int Aid { get; set; }
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public string Staff_Code { get; set; }
        public string Staff_Name { get; set; }
        public DateTime Log_time { get; set; }
        public DateTime LogOut_time { get; set; }
        public string PostIP { get; set; }
    }

    //직원 상세
    public class Staff_Detail_Entity
    {
        public int Detail_Aid { get; set; }
        public string Staff_Detail_Code { get; set; }
        public string Apt_Code { get; set; }
        public string Apt_Staff_Code { get; set; }
        public string Post_Code { get; set; }
        public string Duty_Code { get; set; }
        public string Staff_Division { get; set; }
        public string Staff_Adress { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public DateTime JoinDate { get; set; }
        public DateTime? ResignDate { get; set; }
        public string Staff_Detail_Etc { get; set; }
        public DateTime PostDate { get; set; }
        public string PostIP { get; set; }
    }

    // 직원_조인
    public class Staff_Detaill_Staff_Entity
    {
        public int Aid { get; set; }
        public string Apt_Code { get; set; }
        public string Staff_Division { get; set; }
        public string Apt_Staff_Code { get; set; }
        public string Staff_Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string Staff_Etc { get; set; }

        //public DateTime PostDate { get; set; }
        //public string PostIP { get; set; }
        public int Detail_Aid { get; set; }

        public string Staff_Detail_Code { get; set; }

        //public string Apt_Code { get; set; }
        //public string Staff_Code { get; set; }
        public string Post_Code { get; set; }

        public string Duty_Code { get; set; }

        // public string Staff_Division { get; set; }
        public string Staff_Adress { get; set; }

        public string Telephone { get; set; }
        public DateTime JoinDate { get; set; }
        public DateTime? ResignDate { get; set; }
        public string Staff_Detail_Etc { get; set; }

        //public DateTime PostDate { get; set; }
        public string Resign_Check { get; set; }
    }

    // 주민 대표 엔터티
    public class Apt_Worker_Entity
    {
        public int Aid { get; set; }
        public string Apt_Code { get; set; }
        public string Worker_Code { get; set; }
        public string Worker_Name { get; set; }
        public string Dong { get; set; }
        public string Ho { get; set; }
        public string Post { get; set; }
        public string Duty { get; set; }
        public int Degree { get; set; }
        public string Telephone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Election_Division { get; set; }
        public DateTime Start_Date { get; set; }
        public DateTime End_Date { get; set; }
        public DateTime BirthDate { get; set; }
        public string Worker_Etc { get; set; }
        public DateTime PostDate { get; set; }
        public string Staff_Code { get; set; }
        public string PostIP { get; set; }
    }

    /// <summary>
    /// 주택관리사협회 회원 정보 및 단지 정보 조인 엔터티
    /// </summary>
    public class Khma_Member_Infor_Entity
    {
        /// <summary>
        /// 회원 식별 코드
        /// </summary>
        public string mem_cd { get; set; }

        public string mem_id { get; set; }

        /// <summary>
        /// 공동주택 식별코드
        /// </summary>
        public string apt_cd { get; set; }

        /// <summary>
        /// 공동주택명
        /// </summary>
        public string apt_nm { get; set; }

        /// <summary>
        /// 일련번호
        /// </summary>
        public string seq { get; set; }

        /// <summary>
        /// 배치 여부
        /// </summary>
        public string state { get; set; }

        /// <summary>
        /// 회원명
        /// </summary>
        public string mem_nm { get; set; }

        /// <summary>
        /// 출생일
        /// </summary>
        public string birth_date { get; set; }

        /// <summary>
        /// 시도회 식별코드
        /// </summary>
        public string localcode { get; set; }

        /// <summary>
        /// 회원등급 코드
        /// </summary>
        public string levelcode { get; set; }

        /// <summary>
        /// 공동주택 주소
        /// </summary>
        public string addr_base { get; set; }

        /// <summary>
        /// 공동주택 주소 상세
        /// </summary>
        public string addr_detail { get; set; }

        /// <summary>
        /// 공동주택 연락처
        /// </summary>
        public string tel_no { get; set; }

        /// <summary>
        /// 공동주택 팩스
        /// </summary>
        public string fax_no { get; set; }

        /// <summary>
        /// 공동주택 관리 면적
        /// </summary>
        public string managearea { get; set; }

        /// <summary>
        /// 공동주택 동 수
        /// </summary>
        public string dong_cnt { get; set; }

        /// <summary>
        /// 공동주택 승강기 설치 여부
        /// </summary>
        public string elevator_yn { get; set; }

        /// <summary>
        /// 공동주택 승강기 수
        /// </summary>
        public string elevator_cnt { get; set; }

        /// <summary>
        /// 공동주택 승강기 층수
        /// </summary>
        public string upperfloor { get; set; }

        /// <summary>
        /// 공동주택 지하 층 수
        /// </summary>
        public string underfloor { get; set; }

        /// <summary>
        /// 사용검사일
        /// </summary>
        public string BUILD_DATE { get; set; }
    }

    /// <summary>
    /// 협회 공동주택정보
    /// </summary>
    public class khma_AptInfor_Entity
    {
        public string APT_CD { get; set; }
        public string LOCALCODE { get; set; }
        public string APT_NM { get; set; }
        public string ADDR_BASE { get; set; }
        public string ADDR_DETAIL { get; set; }

        /// <summary>
        /// 공동주택 연락처
        /// </summary>
        public string TEL_NO { get; set; }

        /// <summary>
        /// 공동주택 팩스
        /// </summary>
        public string FAX_NO { get; set; }

        /// <summary>
        /// 공동주택 관리 면적
        /// </summary>
        public string MANAGEAREA { get; set; }

        /// <summary>
        /// 공동주택 동 수
        /// </summary>
        public string DONG_CNT { get; set; }

        /// <summary>
        /// 공동주택 승강기 설치 여부
        /// </summary>
        public string ELEVATOR_YN { get; set; }

        /// <summary>
        /// 공동주택 승강기 수
        /// </summary>
        public string ELEVATOR_CNT { get; set; }

        /// <summary>
        /// 공동주택 승강기 층수
        /// </summary>
        public string UPPERFLOOR { get; set; }

        /// <summary>
        /// 공동주택 지하 층 수
        /// </summary>
        public string UNDERFLOOR { get; set; }

        /// <summary>
        /// 사용검사일
        /// </summary>
        public string BUILD_DATE { get; set; }
    }

    /// <summary>
    /// 협회 회원정보
    /// </summary>
    public class khma_mem_Entity
    {
        public string mem_cd { get; set; }
        public string mem_nm { get; set; }
        public string mem_id { get; set; }
        public string mem_pw { get; set; }
        public string dupcode { get; set; }
        public string birth_date { get; set; }
        public string localcode { get; set; }
        public string levelcode { get; set; }
        public string state { get; set; }
    }

    /// <summary>
    /// 협회 회원 배치 정보
    /// </summary>
    public class khma_mem_career_Entity
    {
        public int Aid { get; set; }
        public string mem_cd { get; set; }
        public string seq { get; set; }
        public string apt_cd { get; set; }
        public string state { get; set; }
        public string mem_id { get; set; }
        public DateTime PostDate { get; set; }
    }

    // 공동주택 및 배치 정보
    public class Apt_career_Join_Entity
    {
        public int Aid { get; set; }
        public string Apt_Name { get; set; }
        public string Apt_Form { get; set; }
        public string Apt_Adress_Sido { get; set; }
        public string Apt_Adress_Gun { get; set; }
        public string Apt_Adress_Rest { get; set; }
        public string CorporateResistration_Num { get; set; }
        public DateTime AcceptancedOfWork_Date { get; set; }
        public int LevelCount { get; set; }
        public DateTime PostDate { get; set; }
        public string PostIP { get; set; }

        // 배치 정보
        public string Apt_Code { get; set; }

        public string mem_cd { get; set; }
        public string seq { get; set; }
        public string apt_cd { get; set; }
        public string state { get; set; }
        public string mem_id { get; set; }

        //public DateTime PostDate { get; set; }
    }
}