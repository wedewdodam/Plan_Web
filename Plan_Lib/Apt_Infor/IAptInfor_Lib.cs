using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plan_Apt_Lib
{
    public interface IAptInfor_Lib
    {
        Task<AptInfor_Entity> Detail_Apt(string Apt_Code);

        Task<AptInfor_Entity> Add_Apt(AptInfor_Entity Apt);

        Task<AptInfor_Entity> Add_Test_Apt(AptInfor_Entity Apt);

        Task<AptInfor_Entity> Edit_Apt(AptInfor_Entity Apt);

        Task<AptInfor_Entity> Edit_Apt_kat(AptInfor_Entity Apt);

        Task Edit_combine(string Apt_Code, string combile);

        Task<int> combibe_infor(string mem_id, string apt_cd);

        Task<List<AptInfor_Entity>> Apt_Search(string SearchField, string SearchQuery);

        Task<int> Apt_Search_count(string SearchField, string SearchQuery);

        Task<int> Apt_count();

        Task Edit_Approve(string Aid, string LevelCount);

        Task<int> Check_Count_Gun(string Apt_Adress_Gun);

        Task<int> Check_Count();

        Task<int> Approve_Count(int LevelCount);

        Task<int> Check_Count_Sido(string Apt_Adress_Gun);

        Task<int> Overlap_Check_Apt(string CorporateResistration_Num);

        Task<int> Overlap_Apt(string Apt_Code);

        Task<List<AptInfor_Entity>> GetList_Apt_Sido(string Apt_Adress_Sido);

        Task<List<AptInfor_Entity>> GetList_Apt();

        Task<List<AptInfor_Entity>> GetList_Apt_Search(string Apt_Name);

        Task<List<AptInfor_Entity>> GetList_Apt_Gun(string Apt_Adress_Gun);

        Task<AptInfor_Entity> Detail_Apt_combile(string Apt_Code);

        Task<string> Apt_Name(string Apt_Code);

        Task Remeove_Apt(int AId);

        Task<List<Apt_Staff_Join_Entity>> GetList_Approve();

        Task<List<Apt_Staff_Join_Entity>> GetList_Approve_Sido(string Apt_Adress_Sido);

        Task<List<Apt_Staff_Join_Entity>> GetList_Approve_Gun(string Apt_Adress_Sido, string Apt_Adress_Gun);

        Task<int> being_apt(string Staff_code);

        Task<List<Apt_Staff_Join_Entity>> GetList_Search(string SearchFeild, string SearchQuery);

        Task<List<Apt_Staff_Join_Entity>> GetList_Search_Sido(string Apt_Adress_Sido, string SearchFeild, string SearchQuery);

        Task<List<Apt_Staff_Join_Entity>> GetList_Search_Gun(string Apt_Adress_Sido, string Apt_Adress_Gun, string SearchFeild, string SearchQuery);

        Task<int> Apt_Level(string Apt_Code);

        Task<int> apt_kai(string apt_cd);

        Task<AptInfor_Entity> Detail_Test_Apt(string Staff_code);

        Task<int> Being_Test_Apt(string Staff_code);

        Task<int> Test_Being_Count(string Apt_Name);

        Task aow_edit(string Apt_Code, DateTime date);
    }

    public interface IApt_Detail_Lib
    {
        Task<Apt_Detail_Entity> Add_AptDetail(Apt_Detail_Entity Apt);

        Task<Apt_Detail_Entity> Edit_AptDetail(Apt_Detail_Entity Apt);

        Task<Apt_Detail_Entity> Detail_AptDetail(string Apt_Code);

        Task<int> Repeat_AptDetail(string Apt_Code);

        Task<string> Apt_Developer(string Apt_Code);

        Task<string> Telephone(string Apt_Code);

        Task<int> Being(string Apt_Code);
    }

    public interface IDong_Lib
    {
        Task<Dong_Entity> Add_Dong(Dong_Entity Dong);

        Task<Dong_Entity> Edit_Dong(Dong_Entity Dong);

        Task<int> Last_Number();

        Task<int> Overlap_Check(string Dong_Name, string Apt_Code);

        Task<int> Being_Dong_Count(string Apt_Code);

        Task<int> Being_Family_Count(string Apt_Code);

        Task Remeove_Dong(int AId);

        Task<List<Dong_Entity>> GetList_Dong(string Apt_Code);

        Task<List<Dong_Entity>> GetList_Dong_Name(string Apt_Code);

        Task<string> Detail_Dong_Code(string Dong_Code);

        Task<Dong_Entity> Detail_Dong(string Dong_Code);
    }

    public interface IDong_Composition
    {
        Task<Dong_Composition_Entity> Add_Dong_Composition(Dong_Composition_Entity Dong);

        Task<Dong_Composition_Entity> Edit_Dong_Composition(Dong_Composition_Entity Dong);

        Task Remeove_Dong_Composition(int AId);

        Task<int> Last_Number();

        Task<int> Overlap_Check(int AId);

        Task<double> Total_Supply_Account(string Apt_Code);

        Task<int> Total_Family_Account(string Apt_Code);

        Task<double> Total_Area_Account(string Apt_Code);

        Task<List<Dong_Composition_Entity>> GetList_Dong_Composition(string Apt_Code);

        Task<Dong_Composition_Entity> Detail_Sort(string Dong_Composition_Code);

        Task<Dong_Composition_Entity> Total_Infor(string Apt_Code);
    }
}