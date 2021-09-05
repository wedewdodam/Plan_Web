using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;

using System.Linq;
using System.Threading.Tasks;

namespace Plan_Blazor_Lib.Common
{
    public class LogView_Entity
    {
        //public int Aid { get; set; }
        public string Apt_Code { get; set; }

        public string Staff_Code { get; set; }
        public string Staff_password { get; set; }
        //public DateTime PostDate { get; set; }
    }

    public class LogView_Lib
    {
        private readonly IConfiguration _db;

        public LogView_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        public async Task<LogView_Entity> Add_LogView(LogView_Entity LogView)
        {
            var khma = "Staff_Insert";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(khma, LogView, commandType: CommandType.StoredProcedure);
                return LogView;
            }
        }

        public async Task<int> LogIn(string Staff_Code, string Staff_password)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Staff_Login", new { Staff_Code, Staff_password }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<string> GetDetail_LogView(string Staff_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<string>("Select Apt_Code From Log_View Where Staff_Code = @Staff_Code", new { Staff_Code });
            }
        }
    }

    /// <summary>
    /// Logs 테이블과 일대일로 매핑되는 모델(뷰 모델, 엔터티) 클래스
    /// </summary>
    public class Log_View_Entity
    {
        /// <summary>
        /// 일련번호
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 비고
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// 응용 프로그램: 게시판 관리, 상품 관리
        /// </summary>
        public string Application { get; set; }

        /// <summary>
        /// 사용자 정보(로그인 사용 아이디)
        /// </summary>
        public string Logger { get; set; }

        /// <summary>
        /// 로그 이벤트(사용자 정의 이벤트 ID)
        /// </summary>
        public string LogEvent { get; set; }

        /// <summary>
        /// 로그 메시지
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 로그 메시지에 대한 템플릿
        /// </summary>
        public string MessageTemplate { get; set; }

        /// <summary>
        /// 로그 레벨(정보, 에러, 경고)
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// 로그 발생 시간(LogCreationDate)
        /// </summary>
        public DateTimeOffset TimeStamp { get; set; }

        /// <summary>
        /// 예외 메시지
        /// </summary>
        public string Exception { get; set; }

        /// <summary>
        /// 기타 여러 속성들을 XML 저장
        /// </summary>
        public string Properties { get; set; }

        /// <summary>
        /// 호출사이트
        /// </summary>
        public string Callsite { get; set; }

        /// <summary>
        /// IP 주소
        /// </summary>
        public string IpAddress { get; set; }
    }

    public interface ILog_View_Lib
    {
        Task<Log_View_Entity> Add(Log_View_Entity model);

        Task<List<Log_View_Entity>> GetAllWithPaging(int pageIndex, int pageSize);

        Task<Log_View_Entity> GetById(int id);

        Task<int> GetCount();

        Task DeleteByIds(params int[] ids);

        Task<List<Log_View_Entity>> SearchLogsBySearchQuery(
            string startDate, string endDate);
    }

    public class Log_View_Lib
    {
        private readonly IConfiguration _db;

        public Log_View_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 로그 저장하기
        /// </summary>
        public async Task<Log_View_Entity> Add(Log_View_Entity model)
        {
            var sql = @"Insert Into Logs (Note, Application, Logger, LogEvent, Message, MessageTemplate, Level, TimeStamp, Exception, Properties, Callsite, IpAddress)
                Values (@Note, @Application, @Logger, @LogEvent, @Message, @MessageTemplate, @Level, @TimeStamp, @Exception, @Properties, @Callsite, @IpAddress);
                Select Cast(SCOPE_IDENTITY() As Int);
            ";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var id = await ctx.QuerySingleOrDefaultAsync<int>(sql, model);
                model.Id = id;
                return model;
            }
        }

        /// <summary>
        /// 페이징 처리된 로그 리스트
        /// </summary>
        public async Task<List<Log_View_Entity>> GetAllWithPaging(int pageIndex, int pageSize)
        {
            // 인라인 SQL(Ad Hoc 쿼리) 또는 저장 프로시저 지정
            string sql = "GetLogsWithPaging"; // 페이징 저장 프로시저

            // 파라미터 추가
            var parameters = new DynamicParameters();
            parameters.Add("@PageIndex",
                value: pageIndex,
                dbType: DbType.Int32,
                direction: ParameterDirection.Input);
            parameters.Add("@PageSize",
                value: pageSize,
                dbType: DbType.Int32,
                direction: ParameterDirection.Input);

            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                // 실행
                var Lsit = await ctx.QueryAsync<Log_View_Entity>(sql, parameters,
                    commandType: CommandType.StoredProcedure);
                return Lsit.ToList();
            }
        }

        /// <summary>
        /// 특정 번호(Id)에 해당하는 로그
        /// </summary>
        public async Task<Log_View_Entity> GetById(int id)
        {
            string sql = "Select * From Logs Where Id = @Id";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Log_View_Entity>(sql, new { id }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 로그 테이블의 전체 레코드 수
        /// </summary>
        public async Task<int> GetCount()
        {
            var sql = "Select Count(*) From Logs";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>(sql, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 선택 삭제
        /// </summary>
        public async Task DeleteByIds(params int[] ids)
        {
            string sql = "Delete Logs Where Id In @Ids";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, new { Ids = ids }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 시작 날짜와 종료 날짜 사이의 데이터 검색
        /// </summary>
        public async Task<List<Log_View_Entity>> SearchLogsBySearchQuery(
            string startDate, string endDate)
        {
            string sql = @"
                Select * From Logs
                Where
                    TimeStamp
                        Between @StartDate And @EndDate";

            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var Lsit = await ctx.QueryAsync<Log_View_Entity>(sql, new { startDate, endDate }, commandType: CommandType.Text);
                return Lsit.ToList();
            }
        }
    }

    //public class Loger
    //{
    //    public static void Log(
    //        string application = "",
    //        string logger = "",
    //        string message = "",
    //        string ipAddress = ""
    //    )
    //    {
    //        try
    //        {
    //            var _repository = new Log_View_Lib();

    //            var model = new Log_View_Entity
    //            {
    //                Application = application,
    //                Logger = logger,
    //                Message = message,
    //                IpAddress = ipAddress,
    //                TimeStamp = DateTimeOffset.Now
    //            };

    //            _repository.Add(model);
    //        }
    //        catch (Exception)
    //        {
    //            // Logger 때문에 발생하는 예외는 무시
    //        }
    //    }

    //    public static void Log(string note = "", string application = "", string logger = "", string message = "", string ipAddress = "")
    //    {
    //        try
    //        {
    //            var _repository = new Log_View_Lib();

    //            var model = new Log_View_Entity
    //            {
    //                Note = note,
    //                Application = application,
    //                Logger = logger,
    //                Level = "",
    //                Message = message,
    //                IpAddress = ipAddress,
    //                TimeStamp = DateTimeOffset.Now
    //            };

    //            _repository.Add(model);
    //        }
    //        catch (Exception)
    //        {
    //            // Logger 때문에 발생하는 예외는 무시
    //        }
    //    }
    //}
}