using System.ComponentModel.DataAnnotations;

namespace Plan_Apt_Lib
{
    /// <summary>
    /// 공동주택 정보
    /// </summary>
    public class khma_apart_info_v_Entity
    {
        /// <summary>
        /// 아파트 식별코드
        /// </summary>
        public string APT_CD { get; set; }

        /// <summary>
        /// 지역코드
        /// </summary>
        public string LOCALCODE { get; set; }

        /// <summary>
        /// 아파트 명
        /// </summary>
        public string APT_NM { get; set; }

        /// <summary>
        /// 아파트 주소 기본
        /// </summary>
        public string ADDR_BASE { get; set; }

        /// <summary>
        /// 아파트 주소 상세
        /// </summary>
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
    /// 회원 정보
    /// </summary>
    public class khma_member_Info_v_Entity
    {
        /// <summary>
        /// 회원 식별코드
        /// </summary>
        public string mem_cd { get; set; }

        /// <summary>
        /// 회원명
        /// </summary>
        public string mem_nm { get; set; }

        /// <summary>
        /// 회원 아아디
        /// </summary>
        [Required(ErrorMessage = "아이디를 입력하세요.")]
        public string mem_id { get; set; }

        /// <summary>
        /// 회원 암호
        /// </summary>
        public string mem_pw { get; set; }

        /// <summary>
        /// 회원 식별코드
        /// </summary>
        public string dupcode { get; set; }

        /// <summary>
        /// 출생일
        /// </summary>
        public string birth_date { get; set; }

        /// <summary>
        /// 지역 코드
        /// </summary>
        public string localcode { get; set; }

        /// <summary>
        /// 정회원여부
        /// </summary>
        public string levelcode { get; set; }

        /// <summary>
        /// 상태
        /// </summary>
        public string state { get; set; }
    }

    /// <summary>
    /// 협회 배치 정보
    /// </summary>
    public class khma_mem_career_v_Enity
    {
        /// <summary>
        /// 회원 식별코드
        /// </summary>
        public string mem_cd { get; set; }

        public string seq { get; set; }

        /// <summary>
        /// 공동주택 식별코드
        /// </summary>
        public string apt_cd { get; set; }

        public string state { get; set; }
    }

    public class khma_AptMemberCareer_Entity
    {
        /// <summary>
        /// 회원 식별코드
        /// </summary>
        public string mem_cd { get; set; }

        public string seq { get; set; }

        /// <summary>
        /// 공동주택 식별코드
        /// </summary>
        public string apt_cd { get; set; }

        public string state { get; set; }

        ////////////

        /// <summary>
        /// 회원명
        /// </summary>
        public string mem_nm { get; set; }

        /// <summary>
        /// 회원 아아디
        /// </summary>
        public string mem_id { get; set; }

        /// <summary>
        /// 회원 암호
        /// </summary>
        public string mem_pw { get; set; }

        /// <summary>
        /// 회원 식별코드
        /// </summary>
        public string dupcode { get; set; }

        /// <summary>
        /// 출생일
        /// </summary>
        public string birth_date { get; set; }

        /// <summary>
        /// 지역 코드
        /// </summary>
        public string localcode { get; set; }

        /// <summary>
        /// 정회원여부
        /// </summary>
        public string levelcode { get; set; }

        //////////

        /// <summary>
        /// 지역코드
        /// </summary>
        public string LOCALCODE { get; set; }

        /// <summary>
        /// 아파트 명
        /// </summary>
        public string APT_NM { get; set; }

        /// <summary>
        /// 아파트 주소 기본
        /// </summary>
        public string ADDR_BASE { get; set; }

        /// <summary>
        /// 아파트 주소 상세
        /// </summary>
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
}