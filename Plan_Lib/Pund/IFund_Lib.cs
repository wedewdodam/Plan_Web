using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plan_Lib.Pund
{
    public interface IRepair_Saving_Using_Pund_Lib
    {
        Task<Repair_Saving_Using_Pund_Entity> Add(Repair_Saving_Using_Pund_Entity sup);

        Task<Repair_Saving_Using_Pund_Entity> Edit(Repair_Saving_Using_Pund_Entity sup);

        Task<List<Repair_Saving_Using_Pund_Entity>> GetList(string Apt_Code);
    }
}