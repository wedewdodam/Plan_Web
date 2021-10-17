using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Plan_Blazor_Lib.Record
{
    /// <summary>
    /// 장기수선충당금 사용 이력 정보 클래스
    /// </summary>
    public class Repair_Record_Lib : IRepair_Record_Lib
    {
        //private readonly SqlConnection _db;

        //public Repair_Record_Lib(string connectionString)
        //{
        //    _db = new SqlConnection(connectionString);
        //}

        private readonly IConfiguration _db;

        public Repair_Record_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 수선공사 이력 정보 입력 메서드
        /// </summary>
        /// <param name="_rr"></param>
        /// <returns></returns>
        public async Task<Repair_Record_Entity> Add(Repair_Record_Entity _rr)
        {
            var sql = "Insert Repair_Record (Apt_Code, Repair_Plan_Code, Sort_A_Code, Sort_A_Name, Sort_B_Code, Sort_B_Name, Sort_C_Code, Sort_C_Name, Repair_Article_Code, Repair_Article_Name, Repair_Division, Repair_Plan_Cost, Repair_contract_Cost, Repair_Cost_Complete, Repair_Start_Date, Repair_Year, Repair_Month, Repair_Day, Repair_End_Date, Repair_laver_Count, Company_Code, CorporRate_Number, Tender_Mothed, Tender_bid, Cost_Division, Charge_Man, ChargeMan_mobile, Work_Division, Repair_Record_Etc, Staff_Code, PostIP, Repair_End_Year) Values (@Apt_Code, @Repair_Plan_Code, @Sort_A_Code, @Sort_A_Name, @Sort_B_Code, @Sort_B_Name, @Sort_C_Code, @Sort_C_Name, @Repair_Article_Code, @Repair_Article_Name, @Repair_Division, @Repair_Plan_Cost, @Repair_contract_Cost, @Repair_Cost_Complete, @Repair_Start_Date, @Repair_Year, @Repair_Month, @Repair_Day, @Repair_End_Date, @Repair_laver_Count, @Company_Code, @CorporRate_Number, @Tender_Mothed, @Tender_bid, @Cost_Division, @Charge_Man, @ChargeMan_mobile, @Work_Division, @Repair_Record_Etc, @Staff_Code, @PostIP, @Repair_End_Year)";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, _rr, commandType: CommandType.Text);
                return _rr;
            }
        }

        /// <summary>
        /// 수선공사 이력 정보 수정
        /// </summary>
        /// <param name="_rr"></param>
        /// <returns></returns>
        public async Task<Repair_Record_Entity> Edit(Repair_Record_Entity _rr)
        {
            var sql = "Update Repair_Record Set Sort_A_Code = @Sort_A_Code, Sort_A_Name = @Sort_A_Name, Sort_B_Code =@Sort_B_Code, Sort_B_Name = @Sort_B_Name, Sort_C_Code = @Sort_C_Code, Sort_C_Name = @Sort_C_Name, Repair_Article_Code = @Repair_Article_Code, Repair_Article_Name = @Repair_Article_Name, Repair_Division = @Repair_Division, Repair_Plan_Cost = @Repair_Plan_Cost, Repair_contract_Cost = @Repair_contract_Cost, Repair_Cost_Complete = @Repair_Cost_Complete, Repair_Start_Date = @Repair_Start_Date, Repair_Year = @Repair_Year, Repair_Month = @Repair_Month, Repair_Day = @Repair_Day, Repair_End_Date = @Repair_End_Date, Repair_laver_Count = @Repair_laver_Count, Company_Code = @Company_Code, CorporRate_Number = @CorporRate_Number, Tender_Mothed = @Tender_Mothed, Tender_bid = @Tender_bid, Cost_Division = @Cost_Division, Charge_Man = @Charge_Man, ChargeMan_mobile = @ChargeMan_mobile, Work_Division = @Work_Division, Repair_Record_Etc = @Repair_Record_Etc, Staff_Code = @Staff_Code, PostDate = @PostDate, PostIP = @PostIP, Repair_End_Year=@Repair_End_Year Where Aid = @Aid";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, _rr, commandType: CommandType.Text);
                return _rr;
            }
        }

        /// <summary>
        /// 해당 공동주택 수선공사 목록(전체)
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<List<Repair_Record_Entity>> GetList(string Apt_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var list = await ctx.QueryAsync<Repair_Record_Entity>("Select * From Repair_Record Where Apt_Code = @Apt_Code Order By Aid Desc", new { Apt_Code });
                return list.ToList();
            }
        }

        /// <summary>
        /// 해당 공동주택 수선공사 수선 연간 목록
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<List<Repair_Record_Entity>> GetList_Year(string Apt_Code, string Repair_Year)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var lsit = await ctx.QueryAsync<Repair_Record_Entity>("Select * From Repair_Record Where Apt_Code = @Apt_Code And Repair_Year = @Repair_Year Order By Aid Desc", new { Apt_Code, Repair_Year }, commandType: CommandType.Text);
                return lsit.ToList();
            }
        }

        /// <summary>
        /// 해당 공동주택 수선공사 이력 목록 (현재년도 입력된 수선이력만 보여주기)
        /// </summary>
        public async Task<List<Repair_Record_Entity>> list_Year_List(string apt_Code, string PostStart, string PostEnd)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var Lsit = await ctx.QueryAsync<Repair_Record_Entity>("Repair_Record_List", new { apt_Code, PostStart, PostEnd }, commandType: CommandType.StoredProcedure);
                return Lsit.ToList();
            }
        }

        /// <summary>
        /// 해당 공동주택 수선공사 이력 목록 (현재년도 입력된 수선이력만 보여주기
        /// </summary>
        public async Task<List<Repair_Record_Entity>> list_Year_List_New(string apt_Code, string PostEnd)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var Lsit = await ctx.QueryAsync<Repair_Record_Entity>("Select a.Aid, a.Apt_Code, a.Repair_Plan_Code, a.Sort_A_Code, a.Sort_A_Name, a.Sort_B_Code, a.Sort_B_Name, a.Sort_C_Code, a.Sort_C_Name, a.Repair_Article_Code, a.Repair_Article_Name, a.Repair_Division, a.Repair_Plan_Cost, a.Repair_contract_Cost, a.Repair_Cost_Complete, a.Repair_Start_Date, a.Repair_Year, a.Repair_Month, a.Repair_Day, a.Repair_End_Date, a.Repair_laver_Count, a.Company_Code, a.CorporRate_Number, a.Tender_Mothed, a.Tender_bid, a.Cost_Division, a.Charge_Man,a. ChargeMan_mobile, a.Work_Division, a.Repair_Record_Etc, a.Staff_Code, a.PostDate, a.PostIP, b.Company_Name From Repair_Record as a Join Company as b on a.Company_Code = b.Company_Code Where a.Apt_Code = @Apt_Code And a.Repair_Year = @PostEnd Order By a.Repair_Month Desc", new { apt_Code, PostEnd });
                return Lsit.ToList();
            }
        }



        /// <summary>
        /// 해당 공동주택의 수선이력 리스트
        /// </summary>
        public async Task<List<Repair_Record_Entity>> list_Year_List_all(string apt_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var Lsit = await ctx.QueryAsync<Repair_Record_Entity>("Repair_Record_List_All", new { apt_Code }, commandType: CommandType.StoredProcedure);
                return Lsit.ToList();
            }
        }

        /// <summary>
        /// 해당 업체의 수선이력 리스트
        /// </summary>
        public async Task<List<Repair_Record_Entity>> List_Apt_all(string Company_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var Lst = await ctx.QueryAsync<Repair_Record_Entity>("Select a.Aid, a.Apt_Code, a.Repair_Plan_Code, a.Sort_A_Code, a.Sort_A_Name, a.Sort_B_Code, a.Sort_B_Name, a.Sort_C_Code, a.Sort_C_Name, a.Repair_Article_Code, a.Repair_Article_Name, a.Repair_Division, a.Repair_Plan_Cost, a.Repair_contract_Cost, a.Repair_Cost_Complete, a.Repair_Start_Date, a.Repair_Year, a.Repair_Month, a.Repair_Day, a.Repair_End_Date, a.Repair_laver_Count, a.Company_Code, a.CorporRate_Number, a.Tender_Mothed, a.Tender_bid, a.Cost_Division, a.Charge_Man, a.ChargeMan_mobile, a.Work_Division, a.Repair_Record_Etc, a.Staff_Code, a.PostDate, a.PostIP, a.Repair_End_Year, b.Apt_Name, b.Apt_Adress_sido From Repair_Record as a Join Apt as b On a.Apt_Code = b.Apt_Code Where a.Company_Code = @Company_Code Order By a.Repair_Start_Date Desc", new { Company_Code });
                return Lst.ToList();
            }
        }

        //Select* From Repair_Record Where Apt_Code = @Apt_Code Order By Repair_Start_Date Desc


        /// <summary>
        /// 수선이력에서 년간 사용 총액
        /// </summary>
        public async Task<double> Year_Sum(string apt_Code, string _Start)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<double>("Select isnull(Sum(Repair_Cost_Complete), 0) From Repair_Record Where Repair_End_Year = @_Start And Apt_Code = @apt_Code", new { apt_Code, _Start }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 해당 수선공사 식별코드로 상세보기
        /// </summary>
        public async Task<Repair_Record_Entity> Detail(int Aid)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Repair_Record_Entity>("Select * From Repair_Record Where Aid = @Aid", new { Aid }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 분류별 수선공사 목록
        /// </summary>
        /// <param name="Aid"></param>
        /// <returns></returns>
        public async Task<List<Repair_Record_Entity>> GetList_Sort(string Apt_Code, string Feild, string Query)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var Lsit = await ctx.QueryAsync<Repair_Record_Entity>("Select * From Repair_Record Where " + Feild + " = @Query And Apt_Code = @Apt_Code Order By Aid Desc", new { Apt_Code, Feild, Query }, commandType: CommandType.Text);
                return Lsit.ToList();
            }
        }

        /// <summary>
        /// 분류별 수선공사 목록 (년도로)
        /// </summary>
        public async Task<List<Repair_Record_Entity>> GetList_Sort_Year(string Apt_Code, string Feild, string Query, string Repair_Year)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var Lsit = await ctx.QueryAsync<Repair_Record_Entity>("Select * From Repair_Record Where " + Feild + " = @Query And Repair_Year = @Repair_Year And Apt_Code = @Apt_Code Order By Aid Desc", new { Apt_Code, Feild, Query, Repair_Year }, commandType: CommandType.Text);
                return Lsit.ToList();
            }
        }

        /// <summary>
        /// 년도별 장기수선 사용금액 합계 (기준년도 이전 사용액)
        /// </summary>
        public async Task<double> UseCost_Year(string Apt_Code, string Repair_Year)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<double>("Select isnull(Sum(Repair_Cost_Complete), 0) From Repair_Record Where Apt_Code = @Apt_Code And Repair_Year <= @Repair_Year", new { Apt_Code, Repair_Year }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 해당 공동주택 장기수선충당금 사용금액 합계(초기화에서 입력 사용액 입력년도 이후 사용한 금액)
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="Year_Use">초기화에서 입력한 사용액 기준년도 입력</param>
        /// <returns></returns>
        public async Task<double> UseCost(string Apt_Code, int Year_Use)
        {
            var sql = "Select isnull(Sum(Repair_Cost_Complete), 0) From Repair_Record Where Repair_Year >= @Year_Use And Apt_Code = @Apt_Code;";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aa = await ctx.QuerySingleOrDefaultAsync<double>(sql, new { Year_Use, Apt_Code });
                return aa;
            }
        }

        /// <summary>
        /// 해당 공동주택 장기수선충당금 사용금액 합계(년도별)
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<double> UseCost_Year_A(string Apt_Code, string Repair_Year)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<double>("Select isnull(Sum(Repair_Cost_Complete), 0) From Repair_Record Where Apt_Code = @Apt_Code And Repair_Year = @Repair_Year", new { Apt_Code, Repair_Year }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 년도별 장기수선 분류별 사용금액 합계
        /// </summary>
        public async Task<double> UseCost_Year_Between(string Apt_Code, string Feild, string Query, int Repair_Year_a, int Repair_Year_b)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<double>("Select isnull(Sum(Repair_Cost_Complete), 0) From Repair_Record Where Apt_Code = @Apt_Code And " + Feild + " = @Query And Repair_Year = @Repair_Year And (Repair_Year >= @Repair_Year_a And Repair_Year <= @Repair_Year_b", new { Apt_Code, Query, Repair_Year_a, Repair_Year_b }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 수선공사 이력 삭제
        /// </summary>
        public async Task Remove(int Aid)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync("Delete Repair_Record Where Aid = @Aid", new { Aid }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 사용한 총액(초기화 사용액 기준년도 이후 조정년도 이전년도 사이 사용액)
        /// </summary>
        public async Task<double> aUsing_Cost(string Apt_Code, string Start_year, string End_year)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<double>("Select isnull(Sum(Repair_Cost_Complete), 0) From Repair_Record Where Apt_Code = @Apt_Code and Repair_End_Date > @Start_year And Repair_End_Date < @End_year", new { Apt_Code, Start_year, End_year }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 장기수선충당금 사용 및 적립 현황의 잔액 구하기
        /// </summary>
        public async Task<Balance_Year_Entity> balance_Year(string Apt_Code, string Reset_Year)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Balance_Year_Entity>("dbBalance_Year", new { Apt_Code, Reset_Year }, commandType: CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// 초기화 년도 이후 부터 조정일 전까지 사용한 금액 구하기
        /// </summary>
        public async Task<double> Using_Cost(string Apt_Code, string Repair_Reset_Date, string Repair_End_Date)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QueryFirstOrDefaultAsync<double>("Repair_Using_Cost", new { Apt_Code, Repair_Reset_Date, Repair_End_Date }, commandType: CommandType.StoredProcedure);
            }
        }
    }

    /// <summary>
    /// 장기수선충당금 관련 정보 입력 완료 정보 클래스
    /// </summary>
    public class Appropriation_Prosess_Lib : IAppropriation_Prosess_Lib
    {
        private readonly SqlConnection _db;

        public Appropriation_Prosess_Lib(string connectionString)
        {
            _db = new SqlConnection(connectionString);
        }

        /// <summary>
        /// 장기수선충당금 관련 정보 입력 함수
        /// </summary>
        /// <param name="_Entity"></param>
        /// <returns></returns>
        public async Task<Appropriation_Prosess_Entity> Add(Appropriation_Prosess_Entity _Entity)
        {
            var sql = "Insert Appropriation_Prosess (Apt_Code, Identifition_Code, Identifition_Name, Complete, Complete_date, Complete_year, PostDate, PostIP, Etc) Values (@Apt_Code, @Identifition_Code, @Identifition_Name, @Complete, @Complete_date, @Complete_year, @PostDate, @PostIP, @Etc)";
            await _db.ExecuteAsync(sql, _Entity, commandType: CommandType.Text);
            return _Entity;
        }

        /// <summary>
        /// 장기수선충당금 관련 정보 입력 함수
        /// </summary>
        public async Task<Appropriation_Prosess_Entity> Edit(Appropriation_Prosess_Entity _Entity)
        {
            var sql = "Update Appropriation_prosess Set Complete, Complete_date, Complete_year, PostDate, PostIP, Etc Where Apt_Code = @Apt_Code, Identifition_Code = @Identifition_Code And Complete_year = @Complete_year";
            await _db.ExecuteAsync(sql, _Entity);
            return _Entity;
        }

        /// <summary>
        /// 장기수선충당금 관련 정보 완료를 수정
        /// </summary>
        public async Task Edit_Complete(string Apt_Code, string Identifition_Name, string Complete)
        {
            var sql = "Update Appropriation_prosess Set Complete = @Complete Where Apt_Code = @Apt_Code And Identifition_Name = @Identifition_Name";
            await _db.ExecuteAsync(sql, new { Apt_Code, Identifition_Name, Complete }, commandType: CommandType.Text);
        }

        /// <summary>
        /// 장기수선충당금(기타) 관련 정보 완료를 수정
        /// </summary>
        /// <param name="_Entity"></param>
        /// <returns></returns>
        public async Task Edit_Complete_year(string Apt_Code, string Identifition_Name, string Complete_year, string Complete)
        {
            var sql = "Update Appropriation_prosess Set Complete = @Complete Where Apt_Code = @Apt_Code And Identifition_Name = @Identifition_Name And Complete_year = @Complete_year";
            await _db.ExecuteAsync(sql, new { Apt_Code, Identifition_Name, Complete_year, Complete }, commandType: CommandType.Text);
        }

        /// <summary>
        /// 입력된 해당 장기수선충당금(기타) 정보 존재 여부 확인
        /// </summary>
        public async Task<int> being_year(string Apt_Code, string Identifition_Name, string Complete_year)
        {
            return await _db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Appropriation_prosess Where Apt_Code = @Apt_Code And Identifition_Name = @Identifition_Name And Complete_year = @Complete_year", new { Apt_Code, Identifition_Name, Complete_year }, commandType: CommandType.Text);
        }

        /// <summary>
        /// 장기수선충당금 초기화 정보 존재 여부 확인
        /// </summary>
        public async Task<int> being(string Apt_Code, string Identifition_Name)
        {
            return await _db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Appropriation_prosess Where Apt_Code = @Apt_Code And Identifition_Name = @Identifition_Name", new { Apt_Code, Identifition_Name }, commandType: CommandType.Text);
        }

        /// <summary>
        /// 장기수선충당금 기준년도 구하기
        /// </summary>
        public async Task<string> _Year(string Apt_Code, string Identifition_Name)
        {
            return await _db.QuerySingleOrDefaultAsync<string>("Select Top 1 Complete_Year From Appropriation_prosess Where Apt_Code = @Apt_Code And Identifition_Name = @Identifition_Name Order By Aid Desc", new { Apt_Code, Identifition_Name }, commandType: CommandType.Text);
        }

        /// <summary>
        /// 장기수선충당금(기타) 기준년도 구하기 (년도)
        /// </summary>
        public async Task<string> year_Year(string Apt_Code, string Identifition_Name, string Complete_year)
        {
            return await _db.QuerySingleOrDefaultAsync<string>("Select Top 1 Complete_Year From Appropriation_prosess Where Apt_Code = @Apt_Code And Identifition_Name = @Identifition_Name And Complete_year = @Complete_year Order By Aid Desc", new { Apt_Code, Identifition_Name, Complete_year }, commandType: CommandType.Text);
        }

        /// <summary>
        /// 장기수선충당금 초기화 완료 정보 구하기
        /// </summary>
        public async Task<string> Complete(string Apt_Code, string Identifition_Name)
        {
            return await _db.QuerySingleOrDefaultAsync<string>("Select Top 1 Complete From Appropriation_prosess Where Apt_Code = @Apt_Code And Identifition_Name = @Identifition_Name Order By Aid Desc", new { Apt_Code, Identifition_Name }, commandType: CommandType.Text);
        }

        /// <summary>
        /// 장기수선충당금(기타) 초기화 완료 정보 구하기
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="Identifition_Name"></param>
        /// <returns></returns>
        public async Task<string> Complete_year(string Apt_Code, string Identifition_Name, string Complete_year)
        {
            return await _db.QuerySingleOrDefaultAsync<string>("Select Top 1 Complete From Appropriation_prosess Where Apt_Code = @Apt_Code And Identifition_Name = @Identifition_Name And Complete_year = @Complete_year Order By Aid Desc", new { Apt_Code, Identifition_Name, Complete_year }, commandType: CommandType.Text);
        }
    }
}