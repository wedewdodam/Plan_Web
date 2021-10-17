using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plan_Blazor_Lib
{
    public class Common_Lib
    {
        public string numberFormat(object code)
        {
            try
            {
                int intA = Convert.ToInt32(code);
                string strA = string.Format("{0: ###,###.###}", intA);

                return strA;
            }
            catch (Exception)
            {
                return "오류";
            }
        }        
    }

    /// <summary>
    /// 시도 클래스
    /// </summary>
    public class Sido_Entity
    {
        /// <summary>
        /// 식별코드
        /// </summary>
        public int Aid { get; set; }

        /// <summary>
        /// 시도 코드
        /// </summary>
        public string Sido_Code { get; set; }

        /// <summary>
        /// 시도명
        /// </summary>
        public string Sido { get; set; }

        /// <summary>
        /// 시군구명
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// 단위 코드
        /// </summary>
        public string Step { get; set; }

        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime PostDate { get; set; }
    }

    public interface ISido_Lib
    {
        /// <summary>
        /// 시도 입력
        /// </summary>
        Task<Sido_Entity> Add_Sido(Sido_Entity sido);

        /// <summary>
        /// 시군구 목록
        /// </summary>
        Task<List<Sido_Entity>> GetList(string Sido);

        /// <summary>
        /// 시군구 수
        /// </summary>
        Task<int> Last_Aid_Sido(string Sido);

        /// <summary>
        /// 시도명 불러오기
        /// </summary>
        Task<string> SidoName(string Sido);

        
    }

    public class Sido_Lib : ISido_Lib
    {
        private readonly IConfiguration _db;

        public Sido_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 시도 입력
        /// </summary>
        public async Task<Sido_Entity> Add_Sido(Sido_Entity sido)
        {
            var sql = "Insert Into sido (Sido_Code, Sido, Region, Step) Values (@Sido_Code, @Sido, @Region, @Step);";
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            await db.ExecuteAsync(sql, sido);
            return sido;
        }

        /// <summary>
        /// 시군구 목록
        /// </summary>
        public async Task<List<Sido_Entity>> GetList(string Sido)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var lst = await db.QueryAsync<Sido_Entity>("Select * From sido Where Sido = @Sido Order By Aid Asc", new { Sido });
            return lst.ToList();
        }

        /// <summary>
        /// 시군구 수
        /// </summary>
        public async Task<int> Last_Aid_Sido(string Sido)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<int>("Select Count(*) From sido Where Sido = @Sido", new { Sido });
        }

        /// <summary>
        /// 시도명 불러오기
        /// </summary>
        public async Task<string> SidoName(string Sido)
        {
            using (var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await db.QuerySingleOrDefaultAsync<string>("Select Top 1 Sido From Sido Where Sido_Code Like '" + Sido + "%'", new { Sido });
            }
        }

        
    }
}