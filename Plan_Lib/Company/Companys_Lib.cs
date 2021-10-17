using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;

using System.Linq;
using System.Threading.Tasks;

namespace Plan_Lib.Company
{
    /// <summary>
    /// 업체분류
    /// </summary>
    public class CompanySort_Lib : ICompanySort_Lib
    {
        private readonly IConfiguration _db;

        public CompanySort_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        //업체 분류 입력하기
        public async Task<Company_Sort_Entity> Add_CompanySort(Company_Sort_Entity Company)
        {
            var sql = "Insert Company_Sort (Company_Sort_Code, Apt_Code, Staff_Code, Company_Sort_Name, Up_Code, Company_Sort_Step, Company_Division, Company_Etc) Values (@Company_Sort_Code, @Apt_Code, @Staff_Code, @Company_Sort_Name, @Up_Code, @Company_Sort_Step, @Company_Division, @Company_Etc);";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, Company, commandType: CommandType.Text);
                return Company;
            }

            //this.ctx.Execute(sql, Company);
            //return Company;
        }

        //시설물분류 수정하기
        public async Task<Company_Sort_Entity> Edit_CompanySort(Company_Sort_Entity Sort)
        {
            var sql = "Update Company_Sort Set Company_Sort_Name = @Compan_Sort_Name, Up_Code = @Up_Code, Company_Sort_Step = @Company_Sort_Step, Company_Division = @Company_Division, Company_Etc = @Company_Etc Where Aid = @Aid;";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, Sort, commandType: CommandType.Text);
                return Sort;
            }

            //this.ctx.Execute(sql, Sort);
            //return Sort;
        }

        //시설물분류 마지막 일련번호 얻기
        public async Task<int> Last_Number()
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Top 1 Aid From Company_Sort Order by Aid Desc", commandType: CommandType.Text);
            }
            //return this.ctx.Query<int>("Select Top 1 Aid From Company_Sort Order by Aid Desc", new { }).SingleOrDefault();
        }

        // 업체 분류 ㅡ> 분류 리스트
        public async Task<List<Company_Sort_Entity>> GetList_CompanySort(string Up_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var apt = await ctx.QueryAsync<Company_Sort_Entity>("Select * From Company_Sort Where Up_Code = @Up_Code Order By Aid Asc", new { Up_Code }, commandType: CommandType.Text);
                return apt.ToList();
            }
            //return this.ctx.Query<Company_Sort_Entity>("Select * From Company_Sort Where Up_Code = @Up_Code Order By Aid Asc", new { Up_Code }).ToList();
        }

        /// <summary>
        /// 상위분류 드롭다운 리스트 만들기
        /// </summary>
        /// <param name="Up_Code"></param>
        /// <returns></returns>
        public async Task<List<Company_Sort_Entity>> GetList_CompanySort_All()
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var apt = await ctx.QueryAsync<Company_Sort_Entity>("Select * From Company_Sort Order By Aid Asc", commandType: CommandType.Text);
                return apt.ToList();
            }
            //return this.ctx.Query<Company_Sort_Entity>("Select * From Company_Sort Order By Aid Asc", new { }).ToList();
        }

        /// <summary>
        /// 분류 단계로 정보 불러오기
        /// </summary>
        /// <param name="Company_Sort_Step"></param>
        /// <returns></returns>
        public async Task<List<Company_Sort_Entity>> GetList_CompanySort_step(string Company_Sort_Step)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var apt = await ctx.QueryAsync<Company_Sort_Entity>("Select * From Company_Sort Where Company_Sort_Step = @Company_Sort_Step Order By Up_Code Asc, Aid Asc", new { Company_Sort_Step }, commandType: CommandType.Text);
                return apt.ToList();
            }
            //return this.ctx.Query<Company_Sort_Entity>("Select * From Company_Sort Where Company_Sort_Step = @Company_Sort_Step Order By Up_Code Asc, Aid Asc", new { Company_Sort_Step }).ToList();
        }

        /// <summary>
        /// 업체분류 코드 분류명 불러오기
        /// </summary>
        public async Task<string> DetailName_CompanySort(string Company_Sort_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<string>("Select Company_Sort_Name From Company_Sort Where Company_Sort_Code = @Company_Sort_Code", new { Company_Sort_Code }, commandType: CommandType.Text);
            }
            //return this.ctx.Query<string>("Select Company_Sort_Name From Company_Sort Where Company_Sort_Code = @Company_Sort_Code", new { Company_Sort_Code }).SingleOrDefault();
        }

        /// <summary>
        /// 해당 업체분류가 있는 확인
        /// </summary>
        /// <param name="Company_Sort_Code"></param>
        /// <returns></returns>
        public async Task<int> CompanySort_Being(string Company_Sort_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Company_Sort Where Company_Sort_Code = @Company_Sort_Code", new { Company_Sort_Code }, commandType: CommandType.Text);
            }
            //return this.ctx.Query<int>("Select Count(*) From Company_Sort Where Company_Sort_Code = @Company_Sort_Code", new { Company_Sort_Code }).SingleOrDefault();
        }

        // 업체분류 코드 상위분류코드 불러오기
        public async Task<string> DetailCode_CompnaySort(string Company_Sort_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<string>("Select Up_Code From Company_Sort Where Company_Sort_Code = @Company_Sort_Code", new { Company_Sort_Code }, commandType: CommandType.Text);
            }
            //return this.ctx.Query<string>("Select Up_Code From Company_Sort Where Company_Sort_Code = @Company_Sort_Code", new { Company_Sort_Code }).SingleOrDefault();
        }

        /// <summary>
        /// 업체분류 상세 불러오기
        /// </summary>
        public async Task<Company_Sort_Entity> Detail_CompanySort_Detail(string Company_Sort_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Company_Sort_Entity>("Select * From Company_Sort Where Company_Sort_Code = @Company_Sort_Code", new { Company_Sort_Code }, commandType: CommandType.Text);
            }
            //return this.ctx.Query<Company_Sort_Entity>("Select * From Company_Sort Where Company_Sort_Code = @Company_Sort_Code", new { Company_Sort_Code }).SingleOrDefault();
        }

        /// <summary>
        /// 업체분류 삭제
        /// </summary>
        /// <param name="Aid"></param>
        public async Task Remove(int Aid)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync("Delete From Company_Sort Where Aid = @Aid", new { Aid }, commandType: CommandType.Text);
            }
            //this.ctx.Execute("Delete From Company_Sort Where Aid = @Aid", new { Aid });
        }
    }

    /// <summary>
    /// 업체정보
    /// </summary>
    public class Company_Lib : ICompany_Lib
    {
        private readonly IConfiguration _db;

        public Company_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        //업체 입력하기
        public async Task<Company_Entity> Add_Company(Company_Entity Company)
        {
            var sql = "Insert Company (Apt_Code, Company_Code, Staff_Code, SortA_Code, SortB_Code, SortA_Name, SortB_Name, Company_Name, CorporRate_Number, Company_Etc, PostIP) Values (@Apt_Code, @Company_Code, @Staff_Code, @SortA_Code, @SortB_Code, @SortA_Name, @SortB_Name, @Company_Name, @CorporRate_Number, @Company_Etc, @PostIP);";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, Company, commandType: CommandType.Text);
                return Company;
            }

            //this.ctx.Execute(sql, Company);
            //return Company;
        }

        // 업체 정보 수정하기
        public async Task<Company_Entity> Edit_Company(Company_Entity Sort)
        {
            var sql = "Update Company_Sort Set SortA_Code = @SortA_Code, SortB_Code = @SortB_Code,  SortA_Name = @SortA_Name, SortB_Name = @SortB_Name, Company_Name = @Company_Name, CorporRate_Number = @CorporRate_Number, Company_Etc = @Company_Etc Where Aid = @Aid;";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, Sort);
                return Sort;
            }

            //this.ctx.Execute(sql, Sort);
            //return Sort;
        }

        // 업체정보 마지막 일련번호 얻기
        public async Task<int> Last_Number()
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Top 1 Aid From Company Order by Aid Desc", commandType: CommandType.Text);
            }
            //return this.ctx.Query<int>("Select Top 1 Aid From Company Order by Aid Desc", new { }).SingleOrDefault();
        }

        /// <summary>
        /// 입력된 전체 업체 수
        /// </summary>
        /// <returns></returns>
        public async Task<int> totalCount()
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Company", commandType: CommandType.Text);
            }
            //return ctx.Query<int>("Select Count(*) From Company").SingleOrDefault();
        }

        // 사업자번호 중복검색
        public async Task<int> CorporRate_Number(string CorporRate_Number)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Company Where CorporRate_Number = @CorporRate_Number", new { CorporRate_Number }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 업체 상세 정보(사업번호)
        /// </summary>
        public async Task<Company_Entity> ByDetails_Company(string CorporRate_Number)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Company_Entity>("Select Aid, Apt_Code, Staff_Code, SortA_Code, SortB_Code, SortA_Name, SortB_Name, Company_Code, Company_Name, CorporRate_Number, Company_Etc, PostDate, PostIP From Company Where CorporRate_Number = @CorporRate_Number Order By Aid Asc", new { CorporRate_Number });                
            }
        }

        /// <summary>
        /// 업체 정보 리스트
        /// </summary>
        public async Task<List<Company_Entity>> GetList_Company(string SortA_Code, string SortB_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var apt = await ctx.QueryAsync<Company_Entity>("Select * From Company Where SortA_Code = @SortA_Code And SortB_Code = @SortB_Code Order By Aid Asc", new { SortA_Code, SortB_Code }, commandType: CommandType.Text);
                return apt.ToList();
            }
        }

        /// <summary>
        /// 업체 정보 리스트(all)
        /// </summary>
        public async Task<List<Company_Entity>> List_Company()
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var apt = await ctx.QueryAsync<Company_Entity>("Select * From Company Order By Aid Desc", commandType: CommandType.Text);
                return apt.ToList();
            }
            //return this.ctx.Query<Company_Entity>("Select * From Company Order By Aid Desc", new { }).ToList();
        }

        /// <summary>
        /// 업체 및 상세 정보 리스트(all)
        /// </summary>
        public async Task<List<Company_Entity_Etc>> List_Detail_Company()
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var apt = await ctx.QueryAsync<Company_Entity_Etc>("Select a.Aid, a.Apt_Code, a.Company_Code, a.Company_Etc, a.Company_Name, a.CorporRate_Number, a.SortA_Code, a.PostDate, a.SortA_Name, a.SortB_Code, a.SortB_Name, b.Capital, b.Ceo_Mobile, b.Ceo_Name, b.Charge_Man, b.ChargeMan_Mobile, b.CompanyEtc_Code, b.Cor_Adress, b.Cor_Email, b.Cor_Etc, b.Cor_Fax, b.Cor_Gun, b.Cor_Sido, b.Cor_Tel, b.Corporation, b.Credit_Rate, b.Staff_Code From Company a Join Company_Etc b on a.Company_Code = b.Company_Code Order By a.Aid Desc", commandType: CommandType.Text);
                return apt.ToList();
            }            
        }


        /// <summary>
        /// 업체 및 상세 정보 리스트(all)
        /// </summary>
        public async Task<List<Company_Entity_Etc>> List_Page_Company(int Page)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var apt = await ctx.QueryAsync<Company_Entity_Etc>("Select Top 15 a.Aid, a.Apt_Code, a.Company_Code, a.Company_Etc, a.Company_Name, a.CorporRate_Number, a.SortA_Code, a.PostDate, a.SortA_Name, a.SortB_Code, a.SortB_Name, b.Capital, b.Ceo_Mobile, b.Ceo_Name, b.Charge_Man, b.ChargeMan_Mobile, b.CompanyEtc_Code, b.Cor_Adress, b.Cor_Email, b.Cor_Etc, b.Cor_Fax, b.Cor_Gun, b.Cor_Sido, b.Cor_Tel, b.Corporation, b.Credit_Rate, b.Staff_Code From Company a Join Company_Etc b on a.Company_Code = b.Company_Code Where a.Aid Not In (Select Top (15 * @Page) a.Aid From Company a Join Company_Etc b on a.Company_Code = b.Company_Code Order By a.Aid Desc) Order By a.Aid Desc", new { Page });
                return apt.ToList();
            }
        }

        /// <summary>
        /// 업체 및 상세 정보 리스트(all) 수
        /// </summary>
        public async Task<int> List_Page_Company_Count()
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Company a Join Company_Etc b on a.Company_Code = b.Company_Code");                
            }
        }

        /// <summary>
        /// 업체 및 상세 정보 리스트(all) 검색 목록
        /// </summary>
        public async Task<List<Company_Entity_Etc>> List_Page_Company_Search(int Page, string Field, string Query)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var sql = "Select Top 15 a.Aid, a.Apt_Code, a.Company_Code, a.Company_Etc, a.Company_Name, a.CorporRate_Number, a.SortA_Code, a.PostDate, a.SortA_Name, a.SortB_Code, a.SortB_Name, b.Capital, b.Ceo_Mobile, b.Ceo_Name, b.Charge_Man, b.ChargeMan_Mobile, b.CompanyEtc_Code, b.Cor_Adress, b.Cor_Email, b.Cor_Etc, b.Cor_Fax, b.Cor_Gun, b.Cor_Sido, b.Cor_Tel, b.Corporation, b.Credit_Rate, b.Staff_Code From Company a Join Company_Etc b on a.Company_Code = b.Company_Code Where a.Aid Not In (Select Top (15 * " + Page + ") a.Aid From Company a Join Company_Etc b on a.Company_Code = b.Company_Code And " + Field + " Like '%" + Query + "%' Order By a.Aid Desc) And a." + Field + " Like '%" + Query + "%' Order By a.Aid Desc";
                var apt = await ctx.QueryAsync<Company_Entity_Etc>(sql, new { Page, Field, Query });
                return apt.ToList();
            }
        }

        /// <summary>
        /// 업체 및 상세 정보 리스트(all) 검색된 수
        /// </summary>
        public async Task<int> List_Page_Company_Count_Search(string Field, string Query)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Company a Join Company_Etc b on a.Company_Code = b.Company_Code Where @Field Like '%" + Query + "%'", new { Field, Query });
            }
        }

        /// <summary>
        /// 업체명으로 검색된 목록 정보
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public async Task<List<Company_Entity_Etc>> Search_Name_List(string Name)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var apt = await ctx.QueryAsync<Company_Entity_Etc>("Select a.Aid, a.Apt_Code, a.Company_Code, a.Company_Etc, a.Company_Name, a.CorporRate_Number, a.SortA_Code, a.PostDate, a.SortA_Name, a.SortB_Code, a.SortB_Name, b.Capital, b.Ceo_Mobile, b.Ceo_Name, b.Charge_Man, b.ChargeMan_Mobile, b.CompanyEtc_Code, b.Cor_Adress, b.Cor_Email, b.Cor_Etc, b.Cor_Fax, b.Cor_Gun, b.Cor_Sido, b.Cor_Tel, b.Corporation, b.Credit_Rate, b.Staff_Code From Company a Join Company_Etc b on a.Company_Code = b.Company_Code Where a.Company_Name Like '%" + Name + "%' Order By a.Aid Desc", new { Name }, commandType: CommandType.Text);
                return apt.ToList();
            }
            
        }

        /// <summary>
        /// 사업자번호로 검색된 목록 정보
        /// </summary>
        /// <param name="CorNum"></param>
        /// <returns></returns>
        public async Task<List<Company_Entity_Etc>> Search_Cor_Num_List(string CorNum)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var apt = await ctx.QueryAsync<Company_Entity_Etc>("Select a.Aid, a.Apt_Code, a.Company_Code, a.Company_Etc, a.Company_Name, a.CorporRate_Number, a.SortA_Code, a.PostDate, a.SortA_Name, a.SortB_Code, a.SortB_Name, b.Capital, b.Ceo_Mobile, b.Ceo_Name, b.Charge_Man, b.ChargeMan_Mobile, b.CompanyEtc_Code, b.Cor_Adress, b.Cor_Email, b.Cor_Etc, b.Cor_Fax, b.Cor_Gun, b.Cor_Sido, b.Cor_Tel, b.Corporation, b.Credit_Rate, b.Staff_Code From Company a Join Company_Etc b on a.Company_Code = b.Company_Code Where a.CorporRate_Number = @CorNum Order By a.Aid Desc", new { CorNum }, commandType: CommandType.Text);
                return apt.ToList();
            }
            //return this.ctx.Query<Company_Entity_Etc>("Select a.Aid, a.Apt_Code, a.Company_Code, a.Company_Etc, a.Company_Name, a.CorporRate_Number, a.SortA_Code, a.PostDate, a.SortA_Name, a.SortB_Code, a.SortB_Name, b.Capital, b.Ceo_Mobile, b.Ceo_Name, b.Charge_Man, b.ChargeMan_Mobile, b.CompanyEtc_Code, b.Cor_Adress, b.Cor_Email, b.Cor_Etc, b.Cor_Fax, b.Cor_Gun, b.Cor_Sido, b.Cor_Tel, b.Corporation, b.Credit_Rate, b.Staff_Code From Company a Join Company_Etc b on a.Company_Code = b.Company_Code Where a.CorporRate_Number = @CorNum Order By a.Aid Desc", new { CorNum }).ToList();
        }

        /// <summary>
        ///  업체코드로 업체명 불러오기
        /// </summary>
        public async Task<string> Detail_Company_Name(string Company_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<string>("Select Company_Name From Company Where Company_Code = @Company_Code", new { Company_Code }, commandType: CommandType.Text);
            }
            //return this.ctx.Query<string>("Select Company_Name From Company Where Company_Code = @Company_Code", new { Company_Code }).SingleOrDefault();
        }

        // 업체코드로 사업자번호 불러오기
        public async Task<string> Detail_Company_CorNum(string Company_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<string>("Select CorporRate_Number From Company Where Company_Code = @Company_Code", new { Company_Code }, commandType: CommandType.Text);
            }
            //return this.ctx.Query<string>("Select CorporRate_Number From Company Where Company_Code = @Company_Code", new { Company_Code }).SingleOrDefault();
        }

        // 업체코드로 사업자번호 있는지 확인
        public async Task<int> Detail_Company_CorNum_Number(string Company_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Company Where Company_Code = @Company_Code", new { Company_Code }, commandType: CommandType.Text);
            }
            //return this.ctx.Query<int>("Select Count(*) From Company Where Company_Code = @Company_Code", new { Company_Code }).SingleOrDefault();
        }

        /// <summary>
        ///  업체정보 상세 불러오기
        /// </summary>
        public async Task<Company_Entity> Detail_Company_Detail(string Company_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Company_Entity>("Select * From Company Where Company_Code = @Company_Code", new { Company_Code }, commandType: CommandType.Text);
            }
            //return this.ctx.Query<Company_Entity>("Select  * From Company Where Company_Code = @Company_Code", new { Company_Code }).SingleOrDefault();
        }

        /// <summary>
        /// 사업자 등록 번호로 업체 기본 및 상세 정보 불러오기
        /// </summary>
        /// <param name="CorporRate_Number"></param>
        /// <returns></returns>
        public async Task<Company_Entity_Etc> Company_View(string CorporRate_Number)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Company_Entity_Etc>("Select Top 1 * From Company a Join Company_Etc b on a.Company_Code = b.Company_Code Where a.CorporRate_Number = @CorporRate_Number Order By a.Aid Desc", new { CorporRate_Number }, commandType: CommandType.Text);
            }
            //return this.ctx.Query<Company_Entity_Etc>("Select Top 1 * From Company a Join Company_Etc b on a.Company_Code = b.Company_Code Where a.CorporRate_Number = @CorporRate_Number Order By a.Aid Desc", new { CorporRate_Number }).SingleOrDefault();
        }

        /// <summary>
        /// 업체 정보 삭제
        /// </summary>
        public async Task ByDelete_Company(int Aid)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            await db.ExecuteAsync("Delete Company Where Aid = @Aid", new { Aid });
        }
    }

    /// <summary>
    /// 업체상세정보
    /// </summary>
    public class Company_Etc_Lib : ICompany_Etc_Lib
    {
        private readonly IConfiguration _db;

        public Company_Etc_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        // 업체 입력하기
        public async Task<Company_Etc_Entity> Add_CompanyEtc(Company_Etc_Entity Company)
        {
            var sql = "Insert Company_Etc (CompanyEtc_Code, Company_Code, Staff_Code, Corporation, Ceo_Name, Credit_Rate, Capital, Cor_Email, Cor_Sido, Cor_Gun, Cor_Adress, Cor_Tel, Cor_Fax, Ceo_Mobile, Charge_Man, ChargeMan_Mobile, Cor_Etc, PostIP) Values (@CompanyEtc_Code, @Company_Code, @Staff_Code, @Corporation, @Ceo_Name, @Credit_Rate, @Capital, @Cor_Email, @Cor_Sido, @Cor_Gun, @Cor_Adress, @Cor_Tel, @Cor_Fax, @Ceo_Mobile, @Charge_Man, @ChargeMan_Mobile, @Cor_Etc, @PostIP);";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, Company);
                return Company;
            }

            //this.ctx.Execute(sql, Company);
            //return Company;
        }

        // 업체 상세 정보 수정하기
        public async Task<Company_Etc_Entity> Edit_CompanyEtc(Company_Etc_Entity Sort)
        {
            var sql = "Update Company_Etc Set Corporation = @Corporation, Ceo_Name = @Ceo_Name, Credit_Rate = @Credit_Rate, Capital = @Capital, Cor_Email = @Cor_Email, Cor_Sido = @Cor_Sido, Cor_Gun = @Cor_Gun, Cor_Adress = @Cor_Adress, Ceo_Tel = @Ceo_Tel, Ceo_Fax = @Ceo_Fax, Ceo_Mobile = @Ceo_Mobile, Charge_Man = @Charge_Man, ChargeMan_Mobile = @ChargeMan_Mobile, Cor_Etc = @Cor_Etc Where Aid = @Aid;";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, Sort);
                return Sort;
            }

            //this.ctx.Execute(sql, Sort);
            //return Sort;
        }

        // 업체 상세 정보 마지막 일련번호 얻기
        public async Task<int> Last_Number()
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Top 1 Aid From Company_Etc Order by Aid Desc", commandType: CommandType.Text);
            }
            //return this.ctx.Query<int>("Select Top 1 Aid From Company_Etc Order by Aid Desc", new { }).SingleOrDefault();
        }

        // 업체 상세 정보 리스트
        public async Task<List<Company_Etc_Entity>> GetList_CompanyEtc(string Company_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var apt = await ctx.QueryAsync<Company_Etc_Entity>("elect * From Company_Etc Where Company_Code = @Company_Code Order By Aid Asc", new { Company_Code }, commandType: CommandType.Text);
                return apt.ToList();
            }
            //return this.ctx.Query<Company_Etc_Entity>("Select * From Company_Etc Where Company_Code = @Company_Code Order By Aid Asc", new { Company_Code }).ToList();
        }

        //// 업체상세정보 상세 불러오기
        public async Task<Company_Entity> Detail_Company_Detail(string CompanyEtc_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Company_Entity>("Select  * From Company_Etc Where Company_Code = @Company_Code", new { CompanyEtc_Code }, commandType: CommandType.Text);
            }
            //return this.ctx.Query<Company_Entity>("Select  * From Company_Etc Where Company_Code = @Company_Code", new { CompanyEtc_Code }).SingleOrDefault();
        }

        /// <summary>
        /// 업체 와 상세정보 조인 불러오기
        /// </summary>
        public async Task<Company_Entity_Etc> Detail_Company_Etc_Detail(string Company_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Company_Entity_Etc>("Select  * From Company, Company_Etc Where Company.Company_Code = Company_Etc.Company_Code and Company.Company_Code = @Company_Code", new { Company_Code }, commandType: CommandType.Text);
            }
            //return this.ctx.Query<Company_Entity_Etc>("Select  * From Company, Company_Etc Where Company.Company_Code = Company_Etc.Company_Code and Company.Company_Code = @Company_Code", new { Company_Code }).SingleOrDefault();
        }

        /// <summary>
        /// 업체 와 상세정보 조인 불러오기(사업자등록번호)
        /// </summary>
        public async Task<Company_Entity_Etc> Detail_Company_Etc_Num(string CorporRate_Number)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Company_Entity_Etc>("Select  * From Company, Company_Etc Where Company.Company_Code = Company_Etc.Company_Code and Company.CorporRate_Number = @CorporRate_Number", new { CorporRate_Number }, commandType: CommandType.Text);
            }
            //return this.ctx.Query<Company_Entity_Etc>("Select  * From Company, Company_Etc Where Company.Company_Code = Company_Etc.Company_Code and Company.CorporRate_Number = @CorporRate_Number", new { CorporRate_Number }).SingleOrDefault();
        }
    }

    /// <summary>
    /// 업종 정보
    /// </summary>
    public class BusinessType_Lib : IBusinessType_Lib
    {
        private readonly IConfiguration _db;

        public BusinessType_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        // 업종 입력하기
        public async Task<BusinessType_Entity> Add_BusinessType(BusinessType_Entity BusiType)
        {
            var sql = "Insert BusinessType (BusinessType_Code, BusinessType_Name, Staff_Code, BusinessType_Etc, PostIP) Values (@BusinessType_Code, @BusinessType_Name, @Staff_Code, @BusinessType_Etc, @PostIP);";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, BusiType, commandType: CommandType.Text);
                return BusiType;
            }

            //this.ctx.Execute(sql, BusiType);
            //return BusiType;
        }

        // 업종 정보 수정하기
        public async Task<BusinessType_Entity> Edit_BusinessType(BusinessType_Entity Sort)
        {
            var sql = "Update BusinessType Set BusinessType_Name = @BusinessType_Name, BusinessType_Etc = @BusinessType_Etc Where Aid = @Aid;";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, Sort, commandType: CommandType.Text);
                return Sort;
            }

            //this.ctx.Execute(sql, Sort);
            //return Sort;
        }

        // 업종정보 마지막 일련번호 얻기
        public async Task<int> Last_Number()
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Top 1 Aid From BusinessType Order by Aid Desc", commandType: CommandType.Text);
            }
            //return this.ctx.Query<int>("Select Top 1 Aid From BusinessType Order by Aid Desc", new { }).SingleOrDefault();
        }

        // 업종 정보 리스트
        public async Task<List<BusinessType_Entity>> GetList_BusinessType()
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var apt = await ctx.QueryAsync<BusinessType_Entity>("Select * From BusinessType Order By Aid As", commandType: CommandType.Text);
                return apt.ToList();
            }
            //return this.ctx.Query<BusinessType>("Select * From BusinessType Order By Aid Asc", new { }).ToList();
        }

        // 업종코드로 업종명 불러오기
        public async Task<string> Detail_BusinessType_Name(string BusinessType_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<string>("Select BusinessType_Name From BusinessType Where BusinessType_Code = @BusinessType_Code", new { BusinessType_Code }, commandType: CommandType.Text);
            }
            //return this.ctx.Query<string>("Select BusinessType_Name From BusinessType Where BusinessType_Code = @BusinessType_Code", new { BusinessType_Code }).SingleOrDefault();
        }

        //// 업종정보 상세 불러오기
        public async Task<BusiList_Entity> Detail_BusiList(string BusiList_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<BusiList_Entity>("Select  * From BusiList Where BusiList_Code = @BusiList_Code", new { BusiList_Code }, commandType: CommandType.Text);
            }
            //return this.ctx.Query<BusiList>("Select  * From BusiList Where BusiList_Code = @BusiList_Code", new { BusiList_Code }).SingleOrDefault();
        }
    }

    /// <summary>
    /// 선택 업종  정보
    /// </summary>
    public class BusiList_Lib : IBusiList_Lib
    {
        private readonly IConfiguration _db;

        public BusiList_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        // 선택 업종 입력하기
        public async Task<BusiList_Entity> Add_BusiList(BusiList_Entity BusiType)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var sql = "Insert BusiList (BusiList_Code, CompanyEtc_Code, BusinessType_Code, Staff_Code, PostIP) Values (@BusiList_Code, @CompanyEtc_Code, @BusinessType_Code, @Staff_Code, @PostIP);";
                await ctx.ExecuteAsync(sql, BusiType, commandType: CommandType.Text);
                return BusiType;
            }

            //this.ctx.Execute(sql, BusiType);
            //return BusiType;
        }

        /// 업종 선택 삭제
        public async Task Remove_BusiList(int Aid)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync("Delete BusiList Where Aid = @Aid", new { Aid }, commandType: CommandType.Text);
            }
            //this.ctx.Execute("Delete From BusiList Where Aid = @Aid", new { Aid });
        }

        // 선택 업종정보 마지막 일련번호 얻기
        public async Task<int> Last_Number()
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Top 1 Aid From BusiList Order by Aid Desc", commandType: CommandType.Text);
            }
            //return this.ctx.Query<int>("Select Top 1 Aid From BusiList Order by Aid Desc", new { }).SingleOrDefault();
        }

        // 선택 업종 정보 리스트
        public async Task<List<BusiList_Entity>> GetList_BusiList(string CompanyEtc_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var apt = await ctx.QueryAsync<BusiList_Entity>("Select * From BusiList Where CompanyEtc_Code = @CompanyEtc_Code Order By Aid Asc", new { CompanyEtc_Code }, commandType: CommandType.Text);
                return apt.ToList();
            }
            //return this.ctx.Query<BusiList>("Select * From BusiList Where CompanyEtc_Code = @CompanyEtc_Code Order By Aid Asc", new { CompanyEtc_Code }).ToList();
        }

        // 업종코드로 업체명 불러오기
        public async Task<string> Detail_BusinessType_Name(string BusinessType_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<string>("Select BusinessType_Name From BusinessType Where BusinessType_Code = @BusinessType_Code", new { BusinessType_Code }, commandType: CommandType.Text);
            }
            //return this.ctx.Query<string>("Select BusinessType_Name From BusinessType Where BusinessType_Code = @BusinessType_Code", new { BusinessType_Code }).SingleOrDefault();
        }

        //// 업종 정보 상세 불러오기
        public async Task<BusinessType_Entity> Detail_BusinessType_Detail(string BusinessType_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<BusinessType_Entity>("Select  * From BusinessType Where BusinessType_Code = @BusinessType_Code", new { BusinessType_Code }, commandType: CommandType.Text);
            }
            //return this.ctx.Query<BusinessType>("Select  * From BusinessType Where BusinessType_Code = @BusinessType_Code", new { BusinessType_Code }).SingleOrDefault();
        }
    }

    /// <summary>
    /// 단지 선택 업체
    /// </summary>
    public class Apt_Company_Lib : IApt_Company_Lib
    {
        private readonly IConfiguration _db;

        public Apt_Company_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        // 선택 업종 입력하기
        public async Task<Apt_Company_Entity> Add_AptCompany(Apt_Company_Entity Apt)
        {
            var sql = "Insert Apt_Company (Apt_Company_Code, Apt_Code, SortA_Code, SortB_Code, Company_Code, Staff_Code, PostIP) Values (@Apt_Company_Code, @Apt_Code, @SortA_Code, @SortB_Code, @Company_Code, @Staff_Code, @PostIP);";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, Apt, commandType: CommandType.Text);
                return Apt;
            }

            //this.ctx.Execute(sql, Apt);
            //return Apt;
        }

        /// 업종 선택 삭제
        public async Task Remove_AptCompany(int Aid)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync("Delete Apt_Company Where Aid = @Aid", new { Aid }, commandType: CommandType.Text);
            }
            //this.ctx.Execute("Delete From Apt_Company Where Aid = @Aid", new { Aid });
        }

        // 선택 업종정보 마지막 일련번호 얻기
        public async Task<int> Last_Number()
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Top 1 Aid From Apt_Company Order by Aid Desc", commandType: CommandType.Text);
            }
            // return this.ctx.Query<int>("Select Top 1 Aid From Apt_Company Order by Aid Desc", new { }).SingleOrDefault();
        }

        // 입력된 업체인 확인 얻기
        public async Task<int> Saved_Company(string Apt_Code, string Company_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Apt_Company Where Apt_Code = @Apt_Code and Company_Code = @Company_Code", new { Apt_Code, Company_Code }, commandType: CommandType.Text);
            }
            //return this.ctx.Query<int>("Select Count(*) From Apt_Company Where Apt_Code = @Apt_Code and Company_Code = @Company_Code", new { Apt_Code, Company_Code }).SingleOrDefault();
        }

        // 선택 업체 정보 리스트(단지)
        public async Task<List<Apt_Company_Entity>> GetList_Apt_Company(string Apt_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var apt = await ctx.QueryAsync<Apt_Company_Entity>("Select * From Apt_Company Where Apt_Code = @Apt_Code Order By Aid Asc", new { Apt_Code }, commandType: CommandType.Text);
                return apt.ToList();
            }
            //return this.ctx.Query<Apt_Company>("Select * From Apt_Company Where Apt_Code = @Apt_Code Order By Aid Asc", new { Apt_Code }).ToList();
        }

        // 선택 업체 정보 전체 리스트(단지_업체, 상세)
        public async Task<List<Company_Entity_Etc>> GetList_Apt_Company_CompanyEtc(string Apt_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var apt = await ctx.QueryAsync<Company_Entity_Etc>("Select * From Company, Company_Etc, Apt_Company Where Apt_Company.company_Code = Company.Company_Code and Company.Company_Code = Company_Etc.Company_Code and Apt_Company.Apt_Code = @Apt_Code Order By Apt_Company.Aid Asc", new { Apt_Code }, commandType: CommandType.Text);
                return apt.ToList();
            }
            //return this.ctx.Query<Company_Entity_Etc>("Select * From Company, Company_Etc, Apt_Company Where Apt_Company.company_Code = Company.Company_Code and Company.Company_Code = Company_Etc.Company_Code and Apt_Company.Apt_Code = @Apt_Code Order By Apt_Company.Aid Asc", new { Apt_Code }).ToList();
        }

        // 선택 업체 정보 리스트(단지, 상위분류, 하위분류)
        public async Task<List<Apt_Company_Entity>> GetList_Apt_Company_Sort(string Apt_Code, string SortA_Code, string SortB_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var apt = await ctx.QueryAsync<Apt_Company_Entity>("Select * From Apt_Company Where Apt_Code = @Apt_Code And SortA_Code = @SortA_Code And SortB_Code = @SortB_Code Order By Aid Asc", new { Apt_Code, SortA_Code, SortB_Code }, commandType: CommandType.Text);
                return apt.ToList();
            }
            //return this.ctx.Query<Apt_Company>("Select * From Apt_Company Where Apt_Code = @Apt_Code And SortA_Code = @SortA_Code And SortB_Code = @SortB_Code Order By Aid Asc", new { Apt_Code, SortA_Code, SortB_Code }).ToList();
        }

        // 업체코드로 업체명 불러오기
        //public string Detail_BusinessType_Name(string BusinessType_Code)
        //{
        //    return this.ctx.Query<string>("Select BusinessType_Name From BusinessType Where BusinessType_Code = @BusinessType_Code", new { BusinessType_Code }).SingleOrDefault();
        //}

        //// 선택 업체정보 상세 불러오기
        public async Task<Apt_Company_Entity> Detail_Apt_Company(string Apt_Company_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Apt_Company_Entity>("Select  * From Apt_Company Where Apt_Company_Code = @Apt_Company_Code", new { Apt_Company_Code }, commandType: CommandType.Text);
            }
            //return this.ctx.Query<Apt_Company>("Select  * From Apt_Company Where Apt_Company_Code = @Apt_Company_Code", new { Apt_Company_Code }).SingleOrDefault();
        }
    }
}