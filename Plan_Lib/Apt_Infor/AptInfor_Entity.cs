using System;

namespace Plan_Apt_Lib
{
    /// <summary>
    /// 공동주택 정보
    /// </summary>
    public class AptInfor_Entity
    {
        public int Aid { get; set; }
        public string Apt_Code { get; set; }
        public string apt_cd { get; set; }
        public string Apt_Name { get; set; }
        public string Apt_Form { get; set; }
        public string Apt_Adress_Sido { get; set; }
        public string Apt_Adress_Gun { get; set; }
        public string Apt_Adress_Rest { get; set; }
        public string CorporateResistration_Num { get; set; }
        public DateTime AcceptancedOfWork_Date { get; set; }
        public int LevelCount { get; set; }
        public string Staff_code { get; set; }
        public DateTime PostDate { get; set; }
        public string PostIP { get; set; }
        public string combile { get; set; }
    }

    /// <summary>
    /// 공동주택 상세 정보
    /// </summary>
    public class Apt_Detail_Entity
    {
        public int Aid { get; set; }
        public string Apt_Detail_Code { get; set; }
        public string Apt_Code { get; set; }
        public string Developer { get; set; }
        public string Builder { get; set; }
        public string District { get; set; }
        public double Site_Area { get; set; }
        public double Build_Area { get; set; }
        public double FloorTotal_Area { get; set; }
        public double Supply_Area { get; set; }
        public double FloorArea_Ratio { get; set; }
        public double BuildingCoverage_Ratio { get; set; }
        public double Heighest { get; set; }
        public string Heating_Way { get; set; }
        public string WaterSupply_Way { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public int Electric_Supply_Capacity { get; set; }
        public double Water_Quantity { get; set; }
        public int Park_Car_Count { get; set; }
        public string Management_Way { get; set; }
        public int Elevator { get; set; }
        public string Joint_Management { get; set; }
        public DateTime PostDate { get; set; }
        public string PostIP { get; set; }
    }

    /// <summary>
    /// 동 정보
    /// </summary>
    public class Dong_Entity
    {
        public int Aid { get; set; }
        public string Dong_Code { get; set; }
        public string Apt_Code { get; set; }
        public string Dong_Name { get; set; }
        public int Family_Num { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Floor_Num { get; set; }
        public int Exit_Num { get; set; }
        public int Elevator_Num { get; set; }
        public int Line_Num { get; set; }
        public string Hall_Form { get; set; }
        public string Roof_Form { get; set; }
        public int Dong_Area { get; set; }
        public string Dong_Etc { get; set; }
        public DateTime PostDate { get; set; }
        public string PostIP { get; set; }
    }

    /// <summary>
    /// 동 구성 정보
    /// </summary>
    public class Dong_Composition_Entity
    {
        public int Aid { get; set; }
        public string Dong_Composition_Code { get; set; }
        public string Apt_Code { get; set; }
        public string Dong_Code { get; set; }
        public double Supply_Area { get; set; }
        public int Area_Family_Num { get; set; }
        public double Only_Area { get; set; }
        public double Total_Area { get; set; }
        public string Dong_Etc { get; set; }
        public DateTime PostDate { get; set; }
        public string PostIP { get; set; }
    }
}