using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMOBA_Server.Objects.Heroes
{
    public class ExpPool
    {
        HeroSyncData sync;
        const int maxLevel = 18;
        int[] needExp;
        int curExp;

        public ExpPool(HeroSyncData sync)
        {
            this.sync = sync;

            needExp = new int[maxLevel - 1];
            needExp[0] = 100;
            for(int i=1; i<needExp.Length; i++)
            {
                needExp[i] = needExp[0] + (int)(needExp[0] * 0.1f) * i;
            }

            curExp = 0;
        }

        public void GetExp(int exp)
        {
            curExp += exp;

            while (true)
            {
                if (sync.level == maxLevel)
                {
                    break;
                }

                if (curExp >= needExp[sync.level - 1])
                {
                    curExp -= needExp[sync.level - 1];
                    sync.LevelUp();
                }
                else
                {
                    break;
                }
            }
        }
    }
}
