using Dapper;
using Microsoft.Extensions.Configuration;
using Plan_Blazor_Lib.Cost;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Plan_Blazor_Lib.Price
{
    /// <summary>
    ///  수선 단가 품목
    /// </summary>
    public class Repair_DetailKind_Lib : IRepair_DetailKind_Lib
    {
        private readonly IConfiguration _db;

        public Repair_DetailKind_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 단가 폼목 입력
        /// </summary>
        public async Task<Repair_DetailKind_Entity> Add_Repair_Detail(Repair_DetailKind_Entity Kind)
        {
            var sql = "Insert Into Detail_Kind (Sort_A_Code, Sort_B_Code, Sort_C_Code, Product_Name, Standard_name, DetailKind_Division, Unit_cd, DetailKind_Etc, Status_kb, Staff_Code) Values (@Sort_A_Code, @Sort_B_Code, @Sort_C_Code, @Product_Name, @Standard_name, @DetailKind_Division, @Unit_cd, @DetailKind_Etc, @Status_kb, @Staff_Code);";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var Aid = await ctx.QuerySingleOrDefaultAsync<int>(sql, Kind);
                Kind.DetailKind_Code = Aid;
                return Kind;
            }
        }

        /// <summary>
        /// 단가 폼목 수정
        /// </summary>
        public async Task<Repair_DetailKind_Entity> Edit_Repair_Detail(Repair_DetailKind_Entity Kind)
        {
            var sql = "Update Detail_Kind Set Product_Name =  @Product_Name, Standard_name = @Standard_name, DetailKind_Division = @DetailKind_Division, Unit_cd = @Unit_cd, DetailKind_Etc = @DetailKind_Etc, Staff_Code = @Staff_Code Where Detailkind_Code = @DetailKind_Code";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, Kind);
                return Kind;
            }
        }

        /// <summary>
        /// 단가 폼목 삭제
        /// </summary>        
        public async Task Remove_DetailKind(int DetailKind_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync("Delete From Detail_Kind Where DetailKind_Code = @DetailKind_Code", new { DetailKind_Code }, commandType: CommandType.Text);
            }
            //this.dnn.ctx.Execute("Delete From Detail_Kind Where DetailKind_Code = @DetailKind_Code", new { DetailKind_Code });
        }

        /// <summary>
        /// 입력 여부 확인
        /// </summary>
        public async Task<int> Cost_Count(string Product_Name, string Standard_name, string Sort_C_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Detail_Kind Where Product_Name = @Product_Name And Standard_name = @Standard_name And Sort_C_Code = @Sort_C_Code", new { Product_Name, Standard_name, Sort_C_Code }, commandType: CommandType.Text);
            }
            //return this.dnn.ctx.Query<int>("Select Count(*) From Detail_Kind Where Product_Name = @Product_Name And Standard_name = @Standard_name And Sort_C_Code = @Sort_C_Code", new { Product_Name, Standard_name, Sort_C_Code }).SingleOrDefault();
        }

        /// <summary>
        /// 입력된 일련번호 불러오기
        /// </summary>
        public async Task<int> Being_Code(string Product_Name, string Standard_name, string Sort_C_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select DetailKind_Code From Detail_Kind Where Product_Name = @Product_Name And Standard_name = @Standard_name And Sort_C_Code = @Sort_C_Code", new { Product_Name, Standard_name, Sort_C_Code }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 시설물 대분류로 단가품목 불러오기
        /// </summary>
        public async Task<List<Repair_DetailKind_Entity>> GetList_DetailKind_A(string Sort_A_Code, string DetailKind_Division)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aa = await ctx.QueryAsync<Repair_DetailKind_Entity>("Select * From Detail_Kind Where Sort_A_Code = @Sort_A_Code And DetailKind_Division = @DetailKind_Division Order By DetailKind_Code Desc", new { Sort_A_Code, DetailKind_Division }, commandType: CommandType.Text);
                return aa.ToList();
            }

        }

        /// <summary>
        /// 시설물 중분류로 단가품목 불러오기
        /// </summary>        
        public async Task<List<Repair_DetailKind_Entity>> GetList_DetailKind_B(string Sort_B_Code, string DetailKind_Division)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aa = await ctx.QueryAsync<Repair_DetailKind_Entity>("Select * From Detail_Kind Where Sort_B_Code = @Sort_B_Code And DetailKind_Division = @DetailKind_Division Order By DetailKind_Code Desc", new { Sort_B_Code, DetailKind_Division }, commandType: CommandType.Text);
                return aa.ToList();
            }

        }

        /// <summary>
        /// 시설물 공사종별로 단가품목 불러오기
        /// </summary>
        public async Task<List<Repair_DetailKind_Entity>> GetList_DetailKind_C(string Sort_C_Code, string DetailKind_Division)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aa = await ctx.QueryAsync<Repair_DetailKind_Entity>("Select * From Detail_Kind Where Sort_C_Code = @Sort_C_Code And DetailKind_Division = @DetailKind_Division Order By DetailKind_Code Desc", new { Sort_C_Code, DetailKind_Division }, commandType: CommandType.Text);
                return aa.ToList();
            }

        }

        /// <summary>
        /// 시설물 공사종별로 단가품목 불러오기
        /// </summary>
        public async Task<List<Repair_DetailKind_Entity>> GetList_DetailKind_Product_Name(string Sort_C_Code, string DetailKind_Division)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aa = await ctx.QueryAsync<Repair_DetailKind_Entity>("Select DISTINCT Product_Name From Detail_Kind Where Sort_C_Code = @Sort_C_Code And DetailKind_Division = @DetailKind_Division Order By Product_Name Asc", new { Sort_C_Code, DetailKind_Division }, commandType: CommandType.Text);
                return aa.ToList();
            }

        }

        /// <summary>
        /// 시설물 공사종별로 단가폼목 규격 정보 불러오기
        /// </summary>
        public async Task<List<Repair_DetailKind_Entity>> GetList_DetailKind_PN(string Sort_C_Code, string DetailKind_Division, string Product_Name)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aa = await ctx.QueryAsync<Repair_DetailKind_Entity>("Select * From Detail_Kind Where Sort_C_Code = @Sort_C_Code And DetailKind_Division = @DetailKind_Division And Product_Name = @Product_Name Order By DetailKind_Code Asc", new { Sort_C_Code, DetailKind_Division, Product_Name }, commandType: CommandType.Text);
                return aa.ToList();
            }
        }

        /// <summary>
        /// 단가 폼목 명 불러오기
        /// </summary>
        public async Task<string> GetDetail_Name(int DetailKind_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<string>("Select Product_Name From Detail_Kind Where DetailKind_Code = @DetailKind_Code", new { DetailKind_Code }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 단가 단위 불러오기
        /// </summary>
        public async Task<string> GetDetail_Unitcd(string DetailKind_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<string>("Select Unit_cd From Detail_Kind Where DetailKind_Code = @DetailKind_Code", new { DetailKind_Code }, commandType: CommandType.Text);
            }

        }

        /// <summary>
        /// 품목 입된 수 가져오기
        /// </summary>
        public async Task<int> Sort_Count(string Sort_Field, string Sort_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("DetailKind_Sort_Count", new { Sort_Field, Sort_Code }, commandType: CommandType.StoredProcedure);
            }

        }
    }

    /// <summary>
    /// 단가모음
    /// </summary>
    public class Price_Set_Lib : IPrice_Set_Lib
    {
        private readonly IConfiguration _db;

        public Price_Set_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 단가 모음 입력
        /// </summary>
        public async Task<Price_Set_Entity> Add_PriceSet(Price_Set_Entity set)
        {
            var sql = "Insert Into Price_Set (Apt_Code, Repair_Plan_Code, DetailKind_Code, Unit_cd, Goods_Name, Repair_Cost_Code, Price_Code, Price_Division, Select_Price, Repair_Cost, Repair_Amount, Regist_dt, Price_Set_Etc, Staff_Code) Values (@Apt_Code, @Repair_Plan_Code, @DetailKind_Code, @Unit_cd, @Goods_Name, @Repair_Cost_Code, @Price_Code, @Price_Division, @Select_Price, @Repair_Cost, @Repair_Amount, @Regist_dt, @Price_Set_Etc, @Staff_Code)";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, set);
                return set;
            }
            //this.dnn.ctx.Execute(sql, set);
            //return set;
        }

        /// <summary>
        /// 단가 모음 수정
        /// </summary>
        public async Task<Price_Set_Entity> Edit_PriceSet(Price_Set_Entity set)
        {
            var sql = "Update Price_Set Set DetailKind_Code = @DetailKind_Code, Goods_Name = @Goods_Name, Unit_cd = @Unit_cd, Price_Code = @Price_Code, Select_Price = @Select_Price, Repair_Cost = @Repair_Cost, Repair_Amount = @Repair_Amount, Regist_dt= @Regist_dt, Price_Set_Etc = @Price_Set_Etc, Staff_Code = @Staff_Code Where Repair_Cost_Code = @Repair_Cost_Code And DetailKind_Code = @DetailKind_Code";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, set);
                return set;
            }
            //this.dnn.ctx.Execute(sql, set);
            //return set;
        }

        /// <summary>
        /// 단가 모음 리스트
        /// </summary>        
        public async Task<List<Price_Set_Entity>> GetList(string Repair_Cost_Code, string Price_Division)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aa = await ctx.QueryAsync<Price_Set_Entity>("Select Price_Set_Code, Apt_Code, Repair_Plan_Code, DetailKind_Code, Repair_Cost_Code, Price_Code, Goods_Name, Unit_cd, Price_Division, Select_Price, Repair_Cost, Repair_Amount, Regist_dt, Price_Set_Etc, Staff_Code, PostDate From Price_Set Where Repair_Cost_Code = @Repair_Cost_Code And Price_Division = @Price_Division And Repair_Cost > 0 and Repair_Amount > 0 Order By Price_Set_Code Asc", new { Repair_Cost_Code, Price_Division }, commandType: CommandType.Text);
                return aa.ToList();
            }

        }

        /// <summary>
        /// 단가 모음 리스트 
        /// </summary>
        public async Task<List<Price_Set_Entity>> GetList_A(string Repair_Cost_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aa = await ctx.QueryAsync<Price_Set_Entity>("Select Price_Set_Code, Apt_Code, Repair_Plan_Code, DetailKind_Code, Repair_Cost_Code, Price_Code, Goods_Name, Unit_cd, Price_Division, Select_Price, Repair_Cost, Repair_Amount, Regist_dt, Price_Set_Etc, Staff_Code, PostDate From Price_Set Where Repair_Cost_Code = @Repair_Cost_Code And Price_Division != 'B' And Repair_Cost > 0 and Repair_Amount > 0 Order By Price_Set_Code Asc", new { Repair_Cost_Code }, commandType: CommandType.Text);
                return aa.ToList();
            }

        }

        /// <summary>
        /// 수선항목 별 일괄단가 입력 여부 확인
        /// </summary>
        public async Task<int> Price_Set_Division(string Repair_Cost_Code, string Price_Division)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Price_Set Where Repair_Cost_Code = @Repair_Cost_Code And Price_Division = @Price_Division", new { Repair_Cost_Code, Price_Division }, commandType: CommandType.Text);
            }

        }

        /// <summary>
        /// 단가 모음 삭제
        /// </summary>
        public async Task Remove(int Price_Set_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync("Delete From Price_Set Where Price_Set_Code = @Price_Set_Code", new { Price_Set_Code }, commandType: CommandType.Text);
            }
            //this.dnn.ctx.Execute("Delete From Price_Set Where Price_Set_Code = @Price_Set_Code", new { Price_Set_Code });
        }

        /// <summary>
        /// 단가 모음 삭제
        /// </summary>
        public async Task Remove_CostCode(string Repair_Cost_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync("Delete From Price_Set Where Repair_Cost_Code = @Repair_Cost_Code", new { Repair_Cost_Code }, commandType: CommandType.Text);
            }
            //this.dnn.ctx.Execute("Delete From Price_Set Where Price_Set_Code = @Price_Set_Code", new { Price_Set_Code });
        }

        /// <summary>
        /// 해당 장기수선계획 식별코드로 입력된 단가모음을 모두 삭제
        /// </summary>
        /// <param name="Repair_Plan_Code">장기수선계획 식별코드</param>
        /// 2017
        public async Task Delete_PriceSet_PlanCode(string Repair_Plan_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync("Delete Price_Set Where Repair_Plan_Code = @Repair_Plan_Code", new { Repair_Plan_Code }, commandType: CommandType.Text);
            }

        }

        /// <summary>
        /// 수선항목 별 단가 합계
        /// </summary>
        public async Task<int> Sum_Artcle_Price(string Repair_Cost_Code, string Price_Division)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select isNull(Sum(Select_Price), 0) From Price_Set Where Repair_Cost_Code = @Repair_Cost_Code And Price_Division = @Price_Division And Repair_Cost > 0 and Repair_Amount > 0", new { Repair_Cost_Code, Price_Division }, commandType: CommandType.Text);
            }

        }

        /// <summary>
        /// 수선항목 별 단가 합계(삭제 후 수정)
        /// </summary>
        public async Task<Repair_Price_Entity> Sum_Artcle_Price_Edit(string Repair_Cost_Code, string Price_Division)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Repair_Price_Entity>("Select ISNULL(Sum(Source_amt), 0) as Source_amt, ISNULL(Sum(Labor_amt), 0) as Labor_amt, ISNULL(Sum(Cost_amt), 0) as Cost_amt From Price_Set as a Join Repair_Price as b on a.DetailKind_Code = b.DetailKind_Code Where a.Repair_Cost_Code = @Repair_Cost_Code and a.Price_Division = @Price_Division And a.Repair_Cost > 0 and a.Repair_Amount > 0", new { Repair_Cost_Code, Price_Division }, commandType: CommandType.Text);
            }

        }

        /// <summary>
        ///  수선항목 별 수선금액(세부단가 혹은 단지단가, 직접단가) 합계
        /// </summary>
        public async Task<int> Sum_Artcle_Cost(string Repair_Cost_Code, string Price_Division)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select isNull(Sum(Repair_Cost), 0) From Price_Set Where Repair_Cost_Code = @Repair_Cost_Code And Price_Division = @Price_Division And Repair_Cost > 0 and Repair_Amount > 0", new { Repair_Cost_Code, Price_Division }, commandType: CommandType.Text);
            }

        }

        /// <summary>
        ///  수선항목 별 수선수량 합계
        /// </summary>
        public async Task<int> Sum_Artcle_Amount(string Repair_Cost_Code, string Price_Division)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select isNull(Sum(Repair_Amount), 0) From Price_Set Where Repair_Cost_Code = @Repair_Cost_Code And Price_Division = @Price_Division And Repair_Cost > 0 and Repair_Amount > 0", new { Repair_Cost_Code, Price_Division }, commandType: CommandType.Text);
            }

        }

        /// <summary>
        /// 단가 모음 존재 여부(단가 구분에 따른)
        /// </summary>
        public async Task<int> Being_PriceSet(string Repair_Cost_Code, string Price_Division)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select isNull(Sum(Repair_Amount), 0) From Price_Set Where Repair_Cost_Code = @Repair_Cost_Code And Price_Division = @Price_Division And Repair_Cost > 0 and Repair_Amount > 0", new { Repair_Cost_Code, Price_Division }, commandType: CommandType.Text);
            }

        }

        /// <summary>
        /// 단가 모음 존재 여부
        /// </summary>
        public async Task<int> Being_PriceSet_be(string Repair_Cost_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) from Price_set Where Repair_Cost_Code = @Repair_Cost_Code", new { Repair_Cost_Code }, commandType: CommandType.Text);
            }

        }

        /// <summary>
        /// 해당 품목 존재 여부
        /// </summary>
        public async Task<int> Being_PriceSet_DetailKind(string Repair_Cost_Code, string DetailKind_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) from Price_set Where Repair_Cost_Code = @Repair_Cost_Code And DetailKind_Code = @DetailKind_Code", new { Repair_Cost_Code, DetailKind_Code }, commandType: CommandType.Text);
            }

        }

        /// <summary>
        /// 해당 품목 존재 여부(단가코드로)
        /// </summary>        
        public async Task<int> Being_PriceSet_PriceCode(string Repair_Cost_Code, string Price_Division, string Price_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) from Price_set Where Repair_Cost_Code = @Repair_Cost_Code And Price_Division = @Price_Division And Price_Code = @Price_Code", new { Repair_Cost_Code, Price_Division, Price_Code }, commandType: CommandType.Text);
            }

        }

        /// <summary>
        /// 식별코드로 상세 내역 불러오기
        /// </summary>
        public async Task<Price_Set_Entity> GetDetail_RPP(string Repair_Cost_Code, string Price_Division, string Price_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Price_Set_Entity>("Select Top 1 * From Price_Set Where Repair_Cost_Code = @Repair_Cost_Code And Price_Division = @Price_Division And Price_Code = @Price_Code Order By Price_Set_Code Desc", new { Repair_Cost_Code, Price_Division, Price_Code }, commandType: CommandType.Text);
            }

        }

        /// <summary>
        /// 장기수선계획 정기 조정 시에 전 장기수선계획 코드를 가진 모든 데이터 입력
        /// </summary>
        public async Task All_Insert_Code(string Apt_Code, string Repair_Plan_Code_A, string Repair_Plan_Code_B, string Repair_Cost_Code_A, string Repair_Cost_Code_B, string Staff_Code)
        {
            var sql = "Insert into Price_Set (Apt_Code, Repair_Plan_Code, DetailKind_Code, Goods_Name, Unit_cd, Repair_Cost_Code, Price_Code, Price_Division, Select_Price, Repair_Cost, Repair_Amount, Regist_dt, Price_Set_Etc, Staff_Code, PostDate) Select Apt_Code, @Repair_Plan_Code_B, DetailKind_Code, Goods_Name, Unit_cd, @Repair_Cost_Code_B, Price_Code, Price_Division, Select_Price, Repair_Cost, Repair_Amount, Regist_dt, Price_Set_Etc, @Staff_Code, GetDate() From Price_Set Where Apt_Code = @Apt_Code And Repair_Plan_Code = @Repair_Plan_Code_A And Repair_Cost_Code = @Repair_Cost_Code_A";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, new { Apt_Code, Repair_Plan_Code_A, Repair_Plan_Code_B, Repair_Cost_Code_A, Repair_Cost_Code_B, Staff_Code }, commandType: CommandType.Text);
            }

        }

        /// <summary>
        /// 해당 수선금액 코드에 따른 단가 모음 존재 여부(17/02/05)
        /// </summary>
        public async Task<int> Being_CostCede_PriceSet(string Apt_Code, string Repair_Plan_Code_A, string Repair_Cost_Code_A)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Price_Set Where Apt_Code = @Apt_Code And Repair_Plan_Code = @Repair_Plan_Code_A And Repair_Cost_Code = @Repair_Cost_Code_A", new { Apt_Code, Repair_Plan_Code_A, Repair_Cost_Code_A }, commandType: CommandType.Text);
            }
            //return dnn.ctx.Query<int>("Select Count(*) From Price_Set Where Apt_Code = @Apt_Code And Repair_Plan_Code = @Repair_Plan_Code_A And Repair_Cost_Code = @Repair_Cost_Code_A", new { Apt_Code, Repair_Plan_Code_A, Repair_Cost_Code_A }).SingleOrDefault();
        }
    }

    /// <summary>
    /// 단가
    /// </summary>
    public class Repair_Price_Lib : IRepair_Price_Lib
    {
        private readonly IConfiguration _db;

        public Repair_Price_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 세부 단가 입력
        /// </summary>
        public async Task<Repair_Price_Entity> Add_RepairPrice(Repair_Price_Entity Price)
        {
            var sql = "Insert Into Repair_Price (DetailKind_Code, Product_name, Source_amt, Labor_amt, Cost_amt, Unit_price, regist_dt, memo_tx, status_kb, User_Code, pk_Code) Values (@DetailKind_Code, @Product_name, @Source_amt, @Labor_amt, @Cost_amt, @Unit_price, @regist_dt, @memo_tx, @status_kb, @User_Code, @pk_Code)";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, Price);
                return Price;
            }
            //this.dnn.ctx.Execute(sql, Price);
            //return Price;
        }

        /// <summary>
        /// 식별코드로 상세 내역 불러오기 
        /// </summary>
        public async Task<Repair_Price_Kind_Entity> GetDetail_RPC(int Price_Code, string DetailKind_Division)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Repair_Price_Kind_Entity>("Select Top 1 a.Price_Code, a.DetailKind_Code, a.Product_name, a.Source_amt, a.Cost_amt, a.Labor_amt, a.Unit_Price, a.regist_dt, b.Sort_A_Code, b.Sort_B_Code, b.Sort_C_Code, b.Standard_name, b.DetailKind_Division, b.Unit_cd From Repair_Price As a INNER JOIN Detail_Kind As b ON a.DetailKind_Code = b.DetailKind_Code Where a.Price_Code = @Price_Code And b.DetailKind_Division = @DetailKind_Division Order By a.Price_Code Desc", new { Price_Code, DetailKind_Division }, commandType: CommandType.Text);
            }

        }

        /// <summary>
        /// 해당 공사종별의 최근 일괄단가 정보 가져오기
        /// </summary>
        public async Task<int> GetDetail_RPC_P(string Sort_C_Code, string DetailKind_Division)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Top 1 a.Unit_Price From Repair_Price As a INNER JOIN Detail_Kind As b ON a.DetailKind_Code = b.DetailKind_Code Where b.Sort_C_Code = @Sort_C_Code And b.DetailKind_Division = @DetailKind_Division Order By a.Price_Code Desc", new { Sort_C_Code, DetailKind_Division }, commandType: CommandType.Text);
            }

        }

        /// <summary>
        /// 적용일자 별로 불러 오기
        /// </summary>
        public async Task<List<Repair_Price_Entity>> GetList_Repair_Price(DateTime regist_dt)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aa = await ctx.QueryAsync<Repair_Price_Entity>("Repair_Price_Infor", new { regist_dt }, commandType: CommandType.StoredProcedure);
                return aa.ToList();
            }
            //return dnn.ctx.Query<Repair_Price_Entity>("Repair_Price_Infor", new { regist_dt }, commandType: CommandType.StoredProcedure).ToList();
        }

        /// <summary>
        /// 공사종별 코드로 불러오기
        /// </summary>        
        public async Task<List<Repair_Price_Kind_Entity>> GetList_Repair_C_Price(string Sort_C_Code, string DetailKind_Division)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aa = await ctx.QueryAsync<Repair_Price_Kind_Entity>("Select * From Repair_Price As a INNER JOIN Detail_Kind As b ON a.DetailKind_Code = b.DetailKind_Code Where b.Sort_C_Code = @Sort_C_Code And b.DetailKind_Division = @DetailKind_Division Order By a.Price_Code Desc", new { Sort_C_Code, DetailKind_Division }, commandType: CommandType.Text);
                return aa.ToList();
            }

        }

        //SELECT * FROM authors AS a INNER JOIN publishers AS p ON a.city = p.city

        /// <summary>
        /// 중분류 코드로 불러오기
        /// </summary>
        public async Task<List<Repair_Price_Kind_Entity>> GetList_Repair_B_Price(string Sort_B_Code, string DetailKind_Division)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aa = await ctx.QueryAsync<Repair_Price_Kind_Entity>("Select * From Repair_Price As a INNER JOIN Detail_Kind As b ON a.DetailKind_Code = b.DetailKind_Code Where b.Sort_B_Code = @Sort_B_Code And b.DetailKind_Division = @DetailKind_Division Order By a.Price_Code Desc", new { Sort_B_Code, DetailKind_Division }, commandType: CommandType.Text);
                return aa.ToList();
            }

        }

        /// <summary>
        /// 대분류 코드로 불러오기
        /// </summary>        
        public async Task<List<Repair_Price_Kind_Entity>> GetList_Repair_A_Price(string Sort_A_Code, string DetailKind_Division)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aa = await ctx.QueryAsync<Repair_Price_Kind_Entity>("Select * From Repair_Price As a INNER JOIN Detail_Kind As b ON a.DetailKind_Code = b.DetailKind_Code Where b.Sort_A_Code = @Sort_A_Code And b.DetailKind_Division = @DetailKind_Division Order By a.Price_Code Desc", new { Sort_A_Code, DetailKind_Division }, commandType: CommandType.Text);
                return aa.ToList();
            }

        }

        /// <summary>
        /// 세부단가 분류별 불러오기
        /// </summary>
        public async Task<List<Repair_Price_Kind_Entity>> GetList_Sort_Price_Detail(string Sort_Field, string Sort_Query, string DetailKind_Division)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aa = await ctx.QueryAsync<Repair_Price_Kind_Entity>("Price_List_Sort", new { Sort_Field, Sort_Query, DetailKind_Division }, commandType: CommandType.StoredProcedure);
                return aa.ToList();
            }

        }

        /// <summary>
        /// 단가 폼목 삭제
        /// </summary>
        public async Task Remove_Price(int Price_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync("Delete From Repair_Price Where Price_Code = @Price_Code", new { Price_Code });
                //return Cost;
            }
            //this.dnn.ctx.Execute("Delete From Repair_Price Where Price_Code = @Price_Code", new { Price_Code });
        }

        /// <summary>
        /// 세부단가 입력된 종사종별 단가 수
        /// </summary>
        public async Task<int> Price_Count_SortC(string Sort_C_Code, string DetailKind_Division)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Repair_Price As a INNER JOIN Detail_Kind As b ON a.DetailKind_Code = b.DetailKind_Code Where b.Sort_C_Code = @Sort_C_Code And b.DetailKind_Division = @DetailKind_Division", new { Sort_C_Code, DetailKind_Division }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 세부단가 입력된 중분류 단가 수
        /// </summary>
        public async Task<int> Price_Count_SortB(string Sort_B_Code, string DetailKind_Division)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Repair_Price As a INNER JOIN Detail_Kind As b ON a.DetailKind_Code = b.DetailKind_Code Where b.Sort_B_Code = @Sort_B_Code And b.DetailKind_Division = @DetailKind_Division", new { Sort_B_Code, DetailKind_Division }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 세부단가 입력된 중분류 단가 수
        /// </summary>
        public async Task<int> Price_Count_SortA(string Sort_A_Code, string DetailKind_Division)
        {
            using var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Repair_Price As a INNER JOIN Detail_Kind As b ON a.DetailKind_Code = b.DetailKind_Code Where b.Sort_A_Code = @Sort_A_Code And b.DetailKind_Division = @DetailKind_Division", new { Sort_A_Code, DetailKind_Division }, commandType: CommandType.Text);
        }

        /// <summary>
        /// 해당 공사종별의 단가표
        /// </summary>
        public async Task<List<Repair_Price_Kind_Entity>> Price_Drop_List(string Sort_C_Code, string DetailKind_Division)
        {
            using var db = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection"));
            var lst = await db.QueryAsync<Repair_Price_Kind_Entity>("Repair_Price_Drop", new { Sort_C_Code, DetailKind_Division }, commandType: CommandType.StoredProcedure);
            return lst.ToList();
        }
    }

    /// <summary>
    /// 장기수선충당금 사용계획서
    /// </summary>
    public class Cost_Using_Plan_Lib : ICost_Using_Plan_Lib
    {
        private readonly IConfiguration _db;

        public Cost_Using_Plan_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 장충금 사용계획서 입력
        /// </summary>
        public async Task<Cost_Using_Plan_Entity> Add(Cost_Using_Plan_Entity cup)
        {
            var sql = "Insert Into Cost_Using_Plan (Apt_Code, Repair_Plan_Code, Repair_Article_Code, Plan_Year, Repair_Name, Repair_Position, Repair_Range, Repair_Cost_Sum, Repair_Detail, Design_Drawing, Repair_Method, Start_Date, End_Date, Tender_Method_Process, PostIP, Staff_Code) Values (@Apt_Code, @Repair_Plan_Code, @Repair_Article_Code, @Plan_Year, @Repair_Name, @Repair_Position, @Repair_Range, @Repair_Cost_Sum, @Repair_Detail, @Design_Drawing, @Repair_Method, @Start_Date, @End_Date, @Tender_Method_Process, @PostIP, @Staff_Code)";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, cup);
                return cup;
            }
            //dn.ctx.Execute(sql, cup);
            //return cup;
        }

        /// <summary>
        /// 장충금 사용계획서 수정
        /// </summary>
        public async Task<Cost_Using_Plan_Entity> Edit(Cost_Using_Plan_Entity cup)
        {
            var sql = "Update Cost_Using_Plan Set Repair_Name = @Repair_Name, Repair_Position = @Repair_Position, Repair_Range = @Repair_Range, Repair_Cost_Sum = @Repair_Cost_Sum, Repair_Detail = @Repair_Detail, Design_Drawing = @Design_Drawing, Repair_Method = @Repair_Method, Start_Date = @Start_Date, End_Date = @End_Date, Tender_Method_Process = @Tender_Method_Process, PostIP = @PostIP, Staff_Code = @Staff_Code Where Cost_Use_Plan_Code = @Cost_Use_Plan_Code";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, cup);
                return cup;
            }
            //dn.ctx.Execute(sql, cup);
            //return cup;
        }

        /// <summary>
        /// 장충금 사용계획서 삭제
        /// </summary>
        public async Task Remove_Repair_Cost(int Cost_Use_Plan_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync("Delete From Cost_Using_Plan Where Cost_Use_Plan_Code = @Cost_Use_Plan_Code", new { Cost_Use_Plan_Code }, commandType: CommandType.Text);
            }
            //this.dn.ctx.Execute("Delete From Cost_Using_Plan Where Cost_Use_Plan_Code = @Cost_Use_Plan_Code", new { Cost_Use_Plan_Code });
        }

        /// <summary>
        /// 장충금 사용계획서 정보 리스트
        /// </summary>
        public async Task<List<Cost_Using_Plan_Entity>> GetList(string Apt_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aa = await ctx.QueryAsync<Cost_Using_Plan_Entity>("Select * From Cost_Using_Plan Where Apt_Code = @Apt_Code Order By Cost_Use_Plan_Code Desc", new { Apt_Code }, commandType: CommandType.Text);
                return aa.ToList();
            }
            //return this.dn.ctx.Query<Cost_Using_Plan_Entity>("Select * From Cost_Using_Plan Where Apt_Code = @Apt_Code Order By Cost_Use_Plan_Code Desc", new { Apt_Code }).ToList();
        }

        /// <summary>
        /// 장충금 사용계획서 중복 여부
        /// </summary>
        public async Task<int> Being(string Apt_Code, string Repair_Plan_Code, string Repair_Article_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From Cost_Using_Plan Where Apt_Code = @Apt_Code And Repair_Plan_Code = @Repair_Plan_Code And Repair_Article_Code = @Repair_Article_Code", new { Apt_Code, Repair_Plan_Code, Repair_Article_Code }, commandType: CommandType.Text);
            }
            //return this.dn.ctx.Query<int>("Select Count(*) From Cost_Using_Plan Where Apt_Code = @Apt_Code And Repair_Plan_Code = @Repair_Plan_Code And Repair_Article_Code = @Repair_Article_Code", new { Apt_Code, Repair_Plan_Code, Repair_Article_Code }).SingleOrDefault();
        }

        /// <summary>
        /// 장충금 사용계획서 존재 여부 확인 (식별코드 찾음)
        /// </summary>
        public async Task<int> BeCode(string Apt_Code, string Repair_Plan_Code, string Repair_Article_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Top 1 Cost_Use_Plan_Code From Cost_Using_Plan Where Apt_Code = @Apt_Code And Repair_Plan_Code = @Repair_Plan_Code And Repair_Article_Code = @Repair_Article_Code", new { Apt_Code, Repair_Plan_Code, Repair_Article_Code }, commandType: CommandType.Text);
            }
            //return this.dn.ctx.Query<int>("Select Top 1 Cost_Use_Plan_Code From Cost_Using_Plan Where Apt_Code = @Apt_Code And Repair_Plan_Code = @Repair_Plan_Code And Repair_Article_Code = @Repair_Article_Code", new { Apt_Code, Repair_Plan_Code, Repair_Article_Code }).SingleOrDefault();
        }

        /// <summary>
        /// 장충금 사용계획서 정보 리스트
        /// </summary>
        public async Task<List<Cost_Using_Plan_Entity>> GetList_PlanCode(string Apt_Code, string Repair_Plan_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aa = await ctx.QueryAsync<Cost_Using_Plan_Entity>("Select * From Cost_Using_Plan Where Apt_Code = @Apt_Code And Repair_Plan_Code = @Repair_Plan_Code Order By Cost_Use_Plan_Code Desc", new { Apt_Code, Repair_Plan_Code }, commandType: CommandType.Text);
                return aa.ToList();
            }
            
        }

        /// <summary>
        /// 장충금 사용계획서 정보 리스트
        /// </summary>
        public async Task<List<Cost_Using_Plan_Entity>> GetList_PlanCode_Year(string Apt_Code, string Repair_Plan_Code, string Plan_Year)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aa = await ctx.QueryAsync<Cost_Using_Plan_Entity>("Select Cost_Use_Plan_Code, Apt_Code, Repair_Plan_Code, Repair_Article_Code, Plan_Year, Repair_Name, Repair_Position, Repair_Range, Repair_Cost_Sum, Repair_Detail, Design_Drawing, Repair_Method, Start_Date, End_Date, Tender_Method_Process, PostDate, PostIP, Staff_Code From Cost_Using_Plan Where Apt_Code = @Apt_Code And Repair_Plan_Code = @Repair_Plan_Code And Plan_Year = @Plan_Year Order By Cost_Use_Plan_Code Desc", new { Apt_Code, Repair_Plan_Code, Plan_Year }, commandType: CommandType.Text);
                return aa.ToList();
            }
            
        }

        /// <summary>
        /// 장충금 사용계획서 정보 리스트
        /// </summary>
        public async Task<List<Cost_Using_Plan_Entity>> GetList_Year(string Apt_Code, string Plan_Year)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aa = await ctx.QueryAsync<Cost_Using_Plan_Entity>("Select Cost_Use_Plan_Code, Apt_Code, Repair_Plan_Code, Repair_Article_Code, Plan_Year, Repair_Name, Repair_Position, Repair_Range, Repair_Cost_Sum, Repair_Detail, Design_Drawing, Repair_Method, Start_Date, End_Date, Tender_Method_Process, PostDate, PostIP, Staff_Code From Cost_Using_Plan Where Apt_Code = @Apt_Code And Plan_Year = @Plan_Year Order By Cost_Use_Plan_Code Desc", new { Apt_Code, Plan_Year }, commandType: CommandType.Text);
                return aa.ToList();
            }

        }

        /// <summary>
        /// 장충금 사용계획서 상세
        /// </summary>        
        public async Task<Cost_Using_Plan_Entity> Detail(string Repair_Plan_Code, string Repair_Article_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Cost_Using_Plan_Entity>("Select * From Cost_Using_Plan Where Repair_Plan_Code = @Repair_Plan_Code And Repair_Article_Code = @Repair_Article_Code", new { Repair_Plan_Code, Repair_Article_Code }, commandType: CommandType.Text);
            }
            //return this.dn.ctx.Query<Cost_Using_Plan_Entity>("Select * From Cost_Using_Plan Where Repair_Plan_Code = @Repair_Plan_Code And Repair_Article_Code = @Repair_Article_Code", new { Repair_Plan_Code, Repair_Article_Code }).SingleOrDefault();
        }

        /// <summary>
        /// 장충금 사용계획서 상세
        /// </summary>
        public async Task<Cost_Using_Plan_Entity> Detail_Code(string Cost_Use_Plan_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Cost_Using_Plan_Entity>("Select * From Cost_Using_Plan Where Cost_Use_Plan_Code = @Cost_Use_Plan_Code", new { Cost_Use_Plan_Code }, commandType: CommandType.Text);
            }
            //return this.dn.ctx.Query<Cost_Using_Plan_Entity>("Select * From Cost_Using_Plan Where Cost_Use_Plan_Code = @Cost_Use_Plan_Code", new { Cost_Use_Plan_Code }).SingleOrDefault();
        }
    }

    /// <summary>
    /// 원가계산서 할증율 관리 입력
    /// </summary>
    public class UnitPrice_Rate_Lib : IUnitPrice_Rate_Lib
    {
        private readonly IConfiguration _db;

        public UnitPrice_Rate_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 할증율 입력
        /// </summary>        
        public async Task<UnitPrice_Rate_Entity> Add(UnitPrice_Rate_Entity upr)
        {
            var sql = "Insert Into UnitPrice_Rate (Standard_Year, Source_Rate, Labor_Rate, Cost_Rate, Indirectness_Rate, Industrial_Accident_Rate, Employ_Insurance_Rate, Well_Insurance_Rate, Pension_Insurance_Rate, OldMan_Insurance_Rate, Health_Safe_Insurance_Rate, Environment_Priserve_Rate, Etc_Cost_Rate, Common_Cost_Rate, Profit_Rate, VAT_Rate, Staff_Code, PostIP) Values (@Standard_Year, @Source_Rate, @Labor_Rate, @Cost_Rate, @Indirectness_Rate, @Industrial_Accident_Rate, @Employ_Insurance_Rate, @Well_Insurance_Rate, @Pension_Insurance_Rate, @OldMan_Insurance_Rate, @Health_Safe_Insurance_Rate, @Environment_Priserve_Rate, @Etc_Cost_Rate, @Common_Cost_Rate, @Profit_Rate, @VAT_Rate, @Staff_Code, @PostIP)";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, upr);
                return upr;
            }
        }

        /// <summary>
        /// 할증율 수정
        /// </summary>        
        public async Task<UnitPrice_Rate_Entity> Edit(UnitPrice_Rate_Entity upr)
        {
            var sql = "Update UnitPrice_Rate Set Standard_Year = @Standard_Year, Source_Rate = @Source_Rate, Labor_Rate =@Labor_Rate, Cost_Rate = @Cost_Rate, Indirectness_Rate = @Indirectness_Rate, Industrial_Accident_Rate = @Industrial_Accident_Rate, Employ_Insurance_Rate = @Employ_Insurance_Rate, Well_Insurance_Rate = @Well_Insurance_Rate, Pension_Insurance_Rate = @Pension_Insurance_Rate, OldMan_Insurance_Rate = @OldMan_Insurance_Rate, Health_Safe_Insurance_Rate = @Health_Safe_Insurance_Rate, Environment_Priserve_Rate = @Environment_Priserve_Rate, Etc_Cost_Rate = @Etc_Cost_Rate, Common_Cost_Rate = @Common_Cost_Rate, Profit_Rate = @Profit_Rate, VAT_Rate = @VAT_Rate, Staff_Code = @Staff_Code, PostDate = @PostDate, PostIP = @PostIP Where UnitPrice_Rate_Code = @UnitPrice_Rate_Code";
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync(sql, upr);
                return upr;
            }
            //dn.ctx.Execute(sql, upr);
            //return upr;
        }

        /// <summary>
        /// 할증율 삭제
        /// </summary>
        public async Task Remove(int UnitPrice_Rate_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                await ctx.ExecuteAsync("Delete From UnitPrice_Rate Where UnitPrice_Rate_Code = @UnitPrice_Rate_Code", new { UnitPrice_Rate_Code }, commandType: CommandType.Text);
            }
            //this.dn.ctx.Execute("Delete From UnitPrice_Rate Where UnitPrice_Rate_Code = @UnitPrice_Rate_Code", new { UnitPrice_Rate_Code });
        }

        /// <summary>
        /// 할증율 기준년도 존재여부
        /// </summary>
        /// <param name="Standard_Year"></param>
        /// <returns></returns>
        public async Task<int> Being(string Standard_Year)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<int>("Select Count(*) From UnitPrice_Rate Where Standard_Year = @Standard_Year", new { Standard_Year }, commandType: CommandType.Text);
            }

        }

        /// <summary>
        /// 할증율 목록
        /// </summary>
        public async Task<List<UnitPrice_Rate_Entity>> GetList()
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aa = await ctx.QueryAsync<UnitPrice_Rate_Entity>("Select * From UnitPrice_Rate Order By UnitPrice_Rate_Code Desc", commandType: CommandType.Text);
                return aa.ToList();
            }
            //return dn.ctx.Query<UnitPrice_Rate_Entity>("Select * From UnitPrice_Rate Order By UnitPrice_Rate_Code Desc", new { }).ToList();
        }

        /// <summary>
        /// 할증율 상세보기
        /// </summary>        
        public async Task<UnitPrice_Rate_Entity> Detail(string UnitPrice_Rate_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<UnitPrice_Rate_Entity>("Select * From UnitPrice_Rate Where UnitPrice_Rate_Code = @UnitPrice_Rate_Code Desc", commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// 할증율 상세보기(년도)
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public async Task<UnitPrice_Rate_Entity> Detail_Year(string Standard_Year)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<UnitPrice_Rate_Entity>("Select Top 1 * From UnitPrice_Rate Where Standard_Year = @Standard_Year Order By UnitPrice_Rate_Code Desc", new { Standard_Year }, commandType: CommandType.Text);
            }

        }

        /// <summary>
        /// 할증율 상세보기(최근 년도)
        /// </summary>
        /// <param name="년도"></param>
        /// <returns></returns>
        public async Task<UnitPrice_Rate_Entity> Detail_New_Year()
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<UnitPrice_Rate_Entity>("Select Top 1 * From UnitPrice_Rate Order By UnitPrice_Rate_Code Desc", commandType: CommandType.Text);
            }
            //return dn.ctx.Query<UnitPrice_Rate_Entity>("Select Top 1 * From UnitPrice_Rate Order By UnitPrice_Rate_Code Desc", new { }).SingleOrDefault();
        }

        /// <summary>
        /// 할증율 년도 리스트
        /// </summary>
        public async Task<List<UnitPrice_Rate_Entity>> DetailList()
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aa = await ctx.QueryAsync<UnitPrice_Rate_Entity>("Select Standard_Year From UnitPrice_Rate Order By UnitPrice_Rate_Code Desc", commandType: CommandType.Text);
                return aa.ToList();
            }
        }
    }

    /// <summary>
    /// 원가계산서 보고서 작성 메서드
    /// </summary>
    public class Prime_Cost_Report_Lib : IPrime_Cost_Report_Lib
    {
        private readonly IConfiguration _db;

        public Prime_Cost_Report_Lib(IConfiguration configuration)
        {
            this._db = configuration;
        }

        /// <summary>
        /// 원가계산서 작성을 위한 수선금액 리스트
        /// </summary>
        public async Task<List<Cost_Entity>> GetList(string Apt_Code, string Repair_Plan_Code, string Price_Sort)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aa = await ctx.QueryAsync<Cost_Entity>("Select Repair_Cost_Code, Repair_All_Cost, Sort_A_Name, Sort_B_Name, Sort_C_Name, Price_Sort, Repair_Amount, (Select a.Repair_Article_Name From Repair_Article As a Where a.Aid = Repair_Article_Code) As Repair_Article_Code From Repair_Cost Where Apt_Code = @Apt_Code And Repair_Plan_Code = @Repair_Plan_Code And (Price_Sort = 'A') Order By Sort_A_Code Asc, CONVERT(int, Sort_B_Code) Asc, CONVERT(int, Sort_C_Code) Asc", new { Apt_Code, Repair_Plan_Code, Price_Sort }, commandType: CommandType.Text);
                return aa.ToList();
            }

        }

        /// <summary>
        /// 원가계산서 작성을 위한 수선금액 리스트
        /// </summary>
        public async Task<List<Cost_Entity>> GetList_Sort(string Apt_Code, string Repair_Plan_Code, string Price_Sort, string Sort, string Sort_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                var aa = await ctx.QueryAsync<Cost_Entity>("Select Repair_Cost_Code, Sort_A_Name, Sort_B_Name, Sort_C_Name, Repair_All_Cost, Repair_Amount, (Select a.Repair_Article_Name From Repair_Article As a Where a.Aid = Repair_Article_Code) As Repair_Article_Code From Repair_Cost Where Apt_Code = @Apt_Code And Repair_Plan_Code = @Repair_Plan_Code And Price_Sort = @Price_Sort And " + Sort + " = @Sort_Code Order By Sort_A_Code Asc", new { Apt_Code, Repair_Plan_Code, Price_Sort, Sort, Sort_Code }, commandType: CommandType.Text);
                return aa.ToList();
            }

        }

        /// <summary>
        /// 단가 모음 정보 불러오기(기준단가)
        /// </summary>
        public async Task<Prime_Cost_Report_Entity> GetDetail_Set(string Repair_Cost_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Prime_Cost_Report_Entity>("Select top 1 Sum(b.Source_amt * a.Repair_Amount) As Source_amt, Sum(b.Labor_amt * a.Repair_Amount) As Labor_amt, Sum(b.Cost_amt * a.Repair_Amount) As Cost_amt From Price_Set As a Join Repair_Price As b On a.DetailKind_Code = b.DetailKind_Code Where Repair_Cost_Code = @Repair_Cost_Code And Price_Division = 'A' And b.PostDate IN (SELECT max(PostDate) FROM Repair_Price GROUP BY Product_name)", new { Repair_Cost_Code }, commandType: CommandType.Text);
            }

        }

        /// <summary>
        /// 단가 모음 정보 불러오기(단지단가)
        /// </summary>
        public async Task<Prime_Cost_Report_Entity> GetDetail_Select_Price(string Repair_Cost_Code)
        {
            using (var ctx = new SqlConnection(_db.GetConnectionString("Khmais_db_Connection")))
            {
                return await ctx.QuerySingleOrDefaultAsync<Prime_Cost_Report_Entity>("Select Top 1 ISNULL(Sum(a.Select_Price), 0) as Select_Price, ISNULL(SUM(a.Repair_Amount),0) as Repair_Amount, ISNULL(sum(b.Source_amt), 0) as Source_amt, ISNULL(sum(b.Labor_amt), 0) as Labor_amt, ISNULL(sum(b.Cost_amt), 0) as Cost_amt, ISNULL(Sum(a.Repair_Cost), 0) as Repair_Cost From Price_Set as a Join Repair_Price as b on a.DetailKind_Code = b.DetailKind_Code Where Repair_Cost_Code = @Repair_Cost_Code and Price_Division = 'Q' and b.PostDate in (select MAX(PostDate) From Repair_Price Group By Product_name)", new { Repair_Cost_Code }, commandType: CommandType.Text);
            }

        }
    }

}