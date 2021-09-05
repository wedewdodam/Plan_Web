using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plan_Blazor_Lib.Review
{
    public interface IPlan_Review_Lib
    {
        Task<Plan_Review_Entity> Add_Plan_Review(Plan_Review_Entity review);

        Task<Plan_Review_Entity> Edit_Plan_Review(Plan_Review_Entity review);

        Task<int> Being_Plan_Review(string Repair_Plan_Code);

        Task<int> Last_Plan_Review_Code(string Repair_Plan_Code);

        Task<DateTime> Last_Plan_ReviewDate_ago(string Repair_Plan_Code);

        Task<DateTime> Last_Plan_ReviewDate_BeDate(string Repair_Plan_Code);

        Task<DateTime> Last_Plan_ReviewDate_Apt(string Apt_Code);

        Task<int> Being_Plan_Review_Division(string Repair_Plan_Code, string RepairReview_Division);

        Task<List<Plan_Review_Entity>> GetList_Plan_Review(string Repair_Plan_Code);

        Task<List<Plan_Review_Entity>> GetList_Plan_Review_Code_Date(string Repair_Plan_Code);

        Task<List<Plan_Review_Entity>> GetList_Apt_Code(string Apt_Code);

        Task<Plan_Review_Entity> Detail_PlanReview(int Plan_Review_Code);

        Task<string> Getlist_Rereview_Code(string Apt_Code);

        Task<string> Rereview_Division(string Plan_Review_Code);

        Task<Plan_Review_Plan_Entity> Plan_Review_infor(string Plan_Review_Code);

        Task Delete_Rereview(string Repair_Plan_Code);

        Task Remove_PlanReview(string Plan_Review_Code);

        Task<List<Plan_Review_Entity>> Getlist_Apt(string Apt_Code);

        Task<int> Non_Complete(string Apt_Code);

        Task<List<Plan_Review_Plan_Entity>> GetList_Apt_Page(int Page, string Apt_Code);

        Task<int> GetList_Apt_Page_Count(string Apt_Code);
    }

    public interface IReview_Content_Lib
    {
        Task<Review_Content_Entity> Add_Review_Content(Review_Content_Entity content);

        Task<Review_Content_Entity> Edit_Review_Content(Review_Content_Entity content);

        Task Remove_Repair_Review_Content(int Plan_Review_Content_Code);

        Task<List<Review_Content_Entity>> GetList_Review_Content_Sort(string Apt_Code, string Plan_Review_Code, string Sort_Code);

        Task<List<Review_Content_Entity>> GetList_Sort(string Plan_Review_Code, string Sort_Field, string Sort_Code, string Apt_Code);

        Task<List<Review_Content_Entity>> GetList_Review_Content(string Plan_Review_Code);

        Task<Review_Content_Entity> Detail_Review_Content(int Plan_Review_Content_Code);

        Task<int> Being_Review_Content_Code(string Repair_Article_Code, string Plan_Review_Code);

        int Being_Review_Content(string Repair_Article_Code, string Plan_Review_Code);

        Task Delete_Repair_Review_Content(string Plan_Review_Code);

        Task<Review_Content_Entity> Detail_PlanReview_Article(string Plan_Review_Code, string Repair_Article_Code);

        Task<Review_Content_Join_Enity> detail_ReeviewCode_ArticleName(string Plan_Review_Code, string Apt_Code, string Repair_Article_Name);

        Task<Review_Content_Join_Enity> View_ReviewCode_ArticleName(string Plan_Review_Code, string Apt_Code, string Repair_Article_Name);

        Task<List<Review_Content_Join_Enity>> detail_ReviewCode_Sort(string Plan_Review_Code, string Apt_Code, string Sort_C_Code);

        Task<int> Detail_PlanReview_Article_Count(string Plan_Review_Code, string Repair_Article_Code);

        Task Remove_Article_ReviewContent(string Repair_Article_Code);

        int Review_Content_Count(string Apt_Code, string Plan_Review_Code);

        Task<List<Review_Content_Join_Enity>> GetList_ReviewCode_Sort(string Plan_Review_Code, string Apt_Code, string Sort_A_Code);
    }
}