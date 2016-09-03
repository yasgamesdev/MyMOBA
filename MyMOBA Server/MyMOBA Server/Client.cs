using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMOBA_Server
{
    public class Client
    {
        public NetConnection connect { get; private set; }
        public ushort objID;

        public Client(NetConnection connect)
        {
            this.connect = connect;
        }
    }
}
