using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Plan_Lib
{
    /// <summary>
    /// 관계법령 테이블 정보
    /// </summary>
    public class Relation_Law_Entity
    {
        public int Relation_Law_Code { get; set; }
        public string Division_Set { get; set; }
        public string Apt_Code { get; set; }
        public string Repair_Plan_Code { get; set; }
        public string Law_Name { get; set; }
        public string Law_Code { get; set; }
        public string Law_Up_Name { get; set; }
        public string Law_Up_Code { get; set; }
        public string Law_Article_Clause { get; set; }
        public string Law_Article_Clause_Code { get; set; }
        public string Law_Article_Detail { get; set; }
        public DateTime Enforce_Date { get; set; }
        public string Relation_Law_Etc { get; set; }
        public string Relation_Index { get; set; }
        public string Staff_Code { get; set; }
        public DateTime PostDate { get; set; }
        public string PostIP { get; set; }
    }

    // 관계법령 입출력 메서드
    public class Relation_Law_Lib : IRelation_Law_Lib
    {
        private readonly IConfiguration _db;

        public Relation_Law_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 관계법령 저장
        /// </summary>
        public async Task<Relation_Law_Entity> Add(Relation_Law_Entity law)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var sql = "Insert Into Relation_Law (Apt_Code, Repair_Plan_Code, Law_Name, Law_Code, Law_Up_Name, Law_Up_Code, Law_Article_Clause, Law_Article_Clause_Code, Law_Article_Detail, Enforce_Date, Relation_Law_Etc, Relation_Index, Staff_Code, PostIP) Values (@Apt_Code, @Repair_Plan_Code, @Law_Name, @Law_Code, @Law_Up_Name, @Law_Up_Code, @Law_Article_Clause, @Law_Article_Clause_Code, @Law_Article_Detail, @Enforce_Date, @Relation_Law_Etc, @Relation_Index, @Staff_Code, @PostIP)";
            await db.ExecuteAsync(sql, law);
            return law;
        }

        /// <summary>
        /// 관계법령 수정
        /// </summary>
        public async Task<Relation_Law_Entity> Edit(Relation_Law_Entity rlae)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var sql = "Update Relation_Law Set Law_Article_Clause = @Law_Article_Clause, Law_Article_Clause_Code = @Law_Article_Clause_Code, Law_Article_Detail = @Law_Article_Detail, Relation_Law_Etc = @Relation_Law_Etc, Relation_Index =@Relation_Index, PostDate = @PostDate, Staff_Code = @Staff_Code, PostIP = @PostIP Where Relation_Law_Code = @Relation_Law_Code";
            await db.ExecuteAsync(sql, rlae);
            return rlae;
        }

        /// <summary>
        /// 관계법령 삭제
        /// </summary>
        public async Task Remove(int Relation_Law_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            await db.ExecuteAsync("Delete Relation_Law Where Relation_Law_Code = @Relation_Law_Code", new { Relation_Law_Code });
        }

        /// <summary>
        /// 관리법령 리스트
        /// </summary>
        public async Task<List<Relation_Law_Entity>> GetList(string Apt_Code, string Repair_Plan_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var lst = await db.QueryAsync<Relation_Law_Entity>("Select * From Relation_Law Where Apt_Code = @Apt_Code Order By Relation_Index Asc", new { Apt_Code, Repair_Plan_Code });
            return lst.ToList();
        }

        /// <summary>
        /// 관리법령 묶음 리스트
        /// </summary>
        public async Task<List<Relation_Law_Entity>> GetList_Set(string Division_Set)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var lst = await db.QueryAsync<Relation_Law_Entity>("Select * From Relation_Law Where Division_Set = @Division_Set Order By Relation_Index Asc", new { Division_Set });
            return lst.ToList();
        }
    }
}