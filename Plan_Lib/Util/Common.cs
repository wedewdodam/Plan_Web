using System;

namespace Plan_Blazor_Lib
{
    public class Common_Lib
    {
        public string numberFormat(object code)
        {
            try
            {
                int intA = Convert.ToInt32(code);
                string strA = string.Format("{0: ###,###.###}", intA);

                return strA;
            }
            catch (Exception)
            {
                return "오류";
            }
        }
    }
}