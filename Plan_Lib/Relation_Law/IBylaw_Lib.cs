using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plan_Lib
{
    public interface IBylaw_Lib
    {
        Task<Bylaw_Entity> Add_Bylaw(Bylaw_Entity By);

        Task<Bylaw_Entity> Edit_Bylaw(Bylaw_Entity By);

        Task Remove_Repair_Cost(int Bylaw_Code);

        Task<List<Bylaw_Entity>> GetList(string Apt_Code);

        Task<int> Bylaw_Last_Code(string Apt_Code);

        Task<int> Being_Bylaw_Code(string Apt_Code);

        Task<string> Being_Bylaw_Code_be(string Apt_Code);

        Task<Bylaw_Entity> GetDetail_Bylaw(int Bylaw_Code);

        Task<int> Bylaw_Revision(string Apt_Code);

        /// <summary>
        /// 관리규약 상세보기 불러오기
        /// </summary>
        Task<Bylaw_Entity> Details_Bylaw(string Apt_Code);

    }
}