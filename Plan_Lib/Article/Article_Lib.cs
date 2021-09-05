using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Plan_Blazor_Lib.Article;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Plan_Blazor_Lib
{
    /// <summary>
    /// 수선항목 인터페이스
    /// </summary>
    public interface IArticle_Lib
    {
        Task<Article_Entity> Add_RepairArticle(Article_Entity model);

        Task<Article_Entity> Edit_RepairArticle(Article_Entity model);

        Task<List<Article_Entity>> GetLIst_RepairArticle(string Apt_Code, string Repair_Plan_Code);

        Task<List<Article_Entity>> GetLIst_Article_All(string Apt_Code, string Repair_Plan_Code);

        Task<List<Article_Entity>> GetLIst_RepairArticle_Sort(string Repair_Plan_Code, string Sort_Field, string Sort_Code, string Apt_Code);

        Task<List<Article_Entity>> GetListArticleSort(string Repair_Plan_Code, string Sort_Field, string Sort_Code, string Apt_Code);

        Task Remove_Repair_Article(int Aid);

        Task Delete_Article_PlanCode(string Repair_Plan_Code);

        Task<Article_Entity> Detail_RepairArticle(string Apt_Code, int Aid);

        Task<string> Sort_A_Name(int Aid);

        Task<string> Sort_B_Name(int Aid);

        Task<string> Sort_C_Name(int Aid);

        Task<string> Article_Name(int Aid);

        Task<string> Article_Unit(int Aid);

        Task<List<Article_Entity>> GetList_Ago(string Apt_Code, string Sort_C_Code, string Adjustment_Date);

        Task<List<Join_Article_Cycle_Cost_Entity>> GetList_CriterionTable(string Apt_Code, string Repair_Plan_Code, string Sort_A_Code);

        int Being_Article_Facility(string Repair_Plan_Code, string Sort_C_Code);

        //Task<int> Being_Article_FacilityQ(string Repair_Plan_Code, string Sort_C_Code);

        //int Being_Article_FacilityQ(string Repair_Plan_Code, string Sort_C_Code);
        Task<int> Being_Article_Code(string Repair_Plan_Code, string Sort_C_Code, string Repair_Article_Name);

        Task<int> Article_Num(string Apt_Code, string Repair_Plan_Code);

        Task<Article_Entity> All_Insert_Code(string Apt_Code, string Repair_Plan_CodeA, string Repair_Plan_CodeB, string User_ID, string PostIP);

        Task<List<Join_Article_Cycle_Cost_Entity>> GetLIst_Repair_Article_Cycle(string Apt_Code, string Repair_Plan_Code);

        Task<List<Join_Article_Cycle_Cost_Entity>> GetLIst_Repair_Article_Cycle_Review(string Apt_Code, string Repair_Plan_Code);

        Task<List<Join_Article_Cycle_Cost_Entity>> GetLIst_Repair_Article_Cycle_Sort_Review(string Repair_Plan_Code, string Sort_Field, string Sort_Code, string Apt_Code);

        /// <summary>
        /// 수선항목 리스트(장기수선계획별, 시설물 분류별)(수선항목과 수선주기의 수선항목 코드가 같은 경우)
        /// </summary>
        Task<List<Join_Article_Cycle_Cost_Entity>> GetList_Repair_Article_Cycle_Sort(string Repair_Plan_Code, string Sort_Field, string Sort_Code, string Apt_Code);

        Task<string> Repair_Cost_Amount(string Article_Code);

        int Being_Article_FacilityQ(string plan_Code, string code);

        Task<Join_Article_Cycle_Cost_Entity> Details_Repair_Article_Cycle_Sort_Review(string Repair_Plan_Code, string Aid, string Apt_Code);

        Task<int> Being_Article(string Repair_Plan_Code, string Sort_C_Code, string Repair_Article_Name);
        Task Update_Cycle_Count_Add(int Aid);
        Task Update_Cycle_Count_Minus(int Aid);
        Task Update_Cost_Count_Add(int Aid);
        Task Update_Cost_Count_Minus(int Aid);
    }

    /// <summary>
    /// 수선항목 라이브러리
    /// </summary>
    public class Article_Lib : IArticle_Lib
    {
        private readonly IConfiguration _db;

        public Article_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 수선항목 입력
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Article_Entity> Add_RepairArticle(Article_Entity model)
        {
            var sql = "Insert Repair_Article (Apt_Code, Repair_Plan_Code, Facility_Code, Sort_A_Code, Sort_B_Code, Sort_C_Code, Sort_A_Name, Sort_B_Name, Sort_C_Name, Repair_Article_Name, Division, Amount, Unit, All_Cycle, Part_Cycle, Repair_Rate, Installation, Installation_Part, Repair_Article_Etc, User_ID, PostIP) Values (@Apt_Code, @RePair_Plan_Code, @Facility_Code, @Sort_A_Code, @Sort_B_Code, @Sort_C_Code, @Sort_A_Name, @Sort_B_Name, @Sort_C_Name, @Repair_Article_Name, @Division, @Amount, @Unit, @All_Cycle, @Part_Cycle, @Repair_Rate, @Installation, @Installation_Part, @Repair_Article_Etc, @User_ID, @PostIP); Select Cast(SCOPE_IDENTITY() As Int);";

            using var connection = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            connection.Open();
            var result = await connection.ExecuteScalarAsync<int>(sql, model);
            model.Aid = result;
            return model;
        }

        /// <summary>
        /// 수선항목 수정
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Article_Entity> Edit_RepairArticle(Article_Entity model)
        {
            var sql = "Update Repair_Article Set Repair_Article_Name = @Repair_Article_Name, Division = @Division, Amount = @Amount, Unit = @Unit, All_Cycle = @All_Cycle, Part_Cycle = @Part_Cycle, Repair_Rate = @Repair_Rate, Repair_Article_Etc = @Repair_Article_Etc, Installation = @Installation, Installation_Part = @Installation_Part, User_ID = @User_ID, PostIP = @PostIP Where Aid = @Aid;";

            using var connection = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            connection.Open();
            var a = await connection.ExecuteAsync(sql, model);
            return model;
        }

        /// <summary>
        /// 수선항목 리스트(단지별)
        /// </summary>
        /// <returns></returns>
        public async Task<List<Article_Entity>> GetLIst_RepairArticle(string Apt_Code, string Repair_Plan_Code)
        {
            string sql = "Select Aid, Apt_Code, Repair_Plan_Code, Sort_A_Code, Sort_B_Code, Sort_C_Code, Sort_A_Name, Sort_B_Name, Sort_C_Name, Repair_Article_Name, Division, Amount, Unit, All_Cycle, Part_Cycle, Repair_Rate, Installation, Installation_Part, Repair_Article_Etc, User_ID From Repair_Article Where Apt_Code = @Apt_Code And Repair_Plan_Code = @Repair_Plan_Code And Sort_A_Code = '3' And del = 'A' Order By Sort_A_Code Asc;";
            using (var connection = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                connection.Open();
                var aa = await connection.QueryAsync<Article_Entity>(sql, new { Apt_Code, Repair_Plan_Code }, commandType: CommandType.Text);
                return aa.ToList();
            }
        }

        /// <summary>
        /// 수선항목 리스트(단지별, 장기수선 전체 목록)
        /// </summary>
        /// <returns></returns>
        public async Task<List<Article_Entity>> GetLIst_Article_All(string Apt_Code, string Repair_Plan_Code)
        {
            string sql = "Select Aid, Apt_Code, Repair_Plan_Code, Sort_A_Code, Sort_B_Code, Sort_C_Code, Sort_A_Name, Sort_B_Name, Sort_C_Name, Repair_Article_Name, Division, Amount, Unit, All_Cycle, Part_Cycle, Repair_Rate, Installation, Installation_Part, Repair_Article_Etc, User_ID, Cycle_Count, Cost_Count From Repair_Article Where Apt_Code = @Apt_Code And Repair_Plan_Code = @Repair_Plan_Code and del = 'A' Order By Sort_A_Code Asc;";
            using (var connection = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                connection.Open();
                var aa = await connection.QueryAsync<Article_Entity>(sql, new { Apt_Code, Repair_Plan_Code }, commandType: CommandType.Text);
                return aa.ToList();
            }
        }

        /// <summary>
        /// 수선항목 리스트(장기수선계획별, 시설물 분류별)
        /// </summary>
        /// <returns></returns>
        public async Task<List<Article_Entity>> GetLIst_RepairArticle_Sort(string Repair_Plan_Code, string Sort_Field, string Sort_Code, string Apt_Code)
        {
            using (var connection = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                connection.Open();
                var aa = await connection.QueryAsync<Article_Entity>("Repair_Article_Sort", new { Repair_Plan_Code, Sort_Field, Sort_Code, Apt_Code }, commandType: CommandType.StoredProcedure);
                return aa.ToList();
            }
        }

        /// <summary>
        /// 수선항목 리스트 만들기(시설물 분류 항목 만들기)
        /// </summary>
        /// <returns></returns>
        public async Task<List<Article_Entity>> GetListArticleSort(string Repair_Plan_Code, string Sort_Field, string Sort_Code, string Apt_Code)
        {
            string sql = "Select a.Aid, a.Apt_Code, a.Sort_A_Code, a.Sort_B_Code, a.Sort_C_Code, a.Sort_A_Name, a.Sort_B_Name, a.Sort_C_Name, a.Repair_Article_Name, a.Repair_Plan_Code, a.All_Cycle, a.Amount, a.Division, a.Facility_Code, a.Installation, a.Installation_Part, a.Part_Cycle, a.Repair_Article_Etc, a.Unit, a.Repair_Rate, a.Cycle_Count, a.Cost_Count From Repair_Article as a Join Repair_Cost as b on a.Aid = b.Repair_Article_Code Where a.Repair_Plan_Code = @Repair_Plan_Code And a." + Sort_Field + " = @Sort_Code And a.Apt_Code = @Apt_Code And a.del = 'A' Order By a.Sort_A_Code Asc, Convert(int, a.Sort_B_Code) Asc, Convert(int, a.Sort_C_Code) Asc;";
            using (var connection = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                connection.Open();
                var aa = await connection.QueryAsync<Article_Entity>(sql, new { Repair_Plan_Code, Sort_Code, Apt_Code }, commandType: CommandType.Text);
                return aa.ToList();
            }
        }

        /// <summary>
        /// 수선항목 삭제
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task Remove_Repair_Article(int Aid)
        {
            const string sql = "Delete From Repair_Article Where Aid = @Aid";
            using (var connection = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                connection.Open();
                await connection.ExecuteAsync(sql, new { Aid }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 해당 장기수선계획 식별코드로 입력된 모든 수선항목 삭제
        /// 2017
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task Delete_Article_PlanCode(string Repair_Plan_Code)
        {
            const string sql = "Delete Repair_Article Where Repair_Plan_Code = @Repair_Plan_Code";
            using (var connection = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                connection.Open();
                await connection.ExecuteAsync(sql, new { Repair_Plan_Code }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 수선항목 상세보기
        /// </summary>
        /// <param name="Aid"></param>
        /// <returns></returns>
        public async Task<Article_Entity> Detail_RepairArticle(string Apt_Code, int Aid)
        {
            string sql = "Select Aid, Apt_Code, Repair_Plan_Code, Facility_Code, Sort_A_Code, Sort_B_Code, Sort_C_Code, Sort_A_Name, Sort_B_Name, Sort_C_Name, Repair_Article_Name, Division, Amount, Unit, All_Cycle, Part_Cycle, Repair_Rate, Installation, Installation_Part, Repair_Article_Etc, Cycle_Count, Cost_Count, Cycle_Count, Cost_Count From Repair_Article Where Apt_Code = @Apt_Code And Aid = @Aid;";
            using (var connection = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                connection.Open();
                Article_Entity aa = await connection.QuerySingleOrDefaultAsync<Article_Entity>(sql, new { Apt_Code, Aid }, commandType: CommandType.Text);
                return aa;
            }
        }

        /// <summary>
        /// 수선항목에 따른 대분류명 가져오기
        /// </summary>
        /// <returns></returns>
        public async Task<string> Sort_A_Name(int Aid)
        {
            string sql = "Select Facility_Sort.Sort_Name From Facility_Sort inner Join Repair_Article on Facility_Sort.Facility_Sort_Code = Repair_Article.Sort_A_Code Where Repair_Article.Aid = @Aid";
            using (var conn = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                conn.Open();
                return await conn.QuerySingleOrDefaultAsync<string>(sql, new { Aid }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 수선항목에 따른 중분류 명 가져오기
        /// </summary>
        /// <returns></returns>
        public async Task<string> Sort_B_Name(int Aid)
        {
            string sql = "Select Facility_Sort.Sort_Name From Facility_Sort inner Join Repair_Article on Facility_Sort.Facility_Sort_Code = Repair_Article.Sort_B_Code Where Repair_Article.Aid = @Aid";
            using (var conn = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                conn.Open();
                return await conn.QuerySingleOrDefaultAsync<string>(sql, new { Aid }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 수선항목에 따른 공사종별 명 가져오기
        /// </summary>
        /// <returns></returns>
        public async Task<string> Sort_C_Name(int Aid)
        {
            string sql = "Select Facility_Sort.Sort_Name From Facility_Sort inner Join Repair_Article on Facility_Sort.Facility_Sort_Code = Repair_Article.Sort_C_Code Where Repair_Article.Aid = @Aid";
            using (var conn = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                conn.Open();
                return await conn.QuerySingleOrDefaultAsync<string>(sql, new { Aid }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 수선항목에 따른 공사종별 명 가져오기
        /// </summary>
        /// <returns></returns>
        public async Task<string> Article_Name(int Aid)
        {
            string sql = "Select Repair_Article_Name From Repair_Article Where Aid = @Aid";
            using (var conn = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                conn.Open();
                return await conn.QuerySingleOrDefaultAsync<string>(sql, new { Aid }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 단위 명 가져오기
        /// </summary>
        /// <returns></returns>
        public async Task<string> Article_Unit(int Aid)
        {
            string sql = "Select Unit From Repair_Article Where Aid = @Aid";
            using (var conn = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                conn.Open();
                return await conn.QuerySingleOrDefaultAsync<string>(sql, new { Aid }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 해당 공동주택의 수선항목 중 현재 선택된 수선계획 코드의 하나 이전 수선항목 정보
        /// </summary>
        /// <returns></returns>
        public async Task<List<Article_Entity>> GetList_Ago(string Apt_Code, string Sort_C_Code, string Adjustment_Date)
        {
            string sql = "Select Aid, Apt_Code, Repair_Plan_Code, Sort_A_Code, Sort_B_Code, Sort_C_Code, Sort_A_Name, Sort_B_Name, Sort_C_Name, Repair_Article_Name, Division, Amount, Unit, All_Cycle, Part_Cycle, Repair_Rate, Installation, Installation_Part, Repair_Article_Etc, User_ID, Cycle_Count, Cost_Count From Repair_Article Where Sort_C_Code = @Sort_C_Code And Repair_Plan_Code in (Select Top 1 Repair_Plan_Code From Repair_Plan Where Adjustment_Date < @Adjustment_Date And Apt_Code = @Apt_Code Order By Adjustment_Date Desc) Order By Aid Desc;";
            using (var connection = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                connection.Open();
                var aa = await connection.QueryAsync<Article_Entity>(sql, new { Apt_Code, Sort_C_Code, Adjustment_Date }, commandType: CommandType.Text);
                return aa.ToList();
            }
        }

        /// <summary>
        /// 수선항목, 수선주기, 수선금액 조인 정보 가져오기
        /// </summary>
        /// <returns></returns>
        public async Task<List<Join_Article_Cycle_Cost_Entity>> GetList_CriterionTable(string Apt_Code, string Repair_Plan_Code, string Sort_A_Code)
        {
            using (var connection = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                connection.Open();
                var aa = await connection.QueryAsync<Join_Article_Cycle_Cost_Entity>("GetList_CriterionTable", new { Apt_Code, Repair_Plan_Code, Sort_A_Code }, commandType: CommandType.StoredProcedure);
                return aa.ToList();
            }
        }

        /// <summary>
        /// 존재여부 확인 (계획코드, 공사종별코드, 수선항목명)
        /// </summary>
        /// <returns></returns>
        public async Task<int> Being_Article(string Repair_Plan_Code, string Sort_C_Code, string Repair_Article_Name)
        {
            string sql = "Select Count(*) From Repair_Article Where Repair_Plan_Code = @Repair_Plan_Code And Sort_C_Code = @Sort_C_Code And Repair_Article_Name = @Repair_Article_Name";
            using (var conn = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                conn.Open();
                return await conn.QuerySingleOrDefaultAsync<int>(sql, new { Repair_Plan_Code, Sort_C_Code, Repair_Article_Name }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 시설물 코드로 수선항목  존재여부 확인 (계획코드, 공사종별코드)
        /// </summary>
        /// <param name="Repair_Plan_Code"></param>
        /// <param name="Sort_C_Code"></param>
        /// <returns></returns>
        public int Being_Article_Facility(string Repair_Plan_Code, string Sort_C_Code)
        {
            string sql = "Select Count(*) From Repair_Article Where Repair_Plan_Code = @Repair_Plan_Code And Sort_C_Code = @Sort_C_Code";
            using (var conn = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                //conn.Open();
                int a = conn.QuerySingleOrDefault<int>(sql, new { Repair_Plan_Code, Sort_C_Code }, commandType: CommandType.Text);
                return a;
            }
        }

        /// <summary>
        /// 시설물 코드로 수선항목  존재여부 확인 (계획코드, 공사종별코드)
        /// </summary>
        /// <param name="Repair_Plan_Code"></param>
        /// <param name="Sort_C_Code"></param>
        /// <returns></returns>
        public int Being_Article_FacilityQ(string Plan_Code, string code)
        {
            string sql = "Select Count(*) From Repair_Article Where Repair_Plan_Code = @Plan_Code And Sort_C_Code = @code";
            using (var conn = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                //conn.Open();
                int a = conn.QueryFirstOrDefault<int>(sql, new { Plan_Code, code }, commandType: CommandType.Text);
                return a;
            }
        }

        /// <summary>
        /// 수선항목 코드 찾기 (계획코드, 공사종별코드, 수선항목명)
        /// </summary>
        /// <param name="Repair_Plan_Code"></param>
        /// <param name="Sort_C_Code"></param>
        /// <returns></returns>
        public async Task<int> Being_Article_Code(string Repair_Plan_Code, string Sort_C_Code, string Repair_Article_Name)
        {
            string sql = "Select Top 1 Aid From Repair_Article Where Repair_Plan_Code = @Repair_Plan_Code And Sort_C_Code = @Sort_C_Code And Repair_Article_Name = @Repair_Article_Name Order By Aid Desc";
            using (var conn = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                conn.Open();
                return await conn.QuerySingleOrDefaultAsync<int>(sql, new { Repair_Plan_Code, Sort_C_Code, Repair_Article_Name }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 해당 장기수선계획에 선택된 수선항목 수
        /// </summary>
        public async Task<int> Article_Num(string Apt_Code, string Repair_Plan_Code)
        {
            string sql = "Select Count(*) From Repair_Article Where Apt_Code = @Apt_Code And Repair_Plan_Code = @Repair_Plan_Code";
            using (var conn = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                conn.Open();
                return await conn.QuerySingleOrDefaultAsync<int>(sql, new { Apt_Code, Repair_Plan_Code }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 장기수선계획 조정 생성 시 해당 정보
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Article_Entity> All_Insert_Code(string Apt_Code, string Repair_Plan_CodeA, string Repair_Plan_CodeB, string User_ID, string PostIP)
        {
            var sql = "Insert into Repair_Article (Apt_Code, Repair_Plan_Code, Facility_Code, Sort_A_Code, Sort_B_Code, Sort_C_Code, Sort_A_Name, Sort_B_Name, Sort_C_Name, Repair_Article_Name, Division, Amount, Unit, All_Cycle, Part_Cycle, Repair_Rate, Installation, Installation_Part, Repair_Article_Etc, User_ID, PostIP) Select Apt_Code, @Repair_Plan_CodeB, Facility_Code, Sort_A_Code, Sort_B_Code, Sort_C_Code, Sort_A_Name, Sort_B_Name, Sort_C_Name, Repair_Article_Name, Division, Amount, Unit, All_Cycle, Part_Cycle, Repair_Rate, Installation, Installation_Part, Repair_Article_Etc, @User_ID, @PostIP From Repair_Article Where Apt_Code = @Apt_Code And Repair_Plan_Code = @Repair_Plan_CodeA;";

            using (var connection = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                connection.Open();
                await connection.ExecuteAsync(sql, new { Apt_Code, Repair_Plan_CodeA, Repair_Plan_CodeB, User_ID, PostIP }, commandType: CommandType.Text);
                Article_Entity aa = new Article_Entity();
                return aa;
            }
        }

        /// <summary>
        /// 수선금액 산정 시에 불러오는 수선항목(수선항목과 수선주기에 있는 수선항목) 목록
        /// </summary>
        public async Task<List<Join_Article_Cycle_Cost_Entity>> GetLIst_Repair_Article_Cycle(string Apt_Code, string Repair_Plan_Code)
        {
            string sql = "Select a.Aid, a.Apt_Code, b.Repair_Article_Code, a.Repair_Plan_Code, a.Sort_A_Code, a.Sort_B_Code, a.Sort_C_Code, a.Sort_A_Name, a.Sort_B_Name, a.Sort_C_Name, a.Repair_Article_Name, a.Division, a.Amount, a.All_Cycle, a.Part_Cycle, a.Repair_Rate, a.Installation, a.Installation_Part, a.Repair_Article_Etc, a.User_ID, a.PostDate, a.PostIP, a.del, b.Set_Repair_Cycle_All, b.Set_Repair_Cycle_Part, b.Repair_Plan_Year_All, b.Repair_Plan_Year_Part, a.Cycle_Count, a.Cost_Count From Repair_Article As a Join Repair_Cycle As b On a.Apt_Code = b.Apt_Code And a.Aid = b.Repair_Article_Code Where a.Apt_Code = @Apt_Code And b.Apt_Code = @Apt_Code And a.Repair_Plan_Code = @Repair_Plan_Code And a.del = 'A' Order By a.Sort_A_Code Asc, Convert(int, a.Sort_B_Code) Asc, Convert(int, a.Sort_C_Code) Asc;";

            using (var connection = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                connection.Open();
                var aa = await connection.QueryAsync<Join_Article_Cycle_Cost_Entity>(sql, new { Apt_Code, Repair_Plan_Code }, commandType: CommandType.Text);
                return aa.ToList();
            }
        }

        /// <summary>
        /// 수선금액 산정 시에 불러오는 수선항목(수선항목과 수선주기에 있는 수선항목) 목록
        /// </summary>
        public async Task<List<Join_Article_Cycle_Cost_Entity>> GetLIst_Repair_Article_Cycle_Review(string Apt_Code, string Repair_Plan_Code)
        {
            string sql = "Select a.Aid, a.Apt_Code, a.Repair_Plan_Code, a.Sort_A_Code, a.Sort_B_Code, a.Sort_C_Code, a.Sort_A_Name, a.Sort_B_Name, a.Sort_C_Name, a.Repair_Article_Name, a.Division, a.Amount, a.All_Cycle, a.Part_Cycle, a.Repair_Rate, a.Installation, a.Installation_Part, a.Repair_Article_Etc, a.User_ID, a.PostDate, a.PostIP, a.del, b.Set_Repair_Cycle_All, b.Set_Repair_Cycle_Part, b.Repair_Plan_Year_All, b.Repair_Plan_Year_Part, c.Repair_All_Cost, c.Repair_Part_Cost, a.Cycle_Count, a.Cost_Count From Repair_Article As a Join Repair_Cycle As b On a.Aid = b.Repair_Article_Code Join Repair_Cost As c On a.Aid = c.Repair_Article_Code Where a.Apt_Code = @Apt_Code And b.Apt_Code = @Apt_Code And c.Apt_Code = @Apt_Code And a.Repair_Plan_Code = @Repair_Plan_Code And b.Repair_Plan_Code = @Repair_Plan_Code And c.Repair_Plan_Code = @Repair_Plan_Code And a.del = 'A' Order By a.Sort_A_Code Asc, Convert(int, a.Sort_B_Code) Asc, Convert(int, a.Sort_C_Code) Asc;";

            using (var connection = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                connection.Open();
                var aa = await connection.QueryAsync<Join_Article_Cycle_Cost_Entity>(sql, new { Apt_Code, Repair_Plan_Code }, commandType: CommandType.Text);
                return aa.ToList();
            }
        }

        /// <summary>
        /// 수선항목 리스트(장기수선계획별, 시설물 분류별)(수선항목과 수선주기의 수선항목 코드가 같은 경우)
        /// </summary>
        public async Task<List<Join_Article_Cycle_Cost_Entity>> GetLIst_Repair_Article_Cycle_Sort(string Repair_Plan_Code, string Sort_Field, string Sort_Code, string Apt_Code)
        {
            string sql = "Select a.Aid, a.Apt_Code, a.Repair_Plan_Code, a.Sort_A_Name, a.Sort_B_Name, a.Sort_C_Name, b.Repair_Article_Code, a.Repair_Article_Name, a.Cycle_Count, a.Cost_Count From Repair_Article As a Join Repair_Cycle As b On a.Aid = b.Repair_Article_Code Where a.Repair_Plan_Code = @Repair_Plan_Code And b.Repair_Plan_Code = @Repair_Plan_Code And a." + Sort_Field + " = @Sort_Code And a.Apt_Code = @Apt_Code And b.Apt_Code = @Apt_Code Order By a.Sort_A_Code Asc, Convert(int, a.Sort_B_Code) Asc, Convert(int, a.Sort_C_Code) Asc";

            using (var connection = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                connection.Open();
                var aa = await connection.QueryAsync<Join_Article_Cycle_Cost_Entity>(sql, new { Repair_Plan_Code, Sort_Field, Sort_Code, Apt_Code }, commandType: CommandType.Text);
                return aa.ToList();
            }
        }

        /// <summary>
        /// 수선항목 리스트(장기수선계획별, 시설물 분류별)(수선항목과 수선주기의 수선항목 코드가 같은 경우)
        /// </summary>
        public async Task<List<Join_Article_Cycle_Cost_Entity>> GetLIst_Repair_Article_Cycle_Sort_Review(string Repair_Plan_Code, string Sort_Field, string Sort_Code, string Apt_Code)
        {
            using (var connection = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                connection.Open();
                var aa = await connection.QueryAsync<Join_Article_Cycle_Cost_Entity>("Repair_Article_Cycle_Sort", new { Repair_Plan_Code, Sort_Field, Sort_Code, Apt_Code }, commandType: CommandType.StoredProcedure);
                return aa.ToList();
            }
        }

        /// <summary>
        /// 수선항목 리스트(장기수선계획별, 시설물 분류별)(수선항목과 수선주기의 수선항목 코드가 같은 경우)
        /// </summary>
        public async Task<List<Join_Article_Cycle_Cost_Entity>> GetList_Repair_Article_Cycle_Sort(string Repair_Plan_Code, string Sort_Field, string Sort_Code, string Apt_Code)
        {
            var sql = "Select a.Aid, a.Apt_Code, a.Repair_Plan_Code, a.Sort_A_Name, a.Sort_B_Name, a.Sort_C_Name, a.Sort_A_Code, a.Sort_B_Code, a.Sort_C_Code, b.Repair_Article_Code, a.Repair_Article_Name, a.Cost_Count, a.Cycle_Count, a.Repair_Rate, a.Amount, a.Unit, b.Set_Repair_Rate, b.Set_Repair_Cycle_All, b.Set_Repair_Cycle_Part, b.Repair_Plan_Year_All, b.Repair_Last_Year_Part, a.All_Cycle, a.Part_Cycle, b.All_Cycle_Num, b.Part_Cycle_Num From Repair_Article As a Join Repair_Cycle As b On a.Aid = b.Repair_Article_Code Where a.Repair_Plan_Code = @Repair_Plan_Code And b.Repair_Plan_Code = @Repair_Plan_Code And a." + Sort_Field + " = @Sort_Code And a.Apt_Code = @Apt_Code And b.Apt_Code = @Apt_Code Order By a.Sort_A_Code Asc, Convert(int, a.Sort_B_Code) Asc, Convert(int, a.Sort_C_Code) Asc";
            using (var connection = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                connection.Open();
                var aa = await connection.QueryAsync<Join_Article_Cycle_Cost_Entity>(sql, new { Repair_Plan_Code, Sort_Field, Sort_Code, Apt_Code });
                return aa.ToList();
            }
        }

        /// <summary>
        /// 수선항목 수량 불러오기
        /// </summary>
        public async Task<string> Repair_Cost_Amount(string Article_Code)
        {
            string sql = "Select Amount From Repair_Article Where Aid = @Article_Code";
            using (var conn = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                conn.Open();
                return await conn.QuerySingleOrDefaultAsync<string>(sql, new { Article_Code }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 수선항목 상세(장기수선계획별, 시설물 분류별)(수선항목과 수선주기의 수선항목 코드가 같은 경우)
        /// </summary>
        public async Task<Join_Article_Cycle_Cost_Entity> Details_Repair_Article_Cycle_Sort_Review(string Repair_Plan_Code, string Aid, string Apt_Code)
        {
            using var connection = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await connection.QuerySingleOrDefaultAsync<Join_Article_Cycle_Cost_Entity>("SELECT a.Aid, a.Apt_Code, a.Repair_Plan_Code, a.Sort_A_Code, a.Sort_B_Code, a.Sort_C_Code, a.Sort_A_Name, a.Sort_B_Name, a.Sort_C_Name, a.Repair_Article_Name, a.Division, a.Amount, a.All_Cycle, a.Part_Cycle, a.Repair_Rate, a.Installation, a.Installation_Part, a.User_ID, a.PostDate, a.PostIP, a.del, b.Law_Repair_Cycle_All, b.Law_Repair_Cycle_Part, b.Law_Repair_Rate, b.Set_Repair_Cycle_All, b.Set_Repair_Cycle_Part, b.Repair_Plan_Year_All, b.Repair_Plan_Year_Part, b.All_Cycle_Num, b.Part_Cycle_Num, b.Set_Repair_Cycle_All, b.Set_Repair_Cycle_Part, b.Repair_Article_Code, c.Repair_All_Cost, c.Repair_Part_Cost, c.Repair_Rate, a.Cycle_Count, a.Cost_Count FROM Repair_Article As a Join Repair_Cycle As b On a.Apt_Code = b.Apt_Code And a.Aid = b.Repair_Article_Code Join Repair_Cost as c on c.Repair_Article_Code = a.Aid Where a.Apt_Code = @Apt_Code And a.Repair_Plan_Code = @Repair_Plan_Code And b.Repair_Plan_Code = @Repair_Plan_Code And b.Apt_Code = @Apt_Code And a.Aid = @Aid", new { Repair_Plan_Code, Aid, Apt_Code });
        }

        /// <summary>
        /// 주기 추가
        /// </summary>
        /// <param name="Aid"></param>
        public async Task Update_Cycle_Count_Add(int Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            await db.ExecuteAsync("Update Repair_Article Set Cycle_Count = 1 Where Aid = @Aid", new { Aid });
        }

        /// <summary>
        /// 주기삭제
        /// </summary>
        /// <param name="Aid"></param>
        public async Task Update_Cycle_Count_Minus(int Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));            
            await db.ExecuteAsync("Update Repair_Article Set Cycle_Count = 0 Where Aid = @Aid", new { Aid });            
        }

        /// <summary>
        /// 수선금액 추가
        /// </summary>
        /// <param name="Aid"></param>
        public async Task Update_Cost_Count_Add(int Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            await db.ExecuteAsync("Update Repair_Article Set Cost_Count = 1 Where Aid = @Aid", new { Aid });
        }

        /// <summary>
        /// 수선금액 삭제
        /// </summary>
        /// <param name="Aid"></param>
        public async Task Update_Cost_Count_Minus(int Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));            
            await db.ExecuteAsync("Update Repair_Article Set Cost_Count = 0 Where Aid = @Aid", new { Aid });            
        }
    }
}