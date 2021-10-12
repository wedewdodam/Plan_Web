using Plan_Blazor_Lib.Cost;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plan_Blazor_Lib.Price
{
    /// <summary>
    ///  수선 단가 품목
    /// </summary>
    public interface IRepair_DetailKind_Lib
    {
        /// <summary>
        /// 단가 폼목 입력
        /// </summary>
        Task<Repair_DetailKind_Entity> Add_Repair_Detail(Repair_DetailKind_Entity Kind);

        /// <summary>
        /// 단가 폼목 수정
        /// </summary>
        Task<Repair_DetailKind_Entity> Edit_Repair_Detail(Repair_DetailKind_Entity Kind);

        /// <summary>
        /// 단가 폼목 삭제
        /// </summary>        
        Task Remove_DetailKind(int DetailKind_Code);

        /// <summary>
        /// 입력 여부 확인
        /// </summary>
        Task<int> Cost_Count(string Product_Name, string Standard_name, string Sort_C_Code);

        /// <summary>
        /// 입력된 일련번호 불러오기
        /// </summary>
        Task<int> Being_Code(string Product_Name, string Standard_name, string Sort_C_Code);

        /// <summary>
        /// 시설물 대분류로 단가품목 불러오기
        /// </summary>
        Task<List<Repair_DetailKind_Entity>> GetList_DetailKind_A(string Sort_A_Code, string DetailKind_Division);

        /// <summary>
        /// 시설물 중분류로 단가품목 불러오기
        /// </summary>        
        Task<List<Repair_DetailKind_Entity>> GetList_DetailKind_B(string Sort_B_Code, string DetailKind_Division);

        /// <summary>
        /// 시설물 공사종별로 단가품목 불러오기
        /// </summary>
        Task<List<Repair_DetailKind_Entity>> GetList_DetailKind_C(string Sort_C_Code, string DetailKind_Division);

        /// <summary>
        /// 시설물 공사종별로 단가품목 불러오기
        /// </summary>
        Task<List<Repair_DetailKind_Entity>> GetList_DetailKind_Product_Name(string Sort_C_Code, string DetailKind_Division);

        /// <summary>
        /// 시설물 공사종별로 단가폼목 규격 정보 불러오기
        /// </summary>
        Task<List<Repair_DetailKind_Entity>> GetList_DetailKind_PN(string Sort_C_Code, string DetailKind_Division, string Product_Name);

        /// <summary>
        /// 단가 폼목 명 불러오기
        /// </summary>
        Task<string> GetDetail_Name(int DetailKind_Code);

        /// <summary>
        /// 단가 단위 불러오기
        /// </summary>
        Task<string> GetDetail_Unitcd(string DetailKind_Code);

        /// <summary>
        /// 품목 입된 수 가져오기
        /// </summary>
        Task<int> Sort_Count(string Sort_Field, string Sort_Code);
    }

    /// <summary>
    /// 단가 모음
    /// </summary>
    public interface IPrice_Set_Lib
    {
        /// <summary>
        /// 단가 모음 입력
        /// </summary>
        Task<Price_Set_Entity> Add_PriceSet(Price_Set_Entity set);

        /// <summary>
        /// 단가 모음 수정
        /// </summary>
        Task<Price_Set_Entity> Edit_PriceSet(Price_Set_Entity set);

        /// <summary>
        /// 단가 모음 리스트
        /// </summary>        
        Task<List<Price_Set_Entity>> GetList(string Repair_Cost_Code, string Price_Division);

        /// <summary>
        /// 단가 모음 리스트 
        /// </summary>
        Task<List<Price_Set_Entity>> GetList_A(string Repair_Cost_Code);

        /// <summary>
        /// 수선항목 별 일괄단가 입력 여부 확인
        /// </summary>
        Task<int> Price_Set_Division(string Repair_Cost_Code, string Price_Division);

        /// <summary>
        /// 단가 모음 삭제
        /// </summary>
        Task Remove(int Price_Set_Code);

        /// <summary>
        /// 단가 모음 삭제(수선금액 기준으로)
        /// </summary>
        Task Remove_CostCode(string Repair_Cost_Code);

        /// <summary>
        /// 해당 장기수선계획 식별코드로 입력된 단가모음을 모두 삭제
        /// </summary>
        Task Delete_PriceSet_PlanCode(string Repair_Plan_Code);

        /// <summary>
        /// 수선항목 별 단가 합계
        /// </summary>
        Task<int> Sum_Artcle_Price(string Repair_Cost_Code, string Price_Division);

        /// <summary>
        /// 수선항목 별 단가 합계(삭제 후 수정)
        /// </summary>
        Task<Repair_Price_Entity> Sum_Artcle_Price_Edit(string Repair_Cost_Code, string Price_Division);

        /// <summary>
        ///  수선항목 별 수선금액(세부단가 혹은 단지단가, 직접단가) 합계
        /// </summary>
        Task<int> Sum_Artcle_Cost(string Repair_Cost_Code, string Price_Division);

        /// <summary>
        ///  수선항목 별 수선수량 합계
        /// </summary>
        Task<int> Sum_Artcle_Amount(string Repair_Cost_Code, string Price_Division);

        /// <summary>
        /// 단가 모음 존재 여부(단가 구분에 따른)
        /// </summary>
        Task<int> Being_PriceSet(string Repair_Cost_Code, string Price_Division);

        /// <summary>
        /// 단가 모음 존재 여부
        /// </summary>
        Task<int> Being_PriceSet_be(string Repair_Cost_Code);

        /// <summary>
        /// 해당 품목 존재 여부
        /// </summary>
        Task<int> Being_PriceSet_DetailKind(string Repair_Cost_Code, string DetailKind_Code);

        /// <summary>
        /// 해당 품목 존재 여부(단가코드로)
        /// </summary>        
        Task<int> Being_PriceSet_PriceCode(string Repair_Cost_Code, string Price_Division, string Price_Code);

        /// <summary>
        /// 식별코드로 상세 내역 불러오기
        /// </summary>
        Task<Price_Set_Entity> GetDetail_RPP(string Repair_Cost_Code, string Price_Division, string Price_Code);

        /// <summary>
        /// 장기수선계획 정기 조정 시에 전 장기수선계획 코드를 가진 모든 데이터 입력
        /// </summary>
        Task All_Insert_Code(string Apt_Code, string Repair_Plan_Code_A, string Repair_Plan_Code_B, string Repair_Cost_Code_A, string Repair_Cost_Code_B, string Staff_Code);

        /// <summary>
        /// 해당 수선금액 코드에 따른 단가 모음 존재 여부(17/02/05)
        /// </summary>
        Task<int> Being_CostCede_PriceSet(string Apt_Code, string Repair_Plan_Code_A, string Repair_Cost_Code_A);

        
    }

    /// <summary>
    /// 단가
    /// </summary>
    public interface IRepair_Price_Lib
    {
        /// <summary>
        /// 세부 단가 입력
        /// </summary>
        Task<Repair_Price_Entity> Add_RepairPrice(Repair_Price_Entity Price);

        /// <summary>
        /// 식별코드로 상세 내역 불러오기 
        /// </summary>
        Task<Repair_Price_Kind_Entity> GetDetail_RPC(int Price_Code, string DetailKind_Division);

        /// <summary>
        /// 해당 공사종별의 최근 일괄단가 정보 가져오기
        /// </summary>
        Task<int> GetDetail_RPC_P(string Sort_C_Code, string DetailKind_Division);

        /// <summary>
        /// 적용일자 별로 불러 오기
        /// </summary>
        Task<List<Repair_Price_Entity>> GetList_Repair_Price(DateTime regist_dt);

        /// <summary>
        /// 공사종별 코드로 불러오기
        /// </summary>        
        Task<List<Repair_Price_Kind_Entity>> GetList_Repair_C_Price(string Sort_C_Code, string DetailKind_Division);

        /// <summary>
        /// 중분류 코드로 불러오기
        /// </summary>
        Task<List<Repair_Price_Kind_Entity>> GetList_Repair_B_Price(string Sort_B_Code, string DetailKind_Division);

        /// <summary>
        /// 대분류 코드로 불러오기
        /// </summary>        
        Task<List<Repair_Price_Kind_Entity>> GetList_Repair_A_Price(string Sort_A_Code, string DetailKind_Division);

        /// <summary>
        /// 세부단가 분류별 불러오기
        /// </summary>
        Task<List<Repair_Price_Kind_Entity>> GetList_Sort_Price_Detail(string Sort_Field, string Sort_Query, string DetailKind_Division);

        /// <summary>
        /// 단가 폼목 삭제
        /// </summary>
        Task Remove_Price(int Price_Code);

        /// <summary>
        /// 세부단가 입력된 종사종별 단가 수
        /// </summary>
        Task<int> Price_Count_SortC(string Sort_C_Code, string DetailKind_Division);

        /// <summary>
        /// 세부단가 입력된 중분류 단가 수
        /// </summary>
        Task<int> Price_Count_SortB(string Sort_B_Code, string DetailKind_Division);

        /// <summary>
        /// 세부단가 입력된 중분류 단가 수
        /// </summary>
        Task<int> Price_Count_SortA(string Sort_A_Code, string DetailKind_Division);

        /// <summary>
        /// 해당 공사종별의 단가표
        /// </summary>
        Task<List<Repair_Price_Kind_Entity>> Price_Drop_List(string Sort_C_Code, string DetailKind_Division);
    }

    /// <summary>
    /// 장기수선충당금 사용계획서
    /// </summary>
    public interface ICost_Using_Plan_Lib
    {
        /// <summary>
        /// 장충금 사용계획서 입력
        /// </summary>
        Task<Cost_Using_Plan_Entity> Add(Cost_Using_Plan_Entity cup);

        /// <summary>
        /// 장충금 사용계획서 수정
        /// </summary>
        Task<Cost_Using_Plan_Entity> Edit(Cost_Using_Plan_Entity cup);

        /// <summary>
        /// 장충금 사용계획서 삭제
        /// </summary>
        Task Remove_Repair_Cost(int Cost_Use_Plan_Code);

        /// <summary>
        /// 장충금 사용계획서 정보 리스트
        /// </summary>
       Task<List<Cost_Using_Plan_Entity>> GetList(string Apt_Code);


        /// <summary>
        /// 장충금 사용계획서 중복 여부
        /// </summary>
        Task<int> Being(string Apt_Code, string Repair_Plan_Code, string Repair_Article_Code);

        /// <summary>
        /// 장충금 사용계획서 존재 여부 확인 (식별코드 찾음)
        /// </summary>
        Task<int> BeCode(string Apt_Code, string Repair_Plan_Code, string Repair_Article_Code);

        /// <summary>
        /// 장충금 사용계획서 정보 리스트
        /// </summary>
        Task<List<Cost_Using_Plan_Entity>> GetList_PlanCode(string Apt_Code, string Repair_Plan_Code);

        /// <summary>
        /// 장충금 사용계획서 정보 리스트
        /// </summary>
        Task<List<Cost_Using_Plan_Entity>> GetList_PlanCode_Year(string Apt_Code, string Repair_Plan_Code, string Plan_Year);

        /// <summary>
        /// 장충금 사용계획서 상세
        /// </summary>        
        Task<Cost_Using_Plan_Entity> Detail(string Repair_Plan_Code, string Repair_Article_Code);

        /// <summary>
        /// 장충금 사용계획서 상세
        /// </summary>
        Task<Cost_Using_Plan_Entity> Detail_Code(string Cost_Use_Plan_Code);

        /// <summary>
        /// 장충금 사용계획서 정보 리스트
        /// </summary>
        Task<List<Cost_Using_Plan_Entity>> GetList_Year(string Apt_Code, string Plan_Year);
    }


    /// <summary>
    /// 원가계산서 할증율 관리 입력
    /// </summary>
    public interface IUnitPrice_Rate_Lib
    {
        // <summary>
        /// 할증율 입력
        /// </summary>        
        Task<UnitPrice_Rate_Entity> Add(UnitPrice_Rate_Entity upr);

        /// <summary>
        /// 할증율 수정
        /// </summary>        
        Task<UnitPrice_Rate_Entity> Edit(UnitPrice_Rate_Entity upr);

        /// <summary>
        /// 할증율 삭제
        /// </summary>
        Task Remove(int UnitPrice_Rate_Code);

        /// <summary>
        /// 할증율 기준년도 존재여부
        /// </summary>
        /// <param name="Standard_Year"></param>
        /// <returns></returns>
        Task<int> Being(string Standard_Year);

        /// <summary>
        /// 할증율 목록
        /// </summary>
        Task<List<UnitPrice_Rate_Entity>> GetList();

        /// <summary>
        /// 할증율 상세보기
        /// </summary>        
        Task<UnitPrice_Rate_Entity> Detail(string UnitPrice_Rate_Code);

        /// <summary>
        /// 할증율 상세보기(년도)
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        Task<UnitPrice_Rate_Entity> Detail_Year(string Standard_Year);

        /// <summary>
        /// 할증율 상세보기(최근 년도)
        /// </summary>
        /// <param name="년도"></param>
        /// <returns></returns>
        Task<UnitPrice_Rate_Entity> Detail_New_Year();

        /// <summary>
        /// 할증율 년도 리스트
        /// </summary>
        Task<List<UnitPrice_Rate_Entity>> DetailList();        
    }

    /// <summary>
    /// 원가계산서 보고서 작성 메서드
    /// </summary>
    public interface IPrime_Cost_Report_Lib
    {
        /// <summary>
        /// 원가계산서 작성을 위한 수선금액 리스트
        /// </summary>
        Task<List<Cost_Entity>> GetList(string Apt_Code, string Repair_Plan_Code, string Price_Sort);

        /// <summary>
        /// 원가계산서 작성을 위한 수선금액 리스트
        /// </summary>
        Task<List<Cost_Entity>> GetList_Sort(string Apt_Code, string Repair_Plan_Code, string Price_Sort, string Sort, string Sort_Code);

        /// <summary>
        /// 단가 모음 정보 불러오기(기준단가)
        /// </summary>
        Task<Prime_Cost_Report_Entity> GetDetail_Set(string Repair_Cost_Code);

        /// <summary>
        /// 단가 모음 정보 불러오기(단지단가)
        /// </summary>
        Task<Prime_Cost_Report_Entity> GetDetail_Select_Price(string Repair_Cost_Code);
    }
}