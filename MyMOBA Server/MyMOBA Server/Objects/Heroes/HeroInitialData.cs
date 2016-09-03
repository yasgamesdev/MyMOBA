using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace MyMOBA_Server.Objects.Heroes
{
    class HeroInitialData : InitialData
    {
        public byte faction { get; private set; }
        public byte heroID { get; private set; }

        public HeroInitialData(byte faction, byte heroID)
        {
            this.faction = faction;
            this.heroID = heroID;
        }

        public override void WriteData(NetOutgoingMessage message)
        {
            message.Write(faction);
            message.Write(heroID);
        }

        public static HeroInitialData ReadData(NetIncomingMessage message)
        {
            return new HeroInitialData(
                message.ReadByte(),
                message.ReadByte()
                );
        }
    }
}
