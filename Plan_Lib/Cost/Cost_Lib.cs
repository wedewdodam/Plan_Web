using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Plan_Blazor_Lib.Plan;
using Plan_Blazor_Lib.Price;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Plan_Blazor_Lib.Cost
{
    public class Cost_Lib : ICost_Lib
    {
        private readonly IConfiguration _db;

        public Cost_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 수선금액 입력하기
        /// </summary>        
        public async Task<Cost_Entity> Add_Cost(Cost_Entity Cost)
        {
            var sql = "Insert Into Repair_Cost (Repair_Cost_CodeA, Apt_Code, Repair_Plan_Code, Sort_A_Code, Sort_B_Code, Sort_C_Code, Sort_A_Name, Sort_B_Name, Sort_C_Name, Repair_Article_Code, Price_Sort, Repair_Amount, Repair_All_Cost, Repair_Part_Cost, Repair_Rate, Staff_Code, Cost_Etc) Values (@Repair_Cost_CodeA, @Apt_Code, @Repair_Plan_Code, @Sort_A_Code, @Sort_B_Code, @Sort_C_Code, @Sort_A_Name, @Sort_B_Name, @Sort_C_Name, @Repair_Article_Code, @Price_Sort, @Repair_Amount, @Repair_All_Cost, @Repair_Part_Cost, @Repair_Rate, @Staff_Code, @Cost_Etc); Select Cast(SCOPE_IDENTITY() As Int);";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var Aid = await ctx.QuerySingleOrDefaultAsync<int>(sql, Cost);
                Cost.Repair_Cost_Code = Aid;
                return Cost;
            }
            
        }

        /// <summary>
        /// 수선금액 수정하기
        /// </summary>
        public async Task<Cost_Entity> Update_Cost(Cost_Entity Cost)
        {
            var sql = "Update Repair_Cost Set Price_Sort = @Price_Sort, Repair_Amount = @Repair_Amount, Repair_All_Cost = @Repair_All_Cost, Repair_Part_Cost = @Repair_Part_Cost, Repair_Rate = @Repair_Rate, Staff_Code = @Staff_Code, Cost_Etc = @Cost_Etc, PostDate = getdate() Where Repair_Cost_Code = @Repair_Cost_Code";
            using var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            await ctx.ExecuteAsync(sql, Cost);
            return Cost;
        }

        /// <summary>
        /// 수선금액 수정하기
        /// </summary>
        public async Task<Cost_Entity> Update_Cost_A(Cost_Entity Cost)
        {
            var sql = "Update Repair_Cost Set Repair_Amount = @Repair_Amount, Repair_All_Cost = @Repair_All_Cost, Repair_Part_Cost = @Repair_Part_Cost, Repair_Rate = @Repair_Rate, PostDate = getdate() Where Repair_Cost_Code = @Repair_Cost_Code";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, Cost);
                return Cost;
            }
            
        }

        /// <summary>
        /// 부분수선금액과 부분수선율만 수정하기
        /// </summary>
        /// <param name="Price_Code"></param>
        public async Task Edit_Part_Price(double Repair_Part_Cost, double Repair_Rate, int Repair_Cost_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync("Update Repair_Cost Set Repair_Part_Cost = @Repair_Part_Cost, Repair_Rate = @Repair_Rate, PostDate = getdate() Where Repair_Cost_Code = @Repair_Cost_Code", new { Repair_Part_Cost, Repair_Rate, Repair_Cost_Code }, commandType: CommandType.Text);
            }
            
        }

        /// <summary>
        ///  수선금액 리스트(수선항목 코드)
        /// </summary>
        /// <param name="코드"></param>
        /// <returns></returns>
        public async Task<Cost_Entity> GetDetail_Cost_RAC(string Apt_Code, string Repair_Plan_Code, string Repair_Article_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Cost_Entity>("Select Repair_Cost_Code, Repair_Cost_CodeA, Apt_Code, Repair_Plan_Code, Sort_A_Code, Sort_B_Code, Sort_C_Code, Sort_A_Name, Sort_B_Name, Sort_C_Name, Repair_Article_Code, Price_Sort, Repair_Amount, Repair_All_Cost, Repair_Part_Cost, Repair_Rate, Cost_Etc From Repair_Cost Where Apt_Code = @Apt_Code And Repair_Plan_Code = @Repair_Plan_Code And Repair_Article_Code = @Repair_Article_Code", new { Apt_Code, Repair_Plan_Code, Repair_Article_Code }, commandType: CommandType.Text);
            }
            
        }

        /// <summary>
        /// 수선금액 리스트(시설물 분류 코드)
        /// </summary>
        public async Task<List<Join_Article_Cycle_Cost_EntityA>> GetLIst_RepairCost_Sort(string Repair_Plan_Code, string Sort_Field, string Sort_Code, string Apt_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aa = await ctx.QueryAsync<Join_Article_Cycle_Cost_EntityA>("SELECT a.Apt_Code, a.Cost_Etc, a.PostDate, a.Price_Sort, a.Repair_All_Cost, a.Repair_Amount, a.Repair_Article_Code, a.Repair_Cost_Code, a.Repair_Cost_CodeA, a.Repair_Part_Cost, a.Repair_Plan_Code, a.Repair_Rate, a.Sort_A_Code, a.Sort_A_Name, a.Sort_B_Code, a.Sort_B_Name, a.Sort_C_Code, a.Sort_C_Name, a.Staff_Code, b.Amount From Repair_Cost as a Join Repair_Article as b on a.Repair_Article_Code = b.Aid Where a.Apt_Code = @Apt_Code and b.Apt_Code = @Apt_Code And a.Repair_Plan_Code = @Repair_Plan_Code and b.Repair_Plan_Code = @Repair_Plan_Code and a." + Sort_Field + " = @Sort_Code and b." + Sort_Field + " = @Sort_Code and a.Repair_Article_Code = b.Aid Order By a.Sort_A_Code Asc, Convert(int, a.Sort_B_Code) Asc, Convert(int, a.Sort_C_Code) Asc", new { Repair_Plan_Code, Sort_Field, Sort_Code, Apt_Code }, commandType: CommandType.Text);
                return aa.ToList();
                
            }
        }

        /// <summary>
        /// 수선금액 리스트(시설물 분류 코드)
        /// </summary>
        public async Task<List<Join_Article_Cycle_Cost_EntityA>> GetLIst_RepairCost_Sort_New(string Repair_Plan_Code, string Sort_Field, string Sort_Code, string Apt_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aa = await ctx.QueryAsync<Join_Article_Cycle_Cost_EntityA>("SELECT a.Apt_Code, a.Cost_Etc, a.PostDate, a.Price_Sort, a.Repair_All_Cost, a.Repair_Amount, a.Repair_Article_Code, a.Repair_Cost_Code, a.Repair_Cost_CodeA, a.Repair_Part_Cost, a.Repair_Plan_Code, a.Repair_Rate, a.Sort_A_Code, a.Sort_A_Name, a.Sort_B_Code, a.Sort_B_Name, a.Sort_C_Code, a.Sort_C_Name, a.Staff_Code, b.Amount, c.Part_Cycle_Num, c.All_Cycle_Num, b.Repair_Article_Name, b.Unit, c.Repair_Plan_Complete, c.Part_Cycle_Num, c.All_Cycle_Num, b.Amount, c.Set_Repair_Cycle_All, c.Set_Repair_Cycle_Part, c.Set_Repair_Rate, c.Law_Repair_Cycle_All, c.Law_Repair_Cycle_Part, b.All_Cycle, b.Amount, b.Part_Cycle, b.Cost_Count, b.Cycle_Count, c.Repair_Plan_Year_All, c.Repair_Plan_Year_Part From Repair_Cost as a Join Repair_Article as b on a.Repair_Article_Code = b.Aid Join Repair_Cycle as c On a.Repair_Article_Code = c.Repair_Article_Code Where a.Apt_Code = @Apt_Code and b.Apt_Code = @Apt_Code And c.Apt_Code = @Apt_Code And a.Repair_Plan_Code = @Repair_Plan_Code and b.Repair_Plan_Code = @Repair_Plan_Code And C.Apt_Code = @Apt_Code and a." + Sort_Field + " = @Sort_Code and b." + Sort_Field + " = @Sort_Code And c." + Sort_Field + " = @Sort_Code and a.Repair_Article_Code = b.Aid And c.Repair_Article_Code = a.Repair_Article_Code Order By a.Sort_A_Code Asc, Convert(int, a.Sort_B_Code) Asc, Convert(int, a.Sort_C_Code) Asc", new { Apt_Code, Repair_Plan_Code, Sort_Field, Sort_Code }, commandType: CommandType.Text);
                return aa.ToList();
            }
            
        }

        /// <summary>
        /// 수선금액 리스트(시설물 분류 코드)
        /// </summary>
        public async Task<List<Join_Article_Cycle_Cost_EntityA>> GetLIst_RepairCost_Sort_A(string Repair_Plan_Code, string Sort_Field, string Sort_Code, string Apt_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aa = await ctx.QueryAsync<Join_Article_Cycle_Cost_EntityA>("Repair_Cost_Sort_A", new { Repair_Plan_Code, Sort_Field, Sort_Code, Apt_Code }, commandType: CommandType.StoredProcedure);
                return aa.ToList();
            }
            
        }

        /// <summary>
        /// 수선항목 명으로 수선금액(전체, 부분) 가져오기 (직전)
        /// </summary>
        public async Task<Cost_Entity> _Cost(string Apt_Code, string Repair_Article_Name)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Cost_Entity>("Select Top 1 a.Repair_All_Cost, a.Repair_Part_Cost, a.Repair_Rate From Repair_Cost as a Join Repair_Article as b on a.Repair_Article_Code = b.Aid Where b.Repair_Article_Name = @Repair_Article_Name And a.Apt_Code = @Apt_Code And b.Apt_Code = @Apt_Code Order By Repair_Cost_Code Desc", new { Apt_Code, Repair_Article_Name }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 존재여부(이전 수선금액)
        /// </summary>
        public async Task<int> beCost(string Apt_Code, string Repair_Article_Name)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Repair_Cost as a Join Repair_Article as b on a.Repair_Article_Code = b.Aid Where b.Repair_Article_Name = @Repair_Article_Name And a.Apt_Code = @Apt_Code", new { Apt_Code, Repair_Article_Name }, commandType: CommandType.Text);
            }
            
        }

        /// <summary>
        /// 입력이 완료된 해당 수선항목 수선금액이 1개뿐인 경우
        /// </summary>
        public async Task<Cost_Entity> _Cost_one(string Apt_Code, string Repair_Article_Name)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Cost_Entity>("Select Top 1 a.Repair_All_Cost, a.Repair_Part_Cost, a.Repair_Rate From Repair_Cost as a Join Repair_Article as b on a.Repair_Article_Code = b.Aid Where a.Apt_Code = @Apt_Code And b.Repair_Article_Name = @Repair_Article_Name", new { Apt_Code, Repair_Article_Name }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 수선금액 삭제
        /// </summary>
        public async Task Remove_Repair_Cost(int Repair_Cost_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync("Delete Repair_Cost Where Repair_Cost_Code = @Repair_Cost_Code", new { Repair_Cost_Code }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 해당 장기수선계획 식별코드로 입력된 수선금액 모두 삭제
        /// </summary>
        public async Task Delete_RepairCost_PlanCode(string Repair_Plan_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync("Delete Repair_Cost Where Repair_Plan_Code = @Repair_Plan_Code", new { Repair_Plan_Code }, commandType: CommandType.Text);
            }
            
        }

        /// <summary>
        /// 수선항목 삭제 수선금액 삭제
        /// </summary>
        public async Task Remove_Article_Cost(string Repair_Plan_Code, string Repair_Article_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync("Delete From Repair_Cost Where Repair_Plan_Code = @Repair_Plan_Code And Repair_Article_Code = @Repair_Article_Code", new { Repair_Plan_Code, Repair_Article_Code }, commandType: CommandType.Text);
            }
            
        }

        /// <summary>
        /// 입력 여부 확인(수선항목 기준)
        /// </summary>
        public async Task<int> Cost_Count(string Repair_Plan_Code, string Repair_Article_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Repair_Cost Where Repair_Plan_Code = @Repair_Plan_Code And Repair_Article_Code = @Repair_Article_Code", new { Repair_Plan_Code, Repair_Article_Code }, commandType: CommandType.Text);
            }
            
        }

        /// <summary>
        /// 입력 여부 확인(장기수선계획 기준)
        /// </summary>
        public async Task<int> Being_Cost_Count(string Repair_Plan_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Repair_Cost Where Repair_Plan_Code = @Repair_Plan_Code", new { Repair_Plan_Code }, commandType: CommandType.Text);
            }            
        }

        /// <summary>
        /// 항목별 전체 수선금액 합계
        /// </summary>
        public async Task<int> Cost_Total_Article(string Repair_Plan_Code, string Repair_Article_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Sum(Repair_All_Cost) From Repair_Cost Where Repair_Plan_Code = @Repair_Plan_Code And Repair_Article_Code = @Repair_Article_Code", new { Repair_Plan_Code, Repair_Article_Code }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 전체 수선 금액 합계 구하기
        /// </summary>
        public double Cost_All_Total(string Apt_Code, string Repair_Plan_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return ctx.QuerySingleOrDefault<double>("Select isNull(Sum(a.All_Cycle_Num * b.Repair_All_Cost), 0) From Repair_Cycle a join Repair_Cost b on a.Repair_Plan_Code = b.Repair_Plan_Code and a.Repair_Article_Code = b.Repair_Article_Code Where a.Apt_Code = @Apt_Code And b.Repair_Plan_Code = @Repair_Plan_Code", new { Apt_Code, Repair_Plan_Code }, commandType: CommandType.Text);
            }            
        }

        /// <summary>
        /// 부분 수선 금액 합계 구하기
        /// </summary>
        public double Cost_Part_Total(string Apt_Code, string Repair_Plan_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return ctx.QuerySingleOrDefault<double>("Select isNull(Sum(a.Part_Cycle_Num * b.Repair_Part_Cost), 0) From Repair_Cycle a join Repair_Cost b on a.Repair_Plan_Code = b.Repair_Plan_Code and a.Repair_Article_Code = b.Repair_Article_Code Where a.Apt_Code = @Apt_Code And b.Repair_Plan_Code = @Repair_Plan_Code", new { Apt_Code, Repair_Plan_Code }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 전체 수선 금액 분류 합계 구하기
        /// </summary>
        public async Task<double> Cost_All_Total_Sort(string Sort_A_Code, string Apt_Code, string Repair_Plan_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<double>("Select isNull(Sum(a.All_Cycle_Num * b.Repair_All_Cost), 0) From Repair_Cycle a join Repair_Cost b on a.Repair_Plan_Code = b.Repair_Plan_Code and a.Repair_Article_Code = b.Repair_Article_Code Where a.Sort_A_Code = @Sort_A_Code", new { Sort_A_Code }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 전체 수선 금액 분류별 합계 구하기
        /// </summary>
        public async Task<double> Cost_All_Total_Sort_ABC(string Feild, string Query, string Apt_Code, string Repair_Plan_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<double>("Select isNull(Sum(a.All_Cycle_Num * b.Repair_All_Cost), 0) From Repair_Cycle a join Repair_Cost b on a.Repair_Plan_Code = b.Repair_Plan_Code and a.Repair_Article_Code = b.Repair_Article_Code Where a." + Feild + " = @Query And b.Apt_Code = @Apt_Code And b.Repair_Plan_Code = @Repair_Plan_Code", new { Feild, Query, Apt_Code, Repair_Plan_Code }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 부분 수선 금액 분류별 합계 구하기
        /// </summary>
        public async Task<double> Cost_Part_Total_Sort_ABC(string Feild, string Query, string Apt_Code, string Repair_Plan_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<double>("Select isNull(Sum(a.All_Cycle_Num * b.Repair_All_Cost), 0) From Repair_Cycle a join Repair_Cost b on a.Repair_Plan_Code = b.Repair_Plan_Code and a.Repair_Article_Code = b.Repair_Article_Code Where a." + Feild + " = @Query And b.Apt_Code = @Apt_Code And b.Repair_Plan_Code = @Repair_Plan_Code", new { Feild, Query, Apt_Code, Repair_Plan_Code }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 해당 장기수선계획에 수선금액이 등록된 수
        /// </summary>
        public async Task<int> Cost_Complete_Article_All(string Apt_Code, string Repair_Plan_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Repair_Cost Where Apt_Code = @Apt_Code And Repair_Plan_Code = @Repair_Plan_Code", new { Apt_Code, Repair_Plan_Code }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 해당 장기수선계획 중에 입력된 분류별 수선금액이 등록된 수
        /// </summary>
        public async Task<int> Cost_Complete_Article_Sort(string Apt_Code, string Repair_Plan_Code, string Feild, string Query)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Repair_Cost Where " + Feild + " = @Query And Apt_Code = @Apt_Code And Repair_Plan_Code = @Repair_Plan_Code", new { Apt_Code, Repair_Plan_Code, Feild, Query }, commandType: CommandType.Text);
            }
            
        }

        /// <summary>
        /// 부분 수선 금액 분류 합계 구하기
        /// </summary>
        public async Task<double> Cost_Part_Total_Sort(string Sort_A_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select isNull(Sum(a.Part_Cycle_Num * b.Repair_Part_Cost), 0) From Repair_Cycle a join Repair_Cost b on a.Repair_Plan_Code = b.Repair_Plan_Code and a.Repair_Article_Code = b.Repair_Article_Code Where a.Sort_A_Code = @Sort_A_Code", new { Sort_A_Code }, commandType: CommandType.Text);
            }
            
        }

        /// <summary>
        /// 계획 식별코드와 수선항목 식별코드로 가장 최근에 입력된 수선금액 식별코드 불러오기
        /// </summary>
        public async Task<string> GetAid_Division(string Repair_Plan_Code, string Repair_Article_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<string>("Select Top 1 isNull(Repair_Cost_Code, 'A') As Repair_Cost_Code From Repair_Cost Where Repair_Plan_Code = @Repair_Plan_Code And Repair_Article_Code = @Repair_Article_Code Order By Repair_Cost_Code Desc", new { Repair_Plan_Code, Repair_Article_Code }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 입력된 식별코드
        /// </summary>
        public async Task<int> GetAid_Last()
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Top 1 Repair_Cost_Code From Repair_Cost Order By Repair_Cost_Code Desc", commandType: CommandType.Text);
            }
            
        }

        /// <summary>
        /// 장기수선계획 정기 조정 시 일괄 저장 메서드
        /// </summary>        
        public async Task All_Insert_Code(string Apt_Code, string Repair_Plan_Code_A, string Repair_Plan_Code_B, string Staff_Code)
        {
            var sql = "Insert Into Repair_Cost (Repair_Cost_CodeA, Apt_Code, Repair_Plan_Code, Sort_A_Code, Sort_B_Code, Sort_C_Code, Repair_Article_Code, Price_Sort, Repair_Amount, Repair_All_Cost, Repair_Part_Cost, Repair_Rate, Staff_Code, Cost_Etc) Select Repair_Cost_CodeA, Apt_Code, @Repair_Plan_Code_B, Sort_A_Code, Sort_B_Code, Sort_C_Code, Repair_Article_Code, Price_Sort, Repair_Amount, Repair_All_Cost, Repair_Part_Cost, Repair_Rate, @Staff_Code, Cost_Etc From Repair_Cost Where Apt_Code = @Apt_Code And Repair_Plan_Code = @Repair_Plan_Code_A";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, new { Apt_Code, Repair_Plan_Code_A, Repair_Plan_Code_B, Staff_Code }, commandType: CommandType.Text);
            }
            
        }

        /// <summary>
        /// 이전 계획에서 수선금액 정보 불러오기
        /// </summary>        
        public async Task<Cost_Entity> Cost_Detail_ArticleCode(string Apt_Code, string Repair_Article_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Cost_Entity>("Select Top 1 * From Repair_Cost As a Join Repair_Article As b On a.Repair_Article_Code = b.Aid Where a.Apt_Code = @Apt_Code And b.Apt_Code = @Apt_Code And b.Aid = @Repair_Article_Code Order By a.Repair_Cost_Code Desc", new { Apt_Code, Repair_Article_Code }, commandType: CommandType.Text);
            }            
        }

        /// <summary>
        /// 이전 계획에서 수선금액 정보 있는지 확인
        /// </summary>
        public async Task<int> Being_Cost_Detail_ArticleCode(string Apt_Code, string Repair_Article_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Repair_Cost As a Join Repair_Article As b On a.Repair_Article_Code = b.Aid Where a.Apt_Code = @Apt_Code And b.Aid = @Repair_Article_Code", new { Apt_Code, Repair_Article_Code }, commandType: CommandType.Text);
            }
            
        }

        /// <summary>
        /// 수선금액 일괄저장을 위한 이전 직전 장기수선계획 수선금액 정보 불러오기(17/2/4)
        /// </summary>
        public async Task<Join_Article_Cycle_Cost_EntityA> Detail_Cycle_Cost(string Ago_Repair_Plan_Code, string Sort_C_Code, string Repair_Article_Name)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Join_Article_Cycle_Cost_EntityA>("Select Top 1 * From Repair_Cost As a Join Repair_Article As b On a.Repair_Plan_Code = b.Repair_Plan_Code And a.Repair_Article_Code = b.Aid Where a.Repair_Plan_Code = @Ago_Repair_Plan_Code And a.Sort_C_Code = @Sort_C_Code And b.Repair_Article_Name = @Repair_Article_Name Order By b.Aid Desc", new { Ago_Repair_Plan_Code, Sort_C_Code, Repair_Article_Name }, commandType: CommandType.Text);
            }
            
        }

        /// <summary>
        /// 해당 장기수선계획의 수선항목, 수선주기, 수선금액 정보 모두를 불러오기(17/7/14)
        /// </summary>
        public async Task<Join_Article_Cycle_Cost_EntityA> Detail_Artile_Cycle_Cost(string Apt_Code, string Repair_Plan_Code, string Repair_Article_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Join_Article_Cycle_Cost_EntityA>("Select Top 1 * From Repair_Cost As a Join Repair_Article As b On a.Repair_Plan_Code = b.Repair_Plan_Code And a.Repair_Article_Code = b.Aid Join Repair_Cycle as c on b.Aid = c.Repair_Article_Code Where a.Apt_Code = @Apt_Code And a.Repair_Plan_Code = @Repair_Plan_Code And a.Repair_Article_Code = @Repair_Article_Code Order By b.Aid Desc", new { Apt_Code, Repair_Plan_Code, Repair_Article_Code }, commandType: CommandType.Text);
            }
            
        }

        /// <summary>
        /// 해당 장기수선계획 연도별 수선항목, 수선주기, 수선금액 정보 모두를 불러오기
        /// </summary>
        public async Task<Join_Article_Cycle_Cost_EntityA> Detail_Artile_Cycle_Cost_Year(string Apt_Code, string Repair_Plan_Code, string Repair_Article_Code, string Repair_Year)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Join_Article_Cycle_Cost_EntityA>("Select Top 1 * From Repair_Cost As a Join Repair_Article As b On a.Repair_Plan_Code = b.Repair_Plan_Code And a.Repair_Article_Code = b.Aid Join Repair_Cycle as c on b.Aid = c.Repair_Article_Code Where a.Apt_Code = @Apt_Code And a.Repair_Plan_Code = @Repair_Plan_Code And a.Repair_Article_Code = @Repair_Article_Code And c.Repair_Plan_Year_All = @Repair_Year or c.Repair_Plan_Year_Part = @Repair_Year Order By b.Aid Desc", new { Apt_Code, Repair_Plan_Code, Repair_Article_Code, Repair_Year }, commandType: CommandType.Text);
            }
            
        }

        /// <summary>
        /// 해당 공동주택의 수선금액 코드로 모든 정보 불러오기
        /// </summary>
        public async Task<Cost_Entity> Detail_Cost(string Apt_Code, string Repair_Cost_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Cost_Entity>("Select  top 1 * From Repair_Cost Where Apt_Code = @Apt_Code And Repair_Cost_Code = @Repair_Cost_Code Order By Repair_Cost_Code Desc", new { Apt_Code, Repair_Cost_Code }, commandType: CommandType.Text);
            }
           
        }

        /// <summary>
        /// 수선금액 일괄저장을 위한 이전 직전 장기수선계획 수선금액 정보 존재여부 불러오기(17/2/5)
        /// </summary>
        public async Task<int> Detail_Cycle_Cost_Count(string Ago_Repair_Plan_Code, string Sort_C_Code, string Repair_Article_Name)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Repair_Cost As a Join Repair_Article As b On a.Repair_Plan_Code = b.Repair_Plan_Code And a.Repair_Article_Code = b.Aid Where a.Repair_Plan_Code = @Ago_Repair_Plan_Code And a.Sort_C_Code = @Sort_C_Code And b.Repair_Article_Name = @Repair_Article_Name", new { Ago_Repair_Plan_Code, Sort_C_Code, Repair_Article_Name }, commandType: CommandType.Text);
            }
            
        }

        /// <summary>
        /// 해당 수선항목 코드로 수선금액 존재 여부 확인(17/2/4)
        /// </summary>
        public async Task<int> Being_Article_Cost(string Apt_Code, string Repair_Plan_Code, string Repair_Article_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Repair_Cost Where Apt_Code = @Apt_Code And Repair_Plan_Code = @Repair_Plan_Code And Repair_Article_Code = @Repair_Article_Code", new { Apt_Code, Repair_Plan_Code, Repair_Article_Code }, commandType: CommandType.Text);
            }
            
        }

        /// <summary>
        /// 해당 수선항목 코드로 수선금액 존재 여부 확인(17/2/4)
        /// </summary>
        public async Task<int> Being_Article_Cost_Code(string Apt_Code, string Repair_Plan_Code, string Repair_Article_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Top 1 Repair_Cost_Code From Repair_Cost Where Apt_Code = @Apt_Code And Repair_Plan_Code = @Repair_Plan_Code And Repair_Article_Code = @Repair_Article_Code Order By Repair_Cost_Code Desc", new { Apt_Code, Repair_Plan_Code, Repair_Article_Code }, commandType: CommandType.Text);
            }
            
        }

        /// <summary>
        /// 해당 공동주택에 입력된 수선금액 수 구하기
        /// </summary>        
        public async Task<int> Last_Count(string Apt_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Repair_Cost Where Apt_Code = @Apt_Code", new { Apt_Code }, commandType: CommandType.Text);
            }
            //return dnn.ctx.Query<int>("Select Count(*) From Repair_Cost Where Apt_Code = @Apt_Code", new { Apt_Code });
        }

        /// <summary>
        /// 해당 년도 실행 예정 장기수선계획 정보 불러오기
        /// </summary>
        public async Task<List<Join_Article_Cycle_Cost_EntityA>> Plan_Repair(string Apt_Code, string Repair_Plan_Code, string Repair_Year)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aa = await ctx.QueryAsync<Join_Article_Cycle_Cost_EntityA>("Select a.Aid, b.Aid As Cycle_Aid, c.Repair_Cost_Code, Repair_Article_Name, a.Sort_A_Name, a.Sort_B_Name, a.Sort_C_Name, Repair_Plan_Year_All, c.Repair_Article_Code, Repair_Plan_Year_Part, Set_Repair_Rate, Repair_All_Cost, Repair_Part_Cost From Repair_Article As a Join Repair_Cycle As b On a.Aid = b.Repair_Article_Code and a.Repair_Plan_Code = b.Repair_Plan_Code Join Repair_Cost As c On c.Repair_Article_Code = a.Aid Where a.Apt_Code = @Apt_Code And b.Apt_Code = @Apt_Code And a.Repair_Plan_Code = @Repair_Plan_Code And b.Repair_Plan_Code = @Repair_Plan_Code And (b.Repair_Plan_Year_All = @Repair_Year or b.Repair_Plan_Year_Part = @Repair_Year)", new { Apt_Code, Repair_Plan_Code, Repair_Year }, commandType: CommandType.Text);
                return aa.ToList();
            }            
        }

        /// <summary>
        /// 최근 수선금액 구하기
        /// </summary>
        public async Task<Join_Article_Cycle_Cost_EntityA> Detail_Cost_Article_AllCost(string Apt_Code, string Repair_Article_Name)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Join_Article_Cycle_Cost_EntityA>("Select Top 1 a.Repair_All_Cost, a.Repair_Amount From Repair_Cost as a Join Repair_Article as b On a.Repair_Article_Code = b.Aid Where a.Apt_Code = @Apt_Code And b.Apt_Code = @Apt_Code And b.Repair_Article_Name = @Repair_Article_Name Order by a.Repair_Cost_Code Desc", new { Apt_Code, Repair_Article_Name }, commandType: CommandType.Text);
            }            
        }

        /// <summary>
        /// 프린트
        /// </summary>        
        public async Task<List<Join_Article_Cycle_Cost_EntityA>> GetPrint(string Apt_Code, string Repair_Plan_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aa = await ctx.QueryAsync<Join_Article_Cycle_Cost_EntityA>("List_Article_Cycle_Cost", new { Apt_Code, Repair_Plan_Code }, commandType: CommandType.StoredProcedure);
                return aa.ToList();
            }            
        }

        /// <summary>
        /// 계획 수선금액 및 수선년도 불러오기
        /// </summary>
        public async Task<Join_Article_Cycle_Cost_EntityA> Detail_Cost_Article_Cost(string Repair_Plan_Code, string Repair_Article_Code, string Repair_Year)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Join_Article_Cycle_Cost_EntityA>("Select * From Repair_Cost as a Join Repair_Article as b On a.Repair_Article_Code = b.Aid Join Repair_Cycle as c On a.Repair_Article_Code = c.Repair_Article_Code Where a.Apt_Code = b.Apt_Code and a.Repair_Plan_Code = @Repair_Plan_Code And a.Repair_Article_Code = @Repair_Article_Code And (c.Repair_Plan_Year_All = @Repair_Year or c.Repair_Plan_Year_Part = @Repair_Year)", new { Repair_Plan_Code, Repair_Article_Code, Repair_Year }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        ///  해당 장기수선계획에서 수선항목코드로 수선금액 정보 불러오기
        /// </summary>
        public async Task<Cost_Entity> Detail_Cost_Article(string Apt_Code, string Repair_Plan_Code, string Repair_Article_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Cost_Entity>("Select Repair_Cost_Code, Repair_Cost_CodeA, Apt_Code, Repair_Plan_Code, Sort_A_Code, Sort_B_Code, Sort_C_Code, Sort_A_Name, Sort_B_Name, Sort_C_Name, Repair_Article_Code, Price_Sort, Repair_Amount, Repair_All_Cost, Repair_Part_Cost, Repair_Rate, PostDate, Staff_Code, Cost_Etc From Repair_Cost Where Apt_Code = @Apt_Code And Repair_Plan_Code = @Repair_Plan_Code And Repair_Article_Code = @Repair_Article_Code", new { Apt_Code, Repair_Plan_Code, Repair_Article_Code }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 해당공동주택의 수선비 총액 및 단가
        /// </summary>
        public async Task<Report_Plan_Cost_Entity> Detail_Report_Plan_Cost(string Apt_Code, string Repair_Plan_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Report_Plan_Cost_Entity>("Report_Plan_Cost", new { Apt_Code, Repair_Plan_Code }, commandType: CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// 입력된 값에서 단가구분 값 불러오기
        /// </summary>
        public async Task<string> Price_Sort_Detail(string Repair_Article_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<string>("Select ISNULL(Price_Sort, 'z') From Repair_Cost Where Repair_Article_Code = @Repair_Article_Code", new { Repair_Article_Code }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 대분류별 전체 수선비 총액
        /// </summary>
        public async Task<double> Sort_total_All_cost(string Apt_Code, string Repair_Plan_Code, string Sort_A_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<double>("select ISNULL(sum(a.Repair_All_Cost * b.All_Cycle_Num), 0) as All_Cost from Repair_Cost as a Join Repair_Cycle as b on a.Repair_Article_Code = b.Repair_Article_Code Where a.Sort_A_Code = @Sort_A_Code  and a.Apt_Code = @Apt_Code and a.Repair_Plan_Code = @Repair_Plan_Code", new { Apt_Code, Repair_Plan_Code, Sort_A_Code }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 대분류별 부분 수선비 총액
        /// </summary>
        public async Task<double> Sort_total_Part_cost(string Apt_Code, string Repair_Plan_Code, string Sort_A_Code)
        {
            using var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await ctx.QuerySingleOrDefaultAsync<double>("Select ISNULL(sum(a.Repair_Part_Cost * b.Part_Cycle_Num), 0) as Part_Cost from Repair_Cost as a Join Repair_Cycle as b on a.Repair_Article_Code = b.Repair_Article_Code Where a.Sort_A_Code = @Sort_A_Code  and a.Apt_Code = @Apt_Code and a.Repair_Plan_Code = @Repair_Plan_Code", new { Apt_Code, Repair_Plan_Code, Sort_A_Code }, commandType: CommandType.Text);
        }

        /// <summary>
        /// 수선금액이 입력 수선항목 수 구하기(입력된 변수에 따라 전체 혹은 부분)
        /// </summary>
        public int Repair_Cost_Article_All_Count(string Apt_Code, string Repair_Plan_Code, string Field)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return ctx.QuerySingleOrDefault<int>("Select Count(*) From Repair_Cost Where Apt_Code = @Apt_Code And Repair_Plan_Code = @Repair_Plan_Code And " + Field + " > 0", new { Apt_Code, Repair_Plan_Code, Field }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 수선비 총액 등 합계 구하기
        /// </summary>
        public async Task<Plan_Total_Cost_Entity> PlanTotalCost(string Apt_Code, string Repair_Plan_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Plan_Total_Cost_Entity>("Plan_Total_Cost", new { Apt_Code, Repair_Plan_Code }, commandType: CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// 수선금액 리스트(시설물 분류 코드)
        /// </summary>
        public async Task<List<Cost_Entity>> GetList(string Apt_Code, string Repair_Plan_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aa = await ctx.QueryAsync<Cost_Entity>("Select a.Repair_Cost_Code, a.Repair_Cost_CodeA, a.Apt_Code, a.Repair_Plan_Code, a.Sort_A_Code, a.Sort_B_Code, a.Sort_C_Code, a.Sort_A_Name, a.Sort_B_Name, a.Sort_C_Name, a.Repair_Article_Code, a.Price_Sort, a.Repair_Amount, a.Repair_All_Cost, a.Repair_Part_Cost, a.Repair_Rate, a.Cost_Etc, b.Repair_Article_Name, b.Unit, c.All_Cycle_Num, c.Part_Cycle_Num From Repair_Cost as a Join Repair_Article as b on a.Repair_Article_Code = b.Aid Join Repair_Cycle as c on c.Repair_Article_Code = a.Repair_Article_Code Where a.Apt_Code = @Apt_Code And a.Repair_Plan_Code = @Repair_Plan_Code Order By a.Sort_A_Code Asc, Convert(int, a.Sort_B_Code) Asc, Convert(int, a.Sort_C_Code) Asc", new { Apt_Code, Repair_Plan_Code });
                return aa.ToList();

            }
        }

        /// <summary>
        /// 수선금액 리스트(시설물 분류 코드)
        /// </summary>
        public async Task<List<Cost_Entity>> GetList_Cost_Ago(string Apt_Code, string Now_Code, string Ago_Code)
        {
            using var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var aa = await ctx.QueryAsync<Cost_Entity>("Select a.Repair_Cost_Code, a.Repair_Cost_CodeA, a.Apt_Code, a.Repair_Plan_Code, a.Sort_A_Code, a.Sort_B_Code, a.Sort_C_Code, a.Sort_A_Name, a.Sort_B_Name, a.Sort_C_Name, a.Repair_Article_Code, a.Price_Sort, a.Repair_Amount, a.Repair_All_Cost, a.Repair_Part_Cost, a.Repair_Rate, a.Cost_Etc, b.Repair_Article_Name, b.Unit, c.All_Cycle_Num, c.Part_Cycle_Num From Repair_Cost as a Join Repair_Article as b on a.Repair_Article_Code = b.Aid Join Repair_Cycle as c on c.Repair_Article_Code = a.Repair_Article_Code Where a.Apt_Code = @Apt_Code And a.Repair_Plan_Code = @Ago_Code And b.Repair_Article_Name = (Select Repair_Article_Name From Repair_Article Where Apt_Code = @Apt_Code And Repair_Plan_Code = @Now_Code And Repair_Article_Name = b.Repair_Article_Name) Order By a.Sort_A_Code Asc, Convert(int, a.Sort_B_Code) Asc, Convert(int, a.Sort_C_Code) Asc", new { Apt_Code, Now_Code, Ago_Code });
            return aa.ToList();
        }

        /// <summary>
        ///  해당 장기수선계획에서 수선항목코드로 수선금액 정보 불러오기
        /// </summary>
        public async Task<Cost_Entity> Detail_Cost_Article_Year(string Apt_Code, string Repair_Plan_Code, string Repair_Article_Code, string Year)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Cost_Entity>("Select a.Repair_Cost_Code, a.Repair_Cost_CodeA, a.Apt_Code, a.Repair_Plan_Code, a.Sort_A_Code, a.Sort_B_Code, a.Sort_C_Code, a.Sort_A_Name, a.Sort_B_Name, a.Sort_C_Name, a.Repair_Article_Code, a.Price_Sort, a.Repair_Amount, a.Repair_All_Cost, a.Repair_Part_Cost, a.Repair_Rate, a.PostDate, a.Staff_Code, a.Cost_Etc From Repair_Cost as a Join Repair_Cycle as b on a.Repair_Article_Code = b.Repair_Article_Code Where a.Apt_Code = @Apt_Code And a.Repair_Plan_Code = @Repair_Plan_Code And a.Repair_Article_Code = @Repair_Article_Code And (b.Repair_Plan_Year_All = @Year or b.Repair_Plan_Year_Part = @Year)", new { Apt_Code, Repair_Plan_Code, Repair_Article_Code, Year }, commandType: CommandType.Text);
            }
        }
    }
}