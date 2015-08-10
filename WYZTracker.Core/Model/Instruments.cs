using System;
using System.Collections.Generic;
using System.Text;

namespace WYZTracker
{
    public class Instruments : System.ComponentModel.BindingList<Instrument>
    {
        internal int GetSafeId()
        {
            int result = 0;
            bool existing = false;
            do
            {
                existing = false;
                foreach (Instrument instr in this)
                {
                    if (instr.ID == result.ToString())
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
