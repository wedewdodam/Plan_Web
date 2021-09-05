using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Plan_Blazor_Lib.Plan
{
    // 장기수선계획 기분 정보 메서드
    public class Repair_Plan_Lib : IRepair_Plan_Lib
    {
        private readonly IConfiguration _db;

        public Repair_Plan_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        ///  장기수선계획 기본 입력
        /// </summary>
        /// <param name="Plan">장기수선계획 기초 정보 엔터티</param>
        /// 2017-01-10
        public async Task<Repair_Plan_Entity> Add_Repair_Plan(Repair_Plan_Entity Plan)
        {
            var sql = "Insert Repair_Plan (Repair_Plan_Code, Apt_Code, Plan_Review_Code, Plan_Review_Date, Adjustment_Date, Adjustment_Year, Adjustment_Month, Founding_Date, Founding_Man, Adjustment_Division, Plan_Period, LastAdjustment_Date, Adjustment_Man, Adjustment_Num, Plan_Review_Date, SmallSum_Unit, Small_Sum, SmallSum_Requirement, SmallSum_Basis, Emergency_Basis, Repair_Plan_Etc, PostIP) Values (@Repair_Plan_Code, @Apt_Code, @Plan_Review_Code, @Plan_Review_Date, @Adjustment_Date, @Adjustment_Year, @Adjustment_Month, @Founding_Date, @Founding_Man, @Adjustment_Division, @Plan_Period, @LastAdjustment_Date, @Adjustment_Man, @Adjustment_Num, @Plan_Review_Date, @SmallSum_Unit, @Small_Sum, @SmallSum_Requirement, @SmallSum_Basis, @Emergency_Basis, @Repair_Plan_Etc, @PostIP);";

            using (var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                db.Open();
                var result = await db.ExecuteScalarAsync<int>(sql, Plan);
                Plan.Aid = result;
                return Plan;
            }
            //this.ctx.Execute(sql, Plan);
            //return Plan;
        }

        /// <summary>
        ///  장기수선계획 간편 수시조정 기본 입력
        /// </summary>
        /// 2021-09-19
        public async Task<int> Add_Repair_Plan_Any(Repair_Plan_Entity Plan)
        {
            var sql = "Insert Repair_Plan (Repair_Plan_Code, Apt_Code, Plan_Review_Code, Adjustment_Date, Adjustment_Year, Adjustment_Month, Founding_Date, Founding_Man, Adjustment_Division, Plan_Period, LastAdjustment_Date, Adjustment_Man, Adjustment_Num, Plan_Review_Date, SmallSum_Unit, Small_Sum, SmallSum_Requirement, SmallSum_Basis, Emergency_Basis, Repair_Plan_Etc, AnyTime, PostIP) Values (@Repair_Plan_Code, @Apt_Code, @Plan_Review_Code, @Adjustment_Date, @Adjustment_Year, @Adjustment_Month, @Founding_Date, @Founding_Man, @Adjustment_Division, @Plan_Period, @LastAdjustment_Date, @Adjustment_Man, @Adjustment_Num, @Plan_Review_Date, @SmallSum_Unit, @Small_Sum, @SmallSum_Requirement, @SmallSum_Basis, @Emergency_Basis, @Repair_Plan_Etc, 'B', @PostIP); Select Cast(SCOPE_IDENTITY() As Int);";

            using (var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                //db.Open();
                Plan.Aid = await db.QuerySingleOrDefaultAsync<int>(sql, Plan);
                //Plan.Aid = result;
                return Plan.Aid;
            }
        }

        //장기수선계획 기본 입력_ 입력 확인
        public async Task<int> Add_Repair_Plan_Add(Repair_Plan_Entity model)
        {
            var sql = "Insert Into Repair_Plan (Repair_Plan_Code, Apt_Code, Plan_Review_Code, Adjustment_Date, Adjustment_Year, Adjustment_Month, Founding_Date, Founding_Man, Adjustment_Division, Plan_Period, LastAdjustment_Date, Adjustment_Man, Adjustment_Num, Plan_Review_Date, SmallSum_Unit, Small_Sum, SmallSum_Requirement, SmallSum_Basis, Emergency_Basis, Repair_Plan_Etc, PostIP) Values (@Repair_Plan_Code, @Apt_Code, @Plan_Review_Code, @Adjustment_Date, @Adjustment_Year, @Adjustment_Month, @Founding_Date, @Founding_Man, @Adjustment_Division, @Plan_Period, @LastAdjustment_Date, @Adjustment_Man, @Adjustment_Num, @Plan_Review_Date, @SmallSum_Unit, @Small_Sum, @SmallSum_Requirement, @SmallSum_Basis, @Emergency_Basis, @Repair_Plan_Etc, @PostIP); Select Cast(SCOPE_IDENTITY() As Int);";

            using (var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                db.Open();
                int a = await db.QuerySingleOrDefaultAsync<int>(sql, model);
                return a;
            }
            //this.ctx.Execute(sql, Plan);
            //var id = this.ctx.Query<int>(sql, Plan).Single();
            //Plan.Aid = id;
            //return Plan;
        }

        // 장기수선계획 기본 정보 수정하기
        public async Task<Repair_Plan_Entity> Edit_Repair_Plan(Repair_Plan_Entity model)
        {
            var sql = "Update Repair_Plan Set Plan_Review_Code = @Plan_Review_Code, Plan_Review_Date = @Plan_Review_Date, Adjustment_Date = @Adjustment_Date, Adjustment_Year = @Adjustment_Year, Adjustment_Month = @Adjustment_Month, Founding_Date = @Founding_Date, Founding_Man = @Founding_Man, Adjustment_Division =@Adjustment_Division, Plan_Period = @Plan_Period, LastAdjustment_Date = @LastAdjustment_Date, Adjustment_Man = @Adjustment_Man, Adjustment_Num = @Adjustment_Num, SmallSum_Unit = @SmallSum_Unit, Small_Sum = @Small_Sum, SmallSum_Basis = @SmallSum_Basis, Emergency_Basis = @Emergency_Basis, Repair_Plan_Etc = @Repair_Plan_Etc, User_ID = @User_ID, PostIP = @PostIP Where Aid = @Aid;";

            using (var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                db.Open();
                var a = await db.ExecuteAsync(sql, model);
                return model;
            }

            //this.ctx.Execute(sql, Plan);
            //return Plan;
        }

        /// <summary>
        /// 계획완료 승인
        /// </summary>
        /// 2017-03-09
        public async Task Edit_Complete(string Repair_Plan_Code, string Complete)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                db.Open();
                await db.ExecuteAsync("Update Repair_Plan Set Complete = @Complete Where Repair_Plan_Code = @Repair_Plan_Code", new { Repair_Plan_Code, Complete }, commandType: CommandType.Text);
            }
            //this.ctx.Execute("Update Repair_Plan Set Complete = @Complete Where Repair_Plan_Code = @Repair_Plan_Code", new { Repair_Plan_Code, Complete });
        }

        /// <summary>
        /// 장기수선계획 기본 정보 최초 장기수선계획 수립 코드
        /// </summary>
        public async Task<string> Last_Code(string Apt_Code)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                db.Open();
                return await db.QueryFirstOrDefaultAsync<string>("Select Top 1 Repair_Plan_Code From Repair_Plan Where Apt_Code = @Apt_Code Order by Aid Asc", new { Apt_Code }, commandType: CommandType.Text);
            }
            //this.ctx.Query<string>("Select Top 1 Repair_Plan_Code From Repair_Plan Where Apt_Code = @Apt_Code Order by Aid Asc", new { Apt_Code });
        }

        /// <summary>
        ///  장기수선계획 기본 정보 마지막 일련번호 얻기
        /// </summary>
        public async Task<int> Last_Number()
        {
            using (var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                db.Open();
                return await db.QueryFirstOrDefaultAsync<int>("Select Top 1 Aid From Repair_Plan Order by Aid Desc", commandType: CommandType.Text);
            }
            //return this.ctx.Query<int>("Select Top 1 Aid From Repair_Plan Order by Aid Desc", new { });
        }

        // 장기수선계획 기본 정보 마지막 일련번호 얻기
        public async Task<int> Last_Aid_Apt(string Apt_Code)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                db.Open();
                return await db.QueryFirstOrDefaultAsync<int>("Select Top 1 Aid From Repair_Plan Where Apt_Code = @Apt_Code Order by Aid Desc", new { Apt_Code }, commandType: CommandType.Text);
            }
            //return this.ctx.Query<int>("Select Top 1 Aid From Repair_Plan Where Apt_Code = @Apt_Code Order by Aid Desc", new { Apt_Code });
        }

        /// <summary>
        /// 미지막 일련번호
        /// </summary>
        public async Task<int> Last_Aid()
        {
            using (var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                db.Open();
                return await db.QueryFirstOrDefaultAsync<int>("Select Top 1 Aid From Repair_Plan Order by Aid Desc", commandType: CommandType.Text);
            }
            
        }

        /// <summary>
        ///  최초 수립 장기수선계획 코드 찾기
        /// </summary>
        public async Task<int> New_Code(string Apt_Code)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                db.Open();
                return await db.QueryFirstOrDefaultAsync<int>("select Top 1 Aid From Repair_Plan Where Apt_Code = @Apt_Code Order By Aid Asc", new { Apt_Code }, commandType: CommandType.Text);
            }
            
        }

        /// <summary>
        ///  최초 수립 장기수선계획 코드 찾기
        /// </summary>
        public async Task<int> Plan_Num_Last(string Apt_Code)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                db.Open();
                return await db.QueryFirstOrDefaultAsync<int>("select Top 1 Adjustment_Num From Repair_Plan Where Apt_Code = @Apt_Code And AnyTime = 'A' Order By Aid Desc", new { Apt_Code }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        ///  마지막 완료된 장기수선계획 코드 찾기(완료된)
        /// </summary>
        public async Task<string> Last_Apt_Code(string Apt_Code)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                db.Open();
                return await db.QueryFirstOrDefaultAsync<string>("Select Top 1 Repair_Plan_Code From Repair_Plan Where Complete = 'B' And AnyTime = 'A' And Apt_Code = @Apt_Code Order By Aid Desc", new { Apt_Code }, commandType: CommandType.Text);
            }
            //return this.ctx.Query<string>("Select Top 1 Repair_Plan_Code From Repair_Plan Where Complete = 'B' And Apt_Code = @Apt_Code Order By Aid Desc", new { Apt_Code });
        }

        /// <summary>
        ///  마지막 장기수선계획 코드 찾기(가장 최근 장기수선계획코드)
        /// </summary>
        public async Task<string> New_Apt_Code(string Apt_Code)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                db.Open();
                return await db.QueryFirstOrDefaultAsync<string>("Select Top 1 Repair_Plan_Code From Repair_Plan Where Apt_Code = @Apt_Code And AnyTime = 'A' Order By Aid Desc", new { Apt_Code }, commandType: CommandType.Text);
            }
            //return this.ctx.Query<string>("Select Top 1 Repair_Plan_Code From Repair_Plan Where Apt_Code = @Apt_Code Order By Aid Desc", new { Apt_Code });
        }

        /// <summary>
        /// 해당 공동주택에 등록된 장기수선계획 정보 수
        /// </summary>
        /// <param name="Apt_Code">공동주택 식별코드</param>
        /// <returns>정보 수</returns>
        public async Task<int> Last_Number_Apt(string Apt_Code)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                db.Open();
                return await db.QueryFirstOrDefaultAsync<int>("Select Count(*) From Repair_Plan Where Apt_Code = @Apt_Code And AnyTime = 'A'", new { Apt_Code }, commandType: CommandType.Text);
            }
            //return this.ctx.Query<int>("Select Count(*) From Repair_Plan Where Apt_Code = @Apt_Code", new { Apt_Code });
        }

        /// <summary>
        /// 해당 공동주택에 등록된 장기수선계획 존재 여부(정기조정이고 완성인 경우)
        /// </summary>
        /// <param name="Apt_Code">공동주택 식별코드</param>
        /// <returns>정보 수</returns>
        public async Task<int> Regular_Number_Apt(string Apt_Code)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                db.Open();
                return await db.QueryFirstOrDefaultAsync<int>("Select Count(*) From Repair_Plan Where Apt_Code = @Apt_Code And Complete = 'B' And (Adjustment_Division = '정기조정' or Adjustment_Division = '최초수립' or Adjustment_Division = '최초입력' or Adjustment_Division = '최초입력' )", new { Apt_Code }, commandType: CommandType.Text);
            }
            //return this.ctx.Query<int>("Select Count(*) From Repair_Plan Where Apt_Code = @Apt_Code And Complete = 'B' And (Adjustment_Division = '정기조정' or Adjustment_Division = '최초수립' or Adjustment_Division = '최초입력' or Adjustment_Division = '최초입력' )", new { Apt_Code });
        }

        /// <summary>
        /// 해당 공동주택에 등록된 장기수선계획 코드(정기조정이고 완성인 경우)
        /// </summary>
        /// <param name="Apt_Code">공동주택 식별코드</param>
        /// <returns>정보 수</returns>
        public async Task<string> Regular_Code_Apt(string Apt_Code)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                db.Open();
                return await db.QueryFirstOrDefaultAsync<string>("Select Top 1 Repair_Plan_Code From Repair_Plan Where Apt_Code = @Apt_Code And Complete = 'B' And (Adjustment_Division = '정기조정' or Adjustment_Division = '최초수립' or Adjustment_Division = '최초입력') Order By Aid Desc", new { Apt_Code }, commandType: CommandType.Text);
            }
            //return this.ctx.Query<string>("Select Top 1 Repair_Plan_Code From Repair_Plan Where Apt_Code = @Apt_Code And Complete = 'B' And (Adjustment_Division = '정기조정' or Adjustment_Division = '최초수립' or Adjustment_Division = '최초입력') Order By Aid Desc", new { Apt_Code });
        }

        // 장기수선계획 기본 정보 중복 확인
        public async Task<int> Repeat_Number(string Repair_Plan_Code)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                db.Open();
                return await db.QueryFirstOrDefaultAsync<int>("Select Count(*) From Repair_Plan Where Repair_Plan_Code = @Repair_Plan_Code", new { Repair_Plan_Code }, commandType: CommandType.Text);
            }
            //return this.ctx.Query<int>("Select Count(*) From Repair_Plan Where Repair_Plan_Code = @Repair_Plan_Code", new { Repair_Plan_Code });
        }

        /// <summary>
        /// 장기수선계획 기본 정보 코드로 상세 불러오기
        /// </summary>
        public async Task<Repair_Plan_Entity> Detail_Repair_Plan(string Apt_Code, string Repair_Plan_Code)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                db.Open();
                return await db.QueryFirstOrDefaultAsync<Repair_Plan_Entity>("Select Aid, Repair_Plan_Code, Apt_Code, Plan_Review_Code, Plan_Review_Date, Adjustment_Date, Adjustment_Year, Adjustment_Month, Founding_Date, Founding_Man, Adjustment_Division, Plan_Period, LastAdjustment_Date, Adjustment_Man, Adjustment_Num, SmallSum_Unit, Small_Sum, SmallSum_Requirement, SmallSum_Basis, Emergency_Basis, Repair_Plan_Etc, User_ID, PostDate, PostIP, Complete From Repair_Plan Where Apt_Code = @Apt_Code And Repair_Plan_Code = @Repair_Plan_Code", new { Apt_Code, Repair_Plan_Code }, commandType: CommandType.Text);
            }
            
        }

        // 해당 공동주택 마지막 장기수선계획 상세 불러오기
        public async Task<Repair_Plan_Entity> Detail_Apt_Last_Plan(string Apt_Code)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                db.Open();
                return await db.QueryFirstOrDefaultAsync<Repair_Plan_Entity>("Select Top 1 Aid, Repair_Plan_Code, Apt_Code, Plan_Review_Code, Plan_Review_Date, Adjustment_Date, Adjustment_Year, Adjustment_Month, Founding_Date, Founding_Man, Adjustment_Division, Plan_Period, LastAdjustment_Date, Adjustment_Man, Adjustment_Num, SmallSum_Unit, Small_Sum, SmallSum_Requirement, SmallSum_Basis, Emergency_Basis, Repair_Plan_Etc, User_ID, PostDate, PostIP, Complete From Repair_Plan Where Apt_Code = @Apt_Code And AnyTime = 'A' Order By Aid Desc", new { Apt_Code }, commandType: CommandType.Text);
            }
            //return this.ctx.Query<Repair_Plan_Entity>("Select Top 1 Aid, Repair_Plan_Code, Apt_Code, Plan_Review_Code, Plan_Review_Date, Adjustment_Date, Adjustment_Year, Adjustment_Month, Founding_Date, Founding_Man, Adjustment_Division, Plan_Period, LastAdjustment_Date, Adjustment_Man, Adjustment_Num, SmallSum_Unit, Small_Sum, SmallSum_Requirement, SmallSum_Basis, Emergency_Basis, Repair_Plan_Etc, User_ID, PostDate, PostIP, Complete From Repair_Plan Where Apt_Code = @Apt_Code Order By Aid Desc", new { Apt_Code });
        }

        // 해당 공동주택 마지막 장기수선계획 상세 불러오기(정기검토에 의한 정기조정인 경우)
        public async Task<Repair_Plan_Entity> Detail_Apt_Regular_Plan(string Apt_Code)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                db.Open();
                return await db.QueryFirstOrDefaultAsync<Repair_Plan_Entity>("Select Top 1 Aid, Repair_Plan_Code, Apt_Code, Plan_Review_Code, Plan_Review_Date, Adjustment_Date, Adjustment_Year, Adjustment_Month, Founding_Date, Founding_Man, Adjustment_Division, Plan_Period, LastAdjustment_Date, Adjustment_Man, Adjustment_Num, SmallSum_Unit, Small_Sum, SmallSum_Requirement, SmallSum_Basis, Emergency_Basis, Repair_Plan_Etc, User_ID, PostDate, PostIP, Complete From Repair_Plan Where Apt_Code = @Apt_Code And Complete = 'B' And (Adjustment_Division = '정기조정' or Adjustment_Division = '최초수립' or Adjustment_Division = '최초입력') Order By Aid Desc", new { Apt_Code }, commandType: CommandType.Text);
            }
            //return this.ctx.Query<Repair_Plan_Entity>("Select Top 1 Aid, Repair_Plan_Code, Apt_Code, Plan_Review_Code, Plan_Review_Date, Adjustment_Date, Adjustment_Year, Adjustment_Month, Founding_Date, Founding_Man, Adjustment_Division, Plan_Period, LastAdjustment_Date, Adjustment_Man, Adjustment_Num, SmallSum_Unit, Small_Sum, SmallSum_Requirement, SmallSum_Basis, Emergency_Basis, Repair_Plan_Etc, User_ID, PostDate, PostIP, Complete From Repair_Plan Where Apt_Code = @Apt_Code And Complete = 'B' And (Adjustment_Division = '정기조정' or Adjustment_Division = '최초수립' or Adjustment_Division = '최초입력') Order By Aid Desc", new { Apt_Code });
        }

        // 해당 공동주택 마지막 장기수선계획 상세 불러오기(정기검토에 의한 정기조정인 경우)
        public async Task<Repair_Plan_Entity> Detail_Apt_Ferst_Plan(string Apt_Code)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                db.Open();
                return await db.QueryFirstOrDefaultAsync<Repair_Plan_Entity>("Select Top 1 Aid, Repair_Plan_Code, Apt_Code, Plan_Review_Code, Plan_Review_Date, Adjustment_Date, Adjustment_Year, Adjustment_Month, Founding_Date, Founding_Man, Adjustment_Division, Plan_Period, LastAdjustment_Date, Adjustment_Man, Adjustment_Num, SmallSum_Unit, Small_Sum, SmallSum_Requirement, SmallSum_Basis, Emergency_Basis, Repair_Plan_Etc, User_ID, PostDate, PostIP, Complete From Repair_Plan Where Apt_Code = @Apt_Code And AnyTime = 'A' Order By Aid Asc", new { Apt_Code }, commandType: CommandType.Text);
            }
        }

        // 식별코드로 장기수선계획 상세 불러오기
        public async Task<Repair_Plan_Entity> Detail_RepairPlan_Aid(int Aid)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                db.Open();
                return await db.QueryFirstOrDefaultAsync<Repair_Plan_Entity>("Select Aid, Repair_Plan_Code, Apt_Code, Plan_Review_Code, Plan_Review_Date, Adjustment_Date, Adjustment_Year, Adjustment_Month, Founding_Date, Founding_Man, Adjustment_Division, Plan_Period, LastAdjustment_Date, Adjustment_Man, Adjustment_Num, SmallSum_Unit, Small_Sum, SmallSum_Requirement, SmallSum_Basis, Emergency_Basis, Repair_Plan_Etc, User_ID, PostDate, PostIP, Complete From Repair_Plan Where Aid = @Aid", new { Aid }, commandType: CommandType.Text);
            }
            //return this.ctx.Query<Repair_Plan_Entity>("Select Aid, Repair_Plan_Code, Apt_Code, Plan_Review_Code, Plan_Review_Date, Adjustment_Date, Adjustment_Year, Adjustment_Month, Founding_Date, Founding_Man, Adjustment_Division, Plan_Period, LastAdjustment_Date, Adjustment_Man, Adjustment_Num, SmallSum_Unit, Small_Sum, SmallSum_Requirement, SmallSum_Basis, Emergency_Basis, Repair_Plan_Etc, User_ID, PostDate, PostIP, Complete From Repair_Plan Where Aid = @Aid", new { Aid });
        }

        /// 장기수선계획 기본정보 삭제
        public async Task Remove_AptCompany(int Aid)
        {
            using (var con = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                con.Open();
                await con.ExecuteAsync("Delete From Repair_Plan Where Aid = @Aid", new { Aid }, commandType: CommandType.Text);
            }
            //this.ctx.Execute("Delete From Repair_Plan Where Aid = @Aid", new { Aid });
        }

        /// <summary>
        /// 장기수선계획 식별코드로 입력된 장기수선계획 정보 삭제
        /// </summary>
        /// <param name="Repair_Plan_Code">장기수선계획 식별코드</param>
        /// 2017
        public async Task Delete_RepairPlan_PlanCode(string Repair_Plan_Code)
        {
            using (var con = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                con.Open();
                await con.ExecuteAsync("Delete From Repair_Plan Where Repair_Plan_Code = @Repair_Plan_Code", new { Repair_Plan_Code }, commandType: CommandType.Text);
            }
            //this.ctx.Execute("Delete From Repair_Plan Where Repair_Plan_Code = @Repair_Plan_Code", new { Repair_Plan_Code });
        }

        // 장기수선계획 기본 정보 해당 단지별 정보 목록 불러오기
        public async Task<List<Repair_Plan_Entity>> GetList_Repair_Plan_Apt(int Page, string Apt_Code)
        {
            using (var con = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                con.Open();
                var result = await con.QueryAsync<Repair_Plan_Entity>("Select Top 15 Aid, Repair_Plan_Code, Apt_Code, Plan_Review_Code, Plan_Review_Date, Adjustment_Date, Adjustment_Year, Adjustment_Month, Founding_Date, Founding_Man, Adjustment_Division, Plan_Period, LastAdjustment_Date, Adjustment_Man, Adjustment_Num, SmallSum_Unit, Small_Sum, SmallSum_Requirement, SmallSum_Basis, Emergency_Basis, Repair_Plan_Etc, User_ID, Complete From Repair_Plan Where Aid Not In (Select Top (15 * @Page) Aid From Repair_Plan Where Apt_Code = @Apt_Code And AnyTime = 'A' Order By Aid Desc) And Apt_Code = @Apt_Code And AnyTime = 'A' Order By Aid Desc", new {Page, Apt_Code }, commandType: CommandType.Text);
                return result.ToList();
            }            
        }

        /// <summary>
        /// 장기수선계획 기본 정보 해당 단지별 정보 목록 불러오기
        /// </summary>
        public async Task<int> GetList_Repair_Plan_Apt_Count(string Apt_Code)
        {
            using (var con = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                con.Open();
                return await con.QuerySingleOrDefaultAsync<int>("Select Count(*) From Repair_Plan Where Apt_Code = @Apt_Code And AnyTime = 'A'", new { Apt_Code }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 장기수선계획 기본 정보 해당 단지별 정보 목록 불러오기
        /// </summary>
        public async Task<List<Repair_Plan_Entity>> GetList_Repair_Plan_Apt_Any(int Page, string Apt_Code)
        {
            using (var con = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                con.Open();
                var result = await con.QueryAsync<Repair_Plan_Entity>("Select Top 10 Aid, Repair_Plan_Code, Apt_Code, Plan_Review_Code, Plan_Review_Date, Adjustment_Date, Adjustment_Year, Adjustment_Month, Founding_Date, Founding_Man, Adjustment_Division, Plan_Period, LastAdjustment_Date, Adjustment_Man, Adjustment_Num, SmallSum_Unit, Small_Sum, SmallSum_Requirement, SmallSum_Basis, Emergency_Basis, Repair_Plan_Etc, User_ID, Complete From Repair_Plan Where Aid Not In (Select Top (10 * @Page) Aid From Repair_Plan Where Apt_Code = @Apt_Code And AnyTime = 'B' Order By Aid Desc) And Apt_Code = @Apt_Code And AnyTime = 'B' Order By Aid Desc", new { Page, Apt_Code }, commandType: CommandType.Text);
                return result.ToList();
            }
        }

        /// <summary>
        /// 장기수선계획 기본 정보 해당 단지별 정보 목록 불러오기
        /// </summary>
        public async Task<int> GetList_Repair_Plan_Apt_Any_Count(string Apt_Code)
        {
            using (var con = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                con.Open();
                return await con.QuerySingleOrDefaultAsync<int>("Select Count(*) From Repair_Plan Where Apt_Code = @Apt_Code And AnyTime = 'B'", new { Apt_Code }, commandType: CommandType.Text);                
            }
        }


        /// <summary>
        /// 해당 공동주택 장기수선계획 기본정보 목록 중에 2개만 불러오기
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<List<Repair_Plan_Entity>> GetList_Repair_Plan_Apt_New(string Apt_Code)
        {
            using (var con = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                con.Open();
                var result = await con.QueryAsync<Repair_Plan_Entity>("Select Top 2 Aid, Repair_Plan_Code, Apt_Code, Plan_Review_Code, Plan_Review_Date, Adjustment_Date, Adjustment_Year, Adjustment_Month, Founding_Date, Founding_Man, Adjustment_Division, Plan_Period, LastAdjustment_Date, Adjustment_Man, Adjustment_Num, SmallSum_Unit, Small_Sum, SmallSum_Requirement, SmallSum_Basis, Emergency_Basis, Repair_Plan_Etc, User_ID, Complete From Repair_Plan Where Apt_Code = @Apt_Code And AnyTime = 'A' Order By Aid Desc", new { Apt_Code }, commandType: CommandType.Text);
                return result.ToList();
            }
        }

        /// <summary>
        /// 장기수선계획 기본 정보 해당 단지 년도별 정보 불러오기
        /// </summary>
        public async Task<List<Repair_Plan_Entity>> GetList_Repair_Plan_Apt_Year(string Apt_Code, string Adjustment_Year)
        {
            using (var con = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                con.Open();
                var result = await con.QueryAsync<Repair_Plan_Entity>("Select Repair_Plan_Code, Adjustment_Year, Adjustment_Month, Adjustment_Date From Repair_Plan Where Apt_Code = @Apt_Code And Adjustment_Year = @Adjustment_Year And AnyTime = 'A' Order By Aid Asc", new { Apt_Code, Adjustment_Year }, commandType: CommandType.Text);
                return result.ToList();
            }
        }

        // 장기수선계획 기본 정보 리스트 불러오기
        public async Task<List<Repair_Plan_Entity>> GetList_Repair_Plan()
        {
            using (var con = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                con.Open();
                var result = await con.QueryAsync<Repair_Plan_Entity>("Select Aid, Repair_Plan_Code, Apt_Code, Plan_Review_Code, Plan_Review_Date, Adjustment_Date, Adjustment_Year, Adjustment_Month, Founding_Date, Founding_Man, Adjustment_Division, Plan_Period, LastAdjustment_Date, Adjustment_Man, Adjustment_Num, SmallSum_Unit, Small_Sum, SmallSum_Requirement, SmallSum_Basis, Emergency_Basis, Repair_Plan_Etc, User_ID, Complete From Repair_Plan Order By Aid Desc", commandType: CommandType.Text);
                return result.ToList();
            }
        }

        // 장기수선계획 기본 수립일 혹은 조정일 가져오기 마지막
        public async Task<DateTime> Repair_Plan_AdjustmentDate(string Apt_Code)
        {
            using (var con = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                con.Open();
                return await con.QueryFirstOrDefaultAsync<DateTime>("Select Top 1 Adjustment_Date From Repair_Plan Where Apt_Code = @Apt_Code And AnyTime = 'A'", new { Apt_Code }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        ///  완료된 장기수선계획이 있으면 1 이상
        /// </summary>
        public async Task<int> BeComplete_Count(string Apt_Code)
        {
            using (var con = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                con.Open();
                return await con.QueryFirstOrDefaultAsync<int>("Select Count(*) From Repair_Plan Where Apt_Code = @Apt_Code And Complete = 'B' And (Adjustment_Division = '정기조정' or Adjustment_Division = '최초수립' or Adjustment_Division = '최초입력')", new { Apt_Code }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 완료된 장기수선계획의 수
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<int> BeComplete_Count_A(string Apt_Code)
        {
            using (var con = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                con.Open();
                return await con.QueryFirstOrDefaultAsync<int>("Select Count(*) From Repair_Plan Where Apt_Code = @Apt_Code And Complete = 'B' And AnyTime = 'A'", new { Apt_Code }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 해당 공동주택에 완성된 장기수선계획 마지막 식별코드
        /// </summary>
        /// <param name="Apt_Code">공동주택 식별코드</param>
        /// <returns>장기수선계획 식별코드</returns>
        public async Task<string> BeComplete_Code(string Apt_Code)
        {
            using (var con = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                con.Open();
                return await con.QueryFirstOrDefaultAsync<string>("Select Top 1 Repair_Plan_Code From Repair_Plan Where Apt_Code = @Apt_Code And Complete = 'B' And (Adjustment_Division = '정기조정' or Adjustment_Division = '최초수립' or Adjustment_Division = '최초입력') Order By Aid Desc", new { Apt_Code }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 해당 공동주택의 완료된 마지막 장기수선계획 식별코드
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<string> BeComplete_Code_A(string Apt_Code)
        {
            using (var con = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                con.Open();
                return await con.QueryFirstOrDefaultAsync<string>("Select Top 1 Repair_Plan_Code From Repair_Plan Where Apt_Code = @Apt_Code And Complete = 'B' And AnyTime = 'A' Order By Aid Desc", new { Apt_Code }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 해당 공동주택 첫 장기수선계획 기본 코드 가져오기
        /// </summary>
        public async Task<string> Repair_Plan_Code(string Apt_Code)
        {
            using (var con = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                con.Open();
                return await con.QueryFirstOrDefaultAsync<string>("Select Top 1 Repair_Plan_Code From Repair_Plan Where Apt_Code = @Apt_Code And AnyTime = 'A' Order by Aid Asc", new { Apt_Code }, commandType: CommandType.Text);
            }
        }

        #region 2019년 1월 5일

        /// <summary>
        /// 해당 장기수선계획에서 검토 코드 불러오기
        /// </summary>
        public async Task<string> Review_Code(string Repair_Plan_Code, string Apt_Code)
        {
            using (var con = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                con.Open();
                return await con.QueryFirstOrDefaultAsync<string>("Select Top 1 Plan_Review_Code From Repair_Plan Where Repair_Plan_Code = @Repair_Plan_Code And Apt_Code = @Apt_Code Order by Aid Asc", new { Repair_Plan_Code, Apt_Code }, commandType: CommandType.Text);
            }
            //return this.ctx.Query<string>("Select Top 1 Plan_Review_Code From Repair_Plan Where Repair_Plan_Code = @Repair_Plan_Code And Apt_Code = @Apt_Code Order by Aid Asc", new { Repair_Plan_Code, Apt_Code });
        }

        #endregion 2019년 1월 5일

        /// <summary>
        /// 장기수선계획 코드로 장기수선계획 기간 불러오기
        /// </summary>
        public async Task<int> Repair_Plan_Period_Code(string Repair_Plan_Code)
        {
            using (var con = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                con.Open();
                return await con.QueryFirstOrDefaultAsync<int>("Select Plan_Period From Repair_Plan Where Repair_Plan_Code = @Repair_Plan_Code", new { Repair_Plan_Code }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 완료된 마지막 장기수선계획의 계획기간 불러오기
        /// </summary>

        public async Task<int> Repair_Plan_Period(string Apt_Code, string Repair_Plan_Code)
        {
            using (var con = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                con.Open();
                return await con.QueryFirstOrDefaultAsync<int>("Select Plan_Period From Repair_Plan Where Apt_Code = @Apt_Code And Repair_Plan_Code = @Repair_Plan_Code", new { Apt_Code, Repair_Plan_Code }, commandType: CommandType.Text);
            }
            //return this.ctx.Query<int>("Select Plan_Period From Repair_Plan Where Apt_Code = @Apt_Code And Repair_Plan_Code = @Repair_Plan_Code", new { Apt_Code, Repair_Plan_Code });
        }

        /// <summary>
        /// 장기수선계획 명(조정일자) 불러오기
        /// </summary>
        public async Task<DateTime> Plan_Date(string Apt_Code, string Repair_Plan_Code)
        {
            using (var con = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                con.Open();
                return await con.QueryFirstOrDefaultAsync<DateTime>("Select Adjustment_Date From Repair_Plan Where Apt_Code = @Apt_Code And Repair_Plan_Code = @Repair_Plan_Code", new { Apt_Code, Repair_Plan_Code }, commandType: CommandType.Text);
            }
            //return this.ctx.Query<DateTime>("Select Adjustment_Date From Repair_Plan Where Apt_Code = @Apt_Code And Repair_Plan_Code = @Repair_Plan_Code", new { Apt_Code, Repair_Plan_Code });
        }

        /// <summary>
        /// 최근 장기수선계획에서 소액지출 금액 불러오기
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<string> Small_Sum(string Apt_Code)
        {
            using (var con = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                con.Open();
                return await con.QueryFirstOrDefaultAsync<string>("Select Top 1 Small_Sum From Repair_Plan Where Apt_Code = @Apt_Code And (Adjustment_Division = '정기조정' or Adjustment_Division = '최초수립' or Adjustment_Division = '최초입력')", new { Apt_Code }, commandType: CommandType.Text);
            }
            //return this.ctx.Query<string>("Select Top 1 Small_Sum From Repair_Plan Where Apt_Code = @Apt_Code And (Adjustment_Division = '정기조정' or Adjustment_Division = '최초수립' or Adjustment_Division = '최초입력')", new { Apt_Code });
        }

        /// <summary>
        /// 해당 공동주택에서 장기수선계획이 있는지 확인
        /// </summary>
        public async Task<int> Being_Plan(string Apt_code)
        {
            using (var con = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                con.Open();
                return await con.QueryFirstOrDefaultAsync<int>("Select Count(*) From Repair_Plan Where Apt_Code = @Apt_Code And AnyTime = 'A'", new { Apt_code }, commandType: CommandType.Text);
            }
            //return this.ctx.Query<int>("Select Count(*) From Repair_Plan Where Apt_Code = @Apt_Code", new { Apt_code });
        }

        /// <summary>
        /// 해당 공동주택에서 완료되지 않은 장기수선계획이 있는지 확인
        /// 2017-10-02
        /// </summary>
        public async Task<int> Being_Plan_None_Complete(string Apt_code)
        {
            using (var con = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                con.Open();
                return await con.QueryFirstOrDefaultAsync<int>("Select Count(*) From Repair_Plan Where Apt_Code = @Apt_Code and Complete = 'A' And AnyTime = 'A'", new { Apt_code }, commandType: CommandType.Text);
            }
            //return this.ctx.Query<int>("Select Count(*) From Repair_Plan Where Apt_Code = @Apt_Code and Complete = 'A'", new { Apt_code });
        }

        /// <summary>
        /// 해당 공동주택에서 장기수선계획이 있는지 확인
        /// </summary>
        public async Task<int> Being_Plan_Review(string Apt_code, string Plan_Review_Code)
        {
            using (var con = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                con.Open();
                return await con.QueryFirstOrDefaultAsync<int>("Select Count(*) From Repair_Plan Where Apt_Code = @Apt_Code And Plan_Review_Code = @Plan_Review_Code And AnyTime = 'A'", new { Apt_code, Plan_Review_Code }, commandType: CommandType.Text);
            }
            //return this.ctx.Query<int>("Select Count(*) From Repair_Plan Where Apt_Code = @Apt_Code And Plan_Review_Code = @Plan_Review_Code", new { Apt_code, Plan_Review_Code });
        }

        /// <summary>
        /// 해당 공동주택에 장기수선계획 조정년도 리스트 만들기
        /// </summary>
        /// <param name="Apt_Code">공동주택 식별코드</param>
        /// <returns>조정년 리스트 반환</returns>
        public async Task<List<Repair_Plan_Entity>> Getlist_Adjustment_Year(string Apt_Code)
        {
            var sql = "Select distinct Adjustment_Year From Repair_Plan Where Apt_Code = @Apt_Code And AnyTime = 'A'";

            using (var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                //connection.Open();
                var aa = await db.QueryAsync<Repair_Plan_Entity>(sql, new { Apt_Code }, commandType: CommandType.Text);
                return aa.ToList();
            }
            //return this.ctx.Query<string>(sql, new { Apt_Code }).ToList();
        }

        /// <summary>
        /// 해당 공동주택에 장기수선계획 선택 후 조정한 조정년도 구하기
        /// </summary>
        /// <param name="Apt_Code">공동주택 식별코드</param>
        /// <param name="Apt_Code">공동주택 식별코드</param>
        /// <returns>조정년 리스트 반환</returns>
        public async Task<string> Getlist_Adjustment_Year_Plan(string Apt_Code, string Repair_Plan_Code)
        {
            var sql = "Select Top 1 Adjustment_Year From Repair_Plan Where Apt_Code = @Apt_Code And Repair_Plan_Code = @Repair_Plan_Code";
            using (var con = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                con.Open();
                return await con.QueryFirstOrDefaultAsync<string>(sql, new { Apt_Code, Repair_Plan_Code }, commandType: CommandType.Text);
            }
            //return this.ctx.Query<string>(sql, new { Apt_Code, Repair_Plan_Code });
        }

        /// <summary>
        /// 선택된 장기수선계획 조정일
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="Repair_Plan_Code"></param>
        /// <returns></returns>
        public async Task<DateTime> Adjustment_Plan_Date(string Apt_Code, string Repair_Plan_Code)
        {
            var sql = "Select Top 1 Adjustment_Date From Repair_Plan Where Apt_Code = @Apt_Code And Repair_Plan_Code = @Repair_Plan_Code";

            using (var con = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                con.Open();
                return await con.QueryFirstOrDefaultAsync<DateTime>(sql, new { Apt_Code, Repair_Plan_Code }, commandType: CommandType.Text);
            }
            //return this.ctx.Query<DateTime>(sql, new { Apt_Code, Repair_Plan_Code });
        }

        /// <summary>
        /// 해당 공동주택의 장기수선계획 입력된 정보 수
        /// </summary>
        public async Task<int> Being_Count(string Apt_Code)
        {
            var sql = "Select Count(*) From Repair_Plan Where Apt_Code = @Apt_Code And AnyTime = 'A'";
            using (var con = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                con.Open();
                return await con.QueryFirstOrDefaultAsync<int>(sql, new { Apt_Code }, commandType: CommandType.Text);
            }
        }


        /// <summary>
        /// 해당 공동주택의 간편 수시조정으로 입력된 정보 수
        /// </summary>
        public async Task<int> Being_Count_Any(string Apt_Code)
        {
            var sql = "Select Count(*) From Repair_Plan Where AnyTime = 'B' And Apt_Code = @Apt_Code And AnyTime = 'A'";
            using (var con = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {                
                return await con.QueryFirstOrDefaultAsync<int>(sql, new { Apt_Code }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 장기수선계획 조정일
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="Repair_Plan_Code"></param>
        /// <returns></returns>
        public async Task<DateTime> Adjustment_Date(string Apt_Code, string Repair_Plan_Code)
        {
            using (var con = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                con.Open();
                return await con.QueryFirstOrDefaultAsync<DateTime>("Select Adjustment_Date From Repair_Plan Where Apt_Code = @Apt_Code And Repair_Plan_Code = @Repair_Plan_Code", new { Apt_Code, Repair_Plan_Code }, commandType: CommandType.Text);
            }
            //return this.ctx.Query<DateTime>("Select Adjustment_Date From Repair_Plan Where Apt_Code = @Apt_Code And Repair_Plan_Code = @Repair_Plan_Code", new { Apt_Code, Repair_Plan_Code });
        }

        /// <summary>
        /// 최근 삼년간 연차별 계획 내용
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="Repair_Plan_Code"></param>
        /// <param name="StartYear"></param>
        /// <param name="EndYear"></param>
        public async Task<List<Join_Article_Cycle_Cost_EntityA>> Year_Plan_practice(string Apt_Code, string Repair_Plan_Code, string StartYear, string EndYear)
        {
            using (var con = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                con.Open();
                var aa = await con.QueryAsync<Join_Article_Cycle_Cost_EntityA>("Report_Year_Plan_practice", new { Apt_Code, Repair_Plan_Code, StartYear, EndYear }, commandType: CommandType.StoredProcedure);
                return aa.ToList();
            }
            //return ctx.Query<Join_Article_Cycle_Cost_Entity>("Report_Year_Plan_practice", new { Apt_Code, Repair_Plan_Code, StartYear, EndYear }, commandType: CommandType.StoredProcedure).ToList();
        }

        /// <summary>
        /// 최근 삼년간 연차별 계획 총액 정보
        /// </summary>
        public async Task<Repair_Plan_practice_total_Entity> Year_Plan_Cost_Totay(string Apt_Code, string Repair_Plan_Code, string StartYear, string EndYear)
        {
            using var con = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            con.Open();
            return await con.QueryFirstOrDefaultAsync<Repair_Plan_practice_total_Entity>("Report_Year_Plan_practice_total", new { Apt_Code, Repair_Plan_Code, StartYear, EndYear }, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// 해당 공동주택에 입력된 장기수선계획의 수
        /// </summary>
        public async Task<int> Plan_Apt_Count(string Apt_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Repair_Plan Where Apt_Code = @Apt_Code", new { Apt_Code }); 
        }

        /// <summary>
        /// 이미 입력 여부 확인
        /// </summary>
        public async Task<int> Repeat_Code(string Apt_Code, string Repair_Plan_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Repair_Plan Where Apt_Code = @Apt_Code And Repair_Plan_Code = @Repair_Plan_Code", new { Apt_Code, Repair_Plan_Code });
        }
    }

    // 장기수선충당금 소액지출 대상 항목
    public class Repair_SmallSum_Object_Lib : IRepair_SmallSum_Object_Lib
    {
        private readonly IConfiguration _db;

        public Repair_SmallSum_Object_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        //장기수선계획 소액지출 대상 입력
        public async Task<Repair_SmallSum_Object_Entity> Add_SmallSum_Object(Repair_SmallSum_Object_Entity Plan)
        {
            var sql = "Insert Repair_SmallSum_Object (SmallSum_Object_Code, SmallSum_Object, SmallSum_Object_Detail, PostIP) Values (@SmallSum_Object_Code, @SmallSum_Object, @SmallSum_Object_Detail, @PostIP);";
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            await db.ExecuteAsync(sql, Plan);
            return Plan;
        }

        // 장기수선계획 소액지출 대상 수정하기
        public async Task<Repair_SmallSum_Object_Entity> Edit_SmallSum_Object(Repair_SmallSum_Object_Entity Plan)
        {
            var sql = "Update Repair_SmallSum_Object Set SmailSum_Object = @SmallSum_Object, SmallSum_Object_Detail = @SmallSum_Object_Detail, PostIP = @PostIP Where Aid = @Aid;";
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            await db.ExecuteAsync(sql, Plan);
            return Plan;
        }

        // 장기수선계획 소액지출 대상 마지막 일련번호 얻기
        public async Task<int> Last_Number()
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Top 1 Aid From Repair_SmallSum_Object Order by Aid Desc", new { });
        }

        // 장기수선계획 소액지출 대상 코드로 상세 불러오기
        public async Task<Repair_SmallSum_Object_Entity> Detail_SmallSum_Object(string Repair_SmallSum_Object_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<Repair_SmallSum_Object_Entity>("Select * From Repair_SmallSum_Object  Where Repair_Plan_Code = @Repair_Plan_Code", new { Repair_SmallSum_Object_Code });
        }

        /// 소액지출 대상 기본정보 삭제(완제 삭제)
        public async Task Remove_SmallSum_Object(int Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            await db.ExecuteAsync("Delete From Repair_SmallSum_Object Where Aid = @Aid", new { Aid });
        }

        /// 소액지출 대상 기본정보 삭제로 만들기
        public async Task Delete_SmallSum_Object(int Aid, string Del)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            await db.ExecuteAsync("Update Repair_SmallSum_Object Set Del = @Del Where Aid = @Aid", new { Aid, Del });
        }

        // 장기수선계획별 소액지출 대상 정보 불러오기
        public async Task<List<Repair_SmallSum_Object_Entity>> GetList_SmallSum_Object(string Del)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var lst = await db.QueryAsync<Repair_SmallSum_Object_Entity>("Select * From Repair_SmallSum_Object Where Del = @Del Order By Aid Asc", new { Del });
            return lst.ToList();
        }

        // 해당 공동주택에 입력된 수
        public async Task<int> BeCount(string Apt_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Repair_SmallSum_Object Where Apt_Code = @Apt_Code", new { Apt_Code });
        }

        //public string Detail_SmallSum_Requirement(string SmallSum_Requirement_Code)
        //{
        //    return this.ctx.Query<string>("Select SmallSum_Requirement From SmallSum_Requirement Where SmallSum_Requirement_Code = @SmallSum_Requirement_Code", new { SmallSum_Requirement_Code });
        //}
    }

    // 장기수선충당금 소액지출 요건 항목
    public class Repair_SmallSum_Requirement_Lib : IRepair_SmallSum_Requirement_Lib
    {
        private readonly IConfiguration _db;

        public Repair_SmallSum_Requirement_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        // 장기수선충당금 소액지출 요건 내용 입력하기
        public async Task<Repair_SmallSum_Requirement_Entity> Add_SmallSum_Requirement(Repair_SmallSum_Requirement_Entity RSR)
        {
            var sql = "Insert SmallSum_Requirement (SmallSum_Requirement_Code, SmallSum_Requirement, SmallSum_Requirement_Etc, PostIP) Values (@SmallSum_Requirement_Code, @SmallSum_Requirement, @SmallSum_Requirement_Etc, @PostIP);";
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            await db.ExecuteAsync(sql, RSR);
            return RSR;
        }

        // 장기수선충당금 소액지출 요건 내용 수정하기
        public async Task<Repair_SmallSum_Requirement_Entity> Edit_SmallSum_Requirement(Repair_SmallSum_Requirement_Entity RSR)
        {
            var sql = "Update SmallSum_Requirement (SmallSum_Requirement_Code = @SmallSum_Requirement_Code, @SmallSum_Requirement = SmallSum_Requirement, SmallSum_Requirement_Etc = @SmallSum_Requirement_Etc);";
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            await db.ExecuteAsync(sql, RSR);
            return RSR;
        }

        // 장기수선계획별 소액지출 요건 정보 불러오기
        public async Task<List<Repair_SmallSum_Requirement_Entity>> GetList_SmallSum_Requirement()
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var lst = await db.QueryAsync<Repair_SmallSum_Requirement_Entity>("Select * From SmallSum_Requirement Order By Aid Asc", new { });
            return lst.ToList();
        }

        // 장기수선계획 소액지출 요건 정보 삭제
        public async Task Delete_SmallSum_Requirement(int Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            await db.ExecuteAsync("Delete From SmallSum_Requirement Where Aid = @Aid", new { Aid }); ;
        }
    }

    // 장기수선충당금 소액지출 대상 선택 정보
    public class Repair_Object_Selection_Lib : IRepair_Object_Selection_Lib
    {
        private readonly IConfiguration _db;

        public Repair_Object_Selection_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        //장기수선계획 소액지출 대상 선택 입력
        public async Task<Repair_SmallSum_Object_Selection_Entity> Add_RSO_Selection(Repair_SmallSum_Object_Selection_Entity RSOS)
        {
            var sql = "Insert Repair_SmallSum_Object_Selection (SmallSum_Object_Selection_Code, Apt_Code, Repair_Plan_Code, SmallSum_Object_Code, SmallSum_Object_Content, PostIP) Values (@SmallSum_Object_Selection_Code, @Apt_Code, @Repair_Plan_Code, @SmallSum_Object_Code, @SmallSum_Object_Content, @PostIP);";
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            await db.ExecuteAsync(sql, RSOS);
            return RSOS;
        }

        // 장기수선계획 소액지출 대상 선택 수정하기
        public async Task<Repair_SmallSum_Object_Selection_Entity> Edit_RSOS(Repair_SmallSum_Object_Selection_Entity RSOS)
        {
            var sql = "Update Repair_SmallSum_Object_Selection Set SmailSum_Object_Code = @SmallSum_Object_Code, SmallSum_Object_Content = @SmallSum_Object_Content Where Aid = @Aid;";
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            await db.ExecuteAsync(sql, RSOS);
            return RSOS;
        }

        /// <summary>
        /// 장기수선계획 소액지출 대상 선택 수정하기
        /// </summary>
        /// <param name="Aid">소액지출 대상 선택 식별코드</param>
        /// <param name="SmallSum_Object_Content">소액지출 대상 내용</param>
        /// 2017-03-07
        public async Task Edit_RSOS_A(string Aid, string SmallSum_Object_Content)
        {
            var sql = "Update Repair_SmallSum_Object_Selection Set SmallSum_Object_Content = @SmallSum_Object_Content Where Aid = @Aid;";
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            await db.ExecuteAsync(sql, new { Aid, SmallSum_Object_Content });
        }

        /// <summary>
        /// 해당 장기수선계획 식별코드로 소액지출 대상 선택 정보 모두 삭제
        /// </summary>
        /// <param name="Repair_Plan_Code">장기수선계획 식별코드</param>
        /// 2017
        public async Task Delete_ObjetSelection_PlanCode(string Repair_Plan_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            await db.ExecuteAsync("Delete Repair_SmallSum_Object_Selection Where Repair_Plan_Code = @Repair_Plan_Code", new { Repair_Plan_Code });
        }

        // 해당 소액지출 대상 삭제
        public async Task Remove(string Apt_Code, string Repair_Plan_Code, string SmallSum_Object_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            await db.ExecuteAsync("Delete Repair_SmallSum_Object_Selection Where Apt_Code = @Apt_Code And Repair_Plan_Code = @Repair_Plan_Code And SmallSum_Object_Code = @SmallSum_Object_Code", new { Apt_Code, Repair_Plan_Code, SmallSum_Object_Code });
        }

        // 장기수선계획 소액지출 대상 선택 마지막 일련번호 얻기
        public async Task<int> Last_Number()
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Top 1 Aid From Repair_SmallSum_Object_Selection Order by Aid Desc");
        }

        // 해당 공동주택에 입력된 수
        public async Task<int> BeCount(string Apt_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Repair_SmallSum_Object_Selection Where Apt_Code = @Apt_Code", new { Apt_Code });
        }

        /// 장기수선계획 소액지출 대상 선택기본정보 삭제(완제 삭제)
        public async Task Remove_SmallSum_Object_Selection(int Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            await db.ExecuteAsync("Delete Repair_SmallSum_Object_Selection Where Aid = @Aid", new { Aid });
        }

        /// 장기수선계획 소액지출 대상 선택 정보 삭제
        public async Task Delete_SmallSum_Object(int Aid, string Del)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            await db.ExecuteAsync("Update Repair_SmallSum_Object_Selection Set Del = @Del Where Aid = @Aid", new { Aid, Del });
        }

        // 장기수선계획별 소액지출 대상 선택 정보 불러오기(장기수선계획 코드로)
        public async Task<List<Repair_SmallSum_Object_Selection_Entity>> GetList_RSOS(string Repair_Plan_Code, string Del)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var lst = await db.QueryAsync<Repair_SmallSum_Object_Selection_Entity>("Select a.Aid, a.SmallSum_Object_Selection_Code, a.Repair_Plan_Code, a.Apt_Code, a.SmallSum_Object_Code, a.SmallSum_Object_Content From Repair_SmallSum_Object_Selection as a Join Repair_SmallSum_Object as b on a.SmallSum_Object_Code = b.SmallSum_Object_Code Where a.Repair_Plan_Code = @Repair_Plan_Code Order By b.Aid Asc", new { Repair_Plan_Code, Del });
            return lst.ToList();
        }

        // 장기수선계획 소액지출 요건 코드로 상세 불러오기
        public async Task<Repair_SmallSum_Requirement_Entity> Detail_SmallSum_Requirement(string SmallSum_Requirement_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<Repair_SmallSum_Requirement_Entity>("Select * From SmallSum_Requirement Where SmallSum_Requirement_Code = @SmallSum_Requirement_Code", new { SmallSum_Requirement_Code });
        }

        // 장기수선계획 소액지출 요건 코드로 요건 내용 불러오기
        public async Task<string> SmallSum_Requirement_Name(string SmallSum_Requirement_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<string>("Select SmallSum_Requirement From SmallSum_Requirement Where SmallSum_Requirement_Code = @SmallSum_Requirement_Code", new { SmallSum_Requirement_Code });
        }

        // 해당 소액지출 요건 존재 여부 확인
        public int Being_Code(string Apt_Code, string Repair_Plan_Code, string SmallSum_Object_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return db.QuerySingleOrDefault<int>("Select Count(*) From Repair_SmallSum_Object_Selection Where Apt_Code = @Apt_Code And Repair_Plan_Code = @Repair_Plan_Code And SmallSum_Object_Code = @SmallSum_Object_Code", new { Apt_Code, Repair_Plan_Code, SmallSum_Object_Code });
        }
    }

    // 장기수선충당금 소액지출 요건 선택 정보
    public class SmallSum_Requirement_Selection_Lib : ISmallSum_Repuirement_Selection_Lib
    {
        private readonly IConfiguration _db;

        public SmallSum_Requirement_Selection_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        //장기수선계획 소액지출 요건 선택 입력
        public async Task<Repair_SmallSum_Requirement_Selection_Entity> Add_RSR_Selection(Repair_SmallSum_Requirement_Selection_Entity RSRS)
        {
            var sql = "Insert SmallSum_Requirement_Selection (SmallSum_Requirement_Selection_Code, Apt_Code, Repair_Plan_Code, SmallSum_Requirement_Code, SmallSum_Requirement_Content) Values (@SmallSum_Requirement_Selection_Code, @Apt_Code, @Repair_Plan_Code, @SmallSum_Requirement_Code, @SmallSum_Requirement_Content);";
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            await db.ExecuteAsync(sql, RSRS);
            return RSRS;
        }

        // 장기수선계획 소액지출 요건 선택 수정하기
        public async Task<Repair_SmallSum_Requirement_Selection_Entity> Edit_RSOS(Repair_SmallSum_Requirement_Selection_Entity RSOS)
        {
            var sql = "Update SmallSum_Requirement_Selection Set SmailSum_Requirement_Code = @SmallSum_Requirement_Code, SmallSum_Requirement_Content = @SmallSum_Requirement_Content Where Aid = @Aid;";
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            await db.ExecuteAsync(sql, RSOS);
            return RSOS;
        }

        /// <summary>
        /// 소액지출 요건 선택 리스트에서 수정하기
        /// </summary>
        /// <param name="Aid">소액지지출 선택 식별코드</param>
        /// <param name="SmallSum_Requirement_Content">소액지출 요건 내용</param>
        /// 2017-03-07
        public async Task Edit_RSOS_A(string Aid, string SmallSum_Requirement_Content)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            await db.ExecuteAsync("Update SmallSum_Requirement_Selection Set SmallSum_Requirement_Content = @SmallSum_Requirement_Content Where Aid = @Aid", new { Aid, SmallSum_Requirement_Content });
        }

        // 해당 소액지출 요건 존재 여부 확인
        public int Being_Code(string Apt_Code, string Repair_Plan_Code, string SmallSum_Requirement_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return db.QuerySingleOrDefault<int>("Select Count(*) From SmallSum_Requirement_Selection Where Apt_Code = @Apt_Code And Repair_Plan_Code = @Repair_Plan_Code And SmallSum_Requirement_Code = @SmallSum_Requirement_Code", new { Apt_Code, Repair_Plan_Code, SmallSum_Requirement_Code });
        }

        // 해당 공동주택에 입력된 수
        public async Task<int> BeCount(string Apt_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From SmallSum_Requirement_Selection Where Apt_Code = @Apt_Code", new { Apt_Code });
        }

        /// 장기수선계획 소액지출 요건 선택기본정보 삭제(완제 삭제)
        public async Task Remove_SmallSum_Requirement_Selection(int Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            await db.ExecuteAsync("Delete SmallSum_Requirement_Selection Where Aid = @Aid", new { Aid });
        }

        // 해당 소액지출 요건 삭제
        public async Task Remove(string Apt_Code, string Repair_Plan_Code, string SmallSum_Requirement_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            await db.ExecuteAsync("Delete SmallSum_Requirement_Selection Where Apt_Code = @Apt_Code And Repair_Plan_Code = @Repair_Plan_Code And SmallSum_Requirement_Code = @SmallSum_Requirement_Code", new { Apt_Code, Repair_Plan_Code, SmallSum_Requirement_Code });
        }

        /// <summary>
        /// 해당 장기수선계획 식별코드로 소액지출 요건 선택 정보 모두 삭제
        /// </summary>
        /// <param name="Repair_Plan_Code">장기수선계획 식별코드</param>
        /// 2017
        public async Task Delete_RequirementSelection_PlanCode(string Repair_Plan_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            await db.ExecuteAsync("Delete SmallSum_Requirement_Selection Where Repair_Plan_Code = @Repair_Plan_Code", new { Repair_Plan_Code });
        }

        // 장기수선계획별 소액지출 요건 선택 정보 불러오기(장기수선계획 코드로)
        public async Task<List<Repair_SmallSum_Requirement_Selection_Entity>> GetList_RSRS(string Repair_Plan_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var lst = await db.QueryAsync<Repair_SmallSum_Requirement_Selection_Entity>("Select a.Aid, a.SmallSum_Requirement_Selection_Code, a.Apt_Code, a.SmallSum_Requirement_Code, a.Repair_Plan_Code, a.SmallSum_Requirement_Content From SmallSum_Requirement_Selection as a Join SmallSum_Requirement as b on a.SmallSum_Requirement_Code = b.SmallSum_Requirement_Code Where a.Repair_Plan_Code = @Repair_Plan_Code Order By b.Aid Asc", new { Repair_Plan_Code });
            return lst.ToList();
        }
    }

    // 장기수선계획 진행 현황
    public class Plan_Prosess_Lib : IPlan_Prosess_Lib
    {
        private readonly IConfiguration _db;

        public Plan_Prosess_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 장기수선계획 진행 현황 최초 입력
        /// </summary>
        public async Task<Plan_Prosess> Add(Plan_Prosess pp)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var sql = "Insert into Plan_Prosess (Apt_Code, Repair_Plan_Code, PostIP) Values (@Apt_Code, @Repair_Plan_Code, @PostIP)";
            await db.ExecuteAsync(sql, pp);
            return pp;
        }

        /// <summary>
        /// 각 공정 완료여부 수정
        /// </summary>
        /// 2017-03-09
        public async Task Edit_Complete_Select(string Apt_Code, string Repair_Plan_Code, string Feild, string Query)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var sql = "Update Plan_Prosess Set " + Feild + " = @Query, " + Feild + "_Date = GetDate() Where Apt_Code = @Apt_Code And Repair_Plan_Code = @Repair_Plan_Code";
            await db.ExecuteAsync(sql, new { Apt_Code, Repair_Plan_Code, Feild, Query });
        }

        /// <summary>
        ///  장기수선계획 진행현황 수선항목 완료 입력
        /// </summary>
        public async Task Edit_Article(string Apt_Code, string Repair_Plan_Code, string Article_Complete)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var sql = "Update Plan_Prosess Set Article_Complete = @Article_Complete, Article_Complete_Date = GetDate() Where Apt_Code = @Apt_Code And Repair_Plan_Code = @Repair_Plan_Code";
            await db.ExecuteAsync(sql, new { Apt_Code, Repair_Plan_Code, Article_Complete });
        }

        /// <summary>
        ///  장기수선계획 진행현황 수선주기 완료 입력
        /// </summary>
        /// 2017-03-07
        public async Task Edit_Cycle(string Apt_Code, string Repair_Plan_Code, string Cycle_Complete)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var sql = "Update Plan_Prosess Set Cycle_Complete = @Cycle_Complete, Cycle_Complete_Date = GetDate() Where Apt_Code = @Apt_Code And Repair_Plan_Code = @Repair_Plan_Code";
            await db.ExecuteAsync(sql, new { Apt_Code, Repair_Plan_Code, Cycle_Complete });
        }

        /// <summary>
        ///  장기수선계획 진행현황 수선금액 완료 입력
        /// </summary>
        /// 2017-03-07
        public async Task Edit_Cost(string Apt_Code, string Repair_Plan_Code, string Cost_Complete)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var sql = "Update Plan_Prosess Set Cost_Complete = @Cost_Complete, Cost_Complete_Date = GetDate() Where Apt_Code = @Apt_Code And Repair_Plan_Code = @Repair_Plan_Code";
            await db.ExecuteAsync(sql, new { Apt_Code, Repair_Plan_Code, Cost_Complete });
        }

        /// <summary>
        ///  장기수선계획 진행현황 수선계획 완료 입력
        /// </summary>
        /// 2017-03-07
        public async Task Edit_Plan(string Apt_Code, string Repair_Plan_Code, string Cost_Complete)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var sql = "Update Plan_Prosess Set Plan_Complete = @Paln_Complete, Plan_Complete_Date = GetDate() Where Apt_Code = @Apt_Code And Repair_Plan_Code = @Repair_Plan_Code";
            await db.ExecuteAsync(sql, new { Apt_Code, Repair_Plan_Code, Cost_Complete });
        }

        /// <summary>
        /// 해당 공동주택의 선택된 장기수선계획 완료 정보 불러오기
        /// </summary>
        /// <returns></returns>
        public async Task<Plan_Prosess> Detail(string Apt_Code, string Repair_Plan_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<Plan_Prosess>("Select * From Plan_Prosess Where Apt_Code = @Apt_Code And Repair_Plan_Code = @Repair_Plan_Code", new { Apt_Code, Repair_Plan_Code });
        }

        /// <summary>
        /// 해당 공동주택의 장기수선계획 진행 정보 있는지 여부
        /// </summary>
        public async Task<int> Being_Complete(string Apt_Code, string Repair_Plan_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Plan_Prosess Where Apt_Code = @Apt_Code And Repair_Plan_Code = @Repair_Plan_Code", new { Apt_Code, Repair_Plan_Code });
        }

        /// <summary>
        /// 각 공정별 완료 여부 불러오기
        /// </summary>
        public async Task<string> Being_Complete_Sort(string Apt_Code, string Repair_Plan_Code, string Query)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<string>("Select " + Query + " From Plan_Prosess Where Apt_Code = @Apt_Code And Repair_Plan_Code = @Repair_Plan_Code", new { Apt_Code, Repair_Plan_Code, Query });
        }

        /// <summary>
        /// 각 공정별 완료여부 삭제
        /// </summary>
        public async Task Delete_Complete(string Repair_Plan_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            await db.ExecuteAsync("Delete Plan_Prosess Where Repair_Plan_Code = @Repair_Plan_Code", new { Repair_Plan_Code });
        }
    }
}