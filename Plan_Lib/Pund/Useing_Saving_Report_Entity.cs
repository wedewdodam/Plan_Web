using System;
using System.Collections.Generic;
using System.Text;

namespace Plan_Lib.Pund
{
    public class Useing_Saving_Report_Entity
    {
        /// <summary>
        /// 전년도 말 잔액
        /// </summary>
        public double dbBalanceAgoSum { get; set; }

        /// <summary>
        /// 전전년도 말 잔액
        /// </summary>
        public double dbBalanceAgoAgoSum { get; set; }

        /// <summary>
        /// 현재 잔액
        /// </summary>
        public double dbBalanceNowSum { get; set; }

        /// <summary>
        /// 전년말 사용 총액
        /// </summary>
        public double dbUseingAgoSum { get; set; }

        /// <summary>
        /// 전전년말 사용 총액
        /// </summary>
        public double dbUseingAgoAgoSum { get; set; }

        /// <summary>
        /// 전년도 말 적립율에 의한 부과 총액
        /// </summary>
        public double dbLateAgoSaving { get; set; }

        /// <summary>
        /// 초기화 잔액
        /// </summary>
        public double dbResetBalance { get; set; }

        /// <summary>
        /// 초기화 사용액
        /// </summary>
        public double dbUsingCostSum { get; set; }

        /// <summary>
        /// 초기화 부과총액
        /// </summary>
        public double dbResetUsing { get; set; }

        /// <summary>
        /// 초기화 이후 전년말 징수적립액
        /// </summary>
        public double dbLavyAgoCapital { get; set; }

        /// <summary>
        /// 초기화 이후 현재가지 징수적립액
        /// </summary>
        public double dbLavyNowCapital { get; set; }

        /// <summary>
        /// 계획기간 중 수선비 총액
        /// </summary>
        public double dbTotalPlanSum { get; set; }

        public double LevyRateSum { get; set; }
    }
}
