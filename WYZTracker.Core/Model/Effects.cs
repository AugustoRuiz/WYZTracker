using System;
using System.Collections.Generic;
using System.Text;

namespace WYZTracker
{
    public class Effects : System.ComponentModel.BindingList<Effect>
    {
        internal int GetSafeId()
        {
            int result = 0;
            bool existing = false;
            do
            {
                existing = false;
                foreach (Effect e in this)
                {
                    if (e.ID == result)
                    {
                        result++;
                        existing = true;
                        break;
                    }
                }
            }
            while (existing == true);
            return result;
        }
    }
}
