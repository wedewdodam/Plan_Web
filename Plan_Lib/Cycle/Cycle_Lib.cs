using Dapper;
using Microsoft.Extensions.Configuration;
using Plan_Blazor_Lib.Plan;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Plan_Blazor_Lib.Cycle
{
    // 장기수선계획 수선주기 설정 메서드
    public class Cycle_Lib : ICycle_Lib
    {
        private readonly IConfiguration _db;

        public Cycle_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        // 수선주기 입력
        public async Task<Cycle_Entity> Add_RepairCycle(Cycle_Entity cycle)
        {
            var msh = "Insert Repair_Cycle (Apt_Code, Repair_Plan_Code, Repair_Article_Code, Division, Sort_A_Code, Sort_B_Code, Sort_C_Code, Sort_A_Name, Sort_B_Name, Sort_C_Name, Law_Repair_Cycle_All, Set_Repair_Cycle_All, Law_Repair_Cycle_Part, Set_Repair_Cycle_Part, Law_Repair_Rate, Set_Repair_Rate, Repair_Plan_Year_Part, Repair_Last_Year_All, Repair_Last_Year_Part, Repair_Plan_Year_All, All_Cycle_Num, Part_Cycle_Num, Repair_Cycle_Etc, User_ID, Post_IP) Values (@Apt_Code, @Repair_Plan_Code, @Repair_Article_Code, @Division, @Sort_A_Code, @Sort_B_Code, @Sort_C_Code, @Sort_A_Name, @Sort_B_Name, @Sort_C_Name, @Law_Repair_Cycle_All, @Set_Repair_Cycle_All, @Law_Repair_Cycle_Part, @Set_Repair_Cycle_Part, @Law_Repair_Rate, @Set_Repair_Rate, @Repair_Plan_Year_Part, @Repair_Last_Year_All, @Repair_Last_Year_Part, @Repair_Plan_Year_All, @All_Cycle_Num, @Part_Cycle_Num, @Repair_Cycle_Etc, @User_ID, @Post_IP); Select Cast(SCOPE_IDENTITY() As Int);";

            using var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var Aid = await ctx.QuerySingleOrDefaultAsync<int>(msh, cycle);
            cycle.Aid = Aid;
            return cycle;

            //    var Aid = ff.ctx.Query<int>(msh, cycle).Single();
            //cycle.Aid = Aid;
            //return cycle;
        }

        // 수선주기 수정
        public async Task<Cycle_Entity> Edit_RepairCycle(Cycle_Entity cycle)
        {
            var aa = "Update Repair_Cycle Set Division = @Division, Set_Repair_Cycle_All = @Set_Repair_Cycle_All, Set_Repair_Cycle_Part = @Set_Repair_Cycle_Part, Set_Repair_Rate = @Set_Repair_Rate, Repair_Plan_Year_Part = @Repair_Plan_Year_Part, Repair_Last_Year_All = @Repair_Last_Year_All, Repair_Last_Year_Part = @Repair_Last_Year_Part, Repair_Plan_Year_All = @Repair_Plan_Year_All, All_Cycle_Num = @All_Cycle_Num, Part_Cycle_Num = @Part_Cycle_Num, Repair_Cycle_Etc = @Repair_Cycle_Etc, User_ID = @User_ID, Post_IP = @Post_IP Where Repair_Plan_Code = @Repair_Plan_Code And Repair_Article_Code = @Repair_Article_Code";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(aa, cycle);
                return cycle;
            }
            //ff.ctx.Execute(aa, cycle);
            //return cycle;
        }

        // 수선주기 반복 체크
        public async Task<int> Repeat_RepairCycle(string Repair_Article_Code, string Repair_Plan_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Repair_Cycle Where Repair_Article_Code = @Repair_Article_Code And Repair_Plan_Code = @Repair_Plan_Code", new { Repair_Article_Code, Repair_Plan_Code }, commandType: CommandType.Text);
            }
            //return this.ff.ctx.Query<int>("Select Count(*) From Repair_Cycle Where Repair_Article_Code = @Repair_Article_Code And Repair_Plan_Code = @Repair_Plan_Code", new { Repair_Article_Code, Repair_Plan_Code });
        }

        // 수선주기 반복 체크
        public int Be_RepairCycle(string Repair_Article_Code, string Repair_Plan_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return ctx.QuerySingleOrDefault<int>("Select Count(*) From Repair_Cycle Where Repair_Article_Code = @Repair_Article_Code And Repair_Plan_Code = @Repair_Plan_Code", new { Repair_Article_Code, Repair_Plan_Code }, commandType: CommandType.Text);
            }
            //return this.ff.ctx.Query<int>("Select Count(*) From Repair_Cycle Where Repair_Article_Code = @Repair_Article_Code And Repair_Plan_Code = @Repair_Plan_Code", new { Repair_Article_Code, Repair_Plan_Code });
        }

        /// <summary>
        /// 수선주기 식별코드로 수선주기 삭제
        /// </summary>
        public async Task Delete_RepairCycle(int Aid)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync("Delete From Repair_Cycle Where Aid = @Aid", new { Aid }, commandType: CommandType.Text);
            }
            // ff.ctx.Execute("Delete From Repair_Cycle Where Aid = @Aid", new { Aid });
        }

        /// <summary>
        /// 장기수선계획 식별코드와 수선항목 식별코드가 맞으면 해당 수선주기 삭제
        /// </summary>
        public async Task Remove_RepairCycle(string Repair_Plan_Code, string Repair_Article_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync("Delete From Repair_Cycle Where Repair_Plan_Code = @Repair_Plan_Code And Repair_Article_Code = @Repair_Article_Code", new { Repair_Plan_Code, Repair_Article_Code }, commandType: CommandType.Text);
            }
            //ff.ctx.Execute("Delete From Repair_Cycle Where Repair_Plan_Code = @Repair_Plan_Code And Repair_Article_Code = @Repair_Article_Code", new { Repair_Plan_Code, Repair_Article_Code });
        }

        /// <summary>
        /// 해당 장기수선계획 식별코드로 입력된 모든 수선주기 중에 수선항목 식별코드로 입력된 수선주기 삭제
        /// </summary>
        /// <param name="Repair_Plan_Code">장기수선계획 식별코드</param>
        /// <param name="Repair_Article_Code">수선항목 식별코드</param>
        /// 2017
        public async Task Delete_Article_Cycle(string Repair_Plan_Code, string Repair_Article_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync("Delete From Repair_Cycle Where Repair_Plan_Code = @Repair_Plan_Code And Repair_Article_Code = @Repair_Article_Code", new { Repair_Plan_Code, Repair_Article_Code }, commandType: CommandType.Text);
            }
            //ff.ctx.Execute("Delete From Repair_Cycle Where Repair_Plan_Code = @Repair_Plan_Code And Repair_Article_Code = @Repair_Article_Code", new { Repair_Plan_Code, Repair_Article_Code });
        }

        /// <summary>
        /// 수선항목 코드로 수선주기 존재 여부 확인
        /// </summary>
        /// <param name="Repair_Article_Code"></param>
        /// <returns></returns>
        public async Task<int> be_cycle_code(string Repair_Article_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Repair_Cycle Where Repair_Article_Code  = @Repair_Article_Code", new { Repair_Article_Code }, commandType: CommandType.Text);
            }
            //return ff.ctx.Query<int>("Select Count(*) FromRepair_Cycle Where Repair_Article_Code  = @Repair_Article_Code", new { Repair_Article_Code });
        }

        /// <summary>
        /// 해당 장기수선계획 식별코드로 입력된 모든 수선주기 삭제
        /// </summary>
        /// <param name="Repair_Plan_Code">장기수선계획 식별코드</param>
        /// 2017
        public async Task Delete_Cycle_PlanCode(string Repair_Plan_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync("Delete Repair_Cycle Where Repair_Plan_Code = @Repair_Plan_Code", new { Repair_Plan_Code }, commandType: CommandType.Text);
            }
            //ff.ctx.Execute("Delete Repair_Cycle Where Repair_Plan_Code = @Repair_Plan_Code", new { Repair_Plan_Code });
        }

        // 수선주기 리스트(수선항목으로)
        public async Task<List<Join_Article_Cycle_Cost_EntityA>> GetList_RepairCycle(string Apt_Code, string Repair_Plan_Code, string Repair_Article_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aa = await ctx.QueryAsync<Join_Article_Cycle_Cost_EntityA>("Select a.Aid, a.All_Cycle_Num, a.Apt_Code, a.Division, a.Law_Repair_Cycle_All, a.Law_Repair_Cycle_Part, a.Law_Repair_Rate, a.Part_Cycle_Num, a.Repair_Article_Code, a.Repair_Last_Year_All, a.Repair_Last_Year_Part, a.Repair_Plan_Code, a.Repair_Plan_Complete, a.Repair_Plan_Year_All, a.Repair_Plan_Year_Part, a.Set_Repair_Cycle_All, a.Set_Repair_Cycle_Part, a.Set_Repair_Rate, a.Sort_A_Code, a.Sort_A_Name, a.Sort_B_Code, a.Sort_B_Name, a.Sort_C_Code, a.Sort_C_Name, b.All_Cycle, b.Part_Cycle From Repair_Cycle as a Join Repair_Article as b on a.Repair_Article_Code = b.Aid Where a.Apt_Code = @Apt_Code and b.Apt_Code = @Apt_Code and a.Repair_Plan_Code = @Repair_Plan_Code And a.Repair_Article_Code = @Repair_Article_Code and b.Repair_Plan_Code = @Repair_Plan_Code And b.Aid = @Repair_Article_Code Order By a.Sort_A_Code Asc, Convert(int, a.Sort_B_Code) Asc, Convert(int, a.Sort_C_Code) Asc", new { Apt_Code, Repair_Plan_Code, Repair_Article_Code }, commandType: CommandType.Text);
                return aa.ToList();
            }
            //return ff.ctx.Query<Join_Article_Cycle_Cost_Entity>("Select a.Aid, a.All_Cycle_Num, a.Apt_Code, a.Division, a.Law_Repair_Cycle_All, a.Law_Repair_Cycle_Part, a.Law_Repair_Rate, a.Part_Cycle_Num, a.Repair_Article_Code, a.Repair_Last_Year_All, a.Repair_Last_Year_Part, a.Repair_Plan_Code, a.Repair_Plan_Complete, a.Repair_Plan_Year_All, a.Repair_Plan_Year_Part, a.Set_Repair_Cycle_All, a.Set_Repair_Cycle_Part, a.Set_Repair_Rate, a.Sort_A_Code, a.Sort_A_Name, a.Sort_B_Code, a.Sort_B_Name, a.Sort_C_Code, a.Sort_C_Name, b.All_Cycle, b.Part_Cycle From Repair_Cycle as a Join Repair_Article as b on a.Repair_Article_Code = b.Aid Where a.Apt_Code = @Apt_Code and b.Apt_Code = @Apt_Code and a.Repair_Plan_Code = @Repair_Plan_Code And a.Repair_Article_Code = @Repair_Article_Code and b.Repair_Plan_Code = @Repair_Plan_Code And b.Aid = @Repair_Article_Code Order By a.Sort_A_Code Asc, Convert(int, a.Sort_B_Code) Asc, Convert(int, a.Sort_C_Code) Asc", new { Apt_Code, Repair_Plan_Code, Repair_Article_Code }).ToList();
        }

        /// <summary>
        /// 수선주기 리스트(장기수선계획별, 시설물 분류별)
        /// </summary>
        /// <param name=""></param>
        /// <param name="분류별"></param>
        /// <returns></returns>
        public async Task<List<Join_Article_Cycle_Cost_EntityA>> GetLIst_RepairCycle_Sort(string Repair_Plan_Code, string Sort_Field, string Sort_Code, string Apt_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aa = await ctx.QueryAsync<Join_Article_Cycle_Cost_EntityA>("Repair_Cycle_Sort", new { Repair_Plan_Code, Sort_Field, Sort_Code, Apt_Code }, commandType: System.Data.CommandType.StoredProcedure);
                return aa.ToList();
            }
            //return ff.ctx.Query<Join_Article_Cycle_Cost_Entity>("Repair_Cycle_Sort", new { Repair_Plan_Code, Sort_Field, Sort_Code, Apt_Code }, commandType: CommandType.StoredProcedure).ToList();
        }

        // 해당 공동주택의 수선주기 중 현재 선택된 수선계획 코드의 하나 이전 수선주기 정보
        public async Task<List<Join_Article_Cycle_Cost_EntityA>> GetList_Ago(string Apt_Code, string Feild, string Sort_Code, string Adjustment_Date)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aa = await ctx.QueryAsync<Join_Article_Cycle_Cost_EntityA>("Select * From Repair_Cycle as a Join Repair_Article as b on a.Repair_Article_Code = b.Aid Join Plan_Prosess as c on a.Repair_Plan_Code = c.Repair_Plan_Code Where a." + Feild + " = @Sort_Code And a.Repair_Plan_Code in (Select Top 1 Repair_Plan_Code From Repair_Plan Where Adjustment_Date < @Adjustment_Date And Apt_Code = @Apt_Code Order By Adjustment_Date Desc) And c.Plan_Complete = 'B' Order By a.Aid Desc", new { Apt_Code, Feild, Sort_Code, Adjustment_Date }, commandType: CommandType.Text);
                return aa.ToList();
            }
            //return this.ff.ctx.Query<Join_Article_Cycle_Cost_Entity>("Select * From Repair_Cycle as a Join Repair_Article as b on a.Repair_Article_Code = b.Aid Join Plan_Prosess as c on a.Repair_Plan_Code = c.Repair_Plan_Code Where a." + Feild + " = @Sort_Code And a.Repair_Plan_Code in (Select Top 1 Repair_Plan_Code From Repair_Plan Where Adjustment_Date < @Adjustment_Date And Apt_Code = @Apt_Code Order By Adjustment_Date Desc) And c.Plan_Complete = 'B' Order By a.Aid Desc", new { Apt_Code, Feild, Sort_Code, Adjustment_Date }).ToList();
        }

        // 수선주기 리스트(수선항목으로)
        public async Task<List<Cycle_Entity>> GetList_Cycle(string Apt_Code, string Repair_Plan_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aa = await ctx.QueryAsync<Cycle_Entity>("Select a.Aid, a.Apt_Code, a.All_Cycle_Num, a.User_ID, a.Post_IP, a.Repair_Plan_Complete, a.Post_Date, a.Division, a.Part_Cycle_Num, a.Repair_Article_Code, a.Repair_Cycle_Etc, a.Repair_Plan_Year_All, a.Law_Repair_Cycle_All, a.Law_Repair_Cycle_Part, a.Law_Repair_Rate, a.Repair_Plan_Code, a.Repair_Plan_Year_All, a.Repair_Plan_Year_Part, a.Set_Repair_Cycle_All, a.Set_Repair_Cycle_Part, a.Set_Repair_Rate, a.Sort_A_Code, a.Sort_A_Name, a.Sort_B_Code, a.Sort_B_Name, a.Sort_C_Code, a.Sort_B_Name, b.Repair_Article_Name, b.All_Cycle, b.Part_Cycle, b.Repair_Rate From Repair_Cycle as a Join Repair_Article as b on a.Repair_Article_Code = b.Aid Where a.Apt_Code = @Apt_Code And a.Repair_Plan_Code = @Repair_Plan_Code Order By a.Sort_A_Code, Convert(int, a.Sort_B_Code) Asc, Convert(int, a.Sort_C_Code) Asc", new { Apt_Code, Repair_Plan_Code }, commandType: CommandType.Text);
                return aa.ToList();
            }
            //return ff.ctx.Query<Repair_Cycle>("Select * From Repair_Cycle Where Apt_Code = @Apt_Code And Repair_Plan_Code = @Repair_Plan_Code Order By Sort_A_Code, Convert(int, Sort_B_Code) Asc, Convert(int, Sort_C_Code) Asc", new { Apt_Code, Repair_Plan_Code }).ToList();
        }

        // 수선주기 상세
        public async Task<Cycle_Entity> Detail_RepairCycle(int Aid)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Cycle_Entity>("Select Aid, Apt_Code, Repair_Plan_Code, Sort_A_Code, Sort_B_Code, Sort_C_Code, Sort_A_Name, Sort_B_Name, Sort_C_Name, Repair_Article_Code, Division, Law_Repair_Cycle_All, Set_Repair_Cycle_All, Law_Repair_Cycle_Part, Set_Repair_Cycle_Part, Law_Repair_Rate, Set_Repair_Rate, Repair_Last_Year_All, Repair_Last_Year_Part, Repair_Plan_Year_All, Repair_Plan_Year_Part, All_Cycle_Num, Part_Cycle_Num, Repair_Cycle_Etc, Repair_Plan_Complete, User_ID, Post_Date From Repair_Cycle Where Aid = @Aid", new { Aid }, commandType: CommandType.Text);
            }
            //return ff.ctx.Query<Repair_Cycle>("Select Aid, Apt_Code, Repair_Plan_Code, Sort_A_Code, Sort_B_Code, Sort_C_Code, Sort_A_Name, Sort_B_Name, Sort_C_Name, Repair_Article_Code, Division, Law_Repair_Cycle_All, Set_Repair_Cycle_All, Law_Repair_Cycle_Part, Set_Repair_Cycle_Part, Law_Repair_Rate, Set_Repair_Rate, Repair_Last_Year_All, Repair_Last_Year_Part, Repair_Plan_Year_All, Repair_Plan_Year_Part, All_Cycle_Num, Part_Cycle_Num, Repair_Cycle_Etc, Repair_Plan_Complete, User_ID, Post_Date From Repair_Cycle Where Aid = @Aid", new { Aid });
        }

        // 수선주기 상세(수선항목)
        public async Task<Cycle_Entity> Detail_RepairCycle_Article(string Repair_Plan_Code, string Repair_Article_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Cycle_Entity>("Select Top 1 Aid, Apt_Code, Repair_Plan_Code, Sort_A_Code, Sort_B_Code, Sort_C_Code, Sort_A_Name, Sort_B_Name, Sort_C_Name, Repair_Article_Code, Division, Law_Repair_Cycle_All, Set_Repair_Cycle_All, Law_Repair_Cycle_Part, Set_Repair_Cycle_Part, Law_Repair_Rate, Set_Repair_Rate, Repair_Last_Year_All, Repair_Last_Year_Part, Repair_Plan_Year_All, Repair_Plan_Year_Part, All_Cycle_Num, Part_Cycle_Num, Repair_Cycle_Etc, Repair_Plan_Complete, User_ID, Post_Date From Repair_Cycle Where Repair_Plan_Code = @Repair_Plan_Code And Repair_Article_Code = @Repair_Article_Code Order By Aid Desc", new { Repair_Plan_Code, Repair_Article_Code }, commandType: CommandType.Text);
            }
            //return ff.ctx.Query<Repair_Cycle>("Select Top 1 Aid, Apt_Code, Repair_Plan_Code, Sort_A_Code, Sort_B_Code, Sort_C_Code, Sort_A_Name, Sort_B_Name, Sort_C_Name, Repair_Article_Code, Division, Law_Repair_Cycle_All, Set_Repair_Cycle_All, Law_Repair_Cycle_Part, Set_Repair_Cycle_Part, Law_Repair_Rate, Set_Repair_Rate, Repair_Last_Year_All, Repair_Last_Year_Part, Repair_Plan_Year_All, Repair_Plan_Year_Part, All_Cycle_Num, Part_Cycle_Num, Repair_Cycle_Etc, Repair_Plan_Complete, User_ID, Post_Date From Repair_Cycle Where Repair_Plan_Code = @Repair_Plan_Code And Repair_Article_Code = @Repair_Article_Code Order By Aid Desc", new { Repair_Plan_Code, Repair_Article_Code });
        }

        // 장기수선계획 조정 생성 시 일괄 저장
        public async Task All_Insert_Code(string Apt_Code, string Repair_Plan_Code_A, string Repair_Plan_Code_B, string Repair_Article_Code, string User_ID, string PostIP)
        {
            var sql = "Insert Into Repair_Cycle (Apt_Code, Repair_Plan_Code, Sort_A_Code, Sort_B_Code, Sort_C_Code, Repair_Article_Code, Division, Law_Repair_Cycle_All, Set_Repair_Cycle_All, Law_Repair_Cycle_Part, Set_Repair_Cycle_Part, Law_Repair_Rate, Set_Repair_Rate, Repair_Last_Year, Repair_Plan_Year_All, Repair_Plan_Year_Part, All_Cycle_Num, Part_Cycle_Num, Repair_Cycle_Etc, User_ID, Post_IP) Select Apt_Code, @Repair_Plan_Code_B, Sort_A_Code, Sort_B_Code, Sort_C_Code, @Repair_Article_Code, Division, Law_Repair_Cycle_All, Set_Repair_Cycle_All, Law_Repair_Cycle_Part, Set_Repair_Cycle_Part, Law_Repair_Rate, Set_Repair_Rate, Repair_Last_Year, Repair_Plan_Year_All, Repair_Plan_Year_Part, All_Cycle_Num, Part_Cycle_Num, Repair_Cycle_Etc, @User_ID, @PostIP From Repair_Cycle Where Apt_Code = @Apt_Code And Repair_Plan_Code = @Repair_Plan_Code_A";

            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, new { Apt_Code, Repair_Plan_Code_A, Repair_Plan_Code_B, Repair_Article_Code, User_ID, PostIP }, commandType: CommandType.Text);
            }

            //ff.ctx.Execute(sql, new { Apt_Code, Repair_Plan_Code_A, Repair_Plan_Code_B, Repair_Article_Code, User_ID, PostIP });
        }

        // 해당 공동주택 계획별 부분 수선항목 수
        public async Task<int> Repair_Part_Num(string Apt_Code, string Repair_Plan_Code)
        {
            var sql = "SELECT Count(*) FROM Repair_Cycle As a Join Repair_Article As b On a.Repair_Article_Code = b.Aid And a.Apt_Code = b.Apt_Code And a.Repair_Plan_Code = b.Repair_Plan_Code Where a.Apt_Code = @Apt_Code And a.Repair_Plan_Code = @Repair_Plan_Code And a.Set_Repair_Cycle_Part > 0 And a.Set_Repair_Rate > 0";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>(sql, new { Apt_Code, Repair_Plan_Code }, commandType: CommandType.Text);
            }
            //return ff.ctx.Query<int>(sql, new { Apt_Code, Repair_Plan_Code });
        }

        // 해당 공동주택 계획별 전체 수선항목 수
        public async Task<int> Repair_All_Num(string Apt_Code, string Repair_Plan_Code)
        {
            //var sql = "SELECT Count(*) FROM Repair_Cycle As a Join Repair_Cost As b On a.Repair_Article_Code = b.Repair_Article_Code And a.Apt_Code = b.Apt_Code And a.Repair_Plan_Code = b.Repair_Plan_Code Where a.Apt_Code = @Apt_Code And a.Repair_Plan_Code = @Repair_Plan_Code And a.Set_Repair_Cycle_All > 0 And b.Repair_All_Cost > 0";
            var sql = "SELECT Count(*) FROM Repair_Cycle As a Join Repair_Article As b On a.Repair_Article_Code = b.Aid And a.Apt_Code = b.Apt_Code And a.Repair_Plan_Code = b.Repair_Plan_Code Where a.Apt_Code = @Apt_Code And a.Repair_Plan_Code = @Repair_Plan_Code And a.Set_Repair_Cycle_All > 0";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>(sql, new { Apt_Code, Repair_Plan_Code }, commandType: CommandType.Text);
            }
            //return ff.ctx.Query<int>(sql, new { Apt_Code, Repair_Plan_Code });
        }

        /// <summary>
        /// 해당 수선항목 코드로 수선주기 존재 여부 확인
        /// </summary>
        public async Task<int> Being_Article_Cycle(string Apt_Code, string Repair_Plan_Code, string Repair_Article_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Repair_Cycle Where Apt_Code = @Apt_Code And Repair_Plan_Code = @Repair_Plan_Code And Repair_Article_Code = @Repair_Article_Code", new { Apt_Code, Repair_Plan_Code, Repair_Article_Code }, commandType: CommandType.Text);
            }
            //return ff.ctx.Query<int>("Select Count(*) From Repair_Cycle Where Apt_Code = @Apt_Code And Repair_Plan_Code = @Repair_Plan_Code And Repair_Article_Code = @Repair_Article_Code", new { Apt_Code, Repair_Plan_Code, Repair_Article_Code });
        }

        /// <summary>
        /// 수선주기 테이블에서 공동주택 식별코드 그리고 계획식별코드, 수선항목 식별코드로 첫번째 수선주기 식별코드 불러오기
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="Repair_Plan_Code"></param>
        /// <param name="Repair_Article_Code"></param>
        /// <returns></returns>
        public async Task<int> Being_Article_Cycle_Code(string Apt_Code, string Repair_Plan_Code, string Repair_Article_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Top 1 Aid From Repair_Cycle Where Apt_Code = @Apt_Code And Repair_Plan_Code = @Repair_Plan_Code And Repair_Article_Code = @Repair_Article_Code Order By Aid Desc", new { Apt_Code, Repair_Plan_Code, Repair_Article_Code }, commandType: CommandType.Text);
            }
            //return ff.ctx.Query<int>("Select Top 1 Aid From Repair_Cycle Where Apt_Code = @Apt_Code And Repair_Plan_Code = @Repair_Plan_Code And Repair_Article_Code = @Repair_Article_Code Order By Aid Desc", new { Apt_Code, Repair_Plan_Code, Repair_Article_Code });
        }

        /// <summary>
        /// 장기수선계획, 수선항목, 수선주기를 조인하여 장기수선계획코드 그리고 공사종별코드, 수선항목명으로 첫번째 모든 정보 불러오기
        /// </summary>
        public async Task<Join_Article_Cycle_Cost_EntityA> Detail_Cycle_Article(string Repair_Plan_Code, string Sort_C_Code, string Repair_Article_Name)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Join_Article_Cycle_Cost_EntityA>("Select Top 1 * From Repair_Cycle As a Join Repair_Article As b On a.Repair_Plan_Code = b.Repair_Plan_Code And a.Repair_Article_Code = b.Aid Where a.Repair_Plan_Code = @Repair_Plan_Code and b.Repair_Plan_Code = @Repair_Plan_Code And a.Sort_C_Code = @Sort_C_Code And b.Repair_Article_Name = @Repair_Article_Name Order By a.Aid Desc", new { Repair_Plan_Code, Sort_C_Code, Repair_Article_Name }, commandType: CommandType.Text);
            }
            //return ff.ctx.Query<Join_Article_Cycle_Cost_Entity>("Select Top 1 * From Repair_Cycle As a Join Repair_Article As b On a.Repair_Plan_Code = b.Repair_Plan_Code And a.Repair_Article_Code = b.Aid Where a.Repair_Plan_Code = @Repair_Plan_Code and b.Repair_Plan_Code = @Repair_Plan_Code And a.Sort_C_Code = @Sort_C_Code And b.Repair_Article_Name = @Repair_Article_Name Order By a.Aid Desc", new { Repair_Plan_Code, Sort_C_Code, Repair_Article_Name });
        }

        /// <summary>
        /// 장기수선계획 테이블과 수선주기, 수선항목을 조인하여 수선항목명과 종사종별 코드로 입력된 정보 수 불러오기
        /// </summary>
        public async Task<int> Being_Detail_Cycle_Article(string Repair_Plan_Code, string Sort_C_Code, string Repair_Article_Name)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Repair_Cycle As a Join Repair_Article As b On a.Repair_Plan_Code = b.Repair_Plan_Code And a.Repair_Article_Code = b.Aid Where a.Repair_Plan_Code = @Repair_Plan_Code and b.Repair_Plan_Code = @Repair_Plan_Code And a.Sort_C_Code = @Sort_C_Code And b.Repair_Article_Name = @Repair_Article_Name", new { Repair_Plan_Code, Sort_C_Code, Repair_Article_Name }, commandType: CommandType.Text);
            }
            //return ff.ctx.Query<int>("Select Count(*) From Repair_Cycle As a Join Repair_Article As b On a.Repair_Plan_Code = b.Repair_Plan_Code And a.Repair_Article_Code = b.Aid Where a.Repair_Plan_Code = @Repair_Plan_Code and b.Repair_Plan_Code = @Repair_Plan_Code And a.Sort_C_Code = @Sort_C_Code And b.Repair_Article_Name = @Repair_Article_Name", new { Repair_Plan_Code, Sort_C_Code, Repair_Article_Name });
        }

        // 해당 장기수선계획 입력된 수선주기 수
        public async Task<int> Cycle_Num(string Apt_Code, string Repair_Plan_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Repair_Cycle as a Join Repair_Article as b on a.Repair_Article_Code = b.Aid  Where a.Apt_Code = @Apt_Code And b.Repair_Plan_Code = @Repair_Plan_Code", new { Apt_Code, Repair_Plan_Code }, commandType: CommandType.Text);
            }
            //return ff.ctx.Query<int>("Select Count(*) From Repair_Cycle as a Join Repair_Article as b on a.Repair_Article_Code = b.Aid  Where a.Apt_Code = @Apt_Code And b.Repair_Plan_Code = @Repair_Plan_Code", new { Apt_Code, Repair_Plan_Code });
        }

        /// <summary>
        /// 수선항목 코드로 수선주기 존재여부 확인
        /// </summary>
        public async Task<int> Being_Cycle_Article_Code(string Repair_Plan_Code, string Repair_Article_Code)
        {
            using (SqlConnection ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Repair_Cycle Where Repair_Plan_Code = @Repair_Plan_Code And Repair_Article_Code = @Repair_Article_Code", new { Repair_Plan_Code, Repair_Article_Code }, commandType: CommandType.Text);
            }
            //return ff.ctx.Query<int>("Select Count(*) From Repair_Cycle Where Repair_Plan_Code = @Repair_Plan_Code And Repair_Article_Code = @Repair_Article_Code", new { Repair_Plan_Code, Repair_Article_Code });
        }

        /// <summary>
        /// 수선항목 코드 불러오기
        /// </summary>
        /// <returns></returns>
        public async Task<string> OnArticleCode(string Repair_Plan_Code, string Sort_C_Code, string Repair_Article_Name)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<string>("Select Top 1 Repair_Article_Code From Repair_Cycle As a Join Repair_Article As b On a.Repair_Plan_Code = b.Repair_Plan_Code And a.Repair_Article_Code = b.Aid Where a.Repair_Plan_Code = @Repair_Plan_Code and b.Repair_Plan_Code = @Repair_Plan_Code And a.Sort_C_Code = @Sort_C_Code And b.Repair_Article_Name = @Repair_Article_Name Order By a.Aid Desc", new { Repair_Plan_Code, Sort_C_Code, Repair_Article_Name });
        }

        /// <summary>
        /// 수선주기 이전 리스트
        /// </summary>
        public async Task<List<Cycle_Entity>> GetList_Cycle_Ago(string Apt_Code, string Now_Code, string Ago_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aa = await ctx.QueryAsync<Cycle_Entity>("Select a.All_Cycle_Num, a.Division, a.Part_Cycle_Num, a.Repair_Article_Code, a.Repair_Cycle_Etc, a.Repair_Plan_Year_All, a.Law_Repair_Cycle_All, a.Law_Repair_Cycle_Part, a.Law_Repair_Rate, a.Repair_Plan_Code, a.Repair_Plan_Year_All, a.Repair_Plan_Year_Part, a.Set_Repair_Cycle_All, a.Set_Repair_Cycle_Part, a.Set_Repair_Rate, a.Sort_A_Code, a.Sort_A_Name, a.Sort_B_Code, a.Sort_B_Name, a.Sort_C_Code, a.Sort_B_Name, b.Repair_Article_Name, b.All_Cycle, b.Part_Cycle, b.Repair_Rate From Repair_Cycle as a Join Repair_Article as b on a.Repair_Article_Code = b.Aid Where a.Apt_Code = @Apt_Code And a.Repair_Plan_Code = @Ago_Code And b.Repair_Article_Name = (Select Repair_Article_Name From Repair_Article Where Apt_Code = @Apt_Code And Repair_Plan_Code = @Now_Code And Repair_Article_Name = b.Repair_Article_Name) Order By a.Sort_A_Code Asc, Convert(int, a.Sort_B_Code) Asc, Convert(int, a.Sort_C_Code) Asc", new { Apt_Code, Now_Code, Ago_Code });
                return aa.ToList();

            }
        }
    }
}