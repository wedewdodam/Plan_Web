using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Plan_Lib
{
    // 관리규약 테이블 정보
    public class Bylaw_Entity
    {
        public int Bylaw_Code { get; set; }
        public string Apt_Code { get; set; }
        public int Bylaw_Revision_Num { get; set; }
        public DateTime Bylaw_Revision_Date { get; set; }
        public string Bylaw_Law_Basis { get; set; }
        public double Approval_Rate { get; set; }
        public string Proposer { get; set; }
        public string Bylaw_Etc { get; set; }
        public string Staff_Code { get; set; }
        public DateTime Post_Date { get; set; }
        public string PostIP { get; set; }
    }

    // 관리규약 입출력 메서드
    public class Bylaw_Lib : IBylaw_Lib
    {
        private readonly IConfiguration _db;

        public Bylaw_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 관리규약 기본 정보 입력
        /// </summary>
        public async Task<Bylaw_Entity> Add_Bylaw(Bylaw_Entity By)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var sql = "Insert Into Bylaw (Apt_Code, Bylaw_Revision_Num, Bylaw_Revision_Date, Bylaw_Law_Basis, Approval_Rate, Proposer, Bylaw_Etc, Staff_Code, PostIP) Values (@Apt_Code, @Bylaw_Revision_Num, @Bylaw_Revision_Date, @Bylaw_Law_Basis, @Approval_Rate, @Proposer, @Bylaw_Etc, @Staff_Code, @PostIP)";
            await db.ExecuteAsync(sql, By);
            return By;
        }

        /// <summary>
        /// 관리규약 기본 정보 수정
        /// </summary>
        public async Task<Bylaw_Entity> Edit_Bylaw(Bylaw_Entity By)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var sql = "Update Bylaw Set Bylaw_Revision_Num = @Bylaw_Revision_Num, Bylaw_Revision_Date = @Bylaw_Revision_Date, Bylaw_Law_Basis = @Bylaw_Law_Basis, Approval_Rate = @Approval_Rate, Proposer = @Proposer, Bylaw_Etc = @Bylaw_Etc, Staff_Code =@Staff_Code, PostIP = @PostIP, Post_Date = @Post_Date Where Bylaw_Code = @Bylaw_Code";
            await db.ExecuteAsync(sql, By);
            return By;
        }

        /// <summary>
        ///관리규약 기본 정보 삭제
        /// </summary>
        public async Task Remove_Repair_Cost(int Bylaw_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            await db.ExecuteAsync("Delete From Bylaw Where Bylaw_Code = @Bylaw_Code", new { Bylaw_Code });
        }

        // 관리규약 기본 정보 리스트(공동주택별)
        public async Task<List<Bylaw_Entity>> GetList(string Apt_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var lst = await db.QueryAsync<Bylaw_Entity>("Select Bylaw_Code, Apt_Code, Bylaw_Revision_Num, Bylaw_Revision_Date, Bylaw_Law_Basis, Approval_Rate, Proposer, Bylaw_Etc, Staff_Code, Post_Date, PostIP From Bylaw Where Apt_Code = @Apt_Code Order By Bylaw_Revision_Date Desc", new { Apt_Code });
            return lst.ToList();
        }

        /// <summary>
        /// 마지막 관리규약 코드
        /// </summary>
        public async Task<int> Bylaw_Last_Code(string Apt_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            int a = 0;
            try
            {
                a = await db.QuerySingleOrDefaultAsync<int>("Select Top 1 Bylaw_Code From Bylaw Where Apt_Code = @Apt_Code Order By Bylaw_Revision_Date Desc", new { Apt_Code });
            }
            catch (Exception)
            {
                a = 0;
            }
            return a;
        }

        /// <summary>
        /// 관리규약 코드 존재 여부
        /// </summary>
        public async Task<int> Being_Bylaw_Code(string Apt_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From Bylaw Where Apt_Code = @Apt_Code", new { Apt_Code });
        }

        /// <summary>
        /// 관리규약 코드정보 불러오기
        /// </summary>
        public async Task<string> Being_Bylaw_Code_be(string Apt_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<string>("Select Top 1 isNull(Bylaw_Code, '0') From Bylaw Where Apt_Code = @Apt_Code Order By Bylaw_Code Desc", new { Apt_Code });
        }

        /// <summary>
        /// 관리규약 상세보기 불러오기
        /// </summary>
        public async Task<Bylaw_Entity> Details_Bylaw(string Apt_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<Bylaw_Entity>("Select Top 1 * From Bylaw Where Apt_Code = @Apt_Code Order By Bylaw_Code Desc", new { Apt_Code });
        }


        /// <summary>
        ///  관리규약 기본 정보 상세보기
        /// </summary>
        public async Task<Bylaw_Entity> GetDetail_Bylaw(int Bylaw_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<Bylaw_Entity>("Select Bylaw_Code, Apt_Code, Bylaw_Revision_Num, Bylaw_Revision_Date, Bylaw_Law_Basis, Approval_Rate, Proposer, Bylaw_Etc, Staff_Code, Post_Date, PostIP From Bylaw Where Bylaw_Code = @Bylaw_Code", new { Bylaw_Code });
        }

        /// <summary>
        ///  관리규약 차수 불러오기
        /// </summary>
        public async Task<int> Bylaw_Revision(string Apt_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Top 1 Bylaw_Revision_Num From Bylaw Where Apt_Code = @Apt_Code Order By Bylaw_Revision_Date Desc", new { Apt_Code });
        }
    }
}