using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Plan_Blazor_Lib.Review
{
    /// <summary>
    /// 총론 검토 기본 정보 클래스
    /// </summary>
    public class Plan_Review_Lib : IPlan_Review_Lib
    {
        private readonly IConfiguration _db;

        public Plan_Review_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        // 장기수선계획 검토 기본 정보 입력
        public async Task<Plan_Review_Entity> Add_Plan_Review(Plan_Review_Entity review)
        {
            var sql = @"Insert Into Plan_Review (Apt_Code, Repair_Plan_Code, PlanReview_Division, PlanReview_Date, PlanReview_Ago, Plan_Period, Saving_Cost, Emergency_Expense, SmallSum_Expense, Levy_Sum, PlanReview_Opinion, PlanReview_Complete, Staff_Code, Plan_Reviewer, PostIP) Values (@Apt_Code, @Repair_Plan_Code, @PlanReview_Division, @PlanReview_Date, @PlanReview_Ago, @Plan_Period, @Saving_Cost, @Emergency_Expense, @SmallSum_Expense, @Levy_Sum, @PlanReview_Opinion, @PlanReview_Complete, @Staff_Code, @Plan_Reviewer, @PostIP);
                Select Cast(SCOPE_IDENTITY() As Int);";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var Aid = await ctx.QuerySingleOrDefaultAsync<int>(sql, review);
                review.Plan_Review_Code = Aid;
                return review;
            }
            //var id = this.dn.ctx.Query<int>(sql, review).Single();
            //review.Plan_Review_Code = id;
            //return review;
        }

        // 장기수선계획 검토 정보 수정
        public async Task<Plan_Review_Entity> Edit_Plan_Review(Plan_Review_Entity review)
        {
            var sql = "Update Plan_Review Set PlanReview_Division = @PlanReview_Division, PlanReview_Date = @PlanReview_Date, PlanReview_Ago = @PlanReview_Ago, Plan_Period = @Plan_Period, Saving_Cost = @Saving_Cost, Emergency_Expense = @Emergency_Expense, SmallSum_Expense = @SmallSum_Expense, Levy_Sum = @Levy_Sum, PlanReview_Opinion = @PlanReview_Opinion, PlanReview_Complete = @PlanReview_Complete, Staff_Code = @Staff_Code, Plan_Reviewer = @Plan_Reviewer, PostIP = @PostIP Where Plan_Review_Code = @Plan_Review_Code";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, review);
                return review;
            }
            //this.dn.ctx.Execute(sql, review);
            //return review;
        }

        // 해당 장기수선계획 검토 정보 존재 여부
        public async Task<int> Being_Plan_Review(string Repair_Plan_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Plan_Review Where Repair_Plan_Code = @Repair_Plan_Code", new { Repair_Plan_Code }, commandType: CommandType.Text);
            }
            //return dn.ctx.Query<int>("Select Count(*) From Plan_Review Where Repair_Plan_Code = @Repair_Plan_Code", new { Repair_Plan_Code }).SingleOrDefault();
        }

        // 해당 장기수선계획 검토 최근 정보 코드 불러오기
        public async Task<int> Last_Plan_Review_Code(string Repair_Plan_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Top 1 Plan_Review_Code From Plan_Review Where Repair_Plan_Code = @Repair_Plan_Code Order By Plan_Review_Code Desc", new { Repair_Plan_Code }, commandType: CommandType.Text);
            }
            //return dn.ctx.Query<int>("Select Top 1 Plan_Review_Code From Plan_Review Where Repair_Plan_Code = @Repair_Plan_Code Order By Plan_Review_Code Desc", new { Repair_Plan_Code }).SingleOrDefault();
        }

        // 해당 장기수선계획 검토 이전 검토일 불러오기
        public async Task<DateTime> Last_Plan_ReviewDate_ago(string Repair_Plan_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<DateTime>("Select Top 2 PlanReview_Date From Plan_Review Where Repair_Plan_Code = @Repair_Plan_Code Order By Plan_Review_Code Desc", new { Repair_Plan_Code }, commandType: CommandType.Text);
            }
            //return dn.ctx.Query<DateTime>("Select Top 2 PlanReview_Date From Plan_Review Where Repair_Plan_Code = @Repair_Plan_Code Order By Plan_Review_Code Desc", new { Repair_Plan_Code }).SingleOrDefault();
        }

        // 해당 장기수선계획 검토 직전 검토일 불러오기
        public async Task<DateTime> Last_Plan_ReviewDate_BeDate(string Repair_Plan_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<DateTime>("Select Top 1 PlanReview_Date From Plan_Review Where Repair_Plan_Code = @Repair_Plan_Code Order By Plan_Review_Code Desc", new { Repair_Plan_Code }, commandType: CommandType.Text);
            }
            //return dn.ctx.Query<DateTime>("Select Top 1 PlanReview_Date From Plan_Review Where Repair_Plan_Code = @Repair_Plan_Code Order By Plan_Review_Code Desc", new { Repair_Plan_Code }).SingleOrDefault();
        }

        // 해당 장기수선계획 검토 직전 검토일 불러오기
        public async Task<DateTime> Last_Plan_ReviewDate_Apt(string Apt_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<DateTime>("Select Top 1 PlanReview_Date From Plan_Review Where Apt_Code = @Apt_Code Order By Plan_Review_Code Desc", new { Apt_Code }, commandType: CommandType.Text);
            }
            //return dn.ctx.Query<DateTime>("Select Top 1 PlanReview_Date From Plan_Review Where Apt_Code = @Apt_Code Order By Plan_Review_Code Desc", new { Apt_Code }).SingleOrDefault();
        }

        // 해당 장기수선계획 정기 검토 존재 여부
        public async Task<int> Being_Plan_Review_Division(string Repair_Plan_Code, string RepairReview_Division)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Plan_Review Where Repair_Plan_Code = @Repair_Plan_Code", new { Repair_Plan_Code, RepairReview_Division }, commandType: CommandType.Text);
            }
            //return dn.ctx.Query<int>("Select Count(*) From Plan_Review Where Repair_Plan_Code = @Repair_Plan_Code", new { Repair_Plan_Code, RepairReview_Division }).SingleOrDefault();
        }

        // 해당 장기수선계획 검토 정보 리스트
        public async Task<List<Plan_Review_Entity>> GetList_Plan_Review(string Repair_Plan_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aa = await ctx.QueryAsync<Plan_Review_Entity>("Select * From Plan_Review Where Repair_Plan_Code = @Repair_Plan_Code", new { Repair_Plan_Code }, commandType: System.Data.CommandType.Text);
                return aa.ToList();
            }
        }

        // 해당 장기수선계획 검토 내용 중에 식별코드와 검토일자 리스트
        public async Task<List<Plan_Review_Entity>> GetList_Plan_Review_Code_Date(string Repair_Plan_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aa = await ctx.QueryAsync<Plan_Review_Entity>("Select Plan_Review_Code, PlanReview_Date From Plan_Review Where Repair_Plan_Code = @Repair_Plan_Code", new { Repair_Plan_Code }, commandType: System.Data.CommandType.Text);
                return aa.ToList();
            }
            //return this.dn.ctx.Query<Plan_Review_Entity>("Select Plan_Review_Code, PlanReview_Date From Plan_Review Where Repair_Plan_Code = @Repair_Plan_Code", new { Repair_Plan_Code }).ToList();
        }

        // 해당 장기수선계획 검토 내용 중에 식별코드와 검토일자 리스트
        public async Task<List<Plan_Review_Entity>> GetList_Apt_Code(string Apt_Code)
        {
            using var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var aa = await ctx.QueryAsync<Plan_Review_Entity>("Select Plan_Review_Code, PlanReview_Date From Plan_Review Where Apt_Code = @Apt_Code", new { Apt_Code }, commandType: System.Data.CommandType.Text);
            return aa.ToList();
        }

        /// <summary>
        /// 검토 목록(페이징)
        /// </summary>
        /// <param name="Page"></param>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<List<Plan_Review_Plan_Entity>> GetList_Apt_Page(int Page, string Apt_Code)
        {
            var sql = "Select Top 15 a.Apt_Code, a.Emergency_Expense, a.Levy_Sum, a.Plan_Period, a.Plan_Review_Code, a.Plan_Reviewer, a.PlanReview_Ago, a.PlanReview_Complete, a.PlanReview_Date, a.PlanReview_Division, a.PlanReview_Opinion, a.PostDate, a.PostIP, a.Repair_Plan_Code, a.Saving_Cost, a.SmallSum_Expense, a.Staff_Code, b.Adjustment_Date, b.Adjustment_Division, b.Adjustment_Man, b.Adjustment_Num, b.Aid, b.Complete, b.Emergency_Basis, b.Repair_Plan_Etc From Plan_Review as a Join Repair_Plan as b On a.Repair_Plan_Code = b.Repair_Plan_Code Where a.Plan_Review_Code Not In (Select Top (15 * @Page) a.Plan_Review_Code From Plan_Review as a Join Repair_Plan as b On a.Repair_Plan_Code = b.Repair_Plan_Code Where a.Apt_Code = @Apt_Code Order By a.Plan_Review_Code Desc) And a.Apt_Code = @Apt_Code Order By a.Plan_Review_Code Desc";
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var lst = await db.QueryAsync<Plan_Review_Plan_Entity>(sql, new { Page, Apt_Code });
            return lst.ToList();
        }

        /// <summary>
        /// 감토 목록 수(페이징)
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<int> GetList_Apt_Page_Count(string Apt_Code)
        {
            var sql = "Select Count(*) From Plan_Review as a Join Repair_Plan as b On a.Repair_Plan_Code = b.Repair_Plan_Code Where a.Apt_Code = @Apt_Code";
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await db.QuerySingleOrDefaultAsync<int>(sql, new { Apt_Code });
        }

        // 장기수선계획 검토 상세정보 불러오기
        public async Task<Plan_Review_Entity> Detail_PlanReview(int Plan_Review_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Plan_Review_Entity>("Select * From Plan_Review Where Plan_Review_Code = @Plan_Review_Code", new { Plan_Review_Code }, commandType: CommandType.Text);
            }
            //return this.dn.ctx.Query<Plan_Review_Entity>("Select * From Plan_Review Where Plan_Review_Code = @Plan_Review_Code", new { Plan_Review_Code }).SingleOrDefault();
        }

        /// <summary>
        /// 최근 검토일 리스트 만들기
        /// </summary>
        /// <param name="Apt_Code">공동주택 식별코드</param>
        /// <returns>검토일 코드 리스트 반환</returns>
        public async Task<string> Getlist_Rereview_Code(string Apt_Code)
        {
            var sql = "Select Top 1 Plan_Review_Code From Repair_Plan Where Apt_Code = @Apt_Code Order By Aid Desc";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<string>(sql, new { Apt_Code }, commandType: CommandType.Text);
            }
            //return this.dn.ctx.Query<string>(sql, new { Apt_Code }).SingleOrDefault();
        }

        /// <summary>
        /// 해당 검토 코드로 검토 구분 불러오기
        /// </summary>
        /// <param name="Plan_Review_Code">검토 식별코드</param>
        /// <returns>검토구분 반환</returns>
        public async Task<string> Rereview_Division(string Plan_Review_Code)
        {
            var sql = "Select PlanReview_Division From Plan_Review Where Plan_Review_Code = @Plan_Review_Code";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<string>(sql, new { Plan_Review_Code }, commandType: CommandType.Text);
            }
            //return this.dn.ctx.Query<string>(sql, new { Plan_Review_Code }).SingleOrDefault();
        }

        /// <summary>
        /// 총론 검토 코드로 장기수선계획 및 총론 검토 내용 불러오기
        /// </summary>
        /// <param name="Plan_Review_Code"></param>
        /// <returns></returns>
        public async Task<Plan_Review_Plan_Entity> Plan_Review_infor(string Plan_Review_Code)
        {
            var sql = "Select top 1 a.PlanReview_Date, b.Adjustment_Date, a.PlanReview_Division From Plan_Review as a Join Repair_Plan as b on a.Repair_Plan_Code = b.Repair_Plan_Code Where a.Plan_Review_Code = @Plan_Review_Code Order by a.Plan_Review_Code Desc";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Plan_Review_Plan_Entity>(sql, new { Plan_Review_Code }, commandType: CommandType.Text);
            }
            //return this.dn.ctx.Query<Plan_Review_Plan_Entity>(sql, new { Plan_Review_Code }).SingleOrDefault();
        }

        /// <summary>
        /// 검토 진행과정 삭제
        /// </summary>
        /// <param name="Repair_Plan_Code"></param>
        public async Task Delete_Rereview(string Repair_Plan_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync("Delete Plan_Review Where Repair_Plan_Code = @Repair_Plan_Code", new { Repair_Plan_Code }, commandType: CommandType.Text);
            }
            //this.dn.ctx.Execute("Delete Plan_Review Where Repair_Plan_Code = @Repair_Plan_Code", new { Repair_Plan_Code });
        }

        /// <summary>
        /// 총론 검토 삭제
        /// </summary>
        /// <param name="Plan_Review_Code"></param>
        public async Task Remove_PlanReview(string Plan_Review_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync("Delete Plan_Review Where Plan_Review_Code = @Plan_Review_Code", new { Plan_Review_Code }, commandType: CommandType.Text);
            }
            //this.dn.ctx.Execute("Delete Plan_Review Where Plan_Review_Code = @Plan_Review_Code", new { Plan_Review_Code });
        }

        /// <summary>
        /// 해당 공동주택 검토서 목록
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<List<Plan_Review_Entity>> Getlist_Apt(string Apt_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aa = await ctx.QueryAsync<Plan_Review_Entity>("Plan_Review_List", new { Apt_Code }, commandType: CommandType.StoredProcedure);
                return aa.ToList();
            }
            //return dn.ctx.Query<Plan_Review_Entity>("Plan_Review_List", new { Apt_Code }, commandType: CommandType.StoredProcedure).ToList();
        }

        /// <summary>
        /// 해당 공동주택 미완료된 검토가 있는지 확인
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <returns></returns>
        public async Task<int> Non_Complete(string Apt_Code)
        {
            using var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await ctx.QuerySingleOrDefaultAsync<int>("SELECT COUNT(*) FROM Plan_Review WHERE (Apt_Code = @Apt_Code) AND (PlanReview_Complete <> '검토 완료')", new { Apt_Code }, commandType: CommandType.Text);
            //return dn.ctx.Query<int>("SELECT COUNT(*) FROM Plan_Review WHERE (Apt_Code = @Apt_Code) AND (PlanReview_Complete <> '검토 완료')", new { Apt_Code }).SingleOrDefault();
        }

        /// <summary>
        /// 검토코드 및 검토일 불러오기
        /// </summary>
        public async Task<List<Plan_Review_Entity>> Review_Infor(string Repair_Plan_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var lst = await db.QueryAsync<Plan_Review_Entity>("Plan_Review_Year", new { Repair_Plan_Code }, commandType: CommandType.StoredProcedure);
            return lst.ToList();
        }
    }

    /// <summary>
    /// 수선항목 검토 정보 클래스
    /// </summary>
    public class Review_Content_Lib : IReview_Content_Lib
    {
        private readonly IConfiguration _db;

        public Review_Content_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        // 상세 검토 입력
        public async Task<Review_Content_Entity> Add_Review_Content(Review_Content_Entity content)
        {
            var sql = "Insert Into Plan_Review_Content (Plan_Review_Code, Sort_A_Code, Sort_B_Code, Sort_C_Code, Repair_Article_Code, Repair_Article_Review, Repair_Cycle_Review, Repair_Part_Rate_Review, Repair_Cost_Review, Review_Content, Staff_Code, Apt_Code, PostIP) Values (@Plan_Review_Code, @Sort_A_Code, @Sort_B_Code, @Sort_C_Code, @Repair_Article_Code, @Repair_Article_Review, @Repair_Cycle_Review, @Repair_Part_Rate_Review, @Repair_Cost_Review, @Review_Content, @Staff_Code, @Apt_Code, @PostIP)";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, content);
                return content;
            }
            //this.dn.ctx.Execute(sql, content);
            //return content;
        }

        // 상세 검토 수정
        public async Task<Review_Content_Entity> Edit_Review_Content(Review_Content_Entity content)
        {
            var sql = "Update Plan_Review_Content Set Repair_Article_Review = @Repair_Article_Review, Repair_Cycle_Review = @Repair_Cycle_Review, Repair_Part_Rate_Review = @Repair_Part_Rate_Review, Repair_Cost_Review = @Repair_Cost_Review, Review_Content = @Review_Content, Staff_Code = @Staff_Code, PostIP = @PostIP Where Plan_Review_Content_Code = @Plan_Review_Content_Code";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, content);
                return content;
            }
            //this.dn.ctx.Execute(sql, content);
            //return content;
        }

        /// <summary>
        /// 수선항목 검토 삭제
        /// </summary>
        public async Task Remove_Repair_Review_Content(int Plan_Review_Content_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync("Delete Plan_Review_Content Where Plan_Review_Content_Code = @Plan_Review_Content_Code", new { Plan_Review_Content_Code });
            }
            //this.dn.ctx.Execute("Delete Plan_Review_Content Where Plan_Review_Content_Code = @Plan_Review_Content_Code", new { Plan_Review_Content_Code });
            //this.ctx.Execute("Delete From Dong_Composition Where AId = @AId", new { AId });
        }

        // 상세 검토 리스트(대분류)
        public async Task<List<Review_Content_Entity>> GetList_Review_Content_Sort(string Apt_Code, string Plan_Review_Code, string Sort_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aa = await ctx.QueryAsync<Review_Content_Entity>("Report_Repair_Review_Content", new { Apt_Code, Plan_Review_Code, Sort_Code }, commandType: CommandType.StoredProcedure);
                return aa.ToList();
            }
            //return this.dn.ctx.Query<Review_Content_Entity>("Report_Repair_Review_Content", new { Apt_Code, Plan_Review_Code, Sort_Code }, commandType: CommandType.StoredProcedure).ToList();
        }

        public async Task<List<Review_Content_Entity>> GetList_Sort(string Plan_Review_Code, string Sort_Field, string Sort_Code, string Apt_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aa = await ctx.QueryAsync<Review_Content_Entity>("Review_Plan_Sort", new { Plan_Review_Code, Sort_Field, Sort_Code, Apt_Code }, commandType: CommandType.StoredProcedure);
                return aa.ToList();
            }
            //return this.dn.ctx.Query<Review_Content_Entity>("Review_Plan_Sort", new { Plan_Review_Code, Sort_Field, Sort_Code, Apt_Code }, commandType: CommandType.StoredProcedure).ToList();
        }

        // 상세 검토 리스트
        public async Task<List<Review_Content_Entity>> GetList_Review_Content(string Plan_Review_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aa = await ctx.QueryAsync<Review_Content_Entity>("Select * From Plan_Review_Content Where Plan_Review_Code = @Plan_Review_Code Order By Sort_A_Code, Sort_B_Code, Sort_C_Code, Plan_Review_Content_Code Asc", new { Plan_Review_Code }, commandType: CommandType.Text);
                return aa.ToList();
            }
            //return this.dn.ctx.Query<Review_Content_Entity>("Select * From Plan_Review_Content Where Plan_Review_Code = @Plan_Review_Code Order By Sort_A_Code, Sort_B_Code, Sort_C_Code, Plan_Review_Content_Code Asc", new { Plan_Review_Code }).ToList();
        }

        // 상세 검토 상세보기
        public async Task<Review_Content_Entity> Detail_Review_Content(int Plan_Review_Content_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Review_Content_Entity>("Select * From Plan_Review_Content Where Plan_Review_Content_Code = @Plan_Review_Content_Code", new { Plan_Review_Content_Code }, commandType: CommandType.Text);
            }
            //return this.dn.ctx.Query<Review_Content_Entity>("Select * From Plan_Review_Content Where Plan_Review_Content_Code = @Plan_Review_Content_Code", new { Plan_Review_Content_Code }).SingleOrDefault();
        }

        // 상세검토 수선항목으로 입력여부 확인
        public async Task<int> Being_Review_Content_Code(string Repair_Article_Code, string Plan_Review_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Plan_Review_Content_Code From Plan_Review_Content Where Repair_Article_Code = @Repair_Article_Code And Plan_Review_Code = @Plan_Review_Code", new { Repair_Article_Code, Plan_Review_Code }, commandType: CommandType.Text);
            }
            //return this.dn.ctx.Query<int>("Select Plan_Review_Content_Code From Plan_Review_Content Where Repair_Article_Code = @Repair_Article_Code And Plan_Review_Code = @Plan_Review_Code", new { Repair_Article_Code, Plan_Review_Code }).SingleOrDefault();
        }

        // 상세검토 수선항목으로 입력여부 확인
        public int Being_Review_Content(string Repair_Article_Code, string Plan_Review_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return ctx.QuerySingleOrDefault<int>("Select Count(*) From Plan_Review_Content Where Repair_Article_Code = @Repair_Article_Code And Plan_Review_Code = @Plan_Review_Code", new { Repair_Article_Code, Plan_Review_Code }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 검토 코드로 수선항목 검토 삭제
        /// </summary>
        /// <param name="Plan_Review_Code">검토 식별 코드</param>
        public async Task Delete_Repair_Review_Content(string Plan_Review_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync("Delete Plan_Review_Content Where Plan_Review_Code = @Plan_Review_Code", new { Plan_Review_Code }, commandType: CommandType.Text);
            }
            //this.dn.ctx.Execute("Delete Plan_Review_Content Where Plan_Review_Code = @Plan_Review_Code", new { Plan_Review_Code });
            //this.ctx.Execute("Delete From Dong_Composition Where AId = @AId", new { AId });
        }

        /// <summary>
        /// 수선항목 및 검토 코드로 해당 정보 가져오기
        /// </summary>
        /// <param name="Plan_Review_Code"></param>
        /// <param name="Repair_Article_Code"></param>
        /// <returns></returns>
        public async Task<Review_Content_Entity> Detail_PlanReview_Article(string Plan_Review_Code, string Repair_Article_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Review_Content_Entity>("Select Plan_Review_Content_Code, Plan_Review_Code, Sort_A_Code, Sort_B_Code, Sort_C_Code, Repair_Article_Code, Repair_Article_Review, Repair_Cycle_Review, Repair_Part_Rate_Review, Repair_Cost_Review, Review_Content, Staff_Code, Apt_Code, PostDate, PostIP From Plan_Review_Content Where Plan_Review_Code = @Plan_Review_Code And Repair_Article_Code = @Repair_Article_Code", new { Plan_Review_Code, Repair_Article_Code }, commandType: CommandType.Text);
            }
        }

        #region 2019년 1월 5일 추가 함.

        /// <summary>
        /// 수선 주기 및 금액 조정에서 검토 내용 불러오기
        /// </summary>
        /// <param name="Plan_Review_Code"></param>
        /// <param name="Apt_Code"></param>
        /// <param name="Repair_Article_Name"></param>
        /// <returns></returns>
        public async Task<Review_Content_Join_Enity> detail_ReeviewCode_ArticleName(string Plan_Review_Code, string Apt_Code, string Repair_Article_Name)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Review_Content_Join_Enity>("Select a.Repair_Article_Name, b.Repair_Article_Code, b.Repair_Article_Review, b.Repair_Cycle_Review, b.Repair_Cost_Review, b.Repair_Part_Rate_Review, b.Review_Content From Repair_Article as a Join Plan_Review_Content as b on a.Repair_Article_Name = b.Repair_Article_Name Where a.Apt_Code = @Apt_Code And b.Plan_Review_Code = @Plan_Review_Code And a.Repair_Article_Name = @Repair_Article_Name", new { Plan_Review_Code, Apt_Code, Repair_Article_Name }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 수선 주기 및 금액 조정에서 검토 내용 불러오기
        /// </summary>
        /// <param name="Plan_Review_Code"></param>
        /// <param name="Apt_Code"></param>
        /// <param name="Repair_Article_Name"></param>
        /// <returns></returns>
        public async Task<Review_Content_Join_Enity> View_ReviewCode_ArticleName(string Plan_Review_Code, string Apt_Code, string Repair_Article_Name)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Review_Content_Join_Enity>("Select a.Repair_Article_Name, b.Repair_Article_Code, b.Repair_Article_Review, b.Repair_Cycle_Review, b.Repair_Cost_Review, b.Repair_Part_Rate_Review, b.Review_Content From Repair_Article as a Join Plan_Review_Content as b on a.Aid = b.Repair_Article_Code Where a.Apt_Code = @Apt_Code And b.Plan_Review_Code = @Plan_Review_Code And a.Repair_Article_Name = @Repair_Article_Name", new { Plan_Review_Code, Apt_Code, Repair_Article_Name }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 수선항목 조정에서 검토내용을 불러오기
        /// </summary>
        /// <param name="Plan_Review_Code"></param>
        /// <param name="Apt_Code"></param>
        /// <param name="Sort_C_Code"></param>
        /// <returns></returns>
        public async Task<List<Review_Content_Join_Enity>> detail_ReviewCode_Sort(string Plan_Review_Code, string Apt_Code, string Sort_C_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aa = await ctx.QueryAsync<Review_Content_Join_Enity>("Select a.Repair_Article_Name, b.Repair_Article_Code, b.Repair_Article_Review, b.Repair_Cycle_Review, b.Repair_Cost_Review, b.Repair_Part_Rate_Review, b.Review_Content From Repair_Article as a Join Plan_Review_Content as b on a.Aid = b.Repair_Article_Code Where a.Apt_Code = @Apt_Code And b.Plan_Review_Code = @Plan_Review_Code And a.Sort_C_Code = @Sort_C_Code", new { Plan_Review_Code, Apt_Code, Sort_C_Code }, commandType: CommandType.Text);
                return aa.ToList();
            }
        }

        #endregion 2019년 1월 5일 추가 함.

        #region 2021년 6월 29일 추가

        /// <summary>
        /// 대분류별 검토내용 목록 불러오기
        /// </summary>
        /// <param name="Plan_Review_Code"></param>
        /// <param name="Apt_Code"></param>
        /// <param name="Sort_C_Code"></param>
        /// <returns></returns>
        public async Task<List<Review_Content_Join_Enity>> GetList_ReviewCode_Sort(string Plan_Review_Code, string Apt_Code, string Sort_A_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aa = await ctx.QueryAsync<Review_Content_Join_Enity>("Select a.Apt_Code, a.Repair_Plan_Code, a.Repair_Article_Name, b.Plan_Review_Content_Code, b.Repair_Article_Code, b.Repair_Article_Review, b.Repair_Cycle_Review, b.Repair_Cost_Review, b.Repair_Part_Rate_Review, b.Review_Content, b.Plan_Review_Code, b.PostDate From Repair_Article as a Join Plan_Review_Content as b on a.Aid = b.Repair_Article_Code Where a.Apt_Code = @Apt_Code And b.Plan_Review_Code = @Plan_Review_Code And a.Sort_A_Code = @Sort_A_Code", new { Plan_Review_Code, Apt_Code, Sort_A_Code }, commandType: CommandType.Text);
                return aa.ToList();
            }
        }

        #endregion 2021년 6월 29일 추가

        /// <summary>
        /// 수선항목 및 검토 코드로 해당 정보 존재 여부 확인
        /// </summary>
        /// <param name="Plan_Review_Code"></param>
        /// <param name="Repair_Article_Code"></param>
        /// <returns></returns>
        public async Task<int> Detail_PlanReview_Article_Count(string Plan_Review_Code, string Repair_Article_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Plan_Review_Content Where Plan_Review_Code = @Plan_Review_Code And Repair_Article_Code = @Repair_Article_Code", new { Plan_Review_Code, Repair_Article_Code }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 검토된 수선항목 수
        /// </summary>
        /// <param name="Apt_Code"></param>
        /// <param name="Plan_Review_Code"></param>
        /// <returns></returns>
        public int Review_Content_Count(string Apt_Code, string Plan_Review_Code)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return db.QuerySingleOrDefault<int>("Select Count(*) From Plan_Review_Content Where Plan_Review_Code = @Plan_Review_Code And Apt_Code = Apt_Code", new { Apt_Code, Plan_Review_Code });
        }

        /// <summary>
        /// 수선항목에서 삭제 시에 검토 수선항목 삭제
        /// </summary>
        /// <param name="Repair_Article_Code"></param>
        public async Task Remove_Article_ReviewContent(string Repair_Article_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync("Delete Plan_Review_Content Where Repair_Article_Code = @Repair_Article_Code", new { Repair_Article_Code }, commandType: CommandType.Text);
            }
        }
    }
}