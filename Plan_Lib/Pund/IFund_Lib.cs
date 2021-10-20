using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plan_Lib.Pund
{
    public interface IRepair_Saving_Using_Pund_Lib
    {
        Task<Repair_Saving_Using_Pund_Entity> Add(Repair_Saving_Using_Pund_Entity sup);

        Task<Repair_Saving_Using_Pund_Entity> Edit(Repair_Saving_Using_Pund_Entity sup);

        Task<List<Repair_Saving_Using_Pund_Entity>> GetList(string Apt_Code);

        /// <summary>
        /// 작성년도로 장충금 사용 및 적립현황 정보 존재여부 확인
        /// </summary>
        Task<int> Being_Count(string Apt_Code, string Report_Year);

        /// <summary>
        /// 해당 식별코드로 장기수선충당금 적립 및 사용 현황 상세정보
        /// </summary>
        /// <param name="Apt_Code">공동주택 식별코드</param>
        /// <param name="Aid">장기수선충당금 사용 및 적립현황 식별코드</param>
        /// <returns>해당 식별코드 검색된 정보</returns>
        Task<Repair_Saving_Using_Pund_Entity> Detail(string Apt_Code, string Report_Year);

        /// <summary>
        /// 장기수선충당금 사용 및 적립현황 해당 공동주택 선택년도 정보
        /// </summary>
        /// <param name="Apt_Code">공동주택 식별코드</param>
        /// <param name="Year">입력된 년도</param>
        /// <returns>년도별 정보</returns>
        Task<Repair_Saving_Using_Pund_Entity> Detail_Year(string Apt_Code, string Year);

        /// <summary>
        /// 장기수선충당금 사용 및 적립현황 삭제
        /// </summary>
        /// <param name="Aid">장기수선충당금 사용 및 적립현황 식별코드</param>
        Task Delete(int Aid);

        /// <summary>
        /// 장기수선충당금 사용 및 적립현황 존재여부
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="_year"></param>
        /// <returns></returns>
        Task<int> Being(string Apt_Code, string _year);

        /// <summary>
        /// 장기수선충당금 적립 및 사용현황 정보 보고일
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="_year"></param>
        /// <returns></returns>
        Task<DateTime> date(string Apt_Code, string _year);

        /// <summary>
        /// 장기수선충당금 적립 및 사용현황 정보의 식별코드
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="_year"></param>
        /// <returns></returns>
       Task<string> Be_Code(string Apt_Code, string _year);
    }
}