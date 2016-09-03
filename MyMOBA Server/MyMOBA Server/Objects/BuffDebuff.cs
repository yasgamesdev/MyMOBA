using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMOBA_Server.Objects
{
    public class BuffDebuff
    {
        public int state { get; private set; }
        public BuffDebuff(int state)
        {
            this.state = state;
        }

        public void Update(float delta)
        {

        }

        public void WriteData(NetOutgoingMessage message)
        {
            message.Write(state);
        }

        public static BuffDebuff ReadData(NetIncomingMessage message)
        {
            return new BuffDebuff(message.ReadInt32());
        }
    }
}
