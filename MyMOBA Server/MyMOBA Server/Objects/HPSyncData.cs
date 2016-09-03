using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace MyMOBA_Server.Objects
{
    public abstract class HPSyncData : SyncData
    {
        public ushort hp { get; private set; }

        public HPSyncData(ushort objID, ushort hp) : base(objID)
        {
            this.hp = hp;
        }

        public void DecreaseHP(int amount)
        {
            if(hp - amount < 0)
            {
                hp = 0;
            }
            else
            {
                hp -= (ushort)amount;
            }
        }

        public void SetHP(ushort hp)
        {
            this.hp = hp;
        }

        public void UpdateHP(ushort hp)
        {
            this.hp = hp;
        }

        public void Heal(ushort amount)
        {
            hp += amount;
        }
    }
}
