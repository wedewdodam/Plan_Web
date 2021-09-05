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
    /// 공동주택 기본정보
    /// </summary>
    public class AptInfor_Lib : IAptInfor_Lib
    {
        private readonly IConfiguration _db;

        public AptInfor_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        public async Task<AptInfor_Entity> Detail_Apt(string Apt_Code)
        {
            var sql = "Select AId, Apt_Code, apt_cd, Apt_Name, Apt_Form, Apt_Adress_Sido, Apt_Adress_Gun, Apt_Adress_Rest, CorporateResistration_Num, AcceptancedOfWork_Date, LevelCount, Staff_code, PostDate, PostIP, combine From Apt Where Apt_Code = @Apt_Code";

            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            db.Open();
            var result = await db.QueryFirstOrDefaultAsync<AptInfor_Entity>(sql, new { Apt_Code }, commandType: CommandType.Text);
            return result;
        }

        /// <summary>
        /// 공동주택 기본 정보 입력
        /// </summary>
        public async Task<AptInfor_Entity> Add_Apt(AptInfor_Entity Apt)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var sql = "Insert into Apt (Apt_Code, apt_cd, Apt_Name, Apt_Form, Apt_Adress_Sido, Apt_Adress_Gun, Apt_Adress_Rest, CorporateResistration_Num, AcceptancedOfWork_Date, Staff_code, LevelCount, PostIP, combine) Values (@Apt_Code, @Apt_Code, @Apt_Name, @Apt_Form, @Apt_Adress_Sido, @Apt_Adress_Gun, @Apt_Adress_Rest, @CorporateResistration_Num, @AcceptancedOfWork_Date, '', 5, @PostIP, 'a')";
            await db.ExecuteAsync(sql, Apt);
            return Apt;
        }

        /// <summary>
        /// 공동주택 기본 정보 테스트 입력
        /// </summary>
        public async Task<AptInfor_Entity> Add_Test_Apt(AptInfor_Entity Apt)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var sql = "Insert into Apt (Apt_Code, apt_cd, Apt_Name, Apt_Form, Apt_Adress_Sido, Apt_Adress_Gun, Apt_Adress_Rest, CorporateResistration_Num, AcceptancedOfWork_Date, LevelCount, Staff_code, PostIP, combine) Values (@Apt_Code, @Apt_Code, @Apt_Name, @Apt_Form, @Apt_Adress_Sido, @Apt_Adress_Gun, @Apt_Adress_Rest, @CorporateResistration_Num, @AcceptancedOfWork_Date, 5, @Staff_code, @PostIP, 'a')";
            await db.ExecuteAsync(sql, Apt);
            return Apt;
        }

        /// <summary>
        /// 공동주택 기본 정보 수정
        /// </summary>
        public async Task<AptInfor_Entity> Edit_Apt(AptInfor_Entity Apt)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var sql = "Update Apt Set Apt_Name = @Apt_Name, Apt_Form = Apt_Form, Apt_Adress_Sido = @Apt_Adress_Sido, Apt_Adress_Gun = @Apt_Adress_Gun, Apt_Adress_Rest = @Apt_Adress_Rest, CorporateResistration_Num = @CorporateResistration_Num, AcceptancedOfWork_Date = @AcceptancedOfWork_Date, combine = 'a' Where AId = @AId";
            await db.ExecuteAsync(sql, Apt);
            return Apt;
        }

        /// <summary>
        /// 공동주택 기본 정보 중에 협회 정보가 다른 경우 수정
        /// </summary>
        public async Task<AptInfor_Entity> Edit_Apt_kat(AptInfor_Entity Apt)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var sql = "Update Apt Set Apt_Name = @Apt_Name, AcceptancedOfWork_Date = @AcceptancedOfWork_Date Where AId = @AId";
            await db.ExecuteAsync(sql, Apt);
            return Apt;
        }

        /// <summary>
        /// 통합된 공동주택 장기수선계획 정보 만들기로 수정
        /// </summary>
        /// <param name="Aid"></param>
        /// <param name="combile"></param>
        public async Task Edit_combine(string Apt_Code, string combile)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            await db.ExecuteAsync("Update Apt Set combine = @combine Where Apt_Code = @Apt_Code", new { Apt_Code, combile });
        }

        /// <summary>
        /// 통합 장기수선계획 정보 여부 불러오기
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<int> combibe_infor(string mem_id, string apt_cd)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From member_placememer Where mem_id = @mem_id And apt_cd = @apt_cd", new { mem_id, apt_cd });
        }

        /// <summary>
        /// 공동주택명 검색
        /// </summary>
        /// <param name="SearchFeild"></param>
        /// <param name="SerarchQuery"></param>
        /// <returns></returns>
        public async Task<List<AptInfor_Entity>> Apt_Search(string SearchField, string SearchQuery)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var lst = await db.QueryAsync<AptInfor_Entity>("Search_Apt", new { SearchField, SearchQuery }, commandType: CommandType.StoredProcedure);
            return lst.ToList();
        }

        /// <summary>
        /// 검색된 공동주택명 수
        /// </summary>
        /// <returns></returns>
        public async Task<int> Apt_Search_count(string SearchField, string SearchQuery)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<int>("Search_Apt_Count", new { SearchField, SearchQuery }, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// 입력된 공동주택 수
        /// </summary>
        /// <returns></returns>
        public async Task<int> Apt_count()
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Apt");
        }

        // 사용신청단지 사용승인
        public async Task Edit_Approve(string Aid, string LevelCount)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            await db.QuerySingleOrDefaultAsync("Update Apt Set LevelCount = @LevelCount Where AId = @AId", new { Aid, LevelCount });

            //this.ctx.Execute("Delete From Apt Where AId = @AId", new { AId });
        }

        // 시군부 별 입력된 수
        public async Task<int> Check_Count_Gun(string Apt_Adress_Gun)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Apt Where Apt_Adress_Gun = @Apt_Adress_Gun", new { Apt_Adress_Gun });
        }

        /// <summary>
        /// 입력된 수
        /// </summary>
        public async Task<int> Check_Count()
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Apt", new { });
        }

        /// <summary>
        /// 공동주택 승인 여부에 따라 그 수
        /// </summary>
        /// <param name="LevelCount">등급</param>
        /// <returns>검색된 수</returns>
        /// 2017-03-11
        public async Task<int> Approve_Count(int LevelCount)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Apt Where LevelCount = @LevelCount", new { LevelCount });
        }

        /// <summary>
        /// 시군부 별 입력된 수
        /// </summary>
        public async Task<int> Check_Count_Sido(string Apt_Adress_Gun)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Apt Where Apt_Adress_Gun = @Apt_Adress_Gun", new { Apt_Adress_Gun });
        }

        /// <summary>
        /// 사업자 번호 중복 체크
        /// </summary>
        public async Task<int> Overlap_Check_Apt(string CorporateResistration_Num)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Apt Where CorporateResistration_Num = @CorporateResistration_Num", new { CorporateResistration_Num });
        }

        /// <summary>
        /// 공동 주택 중복 체크
        /// </summary>
        public async Task<int> Overlap_Apt(string Apt_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Apt Where Apt_Code = @Apt_Code", new { Apt_Code });
        }

        // 공동주택 기본 정보 시도 리스트 구현
        public async Task<List<AptInfor_Entity>> GetList_Apt_Sido(string Apt_Adress_Sido)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var lst = await db.QueryAsync<AptInfor_Entity>("Select * From Apt Where Apt_Adress_Sido = @Apt_Adress_Sido Order By Aid Desc", new { Apt_Adress_Sido });
            return lst.ToList();
        }

        /// <summary>
        ///  공동주택 기본 정보 리스트 구현
        /// </summary>
        public async Task<List<AptInfor_Entity>> GetList_Apt()
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var lst = await db.QueryAsync<AptInfor_Entity>("Select * From Apt Order By Aid Desc", new { });
            return lst.ToList();
        }

        /// <summary>
        ///  공동주택 기본 정보 검색 리스트 구현
        /// </summary>
        public async Task<List<AptInfor_Entity>> GetList_Apt_Search(string Apt_Name)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var lst = await db.QueryAsync<AptInfor_Entity>("Select * From Apt Where Apt_Name Like '%" + Apt_Name + "%' Order By Aid Desc", new { Apt_Name });
            return lst.ToList();
        }

        // 공동주택 기본 정보 시군구 리스트 구현
        public async Task<List<AptInfor_Entity>> GetList_Apt_Gun(string Apt_Adress_Gun)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var lst = await db.QueryAsync<AptInfor_Entity>("Select * From Apt Where Apt_Adress_Gun = @Apt_Adress_Gun Order By Aid Asc", new { Apt_Adress_Gun });
            return lst.ToList();
        }

        /// <summary>
        /// 통합관리 공동주택 정보 불러오기
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<AptInfor_Entity> Detail_Apt_combile(string Apt_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<AptInfor_Entity>("Select AId, Apt_Code, apt_cd, Apt_Name, Apt_Form, Apt_Adress_Sido, Apt_Adress_Gun, Apt_Adress_Rest, CorporateResistration_Num, AcceptancedOfWork_Date, LevelCount, Staff_code, PostDate, PostIP, combine From Apt Where Apt_Code = @Apt_Code And combile = 'b'", new { Apt_Code });
        }

        // 공동주택명 불러오기
        public async Task<string> Apt_Name(string Apt_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<string>("Select Apt_Name From Apt Where Apt_Code = @Apt_Code", new { Apt_Code });
        }

        // 공동주택 정보 삭제
        public async Task Remeove_Apt(int AId)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            await db.ExecuteAsync("Delete From Apt Where AId = @AId", new { AId });
        }

        // 사용신청 중인 공동주택 신청 목록
        public async Task<List<Apt_Staff_Join_Entity>> GetList_Approve()
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var sql = "Select a.Aid, a.Apt_Code, a.Apt_Name, a.Apt_Adress_Sido, a.Apt_Adress_Gun, a.Apt_Adress_Rest, a.Apt_Form, a.LevelCount, a.AcceptancedOfWork_Date, a.CorporateResistration_Num, a.PostDate, b.PostDate, b.Aid As Staff_Aid, b.Staff_Code, b.Staff_Name, b.Staff_Division, b.LevelCount As Staff_LevelCount From Apt As a Join Staff As b On a.Apt_Code = b.Apt_Code Order By b.Aid Desc";
            var lst = await db.QueryAsync<Apt_Staff_Join_Entity>(sql);
            return lst.ToList();
        }

        // 사용신청 중인 공동주택 신청 목록(시도별)
        public async Task<List<Apt_Staff_Join_Entity>> GetList_Approve_Sido(string Apt_Adress_Sido)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var sql = "Select a.Aid, a.Apt_Code, a.Apt_Name, a.Apt_Adress_Sido, a.Apt_Adress_Gun, a.Apt_Adress_Rest, a.Apt_Form, a.LevelCount, a.AcceptancedOfWork_Date, a.CorporateResistration_Num, a.PostDate, b.PostDate, b.Aid As Staff_Aid, b.Staff_Code, b.Staff_Name, b.Staff_Division, b.LevelCount As Staff_LevelCount From Apt As a Join Staff As b On a.Apt_Code = b.Apt_Code Where a.Apt_Adress_Sido = @Apt_Adress_Sido Order By b.Aid Desc";
            var lst = await db.QueryAsync<Apt_Staff_Join_Entity>(sql, new { Apt_Adress_Sido });
            return lst.ToList();
        }

        // 사용신청 중인 공동주택 신청 목록(군별)
        public async Task<List<Apt_Staff_Join_Entity>> GetList_Approve_Gun(string Apt_Adress_Sido, string Apt_Adress_Gun)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var sql = "Select a.Aid, a.Apt_Code, a.Apt_Name, a.Apt_Adress_Sido, a.Apt_Adress_Gun, a.Apt_Adress_Rest, a.Apt_Form, a.LevelCount, a.AcceptancedOfWork_Date, a.CorporateResistration_Num, a.PostDate, b.PostDate, b.Aid As Staff_Aid, b.Staff_Code, b.Staff_Name, b.Staff_Division, b.LevelCount As Staff_LevelCount From Apt As a Join Staff As b On a.Apt_Code = b.Apt_Code Where a.Apt_Adress_Sido = @Apt_Adress_Sido And a.Apt_Adress_Gun = @Apt_Adress_Gun Order By b.Aid Desc";
            var lst = await db.QueryAsync<Apt_Staff_Join_Entity>(sql, new { Apt_Adress_Sido, Apt_Adress_Gun });
            return lst.ToList();
        }

        /// <summary>
        /// 사용자 아이디로 단지 정보 존재 여부 확인
        /// </summary>
        /// <param name="Staff_code"></param>
        /// <returns></returns>
        public async Task<int> being_apt(string Staff_code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Apt Where Staff_code = @Staff_code", new { Staff_code });
        }

        // 전체 검색
        public async Task<List<Apt_Staff_Join_Entity>> GetList_Search(string SearchFeild, string SearchQuery)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var sql = "Select a.Aid, a.Apt_Code, a.Apt_Name, a.Apt_Adress_Sido, a.Apt_Adress_Gun, a.Apt_Adress_Rest, a.Apt_Form, a.LevelCount, a.AcceptancedOfWork_Date, a.CorporateResistration_Num, a.PostDate, b.PostDate, b.Aid As Staff_Aid, b.Staff_Code, b.Staff_Name, b.Staff_Division, b.LevelCount As Staff_LevelCount From Apt As a Join Staff As b On a.Apt_Code = b.Apt_Code Where " + SearchFeild + " Like '%" + SearchQuery + "%' Order By b.Aid Desc";
            var lst = await db.QueryAsync<Apt_Staff_Join_Entity>(sql, new { SearchFeild, SearchQuery });
            return lst.ToList();
        }

        // 전체 검색(시도별)
        public async Task<List<Apt_Staff_Join_Entity>> GetList_Search_Sido(string Apt_Adress_Sido, string SearchFeild, string SearchQuery)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var sql = "Select a.Aid, a.Apt_Code, a.Apt_Name, a.Apt_Adress_Sido, a.Apt_Adress_Gun, a.Apt_Adress_Rest, a.Apt_Form, a.LevelCount, a.AcceptancedOfWork_Date, a.CorporateResistration_Num, a.PostDate, b.PostDate, b.Aid As Staff_Aid, b.Staff_Code, b.Staff_Name, b.Staff_Division, b.LevelCount As Staff_LevelCount From Apt As a Join Staff As b On a.Apt_Code = b.Apt_Code Where  a.Apt_Adress_Sido = @Apt_Adress_Sido And " + SearchFeild + " Like '%" + SearchQuery + "%' Order By b.Aid Desc";
            var lst = await db.QueryAsync<Apt_Staff_Join_Entity>(sql, new { Apt_Adress_Sido, SearchFeild, SearchQuery });
            return lst.ToList();
        }

        // 전체 검색(시군구별)
        public async Task<List<Apt_Staff_Join_Entity>> GetList_Search_Gun(string Apt_Adress_Sido, string Apt_Adress_Gun, string SearchFeild, string SearchQuery)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var sql = "Select a.Aid, a.Apt_Code, a.Apt_Name, a.Apt_Adress_Sido, a.Apt_Adress_Gun, a.Apt_Adress_Rest, a.Apt_Form, a.LevelCount, a.AcceptancedOfWork_Date, a.CorporateResistration_Num, a.PostDate, b.PostDate, b.Aid As Staff_Aid, b.Staff_Code, b.Staff_Name, b.Staff_Division, b.LevelCount As Staff_LevelCount From Apt As a Join Staff As b On a.Apt_Code = b.Apt_Code Where  a.Apt_Adress_Sido = @Apt_Adress_Sido And a.Apt_Adress_Gun = @Apt_Adress_Gun And " + SearchFeild + " Like '%" + SearchQuery + "%' Order By b.Aid Desc";
            var lst = await db.QueryAsync<Apt_Staff_Join_Entity>(sql, new { Apt_Adress_Sido, Apt_Adress_Gun, SearchFeild, SearchQuery });
            return lst.ToList();
        }

        // 단지 등급 검색
        public async Task<int> Apt_Level(string Apt_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<int>("Select LevelCount From Apt Where Apt_Code = @Apt_Code", new { Apt_Code });
        }

        /// <summary>
        /// 협회 공동주택 코드로 공동주택 존재 여부 확인
        /// </summary>
        /// <param name="apt_cd">협회 공동주택 식별코드</param>
        /// <returns>1이면 존재, 0이면 미존재</returns>
        public async Task<int> apt_kai(string apt_cd)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Apt Where Apt_Code = @apt_cd", new { apt_cd });
        }

        /// <summary>
        /// 아이디로 테스트 단지 정보 불러오기
        /// </summary>
        /// <param name="Staff_code"></param>
        /// <returns></returns>
        public async Task<AptInfor_Entity> Detail_Test_Apt(string Staff_code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<AptInfor_Entity>("Select Top 1 AId, Apt_Code, apt_cd, Apt_Name, Apt_Form, Apt_Adress_Sido, Apt_Adress_Gun, Apt_Adress_Rest, CorporateResistration_Num, AcceptancedOfWork_Date, LevelCount, Staff_code, PostDate, PostIP, combine From Apt Where Staff_code = @Staff_code Order By Aid Desc", new { Staff_code });
        }

        /// <summary>
        /// 아이디로 테스트 단지 존재 여부 확인
        /// </summary>
        /// <param name="Staff_code"></param>
        /// <returns></returns>
        public async Task<int> Being_Test_Apt(string Staff_code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Apt Where Staff_code = @Staff_code", new { Staff_code });
        }

        /// <summary>
        /// 테스트 단지 수 확인
        /// </summary>
        /// <returns></returns>
        public async Task<int> Test_Being_Count(string Apt_Name)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Apt Where Apt_Name Like '" + Apt_Name + "%'", new { Apt_Name });
        }

        /// <summary>
        /// 장기 사용검사일 수정
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="date"></param>
        public async Task aow_edit(string Apt_Code, DateTime date)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            await db.ExecuteAsync("Update Apt Set AcceptancedOfWork_Date = @date Where Apt_Code = @Apt_Code", new { Apt_Code, date });
        }
    }

    // 공동주택 기본 정보
    public class Apt_Detail_Lib : IApt_Detail_Lib
    {
        private readonly IConfiguration _db;

        public Apt_Detail_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        // 공동주택 기본 정보 입력
        public async Task<Apt_Detail_Entity> Add_AptDetail(Apt_Detail_Entity Apt)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var sql = "Insert Apt_Detail (Apt_Detail_Code, Apt_Code, Developer, Builder, District, Site_Area, Build_Area, FloorTotal_Area, Supply_Area, FloorArea_Ratio, BuildingCoverage_Ratio, Elevator, Heighest, Heating_Way, WaterSupply_Way, Telephone, Fax, Email, Electric_Supply_Capacity, Water_Quantity, Park_Car_Count, Management_Way, Joint_Management, PostIP) Values (@Apt_Detail_Code, @Apt_Code, @Developer, @Builder, @District, @Site_Area, @Build_Area, @FloorTotal_Area, @Supply_Area, @FloorArea_Ratio, @BuildingCoverage_Ratio, @Elevator, @Heighest, @Heating_Way, @WaterSupply_Way, @Telephone, @Fax, @Email, @Electric_Supply_Capacity, @Water_Quantity, @Park_Car_Count, @Management_Way, @Joint_Management, @PostIP);";
            await db.ExecuteAsync(sql, Apt);
            return Apt;
        }

        public async Task<Apt_Detail_Entity> Edit_AptDetail(Apt_Detail_Entity Apt)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var msh = "Update Apt_Detail Set Developer = @Developer, Builder = @Builder, District = @District, Site_Area = @Site_Area, Build_Area = @Build_Area, FloorTotal_Area = @FloorTotal_Area, Supply_Area = @Supply_Area, FloorArea_Ratio = @FloorArea_Ratio, BuildingCoverage_Ratio = @BuildingCoverage_Ratio, Elevator = @Elevator, Heighest = @Heighest, Heating_Way = @Heating_Way, WaterSupply_Way = @WaterSupply_Way, Telephone = @Telephone, Fax = @Fax, Email = @Email, Electric_Supply_Capacity = @Electric_Supply_Capacity, Water_Quantity = @Water_Quantity, Park_Car_Count = @Park_Car_Count, Management_Way = @Management_Way, Joint_Management = @Joint_Management, PostIP = @PostIP Where Apt_Code = @Apt_Code;";
            await db.ExecuteAsync(msh, Apt);
            return Apt;
        }

        public async Task<Apt_Detail_Entity> Detail_AptDetail(string Apt_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<Apt_Detail_Entity>("Select * From Apt_Detail Where Apt_Code = @Apt_Code", new { Apt_Code });
        }

        // 공동주택 상세정보 존재 확인
        public async Task<int> Repeat_AptDetail(string Apt_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Apt_Detail Where Apt_Code = @Apt_Code", new { Apt_Code });
        }

        /// <summary>
        /// 사업주체 명 불러오기
        /// </summary>
        /// <param name="Apt_Code">공동주택 식별코드</param>
        /// <returns></returns>
        public async Task<string> Apt_Developer(string Apt_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<string>("Select isnull(Developer, '대주관(주)') as Developer From Apt_Detail Where Apt_Code = @Apt_Code", new { Apt_Code });
        }

        /// <summary>
        /// 공동주택코드로 전화번호 불러오기
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<string> Telephone(string Apt_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<string>("Select isnull(Telephone, '기재안함') as Developer From Apt_Detail Where Apt_Code = @Apt_Code", new { Apt_Code });
        }

        /// <summary>
        /// 해당 공동주택에 입력되었는지 확인
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<int> Being(string Apt_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Apt_Detail Where Apt_Code = @Apt_Code", new { Apt_Code });
        }
    }

    // 동 기본 정보
    public class Dong_Lib : IDong_Lib
    {
        private readonly IConfiguration _db;

        public Dong_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        // 동정보 입력
        public async Task<Dong_Entity> Add_Dong(Dong_Entity Dong)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var sql = "Insert Dong (Dong_Code, Apt_Code, Dong_Name, Family_Num, Length, Width, Floor_Num, Exit_Num, Elevator_Num, Line_Num, Hall_Form, Roof_Form, Dong_Area, Dong_Etc, PostIP ) Values (@Dong_Code, @Apt_Code, @Dong_Name, @Family_Num, @Length, @Width, @Floor_Num, @Exit_Num, @Elevator_Num, @Line_Num, @Hall_Form, @Roof_Form, @Dong_Area, @Dong_Etc, @PostIP);";
            await db.ExecuteAsync(sql, Dong);
            return Dong;
        }

        // 동 정보 수정
        public async Task<Dong_Entity> Edit_Dong(Dong_Entity Dong)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var sql = "Update Dong Set Dong_Name = @Dong_Name, Family_Num = @Family_Num, Length = @Length, Width = @Width, Floor_Num = @Floor_Num, Exit_Num = @Exit_Num, Elevator_Num = @Elevator_Num, Line_Num = @Line_Num, Hall_Form = @Hall_Form, Roof_Form = @Roof_Form, Dong_Area = @Dong_Area, Dong_Etc = @Dong_Etc, PostIP = @PostIP Where AId = @AId";
            await db.ExecuteAsync(sql, Dong);
            return Dong;
        }

        // 마지막 일련번호
        public async Task<int> Last_Number()
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync("Select Top 1 Aid From Dong Order by Aid Desc");
        }

        // 중복 체크
        public async Task<int> Overlap_Check(string Dong_Name, string Apt_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Dong Where Dong_Name = @Dong_Name And Apt_Code = @Apt_Code", new { Dong_Name, Apt_Code });
        }

        /// <summary>
        /// 입력된 동 수
        /// </summary>
        /// <param name="Apt_Code">공동주택 동 정보 식별코드</param>
        /// <returns></returns>
        public async Task<int> Being_Dong_Count(string Apt_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Dong Where Apt_Code = @Apt_Code", new { Apt_Code });
        }

        /// <summary>
        /// 공동주택 식별코드로 공동주택 세대수 구하기
        /// </summary>
        /// <param name="Apt_Code">공동주택 식별코드</param>
        /// <returns>세대수 반환</returns>
        public async Task<int> Being_Family_Count(string Apt_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Sum(Family_Num) From Dong Where Apt_Code = @Apt_Code", new { Apt_Code });
        }

        // 동 정보 삭제
        public async Task Remeove_Dong(int AId)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            await db.ExecuteAsync("Delete From Dong Where AId = @AId", new { AId });
        }

        // 동 정보 리스트 구현
        public async Task<List<Dong_Entity>> GetList_Dong(string Apt_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var lst = await db.QueryAsync<Dong_Entity>("Select * From Dong Where Apt_Code = @Apt_Code Order By Dong_Name Asc", new { Apt_Code });
            return lst.ToList();
        }

        // 동 정보 리스트 구현
        public async Task<List<Dong_Entity>> GetList_Dong_Name(string Apt_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var lst = await db.QueryAsync<Dong_Entity>("Select Dong_Name, Dong_Code From Dong Where Apt_Code = @Apt_Code Order By Dong_Name Asc", new { Apt_Code });
            return lst.ToList();
        }

        // 동 정보 코드명으로 검색 정보
        public async Task<string> Detail_Dong_Code(string Dong_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<string>("Select Dong_Name From Dong Where Dong_Code = @Dong_Code", new { Dong_Code });
        }

        // 동 정보 코드명 해당 정보 불러오기
        public async Task<Dong_Entity> Detail_Dong(string Dong_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<Dong_Entity>("Select * From Dong Where Dong_Code = @Dong_Code", new { Dong_Code });
        }
    }

    // 동 구성 정보
    public class Dong_Composition : IDong_Composition
    {
        private readonly IConfiguration _db;

        public Dong_Composition(IConfiguration configuration)
        {
            this._db = configuration;
        }

        // 동 구성 정보 입력
        public async Task<Dong_Composition_Entity> Add_Dong_Composition(Dong_Composition_Entity Dong)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var sql = "Insert Dong_Composition (Dong_Composition_Code, Apt_Code, Dong_Code, Supply_Area, Area_Family_Num, Only_Area, Total_Area, Dong_Etc, PostIP) Values (@Dong_Composition_Code, @Apt_Code, @Dong_Code, @Supply_Area, @Area_Family_Num, @Only_Area, @Total_Area, @Dong_Etc, @PostIP);";
            await db.ExecuteAsync(sql, Dong);
            return Dong;
        }

        // 동 구성 정보 수정
        public async Task<Dong_Composition_Entity> Edit_Dong_Composition(Dong_Composition_Entity Dong)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var sql = "Update Dong_Composition Set Dong_Code = @Dong_Code, Supply_Area = @Supply_Area, Area_Family_Num = @Area_Family_Num, Only_Area = @Only_Area, Total_Area = @Total_Area, Dong_Etc = @Dong_Etc Where Aid = @Aid";
            await db.ExecuteAsync(sql, Dong);
            return Dong;
        }

        // 동 구성 정보 삭제
        public async Task Remeove_Dong_Composition(int AId)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            await db.ExecuteAsync("Delete From Dong_Composition Where AId = @AId", new { AId });
        }

        // 마지막 일련번호 얻기
        public async Task<int> Last_Number()
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Top 1 Aid From Dong_Composition Order by Aid Desc", new { });
        }

        // 중복 체크
        public async Task<int> Overlap_Check(int AId)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Dong_Composition Where AId = @AId", new { AId });
        }

        // 공급면적 합계
        public async Task<double> Total_Supply_Account(string Apt_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<float>("Select isnull(Sum(Supply_Area), 0) From Dong_Composition Where Apt_Code = @Apt_Code", new { Apt_Code });
        }

        // 세대수  합계
        public async Task<int> Total_Family_Account(string Apt_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<int>("Select isnull(Sum(Area_Family_Num), 0) From Dong_Composition Where Apt_Code = @Apt_Code", new { Apt_Code });
        }

        // 관리면적 합계
        public async Task<double> Total_Area_Account(string Apt_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<double>("Select isnull(Sum(Total_Area), 0) From Dong_Composition Where Apt_Code = @Apt_Code", new { Apt_Code });
        }

        /// <summary>
        /// 전체 합계 목록
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<Dong_Composition_Entity> Total_Infor(string Apt_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<Dong_Composition_Entity>("Select (select SUM(Supply_Area)) as Supply_Area, (Select Sum(Area_Family_Num)) as Area_Family_Num, (Select sum(Total_Area)) as Total_Area From Dong_Composition Where Apt_Code = @Apt_Code", new { Apt_Code });
        }

        // 동구성 정보 공동주택코드로 리스트 불러오기
        public async Task<List<Dong_Composition_Entity>> GetList_Dong_Composition(string Apt_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var lst = await db.QueryAsync<Dong_Composition_Entity>("Select * From Dong_Composition Where Apt_Code = @Apt_Code Order By Supply_Area Asc", new { Apt_Code });
            return lst.ToList();
        }

        // 동 구성 정보 코드명으로 해당 상세정보 불러오기
        public async Task<Dong_Composition_Entity> Detail_Sort(string Dong_Composition_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<Dong_Composition_Entity>("Select * From Dong_Composition Where Dong_Composition_Code = @Dong_Composition_Code", new { Dong_Composition_Code });
        }
    }
}