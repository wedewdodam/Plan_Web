using Dapper;
using Microsoft.Extensions.Configuration;
using Plan_Blazor_Lib.Plan;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Plan_Lib.Pund
{
    /// <summary>
    /// 단가관련 클래스
    /// </summary>
    public class Unit_Price_Lib : IUnit_Price_Lib
    {
        private readonly IConfiguration _db;

        public Unit_Price_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 단가 만들기
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="Repair_Plan_Code"></param>
        /// <returns></returns>
        public async Task<Unit_Price_Entity> Unit_Price(string Apt_Code, string Repair_Plan_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Unit_Price_Entity>("Report_Total_Unit_Cost_New", new { Apt_Code, Repair_Plan_Code }, commandType: CommandType.StoredProcedure);
            }
            //return dn.ctx.Query<Unit_Price_Entity>("Report_Total_Unit_Cost_New", new { Apt_Code, Repair_Plan_Code }, commandType: CommandType.StoredProcedure).FirstOrDefault();
        }

        // <summary>
        /// 단가 구하기 (숫자)
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="Repair_Plan_Code"></param>
        /// <param name="Levy_Rate"></param>
        /// <param name="Levy_Period"></param>
        /// <returns></returns>
        public async Task<Unit_Price_Entity> Detail_Unit_Price(string Apt_Code, string Repair_Plan_Code, double Levy_Rate, int Levy_Period)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<Unit_Price_Entity>("Unt_Price", new { Apt_Code, Repair_Plan_Code, Levy_Rate, Levy_Period }, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// 단가 구하기 (숫자)
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="Repair_Plan_Code"></param>
        /// <param name="Levy_Rate"></param>
        /// <param name="Levy_Period"></param>
        /// <returns></returns>
        public async Task<Unit_Price_Entity> Detail_Unit_Price_New(string Apt_Code, string Repair_Plan_Code, double Levy_Rate, int Levy_Period)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<Unit_Price_Entity>("Unt_Price_New", new { Apt_Code, Repair_Plan_Code, Levy_Rate, Levy_Period }, commandType: System.Data.CommandType.StoredProcedure);
        }

        /// <summary>
        ///  단가 텍스트(문자)
        /// </summary>
        public async Task<Unit_Price_string_Entity> Report_Plan_Cost(string Apt_Code, string Repair_Plan_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<Unit_Price_string_Entity>("Report_Plan_Cost_New", new { Apt_Code, Repair_Plan_Code }, commandType: System.Data.CommandType.StoredProcedure);
        }

        /// <summary>
        ///  단가 텍스트(문자)(관리규약 적립률)
        /// </summary>
        public async Task<Unit_Price_string_Entity> Report_Plan_Cost_Bylaw(string Apt_Code, string Repair_Plan_Code, string Bylaw_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<Unit_Price_string_Entity>("Report_Plan_Cost_New_ByLaw", new { Apt_Code, Repair_Plan_Code, Bylaw_Code }, commandType: System.Data.CommandType.StoredProcedure);
        }

        //Task<Unit_Price_Entity> IUnit_Price_Lib.Detail_Unit_Price()
        //{
        //    throw new NotImplementedException();
        //}
    }

    /// <summary>
    /// 장충금 부과 금액 클래스
    /// </summary>
    public class Capital_Levy_Lib : ICapital_Levy_Lib
    {
        private readonly IConfiguration _db;

        public Capital_Levy_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        // 월 부과금액 입력
        public async Task<Capital_Levy_Entity> Add_CL(Capital_Levy_Entity cl)
        {
            var sql = "Insert Into Capital_Levy (Levy_Rate_Code, Apt_Code, Law_Levy_Month_Sum, Levy_Month_Sum, Levy_Account, Levy_Year, Levy_Month, Levy_Day, Levy_Date, Capital_Levy_Etc, Staff_Code, PostIP) Values (@Levy_Rate_Code, @Apt_Code, @Law_Levy_Month_Sum, @Levy_Month_Sum, @Levy_Account, @Levy_Year, @Levy_Month, @Levy_Day, @Levy_Date, @Capital_Levy_Etc, @Staff_Code, @PostIP)";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, cl, commandType: CommandType.Text);
                return cl;
            }
            //this.dn.ctx.Execute(sql, cl);
            //return cl;
        }

        // 월 부과금액 수정
        public async Task<Capital_Levy_Entity> Edit(Capital_Levy_Entity cl)
        {
            var sql = "Update Capital_Levy Set Levy_Rate_Code = @Levy_Rate_Code, Apt_Code = @Apt_Code, Law_Levy_Month_Sum = @Law_Levy_Month_Sum, Levy_Month_Sum = @Levy_Month_Sum, Levy_Account = @Levy_Account, Levy_Year = @Levy_Year, Levy_Month = @Levy_Month, Levy_Day = @Levy_Day, Levy_Date = @Levy_Date, Capital_Levy_Etc =  @Capital_Levy_Etc, Staff_Code = @Staff_Code, PostDate = @PostDate, PostIP = @PostIP Where Capital_Levy_Code = @Capital_Levy_Code";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, cl, commandType: CommandType.Text);
                return cl;
            }
            //this.dn.ctx.Execute(sql, cl);
            //return cl;
        }

        // 월 부과금액 삭제
        public async Task Delete(string Capital_Levy_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync("Delete From Capital_Levy Where Capital_Levy_Code = @Capital_Levy_Code", new { Capital_Levy_Code }, commandType: CommandType.Text);
                //return cl;
            }
            //this.dn.ctx.Execute("Delete From Capital_Levy Where Capital_Levy_Code = @Capital_Levy_Code", new { Capital_Levy_Code });
        }

        /// <summary>
        /// 년도별 월 부과금액 리스트
        /// </summary>
        public async Task<List<Capital_Levy_Entity>> GetList(string Apt_Code, int Levy_Year)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aa = await ctx.QueryAsync<Capital_Levy_Entity>("Select * From Capital_Levy Where Apt_Code = @Apt_Code And Levy_Year = @Levy_Year Order By Levy_Year Asc, Levy_Month Asc, Capital_Levy_Code Asc", new { Apt_Code, Levy_Year }, commandType: CommandType.Text);
                return aa.ToList();
            }
            //return this.dn.ctx.Query<Capital_Levy_Entity>("Select * From Capital_Levy Where Apt_Code = @Apt_Code And Levy_Year = @Levy_Year Order By Levy_Year Asc, Levy_Month Asc, Capital_Levy_Code Asc", new { Apt_Code, Levy_Year }).ToList();
        }

        /// <summary>
        /// 입력된 년도 리스트(중복 제거)
        /// </summary>
        public async Task<List<Capital_Levy_Entity>> Get_Year_List(string Apt_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aa = await ctx.QueryAsync<Capital_Levy_Entity>("Select distinct Levy_Year From Capital_Levy Where Apt_Code = @Apt_Code Order by Levy_Year Asc", new { Apt_Code }, commandType: CommandType.Text);
                return aa.ToList();
            }
            //return this.dn.ctx.Query<Capital_Levy_Entity>("Select distinct Levy_Year From Capital_Levy Where Apt_Code = @Apt_Code Order by Levy_Year Asc", new { Apt_Code }).ToList();
        }

        //public List<string> Getlist_Adjustment_Year(string Apt_Code)
        //{
        //    var sql = "Select distinct Adjustment_Year From Repair_Plan Where Apt_Code = @Apt_Code";
        //    return this.ctx.Query<string>(sql, new { Apt_Code }).ToList();
        //}

        /// <summary>
        /// 부과금액 년간 합계
        /// </summary>

        public async Task<double> Get_Year_Sum(string Apt_Code, int Levy_Year)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<double>("Select isNull(Sum(Levy_Month_Sum), 0) From Capital_Levy Where Apt_Code = @Apt_Code And Levy_Year = @Levy_Year", new { Apt_Code, Levy_Year }, commandType: CommandType.Text);
            }
            //return this.dn.ctx.Query<double>("Select isNull(Sum(Levy_Month_Sum), 0) From Capital_Levy Where Apt_Code = @Apt_Code And Levy_Year = @Levy_Year", new { Apt_Code, Levy_Year });
        }

        /// <summary>
        /// 부과금액 월간 합계
        /// </summary>

        public async Task<double> Get_Year_Month_Sum(string Apt_Code, int Levy_Year, int Levy_Month)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<double>("Select isNull(Sum(Levy_Month_Sum), 0) From Capital_Levy Where Apt_Code = @Apt_Code And Levy_Year = @Levy_Year And Levy_Month <= @Levy_Month", new { Apt_Code, Levy_Year, Levy_Month }, commandType: CommandType.Text);
            }
            //return this.dn.ctx.Query<double>("Select isNull(Sum(Levy_Month_Sum), 0) From Capital_Levy Where Apt_Code = @Apt_Code And Levy_Year = @Levy_Year And Levy_Month <= @Levy_Month", new { Apt_Code, Levy_Year, Levy_Month });
        }

        /// <summary>
        /// 단지별 부과금액 합계(징수 적립액)
        /// </summary>
        /// Use_Year = 기준년도
        public async Task<double> Get_Apt_Sum(string Apt_Code, string Use_Year)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<double>("Select isNull(Sum(Levy_Month_Sum), 0) From Capital_Levy Where Apt_Code = @Apt_Code And Levy_Year > @Use_Year ", new { Apt_Code, Use_Year }, commandType: CommandType.Text);
            }
            //return this.dn.ctx.Query<double>("Select isNull(Sum(Levy_Month_Sum), 0) From Capital_Levy Where Apt_Code = @Apt_Code And Levy_Year > @Use_Year ", new { Apt_Code, Use_Year });
        }

        // 월 부과금액 존재 여부
        public async Task<int> Being_Levy(string Apt_Code, int Levy_Year, int Levy_Month, string Levy_Account)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Capital_Levy Where Apt_Code = @Apt_Code And Levy_Year = @Levy_Year And Levy_Month = @Levy_Month And Levy_Account = @Levy_Account", new { Apt_Code, Levy_Year, Levy_Month, Levy_Account }, commandType: CommandType.Text);
            }
            //return this.dn.ctx.Query<int>("Select Count(*) From Capital_Levy Where Apt_Code = @Apt_Code And Levy_Year = @Levy_Year And Levy_Month = @Levy_Month And Levy_Account = @Levy_Account", new { Apt_Code, Levy_Year, Levy_Month, Levy_Account });
        }

        /// <summary>
        /// 해당 년도 공동주택의 부과 및 징수 존재 여부
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="Levy_Year"></param>
        /// <returns></returns>
        public async Task<int> be_Capital_Levy(string Apt_Code, int Levy_Year)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Capital_Levy Where Apt_Code = @Apt_Code And Levy_Year = @Levy_Year", new { Apt_Code, Levy_Year }, commandType: CommandType.Text);
            }
            //return dn.ctx.Query<int>("Select Count(*) From Capital_Levy Where Apt_Code = @Apt_Code And Levy_Year = @Levy_Year", new { Apt_Code, Levy_Year });
        }

        /// <summary>
        /// 해당 년도 공동주택의 부과 및 징수 마지막 식별코드
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="Levy_Year"></param>
        /// <returns></returns>
        public async Task<string> be_Capital_Levy_Code(string Apt_Code, int Levy_Year)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<string>("Select Top 1 Capital_Levy_Code From Capital_Levy Where Apt_Code = @Apt_Code And Levy_Year = @Levy_Year Order By Capital_Levy_Code Desc", new { Apt_Code, Levy_Year }, commandType: CommandType.Text);
            }
            //return dn.ctx.Query<string>("Select Top 1 Capital_Levy_Code From Capital_Levy Where Apt_Code = @Apt_Code And Levy_Year = @Levy_Year Order By Capital_Levy_Code Desc", new { Apt_Code, Levy_Year });
        }

        /// <summary>
        /// 해당 년도 공동주택의 부과 및 징수 마지막 징수일
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="Levy_Year"></param>
        /// <returns></returns>
        public async Task<DateTime> be_Capital_Levy_date(string Apt_Code, int Levy_Year)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<DateTime>("Select Top 1 Levy_Date From Capital_Levy Where Apt_Code = @Apt_Code And Levy_Year = @Levy_Year Order By Capital_Levy_Code Desc", new { Apt_Code, Levy_Year }, commandType: CommandType.Text);
            }
            //return dn.ctx.Query<DateTime>("Select Top 1 Levy_Date From Capital_Levy Where Apt_Code = @Apt_Code And Levy_Year = @Levy_Year Order By Capital_Levy_Code Desc", new { Apt_Code, Levy_Year });
        }

        /// <summary>
        /// 현재 까지 부과 금액
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="Frist_Year"></param>
        /// <param name="Now_date"></param>
        /// <returns></returns>
        public async Task<double> Capital_Levy_Now_Sum(string Apt_Code, int Frist_Year, string Now_date)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<double>("Select IsNull(Sum(Levy_Month_Sum), 0) From Capital_Levy Where Apt_Code = @Apt_Code And Levy_Year > @Frist_Year and (Levy_Date <= @Now_date)", new { Apt_Code, Frist_Year, Now_date }, commandType: CommandType.Text);
            }
        }
    }

    /// <summary>
    /// 장충금 적립율 클래스(관리규약)
    /// </summary>
    public class Levy_Rate_Lib : ILevy_Rate_Lib
    {
        private readonly IConfiguration _db;

        public Levy_Rate_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 장기수선충당금 적립율 입력
        /// </summary>
        public async Task<Levy_Rate_Entity> Add_Levy_Rate(Levy_Rate_Entity LR)
        {
            var sql = "Insert Into Levy_Rate (Apt_Code, Bylaw_Code, Bylaw_Date, Levy_Start_Year, Levy_Start_Month, Levy_Start_Day, Levy_Start_Date, Levy_End_Year, Levy_End_Month, Levy_End_Day, Levy_End_Date, Levy_Rate, Levy_Period, Levy_Period_New, Levy_Rate_Accumulate, Staff_Code, PostIP) Values (@Apt_Code, @Bylaw_Code, @Bylaw_Date, @Levy_Start_Year, @Levy_Start_Month, @Levy_Start_Day, @Levy_Start_Date, @Levy_End_Year, @Levy_End_Month, @Levy_End_Day, @Levy_End_Date, @Levy_Rate, @Levy_Period, @Levy_Period_New, @Levy_Rate_Accumulate, @Staff_Code, @PostIP)";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, LR);
                return LR;
            }
            //this.dn.ctx.Execute(sql, LR);
            //return LR;
        }

        /// <summary>
        /// 장기수선충당금 적립율 수정
        /// </summary>
        public async Task<Levy_Rate_Entity> Edit_Levy_Rate(Levy_Rate_Entity LR)
        {
            var sql = "Update Levy_Rate Set Levy_Start_Year = @Levy_Start_Year, Levy_Start_Month = @Levy_Start_Month, Levy_Start_Day = @Levy_Start_Day, Levy_Start_Date = @Levy_Start_Date, Levy_End_Year = @Levy_End_Year, Levy_End_Month = @Levy_End_Month, Levy_End_Day = @Levy_End_Day, Levy_End_Date = @Levy_End_Date, Levy_Rate = @Levy_Rate, Levy_Period = @Levy_Period, Levy_Rate_Accumulate = @Levy_Rate_Accumulate, Staff_Code = @Staff_Code, PostDate = @PostDate, PostIP = @PostIP Where Levy_Rate_Code = @Levy_Rate_Code)";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, LR);
                return LR;
            }
            //this.dn.ctx.Execute(sql, LR);
            //return LR;
        }

        /// <summary>
        /// 장기수선충당금 적립율 삭제
        /// </summary>
        /// <param name="Levy_Rate_Code"></param>
        public async Task Delete_Levy_Rate(int Levy_Rate_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync("Delete From Levy_Rate Where Levy_Rate_Code = @Levy_Rate_Code", new { Levy_Rate_Code }, commandType: CommandType.Text);
            }
            //this.dn.ctx.Execute("Delete From Levy_Rate Where Levy_Rate_Code = @Levy_Rate_Code", new { Levy_Rate_Code });
        }

        /// <summary>
        /// 장기수선충당금 적립율 모두 삭제
        /// </summary>
        /// <param name="Levy_Rate_Code"></param>
        public async Task Delete_All_Levy_Rate(string Apt_Code, int Bylaw_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync("Delete From Levy_Rate Where Apt_Code = @Apt_Code And Bylaw_Code = @Bylaw_Code", new { Apt_Code, Bylaw_Code }, commandType: CommandType.Text);
            }
            //this.dn.ctx.Execute("Delete From Levy_Rate Where Apt_Code = @Apt_Code And Bylaw_Code = @Bylaw_Code", new { Apt_Code, Bylaw_Code });
        }

        /// <summary>
        /// 장기수선충당금 적립율 해당 공동주택 리스트
        /// </summary>
        public async Task<List<Levy_Rate_Entity>> GetList_Levy_Rate(string Apt_Code, int Bylaw_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aa = await ctx.QueryAsync<Levy_Rate_Entity>("Select * From Levy_Rate Where Apt_Code = @Apt_Code And Bylaw_Code = @Bylaw_Code Order By Levy_Start_Year Asc", new { Apt_Code, Bylaw_Code }, commandType: CommandType.Text);
                return aa.ToList();
            }
            //return this.dn.ctx.Query<Levy_Rate_Entity>("Select * From Levy_Rate Where Apt_Code = @Apt_Code And Bylaw_Code = @Bylaw_Code Order By Levy_Start_Year Asc", new { Apt_Code, Bylaw_Code }).ToList();
        }

        /// <summary>
        /// 전기 적립율
        /// </summary>
        public async Task<double> Ago_Levy_Rate(string Apt_Code, int Bylaw_Code, string Levy_Start_Year)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<double>("Select isnull(Sum([Levy_Rate]), 0) From Levy_Rate Where Apt_Code = @Apt_Code And Bylaw_Code = @Bylaw_Code And Levy_End_Year <= @Levy_Start_Year", new { Apt_Code, Bylaw_Code, Levy_Start_Year }, commandType: CommandType.Text);
            }
            //return this.dn.ctx.Query<double>("Select isnull(Sum([Levy_Rate]), 0) From Levy_Rate Where Apt_Code = @Apt_Code And Bylaw_Code = @Bylaw_Code And Levy_End_Year <= @Levy_Start_Year", new { Apt_Code, Bylaw_Code, Levy_Start_Year });
        }

        /// <summary>
        /// 장기수선충당금 적립율 해당 공동주택 리스트
        /// </summary>
        public async Task<List<Levy_Rate_Entity>> GetList_Levy_Rate_Bylaw(string Apt_Code, int Bylaw_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aa = await ctx.QueryAsync<Levy_Rate_Entity>("Select * From Levy_Rate Where Apt_Code = @Apt_Code And Bylaw_Code = @Bylaw_Code", new { Apt_Code, Bylaw_Code }, commandType: CommandType.Text);
                return aa.ToList();
            }
            //return this.dn.ctx.Query<Levy_Rate_Entity>("Select * From Levy_Rate Where Apt_Code = @Apt_Code And Bylaw_Code = @Bylaw_Code", new { Apt_Code, Bylaw_Code }).ToList();
        }

        // 부과율 년도 합계
        public async Task<int> Levy_Period_Total(string Apt_Code, int Bylaw_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select isNull(Sum(Levy_Period_New), 0) From Levy_Rate Where  Apt_Code = @Apt_Code And Bylaw_Code = @Bylaw_Code", new { Apt_Code, Bylaw_Code }, commandType: CommandType.Text);
            }
            //return this.dn.ctx.Query<int>("Select isNull(Sum(Levy_Period_New), 0) From Levy_Rate Where  Apt_Code = @Apt_Code And Bylaw_Code = @Bylaw_Code", new { Apt_Code, Bylaw_Code });
        }

        /// <summary>
        /// 해당 공동주택의 입력된 관리규약 상의 요율 중에 입력된 마지막 구간 말의 년도
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="Bylaw_Code"></param>
        /// <returns></returns>
        public async Task<int> Levy_Last_Year(string Apt_Code, int Bylaw_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Top 1 Levy_End_Year From Levy_Rate Where Apt_Code = @Apt_Code And Bylaw_Code = @Bylaw_Code Order By Levy_End_Year Desc", new { Apt_Code, Bylaw_Code }, commandType: CommandType.Text);
            }
            //return this.dn.ctx.Query<int>("Select Top 1 Levy_End_Year From Levy_Rate Where Apt_Code = @Apt_Code And Bylaw_Code = @Bylaw_Code Order By Levy_End_Year Desc", new { Apt_Code, Bylaw_Code });
        }

        /// <summary>
        /// 해당 공동주택의 마지막 관리규에 마지막 적립요율 종료 년월 불러오기
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="Bylaw_Code"></param>
        /// <returns></returns>
        public async Task<DateTime> Levy_Last_EndDate(string Apt_Code, int Bylaw_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<DateTime>("Select Top 1 Levy_End_Date From Levy_Rate Where Apt_Code = @Apt_Code And Bylaw_Code = @Bylaw_Code Order By Levy_End_Year Desc, Levy_Rate_Code Desc", new { Apt_Code, Bylaw_Code }, commandType: CommandType.Text);
            }
            //return dn.ctx.Query<DateTime>("Select Top 1 Levy_End_Date From Levy_Rate Where Apt_Code = @Apt_Code And Bylaw_Code = @Bylaw_Code Order By Levy_End_Year Desc, Levy_Rate_Code Desc", new { Apt_Code, Bylaw_Code });
        }

        /// <summary>
        /// 해당 공동주택의 마지막 관리규에 마지막 적립요율 시작 년월 불러오기
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="Bylaw_Code"></param>
        /// <returns></returns>
        public async Task<DateTime> Levy_Last_StartDate(string Apt_Code, int Bylaw_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<DateTime>("Select Top 1 Levy_Start_Date From Levy_Rate Where Apt_Code = @Apt_Code And Bylaw_Code = @Bylaw_Code Order By Levy_End_Year Desc", new { Apt_Code, Bylaw_Code }, commandType: CommandType.Text);
            }
            //return dn.ctx.Query<DateTime>("Select Top 1 Levy_Start_Date From Levy_Rate Where Apt_Code = @Apt_Code And Bylaw_Code = @Bylaw_Code Order By Levy_End_Year Desc", new { Apt_Code, Bylaw_Code });
        }

        /// <summary>
        /// 부과율 년도 합계
        /// </summary>
        public async Task<double> Levy_Rate_Accumulate(string Apt_Code, int Bylaw_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<double>("Select isnull(Sum(Levy_Rate), 0) From Levy_Rate Where  Apt_Code = @Apt_Code And Bylaw_Code = @Bylaw_Code", new { Apt_Code, Bylaw_Code }, commandType: CommandType.Text);
            }
            //return this.dn.ctx.Query<double>("Select isnull(Sum(Levy_Rate), 0) From Levy_Rate Where  Apt_Code = @Apt_Code And Bylaw_Code = @Bylaw_Code", new { Apt_Code, Bylaw_Code });
        }

        /// <summary>
        /// 현재 년도 부과율 구하기
        /// </summary>
        public async Task<double> Levy_Rate_Now(string Apt_Code, int Now_Year, string Bylaw_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<double>("Select Top 1 Levy_Rate From Levy_Rate Where Levy_Start_Year <= @Now_Year And Levy_End_Year >= @Now_Year And Apt_Code = @Apt_Code And Bylaw_Code = @Bylaw_Code Order By Levy_Rate_Code Desc", new { Apt_Code, Now_Year, Bylaw_Code }, commandType: CommandType.Text);
            }
            //return this.dn.ctx.Query<double>("Select Top 1 Levy_Rate From Levy_Rate Where Levy_Start_Year <= @Now_Year And Levy_End_Year >= @Now_Year And Apt_Code = @Apt_Code And Bylaw_Code = @Bylaw_Code Order By Levy_Rate_Code Desc", new { Apt_Code, Now_Year, Bylaw_Code });
        }

        /// <summary>
        /// 현재 년도 첫 년도, 마지막 년도 구하기
        /// </summary>
        public async Task<Levy_Rate_Entity> Levy_Rate_Year(string Apt_Code, int Now_Year, string Bylaw_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Levy_Rate_Entity>("Select Top 1 * From Levy_Rate Where Levy_Start_Year <= @Now_Year And Levy_End_Year >= @Now_Year And Apt_Code = @Apt_Code And Bylaw_Code = @Bylaw_Code Order By Levy_Rate_Code Desc", new { Apt_Code, Now_Year, Bylaw_Code }, commandType: CommandType.Text);
            }
            //return this.dn.ctx.Query<Levy_Rate_Entity>("Select Top 1 * From Levy_Rate Where Levy_Start_Year <= @Now_Year And Levy_End_Year >= @Now_Year And Apt_Code = @Apt_Code And Bylaw_Code = @Bylaw_Code Order By Levy_Rate_Code Desc", new { Apt_Code, Now_Year, Bylaw_Code });
        }

        /// <summary>
        /// 현재 년도 첫 년도, 마지막 년도 존재 여부 확인
        /// </summary>
        public async Task<int> Levy_Rate_Year_Count(string Apt_Code, int Now_Year, string Bylaw_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Levy_Rate Where Levy_Start_Year <= @Now_Year And Levy_End_Year >= @Now_Year And Apt_Code = @Apt_Code And Bylaw_Code = @Bylaw_Code", new { Apt_Code, Now_Year, Bylaw_Code }, commandType: CommandType.Text);
            }
            //return this.dn.ctx.Query<int>("Select Count(*) From Levy_Rate Where Levy_Start_Year <= @Now_Year And Levy_End_Year >= @Now_Year And Apt_Code = @Apt_Code And Bylaw_Code = @Bylaw_Code", new { Apt_Code, Now_Year, Bylaw_Code });
        }

        /// <summary>
        /// 현재년도 부과기간 구하기
        /// </summary>
        public async Task<int> Levy_Rate_Now_Period(string Apt_Code, int Now_Year, string Bylaw_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Top 1 Levy_Period From Levy_Rate Where Levy_Start_Year <= @Now_Year And Levy_End_Year >= @Now_Year And Apt_Code = @Apt_Code And Bylaw_Code = @Bylaw_Code Order By Levy_Rate_Code Desc", new { Apt_Code, Now_Year, Bylaw_Code }, commandType: CommandType.Text);
            }
            //return this.dn.ctx.Query<int>("Select Top 1 Levy_Period From Levy_Rate Where Levy_Start_Year <= @Now_Year And Levy_End_Year >= @Now_Year And Apt_Code = @Apt_Code And Bylaw_Code = @Bylaw_Code Order By Levy_Rate_Code Desc", new { Apt_Code, Now_Year, Bylaw_Code });
        }

        /// <summary>
        /// 현재년도 부과기간 구하기(new)
        /// </summary>
        public async Task<int> Levy_Rate_Now_Period_New(string Apt_Code, int Now_Year, string Bylaw_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Top 1 Levy_Period_New From Levy_Rate Where Levy_Start_Year <= @Now_Year And Levy_End_Year >= @Now_Year And Apt_Code = @Apt_Code And Bylaw_Code = @Bylaw_Code Order By Levy_Rate_Code Desc", new { Apt_Code, Now_Year, Bylaw_Code }, commandType: CommandType.Text);
            }
            //return this.dn.ctx.Query<int>("Select Top 1 Levy_Period_New From Levy_Rate Where Levy_Start_Year <= @Now_Year And Levy_End_Year >= @Now_Year And Apt_Code = @Apt_Code And Bylaw_Code = @Bylaw_Code Order By Levy_Rate_Code Desc", new { Apt_Code, Now_Year, Bylaw_Code });
        }

        /// <summary>
        /// 해당 단지 모든 적립율에 따른 부과금액 산출( 면적별)
        /// </summary>
        public async Task<List<Levy_Rate_Entity>> GetList_Area_Levy(string Apt_Code, string Bylaw_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aa = await ctx.QueryAsync<Levy_Rate_Entity>("Select a.Levy_Rate_Code, a.Bylaw_Code, a.Levy_Start_Year, a.Levy_End_Year, a.Levy_Period, a.Levy_Period_New, a.Levy_Rate, a.Levy_Rate_Accumulate, b.Supply_Area, b.Area_Family_Num From Levy_Rate As a Join Dong_Composition As b on a.Apt_Code = b.Apt_Code Where a.Bylaw_Code = @Bylaw_Code And a.Apt_Code = @Apt_Code", new { Apt_Code, Bylaw_Code }, commandType: CommandType.Text);
                return aa.ToList();
            }
            //return this.dn.ctx.Query<Levy_Rate_Entity>("Select a.Levy_Rate_Code, a.Bylaw_Code, a.Levy_Start_Year, a.Levy_End_Year, a.Levy_Period, a.Levy_Period_New, a.Levy_Rate, a.Levy_Rate_Accumulate, b.Supply_Area, b.Area_Family_Num From Levy_Rate As a Join Dong_Composition As b on a.Apt_Code = b.Apt_Code Where a.Bylaw_Code = @Bylaw_Code And a.Apt_Code = @Apt_Code", new { Apt_Code, Bylaw_Code }).ToList();
        }

        /// <summary>
        /// 해당 단지에 해당년도 적립요율 정보 가져오기(전)
        /// </summary>
        public async Task<Levy_Rate_Entity> Detail_Year_Levy(string Apt_Code, string Bylaw_Code, int Year)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Levy_Rate_Entity>("Select Top 1 Levy_Rate_Code, Apt_Code, Bylaw_Code, Bylaw_Date, Levy_Start_Date, Levy_Start_Year, Levy_Start_Month, Levy_Start_Day, Levy_End_Date, Levy_End_Year, Levy_End_Month, Levy_End_Day, Levy_Period, Levy_Period_New, Levy_Rate, Levy_Rate_Accumulate, PostDate From Levy_Rate Where Levy_Start_Year <= @Year And Levy_Start_Month > 1 And Levy_End_Year <= @Year And Levy_End_Month < 12 and Apt_Code = @Apt_Code And Bylaw_Code = @Bylaw_Code Order by Levy_Rate_Code Desc", new { Apt_Code, Bylaw_Code, Year }, commandType: CommandType.Text);
            }
            //return this.dn.ctx.Query<Levy_Rate_Entity>("Select Top 1 Levy_Rate_Code, Apt_Code, Bylaw_Code, Bylaw_Date, Levy_Start_Date, Levy_Start_Year, Levy_Start_Month, Levy_Start_Day, Levy_End_Date, Levy_End_Year, Levy_End_Month, Levy_End_Day, Levy_Period, Levy_Period_New, Levy_Rate, Levy_Rate_Accumulate, PostDate From Levy_Rate Where Levy_Start_Year <= @Year And Levy_Start_Month > 1 And Levy_End_Year <= @Year And Levy_End_Month < 12 and Apt_Code = @Apt_Code And Bylaw_Code = @Bylaw_Code Order by Levy_Rate_Code Desc", new { Apt_Code, Bylaw_Code, Year });
        }

        /// <summary>
        /// 현재 년도 적립율 정보 존재 여부
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="Bylaw_Code"></param>
        /// <param name="Year"></param>
        /// <returns></returns>
        public async Task<int> Detail_Year_Levy_Be(string Apt_Code, string Bylaw_Code, int Year)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Levy_Rate Where Levy_Start_Year <= @Year And Levy_Start_Month > 1 And Levy_End_Year <= @Year And Levy_End_Month < 12 and Apt_Code = @Apt_Code And Bylaw_Code = @Bylaw_Code", new { Apt_Code, Bylaw_Code, Year }, commandType: CommandType.Text);
            }
            //return this.dn.ctx.Query<int>("Select Count(*) From Levy_Rate Where Levy_Start_Year <= @Year And Levy_Start_Month > 1 And Levy_End_Year <= @Year And Levy_End_Month < 12 and Apt_Code = @Apt_Code And Bylaw_Code = @Bylaw_Code", new { Apt_Code, Bylaw_Code, Year });
        }

        /// <summary>
        /// 해당 단지에 해당년도 적립요율 정보 가져오기(후)
        /// </summary>
        public async Task<Levy_Rate_Entity> Detail_Year_Levy_Next(string Apt_Code, string Bylaw_Code, int Year)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Levy_Rate_Entity>("Select Top 1 Levy_Rate_Code, Apt_Code, Bylaw_Code, Bylaw_Date, Levy_Start_Date, Levy_Start_Year, Levy_Start_Month, Levy_Start_Day, Levy_End_Date, Levy_End_Year, Levy_End_Month, Levy_End_Day, Levy_Period, Levy_Period_New, Levy_Rate, Levy_Rate_Accumulate, PostDate From Levy_Rate Where Levy_Start_Year <= @Year And Levy_Start_Month > 1 And Levy_End_Year >= @Year And Levy_End_Month <= 12 and Apt_Code = @Apt_Code And Bylaw_Code = @Bylaw_Code Order by Levy_Rate_Code Desc", new { Apt_Code, Bylaw_Code, Year }, commandType: CommandType.Text);
            }
            //return this.dn.ctx.Query<Levy_Rate_Entity>("Select Top 1 Levy_Rate_Code, Apt_Code, Bylaw_Code, Bylaw_Date, Levy_Start_Date, Levy_Start_Year, Levy_Start_Month, Levy_Start_Day, Levy_End_Date, Levy_End_Year, Levy_End_Month, Levy_End_Day, Levy_Period, Levy_Period_New, Levy_Rate, Levy_Rate_Accumulate, PostDate From Levy_Rate Where Levy_Start_Year <= @Year And Levy_Start_Month > 1 And Levy_End_Year >= @Year And Levy_End_Month <= 12 and Apt_Code = @Apt_Code And Bylaw_Code = @Bylaw_Code Order by Levy_Rate_Code Desc", new { Apt_Code, Bylaw_Code, Year });
        }

        /// <summary>
        /// 현재 년도 적립율 정보 존재 여부
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="Bylaw_Code"></param>
        /// <param name="Year"></param>
        /// <returns></returns>
        public async Task<int> Detail_Year_Levy_Next_Be(string Apt_Code, string Bylaw_Code, int Year)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Levy_Rate Where Levy_Start_Year <= @Year And Levy_Start_Month > 1 And Levy_End_Year >= @Year And Levy_End_Month <= 12 and Apt_Code = @Apt_Code And Bylaw_Code = @Bylaw_Code", new { Apt_Code, Bylaw_Code, Year }, commandType: CommandType.Text);
            }
            //return this.dn.ctx.Query<int>("Select Count(*) From Levy_Rate Where Levy_Start_Year <= @Year And Levy_Start_Month > 1 And Levy_End_Year >= @Year And Levy_End_Month <= 12 and Apt_Code = @Apt_Code And Bylaw_Code = @Bylaw_Code", new { Apt_Code, Bylaw_Code, Year });
        }

        /// <summary>
        /// 해당 단지 부과율에 따른 면적별 부과금액 정보 가져 오기
        /// </summary>
        public async Task<List<Levy_Rate_Entity>> GetList_Rate_Levy(string Top_Count)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aa = await ctx.QueryAsync<Levy_Rate_Entity>("Rate_Levy", new { Top_Count }, commandType: System.Data.CommandType.StoredProcedure);
                return aa.ToList();
            }
            //return this.dn.ctx.Query<Levy_Rate_Entity>("Rate_Levy", new { Top_Count }, commandType: System.Data.CommandType.StoredProcedure).ToList();
        }

        /// <summary>
        /// 해당 단지 해당 년도 적립율에 따른 정보 존재여부 가져 오기
        /// </summary>
        public async Task<int> Being_Count(string Apt_Code, int Now_Year)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Levy_Rate Where Levy_Start_Year <= @Now_Year And Levy_End_Year >= @Now_Year And Apt_Code = @Apt_Code", new { Apt_Code, Now_Year }, commandType: CommandType.Text);
            }

            //return this.dn.ctx.Query<int>("Select Count(*) From Levy_Rate Where Levy_Start_Year <= @Now_Year And Levy_End_Year >= @Now_Year And Apt_Code = @Apt_Code", new { Apt_Code, Now_Year });
        }

        /// <summary>
        /// 관리규약 코드로 입력된 적립율 숫자
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="Bylaw_Code"></param>
        /// <returns></returns>
        public async Task<int> LevyRate_Count(string Apt_Code, string Bylaw_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Levy_Rate Where Apt_Code = @Apt_Code And Bylaw_Code = @Bylaw_Code", new { Apt_Code, Bylaw_Code }, commandType: CommandType.Text);
            }

            //return dn.ctx.Query<int>("Select Count(*) From Levy_Rate Where Apt_Code = @Apt_Code And Bylaw_Code = @Bylaw_Code", new { Apt_Code, Bylaw_Code });
        }

        /// <summary>
        /// 해당 단지 부과율에 따른 면적별 부과금액 정보 가져 오기
        /// </summary>
        public async Task<List<Levy_Rate_Entity>> GetList_Rate_Levy_Cost(string Apt_Code, string Repair_Plan_Code, string Levy_Rate_Code, double Levy_Rate, int Levy_Period)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aa = await ctx.QueryAsync<Levy_Rate_Entity>("Report_Cost_Saving", new { Apt_Code, Repair_Plan_Code, Levy_Rate_Code, Levy_Rate, Levy_Period }, commandType: System.Data.CommandType.StoredProcedure);
                return aa.ToList();
            }
            //return this.dn.ctx.Query<Levy_Rate_Entity>("Report_Cost_Saving", new { Apt_Code, Repair_Plan_Code, Levy_Rate_Code, Levy_Rate, Levy_Period }, commandType: System.Data.CommandType.StoredProcedure).ToList();
        }

        /// <summary>
        /// 해당 단지 부과율에 따른 면적별 부과금액 정보 가져 오기
        /// </summary>
        public async Task<List<Levy_Rate_Entity>> GetList_Rate_Levy_Cost_New(string Apt_Code, string Repair_Plan_Code, string Levy_Rate_Code, double Levy_Rate, int Levy_Period)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aa = await ctx.QueryAsync<Levy_Rate_Entity>("Report_Cost_Saving_New", new { Apt_Code, Repair_Plan_Code, Levy_Rate_Code, Levy_Rate, Levy_Period }, commandType: System.Data.CommandType.StoredProcedure);
                return aa.ToList();
            }
            //return this.dn.ctx.Query<Levy_Rate_Entity>("Report_Cost_Saving_New", new { Apt_Code, Repair_Plan_Code, Levy_Rate_Code, Levy_Rate, Levy_Period }, commandType: System.Data.CommandType.StoredProcedure).ToList();
        }

        /// <summary>
        /// 관리규약 코드에 따른 입력된 부과율 수 구하기
        /// </summary>
        public async Task<int> Bylaw_Levy_Count(string Apt_Code, string Bylaw_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Levy_Rate Where Apt_Code = @Apt_Code And Bylaw_Code = @Bylaw_Code", new { Apt_Code, Bylaw_Code }, commandType: CommandType.Text);
            }
            //return this.dn.ctx.Query<int>("Select Count(*) From Levy_Rate Where Apt_Code = @Apt_Code And Bylaw_Code = @Bylaw_Code", new { Apt_Code, Bylaw_Code });
        }

        /// <summary>
        /// 해당 공동주택 부과율 정보 가져오기
        /// </summary>
        public async Task<Levy_Rate_Entity> GetDetail(string Levy_Rate_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Levy_Rate_Entity>("Select * From Levy_Rate Where Levy_Rate_Code = @Levy_Rate_Code", new { Levy_Rate_Code }, commandType: CommandType.Text);
            }
            //return this.dn.ctx.Query<Levy_Rate_Entity>("Select * From Levy_Rate Where Levy_Rate_Code = @Levy_Rate_Code", new { Levy_Rate_Code });
        }

        /// <summary>
        /// 적립율 존재 여부
        /// </summary>
        public async Task<int> Being_LevyRate(string Apt_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Levy_Rate Where Apt_Code = @Apt_Code", new { Apt_Code }, commandType: CommandType.Text);
            }
            //return this.dn.ctx.Query<int>("Select Count(*) From Levy_Rate Where Apt_Code = @Apt_Code", new { Apt_Code });
        }

        public async Task<string> Levy_Rate_Code_Search(string Apt_Code, string Bylaw_Code, string Num)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<string>("WITH Levy_Rate_Temp AS(Select ROW_NUMBER() OVER(ORDER BY Levy_Rate_Accumulate ASC) AS 'Num', Levy_Rate_Code From Levy_Rate Where Apt_Code = @Apt_Code And Bylaw_Code = @Bylaw_Code ) Select Levy_Rate_Code From Levy_Rate_Temp Where Num = @Num", new { Apt_Code, Bylaw_Code, Num }, commandType: CommandType.Text);
            }
            //return this.dn.ctx.Query<string>("WITH Levy_Rate_Temp AS(Select ROW_NUMBER() OVER(ORDER BY Levy_Rate_Accumulate ASC) AS 'Num', Levy_Rate_Code From Levy_Rate Where Apt_Code = @Apt_Code And Bylaw_Code = @Bylaw_Code ) Select Levy_Rate_Code From Levy_Rate_Temp Where Num = @Num", new { Apt_Code, Bylaw_Code, Num });
        }

        /// <summary>
        /// 부과율 코드로 전체 정보 불러오기
        /// </summary>
        /// <param name="Levy_Rate_Code"></param>
        /// <returns></returns>
        public async Task<Levy_Rate_Entity> Detail_Levy_Rate_Code(string Levy_Rate_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Levy_Rate_Entity>("Select * From Levy_Rate Where Levy_Rate_Code = @Levy_Rate_Code", new { Levy_Rate_Code }, commandType: CommandType.Text);
            }
            //return this.dn.ctx.Query<Levy_Rate_Entity>("Select * From Levy_Rate Where Levy_Rate_Code = @Levy_Rate_Code", new { Levy_Rate_Code });
        }
    }

    /// <summary>
    /// 장충금 초기 입력 값 클래스
    /// </summary>
    public class Repair_Capital_Lib : IRepair_Capital_Lib
    {
        private readonly IConfiguration _db;

        public Repair_Capital_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 장기수선충당금 초기 값 입력
        /// </summary>
        public async Task<Repair_Capital_Entity> Add_Repair_Capital(Repair_Capital_Entity ReC)
        {
            var sql = "Insert into Repair_Capital (Apt_Code, Balance_Capital, Levy_Capital, Use_Cost, Staff_Code, PostIP) Values (@Apt_Code, @Balance_Capital, @Levy_Capital, @Use_Cost, @Staff_Code, @PostIP)";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, ReC);
                return ReC;
            }
            //this.dn.ctx.Execute(sql, ReC);
            //return ReC;
        }

        // 장기수선충당금 초기 값 수정
        public async Task<Repair_Capital_Entity> Edit_Repair_Capital(Repair_Capital_Entity ReC)
        {
            var sql = "Update Repair_Capital Set Balance_Capital = @Balance_Capital, Levy_Capital = @Levy_Capital, Use_Cost = @Use_Cost, Staff_Code = @Staff_Code, PostIP = @PostIP Where Repair_Capital_Code = @Repair_Capital_Code";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, ReC);
                return ReC;
            }
            //this.dn.ctx.Execute(sql, ReC);
            //return ReC;
        }

        /// <summary>
        /// 장기수선충당금 초기값에서 잔액 불러오기
        /// </summary>
        public async Task<double> GetDetail_Balance_Capital(string Apt_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<double>("Select Top 1 Balance_Capital From Repair_Capital Where Apt_Code = @Apt_Code Order By Repair_Capital_Code Desc", new { Apt_Code }, commandType: CommandType.Text);
            }
            //return this.dn.ctx.Query<double>("Select Top 1 Balance_Capital From Repair_Capital Where Apt_Code = @Apt_Code Order By Repair_Capital_Code Desc", new { Apt_Code });
        }

        /// <summary>
        /// 장기수선충당금 초기값 존재 여부
        /// </summary>
        public async Task<int> Being_Capital(string Apt_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Repair_Capital Where Apt_Code = @Apt_Code", new { Apt_Code }, commandType: CommandType.Text);
            }
            //return this.dn.ctx.Query<int>("Select Count(*) From Repair_Capital Where Apt_Code = @Apt_Code", new { Apt_Code });
        }

        /// <summary>
        /// 초기화 입력된 년도 구하기
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<DateTime> Being_Capital_Date(string Apt_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<DateTime>("Select Top 1 isNull(PostDate, Getdate()) From Repair_Capital Where Apt_Code = @Apt_Code Order By Repair_Capital_Code Desc", new { Apt_Code }, commandType: CommandType.Text);
            }
            //return this.dn.ctx.Query<DateTime>("Select Top 1 isNull(PostDate, Getdate()) From Repair_Capital Where Apt_Code = @Apt_Code Order By Repair_Capital_Code Desc", new { Apt_Code });
        }

        /// <summary>
        /// 상세정보(잔액, 사용액, 부과액)
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<Repair_Capital_Entity> _detail(string Apt_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Repair_Capital_Entity>("Select Balance_Capital, Levy_Capital, Use_Cost From Repair_Capital Where Apt_Code = @Apt_Code", new { Apt_Code }, commandType: CommandType.Text);
            }
            //return this.dn.ctx.Query<Repair_Capital_Entity>("Select Balance_Capital, Levy_Capital, Use_Cost From Repair_Capital Where Apt_Code = @Apt_Code", new { Apt_Code });
        }

        /// <summary>
        ///  장기수선충당금 초기값 식별코드 불러오기
        /// </summary>
        public async Task<int> Being_Capital_Code(string Apt_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Top 1 Repair_Capital_Code From Repair_Capital Where Apt_Code = @Apt_Code Order By Repair_Capital_Code Desc", new { Apt_Code }, commandType: CommandType.Text);
            }
            //return this.dn.ctx.Query<int>("Select Top 1 Repair_Capital_Code From Repair_Capital Where Apt_Code = @Apt_Code Order By Repair_Capital_Code Desc", new { Apt_Code });
        }

        // 장기수선충당금 초기값에서 부과 총액 불러오기
        public async Task<double> GetDetail_Levy_Capital(string Apt_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<double>("Select Top 1 Levy_Capital From Repair_Capital Where Apt_Code = @Apt_Code Order By Repair_Capital_Code Desc", new { Apt_Code }, commandType: CommandType.Text);
            }
            //return this.dn.ctx.Query<double>("Select Top 1 Levy_Capital From Repair_Capital Where Apt_Code = @Apt_Code Order By Repair_Capital_Code Desc", new { Apt_Code });
        }

        /// <summary>
        /// 장기수선충당금 초기값에서 사용 총액 불러오기
        /// </summary>
        public async Task<double> GetDetail_Use_Cost(string Apt_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<double>("Select Top 1 isnull(Use_Cost, 0) From Repair_Capital Where Apt_Code = @Apt_Code Order By Repair_Capital_Code Desc", new { Apt_Code }, commandType: CommandType.Text);
            }
            //return this.dn.ctx.Query<double>("Select Top 1 isnull(Use_Cost, 0) From Repair_Capital Where Apt_Code = @Apt_Code Order By Repair_Capital_Code Desc", new { Apt_Code });
        }

        /// <summary>
        /// 잔액 만들기
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<double> BalanceSum(string Apt_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));

            double intBalanceSum = await db.QuerySingleOrDefaultAsync<double>("Select Top 1 IsNull(Balance_Capital, 0) From Repair_Capital Where Apt_Code = @Apt_Code Order By Repair_Capital_Code Desc", new { Apt_Code });//초기화 잔액 불러오기

            string strFirst_year = "2016";
            strFirst_year = ((await db.QuerySingleOrDefaultAsync<DateTime>("Select Top 1 isNull(PostDate, Getdate()) From Repair_Capital Where Apt_Code = @Apt_Code Order By Repair_Capital_Code Desc", new { Apt_Code })).Year - 1).ToString(); // 장기수선충당금 초기화 시에 입력된 기준년도 구하기;
            if (strFirst_year == "" || strFirst_year == "0")
            {
                strFirst_year = "2016";
            }
            double dbLavy_Capital = await db.QuerySingleOrDefaultAsync<double>("Select isNull(Sum(Levy_Month_Sum), 0) From Capital_Levy Where Apt_Code = @Apt_Code And Levy_Year > @strFirst_year", new { Apt_Code, strFirst_year });//초기화 이후 징수적립액

            strFirst_year = strFirst_year + "-12-31";
            double dbUseCost = await db.QuerySingleOrDefaultAsync<double>("Select isnull(Sum(Repair_Cost_Complete), 0) From Repair_Record Where Repair_End_Date > @strFirst_year And Apt_Code = @Apt_Code", new { Apt_Code, strFirst_year }); //해당 공동주택 장기수선충당금 사용금액 합계(초기화에서 입력 사용액 입력년도 이후 사용한 금액)

            dbUseCost = dbLavy_Capital + intBalanceSum - dbUseCost;

            return dbUseCost;
        }
    }

    /// <summary>
    /// 장기수선충당금 사용 클래스
    /// </summary>
    public class Repair_Capital_Use_Lib : IRepair_Capital_Use_Lib
    {
        private readonly IConfiguration _db;

        public Repair_Capital_Use_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 장기수선충당금 사용 입력
        /// </summary>
        public async Task<Repair_Capital_Use_Entity> Add_Capital_Use(Repair_Capital_Use_Entity RCU)
        {
            var sql = "Insert Into Repair_Capital_Use (Apt_Code, Repair_Capitail_Code, Repair_Plan_Code, Repair_Article_Code, Repair_Cycle_Code, Repair_Cost_Code, Use_Division, Plan_Cost, Use_Cost, Use_Date, Use_Year, Use_Month, Use_Day, Staff_Code, PostIP) Values (@Apt_Code, @Repair_Capitail_Code, @Repair_Plan_Code, @Repair_Article_Code, @Repair_Cycle_Code, @Repair_Cost_Code, @Use_Division, @Plan_Cost, @Use_Cost, @Use_Date, @Use_Year, @Use_Month, @Use_Day, @Staff_Code, @PostIP)";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, RCU);
                return RCU;
            }
            //this.dn.ctx.Execute(sql, RCU);
            //return RCU;
        }

        /// <summary>
        /// 장기수선충당금 사용 수정
        /// </summary>
        public async Task<Repair_Capital_Use_Entity> Edit_Capital_Use(Repair_Capital_Use_Entity RCU)
        {
            var sql = "Update Repair_Capital_Use Set Use_Division = @Use_Division, Plan_Cost = @Plan_Cost, Use_Cost = @Use_Cost,  Use_Date = @Use_Date, Use_Year = @Use_Year, Use_Month = @Use_Month, Use_Day = @Use_Day, PostDate = @PostDate, Staff_Code = @Staff_Code, PostIP = @PostIP Where Repair_Capital_Use_Code = @Repair_Capital_Use_Code";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, RCU);
                return RCU;
            }
            //this.dn.ctx.Execute(sql, RCU);
            //return RCU;
        }

        // 장기수선충당금 사용 삭제
        public async Task Remove_Capital_Use(int Repair_Capital_Use_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync("Delete From Repair_Capital_Use Where Repair_Capital_Use_Code = @Repair_Capital_Use_Code", new { Repair_Capital_Use_Code });
            }
            //this.dn.ctx.Execute("Delete From Repair_Capital_Use Where Repair_Capital_Use_Code = @Repair_Capital_Use_Code", new { Repair_Capital_Use_Code });
        }

        // 해당 공동주택 년도별 장기수선충당금 사용 정보
        public async Task<List<Repair_Capital_Use_Entity>> GetList_Capital_Use_Year(string Apt_Code, int Use_Year)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aa = await ctx.QueryAsync<Repair_Capital_Use_Entity>("Select * From Repair_Capital_Use Where Apt_Code = @Apt_Code And User_Yaer = @Use_Year Order By Repair_Capital_Use_Code Asc", new { Apt_Code, Use_Year });
                return aa.ToList();
            }
            //return this.dn.ctx.Query<Repair_Capital_Use_Entity>("Select * From Repair_Capital_Use Where Apt_Code = @Apt_Code And User_Yaer = @Use_Year Order By Repair_Capital_Use_Code Asc", new { Apt_Code, Use_Year }).ToList();
        }

        // 해당 공동주택 년도별 장기수선충당금 사용 정보
        public async Task<List<Repair_Capital_Use_Entity>> GetList_Capital_Use_Year_Division(string Apt_Code, int Use_Year, string Use_Division)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aa = await ctx.QueryAsync<Repair_Capital_Use_Entity>("Select * From Repair_Capital_Use Where Apt_Code = @Apt_Code And User_Yaer = @Use_Year And Use_Division = @Use_Division Order By Repair_Capital_Use_Code Asc", new { Apt_Code, Use_Year, Use_Division });
                return aa.ToList();
            }
            //return this.dn.ctx.Query<Repair_Capital_Use_Entity>("Select * From Repair_Capital_Use Where Apt_Code = @Apt_Code And User_Yaer = @Use_Year And Use_Division = @Use_Division Order By Repair_Capital_Use_Code Asc", new { Apt_Code, Use_Year, Use_Division }).ToList();
        }

        // 해당 공동주택 월별 장기수선충당금 사용 정보
        public async Task<List<Repair_Capital_Use_Entity>> GetList_Capital_Use_Month(string Apt_Code, int Use_Year, int Use_Month)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aa = await ctx.QueryAsync<Repair_Capital_Use_Entity>("Select * From Repair_Capital_Use Where Apt_Code = @Apt_Code And User_Yaer = @Use_Year And Use_Month = @Use_Month Order By Repair_Capital_Use_Code Asc", new { Apt_Code, Use_Year, Use_Month });
                return aa.ToList();
            }
            //return this.dn.ctx.Query<Repair_Capital_Use_Entity>("Select * From Repair_Capital_Use Where Apt_Code = @Apt_Code And User_Yaer = @Use_Year And Use_Month = @Use_Month Order By Repair_Capital_Use_Code Asc", new { Apt_Code, Use_Year, Use_Month }).ToList();
        }

        // 해당 공동주택 월별 장기수선충당금 사용 정보
        public async Task<List<Repair_Capital_Use_Entity>> GetList_Capital_Use_Month_Division(string Apt_Code, int Use_Year, int Use_Month, string Use_Division)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aa = await ctx.QueryAsync<Repair_Capital_Use_Entity>("Select * From Repair_Capital_Use Where Apt_Code = @Apt_Code And User_Yaer = @Use_Year And Use_Month = @Use_Month And Use_Division = @Use_Division Order By Repair_Capital_Use_Code Asc", new { Apt_Code, Use_Year, Use_Month, Use_Division });
                return aa.ToList();
            }
            //return this.dn.ctx.Query<Repair_Capital_Use_Entity>("Select * From Repair_Capital_Use Where Apt_Code = @Apt_Code And User_Yaer = @Use_Year And Use_Month = @Use_Month And Use_Division = @Use_Division Order By Repair_Capital_Use_Code Asc", new { Apt_Code, Use_Year, Use_Month, Use_Division }).ToList();
        }

        // 장기수선충당금 사용 정보 상세보기
        public async Task<Repair_Capital_Use_Entity> GetDetail(int Repair_Capital_Use_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Repair_Capital_Use_Entity>("Select * From Repair_Capital_Use Where Repair_Capital_Use_Code = @Repair_Capital_Use_Code", new { Repair_Capital_Use_Code }, commandType: CommandType.Text);
            }
            //return this.dn.ctx.Query<Repair_Capital_Use_Entity>("Select * From Repair_Capital_Use Where Repair_Capital_Use_Code = @Repair_Capital_Use_Code", new { Repair_Capital_Use_Code });
        }

        // 장기수선충당금 사용계획서 작성을 위한 리스트
        public async Task<List<Join_Article_Cycle_Cost_EntityA>> GetList_Using_Cost(string Apt_Code, string Repair_Plan_Code, string Repair_Plan_Year)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aa = await ctx.QueryAsync<Join_Article_Cycle_Cost_EntityA>("Select * From Repair_Cycle As a Join Repair_Cost As b on a.Apt_Code = b.Apt_Code And a.Repair_Article_Code = b.Repair_Article_Code Join Repair_Article As c on a.Apt_Code = c.Apt_Code And  a.Repair_Article_Code = c.Aid Where (a.Repair_Plan_Year_All = @Repair_Plan_Year or a.Repair_Plan_Year_Part = @Repair_Plan_Year) And a.Apt_Code = @Apt_Code And a.Repair_Plan_Code = @Repair_Plan_Code Order By a.Sort_A_Code, a.Sort_B_Code, a.Aid Asc", new { Apt_Code, Repair_Plan_Code, Repair_Plan_Year });
                return aa.ToList();
            }
            //return this.dn.ctx.Query<Join_Article_Cycle_Cost_Entity>("Select * From Repair_Cycle As a Join Repair_Cost As b on a.Apt_Code = b.Apt_Code And a.Repair_Article_Code = b.Repair_Article_Code Join Repair_Article As c on a.Apt_Code = c.Apt_Code And  a.Repair_Article_Code = c.Aid Where (a.Repair_Plan_Year_All = @Repair_Plan_Year or a.Repair_Plan_Year_Part = @Repair_Plan_Year) And a.Apt_Code = @Apt_Code And a.Repair_Plan_Code = @Repair_Plan_Code Order By a.Sort_A_Code, a.Sort_B_Code, a.Aid Asc", new { Apt_Code, Repair_Plan_Code, Repair_Plan_Year }).ToList();
        }

        /// <summary>
        /// 해당 공동주택의 장기수선충당금 사용한 총액
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<double> Using_Cost_TotalSum(string Apt_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<double>("Select isnull(Sum(Repair_Cost_Complete), 0) From Repair_Record Where Apt_Code = @Apt_Code", new { Apt_Code }, commandType: CommandType.Text);
            }
            //return dn.ctx.Query<double>("Select isnull(Sum(Repair_Cost_Complete), 0) From Repair_Record Where Apt_Code = @Apt_Code", new { Apt_Code });
        }

        /// <summary>
        /// 근년 실행 리스트
        /// </summary>
        public async Task<List<Join_Article_Cycle_Cost_EntityA>> GetList_Main_Plan(string Apt_Code, string Repair_Plan_Code, string Repair_Plan_Year)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aa = await ctx.QueryAsync<Join_Article_Cycle_Cost_EntityA>("Select Top 5 * From Repair_Cycle As a Join Repair_Cost As b on a.Apt_Code = b.Apt_Code And a.Repair_Article_Code = b.Repair_Article_Code Join Repair_Article As c on a.Apt_Code = c.Apt_Code And  a.Repair_Article_Code = c.Aid Where (a.Repair_Plan_Year_All = @Repair_Plan_Year or a.Repair_Plan_Year_Part = @Repair_Plan_Year) And a.Apt_Code = @Apt_Code And a.Repair_Plan_Code = @Repair_Plan_Code Order By a.Sort_A_Code, a.Sort_B_Code, a.Aid Asc", new { Apt_Code, Repair_Plan_Code, Repair_Plan_Year });
                return aa.ToList();
            }
        }
    }

    // 장기수선충당금 사용계획서 클래스
    public class Capital_Use_Plan_Lib
    {
        private readonly IConfiguration _db;

        public Capital_Use_Plan_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        public async Task<Capital_Use_Plan_Entity> Add(Capital_Use_Plan_Entity cp)
        {
            var sql = "Insert Into Capital_Use_Plan () Values ()";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, cp);
                return cp;
            }
        }
    }
}