using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace MyMOBA_Server.Objects.Heroes
{
    public class HeroSyncData : HPSyncData
    {
        public ushort mp;
        public float xPos { get; private set; }
        public float zPos { get; private set; }
        public float yRot { get; private set; }
        public float wRot { get; private set; }
        public byte anime { get; private set; }
        public byte level { get; private set; }
        public ushort gold;
        public Items items { get; private set; }
        public BuffDebuff buff { get; private set; }
        public EnemyVision vision { get; private set; }

        public HeroSyncData(ushort objID, ushort hp, ushort mp, float xPos, float zPos, float yRot, float wRot, byte anime, byte level, ushort gold, Items items, BuffDebuff buff, EnemyVision vision) : base(objID, hp)
        {
            this.mp = mp;
            this.xPos = xPos;
            this.zPos = zPos;
            this.yRot = yRot;
            this.wRot = wRot;
            this.anime = anime;
            this.level = level;
            this.gold = gold;
            this.items = items;
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
            message.Write(mp);
            message.Write(xPos);
            message.Write(zPos);
            message.Write(yRot);
            message.Write(wRot);
            message.Write(anime);
            message.Write(level);
            message.Write(gold);
            items.WriteData(message);
            buff.WriteData(message);
            vision.WriteData(message);
        }

        public static HeroSyncData ReadData(NetIncomingMessage message)
        {
            return new HeroSyncData(
                message.ReadUInt16(),
                message.ReadUInt16(),
                message.ReadUInt16(),
                message.ReadFloat(),
                message.ReadFloat(),
                message.ReadFloat(),
                message.ReadFloat(),
                message.ReadByte(),
                message.ReadByte(),
                message.ReadUInt16(),
                Items.ReadData(message),
                BuffDebuff.ReadData(message),
                EnemyVision.ReadData(message)
                );
        }

        public void DecreaseGold(int amount)
        {
            if (gold - amount < 0)
            {
                gold = 0;
            }
            else
            {
                gold -= (ushort)amount;
            }
        }

        public void IncreaseGold(int amount)
        {
            gold += (ushort)amount;
        }

        public void LevelUp()
        {
            level++;
        }

        public void Revive(ushort hp, ushort mp, float xPos, float zPos, float yRot, float wRot)
        {
            SetHP(hp);
            this.mp = mp;
            this.xPos = xPos;
            this.zPos = zPos;
            this.yRot = yRot;
            this.wRot = wRot;
        }

        public void Push(float xPos, float zPos, float yRot, float wRot, byte anime)
        {
            this.xPos = xPos;
            this.zPos = zPos;
            this.yRot = yRot;
            this.wRot = wRot;
            this.anime = anime;
        }
    }
}
