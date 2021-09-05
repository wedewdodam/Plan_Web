using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plan_Lib
{
    public interface IRelation_Law_Lib
    {
        Task<Relation_Law_Entity> Add(Relation_Law_Entity law);

        Task<Relation_Law_Entity> Edit(Relation_Law_Entity rlae);

        Task Remove(int Relation_Law_Code);

        Task<List<Relation_Law_Entity>> GetList(string Apt_Code, string Repair_Plan_Code);

        Task<List<Relation_Law_Entity>> GetList_Set(string Division_Set);
    }
}