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

        public int GetNextFx(int fx)
        {
            int idx = 0;
            for(int i=0,li = this.Count;i < li;++i)
            {
                if(this[i].ID == fx)
                {
                    idx = i + 1;
                    break;
                }
            }
            if(idx>=this.Count)
            {
                idx = 0;
            }
            return this[idx].ID;
        }

        public int GetPreviousFx(int fx)
        {
            int idx = this.Count - 1;
            for (int i = idx, li = 0; i >= li; --i)
            {
                if (this[i].ID == fx)
                {
                    idx = i - 1;
                    break;
                }
            }
            if (idx < 0)
            {
                idx = this.Count - 1;
            }
            return this[idx].ID;
        }
    }
}
