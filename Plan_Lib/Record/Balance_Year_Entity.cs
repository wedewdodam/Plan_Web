namespace Plan_Blazor_Lib.Record
{
    /// <summary>
    /// 장기수선충당금 사용 및 적립현황 값 구하기 엔터티
    /// </summary>
    public class Balance_Year_Entity
    {
        /// <summary>
        /// 현재 잔액
        /// </summary>
        public double balance_sum { get; set; }

        /// <summary>
        /// 초기화 잔액
        /// </summary>
        public double Reset_balance { get; set; }

        /// <summary>
        /// 초기화 이후 전년도까지 사용액
        /// </summary>
        public double Using_Cost { get; set; }

        /// <summary>
        /// 초기화 이후 전년도까지 부과 적립액
        /// </summary>
        public double Capital_Levy { get; set; }
    }
}