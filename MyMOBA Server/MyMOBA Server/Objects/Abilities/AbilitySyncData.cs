using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace MyMOBA_Server.Objects.Abilities
{
    class AbilitySyncData : SyncData
    {
        public byte abilityID { get; private set; }
        public ushort casterObjID { get; private set; }
        public byte faction { get; private set; }
        public float xPos { get; private set; }
        public float zPos { get; private set; }
        public float yRot { get; private set; }
        public float wRot { get; private set; }
        public byte anime { get; private set; }
        public EnemyVision vision { get; private set; }

        public AbilitySyncData(ushort objID, byte abilityID, ushort casterObjID, byte faction, float xPos, float zPos, float yRot, float wRot, byte anime, EnemyVision vision) : base(objID)
        {
            this.abilityID = abilityID;
            this.casterObjID = casterObjID;
            this.faction = faction;
            this.xPos = xPos;
            this.zPos = zPos;
            this.yRot = yRot;
            this.wRot = wRot;
            this.anime = anime;
            this.vision = vision;
        }

        public void Update(float delta)
        {
            vision.Update(delta);
        }

        public override void WriteData(NetOutgoingMessage message)
        {
            message.Write(objID);
            message.Write(abilityID);
            message.Write(casterObjID);
            message.Write(faction);
            message.Write(xPos);
            message.Write(zPos);
            message.Write(yRot);
            message.Write(wRot);
            message.Write(anime);
            vision.WriteData(message);
        }

        public static AbilitySyncData ReadData(NetIncomingMessage message)
        {
            return new AbilitySyncData(
                message.ReadUInt16(),
                message.ReadByte(),
                message.ReadUInt16(),
                message.ReadByte(),
                message.ReadFloat(),
                message.ReadFloat(),
                message.ReadFloat(),
                message.ReadFloat(),
                message.ReadByte(),
                EnemyVision.ReadData(message)
                );
        }

        public void SetPosition(float xPos, float zPos)
        {
            this.xPos = xPos;
            this.zPos = zPos;
        }
    }
}
