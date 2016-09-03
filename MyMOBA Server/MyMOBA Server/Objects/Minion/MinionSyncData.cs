using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace MyMOBA_Server.Objects.Minion
{
    class MinionSyncData : HPSyncData
    {
        public byte faction { get; private set; }
        public float xPos { get; private set; }
        public float zPos { get; private set; }
        public float yRot { get; private set; }
        public float wRot { get; private set; }
        public byte anime { get; private set; }
        public BuffDebuff buff { get; private set; }
        public EnemyVision vision { get; private set; }

        public MinionSyncData(ushort objID, ushort hp, byte faction, float xPos, float zPos, float yRot, float wRot, byte anime, BuffDebuff buff, EnemyVision vision) : base(objID, hp)
        {
            this.faction = faction;
            this.xPos = xPos;
            this.zPos = zPos;
            this.yRot = yRot;
            this.wRot = wRot;
            this.anime = anime;
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
            message.Write(faction);
            message.Write(xPos);
            message.Write(zPos);
            message.Write(yRot);
            message.Write(wRot);
            message.Write(anime);
            buff.WriteData(message);
            vision.WriteData(message);
        }

        public static MinionSyncData ReadData(NetIncomingMessage message)
        {
            return new MinionSyncData(
                message.ReadUInt16(),
                message.ReadUInt16(),
                message.ReadByte(),
                message.ReadFloat(),
                message.ReadFloat(),
                message.ReadFloat(),
                message.ReadFloat(),
                message.ReadByte(),
                BuffDebuff.ReadData(message),
                EnemyVision.ReadData(message)
                );
        }

        public void SetRotate(float yRot, float wRot)
        {
            this.yRot = yRot;
            this.wRot = wRot;
        }

        public void SetAnime(byte anime)
        {
            this.anime = anime;
        }

        public void SetPosition(float xPos, float zPos)
        {
            this.xPos = xPos;
            this.zPos = zPos;
        }
    }
}
