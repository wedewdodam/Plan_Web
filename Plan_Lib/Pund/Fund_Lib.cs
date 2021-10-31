using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Plan_Lib.Pund
{
    /// <summary>
    /// 장기수선충당금 사용 및 적립 현황 클래스
    /// </summary>
    public class Repair_Saving_Using_Pund_Lib : IRepair_Saving_Using_Pund_Lib
    {
        private readonly IConfiguration _db;

        public Repair_Saving_Using_Pund_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 장기수선충당금 사용 및 적립 현황 입력
        /// </summary>
        /// <param name="sup">디비 필드명</param>
        /// <returns>입력된 값</returns>
        public async Task<Repair_Saving_Using_Pund_Entity> Add(Repair_Saving_Using_Pund_Entity sup)
        {
            var sql = "Insert Into Repair_Using_Saving_Fund (Apt_Code, Apt_Name, Staff_Name, Founding_Date, Adjust_Date, Report_Date, Report_Year, Family_Num, Adress, Plan_Funds, Saving_Funds, Using_Funds_ago, Using_Funds_now, Using_Funds, Balance_Funds, Real_Balance_Funds, Need_Funds, Unit_Price, Month_Impose, Supply_Area, Etc, PostIP, Staff_Code) Values (@Apt_Code, @Apt_Name, @Staff_Name, @Founding_Date, @Adjust_Date, @Report_Date, @Report_Year, @Family_Num, @Adress, @Plan_Funds, @Saving_Funds, @Using_Funds_ago, @Using_Funds_now, @Using_Funds, @Balance_Funds, @Real_Balance_Funds, @Need_Funds, @Unit_Price, @Month_Impose, @Supply_Area, @Etc, @PostIP, @Staff_Code)";
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            await db.ExecuteAsync(sql, sup);
            return sup;
        }

        /// <summary>
        /// 장기수선충당금 사용 및 적립 현황 수정
        /// </summary>
        /// <param name="sup">디비 필드</param>
        /// <returns>디비 수정</returns>
        public async Task<Repair_Saving_Using_Pund_Entity> Edit(Repair_Saving_Using_Pund_Entity sup)
        {
            var sql = "Update Repair_Using_Saving_Fund Set Adjust_Date = @Adjust_Date, Report_Date = @Report_Date, Report_Year = @Report_Year, Family_Num = @Family_Num, Adress = @Adress, Plan_Funds = @Plan_Funds, Saving_Funds = @Saving_Funds, Using_Funds_ago = @Using_Funds_ago, Using_Funds_now = @Using_Funds_now, Using_Funds = @Using_Funds, Balance_Funds = @Balance_Funds, Real_Balance_Funds = @Real_Balance_Funds, Need_Funds = @Need_Funds, Unit_Price = @Unit_Price, Month_Impose = @Month_Impose, Supply_Area = @Supply_Area, Etc = @Etc, PostIP = @PostIP, Staff_Code = @Staff_Code Where Aid = @Aid";
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            await db.ExecuteAsync(sql, sup);
            return sup;
        }

        /// <summary>
        /// 장기수선충당금 사용 및 적립현황 목록(단지별)
        /// </summary>
        /// <param name="Apt_Code">공동주택단지 식별코드</param>
        /// <returns>단지별 목록</returns>
        public async Task<List<Repair_Saving_Using_Pund_Entity>> GetList(string Apt_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var lst = await db.QueryAsync<Repair_Saving_Using_Pund_Entity>("Select * From Repair_Using_Saving_Fund Where Apt_Code = @Apt_Code Order By Aid Desc", new { Apt_Code });
            return lst.ToList();
        }

        /// <summary>
        /// 작성년도로 장충금 사용 및 적립현황 정보 존재여부 확인
        /// </summary>
        /// <param name="Write_Date"></param>
        /// <returns></returns>
        public async Task<int> Being_Count(string Apt_Code, string Report_Year)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Repair_Using_Saving_Fund Where Apt_Code = @Apt_Code And Report_Year = @Report_Year", new { Apt_Code, Report_Year });
        }

        /// <summary>
        /// 해당 식별코드로 장기수선충당금 적립 및 사용 현황 상세정보
        /// </summary>
        /// <param name="Apt_Code">공동주택 식별코드</param>
        /// <param name="Aid">장기수선충당금 사용 및 적립현황 식별코드</param>
        /// <returns>해당 식별코드 검색된 정보</returns>
        public async Task<Repair_Saving_Using_Pund_Entity> Detail(string Apt_Code, string Report_Year)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<Repair_Saving_Using_Pund_Entity>("Select * From Repair_Using_Saving_Fund Where Apt_Code = @Apt_Code And Report_Year = @Report_Year", new { Apt_Code, Report_Year });
        }

        /// <summary>
        /// 장기수선충당금 사용 및 적립현황 해당 공동주택 선택년도 정보
        /// </summary>
        /// <param name="Apt_Code">공동주택 식별코드</param>
        /// <param name="Year">입력된 년도</param>
        /// <returns>년도별 정보</returns>
        public async Task<Repair_Saving_Using_Pund_Entity> Detail_Year(string Apt_Code, string Year)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<Repair_Saving_Using_Pund_Entity>("Select * From Repair_Using_Saving_Fund Where Apt_Code = @Apt_Code And Report_Year = @Year", new { Apt_Code, Year });
        }

        /// <summary>
        /// 장기수선충당금 사용 및 적립현황 삭제
        /// </summary>
        /// <param name="Aid">장기수선충당금 사용 및 적립현황 식별코드</param>
        public async Task Delete(int Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            await db.ExecuteAsync("Delete From Repair_Using_Saving_Fund Where Aid = @Aid", new { Aid });
        }

        /// <summary>
        /// 장기수선충당금 사용 및 적립현황 존재여부
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="_year"></param>
        /// <returns></returns>
        public async Task<int> Being(string Apt_Code, string _year)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Repair_Using_Saving_Fund Where Apt_Code = @Apt_Code And Report_Year = @_year", new { Apt_Code, _year });
        }

        /// <summary>
        /// 장기수선충당금 적립 및 사용현황 정보 보고일
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="_year"></param>
        /// <returns></returns>
        public async Task<DateTime> date(string Apt_Code, string _year)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<DateTime>("Select Top 1 Report_Date From Repair_Using_Saving_Fund Where Apt_Code = @Apt_Code And Report_Year = @_year Order By Aid Desc", new { Apt_Code, _year });
        }

        /// <summary>
        /// 장기수선충당금 적립 및 사용현황 정보의 식별코드
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="_year"></param>
        /// <returns></returns>
        public async Task<string> Be_Code(string Apt_Code, string _year)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<string>("Select Top 1 Aid From Repair_Using_Saving_Fund Where Apt_Code = @Apt_Code And Report_Year = @_year Order By Aid Desc", new { Apt_Code, _year });
        }
    }

    public class Present_Condition_Lib
    {
        private readonly IConfiguration _db;

        public Present_Condition_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 장충금 사용 및 적립현황 저장하기
        /// </summary>
        /// <param name="pc"></param>
        /// <returns></returns>
        public async Task<Present_Condition_Entity> Add(Present_Condition_Entity pc)
        {
            var sql = "Insert into Present_Condition (Apt_Code, Write_Year, Write_Date, Standard_Date, PlanCost_Sum, Levy_Saving_Sum, PlanUsing_Sum_ago, PlanUsing_Sum_now, PlanUsing_Sum, Balance_Sum, ReserveSaving_Sum, Month_Reverve_Sum, Founding_Date, Adjustment_Date, Month_Unit_Price, Family_Count_Num, supply_total_Area, Etc, Staff_Name, PostIp) Values (@Apt_Code, @Write_Year, @Write_Date, @Standard_Date, @PlanCost_Sum, @Levy_Saving_Sum, @PlanUsing_Sum_ago, @PlanUsing_Sum_now, @PlanUsing_Sum, @Balance_Sum, @ReserveSaving_Sum, @Month_Reverve_Sum, @Founding_Date, @Adjustment_Date, @Month_Unit_Price, @Family_Count_Num, @supply_total_Area, @Etc, @Staff_Name, @PostIp)";
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            await db.ExecuteAsync(sql, pc);
            return pc;
        }

        /// <summary>
        /// 장충금 사용 및 적립현황 수정하기
        /// </summary>
        /// <param name="pc"></param>
        /// <returns></returns>
        public async Task<Present_Condition_Entity> Edit(Present_Condition_Entity pc)
        {
            var sql = "Update Present_Condition Set Write_Year = @Wirte_Year, Write_Date = @Write_Date, Standard_Date = @Standard_Date, PlanCost_Sum, Levy_Saving_Sum = @Levy_Saving_Sum, PlanUsing_Sum_ago = @PlanUsing_Sum_ago, PlanUsing_Sum_now = @PlanUsing_Sum_now, PlanUsing_Sum = @PlanUsing_Sum, Balance_Sum = @Balance_Sum, ReserveSaving_Sum = @ReserveSaving_Sum, Month_Reverve_Sum = @Month_Reverve_Sum, Founding_Date = @Founding_Date, Adjustment_Date = @Adjustment_Date, Month_Unit_Price = @Month_Unit_Price, Family_Count_Num = @Family_Count_Num, supply_total_Area = @supply_total_Area, Etc = @Etc, Staff_Name = @Staff_Name, PostDate = Getdate(), PostIp = @PostIp)";
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            await db.QuerySingleOrDefaultAsync(sql, pc);
            return pc;
        }

        /// <summary>
        /// 해당 공동주택 식별코드 장충금 사용 및 적립 현황 리스트 만들기
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<List<Present_Condition_Entity>> GetList(string Apt_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var lst = await db.QueryAsync<Present_Condition_Entity>("Select * From Present_Condition Where Apt_Code = @Apt_Code Order By Aid Desc", new { Apt_Code });
            return lst.ToList();
        }

        /// <summary>
        /// 작성년도로 장충금 사용 및 적립현황 정보 불러오기
        /// </summary>
        /// <param name="Write_Date"></param>
        /// <returns></returns>
        public async Task<Present_Condition_Entity> Detail(string Apt_Code, string Write_Year)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<Present_Condition_Entity>("Select * From Present_Condition Where Write_Year = @Write_Year", new { Apt_Code, Write_Year });
        }

        /// <summary>
        /// 장충금 상세 인쇄용
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="Write_Year"></param>
        /// <returns></returns>
        public async Task<Present_Condition_string_Entity> Detail_Read(string Apt_Code, string Write_Year)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<Present_Condition_string_Entity>("Repair_Saving_State_Report_Read", new { Apt_Code, Write_Year }, commandType: System.Data.CommandType.StoredProcedure);
        }

        /// <summary>
        /// 작성년도로 장충금 사용 및 적립현황 정보 존재여부 확인
        /// </summary>
        /// <param name="Write_Date"></param>
        /// <returns></returns>
        public async Task<int> Being_Count(string Apt_Code, string Write_Year)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Present_Condition Where Report_Year = @Write_Year", new { Apt_Code, Write_Year });
        }

        /// <summary>
        /// 장충금 사용 및 적립 현황 삭제하기
        /// </summary>
        /// <param name="Aid"></param>
        public async Task Remove(string Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            await db.ExecuteAsync("Delete Present_Condition Where Aid = @Aid", new { Aid });
        }
    }
}