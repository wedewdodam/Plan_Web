using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Plan_Blazor_Lib.Note_View
{
    /// <summary>
    /// 게시판 분류
    /// </summary>
    public class Sort_Note_Entity
    {
        public int Tid { get; set; }
        public string Sort_A_Code { get; set; }
        public string Sort_A_Name { get; set; }
        public string Sort_B_Code { get; set; }
        public string Sort_B_Name { get; set; }
        public string Sort_C_Code { get; set; }
        public string Sort_C_Name { get; set; }
        public string Sort_D_Code { get; set; }
        public string Sort_D_Name { get; set; }
        public string Sort_E_Code { get; set; }
        public string Sort_E_Name { get; set; }
        public string Sort_F_Code { get; set; }
        public string Sort_F_Name { get; set; }
        public string Sort_G_Code { get; set; }
        public string Sort_G_Name { get; set; }
        public string Sort_Step { get; set; }
        public string Etc { get; set; }
        public int SysopUID { get; set; }
        public Boolean tPublic { get; set; }
        public Boolean MailEnble { get; set; }
        public string Note_Index { get; set; }
        public string Sort_Enable { get; set; }
        public string Staff_Code { get; set; }
        public DateTime PostDate { get; set; }
        public string PostIP { get; set; }
    }

    /// <summary>
    /// 게시판 엔터티
    /// </summary>
    public class Khma_Note_Entity
    {
        public int Aid { get; set; }
        public string Note_Code { get; set; }
        public string Apt_Code { get; set; }
        public string Apt_Name { get; set; }
        public string User_Code { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public DateTime PostDate { get; set; }
        public string PostIP { get; set; }
        public string Content { get; set; }
        public int ReadCount { get; set; }
        public string Encoding { get; set; }
        public DateTime ModifyDate { get; set; }
        public string ModifyIP { get; set; }
        public int Ref { get; set; }
        public int Step { get; set; }
        public int RefOrder { get; set; }
        public int AnswerNum { get; set; }
        public int ParentNum { get; set; }
        public int CommentCount { get; set; }
        public int Notice { get; set; }
        public string Sort_A_Code { get; set; }
        public string Sort_B_Code { get; set; }
        public string Sort_C_Code { get; set; }
        public string Sort_D_Code { get; set; }
        public string Sort_Code { get; set; }
        public string Sort_Name { get; set; }
        public string Sort_Step { get; set; }
        public string Secret { get; set; }
    }

    /// <summary>
    /// 게시판 코멘트
    /// </summary>
    public class Khma_NoteComments_Entity
    {
        /// <summary>
        /// 식별코드
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        /// 부모 게시판 명 식별코드
        /// </summary>
        public string BoardName { get; set; }

        /// <summary>
        /// 부모 게시판 식별코드
        /// </summary>
        public int BoardAid { get; set; }

        /// <summary>
        /// 댓글 작성자 아이디
        /// </summary>
        public string User_Code { get; set; }

        /// <summary>
        /// 작성자 명
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 댓글 내용
        /// </summary>
        public string Opinion { get; set; }

        /// <summary>
        /// 작성일
        /// </summary>
        public DateTime PostDate { get; set; }

        public string PostIP { get; set; }

        /// <summary>
        /// 비밀글 여부
        /// </summary>
        public string Secret { get; set; }

        public DateTime ModifyDate { get; set; }
        public string ModifyIP { get; set; }
    }

    public interface ISort_Note_Lib
    {
        Task<Sort_Note_Entity> Add(Sort_Note_Entity sne);

        Task<Sort_Note_Entity> Edit(Sort_Note_Entity sne);

        Task<List<Sort_Note_Entity>> GetList();

        Task<string> Sort_Name(string Sort_Code, string Sort_Step);

        Task<string> Sort_Etc(string Sort_Code, string Sort_Step);

        Task<List<Sort_Note_Entity>> GetList_Sort(string Sort_Code);

        Task<List<Sort_Note_Entity>> GetList_Step(string Sort_Step);

        Task<List<Sort_Note_Entity>> GetList_Code(string Feild, string Query);

        Task<List<Sort_Note_Entity>> GetList_Index(string Note_Index);

        Task<List<Sort_Note_Entity>> GetList_Step_Index(string Sort_Step, string Note_Index);

        Task<List<Sort_Note_Entity>> GetList_Sort_Index(string Sort_Step, string Code);

        Task<int> Step_Count(string Sort_Step);

        Task<int> Code_Count(string Feild, string Query);

        Task Edit_Enable(int Tid, string Sort_Enable);

        Task Delete(int Tid);

        Task<Sort_Note_Entity> Detail(string SortFeild, string Sort_Code);
    }

    /// <summary>
    /// 게시판 분류
    /// </summary>
    public class Sort_Note_Lib : ISort_Note_Lib
    {
        private readonly IConfiguration _db;

        public Sort_Note_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 게시판 분류 입력
        /// </summary>
        /// <param name="sne">게시판 분류 엔터티</param>
        /// <returns>등록</returns>
        public async Task<Sort_Note_Entity> Add(Sort_Note_Entity sne)
        {
            var sql = "Insert Into Khma_Sort_Note (Sort_A_Name, Sort_A_Code, Sort_B_Name, Sort_B_Code, Sort_C_Name, Sort_C_Code, Sort_D_Name, Sort_D_Code, Sort_E_Name, Sort_E_Code, Sort_F_Name, Sort_F_Code, Sort_G_Name, Sort_G_Code, Sort_Step, Note_Index, Etc, Staff_Code, PostIP) Values (@Sort_A_Name, @Sort_A_Code, @Sort_B_Name, @Sort_B_Code, @Sort_C_Name, @Sort_C_Code, @Sort_D_Name, @Sort_D_Code, @Sort_E_Name, @Sort_E_Code, @Sort_F_Name, @Sort_F_Code, @Sort_G_Name, @Sort_G_Code, @Sort_Step, @Note_Index, @Etc, @Staff_Code, @PostIP)";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, sne, commandType: CommandType.Text);
                return sne;
            }
            //dn.ctx.Execute(sql, sne);
            //return sne;
        }

        /// <summary>
        /// 게시판 분류 수정
        /// </summary>
        /// <param name="sne">게시판 분류 엔터티</param>
        /// <returns>수정</returns>
        public async Task<Sort_Note_Entity> Edit(Sort_Note_Entity sne)
        {
            var sql = "Update Khma_Sort_Note Set Sort_A_Name = @Sort_A_Name, Sort_B_Name = @Sort_B_Name, Sort_C_Name = @Sort_C_Name, Sort_D_Name = @Sort_D_Name, Sort_E_Name = @Sort_E_Name, Sort_F_Name = @Sort_F_Name, Sort_G_Name = @Sort_G_Name, Sort_Step = @Sort_Step, Note_Index = @Note_Index, Etc = @Etc, Sort_Enable = @Sort_Enable, Staff_Code = @Staff_Code, PostDate = @PostDate, PostIP = @PostIP Where Tid = @Tid";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, sne, commandType: CommandType.Text);
                return sne;
            }
            //dn.ctx.Execute(sql, sne);
            //return sne;
        }

        /// <summary>
        /// 게시판 분류 목록 만들기
        /// </summary>
        /// <returns>목록</returns>
        public async Task<List<Sort_Note_Entity>> GetList()
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var ss = await ctx.QueryAsync<Sort_Note_Entity>("Select * From Khma_Sort_Note Order By Tid Desc", commandType: CommandType.Text);
                return ss.ToList();
            }
            //return dn.ctx.Query<Sort_Note_Entity>("Select * From Khma_Sort_Note Order By Tid Desc", new { }).ToList();
        }

        /// <summary>
        /// 게시판 분류 명 불러오기
        /// </summary>
        /// <param name="Sort_Code">게시판 분류 식별코드</param>
        /// <param name="Sort_Step">분류 단계</param>
        /// <returns>분류명</returns>
        public async Task<string> Sort_Name(string Sort_Code, string Sort_Step)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<string>("Select top 1 Sort_" + Sort_Step + "_Name From Khma_Sort_Note Where Sort_" + Sort_Step + "_Code = @Sort_Code Order by Tid Desc", new { Sort_Code, Sort_Step }, commandType: CommandType.Text);
                //return ss.ToList();
            }
            //return dn.ctx.Query<string>("Select top 1 Sort_" + Sort_Step + "_Name From Khma_Sort_Note Where Sort_" + Sort_Step + "_Code = @Sort_Code Order by Tid Desc", new { Sort_Code, Sort_Step }).SingleOrDefault();
        }

        /// <summary>
        /// 게시판 설명 불러오기
        /// </summary>
        /// <param name="Sort_Code">게시판 분류 식별코드</param>
        /// <param name="Sort_Step">분류 단계</param>
        /// <returns>상세설명</returns>
        public async Task<string> Sort_Etc(string Sort_Code, string Sort_Step)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<string>("Select Top 1 Etc From Khma_Sort_Note Where Sort_" + Sort_Step + "_Code = @Sort_Code Order by Tid Desc", new { Sort_Code, Sort_Step }, commandType: CommandType.Text);
            }
            //return dn.ctx.Query<string>("Select Top 1 Etc From Khma_Sort_Note Where Sort_" + Sort_Step + "_Code = @Sort_Code Order by Tid Desc", new { Sort_Code, Sort_Step }).SingleOrDefault();
        }

        /// <summary>
        /// 게시판 분류 목록
        /// </summary>
        /// <param name="Sort_Code">목록 분류 코드</param>
        /// <returns>목록</returns>
        public async Task<List<Sort_Note_Entity>> GetList_Sort(string Sort_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var ss = await ctx.QueryAsync<Sort_Note_Entity>("Select * From Khma_Sort_Note Where Sort_Code = @Sort_Code Order By Tid Desc", new { Sort_Code }, commandType: CommandType.Text);
                return ss.ToList();
            }
            //return dn.ctx.Query<Sort_Note_Entity>("Select * From Khma_Sort_Note Where Sort_Code = @Sort_Code Order By Tid Desc", new { Sort_Code }).ToList();
        }

        /// <summary>
        /// 분류 단계에 따른 게시판 목록
        /// </summary>
        /// <param name="Sort_Step">목록 분류 코드</param>
        /// <returns>목록</returns>
        public async Task<List<Sort_Note_Entity>> GetList_Step(string Sort_Step)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var ss = await ctx.QueryAsync<Sort_Note_Entity>("Select * From Khma_Sort_Note Where Sort_Step = @Sort_Step Order By Tid Desc", new { Sort_Step }, commandType: CommandType.Text);
                return ss.ToList();
            }
            //return dn.ctx.Query<Sort_Note_Entity>("Select * From Khma_Sort_Note Where Sort_Step = @Sort_Step Order By Tid Desc", new { Sort_Step }).ToList();
        }

        /// <summary>
        /// 필드와 변수로 리스트 만들기
        /// </summary>
        /// <param name="Feild">필드명</param>
        /// <param name="Query">변수</param>
        /// <returns>검색된 리스트 반환</returns>
        public async Task<List<Sort_Note_Entity>> GetList_Code(string Feild, string Query)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var ss = await ctx.QueryAsync<Sort_Note_Entity>("Select * From Khma_Sort_Note Where " + Feild + " = @Query", new { Feild, Query }, commandType: CommandType.Text);
                return ss.ToList();
            }
            //return dn.ctx.Query<Sort_Note_Entity>("Select * From Khma_Sort_Note Where " + Feild + " = @Query", new { Feild, Query }).ToList();
        }

        /// <summary>
        /// 게시판 목록
        /// </summary>
        /// <param name="Sort_Code">목록 분류 코드</param>
        /// <returns>목록</returns>
        public async Task<List<Sort_Note_Entity>> GetList_Index(string Note_Index)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var ss = await ctx.QueryAsync<Sort_Note_Entity>("Select * From Khma_Sort_Note Where Note_Index Like '%" + Note_Index + "%' Order By Tid Desc", new { Note_Index }, commandType: CommandType.Text);
                return ss.ToList();
            }
            //return dn.ctx.Query<Sort_Note_Entity>("Select * From Khma_Sort_Note Where Note_Index Like '%" + Note_Index + "%' Order By Tid Desc", new { Note_Index }).ToList();
        }

        /// <summary>
        /// 게시판 목록(등급, 순서)
        /// </summary>
        /// <param name="Sort_Code">목록 분류 코드</param>
        /// <returns>목록</returns>
        public async Task<List<Sort_Note_Entity>> GetList_Step_Index(string Sort_Step, string Note_Index)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var ss = await ctx.QueryAsync<Sort_Note_Entity>("Select * From Khma_Sort_Note Where Note_Index Like '%" + Note_Index + "%' And Sort_Step = @Sort_Step Order By Tid Desc", new { Sort_Step, Note_Index }, commandType: CommandType.Text);
                return ss.ToList();
            }
            //return dn.ctx.Query<Sort_Note_Entity>("Select * From Khma_Sort_Note Where Note_Index Like '%" + Note_Index + "%' And Sort_Step = @Sort_Step Order By Tid Desc", new { Sort_Step, Note_Index }).ToList();
        }

        /// <summary>
        /// 게시판 목록(등급, 순서)
        /// </summary>
        public async Task<List<Sort_Note_Entity>> GetList_Sort_Index(string Sort_Step, string Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var ss = await ctx.QueryAsync<Sort_Note_Entity>("Select * From Khma_Sort_Note Where Sort_B_Code = @Code And Sort_Step = @Sort_Step Order By Tid Desc", new { Sort_Step, Code }, commandType: CommandType.Text);
                return ss.ToList();
            }
            //return dn.ctx.Query<Sort_Note_Entity>("Select * From Khma_Sort_Note Where Sort_B_Code = @Code And Sort_Step = @Sort_Step Order By Tid Desc", new { Sort_Step, Code }).ToList();
        }

        /// <summary>
        /// 같은 등급에 입력 수 구하기
        /// </summary>
        /// <param name="Sort_Step">등급</param>
        /// <returns>입력된 수 반환</returns>
        public async Task<int> Step_Count(string Sort_Step)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Khma_Sort_Note Where Sort_Step = @Sort_Step", new { Sort_Step }, commandType: CommandType.Text);
            }
            //return dn.ctx.Query<int>("Select Count(*) From Khma_Sort_Note Where Sort_Step = @Sort_Step", new { Sort_Step }).SingleOrDefault();
        }

        /// <summary>
        /// 같은 식별코드에 입력 수 구하기
        /// </summary>
        /// <param name="Sort_Code">식별코드</param>
        /// <returns>입력된 수 반환</returns>
        public async Task<int> Code_Count(string Feild, string Query)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Khma_Sort_Note Where " + Feild + " = @Query", new { Feild, Query }, commandType: CommandType.Text);
            }
            //return dn.ctx.Query<int>("Select Count(*) From Khma_Sort_Note Where " + Feild + " = @Query", new { Feild, Query }).SingleOrDefault();
        }

        /// <summary>
        /// 게시판 분류 사용여부
        /// </summary>
        /// <param name="Tid">게시판 분류 식별코드</param>
        public async Task Edit_Enable(int Tid, string Sort_Enable)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync("Update Khma_Sort_Note Set Sort_Enable = @Sort_Enable Where Tid = @Tid", new { Tid, Sort_Enable }, commandType: CommandType.Text);
            }
            //dn.ctx.Execute("Update Khma_Sort_Note Set Sort_Enable = @Sort_Enable Where Tid = @Tid", new { Tid, Sort_Enable });
        }

        /// <summary>
        /// 게시판 분류 정보 삭제
        /// </summary>
        /// <param name="Tid">게시판 분류 식별코드</param>
        public async Task Delete(int Tid)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync("Delete Khma_Sort_Note Where Tid = @Tid", new { Tid }, commandType: CommandType.Text);
            }
            //dn.ctx.Execute("Delete Khma_Sort_Note Where Tid = @Tid", new { Tid });
        }

        /// <summary>
        /// 해당 게시판 분류 코드 정보 불러오기
        /// </summary>
        /// <param name="SortFeild"></param>
        /// <param name="Sort_Code"></param>
        /// <returns></returns>
        public async Task<Sort_Note_Entity> Detail(string SortFeild, string Sort_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Sort_Note_Entity>("Select * From Khma_Sort_Note Where " + SortFeild + " = @Sort_Code", new { SortFeild, Sort_Code }, commandType: CommandType.Text);
            }
            //return dn.ctx.Query<Sort_Note_Entity>("Select * From Khma_Sort_Note Where " + SortFeild + " = @Sort_Code", new { SortFeild, Sort_Code }).SingleOrDefault();
        }
    }

    public interface IKhma_Note_Lib
    {
        Task<Khma_Note_Entity> Add(Khma_Note_Entity nor);

        Task<Khma_Note_Entity> Edit(Khma_Note_Entity nor);

        Task Update_Notice(int Aid, int Notice);

        Task Update_Secret(int Aid, string Secret);

        Task delete(int Aid);

        Task<string> Note_Secret(int Aid);

        Task<int> Sort_Code_Count(string Sort_Feild, string Sort_Code);

        Task<int> Notice_Count(string Sort_Code);

        Task<Khma_Note_Entity> Detail(int Aid);

        Task Update_ReadCount(int Aid);

        Task Update_CommentCount(int Aid);

        Task<List<Khma_Note_Entity>> GetList(int Page, string Sort_Feild, string Sort_Code);

        Task<List<Khma_Note_Entity>> GetList_Search(int Page, string Sort_Feild, string Sort_Code, string Search_Feild, string Search_Query);

        Task<int> Sort_Code_Search_Count(string Sort_Feild, string Sort_Code, string Search_Feild, string Search_Query);

        Task<List<Khma_Note_Entity>> GetList_Main(string Sort_Code);

        Task<Khma_Note_Entity> GetList_Notice(string Sort_Code);

        Task<List<Khma_Note_Entity>> GetList_admin(int Count, string Sort_Code, string Sort_Step);

        Task<int> Sort_Count(string Sort_Code);

        Task<int> Note_Count();

        Task<List<Khma_Note_Entity>> GetList_D(string Sort_A_Code, string Sort_B_Code, string Sort_C_Code, string Sort_D_Code);

        Task<List<Khma_Note_Entity>> GetList_E(string Sort_A_Code, string Sort_B_Code, string Sort_C_Code, string Sort_D_Code, string Sort_E_Code);

        Task<List<Khma_Note_Entity>> GetList_F(string Sort_A_Code, string Sort_B_Code, string Sort_C_Code, string Sort_D_Code, string Sort_E_Code, string Sort_F_Code);
    }

    /// <summary>
    /// 게시판 클래스
    /// </summary>
    public class Khma_Note_Lib : IKhma_Note_Lib
    {
        private readonly IConfiguration _db;

        public Khma_Note_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 게시판 본문 입력
        /// </summary>
        /// <param name="nor">게시판 필드</param>
        /// <returns></returns>
        public async Task<Khma_Note_Entity> Add(Khma_Note_Entity nor)
        {
            var sql = "Insert into Khma_Note (Note_Code, Apt_Code, Apt_Name, User_Code, Name, Email, Title, PostIP, Content, Notice, Sort_A_Code, Sort_B_Code, Sort_C_Code, Sort_D_Code, Sort_Code, Sort_Name, Sort_Step, Secret) Values (@Note_Code, @Apt_Code, @Apt_Name, @User_Code, @Name, @Email, @Title, @PostIP, @Content, @Notice, @Sort_A_Code, @Sort_B_Code, @Sort_C_Code, @Sort_D_Code, @Sort_Code, @Sort_Name, @Sort_Step, @Secret)";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, nor, commandType: CommandType.Text);
                return nor;
            }
            //this.aq.ctx.Execute(sql, nor);
            //return nor;
        }

        /// <summary>
        /// 게시판 내용 수정
        /// </summary>
        /// <param name="nor"></param>
        /// <returns></returns>
        public async Task<Khma_Note_Entity> Edit(Khma_Note_Entity nor)
        {
            var sql = "Update Khma_Note Set Title = @Title, ModifyDate = @ModifyDate, ModifyIP = @ModifyIP, Content = @Content, Sort_A_Code = @Sort_A_Code, Sort_B_Code = @Sort_B_Code, Sort_C_Code = @Sort_C_Code, Sort_D_Code = @Sort_D_Code, Sort_Code = @Sort_Code, Sort_Name = @Sort_Name, Notice = @Notice Where Aid = @Aid";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, nor, commandType: CommandType.Text);
                return nor;
            }
            //this.aq.ctx.Execute(sql, nor);
            //return nor;
        }

        /// <summary>
        /// 공지사항 여부 수정
        /// </summary>
        /// <param name="Aid">계시글 식별코드</param>
        /// <param name="Notice">공지 값</param>
        /// 2017
        public async Task Update_Notice(int Aid, int Notice)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync("Update Khma_Note Set Notice = @Notice Where Aid = @Aid", new { Aid, Notice }, commandType: CommandType.Text);
            }
            //aq.ctx.Execute("Update Khma_Note Set Notice = @Notice Where Aid = @Aid", new { Aid, Notice });
        }

        /// <summary>
        /// 비밀글 여부 수정
        /// </summary>
        /// <param name="Aid">계시글 식별코드</param>
        /// <param name="Notice">비밀글</param>
        /// 2017
        public async Task Update_Secret(int Aid, string Secret)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync("Update Khma_Note Set Secret = @Secret Where Aid = @Aid", new { Aid, Secret }, commandType: CommandType.Text);
            }
            //aq.ctx.Execute("Update Khma_Note Set Secret = @Secret Where Aid = @Aid", new { Aid, Secret });
        }

        /// <summary>
        /// 공지사항 삭제
        /// </summary>
        /// <param name="Aid"></param>
        public async Task delete(int Aid)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync("Delete Khma_Note Where Aid = @Aid", new { Aid }, commandType: CommandType.Text);
            }
            //this.aq.ctx.Execute("Delete Khma_Note Where Aid = @Aid", new { Aid });
        }

        /// <summary>
        /// 비밀글 여부 확인
        /// </summary>
        public async Task<string> Note_Secret(int Aid)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<string>("Select Secret From Khma_Note Where Aid = @Aid", new { Aid }, commandType: CommandType.Text);
            }
            //return this.aq.ctx.Query<string>("Select Secret From Khma_Note Where Aid = @Aid", new { Aid }).SingleOrDefault();
        }

        /// <summary>
        /// 해당 분류 식별코드로 입력된 수
        /// </summary>
        /// <param name="Sort_Code">분류 식별 코드</param>
        /// <returns>입력된 수 반환</returns>
        /// 2017
        public async Task<int> Sort_Code_Count(string Sort_Feild, string Sort_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Khma_Note Where " + Sort_Feild + " = @Sort_Code", new { Sort_Feild, Sort_Code }, commandType: CommandType.Text);
            }
            //return aq.ctx.Query<int>("Select Count(*) From Khma_Note Where " + Sort_Feild + " = @Sort_Code", new { Sort_Feild, Sort_Code }).SingleOrDefault();
        }

        /// <summary>
        /// 가장 많은 공지 수 찾기
        /// </summary>
        public async Task<int> Notice_Count(string Sort_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select max(Notice, 0) From Khma_Note Where Sort_Code = @Sort_Code", new { Sort_Code }, commandType: CommandType.Text);
            }
            //return aq.ctx.Query<int>("Select max(Notice, 0) From Khma_Note Where Sort_Code = @Sort_Code", new { Sort_Code }).SingleOrDefault();
        }

        /// <summary>
        /// 게시판 상세보기
        /// </summary>
        /// <param name="Note_Code"></param>
        /// <returns></returns>
        public async Task<Khma_Note_Entity> Detail(int Aid)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Khma_Note_Entity>("Select * From Khma_Note Where Aid = @Aid", new { Aid }, commandType: CommandType.Text);
            }
            //return aq.ctx.Query<Khma_Note_Entity>("Select * From Khma_Note Where Aid = @Aid", new { Aid }).SingleOrDefault();
        }

        /// <summary>
        /// 읽은 수 입력
        /// </summary>
        /// <param name="Aid"></param>
        public async Task Update_ReadCount(int Aid)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync("Update Khma_Note Set ReadCount = ReadCount +1 Where Aid = @Aid", new { Aid }, commandType: CommandType.Text);
                //return Apt;
            }
            //this.aq.ctx.Execute("Update Khma_Note Set ReadCount = ReadCount +1 Where Aid = @Aid", new { Aid });
        }

        /// <summary>
        /// 댓글 수 입력
        /// </summary>
        /// <param name="Aid"></param>
        public async Task Update_CommentCount(int Aid)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync("Update Khma_Note Set CommentCount = CommentCount +1 Where Aid = @Aid", new { Aid }, commandType: CommandType.Text);
                //return Apt;
            }
            //this.aq.ctx.Execute("Update Khma_Note Set CommentCount = CommentCount +1 Where Aid = @Aid", new { Aid });
        }

        /// <summary>
        /// 게시판 목록
        /// </summary>
        /// <param name="Sort_Code">게시판 분류 코드</param>
        /// <returns></returns>
        public async Task<List<Khma_Note_Entity>> GetList(int Page, string Sort_Feild, string Sort_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var Apt = await ctx.QueryAsync<Khma_Note_Entity>("List_Note", new { Page, Sort_Feild, Sort_Code }, commandType: CommandType.StoredProcedure);
                return Apt.ToList();
            }
            //return this.aq.ctx.Query<Khma_Note_Entity>("List_Note", new { Page, Sort_Feild, Sort_Code }, commandType: CommandType.StoredProcedure).ToList();
        }

        /// <summary>
        /// 찾기로 게시판 목록
        /// </summary>
        /// <param name="Sort_Code">게시판 분류 코드</param>
        /// <returns></returns>
        public async Task<List<Khma_Note_Entity>> GetList_Search(int Page, string Sort_Feild, string Sort_Code, string Search_Feild, string Search_Query)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var Apt = await ctx.QueryAsync<Khma_Note_Entity>("List_Note_Search", new { Page, Sort_Feild, Sort_Code }, commandType: CommandType.StoredProcedure);
                return Apt.ToList();
            }
            //return this.aq.ctx.Query<Khma_Note_Entity>("List_Note_Search", new { Page, Sort_Feild, Sort_Code, Search_Feild, Search_Query }, commandType: CommandType.StoredProcedure).ToList();
        }

        /// <summary>
        /// 찾기로 카운트 수
        /// </summary>
        /// <param name="Sort_Feild"></param>
        /// <param name="Sort_Code"></param>
        /// <returns></returns>
        public async Task<int> Sort_Code_Search_Count(string Sort_Feild, string Sort_Code, string Search_Feild, string Search_Query)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("List_Note_Search_Count", new { Sort_Feild, Sort_Code, Search_Feild, Search_Query }, commandType: CommandType.StoredProcedure);
            }
            //return aq.ctx.Query<int>("List_Note_Search_Count", new { Sort_Feild, Sort_Code, Search_Feild, Search_Query }, commandType: CommandType.StoredProcedure).SingleOrDefault();
        }

        /// <summary>
        /// 메인 게시판 목록
        /// </summary>
        /// <param name="Sort_Code">게시판 분류 코드</param>
        /// <returns></returns>
        public async Task<List<Khma_Note_Entity>> GetList_Main(string Sort_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var Apt = await ctx.QueryAsync<Khma_Note_Entity>("Select Top 5 * From Khma_Note Where Sort_Code = @Sort_Code Order by Aid Desc", new { Sort_Code });
                return Apt.ToList();
            }
            //return this.aq.ctx.Query<Khma_Note_Entity>("Select Top 5 * From Khma_Note Where Sort_Code = @Sort_Code Order by Aid Desc", new { Sort_Code }).ToList();
        }

        public async Task<Khma_Note_Entity> GetList_Notice(string Sort_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Khma_Note_Entity>("Select Top 1 * From Khma_Note Where Sort_Code = @Sort_Code Order by Notice Desc, Aid Desc", new { Sort_Code }, commandType: CommandType.Text);
            }
            //return this.aq.ctx.Query<Khma_Note_Entity>("Select Top 1 * From Khma_Note Where Sort_Code = @Sort_Code Order by Notice Desc, Aid Desc", new { Sort_Code }).SingleOrDefault();
        }

        /// <summary>
        /// 관리자 공지사항 입력 목록 만들기
        /// </summary>
        /// <param name="Count">표시될 게시판 숫자</param>
        /// <param name="Sort_Code">분류 식별코드</param>
        /// <param name="Sort_Step">분류단계</param>
        /// <returns>분류별 게시판 목록 반환</returns>
        public async Task<List<Khma_Note_Entity>> GetList_admin(int Count, string Sort_Code, string Sort_Step)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aaa = await ctx.QueryAsync<Khma_Note_Entity>("Select Top " + Count + " * From Khma_Note Where Sort_Code = @Sort_Code and Sort_Step = @Sort_Step Order by Aid Desc", new { Count, Sort_Code, Sort_Step }, commandType: CommandType.Text);
                return aaa.ToList();
            }
            //return this.aq.ctx.Query<Khma_Note_Entity>("Select Top " + Count + " * From Khma_Note Where Sort_Code = @Sort_Code and Sort_Step = @Sort_Step Order by Aid Desc", new { Count, Sort_Code, Sort_Step }).ToList();
        }

        /// <summary>
        /// 분류 식별코드로 입력된 수
        /// </summary>
        /// <param name="Sort_Code">분류 식별코드</param>
        /// <returns>입력된 수</returns>
        public async Task<int> Sort_Count(string Sort_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Khma_Note Where Sort_Code = @Sort_Code", new { Sort_Code }, commandType: CommandType.Text);
            }
            //return this.aq.ctx.Query<int>("Select Count(*) From Khma_Note Where Sort_Code = @Sort_Code", new { Sort_Code }).SingleOrDefault();
        }

        /// <summary>
        /// 분류 식별코드로 입력된 수
        /// </summary>
        public async Task<int> Note_Count()
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Khma_Note", commandType: CommandType.Text);
            }
            //return this.aq.ctx.Query<int>("Select Count(*) From Khma_Note", new { }).SingleOrDefault();
        }

        /// <summary>
        /// 단위 상위 분류 목록
        /// </summary>
        /// <param name="Sort_D_Code"></param>
        /// <returns></returns>
        public async Task<List<Khma_Note_Entity>> GetList_D(string Sort_A_Code, string Sort_B_Code, string Sort_C_Code, string Sort_D_Code)
        {
            var sql = "Select * From Khma_Note Where Sort_A_Code = @Sort_A_Code And Sort_B_Code = @Sort_B_Code And Sort_C_Code = @Sort_C_Code And Sort_D_Code = @Sort_D_Code Order By Aid Desc";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aaa = await ctx.QueryAsync<Khma_Note_Entity>(sql, new { Sort_A_Code, Sort_B_Code, Sort_C_Code, Sort_D_Code }, commandType: CommandType.Text);
                return aaa.ToList();
            }
            //return this.aq.ctx.Query<Khma_Note_Entity>(sql, new { Sort_A_Code, Sort_B_Code, Sort_C_Code, Sort_D_Code }).ToList();
        }

        /// <summary>
        /// 단위 중위 분류 목록
        /// </summary>
        /// <param name="Sort_E_Code"></param>
        /// <returns></returns>
        public async Task<List<Khma_Note_Entity>> GetList_E(string Sort_A_Code, string Sort_B_Code, string Sort_C_Code, string Sort_D_Code, string Sort_E_Code)
        {
            var sql = "Select * From Khma_Note Where Sort_A_Code = @Sort_A_Code And Sort_B_Code = @Sort_B_Code And Sort_C_Code = @Sort_C_Code And Sort_D_Code = @Sort_D_Code And Sort_E_Code = @Sort_E_Code Order By Aid Desc";

            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aaa = await ctx.QueryAsync<Khma_Note_Entity>(sql, new { Sort_A_Code, Sort_B_Code, Sort_C_Code, Sort_D_Code, Sort_E_Code }, commandType: CommandType.Text);
                return aaa.ToList();
            }
            //return this.aq.ctx.Query<Khma_Note_Entity>(sql, new { Sort_A_Code, Sort_B_Code, Sort_C_Code, Sort_D_Code, Sort_E_Code, }).ToList();
        }

        /// <summary>
        /// 단위 하위 분류 목록
        /// </summary>
        /// <param name="Sort_F_Code"></param>
        /// <returns></returns>
        public async Task<List<Khma_Note_Entity>> GetList_F(string Sort_A_Code, string Sort_B_Code, string Sort_C_Code, string Sort_D_Code, string Sort_E_Code, string Sort_F_Code)
        {
            var sql = "Select * From Khma_Note Where Sort_A_Code = @Sort_A_Code And Sort_B_Code = @Sort_B_Code And Sort_C_Code = @Sort_C_Code And Sort_D_Code = @Sort_D_Code And Sort_E_Code = @Sort_E_Code And Sort_F_Code = @Sort_F_Code Order By Aid Desc";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aaa = await ctx.QueryAsync<Khma_Note_Entity>(sql, new { Sort_A_Code, Sort_B_Code, Sort_C_Code, Sort_D_Code, Sort_E_Code, Sort_F_Code }, commandType: CommandType.Text);
                return aaa.ToList();
            }
            //return this.aq.ctx.Query<Khma_Note_Entity>(sql, new { Sort_A_Code, Sort_B_Code, Sort_C_Code, Sort_D_Code, Sort_E_Code, Sort_F_Code }).ToList();
        }
    }

    /// <summary>
    /// 게시판 댓글 인터페이스
    /// </summary>
    public interface IKhma_NoteComments_Lib
    {
        Task<Khma_NoteComments_Entity> add(Khma_NoteComments_Entity ncn);

        Task<Khma_NoteComments_Entity> edit(Khma_NoteComments_Entity ncn);

        Task<List<Khma_NoteComments_Entity>> getlist(int Page, string BoardAid);

        Task<int> Comment_count(string BoardAid);

        Task<int> Comment_Being(string Num);

        Task delete(int Num, int Aid);
    }

    /// <summary>
    /// 게시판 댓글 클래스
    /// </summary>
    public class Khma_NoteComments_Lib : IKhma_NoteComments_Lib
    {
        private readonly IConfiguration _db;

        public Khma_NoteComments_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 댓글 입력
        /// </summary>
        /// <param name="ncn">댓글 필드</param>
        public async Task<Khma_NoteComments_Entity> add(Khma_NoteComments_Entity ncn)
        {
            var sql = "Insert Into Khma_NoteComments (BoardName, BoardAid, User_Code, Name, Opinion, PostIP, Secret) Values (@BoardName, @BoardAid, @User_Code, @Name, @Opinion, @PostIP, @Secret)";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, ncn, commandType: CommandType.Text);
                return ncn;
            }
            //nb.ctx.Execute(sql, ncn);
            //return ncn;
        }

        /// <summary>
        /// 댓글 수정
        /// </summary>
        /// <param name="ncn"></param>
        public async Task<Khma_NoteComments_Entity> edit(Khma_NoteComments_Entity ncn)
        {
            var sql = "Update Khma_NoteComments Set Opinion = @Opinion, Secret = @Secret, ModifyDate = @ModifyDate, ModifyIP = @ModifyIP Where Num = @Num";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, ncn, commandType: CommandType.Text);
                return ncn;
            }
            //nb.ctx.Execute(sql, ncn);
            //return ncn;
        }

        /// <summary>
        /// 댓글 리스트
        /// </summary>
        /// <param name="BoardAid">부모글 식별코드</param>
        /// <returns>댓글 리스트</returns>
        public async Task<List<Khma_NoteComments_Entity>> getlist(int Page, string BoardAid)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aa = await ctx.QueryAsync<Khma_NoteComments_Entity>("List_Note_Comement", new { Page, BoardAid }, commandType: CommandType.StoredProcedure);
                return aa.ToList();
            }
            //return nb.ctx.Query<Khma_NoteComments_Entity>("List_Note_Comement", new { Page, BoardAid }, commandType: CommandType.StoredProcedure).ToList();
        }

        /// <summary>
        /// 댓글 수
        /// </summary>
        /// <param name="BoardAid"></param>
        /// <returns></returns>
        public async Task<int> Comment_count(string BoardAid)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Khma_NoteComments Where BoardAid = @BoardAid", new { BoardAid }, commandType: CommandType.Text);
            }
            //return nb.ctx.Query<int>("Select Count(*) From Khma_NoteComments Where BoardAid = @BoardAid", new { BoardAid }).SingleOrDefault();
        }

        /// <summary>
        /// 입력 여부 확인
        /// </summary>
        /// <param name="Num"></param>
        /// <returns>존재하면 1을 반환</returns>
        public async Task<int> Comment_Being(string Num)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Khma_NoteComments Where Num = @Num", new { Num }, commandType: CommandType.Text);
            }
            //return nb.ctx.Query<int>("Select Count(*) From Khma_NoteComments Where Num = @Num", new { Num }).SingleOrDefault();
        }

        /// <summary>
        /// 댓글 삭제
        /// </summary>
        /// <param name="Num"></param>
        public async Task delete(int Num, int Aid)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync("Delete Khma_NoteComments Where Num = @Num", new { Num }, commandType: CommandType.Text);
                await ctx.ExecuteAsync("Update Khma_Note Set CommentCount = CommentCount - 1 Where Aid = @Aid", new { Aid }, commandType: CommandType.Text);
                //nb.ctx.Execute("Delete Khma_NoteComments Where Num = @Num", new { Num });
                //nb.ctx.Execute("Update Khma_Note Set CommentCount = CommentCount - 1 Where Aid = @Aid", new { Aid });
            }
        }
    }
}