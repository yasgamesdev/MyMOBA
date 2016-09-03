using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMOBA_Server.Objects.Heroes
{
    class Unitychan : Hero
    {
        //common
        const ushort maxhp = 500;
        const ushort maxmp = 100;

        public Unitychan(byte faction, float xPos, float zPos, float yRot, float wRot)
            : base(faction, (byte)HeroType.Unitychan, maxhp, maxmp, xPos, zPos, yRot, wRot)
        {
        }

        public override ushort GetMaxHP()
        {
            //common
            return (ushort)(maxhp + (int)(maxhp * 0.1f) * (((HeroSyncData)sync).level - 1) + ((HeroSyncData)sync).items.GetHP());
        }

        public override ushort GetMaxMP()
        {
            return maxmp;
        }
    }
}
