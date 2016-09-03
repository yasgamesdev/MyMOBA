using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace MyMOBA_Server.Objects.Core
{
    class CoreSyncData : HPSyncData
    {
        public BuffDebuff buff { get; private set; }
        public EnemyVision vision { get; private set; }

        public CoreSyncData(ushort objID, ushort hp, BuffDebuff buff, EnemyVision vision) : base(objID, hp)
        {
            this.buff = buff;
            this.vision = vision;
        }

        public void Update(float delta)
        {
            buff.Update(delta);
            vision.Update(delta);
        }

        public override void WriteData(NetOutgoingMessage message)
        {
            message.Write(objID);
            message.Write(hp);
            buff.WriteData(message);
            vision.WriteData(message);
        }

        public static CoreSyncData ReadData(NetIncomingMessage message)
        {
            return new CoreSyncData(
                message.ReadUInt16(),
                message.ReadUInt16(),
                BuffDebuff.ReadData(message),
                EnemyVision.ReadData(message)
                );
        }
    }
}
