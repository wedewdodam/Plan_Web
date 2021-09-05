using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Facility
{
    public class Facility_Lib : IFacility_Lib
    {
        private readonly IConfiguration _db;

        public Facility_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        // 시설물 입력하기
        public async Task<int> Add_Facility(Facility_Entity Facility)
        {
            var sql = "Insert Facility (Facility_Code, Apt_Code, Facility_Sort_Code_A, Facility_Sort_Code_B, Facility_Sort_Code_C, Facility_Name, Facility_Position, Facility_Installation_Date, Facility_Etc) Values (@Facility_Code, @Apt_Code, @Facility_Sort_Code_A, @Facility_Sort_Code_B, @Facility_Sort_Code_C, @Facility_Name, @Facility_Position, @Facility_Installation_Date, @Facility_Etc); Select Cast(SCOPE_IDENTITY() As Int);";
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            int Aid = await db.QuerySingleOrDefaultAsync<int>(sql, Facility);
            return Aid;
        }

        // 시설물 수정하기
        public async Task<Facility_Entity> Edit_Facility(Facility_Entity Facility)
        {
            var sql = "Update Facility Set Facility_Name = @Facility_Name, Facility_Position = @Facility_Position, Facility_Installation_Date = @Facility_Installation_Date, Facility_Etc = @Facility_Etc Where Aid = @Aid;";
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            await db.ExecuteAsync(sql, Facility);
            return Facility;
        }

        // 시설물 마지막 일련번호 얻기
        public async Task<int> Last_Number()
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Top 1 Aid From Facility Order by Aid Desc", new { });
        }

        // 시설물 ㅡ> 공동주택 리스트
        public async Task<List<Facility_Entity>> GetList_Facility(string Apt_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var lst = await db.QueryAsync<Facility_Entity>("Select a.Aid, a.Facility_Code, a.Apt_Code, a.Facility_Sort_Code_A, a.Facility_Sort_Code_B, a.Facility_Sort_Code_C, a.Facility_Name, a.Facility_Position, a.Facility_Installation_Date, a.Facility_Etc From Facility as a Join Facility_Sort as b On a.Facility_Sort_Code_C = b.Aid Where a.Apt_Code = @Apt_Code Order By a.Facility_Sort_Code_A Asc, a.Facility_Sort_Code_B Asc, a.Facility_Sort_Code_C Asc, a.Aid Asc", new { Apt_Code });
            return lst.ToList();
        }

        /// <summary>
        /// 시설물 목록
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<List<Facility_Entity>> GetList_Apt_Sort(string Apt_Code, string Sort)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var lst = await db.QueryAsync<Facility_Entity>("Select a.Aid, a.Facility_Code, a.Apt_Code, a.Facility_Sort_Code_A, b.Sort_A_Name, a.Facility_Sort_Code_B, b.Sort_B_Name, a.Facility_Sort_Code_C, b.Sort_Name as Sort_C_Name, a.Facility_Name, a.Facility_Position, a.Facility_Installation_Date, a.Facility_Etc From Facility as a Join Facility_Sort as b On a.Facility_Sort_Code_C = b.Aid Where a.Apt_Code = @Apt_Code And a.Facility_Sort_Code_A = @Sort Order By a.Facility_Sort_Code_A Asc, a.Facility_Sort_Code_B Asc, a.Facility_Sort_Code_C Asc, a.Aid Asc", new { Apt_Code, Sort });
            return lst.ToList();
        }

        /// <summary>
        /// 시설물 목록
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<List<Facility_Entity>> GetList_Apt(int Page, string Apt_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var lst = await db.QueryAsync<Facility_Entity>("Select Top 15  a.Aid, a.Facility_Code, a.Apt_Code, a.Facility_Sort_Code_A, b.Sort_A_Name, a.Facility_Sort_Code_B, b.Sort_B_Name, a.Facility_Sort_Code_C, b.Sort_Name as Sort_C_Name, a.Facility_Name, a.Facility_Position, a.Facility_Installation_Date, a.Facility_Etc From Facility as a Join Facility_Sort as b On a.Facility_Sort_Code_C = b.Aid Where a.Aid Not In (Select Top (15 * @Page) a.Aid From  Facility as a Join Facility_Sort as b On a.Facility_Sort_Code_C = b.Aid Where a.Apt_Code = @Apt_Code Order By a.Facility_Sort_Code_A Asc, a.Facility_Sort_Code_B Asc, a.Facility_Sort_Code_C Asc, a.Aid Asc) And a.Apt_Code = @Apt_Code Order By a.Facility_Sort_Code_A Asc, a.Facility_Sort_Code_B Asc, a.Facility_Sort_Code_C Asc, a.Aid Asc", new { Page, Apt_Code });
            return lst.ToList();
        }

        /// <summary>
        /// 시설물 목록 수
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<int> GetList_Apt_Count(string Apt_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Facility as a Join Facility_Sort as b On a.Facility_Sort_Code_C = b.Aid Where Apt_Code = @Apt_Code", new { Apt_Code });
        }

        /// <summary>
        /// 시설물 목록 찾기
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<List<Facility_Entity>> GetList_Apt_Query(int Page, string Apt_Code, string Feild, string Query)
        {
            var sql = "Select Top 15  a.Aid, a.Facility_Code, a.Apt_Code, a.Facility_Sort_Code_A, b.Sort_A_Name, a.Facility_Sort_Code_B, b.Sort_B_Name, a.Facility_Sort_Code_C, b.Sort_Name as Sort_C_Name, a.Facility_Name, a.Facility_Position, a.Facility_Installation_Date, a.Facility_Etc From Facility as a Join Facility_Sort as b On a.Facility_Sort_Code_C = b.Aid Where a.Aid Not In (Select Top (15 * @Page) a.Aid From Facility as a Join Facility_Sort as b On a.Facility_Sort_Code_C = b.Aid Where a.Apt_Code = @Apt_Code And a." + Feild + " = @Query Order By a.Facility_Sort_Code_A Asc, a.Facility_Sort_Code_B Asc, a.Facility_Sort_Code_C Asc, a.Aid Asc) And a.Apt_Code = @Apt_Code And a." + Feild + " = @Query Order By a.Facility_Sort_Code_A Asc, a.Facility_Sort_Code_B Asc, a.Facility_Sort_Code_C Asc, a.Aid Asc";
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var lst = await db.QueryAsync<Facility_Entity>(sql, new { Page, Apt_Code, Feild, Query });
            return lst.ToList();
        }

        /// <summary>
        /// 시설물 목록 찾기 수
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<int> GetList_Apt_Query_Count(string Apt_Code, string Feild, string Query)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Facility as a Join Facility_Sort as b On a.Facility_Sort_Code_C = b.Aid Where Apt_Code = @Apt_Code And a." + Feild + " = @Query Order", new { Apt_Code, Feild, Query });
        }

        public async Task<List<Facility_Entity>> GetList_A(string Apt_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var lst = await db.QueryAsync<Facility_Entity>("Select Aid, Facility_Code, Apt_Code, Facility_Sort_Code_A, Facility_Sort_Code_B, Facility_Sort_Code_C, Facility_Name, Facility_Position, Facility_Installation_Date, Facility_Etc From Facility, Facility_Sort Where Facility.Facility_Sort_Code_C = Facility_Sort.Aid And Facility.Apt_Code = @Apt_Code Order By Facility_Sort.Sort_Order Asc", new { Apt_Code });
            return lst.ToList();
        }

        // 시설물 ㅡ> 공동주택 대분류 시설물 리스트
        public async Task<List<Facility_Entity>> GetList_Facility_Sort_A(string Apt_Code, string Facility_Sort_Code_A)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var lst = await db.QueryAsync<Facility_Entity>("Select Aid, Facility_Code, Apt_Code, Facility_Sort_Code_A, Facility_Sort_Code_B, Facility_Sort_Code_C, Facility_Name, Facility_Position, Facility_Installation_Date, Facility_Etc From Facility Where Apt_Code = @Apt_Code And Facility_Sort_Code_A = @Facility_Sort_Code_A Order By Aid Asc", new { Apt_Code, Facility_Sort_Code_A });
            return lst.ToList();
        }

        // 시설물 ㅡ> 공동주택 세분류 시설물 리스트
        public async Task<List<Facility_Entity>> GetList_Facility_Sort_B(string Apt_Code, string Facility_Sort_Code_B)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var lst = await db.QueryAsync<Facility_Entity>("Select Aid, Facility_Code, Apt_Code, Facility_Sort_Code_A, Facility_Sort_Code_B, Facility_Sort_Code_C, Facility_Name, Facility_Position, Facility_Installation_Date, Facility_Etc From Facility Where Apt_Code = @Apt_Code And Facility_Sort_Code_B = @Facility_Sort_Code_B Order By Aid Asc", new { Apt_Code, Facility_Sort_Code_B });
            return lst.ToList();
        }

        // 시설물 ㅡ> 공동주택 세분류 시설물 리스트
        public async Task<List<Facility_Entity>> GetList_Facility_Sort_C(string Apt_Code, string Facility_Sort_Code_C)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var lst = await db.QueryAsync<Facility_Entity>("Select Aid, Facility_Code, Apt_Code, Facility_Sort_Code_A, Facility_Sort_Code_B, Facility_Sort_Code_C, Facility_Name, Facility_Position, Facility_Installation_Date, Facility_Etc From Facility Where Apt_Code = @Apt_Code And Facility_Sort_Code_C = @Facility_Sort_Code_C Order By Aid Asc", new { Apt_Code, Facility_Sort_Code_C });
            return lst.ToList();
        }

        // 시설물 코드명 해당 정보 불러오기
        public async Task<Facility_Entity> Detail_Facility(string Facility_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<Facility_Entity>("Select Aid, Facility_Code, Apt_Code, Facility_Sort_Code_A, Facility_Sort_Code_B, Facility_Sort_Code_C, Facility_Name, Facility_Position, Facility_Installation_Date, Facility_Etc From Facility Where Facility_Code = @Facility_Code", new { Facility_Code });
        }

        /// <summary>
        /// 시설물 명 존재 여부 확인
        /// </summary>
        /// <param name="Facility_Name"></param>
        /// <returns></returns>
        public async Task<int> Repeat_Article(string Facility_Name)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Facility Where Facility_Name = @Facility_Name", new { Facility_Name });
        }

        /// <summary>
        /// 시설물 삭제
        /// </summary>
        /// <param name="Aid"></param>
        public async Task Remove(string Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            await db.ExecuteAsync("Delete Facility Where Aid = @Aid", new { Aid });
        }
    }

    public class Facility_Detail_Lib : IFacility_Detail_Lib
    {
        private readonly IConfiguration _db;

        public Facility_Detail_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        // 시설물 상세 입력
        public async Task<Facility_Detail_Entity> Add_Facility_Detail(Facility_Detail_Entity Fac_D)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var sql = "Insert Facility_Detail (Facility_Code, Apt_Code, Sort_A, Sort_B, Sort_C, Manufacture_Date, Manufacture_Num, Facility_Use, Facility_Form, Manufacture_Corporation, Facility_Standard, Quantity, Unit, Facility_Detail_Etc, User_ID, PostIP) Values (@Facility_Code, @Apt_Code, @Sort_A, @Sort_B, @Sort_C, @Manufacture_Date, @Manufacture_Num, @Facility_Use, @Facility_Form, @Manufacture_Corporation, @Facility_Standard, @Quantity, @Unit, @Facility_Detail_Etc, @User_ID, @PostIP);";
            await db.QuerySingleOrDefaultAsync(sql, Fac_D);
            return Fac_D;
        }

        // 시설물 상세 수정
        public async Task<Facility_Detail_Entity> Edit_Facility_Detail(Facility_Detail_Entity Fac_D)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var sql = "Update Facility_Detail Set Manufacture_Date = @manufacture_Date, Manufacture_Corporation = @Manufacture_Corporation, Facility_Use = @Facility_Use, Facility_Standard = @Facility_Standard, Facility_Form = @Facility_Form, Quantity = @Quantity, Unit = @Unit, Facility_Detail_Etc = @Facility_Detail_Etc, User_ID = @User_ID, PostIP = @PostIP";
            await db.ExecuteAsync(sql, Fac_D);
            return Fac_D;
        }

        // 시설물 상세 상세보기
        public async Task<Facility_Detail_Entity> Detail_Facility_Detail(int Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<Facility_Detail_Entity>("Select * From Facility_Detail Where Aid = @Aid", new { Aid });
        }

        // 시설물 상세 리스트(시설물코드로)
        public async Task<List<Facility_Detail_Entity>> GetList_Facility_Detail(string Facility_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var lst = await db.QueryAsync<Facility_Detail_Entity>("Select * From Facility_Detail Where Facility_Code = @Facility_Code Order By Aid Desc", new { Facility_Code });
            return lst.ToList();
        }

        // 시설물 상세 상세보기
        public async Task<Facility_Detail_Entity> Detail_Facility_Detail_FacilityCode(string Facility_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<Facility_Detail_Entity>("Select Top 1 * From Facility_Detail Where Facility_Code = @Facility_Code Order By Aid Desc", new { Facility_Code });
        }

        // 시설물 상세 상세보기(존재여부)
        public async Task<int> Detail_Facility_Detail_FacilityCode_Count(string Facility_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Facility_Detail Where Facility_Code = @Facility_Code", new { Facility_Code });
        }

        //시설물 상세 마지막 번호

        public async Task<int> Last_Number()
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Top 1 Aid From Facility_Detail", new { });
        }

        //시설물 상세 반복 체크
        public async Task<int> Repeat_Check(string Facility_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Facility_Detail Where Facility_Code = @Facility_Code", new { Facility_Code });
        }
    }

    /// <summary>
    /// 시설물 설명 및 보수
    /// </summary>
    public class Facility_Explanation_Lib : IFacility_Explanation_Lib
    {
        private readonly IConfiguration _db;

        public Facility_Explanation_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        // 시설물 설명(상세) 입력 메서드
        public async Task<Facility_Explanation_Entity> Add_Facility_Explanation(Facility_Explanation_Entity Facility)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var sql = "Insert into Facility_Explanation (Facility_Sort_Code, Explanation, Repair_Method, PostIP) Values (@Facility_Sort_Code, @Explanation, @Repair_Method, @PostIP)";
            await db.ExecuteAsync(sql, Facility);
            return Facility;
        }

        // 시설물 설명(상세) 수정 메서드
        public async Task<Facility_Explanation_Entity> Edit_Facility_Explanation(Facility_Explanation_Entity Facility)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var sql = "Update Facility_Explanation Set Explanation = @Explanation, Repair_Method = @Repair_Method, PostIP = @PostIP Where Aid = @Aid";
            await db.ExecuteAsync(sql, Facility);
            return Facility;
        }

        // 시설물 설명 상세정보 메서드
        public async Task<Facility_Explanation_Entity> Detail_Facility_Explanation(string Facility_Sort_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<Facility_Explanation_Entity>("Select Top 1 * From Facility_Explanation Where Facility_Sort_Code = @Facility_Sort_Code Order By Aid Desc", new { Facility_Sort_Code });
        }

        // 시설물 설명 삭제 메서드
        public async Task Remove_Facility(int Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            await db.ExecuteAsync("Delete From Facility_Explanation Where Aid = @Aid", new { Aid });
        }

        // 시설물 설명 삭제 메서드
        public async Task<int> Repeat_Facility(string Facility_Sort_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Facility_Explanation Where Facility_Sort_Code = @Facility_Sort_Code", new { Facility_Sort_Code });
        }
    }

    // 부대 복리 시설 관련 클래스
    public class Additional_Welfare_Facility_Lib : IAdditional_Welfare_Facility_Lib
    {
        private readonly IConfiguration _db;

        public Additional_Welfare_Facility_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 부대 복리시설 등록
        /// </summary>
        /// <param name="AWF"></param>
        /// <returns></returns>
        public async Task<Additional_Welfare_Facility> Add_AdditionalWelfareFacility(Additional_Welfare_Facility AWF)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var msh = "Insert Additional_Welfare_Facility (Apt_Code, Facility_Name, Facility_Position, Facility_Division, Quantity, Unit, Facility_Etc, User_ID, Post_IP) Values (@Apt_Code, @Facility_Name, @Facility_Position, @Facility_Division, @Quantity, @Unit, @Facility_Etc, @User_ID, @Post_IP)";
            await db.ExecuteAsync(msh, AWF);
            return AWF;
        }

        // 부대 복리시설 수정
        public async Task<Additional_Welfare_Facility> Edit_AdditionalWelfareFacility(Additional_Welfare_Facility AWF)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var msh = "Update Additional_Welfare_Facility Set Facility_Name = @Facility_Name, Facility_Position = @Facility_Position, Quantity = @Quantity, Unit = @Unit, Facility_Etc =@Facility_Etc, User_ID = @User_ID, Post_IP = @Post_IP, PostDate = GetDate() Where Aid = @Aid";
            await db.ExecuteAsync(msh, AWF);
            return AWF;
        }

        // 부대 복리시설 목록
        public async Task<List<Additional_Welfare_Facility>> GetList_AdditionalWelfareFacility(string Apt_Code, string Facility_Division)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var lst = await db.QueryAsync<Additional_Welfare_Facility>("Select * From Additional_Welfare_Facility Where Apt_Code = @Apt_Code And Facility_Division = @Facility_Division Order By Aid Asc", new { Apt_Code, Facility_Division });
            return lst.ToList();
        }

        /// <summary>
        /// 부대 복리 시설 반복 여부 체크
        /// </summary>
        /// <param name="Facility_Name"></param>
        /// <param name="Facitlty_Position"></param>
        /// <returns></returns>
        public async Task<int> Being_Repeat(string Facility_Name, string Facitlty_Position, string Apt_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Additional_Welfare_Facility Where Facility_Name = @Facility_Name And Facility_Position = Facility_Position And Apt_Code = @Apt_Code", new { Facility_Name, Facitlty_Position, Apt_Code });
        }

        /// <summary>
        /// 상세보기
        /// </summary>
        /// <param name="Aid"></param>
        /// <returns></returns>
        public async Task<Additional_Welfare_Facility> Detail_AdditionalWelfareFacility(string Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<Additional_Welfare_Facility>("Select * From Additional_Welfare_Facility Where Aid = @Aid", new { Aid });
        }

        /// <summary>
        /// 부대복리시설 정보 삭제
        /// </summary>
        /// <param name="Aid"></param>
        public async Task Remove(string Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            await db.ExecuteAsync("Delete Additional_Welfare_Facility Where Aid = @Aid", new { Aid });
        }

        /// <summary>
        /// 해당 식별코드 번호 존재 여부
        /// </summary>
        public async Task<int> Being(int Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Additional_Welfare_Facility Where Aid = Aid", new { Aid });
        }

        /// <summary>
        /// 해당 식별코드 번호 존재 여부
        /// </summary>
        public async Task<int> BeingCount(string Apt_Code, string Division)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Additional_Welfare_Facility Where Apt_Code = @Apt_Code And Facility_Division = @Division", new { Apt_Code, Division });
        }
    }

    public class Facility_Sort_Lib : IFacility_Sort_Lib
    {
        private readonly IConfiguration _db;

        public Facility_Sort_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 분류 입력
        /// </summary>
        public async Task<Facility_Sort_Entity> Add_FacilitySort(Facility_Sort_Entity model)
        {
            var sql = "Insert Into Facility_Sort (Facility_Sort_Code, Sort_Name, Sort_A_Code, Sort_A_Name, Sort_B_Code, Sort_B_Name, Up_Code, Sort_Step, Sort_Order, Repair_Cycle, Repair_Cycle_Part, Repair_Rate, Sort_Division, Sort_Detail, Enforce_Date) Value (@Facility_Sort_Code, @Sort_Name, @Sort_A_Code, @Sort_B_Code, @Up_Code, @Sort_Step, @Sort_Order, @Repair_Cycle, @Repair_Cycle_Part, @Repair_Rate, @Sort_Division, @Sort_Detail, @Enforce_Date); Select Cast(SCOPE_IDENTITY() As Int);";

            using (var connection = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                connection.Open();
                var result = await connection.ExecuteScalarAsync<int>(sql, model);
                model.Aid = result;
                return model;
            }
        }

        public async Task<Facility_Sort_Entity> Edit_FacilitySort(Facility_Sort_Entity model)
        {
            var sql = "Update Facility_Sort Set Sort_Name = @Sort_Name, Sort_A_Code= @Sort_A_Code, Sort_A_Name = @Sort_A_Name, Sort_B_Code = @Sort_B_Code, Sort_B_Name = @Sort_B_Name, Up_Code = @Up_Code, Sort_Step = @Sort_Step, Sort_Order = @Sort_Order, Repair_Cycle = @Repair_Cycle, Repair_Cycle_Part = @Repair_Cycle_Part, Repair_Rate = @Repair_Rate, Sort_Division = @Sort_Division, Sort_Detail = @Sort_Detail, Enforce_Date = @Enforce_Date Where Aid = @Aid;";

            using (var connection = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                connection.Open();
                var a = await connection.ExecuteAsync(sql, model);
                return model;
            }
        }

        /// <summary>
        /// 상세보기
        /// </summary>
        public async Task<Facility_Sort_Entity> Details_FacilitySort(string Aid)
        {
            string sql = "Select Aid, Facility_Sort_Code, Sort_Name, Sort_A_Code, Sort_A_Name, Sort_B_Code, Sort_B_Name, Up_Code, Sort_Step, Sort_Order, Repair_Cycle, Repair_Cycle_Part, Repair_Rate, Sort_Division, Sort_Detail, Enforce_Date, PostDate From Facility_Sort Where Aid = @Aid;";
            using (var connection = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                connection.Open();
                Facility_Sort_Entity aa = await connection.QuerySingleOrDefaultAsync<Facility_Sort_Entity>(sql, new { Aid }, commandType: CommandType.Text);
                return aa;
            }
        }

        /// <summary>
        /// 시설물 분류 마지막 일련번호 얻기
        /// </summary>
        /// <returns></returns>
        public async Task<int> Last_Number()
        {
            string sql = "Select Top 1 Aid From Facility_Sort Order by Aid Desc";
            using var conn = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await conn.QuerySingleOrDefaultAsync<int>(sql, commandType: CommandType.Text);
        }

        /// <summary>
        /// 시설물 분류별 입력된 수
        /// </summary>
        /// <returns></returns>
        public async Task<int> Sort_Code_Last_Number(string Facility_Sort_Code)
        {
            string sql = "Select Count(*) From Facility_Sort Where Up_Code = @Facility_Sort_Code";
            using (var conn = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await conn.QuerySingleOrDefaultAsync<int>(sql, new { Facility_Sort_Code }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 대분류 리스트
        /// </summary>
        /// <returns></returns>
        public async Task<List<Facility_Sort_Entity>> GetList_A_FacilitySort()
        {
            string sql = "Select Aid, Facility_Sort_Code, Sort_Name, Sort_A_Code, Sort_A_Name, Sort_B_Code, Sort_B_Name, Up_Code, Sort_Step, Sort_Order, Repair_Cycle, Repair_Cycle_Part, Repair_Rate, Sort_Division, Sort_Detail From Facility_Sort Where Sort_Step = 'A' Order By Sort_Order Asc;";
            using (var connection = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                //connection.Open();
                var aa = await connection.QueryAsync<Facility_Sort_Entity>(sql, commandType: CommandType.Text);

                return aa.ToList();
            }
        }

        /// <summary>
        /// 소분류 리스트
        /// </summary>
        /// <returns></returns>
        public async Task<List<Facility_Sort_Entity>> GetList_C_FacilitySort()
        {
            string sql = "Select Aid, Facility_Sort_Code, Sort_Name, Sort_A_Code, Sort_A_Name, Sort_B_Code, Sort_B_Name, Repair_Cycle, Repair_Cycle_Part, Repair_Rate, Sort_Division From Facility_Sort Where Sort_Step = 'C' Order By Sort_Order Asc;";
            using (var connection = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                connection.Open();
                var aa = await connection.QueryAsync<Facility_Sort_Entity>(sql, commandType: CommandType.Text);
                return aa.ToList();
            }
        }

        /// <summary>
        /// 소분류 리스트
        /// </summary>
        /// <returns></returns>
        public async Task<List<Facility_Sort_Entity>> GetList_C_FacilitySortA(string Plan_Code, string Sort)
        {
            string sql = "Select a.Aid, a.Facility_Sort_Code, a.Sort_Name, Sort_A_Code, Sort_A_Name, Sort_B_Code, Sort_B_Name, a.Repair_Cycle, a.Repair_Cycle_Part, a.Repair_Rate, a.Sort_Division, (Select Count(*) From Repair_Article Where Repair_Plan_Code = @Plan_Code And Sort_C_Code = a.Aid) as Ar From Facility_Sort as a Where a.Sort_Step = 'C' And a.Sort_Order Like '" + Sort + "%' Order By Sort_Order Asc;";
            using (var connection = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                connection.Open();
                var aa = await connection.QueryAsync<Facility_Sort_Entity>(sql, new { Plan_Code }, commandType: CommandType.Text);
                return aa.ToList();
            }
        }

        /// <summary>
        /// 소분류 리스트 A
        /// </summary>
        /// <returns></returns>
        public async Task<List<Facility_Sort_Entity>> GetList_C_FacilitySort_A(string Sort)
        {
            string sql = "Select a.Aid, a.Facility_Sort_Code, a.Sort_Name, a.Sort_A_Code, a.Sort_A_Name, a.Sort_B_Code, a.Sort_B_Name, a.Repair_Cycle, a.Repair_Cycle_Part, a.Repair_Rate, a.Sort_Division From Facility_Sort as a Where a.Sort_Step = 'C' And a.Sort_Order Like '" + Sort + "%' Order By Sort_Order Asc;";
            using (var connection = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                connection.Open();
                var aa = await connection.QueryAsync<Facility_Sort_Entity>(sql, new { Sort }, commandType: CommandType.Text);
                return aa.ToList();
            }
        }

        /// <summary>
        /// 상위코드로 만든 리스트
        /// </summary>
        /// <returns></returns>
        public async Task<List<Facility_Sort_Entity>> GetList_C_SortB(string Up_Code)
        {
            string sql = "Select Aid, Facility_Sort_Code, Sort_Name, Sort_A_Code, Sort_A_Name, Sort_B_Code, Sort_B_Name, Up_Code, Sort_Step, Sort_Order, Repair_Cycle, Repair_Cycle_Part, Repair_Rate, Sort_Division, Sort_Detail From Facility_Sort Where Sort_Step = 'C' And Up_Code = @Up_Code Order By Sort_Order Asc;";
            using (var connection = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                connection.Open();
                var aa = await connection.QueryAsync<Facility_Sort_Entity>(sql, new { Up_Code }, commandType: CommandType.Text);
                return aa.ToList();
            }
        }

        /// <summary>
        /// 상위코드로 만든 리스트
        /// </summary>
        /// <returns></returns>
        public async Task<List<Facility_Sort_Entity>> GetList_C_Sort_B(string Up_Code, string Plan_Code)
        {
            string sql = "Select b.Aid, b.Facility_Sort_Code, b.Sort_Name, b.Sort_A_Code, b.Sort_A_Name, b.Sort_B_Code, b.Sort_B_Name, b.Up_Code, Sort_Step, b.Sort_Order, b.Repair_Cycle, b.Repair_Cycle_Part, b.Repair_Rate, b.Sort_Division, b.Sort_Detail, (Select Count(*) From Repair_Article as a Where a.Repair_Plan_Code = @Plan_Code And a.Sort_C_Code = b.Aid) as Ar From Facility_Sort as b Where Sort_Step = 'C' And Up_Code = @Up_Code Order By Sort_Order Asc;";
            using (var connection = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                connection.Open();
                var aa = await connection.QueryAsync<Facility_Sort_Entity>(sql, new { Up_Code, Plan_Code }, commandType: CommandType.Text);
                return aa.ToList();
            }
        }

        /// <summary>
        /// 상위코드로 만든 리스트(코드로)
        /// </summary>
        /// <returns></returns>
        public async Task<List<Facility_Sort_Entity>> GetList_FacilitySortA(string Up_Code)
        {
            string sql = "Select Aid, Facility_Sort_Code, Sort_Name, Up_Code, Sort_A_Code, Sort_A_Name, Sort_B_Code, Sort_B_Name, Sort_Step, Sort_Order, Repair_Cycle, Repair_Cycle_Part, Repair_Rate, Sort_Division From Facility_Sort Where Up_Code = @Up_Code Order By Sort_Order Asc;";
            using (var connection = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                connection.Open();
                var aa = await connection.QueryAsync<Facility_Sort_Entity>(sql, new { Up_Code }, commandType: CommandType.Text);
                return aa.ToList();
            }
        }

        /// <summary>
        /// 상위코드로 만든 리스트(코드로)
        /// </summary>
        /// <returns></returns>
        public async Task<List<Facility_Sort_Entity>> GetList_FacilitySort(string Apt_Code, string Repair_Plan_Code, string Up_Code)
        {
            string sql = "Select Aid, Facility_Sort_Code, Sort_Name, Up_Code, Sort_A_Code, Sort_A_Name, Sort_B_Code, Sort_B_Name, Sort_Step, Sort_Order, Repair_Cycle, Repair_Cycle_Part, Repair_Rate, Sort_Division, (Select Count(*) From Repair_Article Where Sort_C_Code = Facility_Sort.Aid And Apt_Code = @Apt_Code And Repair_Plan_Code = @Repair_Plan_Code) As Ar From Facility_Sort Where Up_Code = @Up_Code Order By Sort_Order Asc;";
            using (var connection = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                connection.Open();
                var aa = await connection.QueryAsync<Facility_Sort_Entity>(sql, new { Apt_Code, Repair_Plan_Code, Up_Code }, commandType: CommandType.Text);
                return aa.ToList();
            }
        }

        /// <summary>
        /// 상위코드로 만든 리스트(코드로)
        /// </summary>
        /// <returns></returns>
        public async Task<List<Facility_Sort_Entity>> GetList_Sort_A_List(string Sort_A_Code)
        {
            string sql = "Select Aid, Facility_Sort_Code, Sort_Name, Sort_A_Code, Sort_A_Name, Sort_B_Code, Sort_B_Name, Up_Code, Sort_Step, Sort_Order, Repair_Cycle, Repair_Cycle_Part, Repair_Rate, Sort_Division, Sort_Detail From Facility_Sort WHERE Sort_A_Code = @Sort_A_Code And Sort_Step ='C' Order By Sort_Order Asc;";
            using (var connection = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                connection.Open();
                var aa = await connection.QueryAsync<Facility_Sort_Entity>(sql, new { Sort_A_Code }, commandType: CommandType.Text);
                return aa.ToList();
            }
        }

        /// <summary>
        /// 상위코드로 만든 리스트(코드로)
        /// </summary>
        /// <returns></returns>
        public async Task<List<Facility_Sort_Entity>> GetList_Sort_AA_List(string Apt_Code, string Repair_Plan_Code, string Sort_A_Code)
        {
            string sql = "Select Aid, Facility_Sort_Code, Sort_Name, Sort_A_Code, Sort_A_Name, Sort_B_Code, Sort_B_Name, Repair_Cycle, Repair_Cycle_Part, Repair_Rate, Sort_Division, (Select Count(*) From Repair_Article Where Sort_C_Code = Facility_Sort.Aid And Apt_Code = @Apt_Code And Repair_Plan_Code = @Repair_Plan_Code) As Ar From Facility_Sort WHERE Sort_A_Code = @Sort_A_Code And Sort_Step ='C' Order By Sort_Order Asc;";
            using (var connection = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                connection.Open();
                var aa = await connection.QueryAsync<Facility_Sort_Entity>(sql, new { Apt_Code, Repair_Plan_Code, Sort_A_Code }, commandType: CommandType.Text);
                return aa.ToList();
            }
        }

        /// <summary>
        /// 설물분류 코드 분류명 불러오기
        /// </summary>
        /// <returns></returns>
        public async Task<string> DetailName_FacilitySort(string Aid)
        {
            string sql = "Select Sort_Name From Facility_Sort Where Aid = @Aid";
            using (var conn = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await conn.QuerySingleOrDefaultAsync<string>(sql, new { Aid }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 설물분류 코드 분류명 불러오기
        /// </summary>
        /// <returns></returns>
        public async Task<string> Detail_FacilitySort_Division(string Aid)
        {
            string sql = "Select Sort_Division From Facility_Sort Where Aid = @Aid";
            using (var conn = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await conn.QuerySingleOrDefaultAsync<string>(sql, new { Aid }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 시설물분류 코드명 해당 정보 불러오기
        /// </summary>
        /// <param name="Aid"></param>
        /// <returns></returns>
        public async Task<Facility_Sort_Entity> Detail_Sort(string Aid)
        {
            string sql = "Select Aid, Facility_Sort_Code, Sort_Name, Sort_A_Code, Sort_A_Name, Sort_B_Code, Sort_B_Name, Up_Code, Sort_Step, Sort_Order, Repair_Cycle, Repair_Cycle_Part, Repair_Rate, Sort_Division, Sort_Detail, Enforce_Date, PostDate, OrderBy From Facility_Sort Where Aid = @Aid;";
            using (var connection = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                connection.Open();
                Facility_Sort_Entity aa = await connection.QuerySingleOrDefaultAsync<Facility_Sort_Entity>(sql, new { Aid }, commandType: CommandType.Text);
                return aa;
            }
        }

        /// <summary>
        /// 설물분류 코드 분류명 불러오기
        /// </summary>
        /// <returns></returns>
        public async Task<string> DetailCode_FacilitySort(string Aid)
        {
            string sql = "Select Up_Code From Facility_Sort Where Aid = @Aid";
            using (var conn = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await conn.QuerySingleOrDefaultAsync<string>(sql, new { Aid }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 설물분류 코드 분류명 불러오기
        /// </summary>
        /// <returns></returns>
        public async Task<string> FacilitySort_Order(string Aid)
        {
            string sql = "Select Sort_Order From Facility_Sort Where Aid = @Aid";
            using (var conn = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await conn.QuerySingleOrDefaultAsync<string>(sql, new { Aid }, commandType: CommandType.Text);
            }
        }
    }
}