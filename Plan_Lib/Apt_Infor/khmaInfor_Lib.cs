using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Plan_Apt_Lib;
using System.Data;
using System.Threading.Tasks;

namespace Plan_Blazor_Lib
{
    public interface IkhmaInfor_Lib
    {
        /// <summary>
        /// 로그인 확인 int 값으로
        /// </summary>
        //Task<int> logview(string mem_id, string mem_pw);
        int logview(string mem_id, string mem_pw);

        /// <summary>
        /// 아이디로 로그인된 아파트정보, 회원정보, 배치정보 불러오기
        /// </summary>
        Task<khma_AptMemberCareer_Entity> kmi_detail(string mem_id);

        //khma_AptMemberCareer_Entity kmi_detail(string mem_id);
    }

    public class khmaInfor_Lib : IkhmaInfor_Lib
    {
        private readonly IConfiguration _db;

        public khmaInfor_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 로그인
        /// </summary>
        public int logview(string mem_id, string mem_pw)
        {
            var sql = "Select Count(*) From member_info_v Where mem_id = @mem_id And mem_pw = SecureDB.dbsec.encrypt('" + mem_pw + "','SecureDB.dbsec.SECUREKEY.SHA256','')";

            using (var df = new SqlConnection(_db.GetConnectionString("Khma_db_Connettion")))
            {
                df.Open();
                var result = df.QuerySingleOrDefault<int>(sql, new { mem_id, mem_pw }, commandType: CommandType.Text);
                df.Close();
                return result;
            }

            //int aaa = _db.Query<int>(sql, new { mem_id, mem_pw }).FirstOrDefault();

            //return aaa;
        }

        public async Task<khma_AptMemberCareer_Entity> kmi_detail(string mem_id)
        {
            //var sql = "Select Top 1 * From member_info_v as a join member_career_v as b on a.mem_cd = b.mem_cd  Join apart_info_v as c on b.apt_cd = c.APT_CD Where mem_id = @mem_id";
            using (var df = new SqlConnection(_db.GetConnectionString("Khma_db_Connettion")))
            {
                df.Open();
                return await df.QuerySingleOrDefaultAsync<khma_AptMemberCareer_Entity>("Select Top 1 * From member_info_v as a join member_career_v as b on a.mem_cd = b.mem_cd Join apart_info_v as c on b.apt_cd = c.APT_CD Where mem_id = @mem_id", new { mem_id }, commandType: CommandType.Text);
                //string a = "";
                //df.Close();
            }
        }
    }
}