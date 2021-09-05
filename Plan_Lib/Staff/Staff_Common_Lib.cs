using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Plan_Apt_Lib
{
    /// <summary>
    /// 부서 직책 정보
    /// </summary>
    public class Staff_PostDuty
    {
        private readonly IConfiguration _db;

        public Staff_PostDuty(IConfiguration configuration)
        {
            this._db = configuration;
        }

        // 부서 직책 입력
        public async Task<Staff_PostDuty_Entity> Add_Staff_PostDuty(Staff_PostDuty_Entity PostDuty)
        {
            var sql = "Insert Post_Duty (Sort_Division, PostDuty_Division, PostDuty_Code,  PostDuty_Name, Up_Code, PostDuty_Step, Staff_Code, PostDuty_Etc, PostIP) Values (@Sort_Division, @PostDuty_Division, @PostDuty_Code,  @PostDuty_Name, @Up_Code, @PostDuty_Step, @Staff_Code, @PostDuty_Etc, @PostIP);";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, PostDuty, commandType: CommandType.Text);
                return PostDuty;
            }

            //this.ctx.Execute(sql, PostDuty);
            //return PostDuty;
        }

        // 부서 및 직책 수정하기
        public async Task<Staff_PostDuty_Entity> Edit_PostDuty(Staff_PostDuty_Entity PostDuty)
        {
            var sql = "Update Post_Duty Set Sort_Division = @Sort_Division, PostDuty_Division = @PostDuty_Division, PostDuty_Name = @PostDuty_Name, Up_Code = @Up_Code, PostDuty_Step = @PostDuty_Step, PostDuty_Etc = @PostDuty_Etc, Staff_Code = @Staff_Code, PostDate = @PostDate, PostIP = @PostIP Where Aid = @Aid;";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, PostDuty, commandType: CommandType.Text);
                return PostDuty;
            }

            //this.ctx.Execute(sql, PostDuty);
            //return PostDuty;
        }

        // 부서 및 직책 정보 삭제
        public async Task Remove(string Aid)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync("Delete Post_Duty Where Aid = @Aid", new { Aid }, commandType: CommandType.Text);
            }
            //this.ctx.Execute("Delete Post_Duty Where Aid = @Aid", new { Aid });
        }

        // 부서 및 직책 마지막 일련번호 얻기
        public async Task<int> Last_Number()
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Top 1 Aid From Post_Duty Order by Aid Desc", commandType: CommandType.Text);
            }
            //return this.ctx.Query<int>("Select Top 1 Aid From Post_Duty Order by Aid Desc", new { }).SingleOrDefault();
        }

        // 해당 존재 여부 확인
        public async Task<int> Being(string Aid)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Post_Duty Where Aid = @Aid", new { Aid }, commandType: CommandType.Text);
            }
            //return this.ctx.Query<int>("Select Count(*) From Post_Duty Where Aid = @Aid", new { Aid }).SingleOrDefault();
        }

        // 부서 및 직책 상위코드로 검색 리스트
        public async Task<List<Staff_PostDuty_Entity>> GetList_PostDuty_Up(string Up_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var apt = await ctx.QueryAsync<Staff_PostDuty_Entity>("Select * From Post_Duty Where Up_Code = @Up_Code Order By Aid Asc", new { Up_Code }, commandType: CommandType.Text);
                return apt.ToList();
            }
            // this.ctx.Query<Staff_PostDuty>("Select * From Post_Duty Where Up_Code = @Up_Code Order By Aid Asc", new { Up_Code }).ToList();
        }

        // 부서 및 직책 분류 코드로 검색 리스트
        public async Task<List<Staff_PostDuty_Entity>> GetList_PostDuty_Sort(string Sort_Division)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var apt = await ctx.QueryAsync<Staff_PostDuty_Entity>("Select * From Post_Duty Where Sort_Division = @Sort_Division Order By Aid Asc", new { Sort_Division }, commandType: CommandType.Text);
                return apt.ToList();
            }
            // this.ctx.Query<Staff_PostDuty>("Select * From Post_Duty Where Sort_Division = @Sort_Division Order By Aid Asc", new { Sort_Division }).ToList();
        }

        // 부서 및 직책 분류 코드로 검색 리스트
        public async Task<List<Staff_PostDuty_Entity>> GetList_PostDuty_Sort_Drop(string Sort_Division, string PostDuty_Division)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var apt = await ctx.QueryAsync<Staff_PostDuty_Entity>("Select * From Post_Duty Where Sort_Division = @Sort_Division And PostDuty_Division = @PostDuty_Division Order By Aid Asc", new { Sort_Division, PostDuty_Division }, commandType: CommandType.Text);
                return apt.ToList();
            }
            //return this.ctx.Query<Staff_PostDuty>("Select * From Post_Duty Where Sort_Division = @Sort_Division And PostDuty_Division = @PostDuty_Division Order By Aid Asc", new { Sort_Division, PostDuty_Division }).ToList();
        }

        // 부서 및 직책 검색 리스트
        public async Task<List<Staff_PostDuty_Entity>> GetList_PostDuty(string PostDuty_Division)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var apt = await ctx.QueryAsync<Staff_PostDuty_Entity>("Select * From Post_Duty Where PostDuty_Division = @PostDuty_Division Order By Aid Asc", new { PostDuty_Division }, commandType: CommandType.Text);
                return apt.ToList();
            }
            // return this.ctx.Query<Staff_PostDuty>("Select * From Post_Duty Where PostDuty_Division = @PostDuty_Division Order By Aid Asc", new { PostDuty_Division }).ToList();
        }

        // 부서 및 직책 구분 검색 리스트
        public async Task<List<Staff_PostDuty_Entity>> GetList_PostDuty_Division(string PostDuty_Division, string Sort_Division)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var apt = await ctx.QueryAsync<Staff_PostDuty_Entity>("Select * From Post_Duty Where PostDuty_Division = @PostDuty_Division And Sort_Division = @Sort_Division Order By Aid Asc", new { PostDuty_Division, Sort_Division }, commandType: CommandType.Text);
                return apt.ToList();
            }
            //return this.ctx.Query<Staff_PostDuty>("Select * From Post_Duty Where PostDuty_Division = @PostDuty_Division And Sort_Division = @Sort_Division Order By Aid Asc", new { PostDuty_Division, Sort_Division }).ToList();
        }

        // 시설물분류 코드 상위분류코드 불러오기
        public async Task<string> Detail_PostDuty_Name(string PostDuty_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<string>("Select PostDuty_Name From Post_Duty Where PostDuty_Code = @PostDuty_Code", new { PostDuty_Code }, commandType: CommandType.Text);
            }
            //return this.ctx.Query<string>("Select PostDuty_Name From Post_Duty Where PostDuty_Code = @PostDuty_Code", new { PostDuty_Code }).SingleOrDefault();
        }
    }

    public interface IStaff
    {
        Task<Staff_Entity> Add_Staff(Staff_Entity Staff);

        Task<Staff_Entity> Edit_Staff(Staff_Entity Staff);

        Task<int> Last_Number();

        Task<int> Staff_Being(string Apt_Code);

        Task<int> Staff_Overlap(string Staff_Code);

        Task<List<Staff_Detaill_Staff_Entity>> GetList_Staff_Join(string Apt_Code);

        Task<List<Staff_Detaill_Staff_Entity>> GetList_PostDuty_Up(string Apt_Code, string PostDuty_Code);

        Task<List<Staff_Entity>> GetList_Staff(string Apt_Code);

        Task<Staff_Entity> Detail_Staff(string Apt_Code, string Staff_Code);

        Task<Staff_Entity> Staff_infor(string Staff_Code);

        Task<string> Staff_Name(string Staff_Code);

        Task<string> Staff_Name_Last(string Apt_Code);

        Task Remove(string Aid);

        Task<List<Staff_Entity>> GetList_Staff_Manager(string Apt_Code);

        Task<List<Apt_Staff_Join_Entity>> GetList_Approve();

        Task<List<Apt_Staff_Join_Entity>> GetList_Approve_Sido(string Apt_Adress_Sido);

        Task<List<Apt_Staff_Join_Entity>> GetList_Approve_Gun(string Apt_Adress_Sido, string Apt_Adress_Gun);

        Task<List<Apt_Staff_Join_Entity>> GetList_Search(string SearchFeild, string SearchQuery);

        Task<List<Apt_Staff_Join_Entity>> GetList_Search_Sido(string Apt_Adress_Sido, string SearchFeild, string SearchQuery);

        Task<List<Apt_Staff_Join_Entity>> GetList_Search_Gun(string Apt_Adress_Sido, string Apt_Adress_Gun, string SearchFeild, string SearchQuery);

        Task Edit_Approve(string Aid, string LevelCount);
    }

    /// <summary>
    /// 관리자 정보
    /// </summary>
    public class Staff : IStaff
    {
        private readonly IConfiguration _db;

        public Staff(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 직원정보 입력
        /// </summary>
        public async Task<Staff_Entity> Add_Staff(Staff_Entity Staff)
        {
            var sql = "Insert Staff (Apt_Code, Staff_Division, Staff_Code, Staff_Name, License_Number, License_Order, BirthDate, LevelCount, Staff_Etc, PostIP) Values (@Apt_Code, @Staff_Division, @Staff_Code, @Staff_Name, @License_Number, @License_Order, @BirthDate, @LevelCount, @Staff_Etc, @PostIP);";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, Staff, commandType: CommandType.Text);
                return Staff;
            }

            //this.ctx.Execute(sql, Staff);
            //return Staff;
        }

        /// <summary>
        /// 직원정보 수정하기
        /// </summary>
        public async Task<Staff_Entity> Edit_Staff(Staff_Entity Staff)
        {
            var sql = "Update Staff Set Staff_Division = @Staff_Division, Staff_Name = @Staff_Name, BirthDate = @BirthDate, LevelCount, @LevelCount, Staff_Etc = @Staff_Etc Where Aid = @Aid;";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, Staff, commandType: CommandType.Text);
                return Staff;
            }

            //this.ctx.Execute(sql, Staff);
            //return Staff;
        }

        ///` 직원정보 마지막 일련번호 얻기
        public async Task<int> Last_Number()
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Top 1 Aid From Staff Order by Aid Desc", commandType: CommandType.Text);
            }
            //return this.ctx.Query<int>("Select Top 1 Aid From Staff Order by Aid Desc", new { }).SingleOrDefault();
        }

        /// <summary>
        /// 해당 공동주택에 입력된 관리자 수 불러오기
        /// </summary>
        /// <param name="Apt_Code">공동주택 식별코드</param>
        /// <returns>입력된 관리자 수</returns>
        public async Task<int> Staff_Being(string Apt_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Staff Where Apt_Code = @Apt_Code", new { Apt_Code }, commandType: CommandType.Text);
            }
            //return ctx.Query<int>("Select Count(*) From Staff Where Apt_Code = @Apt_Code", new { Apt_Code }).SingleOrDefault();
        }

        // 직원정보 마지막 일련번호 얻기
        public async Task<int> Staff_Overlap(string Staff_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Staff Where Staff_Code = @Staff_Code", new { Staff_Code }, commandType: CommandType.Text);
            }
            //return this.ctx.Query<int>("Select Count(*) From Staff Where Staff_Code = @Staff_Code", new { Staff_Code }).SingleOrDefault();
        }

        // 단지별 직원 검색 리스트(조인)
        public async Task<List<Staff_Detaill_Staff_Entity>> GetList_Staff_Join(string Apt_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var apt = await ctx.QueryAsync<Staff_Detaill_Staff_Entity>("Select * From Post_Duty Where Up_Code = @Up_Code Order By Aid Asc", new { Apt_Code }, commandType: CommandType.Text);
                return apt.ToList();
            }
            //return this.ctx.Query<Staff_Detaill_Staff>("Select * From Post_Duty Where Up_Code = @Up_Code Order By Aid Asc", new { Apt_Code }).ToList();
        }

        // 단지 부서별 직원 검색 리스트
        public async Task<List<Staff_Detaill_Staff_Entity>> GetList_PostDuty_Up(string Apt_Code, string PostDuty_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var apt = await ctx.QueryAsync<Staff_Detaill_Staff_Entity>("Select * From Post_Duty Where Up_Code = @Up_Code Order By Aid Asc", new { Apt_Code, PostDuty_Code }, commandType: CommandType.Text);
                return apt.ToList();
            }
            //return this.ctx.Query<Staff_Detaill_Staff>("Select * From Post_Duty Where Up_Code = @Up_Code Order By Aid Asc", new { Apt_Code }).ToList();
        }

        // 단지별 직원 검색 리스트
        public async Task<List<Staff_Entity>> GetList_Staff(string Apt_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var apt = await ctx.QueryAsync<Staff_Entity>("Select Aid, Apt_Code, Staff_Division, Staff_Code, Staff_Name, BirthDate, License_Number, License_Order, LevelCount, Staff_Etc, PostDate From Staff Where Apt_Code = @Apt_Code Order By Aid Asc", new { Apt_Code }, commandType: CommandType.Text);
                return apt.ToList();
            }
            //return this.ctx.Query<Staff>("Select Aid, Apt_Code, Staff_Division, Staff_Code, Staff_Name, BirthDate, License_Number, License_Order, LevelCount, Staff_Etc, PostDate From Staff Where Apt_Code = @Apt_Code Order By Aid Asc", new { Apt_Code }).ToList();
        }

        /// <summary>
        /// 직원코드로 불러오기((공동주택 코드, 관리자 코드)
        /// </summary>
        public async Task<Staff_Entity> Detail_Staff(string Apt_Code, string Staff_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Staff_Entity>("Select Aid, Apt_Code, Staff_Division, Staff_Code, Staff_Name, BirthDate, License_Number, License_Order, LevelCount, Staff_Etc, PostDate From Staff Where Apt_Code = @Apt_Code And Staff_Code = @Staff_Code", new { Apt_Code, Staff_Code }, commandType: CommandType.Text);
            }
            //return this.ctx.Query<Staff>("Select Aid, Apt_Code, Staff_Division, Staff_Code, Staff_Name, BirthDate, License_Number, License_Order, LevelCount, Staff_Etc, PostDate From Staff Where Apt_Code = @Apt_Code And Staff_Code = @Staff_Code", new { Apt_Code, Staff_Code }).SingleOrDefault();
        }

        /// <summary>
        /// 직원코드로 불러오기
        /// </summary>
        public async Task<Staff_Entity> Staff_infor(string Staff_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Staff_Entity>("Select Aid, Apt_Code, Staff_Division, Staff_Code, Staff_Name, BirthDate, License_Number, License_Order, LevelCount, Staff_Etc, PostDate From Staff Where Staff_Code = @Staff_Code", new { Staff_Code }, commandType: CommandType.Text);
            }
            //return this.ctx.Query<Staff>("Select Aid, Apt_Code, Staff_Division, Staff_Code, Staff_Name, BirthDate, License_Number, License_Order, LevelCount, Staff_Etc, PostDate From Staff Where Staff_Code = @Staff_Code", new { Staff_Code }).SingleOrDefault();
        }

        // 직원 코드로 직원 이름 불러오기
        public async Task<string> Staff_Name(string Staff_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<string>("Select Staff_Name From Staff Where Staff_Code = @Staff_Code", new { Staff_Code }, commandType: CommandType.Text);
            }
            //return this.ctx.Query<string>("Select Staff_Name From Staff Where Staff_Code = @Staff_Code ", new { Staff_Code }).SingleOrDefault();
        }

        // 공동주택 코드로 현재 관리소장 이름 불러오기
        public async Task<string> Staff_Name_Last(string Apt_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<string>("", new { Apt_Code }, commandType: CommandType.Text);
            }
            //return this.ctx.Query<string>("Select Top 1 Staff_Name From Staff Where Apt_Code = @Apt_Code Order By Aid Desc", new { Apt_Code }).SingleOrDefault();
        }

        // 직원 정보 삭제
        public async Task Remove(string Aid)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync("Delete Staff Where Aid = @Aid", new { Aid }, commandType: CommandType.Text);
            }
            //this.ctx.Execute("Delete Staff Where Aid = @Aid", new { Aid });
        }

        // 단지별 직원 검색 관리자 정보
        public async Task<List<Staff_Entity>> GetList_Staff_Manager(string Apt_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var apt = await ctx.QueryAsync<Staff_Entity>("Select Top 1 * From Staff Where Apt_Code = @Apt_Code Order By LevelCount Desc", new { Apt_Code }, commandType: CommandType.Text);
                return apt.ToList();
            }
            //return this.ctx.Query<Staff>("Select Top 1 * From Staff Where Apt_Code = @Apt_Code Order By LevelCount Desc", new { Apt_Code }).ToList();
        }

        // 사용신청 중인 공동주택 신청 목록
        public async Task<List<Apt_Staff_Join_Entity>> GetList_Approve()
        {
            var sql = "Select a.Aid, a.Apt_Code, a.Apt_Name, a.Apt_Adress_Sido, a.Apt_Adress_Gun, a.Apt_Adress_Rest, a.Apt_Form, a.LevelCount, a.AcceptancedOfWork_Date, a.CorporateResistration_Num, a.PostDate, b.PostDate, b.Aid As Staff_Aid, b.Staff_Code, b.Staff_Name, b.Staff_Division, b.LevelCount As Staff_LevelCount From Apt As a Join Staff As b On a.Apt_Code = b.Apt_Code Order By b.Aid Desc";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var apt = await ctx.QueryAsync<Apt_Staff_Join_Entity>(sql, commandType: CommandType.Text);
                return apt.ToList();
            }
            //return this.ctx.Query<Apt_Staff_Join_Entity>(sql).ToList();
        }

        // 사용신청 중인 공동주택 신청 목록(시도별)
        public async Task<List<Apt_Staff_Join_Entity>> GetList_Approve_Sido(string Apt_Adress_Sido)
        {
            var sql = "Select a.Aid, a.Apt_Code, a.Apt_Name, a.Apt_Adress_Sido, a.Apt_Adress_Gun, a.Apt_Adress_Rest, a.Apt_Form, a.LevelCount, a.AcceptancedOfWork_Date, a.CorporateResistration_Num, a.PostDate, b.PostDate, b.Aid As Staff_Aid, b.Staff_Code, b.Staff_Name, b.Staff_Division, b.LevelCount As Staff_LevelCount From Apt As a Join Staff As b On a.Apt_Code = b.Apt_Code Where a.Apt_Adress_Sido = @Apt_Adress_Sido Order By b.Aid Desc";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var apt = await ctx.QueryAsync<Apt_Staff_Join_Entity>(sql, new { Apt_Adress_Sido }, commandType: CommandType.Text);
                return apt.ToList();
            }
            //return this.ctx.Query<Apt_Staff_Join_Entity>(sql, new { Apt_Adress_Sido }).ToList();
        }

        // 사용신청 중인 공동주택 신청 목록(군별)
        public async Task<List<Apt_Staff_Join_Entity>> GetList_Approve_Gun(string Apt_Adress_Sido, string Apt_Adress_Gun)
        {
            var sql = "Select a.Aid, a.Apt_Code, a.Apt_Name, a.Apt_Adress_Sido, a.Apt_Adress_Gun, a.Apt_Adress_Rest, a.Apt_Form, a.LevelCount, a.AcceptancedOfWork_Date, a.CorporateResistration_Num, a.PostDate, b.PostDate, b.Aid As Staff_Aid, b.Staff_Code, b.Staff_Name, b.Staff_Division, b.LevelCount As Staff_LevelCount From Apt As a Join Staff As b On a.Apt_Code = b.Apt_Code Where a.Apt_Adress_Sido = @Apt_Adress_Sido And a.Apt_Adress_Gun = @Apt_Adress_Gun Order By b.Aid Desc";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var apt = await ctx.QueryAsync<Apt_Staff_Join_Entity>(sql, new { Apt_Adress_Sido, Apt_Adress_Gun }, commandType: CommandType.Text);
                return apt.ToList();
            }
            // return this.ctx.Query<Apt_Staff_Join_Entity>(sql, new { Apt_Adress_Sido, Apt_Adress_Gun }).ToList();
        }

        // 전체 검색
        public async Task<List<Apt_Staff_Join_Entity>> GetList_Search(string SearchFeild, string SearchQuery)
        {
            var sql = "Select a.Aid, a.Apt_Code, a.Apt_Name, a.Apt_Adress_Sido, a.Apt_Adress_Gun, a.Apt_Adress_Rest, a.Apt_Form, a.LevelCount, a.AcceptancedOfWork_Date, a.CorporateResistration_Num, a.PostDate, b.PostDate, b.Aid As Staff_Aid, b.Staff_Code, b.Staff_Name, b.Staff_Division, b.LevelCount As Staff_LevelCount From Apt As a Join Staff As b On a.Apt_Code = b.Apt_Code Where " + SearchFeild + " Like '%" + SearchQuery + "%' Order By b.Aid Desc";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var apt = await ctx.QueryAsync<Apt_Staff_Join_Entity>(sql, new { SearchFeild, SearchQuery }, commandType: CommandType.Text);
                return apt.ToList();
            }
            //return this.ctx.Query<Apt_Staff_Join_Entity>(sql, new { SearchFeild, SearchQuery }).ToList();
        }

        // 전체 검색(시도별)
        public async Task<List<Apt_Staff_Join_Entity>> GetList_Search_Sido(string Apt_Adress_Sido, string SearchFeild, string SearchQuery)
        {
            var sql = "Select a.Aid, a.Apt_Code, a.Apt_Name, a.Apt_Adress_Sido, a.Apt_Adress_Gun, a.Apt_Adress_Rest, a.Apt_Form, a.LevelCount, a.AcceptancedOfWork_Date, a.CorporateResistration_Num, a.PostDate, b.PostDate, b.Aid As Staff_Aid, b.Staff_Code, b.Staff_Name, b.Staff_Division, b.LevelCount As Staff_LevelCount From Apt As a Join Staff As b On a.Apt_Code = b.Apt_Code Where  a.Apt_Adress_Sido = @Apt_Adress_Sido And " + SearchFeild + " Like '%" + SearchQuery + "%' Order By b.Aid Desc";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var apt = await ctx.QueryAsync<Apt_Staff_Join_Entity>(sql, new { Apt_Adress_Sido, SearchFeild, SearchQuery }, commandType: CommandType.Text);
                return apt.ToList();
            }
            //return this.ctx.Query<Apt_Staff_Join_Entity>(sql, new { Apt_Adress_Sido, SearchFeild, SearchQuery }).ToList();
        }

        // 전체 검색(시군구별)
        public async Task<List<Apt_Staff_Join_Entity>> GetList_Search_Gun(string Apt_Adress_Sido, string Apt_Adress_Gun, string SearchFeild, string SearchQuery)
        {
            var sql = "Select a.Aid, a.Apt_Code, a.Apt_Name, a.Apt_Adress_Sido, a.Apt_Adress_Gun, a.Apt_Adress_Rest, a.Apt_Form, a.LevelCount, a.AcceptancedOfWork_Date, a.CorporateResistration_Num, a.PostDate, b.PostDate, b.Aid As Staff_Aid, b.Staff_Code, b.Staff_Name, b.Staff_Division, b.LevelCount As Staff_LevelCount From Apt As a Join Staff As b On a.Apt_Code = b.Apt_Code Where  a.Apt_Adress_Sido = @Apt_Adress_Sido And a.Apt_Adress_Gun = @Apt_Adress_Gun And " + SearchFeild + " Like '%" + SearchQuery + "%' Order By b.Aid Desc";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var apt = await ctx.QueryAsync<Apt_Staff_Join_Entity>(sql, new { Apt_Adress_Sido, Apt_Adress_Gun, SearchFeild, SearchQuery }, commandType: CommandType.Text);
                return apt.ToList();
            }
            //return this.ctx.Query<Apt_Staff_Join_Entity>(sql, new { Apt_Adress_Sido, Apt_Adress_Gun, SearchFeild, SearchQuery }).ToList();
        }

        // 사용신청 관리자 사용승인
        public async Task Edit_Approve(string Aid, string LevelCount)
        {
            var sql = "Update Staff Set LevelCount = @LevelCount Where Aid = @Aid";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, new { Aid, LevelCount }, commandType: CommandType.Text);
            }
            //this.ctx.Execute(sql, new { Aid, LevelCount });
        }
    }

    public interface IApt_Staff
    {
        Task<Apt_Staff_Entity> Add_Staff(Apt_Staff_Entity Staff);

        Task<Apt_Staff_Entity> Edit_Staff(Apt_Staff_Entity Staff);

        Task<int> Last_Number(string Apt_Code);

        Task<List<Staff_Detaill_Staff_Entity>> GetList_Staff_Join(string Apt_Code);

        Task<List<Staff_Detaill_Staff_Entity>> GetList_Post(string Apt_Code, string Post_Code);

        Task<List<Staff_Detaill_Staff_Entity>> GetList_Staff_Search(string Apt_Code, string Staff_Name);

        Task<Staff_Detaill_Staff_Entity> Detail_Staff(string Apt_Code, string Apt_Staff_Code);

        Task<string> Staff_Name(string Apt_Staff_Code);

        Task<int> Being(string Apt_Code, string Staff_Name, string BirthDate);

        Task<string> Staff_Name_Last(string Apt_Code);

        Task Remove(string Aid);

        Task<List<Staff>> GetList_Staff_Manager(string Apt_Code);

        Task<List<Staff_Detaill_Staff_Entity>> GetList_Post_Staff(string Apt_Code, string Post_Code);
    }

    /// <summary>
    /// 공동주택 직원 정보
    /// </summary>
    public class Apt_Staff : IApt_Staff
    {
        private readonly IConfiguration _db;

        public Apt_Staff(IConfiguration configuration)
        {
            this._db = configuration;
        }

        // 직원정보 입력
        public async Task<Apt_Staff_Entity> Add_Staff(Apt_Staff_Entity Staff)
        {
            var sql = "Insert Apt_Staff (Apt_Code, Staff_Division, Apt_Staff_Code, Staff_Name, BirthDate, LevelCount, Staff_Etc, PostIP) Values (@Apt_Code, @Staff_Division, @Apt_Staff_Code, @Staff_Name, @BirthDate, @LevelCount, @Staff_Etc, @PostIP);";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, Staff, commandType: CommandType.Text);
                return Staff;
            }
            //dn.ctx.Execute(sql, Staff);
            //return Staff;
        }

        // 직원정보 수정하기
        public async Task<Apt_Staff_Entity> Edit_Staff(Apt_Staff_Entity Staff)
        {
            var sql = "Update Apt_Staff Set Staff_Division = @Staff_Division, Staff_Name = @Staff_Name, BirthDate = @BirthDate, LevelCount = @LevelCount, Staff_Etc = @Staff_Etc Where Aid = @Aid";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, Staff, commandType: CommandType.Text);
                return Staff;
            }
            //this.dn.ctx.Execute(sql, Staff);
            //return Staff;
        }

        // 직원정보 마지막 일련번호 얻기
        public async Task<int> Last_Number(string Apt_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Apt_Staff Where Apt_Code = @Apt_Code", new { Apt_Code }, commandType: CommandType.Text);
            }
            //return dn.ctx.Query<int>("Select Count(*) From Apt_Staff Where Apt_Code = @Apt_Code", new { Apt_Code }).SingleOrDefault();
        }

        // 단지별 직원 검색 리스트(조인)
        public async Task<List<Staff_Detaill_Staff_Entity>> GetList_Staff_Join(string Apt_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var apt = await ctx.QueryAsync<Staff_Detaill_Staff_Entity>("Select * From Apt_Staff As a Join Staff_Detail As b On a.Apt_Code = b.Apt_Code And a.Apt_Staff_Code = b.Apt_Staff_Code Where  a.Apt_Code = @Apt_Code And a.Del = 'A' Order By a.Aid Asc", new { Apt_Code }, commandType: CommandType.Text);
                return apt.ToList();
            }
            //return dn.ctx.Query<Staff_Detaill_Staff>("Select * From Apt_Staff As a Join Staff_Detail As b On a.Apt_Code = b.Apt_Code And a.Apt_Staff_Code = b.Apt_Staff_Code Where  a.Apt_Code = @Apt_Code And a.Del = 'A' Order By a.Aid Asc", new { Apt_Code }).ToList();
        }

        // 단지 부서별 직원 검색 리스트
        public async Task<List<Staff_Detaill_Staff_Entity>> GetList_Post(string Apt_Code, string Post_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var apt = await ctx.QueryAsync<Staff_Detaill_Staff_Entity>("Select * From Apt_Staff As a Join Staff_Detail As b On a.Apt_Code = b.Apt_Code And a.Apt_Staff_Code = b.Apt_Staff_Code Where  a.Apt_Code = @Apt_Code, And b.Post_Code = @Post_Code And a.Del = 'A' Order By a.Aid Asc", new { Apt_Code, Post_Code }, commandType: CommandType.Text);
                return apt.ToList();
            }
            //return dn.ctx.Query<Staff_Detaill_Staff>("Select * From Apt_Staff As a Join Staff_Detail As b On a.Apt_Code = b.Apt_Code And a.Apt_Staff_Code = b.Apt_Staff_Code Where  a.Apt_Code = @Apt_Code, And b.Post_Code = @Post_Code And a.Del = 'A' Order By a.Aid Asc", new { Apt_Code, Post_Code }).ToList();
        }

        // 단지별 직원 검색 리스트
        public async Task<List<Staff_Detaill_Staff_Entity>> GetList_Staff_Search(string Apt_Code, string Staff_Name)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var apt = await ctx.QueryAsync<Staff_Detaill_Staff_Entity>("Search_AptStaff", new { Apt_Code, Staff_Name }, commandType: CommandType.StoredProcedure);
                return apt.ToList();
            }
            //return dn.ctx.Query<Staff_Detaill_Staff>("Search_AptStaff", new { Apt_Code, Staff_Name }, commandType: CommandType.StoredProcedure).ToList();
        }

        // 직원코드로 상세 불러오기
        public async Task<Staff_Detaill_Staff_Entity> Detail_Staff(string Apt_Code, string Apt_Staff_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Staff_Detaill_Staff_Entity>("Select * From Apt_Staff As a Join Staff_Detail As b On a.Apt_Code = b.Apt_Code And a.Apt_Staff_Code = b.Apt_Staff_Code Where  a.Apt_Code = @Apt_Code And Apt_Staff_Code = @Apt_Staff_Code", new { Apt_Code, Apt_Staff_Code }, commandType: CommandType.Text);
            }
            //return dn.ctx.Query<Staff_Detaill_Staff>("Select * From Apt_Staff As a Join Staff_Detail As b On a.Apt_Code = b.Apt_Code And a.Apt_Staff_Code = b.Apt_Staff_Code Where  a.Apt_Code = @Apt_Code And Apt_Staff_Code = @Apt_Staff_Code", new { Apt_Code, Apt_Staff_Code }).SingleOrDefault();
        }

        // 직원 코드로 직원 이름 불러오기
        public async Task<string> Staff_Name(string Apt_Staff_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<string>("Select Staff_Name From Apt_Staff Where Apt_Staff_Code = @Apt_Staff_Code", new { Apt_Staff_Code }, commandType: CommandType.Text);
            }
            //return dn.ctx.Query<string>("Select Staff_Name From Apt_Staff Where Apt_Staff_Code = @Apt_Staff_Code ", new { Apt_Staff_Code }).SingleOrDefault();
        }

        // 중복 여부
        public async Task<int> Being(string Apt_Code, string Staff_Name, string BirthDate)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Apt_Staff Where Apt_Code = @Apt_Code And Staff_Name = @Staff_Name And BirthDate = @BirthDate", new { Apt_Code, Staff_Name, BirthDate }, commandType: CommandType.Text);
            }
            //return dn.ctx.Query<int>("Select Count(*) From Apt_Staff Where Apt_Code = @Apt_Code And Staff_Name = @Staff_Name And BirthDate = @BirthDate", new { Apt_Code, Staff_Name, BirthDate }).SingleOrDefault();
        }

        // 공동주택 코드로 현재 관리소장 이름 불러오기
        public async Task<string> Staff_Name_Last(string Apt_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<string>("Select Top 1 Staff_Name From Staff Where Apt_Code = @Apt_Code Order By Aid Desc", new { Apt_Code }, commandType: CommandType.Text);
            }
            //return dn.ctx.Query<string>("Select Top 1 Staff_Name From Staff Where Apt_Code = @Apt_Code Order By Aid Desc", new { Apt_Code }).SingleOrDefault();
        }

        // 직원 정보 삭제
        public async Task Remove(string Aid)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync("Update Apt_Staff Set Del = 'B' Where Aid = @Aid", new { Aid }, commandType: CommandType.Text);
            }
            //dn.ctx.Execute("Update Apt_Staff Set Del = 'B' Where Aid = @Aid", new { Aid });
        }

        // 단지별 직원 검색 관리자 정보
        public async Task<List<Staff>> GetList_Staff_Manager(string Apt_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var apt = await ctx.QueryAsync<Staff>("Select Top 1 * From Staff Where Apt_Code = @Apt_Code Order By LevelCount Desc", new { Apt_Code }, commandType: CommandType.Text);
                return apt.ToList();
            }
            // return dn.ctx.Query<Staff>("Select Top 1 * From Staff Where Apt_Code = @Apt_Code Order By LevelCount Desc", new { Apt_Code }).ToList();
        }

        // 단지별 부서별 정보 불러오기
        public async Task<List<Staff_Detaill_Staff_Entity>> GetList_Post_Staff(string Apt_Code, string Post_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var apt = await ctx.QueryAsync<Staff_Detaill_Staff_Entity>("Select * From Apt_Staff As a Join Staff_Detail As b On a.Apt_Code = b.Apt_Code and a.Apt_Staff_Code = b.Apt_Staff_Code Where a.Apt_Code = @Apt_Code And b.Post_Code = @Post_Code And a.Del = 'A' Order By a.Aid Desc", new { Apt_Code, Post_Code }, commandType: CommandType.Text);
                return apt.ToList();
            }
            //return dn.ctx.Query<Staff_Detaill_Staff>("Select * From Apt_Staff As a Join Staff_Detail As b On a.Apt_Code = b.Apt_Code and a.Apt_Staff_Code = b.Apt_Staff_Code Where a.Apt_Code = @Apt_Code And b.Post_Code = @Post_Code And a.Del = 'A' Order By a.Aid Desc", new { Apt_Code, Post_Code }).ToList();
        }
    }

    public interface IStaff_Detail
    {
        Task<Staff_Detail_Entity> Add_Staff_Detail(Staff_Detail_Entity Staff);

        Task<Staff_Detail_Entity> Edit_Staff(Staff_Detail_Entity Staff);

        Task<Staff_Detail_Entity> Edit_Staff_Resign(Staff_Detail_Entity Staff);

        Task<int> Last_Number();

        Task<int> Being(string Apt_Code, string Apt_Staff_Code);

        Task<Staff_Detaill_Staff_Entity> Detail_Staff(string Staff_Code);

        Task<Staff_Detail> Detail_Staff_Detail(string Apt_Staff_Code);

        Task<List<Staff_Detaill_Staff_Entity>> GetList_StaffDetail(string Apt_Code);

        Task<List<Staff_Detaill_Staff_Entity>> GetList_Post_Staff(string Apt_Code, string Post_Code);
    }

    // 직원 상세 정보
    public class Staff_Detail : IStaff_Detail
    {
        private readonly IConfiguration _db;

        public Staff_Detail(IConfiguration configuration)
        {
            this._db = configuration;
        }

        // 직원상세정보 입력
        public async Task<Staff_Detail_Entity> Add_Staff_Detail(Staff_Detail_Entity Staff)
        {
            var sql = "Insert Staff_Detail (Staff_Detail_Code, Apt_Code, Apt_Staff_Code, Post_Code, Duty_Code, Staff_Division, Staff_Adress, Telephone, Email, JoinDate, Staff_Detail_Etc, PostIP) Values (@Staff_Detail_Code, @Apt_Code, @Apt_Staff_Code, @Post_Code, @Duty_Code, @Staff_Division, @Staff_Adress, @Telephone, @Email, @JoinDate, @Staff_Detail_Etc, @PostIP);";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, Staff, commandType: CommandType.Text);
                return Staff;
            }
            // this.ctx.Execute(sql, Staff);
            //return Staff;
        }

        // 직원상세정보 수정하기
        public async Task<Staff_Detail_Entity> Edit_Staff(Staff_Detail_Entity Staff)
        {
            var sql = "Update Staff_Detail Set Post_Code = @Post_Code, Duty_Code = @Duty_Code, Staff_Division = @Staff_Division, Telephone = @Telephone, Email = @Email, JoinDate = @JoinDate, Staff_Detail_Etc = @Staff_Detail_Etc Where Detail_Aid = @Detail_Aid;";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, Staff, commandType: CommandType.Text);
                return Staff;
            }
            //this.ctx.Execute(sql, Staff);
            //return Staff;
        }

        // 직원상세정보(퇴사처리) 수정하기
        public async Task<Staff_Detail_Entity> Edit_Staff_Resign(Staff_Detail_Entity Staff)
        {
            var sql = "Update Staff_Detail Set ResignDate = @ResignDate, Resign_Check = @Resign_Check, Staff_Detail_Etc = @Staff_Detail_Etc Where Staff_Code = @Staff_Code;";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, Staff, commandType: CommandType.Text);
                return Staff;
            }
            //this.ctx.Execute(sql, Staff);
            //return Staff;
        }

        // 직원정보 마지막 일련번호 얻기
        public async Task<int> Last_Number()
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Top 1 Aid From Staff_Detail Order by Aid Desc", commandType: CommandType.Text);
            }
            //return this.ctx.Query<int>("Select Top 1 Aid From Staff_Detail Order by Aid Desc", new { }).SingleOrDefault();
        }

        // 중복 체크
        public async Task<int> Being(string Apt_Code, string Apt_Staff_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Staff_Detail Where Apt_Code = @Apt_Code And Apt_Staff_Code = @Apt_Staff_Code", new { Apt_Code, Apt_Staff_Code }, commandType: CommandType.Text);
            }
            //return ctx.Query<int>("Select Count(*) From Staff_Detail Where Apt_Code = @Apt_Code And Apt_Staff_Code = @Apt_Staff_Code ", new { Apt_Code, Apt_Staff_Code }).SingleOrDefault();
        }

        // 직원 및 상세에서 정보 불러오기
        public async Task<Staff_Detaill_Staff_Entity> Detail_Staff(string Staff_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Staff_Detaill_Staff_Entity>("Select * From Apt_Staff As a Join Staff_Detail As b On a.Apt_Code = b.Apt_Code And a.Apt_Staff_Code = b.Apt_Staff_Code Where a.Staff_Code = @Staff_Code", new { Staff_Code }, commandType: CommandType.Text);
            }
            //return this.ctx.Query<Staff_Detaill_Staff>("Select * From Apt_Staff As a Join Staff_Detail As b On a.Apt_Code = b.Apt_Code And a.Apt_Staff_Code = b.Apt_Staff_Code Where a.Staff_Code = @Staff_Code", new { Staff_Code }).SingleOrDefault();
        }

        // 직원코드로 상세 불러오기
        public async Task<Staff_Detail> Detail_Staff_Detail(string Apt_Staff_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Staff_Detail>("Select Top 1 * From Staff_Detail Where Apt_Staff_Code = @Apt_Staff_Code Order By Detail_Aid Desc", new { Apt_Staff_Code }, commandType: CommandType.Text);
            }
            //return this.ctx.Query<Staff_Detail>("Select Top 1 * From Staff_Detail Where Apt_Staff_Code = @Apt_Staff_Code Order By Detail_Aid Desc", new { Apt_Staff_Code }).SingleOrDefault();
        }

        // 직원 및 직원 상세 정보 해당 단지별 정보 불러오기
        public async Task<List<Staff_Detaill_Staff_Entity>> GetList_StaffDetail(string Apt_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var apt = await ctx.QueryAsync<Staff_Detaill_Staff_Entity>("Select * From Apt_Staff As a Join Staff_Detail As b On a.Apt_Code = b.Apt_Code And a.Apt_Staff_Code = b.Apt_Staff_Code Where a.Apt_Code = @Apt_Code And a.Del = 'A' Order By a.Aid Desc", new { Apt_Code }, commandType: CommandType.Text);
                return apt.ToList();
            }
            //return this.ctx.Query<Staff_Detaill_Staff>("Select * From Apt_Staff As a Join Staff_Detail As b On a.Apt_Code = b.Apt_Code And a.Apt_Staff_Code = b.Apt_Staff_Code Where a.Apt_Code = @Apt_Code And a.Del = 'A' Order By a.Aid Desc", new { Apt_Code }).ToList();
        }

        // 단지별 부서별 정보 불러오기
        public async Task<List<Staff_Detaill_Staff_Entity>> GetList_Post_Staff(string Apt_Code, string Post_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var apt = await ctx.QueryAsync<Staff_Detaill_Staff_Entity>("Select a.Apt_Staff_Code, a.Staff_Name From Apt_Staff As a Join Staff_Detail As b On a.Apt_Code = b.Apt_Code and a.Apt_Staff_Code = b.Apt_Staff_Code Where a.Apt_Code = @Apt_Code And b.Post_Code = @Post_Code And a.Del = 'A' Order By a.Aid Desc", new { Apt_Code, Post_Code }, commandType: CommandType.Text);
                return apt.ToList();
            }
            //return this.ctx.Query<Staff_Detaill_Staff>("Select a.Apt_Staff_Code, a.Staff_Name From Apt_Staff As a Join Staff_Detail As b On a.Apt_Code = b.Apt_Code and a.Apt_Staff_Code = b.Apt_Staff_Code Where a.Apt_Code = @Apt_Code And b.Post_Code = @Post_Code And a.Del = 'A' Order By a.Aid Desc", new { Apt_Code, Post_Code }).ToList();
        }
    }

    public interface IApt_Worker
    {
        Task<Apt_Worker_Entity> Add(Apt_Worker_Entity aww);

        Task<Apt_Worker_Entity> Edit(Apt_Worker_Entity aww);

        Task<int> Count_Apt(string Apt_Code);

        Task<int> Being(string Apt_Code, string Worker_Name, string Dong, string Ho, int Degree);

        Task Remove(string Aid);

        Task<List<Apt_Worker>> GetList(string Apt_Code);

        Task<List<Apt_Worker_Entity>> GetList_Post(string Apt_Code, string Post);

        Task<List<Apt_Worker_Entity>> GetList_Degree_Post(string Apt_Code, string Degree, string Post);

        Task<int> Degree_New(string Apt_Code);

        Task<Apt_Worker_Entity> Detail(string Aid);
    }

    // 주민 대표 정보
    public class Apt_Worker : IApt_Worker
    {
        private readonly IConfiguration _db;

        public Apt_Worker(IConfiguration configuration)
        {
            this._db = configuration;
        }

        // 주민대표 정보 입력
        public async Task<Apt_Worker_Entity> Add(Apt_Worker_Entity aww)
        {
            var sql = "Insert Into Apt_Worker (Apt_Code, Worker_Name, Worker_Code, Dong, Ho, Post, Duty, Election_Division, Degree, Telephone, Mobile, Email, BirthDate, Start_Date, End_Date, Worker_Etc, PostIP, Staff_Code) Values (@Apt_Code, @Worker_Name, @Worker_Code, @Dong, @Ho, @Post, @Duty, @Election_Division, @Degree, @Telephone, @Mobile, @Email, @BirthDate, @Start_Date, @End_Date, @Worker_Etc, @PostIP, @Staff_Code)";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, aww, commandType: CommandType.Text);
                return aww;
            }
            //dn.ctx.Execute(sql, aww);
            //return aww;
        }

        // 주민대표 정보 수정
        public async Task<Apt_Worker_Entity> Edit(Apt_Worker_Entity aww)
        {
            var sql = "Update Apt_Worker Set Worker_Name = @Worker_Name, Dong = @Dong, Ho = @Ho, Post = @Post, Duty = @Duty, Election_Division = @Election_Division, Degree = @Degree, Telephone =@Telephone, Mobile = @Mobile, Email = @Email, BirthDate = @BirthDate, Start_Date = @Start_Date, End_Date = @End_Date, Worker_Etc = @Worker_Etc, PostDate = @PostDate, PostIP = @PostIP, Staff_Code = @Staff_Code Where Aid = @Aid";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, aww, commandType: CommandType.Text);
                return aww;
            }
            //dn.ctx.Execute(sql, aww);
            //return aww;
        }

        // 공동주택 입력된 수
        public async Task<int> Count_Apt(string Apt_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Apt_Worker Where Apt_Code = @Apt_Code", new { Apt_Code }, commandType: CommandType.Text);
            }
            //return dn.ctx.Query<int>("Select Count(*) From Apt_Worker Where Apt_Code = @Apt_Code", new { Apt_Code }).SingleOrDefault();
        }

        // 중복 여부
        public async Task<int> Being(string Apt_Code, string Worker_Name, string Dong, string Ho, int Degree)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Apt_Worker Where Apt_Code = @Apt_Code And Worker_Name = @Worker_Name And Dong = @Dong And Ho = @Ho And Degree = @Degree", new { Apt_Code, Worker_Name, Dong, Ho, Degree }, commandType: CommandType.Text);
            }
            //return dn.ctx.Query<int>("Select Count(*) From Apt_Worker Where Apt_Code = @Apt_Code And Worker_Name = @Worker_Name And Dong = @Dong And Ho = @Ho And Degree = @Degree", new { Apt_Code, Worker_Name, Dong, Ho, Degree }).SingleOrDefault();
        }

        // 주민대표 정보 삭제
        public async Task Remove(string Aid)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync("Delete Apt_Worker Where Aid = @Aid", new { Aid }, commandType: CommandType.Text);
            }
            //this.dn.ctx.Execute("Delete Apt_Worker Where Aid = @Aid", new { Aid });
        }

        // 주민대표 정보 목록
        public async Task<List<Apt_Worker>> GetList(string Apt_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var apt = await ctx.QueryAsync<Apt_Worker>("Select * From Apt_Worker Where Apt_Code = @Apt_Code Order By Aid Desc", new { Apt_Code }, commandType: CommandType.Text);
                return apt.ToList();
            }
            //return dn.ctx.Query<Apt_Worker>("Select * From Apt_Worker Where Apt_Code = @Apt_Code Order By Aid Desc", new { Apt_Code }).ToList();
        }

        // 주민대표 정보 목록(차수별)
        public async Task<List<Apt_Worker_Entity>> GetList_Post(string Apt_Code, string Post)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var apt = await ctx.QueryAsync<Apt_Worker_Entity>("Select * From Apt_Worker Where Apt_Code = @Apt_Code And Post = @Post Order By Aid Desc", new { Apt_Code, Post }, commandType: CommandType.Text);
                return apt.ToList();
            }
            //return dn.ctx.Query<Apt_Worker>("Select * From Apt_Worker Where Apt_Code = @Apt_Code And Post = @Post Order By Aid Desc", new { Apt_Code, Post }).ToList();
        }

        // 주민대표 정보 목록()
        public async Task<List<Apt_Worker_Entity>> GetList_Degree_Post(string Apt_Code, string Degree, string Post)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var apt = await ctx.QueryAsync<Apt_Worker_Entity>("Select * From Apt_Worker Where Apt_Code = @Apt_Code And Degree = @Degree And Post = @Post Order By Aid Asc", new { Apt_Code, Degree, Post }, commandType: CommandType.Text);
                return apt.ToList();
            }
            //return dn.ctx.Query<Apt_Worker>("Select * From Apt_Worker Where Apt_Code = @Apt_Code And Degree = @Degree And Post = @Post Order By Aid Asc", new { Apt_Code, Degree, Post }).ToList();
        }

        // 최근 동대표 기수
        public async Task<int> Degree_New(string Apt_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Top 1 Degree From Apt_Worker Where Apt_Code = @Apt_Code Order By Aid Desc", new { Apt_Code }, commandType: CommandType.Text);
            }
            //return dn.ctx.Query<int>("Select Top 1 Degree From Apt_Worker Where Apt_Code = @Apt_Code Order By Aid Desc", new { Apt_Code }).SingleOrDefault();
        }

        // 주민대표 정보 상세보기
        public async Task<Apt_Worker_Entity> Detail(string Aid)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Apt_Worker_Entity>("Select * From Apt_Worker Where Aid = @Aid", new { Aid }, commandType: CommandType.Text);
            }
            //return dn.ctx.Query<Apt_Worker>("Select * From Apt_Worker Where Aid = @Aid", new { Aid }).SingleOrDefault();
        }
    }

    public interface IPresent
    {
        Task<Present_Entity> Add(Present_Entity prs);

        Task Update(string Apt_Code, string Staff_Code, DateTime LogOut_time);

        Task<List<Present_Entity>> GetList();

        Task<List<Present_Entity>> GetList_Apt(string Apt_Code);

        Task<List<Present_Entity>> GetList_Apt_Staff(string Apt_Code, string Staff_Code);
    }

    // 로그인 정보
    public class Present : IPresent
    {
        private readonly IConfiguration _db;

        public Present(IConfiguration configuration)
        {
            this._db = configuration;
        }

        // 로그인 시에 입력
        public async Task<Present_Entity> Add(Present_Entity prs)
        {
            var sql = "Insert Into Present (Apt_Code, Apt_Name, Staff_Code, Staff_Name, PostIP) Values (@Apt_Code, @Apt_Name, @Staff_Code, @Staff_Name, @PostIP)";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, prs, commandType: CommandType.Text);
                return prs;
            }
            //this.pr.ctx.Execute(sql, prs);
            //return prs;
        }

        // 로그아웃 시에 입력
        public async Task Update(string Apt_Code, string Staff_Code, DateTime LogOut_time)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync("Update Present Set LogOut_time = @LogOut_time Where Apt_Code = @Apt_Code And Staff_Code = @Staff_Code", new { Apt_Code, Staff_Code, LogOut_time }, commandType: CommandType.Text);
            }
            //this.pr.ctx.Execute("Update Present Set LogOut_time = @LogOut_time Where Apt_Code = @Apt_Code And Staff_Code = @Staff_Code", new { Apt_Code, Staff_Code, LogOut_time });
        }

        // 로그인 정보 목록
        public async Task<List<Present_Entity>> GetList()
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var apt = await ctx.QueryAsync<Present_Entity>("Select * From Present Order By Aid Desc", commandType: CommandType.Text);
                return apt.ToList();
            }
            //return this.pr.ctx.Query<Present_Entity>("Select * From Present Order By Aid Desc").ToList();
        }

        // 로그인 정보 목록(공동주택별)
        public async Task<List<Present_Entity>> GetList_Apt(string Apt_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var apt = await ctx.QueryAsync<Present_Entity>("Select * From Present Where Apt_Code = @Apt_Code Order By Aid Desc", new { Apt_Code }, commandType: CommandType.Text);
                return apt.ToList();
            }
            //return this.pr.ctx.Query<Present_Entity>("Select * From Present Where Apt_Code = @Apt_Code Order By Aid Desc", new { Apt_Code }).ToList();
        }

        // 로그인 정보 목록( 공동주별, 사용자별)
        public async Task<List<Present_Entity>> GetList_Apt_Staff(string Apt_Code, string Staff_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var apt = await ctx.QueryAsync<Present_Entity>("Select * From Present Where Apt_Code = @Apt_Code And Staff_Code = @Staff_Code Order By Aid Desc", new { Apt_Code, Staff_Code }, commandType: CommandType.Text);
                return apt.ToList();
            }
            //return this.pr.ctx.Query<Present_Entity>("Select * From Present Where Apt_Code = @Apt_Code And Staff_Code = @Staff_Code Order By Aid Desc", new { Apt_Code, Staff_Code }).ToList();
        }
    }

    public interface IKnma_Log
    {
        Task<int> logview(string mem_id, string mem_pw);

        Task<Khma_Member_Infor_Entity> kmi_detail(string mem_id);

        Task<List<khma_mem_career_Entity>> kmci_detail(string mem_cd);

        Task<string> apt_cd(string mem_id);

        Task<List<khma_AptInfor_Entity>> apt_mn(string APT_NM);

        Task<int> being_m_v(string mem_id);

        Task<int> being_a_v(string apt_cd);

        Task<string> apt_code_mem_nm(string apt_cd);

        Task<string> mem_nm(string mem_id);

        Task<int> apt_count(string mem_id);

        Task<khma_mem_Entity> member_v_infor(string mem_id);

        Task<List<khma_mem_Entity>> member_v_infor_List(string mem_id);

        Task<List<khma_mem_Entity>> member_v_name_List(string mem_nm);

        Task<khma_mem_Entity> member_v_name(string mem_nm);

        Task<int> member_v_mn_count(string mem_mn);

        Task<khma_AptInfor_Entity> kai_detail(string apt_cd);

        Task<string> kai_AptName(string apt_cd);

        Task<Khma_Member_Infor_Entity> apt_member_infor_mem(string mem_id);

        Task<Khma_Member_Infor_Entity> apt_member_infor_apt(string apt_cd);

        Task<int> apt_member_infor_count(string apt_cd);

        Task<List<Khma_Member_Infor_Entity>> apt_member_infor_list(string mem_id);

        Task<khma_mem_career_Entity> add_memplace(khma_mem_career_Entity kmc);

        Task<List<Apt_career_Join_Entity>> getlist_repair(string mem_id);

        Task<List<khma_mem_career_Entity>> career();

        Task<List<Apt_memverinfor_v_Entity>> Apt_Search_ID(string mem_id);

        Task<int> Apt_Search_ID_Count(string mem_id);

        Task<int> kai_apt_be(string apt_cd);

        Task<khma_mem_career_Entity> andnd(khma_mem_career_Entity kmc);
    }

    /// <summary>
    /// 협회 회원 정보 연동
    /// </summary>
    public class Khma_Log : IKnma_Log
    {
        private readonly IConfiguration _db;

        public Khma_Log(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 회원 정보 여부 확인
        /// </summary>
        /// <param name="mem_id">회원 아이디</param>
        /// <param name="mem_pw">암호</param>
        /// <returns>확인 되면 1, 안되면 0 반환</returns>
        public async Task<int> logview(string mem_id, string mem_pw)
        {
            var sql = "Select Count(*) From member_info_v Where mem_id = @mem_id And mem_pw = fsplugin.FSB.HASHEX('" + mem_pw + "', null,0)";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khma_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>(sql, new { mem_id, mem_pw }, commandType: CommandType.Text);
            }
            //return kn.ktx.Query<int>(sql, new { mem_id, mem_pw }).SingleOrDefault();
        }

        /// <summary>
        /// 회원정보 불러오기(로그인 된 경우만)
        /// </summary>
        /// <param name="mem_id">회원 아이디</param>
        /// <param name="mem_pw">암호</param>
        /// <returns>회원 및 공동주택 상세정보 불러오기</returns>
        public async Task<Khma_Member_Infor_Entity> kmi_detail(string mem_id)
        {
            var sql = "Select * From member_info_v as a join member_career_v as b on a.mem_cd = b.mem_cd  Join apart_info_v as c on b.apt_cd = c.APT_CD Where mem_id = @mem_id";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khma_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Khma_Member_Infor_Entity>(sql, new { mem_id }, commandType: CommandType.Text);
            }
            //return kn.ktx.Query<Khma_Member_Infor>(sql, new { mem_id }).SingleOrDefault();
        }

        /// <summary>
        /// 회원정보 식별코드로 배치정보 불러오기
        /// </summary>
        /// <param name="mem_cd">회원 식별코드</param>
        /// <returns>배치정보 반환</returns>
        public async Task<List<khma_mem_career_Entity>> kmci_detail(string mem_cd)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khma_db_Connection")))
            {
                var apt = await ctx.QueryAsync<khma_mem_career_Entity>("Select * From member_career_v Where mem_cd = @mem_cd", new { mem_cd }, commandType: CommandType.Text);
                return apt.ToList();
            }
            //return kn.ktx.Query<khma_mem_career>("Select * From member_career_v Where mem_cd = @mem_cd", new { mem_cd }).ToList();
        }

        /// <summary>
        /// 회원 식별코드로 공동주택 식별코드 불러오기
        /// </summary>
        /// <param name="mem_id">회원 식별코드</param>
        /// <returns>공동주택 식별코드 반환</returns>
        public async Task<string> apt_cd(string mem_id)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khma_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<string>("Select Top 1 apt_cd From member_career_v as a Join member_info_v as b on a.mem_cd = b.mem_cd  Where mem_id = @mem_id", new { mem_id }, commandType: CommandType.Text);
            }
            //return kn.ktx.Query<string>("Select Top 1 apt_cd From member_career_v as a Join member_info_v as b on a.mem_cd = b.mem_cd  Where mem_id = @mem_id", new { mem_id }).SingleOrDefault();
        }

        /// <summary>
        /// 공동주택명으로 공동주택 정보 불러오기
        /// </summary>
        /// <param name="APT_MN">회원 식별코드</param>
        public async Task<List<khma_AptInfor_Entity>> apt_mn(string APT_NM)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khma_db_Connection")))
            {
                var apt = await ctx.QueryAsync<khma_AptInfor_Entity>("Select * From apart_info_v Where  APT_NM Like '%" + APT_NM + "%' Order By APT_CD ASC", new { APT_NM }, commandType: CommandType.Text);
                return apt.ToList();
            }
            //eturn kn.ktx.Query<khma_apt>("Select * From apart_info_v Where  APT_NM Like '%" + APT_NM + "%' Order By APT_CD ASC", new { APT_NM }).ToList();
        }

        /// <summary>
        /// 단지 식별코드로 회원정보 존재 여부 확인
        /// </summary>
        /// <param name="mem_id"></param>
        /// <returns></returns>
        public async Task<int> being_m_v(string mem_id)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khma_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From member_info_v as a Join member_career_v as b On a.mem_cd = b.mem_cd Where a.mem_id = @mem_id", new { mem_id }, commandType: CommandType.Text);
            }
            //return kn.ktx.Query<int>("Select Count(*) From member_info_v as a Join member_career_v as b On a.mem_cd = b.mem_cd Where a.mem_id = @mem_id", new { mem_id }).SingleOrDefault();
        }

        /// <summary>
        /// 단지 식별코드로 회원정보 존재 여부 확인
        /// </summary>
        /// <param name="mem_id"></param>
        /// <returns></returns>
        public async Task<int> being_a_v(string apt_cd)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khma_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From member_info_v as a Join member_career_v as b On a.mem_cd = b.mem_cd Where b.apt_cd = @apt_cd", new { apt_cd }, commandType: CommandType.Text);
            }
            //return kn.ktx.Query<int>("Select Count(*) From member_info_v as a Join member_career_v as b On a.mem_cd = b.mem_cd Where b.apt_cd = @apt_cd", new { apt_cd }).SingleOrDefault();
        }

        /// <summary>
        /// 단지 식별코드로 관리소장명 불러오기
        /// </summary>
        /// <param name="mem_id">회원 아이디</param>
        /// <returns>회원명 반환</returns>
        public async Task<string> apt_code_mem_nm(string apt_cd)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khma_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<string>("Select top 1 a.mem_nm From member_info_v as a Join member_career_v as b On a.mem_cd = b.mem_cd Where b.apt_cd = @apt_cd", new { apt_cd }, commandType: CommandType.Text);
            }
            //return kn.ktx.Query<string>("Select top 1 a.mem_nm From member_info_v as a Join member_career_v as b On a.mem_cd = b.mem_cd Where b.apt_cd = @apt_cd", new { apt_cd }).SingleOrDefault();
        }

        /// <summary>
        /// 회원 식별코드로 관리소장명 불러오기
        /// </summary>
        /// <param name="mem_id">회원 아이디</param>
        /// <returns>회원명 반환</returns>
        public async Task<string> mem_nm(string mem_id)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<string>("Select mem_nm From member_info_v Where mem_id = @mem_id", new { mem_id }, commandType: CommandType.Text);
            }
            //return kn.ktx.Query<string>("Select mem_nm From member_info_v Where mem_id = @mem_id", new { mem_id }).SingleOrDefault();
        }

        /// <summary>
        /// 로그인된 회원의 배치 단지 수
        /// </summary>
        /// <param name="mem_id">회원 아이디</param>
        /// <returns>배치 단지 수</returns>
        public async Task<int> apt_count(string mem_id)
        {
            var sql = "Select Count(*) From member_career_v as a Join member_info_v as b on a.mem_cd = b.mem_cd Where mem_id = @mem_id";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khma_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>(sql, new { mem_id }, commandType: CommandType.Text);
            }
            //return kn.ktx.Query<int>(sql, new { mem_id }).SingleOrDefault();
        }

        /// <summary>
        /// 아이디로 회원관리에서 회원정보 불러오기
        /// </summary>
        /// <param name="mem_id"></param>
        /// <returns></returns>
        public async Task<khma_mem_Entity> member_v_infor(string mem_id)
        {
            var sql = "Select Top 1 mem_cd, mem_nm, mem_id, mem_pw, dupcode, birth_date, localcode, levelcode, state From member_info_v Where mem_id = @mem_id";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<khma_mem_Entity>(sql, new { mem_id }, commandType: CommandType.Text);
            }
            //return kn.ktx.Query<khma_mem>(sql, new { mem_id }).SingleOrDefault();
        }

        /// <summary>
        /// 아이디로 회원관리에서 회원정보 목록 불러오기
        /// </summary>
        /// <param name="mem_id"></param>
        /// <returns></returns>
        public async Task<List<khma_mem_Entity>> member_v_infor_List(string mem_id)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aot = await ctx.QueryAsync<khma_mem_Entity>("Select mem_cd, mem_nm, mem_id, mem_pw, dupcode, birth_date, localcode, levelcode, state From member_info_v a Where ((levelcode != 1 and mem_id != '') or mem_id is null) and mem_id = @mem_id", new { mem_id }, commandType: CommandType.Text);
                return aot.ToList();
            }
            //return kn.ktx.Query<khma_mem>("Select mem_cd, mem_nm, mem_id, mem_pw, dupcode, birth_date, localcode, levelcode, state From member_info_v a Where ((levelcode != 1 and mem_id != '') or mem_id is null) and mem_id = @mem_id", new { mem_id }).ToList();
        }

        /// <summary>
        /// 이름로 회원관리에서 회원정보 목록 불러오기
        /// </summary>
        /// <param name="mem_id"></param>
        /// <returns></returns>
        public async Task<List<khma_mem_Entity>> member_v_name_List(string mem_nm)
        {
            var sql = "Select mem_cd, mem_nm, mem_id, mem_pw, dupcode, birth_date, localcode, levelcode, state From member_info_v Where ((levelcode != 1 and mem_id != '') or mem_id is null) And mem_nm = @mem_nm";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khma_db_Connection")))
            {
                var apt = await ctx.QueryAsync<khma_mem_Entity>(sql, new { mem_nm }, commandType: CommandType.Text);
                return apt.ToList();
            }
            //return kn.ktx.Query<khma_mem>(sql, new { mem_nm }).ToList();
        }

        /// <summary>
        /// 이름로 회원관리에서 회원정보 불러오기
        /// </summary>
        /// <param name="mem_id"></param>
        /// <returns></returns>
        public async Task<khma_mem_Entity> member_v_name(string mem_nm)
        {
            var sql = "Select top 1 mem_cd, mem_nm, mem_id, mem_pw, dupcode, birth_date, localcode, levelcode, state From member_info_v a Where ((levelcode != 1 and mem_id != '') or mem_id is null) And mem_nm = @mem_nm";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khma_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<khma_mem_Entity>(sql, new { mem_nm }, commandType: CommandType.Text);
            }
            //return kn.ktx.Query<khma_mem>(sql, new { mem_nm }).SingleOrDefault();
        }

        /// <summary>
        /// 이름으로 회원관리에서 회원정보 수 불러오기
        /// </summary>
        /// <param name="mem_id"></param>
        /// <returns></returns>
        public async Task<int> member_v_mn_count(string mem_mn)
        {
            var sql = "Select Count(*) From member_info_v Where mem_nm = @mem_mn";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khma_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>(sql, new { mem_mn }, commandType: CommandType.Text);
            }
            //return kn.ktx.Query<int>(sql, new { mem_mn }).SingleOrDefault();
        }

        /// <summary>
        /// 협회 공동주택 정보 불러오기
        /// </summary>
        /// <param name="apt_cd">공동주택 식별코드</param>
        /// <returns>공동주택 정보 반환</returns>
        public async Task<khma_AptInfor_Entity> kai_detail(string apt_cd)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khma_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<khma_AptInfor_Entity>("Select APT_CD, LOCALCODE, APT_NM, ADDR_BASE, ADDR_DETAIL, TEL_NO, FAX_NO, MANAGEAREA, DONG_CNT, ELEVATOR_YN, ELEVATOR_CNT, UPPERFLOOR, UNDERFLOOR, BUILD_DATE From apart_info_v Where apt_cd = @apt_cd", new { apt_cd }, commandType: CommandType.Text);
            }
            //return kn.ktx.Query<khma_apt>("Select APT_CD, LOCALCODE, APT_NM, ADDR_BASE, ADDR_DETAIL, TEL_NO, FAX_NO, MANAGEAREA, DONG_CNT, ELEVATOR_YN, ELEVATOR_CNT, UPPERFLOOR, UNDERFLOOR, BUILD_DATE From apart_info_v Where apt_cd = @apt_cd", new { apt_cd }).SingleOrDefault();
        }

        /// <summary>
        /// 협회 공동주택 정보 불러오기
        /// </summary>
        /// <param name="apt_cd">공동주택 식별코드</param>
        /// <returns>공동주택 정보 반환</returns>
        public async Task<string> kai_AptName(string apt_cd)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<string>("", new { apt_cd }, commandType: CommandType.Text);
            }
            //return kn.ktx.Query<string>("Select APT_NM From apart_info_v Where apt_cd = @apt_cd", new { apt_cd }).SingleOrDefault();
        }

        /// <summary>
        /// 회원정보와 단지 정보 가져오기
        /// </summary>
        /// <param name="mem_id">회원 아이디</param>
        /// <returns>단지와 회원 정보 반환</returns>
        public async Task<Khma_Member_Infor_Entity> apt_member_infor_mem(string mem_id)
        {
            var sql = "Select * From member_career_v as a Join member_info_v  as b on a.mem_cd = b.mem_cd Join apart_info_v as c on a.apt_cd = c.apt_cd Where b.mem_id = @mem_id";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Khma_Member_Infor_Entity>(sql, new { mem_id }, commandType: CommandType.Text);
            }
            //return kn.ktx.Query<Khma_Member_Infor>(sql, new { mem_id }).SingleOrDefault();
        }

        /// <summary>
        /// 회원정보와 단지 정보 가져오기
        /// </summary>
        /// <param name="mem_id">회원 아이디</param>
        /// <returns>단지와 회원 정보 반환</returns>
        public async Task<Khma_Member_Infor_Entity> apt_member_infor_apt(string apt_cd)
        {
            var sql = "Select * From member_career_v as a Join member_info_v  as b on a.mem_cd = b.mem_cd Join apart_info_v as c on a.apt_cd = c.apt_cd Where c.apt_cd = @apt_cd";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Khma_Member_Infor_Entity>(sql, new { apt_cd }, commandType: CommandType.Text);
            }
            //return kn.ktx.Query<Khma_Member_Infor>(sql, new { apt_cd }).SingleOrDefault();
        }

        /// <summary>
        /// 공동주택 식별코드로 협회 단지 및 회원 존재 여부 불러오기
        /// </summary>
        /// <param name="apt_cd"></param>
        /// <returns></returns>
        public async Task<int> apt_member_infor_count(string apt_cd)
        {
            var sql = "Select count(*) From member_career_v as a Join member_info_v  as b on a.mem_cd = b.mem_cd Join apart_info_v as c on a.apt_cd = c.apt_cd Where c.apt_cd = @apt_cd";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>(sql, new { apt_cd }, commandType: CommandType.Text);
            }
            //return kn.ktx.Query<int>(sql, new { apt_cd }).SingleOrDefault();
        }

        /// <summary>
        /// 두개 이상 회원정보와 단지 정보 가져오기
        /// </summary>
        /// <param name="mem_id"></param>
        /// <returns></returns>
        public async Task<List<Khma_Member_Infor_Entity>> apt_member_infor_list(string mem_id)
        {
            var sql = "Select * From member_career_v as a Join member_info_v  as b on a.mem_cd = b.mem_cd Join apart_info_v as c on a.apt_cd = c.apt_cd Where b.mem_id = @mem_id";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var apt = await ctx.QueryAsync<Khma_Member_Infor_Entity>(sql, new { mem_id }, commandType: CommandType.Text);
                return apt.ToList();
            }
            //return kn.ktx.Query<Khma_Member_Infor>(sql, new { mem_id }).ToList();
        }

        /// <summary>
        /// 배치정보 입력
        /// </summary>
        /// <param name="kmc"></param>
        /// <returns></returns>
        public async Task<khma_mem_career_Entity> add_memplace(khma_mem_career_Entity kmc)
        {
            var sql = "Insert into member_placememer (apt_cd, seq, mem_cd, mem_id, state) values (@apt_cd, @seq, @mem_cd, @mem_id, @state);";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, kmc, commandType: CommandType.Text);
                return kmc;
            }

            //return kmc;
        }

        /// <summary>
        /// 배치정보 공동주택 정보 가져 오기( 장기시스템에서)
        /// </summary>
        /// <param name="mem_id"></param>
        /// <returns></returns>
        public async Task<List<Apt_career_Join_Entity>> getlist_repair(string mem_id)
        {
            var sql = "Select * From member_placememer as a Join Apt as b on a.apt_cd = b.Apt_Code Where a.mem_id = @mem_id";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var apt = await ctx.QueryAsync<Apt_career_Join_Entity>(sql, new { mem_id }, commandType: CommandType.Text);
                return apt.ToList();
            }
            //return cn.ctx.Query<Apt_career_Join_Entity>(sql, new { mem_id }).ToList();
        }

        public async Task<List<khma_mem_career_Entity>> career()
        {
            var sql = "select * from member_career_v";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var apt = await ctx.QueryAsync<khma_mem_career_Entity>(sql, commandType: CommandType.Text);
                return apt.ToList();
            }
            //return kn.ktx.Query<khma_mem_career>(sql, new { }).ToList();
        }

        /// <summary>
        /// 아이디로 공동주택 식별코드 찾기
        /// </summary>
        public async Task<List<Apt_memverinfor_v_Entity>> Apt_Search_ID(string mem_id)
        {
            var sql = "select b.AId, b.Apt_Name, b.Apt_Adress_Gun, b.Apt_Adress_Rest, b.Apt_Adress_Sido, b.apt_cd, b.Apt_Code, b.Apt_Form, b.AcceptancedOfWork_Date, b.CorporateResistration_Num, d.mem_id, d.mem_nm, b.PostDate from khma.dbo.apart_info_v a join khmais.dbo.Apt b on a.APT_CD = b.apt_cd Join khma.dbo.member_career_v c on a.APT_CD = c.apt_cd join khma.dbo.member_info_v d on d.mem_cd = c.mem_cd Where d.mem_id = @mem_id";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var apt = await ctx.QueryAsync<Apt_memverinfor_v_Entity>(sql, new { mem_id }, commandType: CommandType.Text);
                return apt.ToList();
            }
            //return kn.ktx.Query<Apt_memverinfor_v_Entity>(sql, new { mem_id }).ToList();
        }

        /// <summary>
        /// 아이디로 검색된 공동주택 수
        /// </summary>
        /// <param name="mem_id"></param>
        /// <returns></returns>
        public async Task<int> Apt_Search_ID_Count(string mem_id)
        {
            var sql = "select COUNT(*) from khma.dbo.apart_info_v a join khmais.dbo.Apt b on a.APT_CD = b.apt_cd Join khma.dbo.member_career_v c on a.APT_CD = c.apt_cd join khma.dbo.member_info_v d on d.mem_cd = c.mem_cd Where d.mem_id = @mem_id";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>(sql, new { mem_id }, commandType: CommandType.Text); ;
            }
            //return kn.ktx.Query<int>(sql, new { mem_id }).SingleOrDefault();
        }

        /// <summary>
        /// 존재여부 확인
        /// </summary>
        /// <param name="apt_cd"></param>
        /// <returns></returns>
        public async Task<int> kai_apt_be(string apt_cd)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khma_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From apart_info_v Where APT_CD = @apt_cd", new { apt_cd }, commandType: CommandType.Text);
            }
            //return kn.ktx.Query<int>("Select Count(*) From apart_info_v Where APT_CD = @apt_cd", new { apt_cd }).SingleOrDefault();
        }

        /// <summary>
        /// 테스트
        /// </summary>
        /// <param name="kmc"></param>
        /// <returns></returns>
        public async Task<khma_mem_career_Entity> andnd(khma_mem_career_Entity kmc)
        {
            var sql = "insert into member_car (mem_cd, seq, apt_cd, state) Values (@mem_cd, @seq, @apt_cd, @state)";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khma_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, kmc, commandType: CommandType.Text);
                return kmc;
            }
            // cn.ctx.Execute(sql, kmc);
            //return kmc;
        }
    }

    public interface IRepair_member_career
    {
        Task<khma_mem_career_Entity> Add(khma_mem_career_Entity kmc);

        Task<khma_mem_career_Entity> Edit(khma_mem_career_Entity kmc);

        Task<khma_mem_career_Entity> Detail(string mem_id);

        Task<List<khma_mem_career_Entity>> GetList(string mem_id);

        Task<List<khma_mem_career_Entity>> GetList_All();

        Task<int> being(string mem_id);

        Task delete(string Aid);
    }

    /// <summary>
    /// 비의무단지 연결 정보
    /// </summary>
    public class Repair_member_career : IRepair_member_career
    {
        private readonly IConfiguration _db;

        public Repair_member_career(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 비의무단지 연결 정보 입력
        /// </summary>
        /// <param name="kmc"></param>
        /// <returns></returns>
        public async Task<khma_mem_career_Entity> Add(khma_mem_career_Entity kmc)
        {
            var sql = "Insert into member_career (mem_cd, seq, apt_cd, state, mem_id) Values (@mem_cd, @seq, @apt_cd, @state, @mem_id)";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, kmc, commandType: CommandType.Text);
                return kmc;
            }
            //this.db.ctx.Execute(sql, kmc);
            //return kmc;
        }

        /// <summary>
        /// 비의무단지 연결 정보 수정
        /// </summary>
        /// <param name="kmc"></param>
        /// <returns></returns>
        public async Task<khma_mem_career_Entity> Edit(khma_mem_career_Entity kmc)
        {
            var sql = "Update member_career set mem_cd = @mem_cd, seq =  @seq, apt_cd = @apt_cd, state = @state, mem_id = @mem_id Where Aid = @Aid";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, kmc, commandType: CommandType.Text);
                return kmc;
            }
            //this.db.ctx.Execute(sql, kmc);
            //return kmc;
        }

        /// <summary>
        /// 비의무단지 연결 정보 상세보기
        /// </summary>
        /// <param name="mem_id"></param>
        /// <returns></returns>
        public async Task<khma_mem_career_Entity> Detail(string mem_id)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<khma_mem_career_Entity>("Select mem_cd, seq, apt_cd, state, mem_id from member_career where mem_id = @mem_id", new { mem_id }, commandType: CommandType.Text);
            }
            //return db.ctx.Query<khma_mem_career>("Select mem_cd, seq, apt_cd, state, mem_id from member_career where mem_id = @mem_id", new { mem_id }).SingleOrDefault();
        }

        /// <summary>
        /// 비의무단지 연결 정보 목록
        /// </summary>
        /// <param name="mem_id"></param>
        /// <returns></returns>
        public async Task<List<khma_mem_career_Entity>> GetList(string mem_id)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var apt = await ctx.QueryAsync<khma_mem_career_Entity>("Select mem_cd, seq, apt_cd, state, mem_id from member_career where mem_id = @mem_id", new { mem_id }, commandType: CommandType.Text);
                return apt.ToList();
            }
            //return db.ctx.Query<khma_mem_career>("Select mem_cd, seq, apt_cd, state, mem_id from member_career where mem_id = @mem_id", new { mem_id }).ToList();
        }

        /// <summary>
        /// 비의무단지 연결 정보 목록(all)
        /// </summary>
        /// <param name="mem_id"></param>
        /// <returns></returns>
        public async Task<List<khma_mem_career_Entity>> GetList_All()
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var apt = await ctx.QueryAsync<khma_mem_career_Entity>("Select Aid, mem_cd, seq, apt_cd, state, mem_id, PostDate from member_career Order by Aid Desc", commandType: CommandType.Text);
                return apt.ToList();
            }
            //return db.ctx.Query<khma_mem_career>("Select Aid, mem_cd, seq, apt_cd, state, mem_id, PostDate from member_career Order by Aid Desc").ToList();
        }

        /// <summary>
        /// 비의무단지 연결 정보 존재 여부
        /// </summary>
        /// <param name="mem_id"></param>
        /// <returns></returns>
        public async Task<int> being(string mem_id)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From member_career where mem_id = @mem_id", new { mem_id }, commandType: CommandType.Text);
            }
            //return db.ctx.Query<int>("Select Count(*) From member_career where mem_id = @mem_id", new { mem_id }).SingleOrDefault();
        }

        /// <summary>
        /// 비의무 단지 연결 정보 삭제
        /// </summary>
        /// <param name="Aid"></param>
        public async Task delete(string Aid)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync("Delete member_career Where Aid = @Aid", new { Aid }, commandType: CommandType.Text);
            }
            //this.db.ctx.Execute("Delete member_career Where Aid = @Aid", new { Aid });
        }
    }
}