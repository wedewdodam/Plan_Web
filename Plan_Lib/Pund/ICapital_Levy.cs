using Plan_Blazor_Lib.Plan;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plan_Lib.Pund
{
    /// <summary>
    /// 단가 관련 클래스 인터페이스
    /// </summary>
    public interface IUnit_Price_Lib
    {
        Task<Unit_Price_Entity> Unit_Price(string Apt_Code, string Repair_Plan_Code);

        Task<Unit_Price_Entity> Detail_Unit_Price(string Apt_Code, string Repair_Plan_Code, double Levy_Rate, int Levy_Period);

        Task<Unit_Price_Entity> Detail_Unit_Price_New(string Apt_Code, string Repair_Plan_Code, double Levy_Rate, int Levy_Period);

        Unit_Price_Entity Detail_Unit_Price_Up(string Apt_Code, string Repair_Plan_Code, double Levy_Rate, int Levy_Period);

        Task<Unit_Price_string_Entity> Report_Plan_Cost(string Apt_Code, string Repair_Plan_Code);

        Task<Unit_Price_string_Entity> Report_Plan_Cost_Bylaw(string Apt_Code, string Repair_Plan_Code, string Bylaw_Code);

        //Task<Unit_Price_Entity> Detail_Unit_Price();
    }

    /// <summary>
    /// 장충금 부과 금액 인터페이스
    /// </summary>
    public interface ICapital_Levy_Lib
    {
        Task<Capital_Levy_Entity> Add_CL(Capital_Levy_Entity cl);

        Task<Capital_Levy_Entity> Edit(Capital_Levy_Entity cl);

        Task Delete(string Capital_Levy_Code);

        Task<List<Capital_Levy_Entity>> GetList(string Apt_Code, int Levy_Year);

        Task<List<Capital_Levy_Entity>> Get_Year_List(string Apt_Code);

        Task<double> Get_Year_Sum(string Apt_Code, int Levy_Year);

        Task<double> Get_Year_Month_Sum(string Apt_Code, int Levy_Year, int Levy_Month);

        Task<double> Get_Apt_Sum(string Apt_Code, string Use_Year);

        Task<int> Being_Levy(string Apt_Code, int Levy_Year, int Levy_Month, string Levy_Account);

        Task<int> be_Capital_Levy(string Apt_Code, int Levy_Year);

        Task<string> be_Capital_Levy_Code(string Apt_Code, int Levy_Year);

        Task<DateTime> be_Capital_Levy_date(string Apt_Code, int Levy_Year);

        Task<double> Capital_Levy_Now_Sum(string Apt_Code, int Frist_Year, string Now_date);

        /// <summary>
        /// 년간합계(월 기준)
        /// </summary>
       double _Year_Month_Sum(string Apt_Code, int Levy_Year, int Levy_Month);

        /// <summary>
        /// 부과징수액(년간)
        /// </summary>
        Task<double> _Year_Plan_Sum(string Apt_Code, int Year, string Account);

        /// <summary>
        /// 잉여금 등 합계액(년간)
        /// </summary>
        Task<double> _Year_Etc_Sum(string Apt_Code, int Year, string Account);

        /// <summary>
        /// 해당 년도, 월, 계정과목 중복확인
        /// </summary>
        Task<int> being_Capital_Levy(string Apt_Code, int Levy_Year, int Levy_Month, string Levy_Account);
    }

    public interface ILevy_Rate_Lib
    {
        Task<Levy_Rate_Entity> Add_Levy_Rate(Levy_Rate_Entity LR);

        Task<Levy_Rate_Entity> Edit_Levy_Rate(Levy_Rate_Entity LR);

        Task Delete_Levy_Rate(int Levy_Rate_Code);

        Task Delete_All_Levy_Rate(string Apt_Code, int Bylaw_Code);

        Task<List<Levy_Rate_Entity>> GetList_Levy_Rate(string Apt_Code, int Bylaw_Code);

        Task<double> Ago_Levy_Rate(string Apt_Code, int Bylaw_Code, string Levy_Start_Year);

        Task<List<Levy_Rate_Entity>> GetList_Levy_Rate_Bylaw(string Apt_Code, int Bylaw_Code);

        Task<int> Levy_Period_Total(string Apt_Code, int Bylaw_Code);

        Task<int> Levy_Last_Year(string Apt_Code, int Bylaw_Code);

        Task<DateTime> Levy_Last_EndDate(string Apt_Code, int Bylaw_Code);

        Task<DateTime> Levy_Last_StartDate(string Apt_Code, int Bylaw_Code);

        Task<double> Levy_Rate_Accumulate(string Apt_Code, int Bylaw_Code);

        Task<double> Levy_Rate_Now(string Apt_Code, int Now_Year, string Bylaw_Code);

        Task<Levy_Rate_Entity> Levy_Rate_Year(string Apt_Code, int Now_Year, string Bylaw_Code);

        Task<int> Levy_Rate_Year_Count(string Apt_Code, int Now_Year, string Bylaw_Code);

        Task<int> Levy_Rate_Now_Period(string Apt_Code, int Now_Year, string Bylaw_Code);

        Task<int> Levy_Rate_Now_Period_New(string Apt_Code, int Now_Year, string Bylaw_Code);

        Task<List<Levy_Rate_Entity>> GetList_Area_Levy(string Apt_Code, string Bylaw_Code);

        Task<Levy_Rate_Entity> Detail_Year_Levy(string Apt_Code, string Bylaw_Code, int Year);

        /// <summary>
        /// 해당 단지의 마지막 적립요율 정보 가져오기
        /// </summary>
        Task<Levy_Rate_Entity> Detail_New(string Apt_Code, string Bylaw_Code);

        Task<int> Detail_Year_Levy_Be(string Apt_Code, string Bylaw_Code, int Year);

        Task<Levy_Rate_Entity> Detail_Year_Levy_Next(string Apt_Code, string Bylaw_Code, int Year);

        Task<int> Detail_Year_Levy_Next_Be(string Apt_Code, string Bylaw_Code, int Year);

        Task<List<Levy_Rate_Entity>> GetList_Rate_Levy(string Top_Count);

        Task<int> Being_Count(string Apt_Code, int Now_Year);

        Task<int> LevyRate_Count(string Apt_Code, string Bylaw_Code);

        Task<List<Levy_Rate_Entity>> GetList_Rate_Levy_Cost(string Apt_Code, string Repair_Plan_Code, string Levy_Rate_Code, double Levy_Rate, int Levy_Period);

        Task<List<Levy_Rate_Entity>> GetList_Rate_Levy_Cost_New(string Apt_Code, string Repair_Plan_Code, string Levy_Rate_Code, double Levy_Rate, int Levy_Period);

        Task<int> Bylaw_Levy_Count(string Apt_Code, string Bylaw_Code);

        Task<Levy_Rate_Entity> GetDetail(string Levy_Rate_Code);

        Task<int> Being_LevyRate(string Apt_Code);

        Task<string> Levy_Rate_Code_Search(string Apt_Code, string Bylaw_Code, string Num);

        Task<Levy_Rate_Entity> Detail_Levy_Rate_Code(string Levy_Rate_Code);

        /// <summary>
        /// 부과율 합계
        /// </summary>
        Task<int> Levy_Period_Total_New(string Apt_Code, int Bylaw_Code);

        /// <summary>
        /// 해당 단지 부과율에 따른 면적별 부과금액 정보 가져 오기
        /// </summary>
        List<Levy_Rate_Entity> GetList_Rate_Levy_Cost_A(string Apt_Code, string Repair_Plan_Code, string Levy_Rate_Code, double Levy_Rate, int Levy_Period);

        /// <summary>
        /// 부과개월 수 합계
        /// </summary>
        Task<int> Levy_Month(string Apt_Code, int ByLaw_Code);

        /// <summary>
        /// 해당 단지 부과율에 따른 면적별 부과금액 정보 가져 오기(단일)
        /// </summary>
        Task<Levy_Rate_Entity> Details_Rate_Levy_Cost_Now(string Apt_Code, string Repair_Plan_Code, string Levy_Rate_Code, double Levy_Rate, int Levy_Period);

    }

    /// <summary>
    /// 초기화 인터페이스
    /// </summary>
    public interface IRepair_Capital_Lib
    {
        /// <summary>
        /// 초기화 입력
        /// </summary>
        /// <param name="ReC"></param>
        /// <returns></returns>
        Task<Repair_Capital_Entity> Add_Repair_Capital(Repair_Capital_Entity ReC);

        /// <summary>
        /// 초기화 수정
        /// </summary>
        /// <param name="ReC"></param>
        /// <returns></returns>
        Task<Repair_Capital_Entity> Edit_Repair_Capital(Repair_Capital_Entity ReC);

        /// <summary>
        /// 초기화 잔액 불러오기
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        Task<double> GetDetail_Balance_Capital(string Apt_Code);

        Task<int> Being_Capital(string Apt_Code);

        Task<DateTime> Being_Capital_Date(string Apt_Code);

        Task<Repair_Capital_Entity> _detail(string Apt_Code);

        Task<int> Being_Capital_Code(string Apt_Code);

        /// <summary>
        /// 초기화 부과 총액 불러오기
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        Task<double> GetDetail_Levy_Capital(string Apt_Code);

        /// <summary>
        /// 초기화 사용액 불러오기
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        Task<double> GetDetail_Use_Cost(string Apt_Code);

        /// <summary>
        /// 잔액만들기
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        Task<double> BalanceSum(string Apt_Code);

        /// <summary>
        /// 해당년도 말 잔액 만들기
        /// </summary>
       Task<Useing_Saving_Report_Entity> BalanceSum_Year(string Apt_Code, int Year);
    }

    /// <summary>
    /// 장기수선충당금 사용 인터페이스
    /// </summary>
    public interface IRepair_Capital_Use_Lib
    {
        Task<Repair_Capital_Use_Entity> Add_Capital_Use(Repair_Capital_Use_Entity RCU);

        Task<Repair_Capital_Use_Entity> Edit_Capital_Use(Repair_Capital_Use_Entity RCU);

        Task Remove_Capital_Use(int Repair_Capital_Use_Code);

        Task<List<Repair_Capital_Use_Entity>> GetList_Capital_Use_Year(string Apt_Code, int Use_Year);

        Task<List<Repair_Capital_Use_Entity>> GetList_Capital_Use_Year_Division(string Apt_Code, int Use_Year, string Use_Division);

        Task<List<Repair_Capital_Use_Entity>> GetList_Capital_Use_Month(string Apt_Code, int Use_Year, int Use_Month);

        Task<List<Repair_Capital_Use_Entity>> GetList_Capital_Use_Month_Division(string Apt_Code, int Use_Year, int Use_Month, string Use_Division);

        Task<Repair_Capital_Use_Entity> GetDetail(int Repair_Capital_Use_Code);

        Task<List<Join_Article_Cycle_Cost_EntityA>> GetList_Using_Cost(string Apt_Code, string Repair_Plan_Code, string Repair_Plan_Year);

        Task<double> Using_Cost_TotalSum(string Apt_Code);

        Task<List<Join_Article_Cycle_Cost_EntityA>> GetList_Main_Plan(string Apt_Code, string Repair_Plan_Code, string Repair_Plan_Year);
    }
    
}