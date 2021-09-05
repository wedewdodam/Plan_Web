using System;

namespace Plan_Blazor_Lib.Record
{
    /// <summary>
    /// 장기수선충당금 관련 정보 입력 완료 정보 엔터티
    /// </summary>
    public class Appropriation_Prosess_Entity
    {
        public int Aid { get; set; }
        public string Apt_Code { get; set; }
        public string Identifition_Code { get; set; }
        public string Identifition_Name { get; set; }
        public string Complete { get; set; }
        public DateTime Complete_date { get; set; }
        public string Complete_year { get; set; }
        public DateTime PostDate { get; set; }
        public string PostIP { get; set; }
        public string Etc { get; set; }
    }
}