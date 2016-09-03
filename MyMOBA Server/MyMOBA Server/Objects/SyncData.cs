using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMOBA_Server.Objects
{
    public abstract class SyncData
    {
        public ushort objID { get; private set; }

        public SyncData(ushort objID)
        {
            this.objID = objID;
        }

        public abstract void WriteData(NetOutgoingMessage message);
    }
}
