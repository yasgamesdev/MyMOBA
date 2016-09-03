using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMOBA_Server.Objects
{
    public abstract class InitialData
    {
        public abstract void WriteData(NetOutgoingMessage message);
    }
}
