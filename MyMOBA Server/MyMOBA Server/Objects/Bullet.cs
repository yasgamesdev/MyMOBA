using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMOBA_Server.Objects
{
    public class Bullet
    {
        public ushort srcObjID { get; private set; }
        public ushort destObjID { get; private set; }

        public Bullet(ushort srcObjID, ushort destObjID)
        {
            this.srcObjID = srcObjID;
            this.destObjID = destObjID;
        }

        public void WriteData(NetOutgoingMessage message)
        {
            message.Write(srcObjID);
            message.Write(destObjID);
        }

        public static Bullet ReadData(NetIncomingMessage message)
        {
            return new Bullet(message.ReadUInt16(), message.ReadUInt16());
        }
    }
}
