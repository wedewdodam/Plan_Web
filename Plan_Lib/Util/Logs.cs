using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Threading.Tasks;

namespace Plan_Blazor_Lib
{
    public interface ILogView
    {
        Task<LogView_Entity> Add_LogView(LogView_Entity LogView);

        Task<int> LogIn(string Staff_Code, string Staff_password);

        Task<string> GetDetail_LogView(string Staff_Code);

        Task<LogView_Entity> Detail_LogView(string Staff_Code);
    }

    public class LogView : ILogView
    {
        private readonly IConfiguration _db;

        public LogView(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 로그인 회원 정보 입력
        /// </summary>
        /// <param name="LogView"></param>
        /// <returns></returns>
        public async Task<LogView_Entity> Add_LogView(LogView_Entity LogView)
        {
            var khma = "Staff_Insert";
            using (var aa = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await aa.ExecuteAsync(khma, LogView, commandType: CommandType.StoredProcedure);
                return LogView;
            }
        }

        /// <summary>
        /// 로그인
        /// </summary>
        /// <param name="Staff_Code"></param>
        /// <param name="Staff_password"></param>
        /// <returns></returns>
        public async Task<int> LogIn(string Staff_Code, string Staff_password)
        {
            var khma = "Staff_Login";
            using (var aa = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await aa.QuerySingleOrDefaultAsync<int>(khma, new { Staff_Code, Staff_password }, commandType: CommandType.StoredProcedure);
                //return LogView;
            }
            //return await aa.ctx.Query<int>("Staff_Login", new { Staff_Code, Staff_password }, commandType: CommandType.StoredProcedure).SingleOrDefault();
        }

        /// <summary>
        /// 해당 회원의 공동주택 코드 가져오기
        /// </summary>
        /// <param name="Staff_Code"></param>
        /// <returns></returns>
        public async Task<string> GetDetail_LogView(string Staff_Code)
        {
            using (var aa = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await aa.QuerySingleOrDefaultAsync<string>("Select Apt_Code From Log_View Where Staff_Code = @Staff_Code", new { Staff_Code }, commandType: CommandType.Text);
                //return LogView;
            }
            //return aa.ctx.Query<string>("Select Apt_Code From Log_View Where Staff_Code = @Staff_Code", new { Staff_Code }).SingleOrDefault();
        }

        /// <summary>
        /// 해당 회원의 공동주택 코드 가져오기
        /// </summary>
        /// <param name="Staff_Code"></param>
        /// <returns></returns>
        public async Task<LogView_Entity> Detail_LogView(string Staff_Code)
        {
            using (var aa = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await aa.QuerySingleOrDefaultAsync<LogView_Entity>("Select * From Log_View Where Staff_Code = @Staff_Code", new { Staff_Code }, commandType: CommandType.Text);
                //return LogView;
            }
            //return aa.ctx.Query<string>("Select Apt_Code From Log_View Where Staff_Code = @Staff_Code", new { Staff_Code }).SingleOrDefault();
        }
    }
}